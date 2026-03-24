
-- =============================================
-- Created:	    Jan. 17, 2017   Bong Lee
-- Description:	Check label print queue. If the sumbitted time between the first and last entries 
--				is great than the threshold, send out email.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_CheckLabelPrintQueue]
	@intThresholdMinutes int = 10
AS
BEGIN

DECLARE @intTimeDiff as integer
DECLARE @vchMsgBody nvarchar (MAX)
DECLARE @vchSubject nvarchar(512)
DECLARE @vchServerName nvarchar(50)
DECLARE @vchEarliest nvarchar(50)
DECLARE @vchLatest nvarchar(50) 

	BEGIN TRY
	SELECT  @intTimeDiff = DateDiff(minute,Min(TimeSubmit),Max(TimeSubmit))
		, @vchEarliest = CONVERT(nvarchar(50), Min(TimeSubmit), 120)
		, @vchLatest =CONVERT(nvarchar(50),Max(TimeSubmit),120)
	FROM [dbo].[tblCimControlJob]

	  If @intTimeDiff > @intThresholdMinutes 
	  BEGIN
		SELECT @vchServerName = CAST(SERVERPROPERTY('ServerName') as nvarchar(50))
		SELECT @vchSubject = N'Print program has not printed for ' + CAST(@intTimeDiff as nvarchar(10)) + N' minutes on server ' + @vchServerName 
		SET @vchMsgBody = N'http://' + @vchServerName + N':55000   Earliest submitted: ' + @vchEarliest + N', Latest submitted: ' + @vchLatest
		EXEC PPsp_SndMsgToOperator @vchMsgBody ,@vchSubject, @vchName=N'Print Queue Monitor Notification List'
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

