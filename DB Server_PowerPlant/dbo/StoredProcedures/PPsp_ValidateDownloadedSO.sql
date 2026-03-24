-- =============================================
-- Author:		Bong Lee
-- Create date: Mar. 9, 2017	ITR-3982
-- Description:	Validate downloaded shop orders
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ValidateDownloadedSO]
	
AS
BEGIN
	
	DECLARE
		@vchMsgBody varchar (MAX) 
		,@vchProfile_name varchar(128)
		,@vchSubject varchar(512)
		,@vchRecipients varchar(128)
		,@vchQuery varchar(1024)

	DECLARE @vchFacility as varchar(3);	
	DECLARE @intRetentionDays as int;
	DECLARE @vchRetentionDays as varchar(3);
	DECLARE @vchProcEnv as varchar(50);
	
	SET NOCOUNT ON;

	BEGIN TRY

		SELECT @vchFacility = Value1 FROM tblControl WHERE [Key]='Facility'
		SELECT @vchProcEnv = Value2 FROM tblControl WHERE [Key]='CompanyName'
		SELECT @vchRecipients = Value1, @vchRetentionDays = value2, @intRetentionDays = CAST(Value2 as int)
			FROM tblControl WHERE [Key]='SOErrRecipients'

		SELECT
			@vchProfile_name  = 'PowerPlantSupport'	
			,@vchSubject  = 'Missing data found in Power Plant shop order download validation for Site ' + @vchFacility + ', Proc. Env. ' + @vchProcEnv	
			,@vchMsgBody = 'The interface validation found downloaded active shop orders having missing information in Power Plant. ' + 
							'Please open the attachment for the details.' + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) +
							'Note: In the attachment, the columns show the possible errors from the downloaded shop orders/items on the rows. ''1'' indicates error.' + CHAR(13) + CHAR(10) +
							'Same error will be sent once in the e-mail, if it is resolved within ' + @vchRetentionDays + ' days'

		IF EXISTS(SELECT * FROM [tempdb].[INFORMATION_SCHEMA].[TABLES]
		WHERE TABLE_NAME = '##tblShopOrderErrors')
		DROP TABLE ##tblShopOrderErrors

		IF EXISTS(SELECT * FROM [tempdb].[INFORMATION_SCHEMA].[TABLES]
		WHERE TABLE_NAME = '##tblItemErrors')
		DROP TABLE ##tblItemErrors

		CREATE TABLE [##tblShopOrderErrors](
			[Site] varchar(3)
			,[Shop Order] int
			,[Item] [varchar](35)
			,[Work Center] int
			,[Line] [varchar](10)
			,[Missing: Routing] bit
			,[Lot_ID] bit
			,[Ship_Shelf_Life_Days] bit
			,[Production_Shelf_Life_Days] bit
			,[Invalid_Operation] bit
		)
		ON [PRIMARY]

		-- Put all errors in global temporary tables
		/*----------------------*/
		/* Validate shop order  */
		/*----------------------*/

		INSERT INTO ##tblShopOrderErrors
		([Site]
		,[Shop Order]
		,Item
		,[Work Center]
		,Line
		,[Missing: Routing]
		,[Lot_ID]
		,[Ship_Shelf_Life_Days]
		,[Production_Shelf_Life_Days]
		,[Invalid_Operation]
		)
		SELECT  tSO.Facility, tSO.ShopOrder,  tSO.ItemNumber , tSO.WorkCenter , tSO.PackagingLine 
		  , CASE WHEN tMER_PL.ItemNumber is null and tMER_WC.ItemNumber is null THEN 1 ELSE 0 END as [Missing: Routing]
		  , CASE WHEN DATEDIFF(hour, GETDATE(),tSO.StartTime) < 72 AND tIM.PrintSOLot = 'Y' And RTrim(tSO.LotID)= '' THEN 1 ELSE 0 END as [Missing LotID]
		  , CASE WHEN tCC.EnableOvrExpDate = 1 AND ISNULL(tIM.ShipShelfLifeDays,0) = 0 THEN 1 ELSE 0 END as [Missing ShipShelfLifeDays]
		  , CASE WHEN tCC.EnableOvrExpDate = 1 AND ISNULL(tIM.ProductionShelfLifeDays,0) = 0 THEN 1 ELSE 0 END as [Missing ProductionShelfLifeDays]
		  , CASE WHEN tCC.ProbatEnabled = 1 AND ((tSO.ComponentItem <> '' AND (tSO.OrderType <> 3 and tSO.OrderType <> 5 ))
					OR (tSO.ComponentItem = '' AND (tSO.OrderType = 3 OR tSO.OrderType = 5))) THEN 1 ELSE 0 END as [Invalid Operation]
			-- Order Type 3 - pack WHole bean; 5 - pack ground coffee
		FROM 
		  (SELECT tS.Facility, tS.ShopOrder, tS.LotID, tS.ItemNumber, CAST(tEQ.WorkCenter as int) as WorkCenter
		  ,tS.PackagingLine, tS.OrderType, tS.ComponentItem
		  ,CONVERT(datetime ,STUFF(STUFF(tS.StartDate,7,0,'-'),5,0,'-') + ' ' + STUFF(STUFF([dbo].[fnFillLeadingZeros](6,tS.StartTime),5,0,':'),3,0,':'), 120) as StartTime
			FROM tblShopOrder tS
			LEFT OUTER JOIN tblEquipment tEQ
			ON	tS.Facility = tEQ.Facility 
				AND tS.PackagingLine = tEQ.EquipmentID
				AND tEQ.[Type] = 'P'
			WHERE tS.Closed is NULL 
				AND tEQ.WorkCenter is NOT NULL
				AND DATEDIFF(hour,CONVERT(datetime ,STUFF(STUFF(tS.StartDate,7,0,'-'),5,0,'-') + ' ' + STUFF(STUFF([dbo].[fnFillLeadingZeros](6,tS.StartTime),5,0,':'),3,0,':'), 120) , getdate()) < 48
			) tSO
		LEFT OUTER JOIN tblStdMachineEfficiencyRate as tMER_PL
		ON  tSO.Facility = tMER_PL.Facility 
			AND tSO.ItemNumber = tMER_PL.ItemNumber 
			AND tSO.PackagingLine = tMER_PL.MachineID

		LEFT OUTER JOIN tblStdMachineEfficiencyRate as tMER_WC
		ON  tSO.Facility = tMER_WC.Facility
			AND tSO.ItemNumber = tMER_WC.ItemNumber 
			AND tSO.[WorkCenter] = tMER_WC.WorkCenter
			AND tMER_PL.MachineID = ''

		LEFT OUTER JOIN tblItemMaster as tIM
		ON  tSO.Facility = tIM.Facility 
			AND tSO.ItemNumber = tIM.ItemNumber

		LEFT OUTER JOIN tblComputerConfig as tCC
		ON	tSO.Facility = tCC.Facility
			AND	tSO.PackagingLine = tCC.PackagingLine 
			AND	tCC.[RecordStatus] = 1 

		WHERE (tMER_PL.ItemNumber is null and tMER_WC.ItemNumber is null)
			OR (DATEDIFF(hour, GETDATE(),tSO.StartTime) < 72 AND tIM.PrintSOLot = 'Y' And RTrim(tSO.LotID) = '')
			OR (tCC.EnableOvrExpDate = 1 AND ISNULL(tIM.ShipShelfLifeDays,0) = 0)
			OR (tCC.EnableOvrExpDate = 1 AND ISNULL(tIM.ProductionShelfLifeDays,0) = 0)
			OR (tCC.ProbatEnabled = 1 AND ((tSO.ComponentItem <> '' AND (tSO.OrderType <> 3 and tSO.OrderType <> 5 ))
					OR (tSO.ComponentItem = '' AND (tSO.OrderType = 3 OR tSO.OrderType = 5))) )

		ORDER BY tSO.Facility, tSO.ShopOrder

		-- Delete error records from temporary table if the same errors appeared less than x days, so they will not be included in email.
		DELETE FROM ##tblShopOrderErrors 
		--select *
			FROM ##tblShopOrderErrors t##SOE
			INNER JOIN tblShopOrderErrors tSOE
			ON  t##SOE.[Site]= tSOE.Facility
				AND t##SOE.[Shop Order] = tSOE.ShopOrder
				AND t##SOE.Item = tSOE.ItemNumber
				AND t##SOE.[Work Center] = tSOE.WorkCenter
				AND t##SOE.[Line] = tSOE.PackagingLine
			WHERE [Missing: Routing] =[Routing]
				AND t##SOE.[Lot_ID] = [LotID]
				AND t##SOE.[Ship_Shelf_Life_Days] = [ShipShelfLifeDays]
				AND t##SOE.[Production_Shelf_Life_Days] = [ProductionShelfLifeDays]
				AND t##SOE.[Invalid_Operation] = [InvalidOperation]
				AND DATEDIFF(minute,tSOE.[DateUpdated],getdate()) <= @intRetentionDays * 24 * 60


		CREATE TABLE [##tblItemErrors](
			[Site] varchar(3)
			,[Item] [varchar](35)
			,[Item_Not_Found] bit
			,[Missing: Qty_Per_Pallet] bit
			,[Label_Date_Fmt_Code] bit
			,[Label_Image_Name] bit
			,Bag_Length bit
			,Production_Shelf_Life_Days bit
		)
		 ON [PRIMARY]

		/*----------------------*/
		/* Validate item		*/
		/*----------------------*/

		INSERT INTO ##tblItemErrors
		SELECT tSO.Facility, tSO.ItemNumber
			,CASE WHEN tIM.ItemNumber is NULL THEN 1 ELSE 0 END as [ItemNotFound]
			,CASE WHEN QtyPerPallet = 0 THEN 1 ELSE 0 END as [MissingQtyPerPallet]
			,CASE WHEN DateToPrintFlag='1' AND LabelDateFmtCode ='' THEN 1 ELSE 0 END as [MissingLabelDateFmtCode]
			,CASE WHEN PrintCaseLabel = 'Y' And RTrim(CaseLabelFmt1) = '' And RTrim(CaseLabelFmt2) = '' And RTrim(CaseLabelFmt3) = '' Then 1 ELSE 0 END as [MissingLabelFmt]
			,CASE WHEN BagLengthRequired = 'Y' AND BagLength = 0 THEN 1 ELSE 0 END as [MissingBagLength]
			,CASE WHEN PrintCaseLabel = 'Y' And DateToPrintFlag = '1' AND ISNULL(tIM.ProductionShelfLifeDays,0) = 0 THEN 1 ELSE 0 END as [MissingProductionShelfLifeDays]
		FROM
			(SELECT DISTINCT Facility, ItemNumber FROM tblShopOrder) tSO
			LEFT OUTER JOIN tblItemMaster tIM
			ON tSO.Facility = tIM.Facility
			AND tSO.ItemNumber = tIM.ItemNumber
			WHERE	CASE WHEN tIM.ItemNumber is NULL THEN 1 ELSE 0 END = 1  
			OR		CASE WHEN QtyPerPallet = 0 THEN 1 ELSE 0 END = 1  
			OR		CASE WHEN DateToPrintFlag='1' AND LabelDateFmtCode ='' THEN 1 ELSE 0 END = 1
			OR		CASE WHEN PrintCaseLabel = 'Y' And RTrim(CaseLabelFmt1) = '' And RTrim(CaseLabelFmt2) = '' And RTrim(CaseLabelFmt3) = '' Then 1 ELSE 0 END = 1
			OR		CASE WHEN BagLengthRequired = 'Y' AND BagLength = 0 THEN 1 ELSE 0 END = 1
			OR		CASE WHEN PrintCaseLabel = 'Y' And DateToPrintFlag = '1' AND ISNULL(tIM.ProductionShelfLifeDays,0) = 0 THEN 1 ELSE 0 END = 1
		ORDER BY  tSO.Facility, tSO.ItemNumber
		-- DateToPrintFlag: 0 - none;	1 - Expiry;	2 - Production;	3 - Both 1 & 2

		DELETE FROM ##tblItemErrors 
			FROM ##tblItemErrors t##IE
			INNER JOIN [tblItemErrors] tIE
			ON  t##IE.Site= tIE.Facility
				AND t##IE.Item = tIE.ItemNumber
			WHERE [Item_Not_Found] = [ItemNotFound]
				AND t##IE.[Missing: Qty_Per_Pallet] = [QtyPerPallet]
				AND t##IE.[Label_Date_Fmt_Code] = [LabelDateFmtCode]
				AND t##IE.[Label_Image_Name] = [LabelImageName]
				AND t##IE.[Bag_Length] = [BagLength]
				AND t##IE.[Production_Shelf_Life_Days] = [ProductionShelfLifeDays]
				AND DATEDIFF(minute,tIE.[DateUpdated],getdate()) <= @intRetentionDays * 24 * 60

		-- Initialize query statement variable
		SET @vchQuery = ''

		-- If shop order errors exist, save the errors to table and construct SQL statement to send email
		IF EXISTS (select * from ##tblShopOrderErrors)
		BEGIN

		-- Maintenance Shop Order Errors
			INSERT INTO tblShopOrderErrors
				(Facility
				, ShopOrder
				, ItemNumber
				, WorkCenter
				, PackagingLine
				, Routing, LotID
				, ShipShelfLifeDays
				, ProductionShelfLifeDays
				, InvalidOperation
				)
			SELECT t#SOE.* FROM ##tblShopOrderErrors t#SOE
			LEFT OUTER JOIN tblShopOrderErrors tSOE
			ON t#SOE.[Site] = tSOE.[Facility]
				AND t#SOE.[Shop Order] = tSOE.[ShopOrder]
				AND t#SOE.[Item] =  tSOE.[ItemNumber]
				AND t#SOE.[Work Center] = tSOE.[WorkCenter]
				AND t#SOE.[Line] = tSOE.[PackagingLine]
			WHERE tSOE.Facility is NULL

			UPDATE tSOE
			SET Routing = t#SOE.[Missing: Routing]
				,LotID = t#SOE.[Lot_ID]
				,[ShipShelfLifeDays] = t#SOE.[Ship_Shelf_Life_Days]
				,ProductionShelfLifeDays = t#SOE.[Production_Shelf_Life_Days]
				,InvalidOperation = t#SOE.[Invalid_Operation] 
				,[DateUpdated] = getdate()
			FROM tblShopOrderErrors tSOE
			INNER JOIN ##tblShopOrderErrors t#SOE
			ON tSOE.[Facility] = t#SOE.[Site]
				AND tSOE.[ShopOrder] = t#SOE.[Shop Order]
				AND tSOE.[ItemNumber] =  t#SOE.[Item]
				AND tSOE.[WorkCenter] = t#SOE.[Work Center]
				AND tSOE.[PackagingLine] = t#SOE.[Line]

			SET @vchQuery = 'SELECT REPLICATE(''*'', 60) + ''     Shop Order Validation Error(s)     '' + REPLICATE(''*'', 60) ;' + 'SELECT t2.StartDate,T1.* from ##tblShopOrderErrors T1 ' + 
					'LEFT OUTER JOIN ' + DB_NAME() + '.dbo.tblShopOrder T2 on T1.[Shop Order] = t2.ShopOrder;'
		END

		-- If item in the active shop order has errors, save the errors to table and construct SQL statement to send email
		IF EXISTs (SELECT * From ##tblItemErrors)
		BEGIN
		-- Maintenance Item Errors
			INSERT INTO tblItemErrors
				(Facility
				, ItemNumber
				, [ItemNotFound]
				, [QtyPerPallet]
				, [LabelDateFmtCode]
				, [LabelImageName]
				, [BagLength]
				, [ProductionShelfLifeDays]
				)
			SELECT t#IE.* FROM ##tblItemErrors t#IE
			LEFT OUTER JOIN tblItemErrors tIE
			ON t#IE.[Site] = tIE.[Facility]
				AND t#IE.[Item] =  tIE.[ItemNumber]
			WHERE tIE.Facility is NULL

			UPDATE tIE
			SET [ItemNotFound] = t#IE.[Item_Not_Found] 
				, [QtyPerPallet] = t#IE.[Missing: Qty_Per_Pallet]
				, [LabelDateFmtCode] = t#IE.[Label_Date_Fmt_Code]
				, [LabelImageName] = t#IE.Label_Image_Name
				, [BagLength] =t#IE.Bag_Length
				, [ProductionShelfLifeDays] = t#IE.Production_Shelf_Life_Days
				, [DateUpdated] = getdate()
			FROM tblItemErrors tIE
			INNER JOIN  ##tblItemErrors t#IE
			ON tIE.[Facility] = t#IE.[Site]
				AND  tIE.[ItemNumber] = t#IE.[Item]


		SET @vchQuery = @vchQuery + 'SELECT REPLICATE(''*'', 60) + ''     Item Validation Error(s)    '' + REPLICATE(''*'', 60) ;' + 'SELECT * from ##tblItemErrors;'
		END

		IF @vchQuery <> ''
		BEGIN
			SET @vchQuery = 'SET NOCOUNT ON;' + @vchQuery 
		 --PRINT  @vchQuery

		-- Send email
			EXEC msdb.dbo.sp_send_dbmail
			@profile_name = @vchProfile_name		 
			,@recipients = @vchRecipients			 
			,@body = @vchMsgBody					
			,@subject = @vchSubject					
			,@query =  @vchQuery
			,@attach_query_result_as_file = 1
			,@query_attachment_filename= 'Missing Information in Powerplant.txt'
		END

		-- Clear records from error history table
		DELETE FROM tblShopOrderErrors 
			FROM tblShopOrderErrors tSOE
		LEFT OUTER JOIN tblShopOrder tSO
		ON	tSOE.[Facility] = tSO.[Facility]
			AND tSOE.[ShopOrder] = tSO.[ShopOrder]
		WHERE tSO.ShopOrder IS NULL

		DELETE FROM tblItemErrors 
		WHERE DATEDIFF(day,DateUpdated,getdate()) > 30

		DROP TABLE ##tblShopOrderErrors

		DROP TABLE ##tblItemErrors

	END TRY
	BEGIN CATCH
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure nvarchar(200);
		DECLARE @ErrorLine int;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorMessage NVARCHAR(4000);

		IF EXISTS(SELECT * FROM [tempdb].[INFORMATION_SCHEMA].[TABLES]
		WHERE TABLE_NAME = '##tblShopOrderErrors')
		DROP TABLE ##tblShopOrderErrors

		IF EXISTS(SELECT * FROM [tempdb].[INFORMATION_SCHEMA].[TABLES]
		WHERE TABLE_NAME = '##tblItemErrors')
		DROP TABLE ##tblItemErrors

		SELECT 
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ', Message: ' + CHAR(13) + ERROR_MESSAGE();

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)

	END CATCH
END

GO

