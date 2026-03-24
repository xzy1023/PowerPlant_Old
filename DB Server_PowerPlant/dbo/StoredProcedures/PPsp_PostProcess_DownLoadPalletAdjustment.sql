
-- =============================================
-- WO#1297      Jul. 9, 2014   Bong Lee
-- Description:	Post process for downloading MS Dynamics AX Pallet Adjustment
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PostProcess_DownLoadPalletAdjustment]
	@chrFacility as char(3)
AS
BEGIN

	DECLARE @vchImportData as varchar(50);
	DECLARE @vchTableName as nvarchar(100);
	DECLARE @vchSQLStmt as nvarchar(2000);
	DECLARE	@vchParmDefinition nvarchar(500);

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRANSACTION
			SELECT @vchImportData = Value2 
				FROM tblControl
				WHERE [Key] = 'StagingDBName' and SubKey = 'General';

			SET @vchTableName = 'tblPalletAdjustment';

			-- Update imported AX pallet adjustment from staging DB to Local DB in Power plant server.
			SET @vchSQLStmt = N'Update ' + @vchImportData + N'.[dbo].' + @vchTableName + N' SET Processed = 1 WHERE Facility = @chrFacility and Processed = 0; ' +
				N'Insert into tblPalletAdjustment ' +
				N'(RRN,Facility,ShopOrder,MachineID,Operator,PalletID,LotNumber,AdjustedQty,TransactionReasonCode,TransactionDate) ' +
				N'Select RRN,Facility,ShopOrder,MachineID,Operator,PalletID,LotNumber,AdjustedQty,TransactionReasonCode,TransactionDate from ' + 
				@vchImportData + N'.[dbo].' + @vchTableName + N' WHERE Facility = @chrFacility and Processed = 1; ' +
				N'Insert into tblPalletAdjustmentHst Select * From ' + 
				@vchImportData + N'.[dbo].' + @vchTableName + N' WHERE Facility = @chrFacility and Processed = 1; ' +
				N'Delete From ' + 
				@vchImportData + N'.[dbo].' + @vchTableName + N' WHERE Facility = @chrFacility and Processed = 1; ';

			SET @vchParmDefinition = N'@chrFacility char(3)';
			Print @vchSQLStmt
			EXECUTE sp_executesql @vchSQLStmt, @vchParmDefinition, @chrFacility;
		COMMIT

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

