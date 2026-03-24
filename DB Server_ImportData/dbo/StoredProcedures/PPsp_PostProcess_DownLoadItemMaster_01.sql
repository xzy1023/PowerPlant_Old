
-- =============================================
-- WO#1297      Jul. 4, 2014   Bong Lee
-- Description:	Post process for downloading MS Dynamics AX Item Master data to Power Plant for facility 01
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PostProcess_DownLoadItemMaster_01]

AS
BEGIN

	DECLARE	@vchFacility as varchar(3);
	DECLARE	@vchDownLoadLogFileName varchar(255);
	DECLARE	@vchText varchar(255);
	DECLARE	@vchPPDBName nvarchar(100); 
	DECLARE	@vchSQLStmt nvarchar(500);
	DECLARE	@vchParmDefinition nvarchar(500);

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRY
		SELECT @vchFacility = Right(OBJECT_NAME(@@PROCID), Charindex('_', Reverse(OBJECT_NAME(@@PROCID)))-1);

		SELECT @vchDownLoadLogFileName = Value1  + Value2 + convert(varchar(8),getdate(),112) + '.txt' 
			FROM [tblControl] 
			WHERE [Key] = 'ERPDataXChgLogFolder' and [SubKey] = 'Interface';

		SELECT @vchPPDBName = Value1
			FROM [tblControl] 
			WHERE [Key] = 'PowerPlantDBName' and [SubKey] = 'General';

		--SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + ISNULL(OBJECT_NAME(@@PROCID),'') + ' - Started.';
		--EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;

		-- Call generic stored procedure to run post process of the Item Master Down Load for specific facility
		SET @vchSQLStmt =  N'EXEC ' + @vchPPDBName + N'.dbo.PPsp_PostProcess_DownLoadItemMaster @vchFacility';
		SET @vchParmDefinition = N'@vchFacility varchar(3)';

		EXECUTE sp_executesql @vchSQLStmt, @vchParmDefinition, @vchFacility;

		-- Copy the processed records to history table
		SET @vchText = 'Move the processed records to transaction history table';
		EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;

		BEGIN TRANSACTION;
			INSERT INTO tblItemMasterTxFromERPHst
			SELECT	getdate(), *
			FROM    tblItemMasterTxFromERP
			WHERE	Facility = @vchFacility and Processed = 1

		-- Delete the processed records from the original table
			DELETE FROM    tblItemMasterTxFromERP
			WHERE	Facility = @vchFacility and Processed = 1
		COMMIT TRANSACTION;

		SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + ISNULL(OBJECT_NAME(@@PROCID),'') + ' - Completed.';
		EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;

	END TRY
	BEGIN CATCH
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure nvarchar(200);
		DECLARE @ErrorLine int;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorMessage NVARCHAR(4000);

		DECLARE @nvchSubject nvarchar(256);
		DECLARE @nvchBody nvarchar(MAX);

		IF (XACT_STATE()) = -1
			ROLLBACK TRANSACTION; 

		SELECT 
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d, Message:' + CHAR(13) + ERROR_MESSAGE();

		BEGIN TRY
			SELECT 
				@nvchSubject = @@ServerName + '.' + DB_NAME() + ' - ' + ISNULL(ERROR_PROCEDURE(),'') + N' fails.',
				@nvchBody = N'Error ' + CAST(@ErrorNumber as varchar(10)) +
							N', Level ' + CAST(@ErrorSeverity as varchar(10)) +
							N', State ' + CAST(@ErrorState as varchar(10)) + 
							N', Procedure ' + @ErrorProcedure +
							N', Line ' + CAST(@ErrorLine as varchar(10)) +
							N', Message:' + CHAR(13) + ERROR_MESSAGE();

			EXEC PPsp_SndMsgToOperator @nvchBody, @nvchSubject;

			SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + ISNULL(OBJECT_NAME(@@PROCID),'') + ' - Failed.';
			EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;
		END TRY
		BEGIN CATCH
		END CATCH
		-- Use RAISERROR inside the CATCH block to return error
		-- information about the original error that caused
		-- execution to jump to the CATCH block.
		RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)
	END CATCH
END

GO

