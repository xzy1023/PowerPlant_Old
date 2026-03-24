
-- =============================================
-- Author:		Bong Lee
-- Create date: Sept. 21, 2006
-- Description:	Shift I/O Module
-- WO#194:		Sep. 9, 2010	Bong Lee	
-- Description:	If the time frame for the expected shift crosses mid-night
--				and the given time is prior to mid-night, the Shift Production 
--				Date will be the given time plus one day. Before this change the 	
--				Shift Production Date for this case was same as the given time but 
--				if the given time after mid-night, the result was given time 
--				minus one day.
-- WO#359:		Jun. 17, 2011	Bong Lee	
--				Add Action 'AllShiftsWithBlank' & 'AllShiftsWithZero'
-- WO#2645:		Oct. 25, 2015	Bong Lee
--				Add parameter for selecting displayable shift
-- WO#3695:		Dev. 13, 2016	Bong Lee
--				Implement 4 shifts pattern. The shift no. is also depended on 
--				the shift pattern. Show shift method field to result set.
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_ShiftIO] 
	-- Add the parameters for the stored procedure here
	@vchAction varchar(30),
	@chrFacility char(3), 
	@vchWorkGroup varchar(10),
	@dteTime datetime = NULL,
	@intShift int = 0
	,@bitDisableDisplayable bit = 0							-- WO#2645
AS
BEGIN
	DECLARE  @dteFmTime as datetime
	DECLARE  @dteToTime as datetime
	DECLARE  @intShiftNo as int
	DECLARE  @vchDescription as varchar(50)
	DECLARE  @vchInputTime as varchar(8)
	DECLARE  @intShiftSequence as tinyint	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- WO#2645 ADD Start
	IF @bitDisableDisplayable is null 
		SET @bitDisableDisplayable = 0;
	IF @intShift is null
		SET @intShift = 0;
	-- WO#2645 ADD Stop

	SET @vchInputTime = CONVERT(varchar(8),@dteTime,14)
    -- Expected shift no
	IF @vchAction = 'ExpectedShift'
	BEGIN
		-- WO#2645 DEL Start
		/*
		DECLARE object_cursor CURSOR FOR
		SELECT Shift,FromTime,ToTime,Description
				,ShiftSequence			-- WO#194
				FROM dbo.tblShift 
				WHERE Facility = @chrFacility and WorkGroup = @vchWorkGroup
				ORDER BY Shift
		OPEN object_cursor

		FETCH NEXT FROM object_cursor INTO @intShiftNo,@dteFmTime,@dteToTime,@vchDescription
				,@intShiftSequence		-- WO#194

		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF @dteFmTime < @dteToTime
			BEGIN
				IF @vchInputTime >= CONVERT(varchar(8),@dteFmTime,14) AND
				   @vchInputTime <= CONVERT(varchar(8),@dteToTime,14)	
				Break
			END
			ELSE
			BEGIN
				IF (@vchInputTime >= CONVERT(varchar(8),@dteFmTime,14)) OR
				   (@vchInputTime <= CONVERT(varchar(8),@dteToTime,14))
				Break
			END
			FETCH NEXT FROM object_cursor INTO @intShiftNo,@dteFmTime,@dteToTime,@vchDescription
				,@intShiftSequence		-- WO#194
		END
			SELECT @intShiftNo as Shift, @vchDescription as Description,
				@intShiftSequence as ShiftSequence,			-- WO#194
				CASE WHEN @dteFmTime < @dteToTime OR (@dteFmTime > @dteToTime AND (CONVERT(varchar(8),@dteTime,14) > CONVERT(varchar(8),@dteToTime,14)))
					 THEN CONVERT(datetime,CONVERT(varchar(10),@dteTime,110) + ' ' + CONVERT(varchar(8),@dteFmTime,14),120) 
					 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,-1,@dteTime),110) + ' ' + CONVERT(varchar(8),@dteFmTime,14),120) 
				END FromTime,
				CASE WHEN @dteFmTime < @dteToTime OR (@dteFmTime > @dteToTime AND (CONVERT(varchar(8),@dteTime,14) <= CONVERT(varchar(8),@dteToTime,14)))
					 THEN CONVERT(datetime,CONVERT(varchar(10),@dteTime,110) + ' ' + CONVERT(varchar(8),@dteToTime,14),120) 
					 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,1,@dteTime),110) + ' ' + CONVERT(varchar(8),@dteToTime,14),120)
 				END ToTime
		CLOSE object_cursor
		DEALLOCATE object_cursor
		*/
		-- WO#2645 DEL Stop
		-- WO#2645 ADD Start
		SELECT Shift, Description 
			 ,ShiftSequence
			 ,CASE WHEN FromTime < ToTime OR (FromTime > ToTime AND (CONVERT(varchar(8),@dteTime,14) > CONVERT(varchar(8),ToTime,14)))
				 THEN CONVERT(datetime,CONVERT(varchar(10),@dteTime,110) + ' ' + CONVERT(varchar(8),FromTime,14),120) 
				 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,-1,@dteTime),110) + ' ' + CONVERT(varchar(8),FromTime,14),120) 
			END FromTime
			,CASE WHEN FromTime < ToTime OR (FromTime > ToTime AND (CONVERT(varchar(8),@dteTime,14) <= CONVERT(varchar(8),ToTime,14)))
				 THEN CONVERT(datetime,CONVERT(varchar(10),@dteTime,110) + ' ' + CONVERT(varchar(8),ToTime,14),120) 
				 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,1,@dteTime),110) + ' ' + CONVERT(varchar(8),ToTime,14),120)
			END ToTime
			,Method					-- WO#3695
				FROM tblShift
				WHERE Shift = [dbo].[fnGetExpectedShift](@chrFacility, @dteTime, @vchWorkGroup) 
					AND facility= @chrfacility AND WorkGroup = @vchWorkGroup
					AND (@bitDisableDisplayable = 0 OR (@bitDisableDisplayable = 1 AND Displayable = 1))
		-- WO#2645 ADD Stop
	END 
	ELSE
	IF @vchAction = 'ShiftTime'
	BEGIN
		SELECT Shift, Description,
			ShiftSequence,			-- WO#194 
			CASE WHEN FromTime < ToTime OR (FromTime > ToTime AND (CONVERT(varchar(8),@dteTime,14) > CONVERT(varchar(8),ToTime,14)))
			 THEN CONVERT(datetime,CONVERT(varchar(10),@dteTime,110) + ' ' + CONVERT(varchar(8),FromTime,14),120) 
			 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,-1,@dteTime),110) + ' ' + CONVERT(varchar(8),FromTime,14),120) 
		END FromTime,
		CASE WHEN FromTime < ToTime OR (FromTime > ToTime AND (CONVERT(varchar(8),@dteTime,14) <= CONVERT(varchar(8),ToTime,14)))
			 THEN CONVERT(datetime,CONVERT(varchar(10),@dteTime,110) + ' ' + CONVERT(varchar(8),ToTime,14),120) 
			 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,1,@dteTime),110) + ' ' + CONVERT(varchar(8),ToTime,14),120) 
		END ToTime
		,Method					-- WO#3695
			FROM dbo.tblShift 
			WHERE Facility = @chrFacility and WorkGroup = @vchWorkGroup and Shift = @intShift
			AND (@bitDisableDisplayable = 0 or (@bitDisableDisplayable = 1 and Displayable = 1))	-- WO#2645
	END
	ELSE
		IF @vchAction = 'AllShifts'
		BEGIN
			SELECT Shift, Description,
				ShiftSequence,			-- WO#194
				CASE WHEN FromTime < ToTime OR (FromTime > ToTime AND (CONVERT(varchar(8),@dteTime,14) > CONVERT(varchar(8),ToTime,14)))
				 THEN CONVERT(datetime,CONVERT(varchar(10),@dteTime,110) + ' ' + CONVERT(varchar(8),FromTime,14),120) 
				 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,-1,@dteTime),110) + ' ' + CONVERT(varchar(8),FromTime,14),120) 
			END FromTime,
			CASE WHEN FromTime < ToTime OR (FromTime > ToTime AND (CONVERT(varchar(8),@dteTime,14) <= CONVERT(varchar(8),ToTime,14)))
				 THEN CONVERT(datetime,CONVERT(varchar(10),@dteTime,110) + ' ' + CONVERT(varchar(8),ToTime,14),120) 
				 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,1,@dteTime),110) + ' ' + CONVERT(varchar(8),ToTime,14),120) 
			END ToTime
			,Method					-- WO#3695
				FROM dbo.tblShift 
				WHERE Facility = @chrFacility and WorkGroup = @vchWorkGroup
					AND (@bitDisableDisplayable = 0 or (@bitDisableDisplayable = 1 and Displayable = 1))	-- WO#2645
			ORDER BY ShiftSequence
		END
-- WO#359 Add Start
		ELSE
		IF @vchAction = 'AllShiftsWithBlank'
		BEGIN
			SELECT Shift, Description,
				ShiftSequence,
				CASE WHEN FromTime < ToTime OR (FromTime > ToTime AND (CONVERT(varchar(8),@dteTime,14) > CONVERT(varchar(8),ToTime,14)))
				 THEN CONVERT(datetime,CONVERT(varchar(10),@dteTime,110) + ' ' + CONVERT(varchar(8),FromTime,14),120) 
				 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,-1,@dteTime),110) + ' ' + CONVERT(varchar(8),FromTime,14),120) 
			END FromTime,
			CASE WHEN FromTime < ToTime OR (FromTime > ToTime AND (CONVERT(varchar(8),@dteTime,14) <= CONVERT(varchar(8),ToTime,14)))
				 THEN CONVERT(datetime,CONVERT(varchar(10),@dteTime,110) + ' ' + CONVERT(varchar(8),ToTime,14),120) 
				 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,1,@dteTime),110) + ' ' + CONVERT(varchar(8),ToTime,14),120) 
			END ToTime
			,Method					-- WO#3695
				FROM dbo.tblShift 
				WHERE Facility = @chrFacility and WorkGroup = @vchWorkGroup
						AND (@bitDisableDisplayable = 0 or (@bitDisableDisplayable = 1 and Displayable = 1))	-- WO#2645
			UNION
			SELECT NULL, '*ALL', NULL, NULL, NULL
			,NULL					-- WO#3695
			ORDER BY ShiftSequence
		END
		ELSE
			IF @vchAction = 'AllShiftsWithZero'
			BEGIN
				SELECT Shift, Description,
					ShiftSequence,
					CASE WHEN FromTime < ToTime OR (FromTime > ToTime AND (CONVERT(varchar(8),@dteTime,14) > CONVERT(varchar(8),ToTime,14)))
					 THEN CONVERT(datetime,CONVERT(varchar(10),@dteTime,110) + ' ' + CONVERT(varchar(8),FromTime,14),120) 
					 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,-1,@dteTime),110) + ' ' + CONVERT(varchar(8),FromTime,14),120) 
				END FromTime,
				CASE WHEN FromTime < ToTime OR (FromTime > ToTime AND (CONVERT(varchar(8),@dteTime,14) <= CONVERT(varchar(8),ToTime,14)))
					 THEN CONVERT(datetime,CONVERT(varchar(10),@dteTime,110) + ' ' + CONVERT(varchar(8),ToTime,14),120) 
					 ELSE CONVERT(datetime,CONVERT(varchar(10),DATEADD(day,1,@dteTime),110) + ' ' + CONVERT(varchar(8),ToTime,14),120) 
				END ToTime
				,Method					-- WO#3695
					FROM dbo.tblShift 
					WHERE Facility = @chrFacility and WorkGroup = @vchWorkGroup
						AND (@bitDisableDisplayable = 0 or (@bitDisableDisplayable = 1 and Displayable = 1))	-- WO#2645
				UNION
				SELECT 0, '*ALL', NULL, NULL, NULL
				,NULL					-- WO#3695
				ORDER BY ShiftSequence
			END
-- WO#359 Add Stop
END

GO

