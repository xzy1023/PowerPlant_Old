



-- =============================================
-- Author:		Bong Lee
-- Create date: Sep. 5, 2008
-- Description:	Select Roasting Log
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoastingLog_Sel] 
	@vchAction varchar(50) , 
	@chrFacility char(3),
	@vchRoaster varchar(10) = NULL,
	@vchBlend varchar(6) =NULL ,
	@dteFromTime dateTime = NULL,
	@dtetoTime dateTime = NULL,
	@intShift tinyint = 0,
	@intNoOfLastReading tinyint = 0
 AS
BEGIN
	DECLARE @dteCurTime as datetime
	DECLARE @dteCurShiftFmTime as datetime
--	DECLARE @intPrevShift int
	DECLARE @vchWorkGroup varchar(10)

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	BEGIN TRY
		IF @vchAction = 'ForCharts'
		BEGIN
			SET @dteCurTime = getdate()
			SET @vchWorkGroup = 'R'

	--		SELECT @intPrevShift = CASE WHEN @intShift - 1  > 0 THEN @intShift - 1 ELSE 
	--				(SELECT MAX(Shift) from tblShift WHERE Facility = @chrFacility and WorkGroup = @vchWorkGroup) END

			SELECT @dteCurShiftFmTime =  
				CASE WHEN FromTime < ToTime OR (FromTime > ToTime AND (CONVERT(varchar(8),@dteCurTime,14) > CONVERT(varchar(8),ToTime,14)))
				 THEN CONVERT(datetime,CONVERT(varchar(10),@dteCurTime,110) + ' ' + CONVERT(varchar(8),FromTime,14),120) 
				 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,-1,@dteCurTime),110) + ' ' + CONVERT(varchar(8),FromTime,14),120) 
				END, @dtetoTime =
				CASE WHEN FromTime < ToTime OR (FromTime > ToTime AND (CONVERT(varchar(8),@dteCurTime,14) <= CONVERT(varchar(8),ToTime,14)))
				 THEN CONVERT(datetime,CONVERT(varchar(10),@dteCurTime,110) + ' ' + CONVERT(varchar(8),ToTime,14),120) 
				 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,1,@dteCurTime),110) + ' ' + CONVERT(varchar(8),ToTime,14),120) 
				END 
				FROM dbo.tblShift 
				WHERE Facility = @chrFacility and WorkGroup = @vchWorkGroup and Shift = @intShift

			SET @dteFromTime = @dteCurShiftFmTime;

			WITH tblPrevRoastingLog(Facility, DateTest, Shift, BatchNo, RoasterNo, Blend, Colour, Moisture, FinalTemp, Quench, RoasterInit, Rejected, Comments,
								   SpecMin, SpecMax, SpecTarget, MaxMoisture, TargetMoisture, MinMoisture, SpecFinalTemp) AS
			(
					SELECT top (@intNoOfLastReading) *
					FROM tblRoastingLog
					WHERE Facility = @chrFacility AND (DateTest< @dteCurShiftFmTime) AND RoasterNo = @vchRoaster AND Blend = @vchBlend AND Rejected <> 'Y'
					ORDER BY DateTest desc
			)
			SELECT * FROM tblPrevRoastingLog
			UNION
			SELECT * FROM tblRoastingLog
			WHERE Facility = @chrFacility AND (DateTest BETWEEN @dteFromTime AND @dteToTime) AND RoasterNo = @vchRoaster AND Blend = @vchBlend AND Rejected <> 'Y'
			ORDER BY DateTest
		END
		ELSE
			IF @vchAction = 'ByDateRange'
			BEGIN
				SELECT * FROM tblRoastingLog 
				WHERE Facility = @chrFacility
					AND (DateTest Between @dteFromTime and @dteToTime ) 
					AND (@vchRoaster is null OR RoasterNo = @vchRoaster)
					AND (@vchBlend is null OR Blend = @vchBlend)
				ORDER BY 
					CASE WHEN @vchRoaster is NULL THEN RoasterNo END,
					CASE WHEN @vchBlend is NULL THEN Blend END,
					DateTest Desc 
			END
			ELSE
				IF @vchAction = 'Last2LogByRoasterBlend'
				BEGIN
					SELECT Top 2 * FROM tblRoastingLog 
						WHERE Facility = @chrFacility 
								AND RoasterNo = @vchRoaster 
								AND tblRoastingLog.blend = @vchBlend
						ORDER BY DateTest DESC
				END

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

