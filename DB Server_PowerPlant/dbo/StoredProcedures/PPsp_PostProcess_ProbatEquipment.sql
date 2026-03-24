
-- =============================================
-- WO#1297      Jul. 9, 2014   Bong Lee
-- Description:	Post process for downloading MS Dynamics AX Probat Equipment related data to Power Plant
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PostProcess_ProbatEquipment]
	@vchFacility as varchar(3)
AS
BEGIN

	DECLARE @vchImportData as varchar(50);
	DECLARE @vchSQLStmt as varchar(1000);
	DECLARE @vchLogFileName nvarchar(200);
	DECLARE @vchDownLoadLogFileName nvarchar(200);
	DECLARE	@vchText varchar(255);
	DECLARE	@vchTableName varchar(100);
	DECLARE @vchCurrentDBName nvarchar(100);
	DECLARE @tblTemp as Table
	(
	 intRow int
	)

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		
		SELECT @vchImportData = Value2 
			FROM tblControl
			WHERE [Key] = 'StagingDBName' and SubKey = 'General';

		Select @vchLogFileName = Value1  + 'DownLoadLog_' + convert(varchar(8),getdate(),112) + '.txt', 
				@vchDownLoadLogFileName =  Value1  + Value2 + convert(varchar(8),getdate(),112) + '.txt' 
			FROM [tblControl] 
			WHERE [Key] = 'ERPDataXChgLogFolder' and [SubKey] = 'Interface';

		SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + OBJECT_NAME(@@PROCID) + ' - Started.'
		EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;
		-- Update imported AX Proabt equipment related data from staging DB to Local DB in Power plant server.

		-- refresh the Standard Machine Efficiency Rate table. This table will not be pushed to IPC.
		SET @vchTableName = 'tblProbatEquipment';
		DELETE From @tblTemp
		SET @vchSQLStmt = 'SELECT top 1 1 FROM ' + @vchImportData + '.dbo.'+ @vchTableName + ';';
		Insert into @tblTemp EXECUTE (@vchSQLStmt);

		IF EXISTS (SELECT * FROM @tblTemp)
		BEGIN
			SET @vchText = 'Import ' + @vchTableName;
			EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;
			EXECUTE PPsp_ImportProbatEquipment @vchFacility, @vchImportData;
		END
		-- Flag the Down Load Table List to idicate the table is ready to populate to IPCs
		UPDATE tblDownLoadTableList SET Active = 1 
			WHERE facility = @vchFacility AND TableName = 'tblProbatEquipment';
		
		-- Populate tables to IPCs based on the Down Load Table List
		SET  @vchCurrentDBName = DB_NAME();
		EXECUTE PPsp_ExportDataToIPCDB @vchFacility, @vchLogFileName, @vchCurrentDBName ;

		SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + OBJECT_NAME(@@PROCID) + ' - Completed.';
		EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;

	END TRY
	BEGIN CATCH
		
		BEGIN TRY
			SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + OBJECT_NAME(@@PROCID) + ' - Failed.';
			EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;
		END TRY
		BEGIN CATCH
		END CATCH

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

