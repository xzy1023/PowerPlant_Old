

-- =============================================
-- Author:		Bong Lee
-- Create date: Feb 23, 2009
-- Description:	Get Work Shift information
-- Note: If shift no is not given (i.e. 0), it will find out the shift no
--		 from the given time first. Then it will find out and calculate the
--		 the rest of the information for the shift
--
-- This functon can also be use for Grinding and Roasting which they do not
-- have record in Computer Configure table, so for those groups the Work Group 
-- is required only.
-- WO#359		Mar. 1, 2012	Bong Lee	
--				Output another column - Shift Sequence 
--				Use vwLineWorkShiftType instead of tblComputerConfigure to get WorkShiftType
-- WO#3695:		Nov. 29, 2016	Bong Lee
--				Impplement 4 shifts pattern. The shift no. is also depended on 
--				the shift pattern.
-- =============================================
CREATE FUNCTION [dbo].[fnShiftInfo] 
(
	-- Add the parameters for the function here
	@chrFacility char(3),
	@dteGivenDateTime DateTime,
	@intGivenShiftNo int = 0,
	@vchWorkGroup varchar(10) = NULL,	-- required if vchMachineID is NULL
	@vchMachineID varchar(10) = NULL	-- required if vchWorkGroup is NULL
)
RETURNS 
@T TABLE 
(
	-- Add the column definitions for the TABLE variable here
	Shift tinyint, 
	ShiftStartTime datetime,
	ShiftEndTime datetime,
	ShiftTotalTime decimal(4,2),
	[Description] varchar(50),
	ProductionDate datetime
	,ShiftSequence int		--WO#359
	,Method int				-- WO#3695
)
AS
BEGIN
	DECLARE  @vchGivenDateTime as varchar(8)
	DECLARE  @dteFmTime as datetime
	DECLARE  @dteToTime as datetime
	DECLARE  @vchDescription as varchar(50)
	DECLARE  @dteStartTime as datetime
	DECLARE  @dteEndTime as datetime
	DECLARE  @decTotalShiftTime as decimal(4,2)
	DECLARE  @dteProductionDate as datetime
	DECLARE  @intExpectedShiftNo as tinyint
	DECLARE  @intShiftSequence int		--WO#359
	DECLARE  @intMethod int				--WO#3695
	
	-- Initialize variables
	Select  @dteStartTime = NULL, @dteEndTime = NULL, @decTotalShiftTime=0 ,@vchDescription='',
		 @vchGivenDateTime = CONVERT(varchar(8),@dteGivenDateTime,14)
		 ,@intShiftSequence = 0		--WO#359
		 ,@intMethod = 0			--WO#3695
		 	
	-- Get the Work Group ID from Packaging Line ID if Work Group ID is not provided.
	IF @vchWorkGroup is NULL and Not @vchMachineID is NULL
		--WO#359	Select @vchWorkGroup = WorkShiftType FROM tblComputerConfig
		SELECT @vchWorkGroup = ISNULL(						--WO#359
			(SELECT WorkShiftType FROM vwLineWorkShiftType	--WO#359
			WHERE Facility = @chrFacility and PackagingLine = @vchMachineID),'P')
		
	-- Get the expected shift from given date time if shift no is not provide
	IF @intGivenShiftNo = 0
	BEGIN
		SELECT @intGivenShiftNo = [dbo].[fnGetExpectedShift](@chrFacility ,@dteGivenDateTime, @vchWorkGroup)		-- WO#3695
		--WO#3695 DEL Start
		--DECLARE object_cursor CURSOR FOR
		--SELECT Shift,FromTime,ToTime,[Description]
		--	   ,ShiftSequence 		--WO#359	
		--		FROM dbo.tblShift 
		--		WHERE Facility = @chrFacility and WorkGroup = @vchWorkGroup
		--		ORDER BY Shift
		--OPEN object_cursor

		--FETCH NEXT FROM object_cursor INTO @intExpectedShiftNo,@dteFmTime,@dteToTime,@vchDescription
		--								   ,@intShiftSequence		--WO#359

		--WHILE @@FETCH_STATUS = 0
		--BEGIN
		--	IF @dteFmTime < @dteToTime
		--	BEGIN
		--		IF @vchGivenDateTime >= CONVERT(varchar(8),@dteFmTime,14) AND
		--		   @vchGivenDateTime <= CONVERT(varchar(8),@dteToTime,14)	
		--		Break
		--	END
		--	ELSE
		--	BEGIN
		--		IF (@vchGivenDateTime >= CONVERT(varchar(8),@dteFmTime,14)) OR
		--		   (@vchGivenDateTime <= CONVERT(varchar(8),@dteToTime,14))
		--		Break
		--	END
		--	FETCH NEXT FROM object_cursor INTO @intExpectedShiftNo,@dteFmTime,@dteToTime,@vchDescription
		--									   ,@intShiftSequence		--WO#359	
		--END

		--Set @intGivenShiftNo = @intExpectedShiftNo
		--CLOSE object_cursor
		--DEALLOCATE object_cursor
		--WO#3695 DEL Stop
	END
--WO#3695	ELSE
--WO#3695	BEGIN
		Select @dteFmTime = FromTime, @dteToTime = ToTime, @vchDescription = [Description]
		    ,@intShiftSequence = ShiftSequence		--WO#359
			,@intMethod = Method					-- WO#3695
			FROM tblShift
			WHERE Facility = @chrFacility and WorkGroup = @vchWorkGroup and shift = @intGivenShiftNo
--WO#3695	END 

	-- Get the detail info. of expected shift
	If @intGivenShiftNo >0 
	BEGIN
		SELECT
			@dteStartTime = CASE WHEN @dteFmTime < @dteToTime OR (@dteFmTime> @dteToTime AND (CONVERT(varchar(8),@dteGivenDateTime,14) > CONVERT(varchar(8),@dteToTime,14)))
				 THEN CONVERT(datetime,CONVERT(varchar(10),@dteGivenDateTime,110) + ' ' + CONVERT(varchar(8),@dteFmTime,14),120) 
				 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,-1,@dteGivenDateTime),110) + ' ' + CONVERT(varchar(8),@dteFmTime,14),120) 
			END,
			@dteEndTime = CASE WHEN @dteFmTime < @dteToTime OR (@dteFmTime > @dteToTime AND (CONVERT(varchar(8),@dteGivenDateTime,14) <= CONVERT(varchar(8),@dteToTime,14)))
				 THEN CONVERT(datetime,CONVERT(varchar(10),@dteGivenDateTime,110) + ' ' + CONVERT(varchar(8),@dteToTime,14),120) 
				 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,1,@dteGivenDateTime),110) + ' ' + CONVERT(varchar(8),@dteToTime,14),120)
			END,
			@decTotalShiftTime = ROUND(ROUND(DateDiff(second, (CASE WHEN @dteFmTime< @dteToTime OR (@dteFmTime> @dteToTime AND (CONVERT(varchar(8),@dteGivenDateTime,14) > CONVERT(varchar(8),@dteToTime,14)))
				 THEN CONVERT(datetime,CONVERT(varchar(10),@dteGivenDateTime,110) + ' ' + CONVERT(varchar(8),@dteFmTime,14),120) 
				 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,-1,@dteGivenDateTime),110) + ' ' + CONVERT(varchar(8),@dteFmTime,14),120) END),
				(CASE WHEN @dteFmTime < @dteToTime OR (@dteFmTime > @dteToTime AND (CONVERT(varchar(8),@dteGivenDateTime,14) <= CONVERT(varchar(8),@dteToTime,14)))
				 THEN CONVERT(datetime,CONVERT(varchar(10),@dteGivenDateTime,110) + ' ' + CONVERT(varchar(8),@dteToTime,14),120) 
				 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,1,@dteGivenDateTime),110) + ' ' + CONVERT(varchar(8),@dteToTime,14),120) END))/3600.000,2),1) 
	
		/* ---------------------------------
			Determine the production date	
		   ---------------------------------*/

		Select @dteProductionDate = dbo.fnGetProdDateByShift (@chrFacility,@intGivenShiftNo,@dteGivenDateTime ,@vchMachineID,@vchWorkGroup)
	END

	INSERT INTO @t
		VALUES(@intGivenShiftNo, @dteStartTime, @dteEndTime, @decTotalShiftTime ,@vchDescription, @dteProductionDate
				,@intShiftSequence		--WO#359
				,@intMethod				-- WO#3695
				)						--WO#359
--WO#359  VALUES(@intGivenShiftNo, @dteStartTime, @dteEndTime, @decTotalShiftTime ,@vchDescription, @dteProductionDate)
		

	RETURN 
END

GO

