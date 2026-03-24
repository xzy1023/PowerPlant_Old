

-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 06, 2013
-- Description:	Ping the device to check network connectivity
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_IsDeviceConnected] 
	@vchDevice varchar(30) 
	,@bitConnected bit = NULL OUTPUT
AS
BEGIN
	
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
		SET @bitConnected = 0;
		SET @nvchCmd = 'Exec("cmd /c ping -n 3 -w 1000 ' + @vchDevice + '")';

		EXEC @intRC = sp_OACreate 'Wscript.Shell', @intShell OUT;
		EXEC @intRC = sp_OAMethod @intShell, @nvchCmd, @iStm OUT;
		EXEC @intRC = sp_OAGetProperty @iStm, 'StdOut', @intStdOut OUT;
		EXEC @intRC = sp_OAGetProperty @intStdOut, 'AtEndOfStream', @intEndOfStream OUT;
		WHILE @intEndOfStream = 0 BEGIN
			EXEC @intRC = sp_OAGetProperty @intStdOut, 'ReadLine', @nvarText OUT;
			If charindex('Minimum',@nvarText) > 0
				set @bitConnected = 1;
			print @nvarText;
			EXEC @intRC = sp_OAGetProperty @intStdOut, 'AtEndOfStream', @intEndOfStream OUT;
		END
		EXEC @intRC = sp_OADestroy @intShell;

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

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
	END CATCH;
END

GO

