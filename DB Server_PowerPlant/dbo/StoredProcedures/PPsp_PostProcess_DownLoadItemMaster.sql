
-- =============================================
-- WO#1297      Jul. 4, 2014   Bong Lee
-- Description:	Post process for downloading MS Dynamics AX Item Master data to Power Plant
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PostProcess_DownLoadItemMaster]
	@chrFacility as char(3)
AS
BEGIN

	DECLARE @vchImportData as varchar(50);
	DECLARE @vchLogFileName nvarchar(200);
	DECLARE @vchCurrentDBName nvarchar(100);

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		
		SELECT @vchImportData = Value2 
			FROM tblControl
			WHERE [Key] = 'StagingDBName' and SubKey = 'General';

		Select @vchLogFileName = Value1  + Value2 + convert(varchar(8),getdate(),112) + '.txt' 
			FROM [tblControl] 
			WHERE [Key] = 'ERPDataXChgLogFolder' and [SubKey] = 'Interface';

		-- Update imported AX item master changes from staging DB to Local DB and join the Item Label Override table to 
		-- refresh the Item Master table in the Local DB
		EXECUTE PPsp_ImportItemMaster @chrFacility, @vchImportData

		-- Flag the Down Load Table List to idicate the table is ready to populate to IPCs
		UPDATE tblDownLoadTableList SET Active = 1 
			WHERE facility = @chrFacility AND TableName = 'tblItemMaster'
		
		-- Populate tables to IPCs based on the Down Load Table List
		--EXECUTE PPsp_ExportDataToLocalDB @chrFacility
		SET  @vchCurrentDBName = DB_NAME();
		EXECUTE PPsp_ExportDataToIPCDB @chrFacility, @vchLogFileName, @vchCurrentDBName ;

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

		-- Error: .NET Framework execution was aborted by escalation policy because of out of memory. 
		IF @ErrorNumber = 6535
			UPDATE tblDownLoadTableList SET Active = 0 
			WHERE facility = @chrFacility AND TableName = 'tblItemMaster'

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)
	END CATCH
END

GO

