
-- =============================================
-- Author:		Bong Lee
-- Create date: Jul. 14, 2011
-- Description:	Scrapped Component by Shift Production Date & Shift
-- Task#6631	Aug. 31, 2015	Bong Lee
-- Description:	Use information from Dynamics AX (rewrite)
/* -- To Test --
EXEC	[PPsp_ScrappedComponent]
		@vchAction = N'ByOperator',
		@vchComponentType = N'RawMaterial',
		@vchFacility = N'01',
		@dteFromDate = N'8/1/2015',
		@intFromShift = 1,
		@dteToDate = N'8/25/2015',
		@intToShift = 3,
		@intShift = NULL,
		@vchWorker = NULL
*/
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ScrappedComponent]
	@vchAction varchar(50) = NULL,
	@vchComponentType varchar(15),
	@vchFacility varchar(3),
	@dteFromDate datetime , 
	@intFromShift int, 
	@dteToDate datetime,
	@intToShift int, 
	@intShift int = NULL,
	@vchWorker varchar(10) = NULL
AS
BEGIN
	DECLARE @vchSQLStmt as nvarchar(3000)
	DECLARE @vchFinalSQLStmt as nvarchar(3000)
	DECLARE @vchOrderBy as nvarchar(200)
	DECLARE @vchServerName as nvarchar(50)
	DECLARE @vchDBName nvarchar(50)
	DECLARE @vchFromShopOrder nvarchar(20)
	DECLARE @vchToShopOrder nvarchar(20)
	DECLARE @vchCompanyNo nvarchar(10)
	DECLARE @decLBtoGM as decimal(10,7)
	DECLARE @cstComponentType varchar(15)	
	DECLARE @cstItemType varchar(30)

	DECLARE @tIQty Table (
		ShopOrder varchar(20),
		Item varchar(35),
		QtyIssued decimal(13,5)
	)

	SET NOCOUNT ON;
	BEGIN TRY
		SELECT @cstComponentType = 'RawMaterial'
			   ,@cstItemType = '''''SubWIP'''' , ''''RMOth'''''  

		SELECT @vchCompanyNo = Value2 FROM tblControl WHERE [Key] = 'Facility' AND SubKey = 'General'
		SELECT @vchServerName = Value1, @vchDBName = Value2 FROM tblControl WHERE [Key] = 'ERPEnv' AND SubKey = 'General'
	
		-- Get the Pound to Gram conversion rate
		SELECT @decLBtoGM = Value1 FROM tblControl 
			WHERE [KEY] = 'WeightConversion' AND SubKey = 'General'

		-- obtain the shop order range on the requested time range.
		SELECT  @vchToShopOrder = CAST(MAX(ShopOrder) as nvarchar(20)), @vchFromShopOrder = CAST(MIN(ShopOrder) as nvarchar(20))
		FROM dbo.tblComponentScrap
		WHERE Facility = @vchFacility AND (StartTime between dateadd(d,-1,@dteFromDate) AND dateadd(d,2,@dteToDate))

		IF @vchComponentType = 'NonRawMaterial' OR @vchComponentType = 'RawMaterial'
		BEGIN
			SET @vchSQLStmt = 
				N'SELECT tPJ.prodid, tPJB.itemid, SUM(tPJB.BOMCONSUMP) as QtyIssue 
				FROM ' + @vchDBName + N'.dbo.ProdJournalTable tPJ
				LEFT OUTER JOIN ' + @vchDBName + N'.dbo.ProdJournalBOM tPJB
				ON tPJ.Journalid = tPJB.Journalid 
				AND tPJ.Dataareaid = tPJB.Dataareaid
				LEFT OUTER JOIN ' + @vchDBName + N'.dbo.InventItemGroupItem tIIGI
				ON tPJB.itemid = tIIGI.itemid
				AND tPJ.dataareaid = tIIGI.ItemDataareaId
				WHERE tPJ.prodid BETWEEN ''''' + @vchFromShopOrder + ''''' AND ''''' + @vchToShopOrder +
					 ''''' AND tPJ.dataareaid = ''''' + @vchCompanyNo +
					 ''''' AND journaltype = 0 AND JOURNALNAMEID <> ''''PICK''''
					 AND ((''''' + @vchComponentType + ''''' = ''''' + @cstComponentType + ''''' AND tIIGI.ItemGroupId in (' + @cstItemType + '))
					 OR (''''' + @vchComponentType + ''''' <> ''''' + @cstComponentType + ''''' AND tIIGI.ItemGroupId not in (' + @cstItemType + ')))
				group by tPJ.prodid, tPJB.itemid'

			SELECT @vchFinalSQLStmt = 'SELECT * FROM OPENQUERY([' + @vchServerName + '],''' + @vchSQLStmt + ''')'; 

print @vchFinalSQLStmt

			INSERT INTO @tIQty
			EXEC sp_ExecuteSQL @vchFinalSQLStmt
		END
		ELSE
		BEGIN
			;With cteIssue As
			(
				SELECT tSCH.ShopOrder
					,CASE WHEN @vchComponentType = 'lbs'
						THEN SUM(Round((tSCH.CasesProduced + ISNULL(tSCH.AdjustedQty,0)) * Round(tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit/ (Case When tIM.labelweightUOM = 'GM' Then @decLBtoGM Else 1 End),3),0))
					 ELSE
						SUM((tSCH.CasesProduced + ISNULL(tSCH.AdjustedQty,0)) * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit)
					 END as QtyIssued
				FROM tfnSessionControlHstSummary ('WithAdjByLineSO',@vchFacility,NULL,NULL,NULL,NULL,@dteFromDate,@intFromShift,@dteToDate,@intToShift) as tSCH
				LEFT OUTER JOIN tblItemMaster tIM
				ON tSCH.Facility = tIM.Facility AND tSCH.ItemNumber = tIM.ItemNumber
				GROUP BY tSCH.ShopOrder
			)
			INSERT INTO @tIQty
			SELECT cteIssue.ShopOrder, tSS.Component, cteIssue.QtyIssued from cteIssue
			CROSS JOIN tblSpecialScrap tSS
			WHERE tSS.[Type] = @vchComponentType AND tSS.Active = 1
		END

		IF @vchAction = 'ByOperator'
		BEGIN
			SELECT tCS.Component																	
				,ISNULL(tIM.ItemDesc1,tSS.Description) as ItemDesc									
				,tSCH.ShopOrder, tSCH.DefaultPkgLine, tEqt.Description, tSCH.ShiftProductionDate	
				,tSCH.ShiftSequence, tSCH.ShiftDesc																						
				,tCS.Quantity, tSCH.Operator as Worker, tPS.FirstName + ' ' + tPS.LastName as WorkerName	
				,tQI.QtyIssued
				,tEqt.WorkCenter
			FROM tblComponentScrap tCS
			INNER JOIN tfnSessionControlHstDetail(NULL, @vchFacility, NULL, @vchWorker, NULL, NULL, @dteFromDate, @intFromShift,@dteToDate, @intToShift) tSCH
			ON tCS.ShopOrder = tSCH.ShopOrder AND CONVERT(varchar(19),tCS.StartTime,120) = CONVERT(varchar(19),tSCH.StartTime,120)
			LEFT OUTER JOIN tblItemMaster tIIM
			ON tCS.Component = tIIM.ItemNumber
			LEFT OUTER JOIN tblSpecialScrap tSS								
			ON tCS.Component = tSS.Component								
			LEFT OUTER JOIN @tIQty tQI
			On tCS.ShopOrder = tQI.ShopOrder AND tCS.Component = tQI.item
			LEFT OUTER JOIN tblItemMaster tIM
			ON tCS.Facility = tIM.Facility AND tCS.Component = tIM.ItemNumber
			LEFT OUTER JOIN dbo.tblPlantStaff tPS 
			On tCS.Facility = tPS.Facility AND tSCH.Operator = tPS.StaffID 
			LEFT OUTER JOIN dbo.tblEquipment tEqt
			ON tCS.Facility = tEqt.Facility AND tSCH.DefaultPkgLine = tEqt.EquipmentID 
			WHERE tCS.Facility = @vchFacility AND tSCH.ShiftProductionDate BETWEEN @dteFromDate AND @dteToDate
				AND (@vchWorker IS NULL OR tSCH.Operator = @vchWorker)
				AND (@intShift IS NULL OR tSCH.OverrideShiftNo = @intShift)
				AND ((@vchComponentType = @cstComponentType AND tIIM.ItemType in ('SubWIP' , 'RMOth'))
				OR (@vchComponentType <> @cstComponentType AND tIIM.ItemType not in ('SubWIP' , 'RMOth')))
		END
		ELSE
		IF @vchAction = 'ByUtilityTech'
		BEGIN
			SELECT tCS.Component																	
				,ISNULL(tIM.ItemDesc1,tSS.Description) as ItemDesc									
				,tSCH.ShopOrder, tSCH.DefaultPkgLine, tEqt.Description, tSCH.ShiftProductionDate	
				,tSCH.ShiftSequence, tSCH.ShiftDesc
				,tSCH.OverrideShiftNo as Shift,	tCS.Quantity, tOS.StaffID as Worker, tPS.FirstName + ' ' + tPS.LastName as WorkerName
				,tQI.QtyIssued
				,tEqt.WorkCenter
			FROM tblComponentScrap tCS
			INNER JOIN tfnSessionControlHstDetail(NULL, @vchFacility, NULL, NULL, NULL, NULL, @dteFromDate, @intFromShift,@dteToDate, @intToShift) tSCH
			ON tCS.ShopOrder = tSCH.ShopOrder AND CONVERT(varchar(19),tCS.StartTime,120) = CONVERT(varchar(19),tSCH.StartTime,120)
			LEFT OUTER JOIN tblItemMaster tIIM
			ON tCS.Component = tIIM.ItemNumber
			LEFT OUTER JOIN tblSpecialScrap tSS								
			ON tCS.Component = tSS.Component								
			LEFT OUTER JOIN @tIQty tQI
			On tCS.ShopOrder = tQI.ShopOrder AND tCS.Component = tQI.item
			INNER JOIN [tblOperationStaffing] tOS
			On tSCH.Facility = tOS.Facility and tSCH.DefaultPkgLine = tOS.PackagingLine And tSCH.[StartTime] = tOS.[StartTime]
			LEFT OUTER JOIN tblItemMaster tIM
			ON tCS.Facility = tIM.Facility AND tCS.Component = tIM.ItemNumber
			LEFT OUTER JOIN dbo.tblPlantStaff tPS 
			On tCS.facility = tPS.facility AND tOS.StaffID = tPS.StaffID 
			LEFT OUTER JOIN dbo.tblEquipment tEqt
			ON tCS.facility = tEqt.facility AND tSCH.DefaultPkgLine = tEqt.EquipmentID 
			WHERE tCS.Facility = @vchFacility AND tSCH.ShiftProductionDate BETWEEN @dteFromDate AND @dteToDate
				AND (@vchWorker IS NULL OR tOS.StaffID = @vchWorker)
				AND (@intShift IS NULL OR tSCH.OverrideShiftNo = @intShift)
		END	

	END TRY
	BEGIN CATCH
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure nvarchar(200);
		DECLARE @ErrorLine int;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorMessage NVARCHAR(4000);

		SELECT 
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ' Message: ' + CHAR(13) + ERROR_MESSAGE();

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)
	END CATCH
END

GO

