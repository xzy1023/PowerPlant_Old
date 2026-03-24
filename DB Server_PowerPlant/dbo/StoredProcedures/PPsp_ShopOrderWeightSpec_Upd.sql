
-- =============================================
-- Author:		Bong Lee
-- Create date: June 17, 2009
-- Description:	Update Shop Order Weight Specifications
-- WO#1297      Aug. 19, 2014   Bong Lee
-- Description: Enhance error handling.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ShopOrderWeightSpec_Upd]
	@chrFacility as char(3),
	@vchImportData as varchar(50),
	@intRetentionDays as Int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	DECLARE @vchSQLStmt varchar(1000);
	DECLARE @bitTableChanged bit;

	--WO#871	DECLARE @ErrorMessage NVARCHAR(4000);
	--WO#871	DECLARE @ErrorSeverity INT;
	--WO#871	DECLARE @ErrorState INT;
	SET NOCOUNT ON;

	BEGIN TRANSACTION [Tran1]		-- WO#1297
	BEGIN TRY

		Set	@bitTableChanged = 0

	-- Update Shop Order Weight Spec. record if value(s) are changed
		SET @vchSQLStmt = 	
		'Update dbo.tblShopOrderWeightSpec ' + 
		'SET ItemNumber = T2.ItemNumber,' +
		'Blend = T3.Blend, ' +
		'Grind = T3.Grind, ' +
		'LabelWeight = T3.LabelWeight, ' +
		'TargetWeight = T3.TargetWeight, ' +
		'MinWeight = T3.MinWeight, ' +
		'MaxWeight = T3.MaxWeight, ' +
		'LowerControlLimit = T3.LowerControlLimit ' +
 		'FROM tblShopOrderWeightSpec AS T1 ' +
		'INNER JOIN ' + @vchImportData + '.dbo.tblShopOrder AS T2 ' +
		'ON T1.ShopOrder = T2.ShopOrder ' +
		'Cross Apply dbo.fnShopOrderWeightSpec(T1.Facility,T1.ShopOrder) T3 ' +
		'WHERE T1.Facility = ''' + @chrFacility + ''' AND ' +
		  '(T1.ItemNumber <> T2.ItemNumber ' +
		'OR T1.Blend <> T3.Blend ' +
		'OR T1.Grind <> T3.Grind ' +
		'OR T1.LabelWeight <> T3.LabelWeight ' +
		'OR T1.TargetWeight <> T3.TargetWeight ' +
		'OR T1.MinWeight <> T3.MinWeight ' +
		'OR T1.MaxWeight <> T3.MaxWeight ' +
		'OR T1.LowerControlLimit <> T3.LowerControlLimit)'
	Print @vchSQLStmt
		execute (@vchSQLStmt)

		-- If table is updated, the table is required to download to IPCs
		IF @@ROWCOUNT > 0 
			Set	@bitTableChanged = 1;

		--Add New Shop Order Weight Spec. Records
		SET @vchSQLStmt = 
		'INSERT INTO tblShopOrderWeightSpec ' +
		'SELECT T1.Facility, T1.ShopOrder, T1.ItemNumber, T3.Blend, T3.grind, ISNULL(T3.LabelWeight,0),' +
		'ISNULL(T3.TargetWeight,0), ISNULL(T3.MinWeight,0), ISNULL(T3.MaxWeight,0), ' +
		'ISNULL(T3.LowerControlLimit,0) ' +
		'FROM ' + @vchImportData + '.dbo.tblShopOrder As T1  ' +
		'LEFT OUTER JOIN tblShopOrderWeightSpec AS T2 ' +
		'ON T1.ShopOrder = T2.ShopOrder ' +
		'Cross Apply dbo.fnShopOrderWeightSpec(T1.Facility,T1.ShopOrder) T3 ' +
		'WHERE T1.Facility = ''' + @chrFacility + ''' AND T2.ShopOrder IS NULL' 
Print @vchSQLStmt
		execute (@vchSQLStmt)

		-- If table has new records, the table is required to download to IPCs
		IF @@ROWCOUNT > 0 
			Set	@bitTableChanged = 1;

		--Delete Records if record is not found in ImportData
		SET @vchSQLStmt = 
		'DELETE tblShopOrderWeightSpec ' +
		'FROM tblShopOrderWeightSpec AS T1 ' +
		'LEFT OUTER JOIN ' + @vchImportData + '.dbo.tblShopOrder AS T2 ' +
		'ON T1.ShopOrder = T2.ShopOrder ' +
		'WHERE T1.Facility = ''' + @chrFacility + ''' AND T2.ShopOrder IS NULL' 
Print @vchSQLStmt
		execute (@vchSQLStmt)

		-- If records have been deleted from table, the table is required to download to IPCs
		IF @@ROWCOUNT > 0 
			Set	@bitTableChanged = 1;

Print @bitTableChanged
		-- If the table has been changed(i.e. Insert, Update or Delete), set on the Active Flag in tblDownLoadTableList
		-- to indicate the table will be downloaded.
		If @bitTableChanged = 1
			Update dbo.tblDownLoadTableList Set Active = 1 Where Facility = @chrFacility AND TableName = 'tblShopOrderWeightSpec';

		/* Update History File */
		
		-- Update Shop Order Weight Spec. History record if value(s) are changed
	
		Update dbo.tblShopOrderWeightSpecHst 
		SET ItemNumber = T2.ItemNumber,
			Blend = T2.Blend, 
			Grind = T2.Grind, 
			LabelWeight = T2.LabelWeight,
			TargetWeight = T2.TargetWeight,
			MinWeight = T2.MinWeight,
			MaxWeight = T2.MaxWeight, 
			LowerControlLimit = T2.LowerControlLimit,
			DateUpdated = getdate()
 		FROM tblShopOrderWeightSpecHst AS T1 
		INNER JOIN tblShopOrderWeightSpec AS T2 
		ON T1.Facility = T2.Facility AND T1.ShopOrder = T2.ShopOrder 
		WHERE T1.ItemNumber <> T2.ItemNumber
			OR T1.Blend <> T2.Blend 
			OR T1.Grind <> T2.Grind 
			OR T1.LabelWeight <> T2.LabelWeight 
			OR T1.TargetWeight <> T2.TargetWeight 
			OR T1.MinWeight <> T2.MinWeight
			OR T1.MaxWeight <> T2.MaxWeight 
			OR T1.LowerControlLimit <> T2.LowerControlLimit

		--Add New Shop Order Weight Spec. History Records

		INSERT INTO tblShopOrderWeightSpecHst 
		SELECT T1.*, getdate(), getdate()
		FROM tblShopOrderWeightSpec As T1 
		LEFT OUTER JOIN tblShopOrderWeightSpecHst AS T2 
		ON T1.Facility = T2.Facility AND T1.ShopOrder = T2.ShopOrder 
		WHERE T2.ShopOrder IS NULL 

		--Delete Shop Order Weight Spec. History Records if the DateUpdated of the record is greater than the retention days
		DELETE tblShopOrderWeightSpecHst FROM tblShopOrderWeightSpecHst
		WHERE Facility =  @chrFacility 
			AND DateDiff(d,DateUpdated,getdate()) > @intRetentionDays
		
		COMMIT TRANSACTION [Tran1]	-- WO#1297	

	END TRY
	BEGIN CATCH
	/* WO#1297 DEL Start
	SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();

			-- Use RAISERROR inside the CATCH block to return error
			-- information about the original error that caused
			-- execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, -- Message text.
					   @ErrorSeverity, -- Severity.
					   @ErrorState -- State.
					   );
	WO#1297 DEL Stop */
	-- WO#1297 ADD Start
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION [Tran1]

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
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ', Message: ' + CHAR(13) + ERROR_MESSAGE();

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)
	-- WO#1297 ADD Stop
	END CATCH
END

GO

