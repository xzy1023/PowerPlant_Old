
-- =============================================
-- Created:	    Oct. 25, 2018   Bong Lee
-- Description:	Monitor number of load G transactions in Probat have not been transferred to ERP.
--				If it is great than the threshold, send out alert email.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_MonitorProbatLoadGTx]
	@intThreshold int = 200
	,@intThresholdUpperLimit as integer = 1200
	,@intThreholdBeyondLimit as integer = 2000
AS
BEGIN

DECLARE @vchMsgBody nvarchar (MAX)
DECLARE @vchSubject nvarchar(512)
DECLARE @vchServerName nvarchar(50)
DECLARE @vchEarliest nvarchar(50)
DECLARE @vchLatest nvarchar(50) 
DECLARE @intCount int


	BEGIN TRY
		
		SELECT @intCount = Count(*) FROM PRO_EXP_ORDER_LOAD_G WHERE TRANSFERED = 0

		  If (@intCount > @intThreshold AND @intCount < @intThresholdUpperLimit) 
			OR @intCount > @intThreholdBeyondLimit
		  BEGIN
			--SELECT @vchServerName = CAST(SERVERPROPERTY('ServerName') as nvarchar(50))
			SELECT @vchSubject = N'Main plant Probat Exp_Load_G table has more than ' + CAST(@intThreshold  as nvarchar(5)) + N' unprocessed records waiting for AX to process.'
			SET @vchMsgBody = N'No. of Unprocessed Records: ' + CAST(@intCount as nvarchar(5)) 
 
			EXEC PPsp_SndMsgToOperator @vchMsgBody ,@vchSubject, @vchName=N'Probat Alert Notification List'
		  END

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
			@ErrorProcedure = ISNULL(ERROR_PROCEDURE(),'PPsp_MonitorProbatLoadGTx'),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d, Message:' + CHAR(13) + ERROR_MESSAGE();

		BEGIN TRY
			SELECT 
				@nvchSubject = @@ServerName + '.' + DB_NAME() + ' - ' + @ErrorProcedure + N' fails.',
				@nvchBody = N'Monitoring on server: ' + @vchServerName + CHAR(13) +
							N'Message: '  + ERROR_MESSAGE() + CHAR(13) +
							N'Error ' + CAST(@ErrorNumber as varchar(10)) +
							N', Level ' + CAST(@ErrorSeverity as varchar(10)) +
							N', State ' + CAST(@ErrorState as varchar(10)) + 
							N', Procedure ' + @ErrorProcedure +
							N', Line ' + CAST(@ErrorLine as varchar(10))
							; 
			EXEC PPsp_SndMsgToOperator @nvchBody, @nvchSubject;

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

