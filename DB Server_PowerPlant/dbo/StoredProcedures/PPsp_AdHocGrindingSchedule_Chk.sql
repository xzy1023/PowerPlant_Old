
-- =============================================
-- Author:		Bong Lee
-- Create date: April 11, 2008
-- Description:	Check Grinding Ad-Hoc Schedule 
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_AdHocGrindingSchedule_Chk]
	@vchGrinder varchar(6),
	@vchFacility as varchar(3),
	--@intProductionDate as int,
	@vchRequestStartDate as varchar(10),
	@vchRequestStartTime as varchar(6),
	@vchRequestEndDate as varchar(10),
	@vchRequestEndTime as varchar(6),
	@vchScheduleID as varchar(50) = '',
	@vchMessage as varchar(256) OUTPUT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @vchStartTime varchar(14), @vchEndTime varchar(14);
	DECLARE @vchRequestStartFrom as varchar(14), @vchRequestEndAt as varchar(14);
	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);

	BEGIN TRY;

	Select @vchRequestStartFrom = CONVERT(CHAR(8),CAST(@vchRequestStartDate AS DateTime),112) + @vchRequestStartTime,
		   @vchRequestEndAt = CONVERT(CHAR(8),CAST(@vchRequestEndDate AS DateTime),112) + @vchRequestEndTime

	CREATE TABLE #tblAllGrindingSchedules (ScheduleID varchar(50) , StartDateTime varchar(14), EndDateTime varchar(14)) 
	
	INSERT INTO #tblAllGrindingSchedules
	SELECT tGS.ScheduleID, CAST(tGS.StartDate AS CHAR(8)) + dbo.fnFillLeadingZeros(6,tGS.StartTime), 
			CAST(tGS.EndDate AS Char(8)) + dbo.fnFillLeadingZeros(6,tGS.EndTime) FROM (
		SELECT GSCACTI AS ScheduleID, GSCSDTE AS StartDate, GSCSTIM AS StartTime, GSCEDTE AS EndDate, GSCETIM AS EndTime 
			FROM dbo.tblGrindingSchedule 
			WHERE GSCFAC = @vchFacility AND GSCGRNDR = @vchGrinder and (GSCSDTE BETWEEN CONVERT(CHAR(8),CAST(@vchRequestStartDate AS DateTime) - 1,112) 
			AND CONVERT(CHAR(8),CAST(@vchRequestEndDate AS DateTime) + 1,112))
		Union
		SELECT ScheduleID, StartDate, StartTime, EndDate, EndTime from dbo.tblAdHocGrindingSchedule tAGS
			WHERE Facility = @vchFacility AND Grinder = @vchGrinder and (StartDate BETWEEN CONVERT(CHAR(8),CAST(@vchRequestStartDate AS DateTime) - 1,112)
			AND CONVERT(CHAR(8),CAST(@vchRequestEndDate AS DateTime) + 1,112)) AND ScheduleID <> @vchScheduleID
		) AS tGS
		LEFT OUTER JOIN dbo.tblGrindingLog AS tGL on tGS.ScheduleID = tGL.ScheduleID
		WHERE (Status <> 'Dropped' or Status is null)
	
	-- use the requested start time to set lower limit on start time
	-- if the requested start time is between scheduled start time and end time then
			-- Overlap
	-- else
	-- use the requested End time to set lower limit on start time
	-- if the requested end time is between scheduled start time and end time then
			-- Overlap

	SELECT top 1 @vchStartTime = StartDateTime, @vchEndTime= EndDateTime from #tblAllGrindingSchedules
	WHERE startDatetime <=  @vchRequestStartFrom
--	WHERE startDatetime <= '20080221161000'
	ORDER BY startDatetime desc

	IF @vchRequestStartFrom >= @vchStartTime and @vchRequestStartFrom <= @vchEndTime
		SET @vchMessage = 'Overlap another schedule.'
	ELSE
	BEGIN
		SELECT top 1 @vchStartTime = StartDateTime, @vchEndTime= EndDateTime from #tblAllGrindingSchedules
		WHERE startDatetime <= @vchRequestEndAt		-- end time
--		WHERE startDatetime <= '20080221180000' -- end time
		order by startDatetime desc
		
		IF @vchRequestEndAt >= @vchStartTime and @vchRequestEndAt <= @vchEndTime
			SET @vchMessage = 'Overlap another schedule.'
		Else
			IF 	@vchRequestStartFrom <= @vchStartTime	
			SET @vchMessage = 'Overlap another schedule.'		
	END
	DROP TABLE #tblAllGrindingSchedules
	END TRY

	BEGIN CATCH
		
		DROP TABLE #tblAllGrindingSchedules
		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorNumber = ERROR_NUMBER(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');

		RAISERROR 
			(
			@ErrorMessage, 
			@ErrorSeverity, 
			@ErrorState,     -- parameter: original error state.               
			@ErrorNumber,    -- parameter: original error number.
			@ErrorSeverity,  -- parameter: original error severity.
			@ErrorProcedure, -- parameter: original error procedure name.
			@ErrorLine       -- parameter: original error line number.
			);
	END CATCH
END

GO

