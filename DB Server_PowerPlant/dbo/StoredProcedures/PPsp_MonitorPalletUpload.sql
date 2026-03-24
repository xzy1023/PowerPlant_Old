
-- =============================================
-- Created:	    Oct. 25, 2018   Bong Lee
-- Description:	Monitor Pallet Upload. If the no. of printed but unposted LPs
--				in any sites is great than the threshold, send out email.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_MonitorPalletUpload]
	@intThreshold int = 15
AS
BEGIN

DECLARE @vchMsgBody nvarchar (MAX)
DECLARE @vchSubject nvarchar(512)
DECLARE @vchServerName nvarchar(50)
DECLARE @vchEarliest nvarchar(50)
DECLARE @vchLatest nvarchar(50) 
DECLARE @intCountHO as integer
DECLARE @intCountSP as integer
DECLARE @intCountFW as integer
DECLARE @intCountAJ as integer
DECLARE @intUpperLimit as integer

	BEGIN TRY
		SET @vchServerName = N'MPHOPP01'
		SELECT @intCountHO = Count(*) FROM Powerplant_prd.dbo.tblPallet WHERE PrintStatus = 2
		SET @vchServerName = N'MPSPPP01'
		SELECT @intCountSP = Count(*) FROM MPSPPP01.PowerPlant_prd.dbo.tblPallet WHERE PrintStatus = 2
		SET @vchServerName = N'MPFWPP01'
		SELECT @intCountFW = Count(*) FROM MPFWPP01.PowerPlant_prd.dbo.tblPallet tblPallet WHERE PrintStatus = 2
		--SET @vchServerName = N'MPAJPP01'
		--SELECT @intCountAJ = Count(*) FROM MPAJPP01.PowerPlant_prd.dbo.tblPallet tblPallet WHERE PrintStatus = 2
		SET @vchServerName = N''

		SET @intUpperLimit = @intThreshold + 50

		  If (@intCountHO > @intThreshold AND @intCountHO < @intUpperLimit) 
			OR (@intCountSP > @intThreshold AND @intCountSP < @intUpperLimit)
			OR (@intCountFW > @intThreshold AND @intCountFW < @intUpperLimit)
			OR (@intCountAJ > @intThreshold AND @intCountAJ < @intUpperLimit)
		  BEGIN
			--SELECT @vchServerName = CAST(SERVERPROPERTY('ServerName') as nvarchar(50))
			SELECT @vchSubject = N'At least one of the plants has more than ' + CAST(@intThreshold  as nvarchar(5)) + N' LPs waiting for AX to post.'
			SET @vchMsgBody = N'LP counts: HO - ' + CAST(@intCountHO as nvarchar(5)) +
								'; SP - ' + CAST(@intCountSP as nvarchar(5)) +
								'; FW - ' + CAST(@intCountFW as nvarchar(5)) +
								'; AJ - ' + CAST(@intCountAJ as nvarchar(5)) 
			EXEC PPsp_SndMsgToOperator @vchMsgBody ,@vchSubject, @vchName=N'Pallet Upload Alert'
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
			@ErrorProcedure = ISNULL(ERROR_PROCEDURE(),'PPsp_MonitorPalletUpload'),
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

