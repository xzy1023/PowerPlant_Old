

-- =============================================
-- Author:		Bong Lee
-- Create date: Dec. 05, 2006
-- Description:	Get Production Date By Shift
-- Assumptions: 3 shifts a day.
--				Either shift no. 2 or 3 can be cross mid-night.
-- WO#194:		Sep. 9, 2010	Bong Lee	
-- Description:	If the time frame for the expected shift crosses mid-night
--				and the given time is prior to mid-night, the Shift Production 
--				Date will be the given time plus one day. Before this change the 	
--				Shift Production Date for this case was same as the given time but 
--				if the given time after mid-night, the result was given time 
--				minus one day.
-- WO#2645:		Oct. 25, 2015	Bong Lee
--				When the given date fails on the cross over midnight shift,
--				the shift production date is not always the date of the ending date of the midnight shift
--				but it can be the date of the starting date.
-- WO#3695:		Nov. 29, 2016	Bong Lee
--				Impplement 4 shifts pattern. The shift no. and shift production date are also depended on the shift pattern.
-- =============================================
CREATE FUNCTION [dbo].[fnGetProdDateByShift]
(
	@chrFacility char(3),
	@intGivenShiftNo tinyint,
	@dteGivenDateTime datetime, 
	@vchMachineID varchar(10) = NULL,
	@vchWorkGroup varchar(3) = NULL
)
RETURNS DateTime
AS
BEGIN
	DECLARE @intCurrentShift AS tinyint;
	DECLARE @chrBegTimeOfADay AS char(8); 
	DECLARE @chrCurrentTime AS char(8); 
	DECLARE @dteProductionDate datetime;
	DECLARE @dteFmTime as datetime
	DECLARE @dteToTime as datetime
-- WO#2645	DECLARE @intShiftNo as int
	DECLARE @vchDescription as varchar(50)
	DECLARE @vchInputTime as varchar(8)
-- WO#194 DECLARE @vchFirstShift_StartTime as varchar(8)
	DECLARE @dteGivenDateMinusOne DateTime,
			@dteGivenDatePlusOne DateTime,
			@intExpectedShiftNo int,
			@vchExpectedShiftNo_StartTime varchar(8),
-- WO#194	@vchExpectedShiftNo_EndTime varchar(8),
-- WO#194	@vchGivenShiftNo_StartTime varchar(8),
-- WO#194	@vchGivenShiftNo_EndTime varchar(8),
-- WO#194	@intLastShiftNo tinyint,
			@vchGivenTime as varchar(8),	-- hh:mm:ss
			@bitExpectedShiftXMidNight as bit,
			@bitGivenShiftXMidNight as bit
			,@intShiftsPerDay as tinyint						-- WO#2645
			,@bitUseSEDateForShiftPD as bit						-- WO#2645
			,@bit4ShiftsPattern as bit							-- WO#3695
			,@dteExpectedShiftEndTime as datetime				-- WO#3695
			,@intExpectedShiftNo_Prior as int					-- WO#3695
			,@intExpectedShiftNo_Next as int					-- WO#3695
	-- Initialize variables
	Select  @vchGivenTime = CONVERT(varchar(8),@dteGivenDateTime,14)

	-- Find the expected shift no from the Given Time
	If @vchMachineID is not null
		select @vchWorkGroup = WorkShiftType from tblComputerConfig Where PackagingLine = @vchMachineID

	-- WO#2645 DEL Start
	--DECLARE object_cursor CURSOR FOR
	--SELECT Shift,FromTime,ToTime,Description
	--		FROM dbo.tblShift 
	--		WHERE Facility = @chrFacility and WorkGroup = @vchWorkGroup
	--		ORDER BY Shift
	--OPEN object_cursor

	--FETCH NEXT FROM object_cursor INTO @intShiftNo,@dteFmTime,@dteToTime,@vchDescription

	--WHILE @@FETCH_STATUS = 0
	--BEGIN
	--	IF @dteFmTime < @dteToTime
	--	BEGIN
	--		IF @vchGivenTime >= CONVERT(varchar(8),@dteFmTime,14) AND
	--		   @vchGivenTime <= CONVERT(varchar(8),@dteToTime,14)	
	--		Break
	--	END
	--	ELSE
	--	BEGIN
	--		IF (@vchGivenTime >= CONVERT(varchar(8),@dteFmTime,14)) OR
	--		   (@vchGivenTime <= CONVERT(varchar(8),@dteToTime,14))
	--		Break
	--	END
	--	FETCH NEXT FROM object_cursor INTO @intShiftNo,@dteFmTime,@dteToTime,@vchDescription
	--END

	--CLOSE object_cursor
	--DEALLOCATE object_cursor

	--SET @intExpectedShiftNo = @intShiftNo
	-- WO#2645 DEL Stop
	-- WO#2645 ADD Start
	SELECT @intExpectedShiftNo = dbo.fnGetExpectedShift(@chrFacility, @dteGivenDateTime, @vchWorkGroup)
	-- WO#2645 ADD Stop

	-- Shift time based on the Calculated Shift No from the Given Date Time
	Select @dteFmTime = FromTime, @vchExpectedShiftNo_StartTime = CONVERT(varchar(8),FromTime,14),
			@dteToTime = ToTime, 
--	WO#194	@vchExpectedShiftNo_EndTime = CONVERT(varchar(8),ToTime,14),
			@vchDescription = Description, @bitExpectedShiftXMidNight =  CASE WHEN FromTime > ToTime THEN 1 ELSE  0 END 
			,@intShiftsPerDay = ShiftsPerDay								-- WO#2645
			,@bitUseSEDateForShiftPD = UseSEDateForShiftPD					-- WO#2645	
			--WO#3695 ADD Start
			,@bit4ShiftsPattern = CASE WHEN ISNULL(ShiftPatternCode,0) > 0 THEN 1 ELSE 0 END	
			,@dteExpectedShiftEndTime = CASE WHEN FromTime < ToTime OR (FromTime > ToTime AND (CONVERT(varchar(8),@dteGivenDateTime,14) <= CONVERT(varchar(8),ToTime,14)))
				THEN CONVERT(datetime,CONVERT(varchar(10),@dteGivenDateTime,110) + ' ' + CONVERT(varchar(8),ToTime,14),120) 
				ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,1,@dteGivenDateTime),110) + ' ' + CONVERT(varchar(8),ToTime,14),120)
				END		 
			--WO#3695 ADD Stop
	FROM tblShift
	WHERE Facility = @chrFacility and WorkGroup = @vchWorkGroup and shift = @intExpectedShiftNo

	 --WO#3695 ADD Start
	IF @bit4ShiftsPattern = 1
	BEGIN
		-- Assuming 12 hours per shift
		SELECT @intExpectedShiftNo_Prior = dbo.fnGetExpectedShift(@chrFacility, DateAdd(hour, -12, @dteGivenDateTime), @vchWorkGroup)
		SELECT @intExpectedShiftNo_Next  = dbo.fnGetExpectedShift(@chrFacility, DateAdd(hour,  12, @dteGivenDateTime), @vchWorkGroup)
--		SELECT @dteExpectedShiftEndTime = ShiftEndTime
--			FROM [dbo].[fnShiftInfo] (@chrFacility, @dteGivenDateTime,	@intExpectedShiftNo, @vchWorkGroup,NULL)
	END
	 --WO#3695 ADD Stop
	 	
	-- Shift time based on the Given Shift No
-- WO#194 Del Start 
--	Select  @vchGivenShiftNo_StartTime = CONVERT(varchar(8),FromTime,14),
--			@vchGivenShiftNo_EndTime = CONVERT(varchar(8),ToTime,14),
-- WO#194 Del Stop
	SELECT		-- WO#194
			@bitGivenShiftXMidNight =  CASE WHEN FromTime > ToTime THEN 1 ELSE  0 END
	FROM tblShift
	WHERE Facility = @chrFacility and WorkGroup = @vchWorkGroup and shift = @intGivenShiftNo
-- WO#194 Del Start
--
--	Select @vchFirstShift_StartTime = CONVERT(varchar(8),FromTime,14) FROM tblShift
--		WHERE Facility = @chrFacility and WorkGroup = @vchWorkGroup and shift = 1
-- 
--	Select @intLastShiftNo = Max(shift) FROM tblShift
--		WHERE Facility = @chrFacility and WorkGroup = @vchWorkGroup
-- WO#194 Del Stop

	/* ---------------------------------
		Determine the production date	
	   ---------------------------------*/
	-- Assumption: Mid-night does not fall between the Start and End time of first shift. 

	-- Set default Production Date in (yyyy-mm-dd 00:00:00)
	SET @dteProductionDate =  CONVERT(datetime,CONVERT(CHAR(10),@dteGivenDateTime,111),111)	
	SET @dteGivenDateMinusOne = DATEADD(day,-1,@dteProductionDate)
	SET @dteGivenDatePlusOne = DATEADD(day,1,@dteProductionDate)

-- WO#194 Add Start 
	If @bitExpectedShiftXMidNight = 1 	-- The expected shift crosses mid-night
	Begin
		IF @bitUseSEDateForShiftPD = 1							-- WO#2645
		BEGIN													-- WO#2645
			If @intExpectedShiftNo - @intGivenShiftNo = 1
			Begin
				-- Given Time passes mid-night
		  		If @vchGivenTime < @vchExpectedShiftNo_StartTime	
					SET @dteProductionDate = @dteGivenDateMinusOne
			End	
			Else
			Begin
				-- Given Time prior mid-night
				If @vchGivenTime >= @vchExpectedShiftNo_StartTime
					SET @dteProductionDate = @dteGivenDatePlusOne
			End
		-- WO#2645 ADD Start
		END
		ELSE
		BEGIN
			If @intShiftsPerDay = 2
			BEGIN
				-- WO#3695 ADD Start
				IF @bit4ShiftsPattern = 1
				BEGIN
					IF @intExpectedShiftNo = @intGivenShiftNo
					BEGIN 
						If @vchGivenTime < @vchExpectedShiftNo_StartTime
							SET @dteProductionDate = @dteGivenDateMinusOne
					END
					ELSE
					BEGIN
						IF @intExpectedShiftNo_Prior = @intExpectedShiftNo_Next
						BEGIN
							IF  DATEDIFF(second, @dteGivenDateTime,@dteExpectedShiftEndTime) > 7200
								SET @dteProductionDate = @dteGivenDateMinusOne
						END
						ELSE
						IF @intGivenShiftNo = @intExpectedShiftNo_Prior
						BEGIN
							IF  @vchGivenTime <= CONVERT(varchar(8),@dteExpectedShiftEndTime,14) 
								SET @dteProductionDate = @dteGivenDateMinusOne
						END
						ELSE
						BEGIN
							IF @intGivenShiftNo = @intExpectedShiftNo_Next
							BEGIN
								IF  @vchGivenTime > CONVERT(varchar(8),@dteExpectedShiftEndTime,14)
									SET @dteProductionDate = @dteGivenDatePlusOne
							END
						END
					END
				END
				ELSE
				BEGIN
				-- WO#3695 ADD Stop
					If @vchGivenTime < @vchExpectedShiftNo_StartTime AND @intExpectedShiftNo = @intGivenShiftNo 
					BEGIN
						SET @dteProductionDate = @dteGivenDateMinusOne
					END
				END		-- WO#3695
			END
		END
		-- WO#2645 ADD Stop
	End
	ELSE
	If @bitGivenShiftXMidNight = 1 	-- The Given shift crosses mid-night
	Begin
		-- WO#3695 ADD Start
		IF @bit4ShiftsPattern = 1
		BEGIN
			IF @bitUseSEDateForShiftPD = 0
			BEGIN
				IF @intExpectedShiftNo_Prior = @intExpectedShiftNo_Next
				BEGIN
					IF  DATEDIFF(second, @dteGivenDateTime,@dteExpectedShiftEndTime) > 7200
						SET @dteProductionDate = @dteGivenDateMinusOne
				END
				ELSE
					IF @intGivenShiftNo = @intExpectedShiftNo_Prior		-- stay late
					BEGIN
						SET @dteProductionDate = @dteGivenDateMinusOne
					END
			END
		END
		ELSE
		BEGIN
		-- WO#3695 ADD Stop
			If @intGivenShiftNo - @intExpectedShiftNo  = 1
				-- WO#3695	IF  @bitUseSEDateForShiftPD = 0 and @intShiftsPerDay <> 2					-- WO#2645
				-- WO#3695 IF @bitUseSEDateForShiftPD <> 0 then the Production date will be same as given date.
				IF  @bitUseSEDateForShiftPD = 0												-- WO#3695				
					IF @intShiftsPerDay <> 2												-- WO#3695
						SET @dteProductionDate = @dteGivenDatePlusOne
					ELSE																	-- WO#3695
						SET @dteProductionDate = @dteGivenDateMinusOne						-- WO#3695 
		END					-- WO#2645
	End
-- WO#194 Add Stop
	
-- WO#194 Del Start 
--	If @intExpectedShiftNo = @intGivenShiftNo
--	Begin
--		If @bitExpectedShiftXMidNight = 1 	-- The shift crosses mid-night
--		Begin
--			If @vchGivenTime < @vchExpectedShiftNo_StartTime 
--				SET @dteProductionDate =  @dteGivenDateMinusOne
--		End
--		ELSE
--		Begin
--			IF @vchGivenShiftNo_StartTime < @vchFirstShift_StartTime -- If the start time of the given shift is on next day of the 1st shift
--				SET @dteProductionDate =  @dteGivenDateMinusOne
--		End
--	End
--	Else
--	Begin
--		IF @intExpectedShiftNo < @intGivenShiftNo
--		Begin
--			IF @intGivenShiftNo - @intExpectedShiftNo	= 1
--			Begin
--				IF @bitExpectedShiftXMidNight = 1	-- The expected shift crosses mid-night
--				Begin
--					If @vchGivenTime < @vchExpectedShiftNo_StartTime	-- Given Time passes mid-night
--						SET @dteProductionDate = @dteGivenDateMinusOne	
--				End
--			End
--			Else
--			Begin
--				If @vchExpectedShiftNo_StartTime < @vchGivenShiftNo_StartTime
--					SET @dteProductionDate =  @dteGivenDateMinusOne
--				Else
--					If @vchGivenTime < @vchExpectedShiftNo_StartTime
--						SET @dteProductionDate =  @dteGivenDateMinusOne
--					Else
--					   If @intGivenShiftNo = @intLastShiftNo and @intExpectedShiftNo = 1
--							SET @dteProductionDate =  @dteGivenDateMinusOne
--			End 
--		END
--		Else	-- i.e. @intExpectedShiftNo > @intGivenShiftNo
--		Begin
--			If @bitGivenShiftXMidNight = 1 
--			Begin
--				SET @dteProductionDate =  @dteGivenDateMinusOne
--			End
--			Else
--			-- Start and End time of Given Shift are on same day 
--			Begin
--				If @intGivenShiftNo = 1 and @intExpectedShiftNo = @intLastShiftNo 
--				Begin
--					 If @vchExpectedShiftNo_StartTime > @vchGivenShiftNo_StartTime
--						SET @dteProductionDate =  @dteGivenDatePlusOne
--				End
--				Else
--				Begin
--					-- Start and End time of Expected Shift are on different days
--					If @bitExpectedShiftXMidNight = 1
--					Begin
--						If @vchGivenTime < @vchExpectedShiftNo_StartTime 
--							SET @dteProductionDate =  @dteGivenDateMinusOne
--					End
--				End
--			End
--		End
--	End
-- WO#194 Del Stop 

	Return CONVERT(datetime,CONVERT(CHAR(10),@dteProductionDate,111),111)
END

GO

