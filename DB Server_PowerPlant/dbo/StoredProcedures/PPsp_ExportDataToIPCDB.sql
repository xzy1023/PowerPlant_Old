
-- =============================================
-- Author:		Bong Lee
-- Create date: May 23, 2016
-- Description:	Export data from PP server to IPCs
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ExportDataToIPCDB] 
	@chrFacility as varchar(3),
	@vchDownLoadLogFileName as varchar(255), 
	@vchCurrentDBName as varchar(50)
AS
BEGIN
	DECLARE @vchServer varchar(50) 
	DECLARE @vchFilePath as varchar(255)

	DECLARE @intShell int
			,@iStm int
			,@intStdOut int
			,@intRC int
			,@intEndOfStream int 
			,@nvarText nvarchar(4000)
			,@nvchCmd nvarchar(4000)
			;
	BEGIN TRY

		SET NOCOUNT ON;
				
		SELECT  @vchFilePath = left(@vchDownLoadLogFileName, CharIndex('\Log', @vchDownLoadLogFileName)) + 'ExportSQLDataToIPC.EXE'
		SELECT  @vchServer =  Value1
			FROM tblControl WHERE [Key] ='ProbatInterfaceDB'

		SET @nvchCmd = 'Exec("cmd /c ' + @vchFilePath + ' '
									   + @vchServer + ' ' 
									   + @vchCurrentDBName + ' '
									   + @chrFacility + ' '
									   + @vchDownLoadLogFileName + '")';

			EXEC @intRC = sp_OACreate 'Wscript.Shell', @intShell OUT;
			EXEC @intRC = sp_OAMethod @intShell, @nvchCmd, @iStm OUT;
			EXEC @intRC = sp_OAGetProperty @iStm, 'StdOut', @intStdOut OUT;
			EXEC @intRC = sp_OAGetProperty @intStdOut, 'AtEndOfStream', @intEndOfStream OUT;
			WHILE @intEndOfStream = 0 BEGIN
				EXEC @intRC = sp_OAGetProperty @intStdOut, 'ReadLine', @nvarText OUT;
				--print @nvarText;
				EXEC @intRC = sp_OAGetProperty @intStdOut, 'AtEndOfStream', @intEndOfStream OUT;
			END
			EXEC @intRC = sp_OADestroy @intShell;	

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

		SELECT 
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d, Message:' + CHAR(13) + ERROR_MESSAGE();

		-- Use RAISERROR inside the CATCH block to return error
		-- information about the original error that caused
		-- execution to jump to the CATCH block.
		RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)
	END CATCH
END

GO

