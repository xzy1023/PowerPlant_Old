
-- =============================================
-- Author:		Bong Lee
-- Create date: Oct 21,2008
-- Description:	Get Expected Work Shift
-- WO#2645:		Oct. 29, 2015	Bong Lee
--				Do not use record cursor to improve performance
--				and simplier the code
-- WO#3695:		Nov. 29, 2016	Bong Lee
--				Impplement 4 shifts pattern. The shift no. is also depended on 
--				the shift pattern.
-- =============================================
CREATE FUNCTION [dbo].[fnGetExpectedShift] 
(
	@chrFacility char(3), 
	@dteTime datetime,
	@vchWorkGroup varchar(10)
)
RETURNS int
AS
BEGIN
	-- WO#2645 DEL Start
	---- Declare the return variable here
	--DECLARE  @dteFmTime as datetime
	--DECLARE  @dteToTime as datetime
	--DECLARE  @intShiftNo as int
	--DECLARE  @vchDescription as varchar(50)
	--DECLARE  @vchInputTime as varchar(8)
	---- SET NOCOUNT ON added to prevent extra result sets from
	---- interfering with SELECT statements.

	--SET @vchInputTime = CONVERT(varchar(8),@dteTime,14)

	--	DECLARE object_cursor CURSOR FOR
	--	SELECT Shift,FromTime,ToTime,Description
	--			FROM dbo.tblShift 
	--			WHERE Facility = @chrFacility and WorkGroup = @vchWorkGroup
	--			ORDER BY Shift
	--	OPEN object_cursor

	--	FETCH NEXT FROM object_cursor INTO @intShiftNo,@dteFmTime,@dteToTime,@vchDescription

	--	WHILE @@FETCH_STATUS = 0
	--	BEGIN
	--		IF @dteFmTime < @dteToTime
	--		BEGIN
	--			IF @vchInputTime >= CONVERT(varchar(8),@dteFmTime,14) AND
	--			   @vchInputTime <= CONVERT(varchar(8),@dteToTime,14)	
	--			Break
	--		END
	--		ELSE
	--		BEGIN
	--			IF (@vchInputTime >= CONVERT(varchar(8),@dteFmTime,14)) OR
	--			   (@vchInputTime <= CONVERT(varchar(8),@dteToTime,14))
	--			Break
	--		END
	--		FETCH NEXT FROM object_cursor INTO @intShiftNo,@dteFmTime,@dteToTime,@vchDescription
	--	END

	--	CLOSE object_cursor
	--	DEALLOCATE object_cursor
	-- WO#2645 DEL Stop
	-- WO#2645 ADD Start
	DECLARE  @intShiftNo as int
	DECLARE @intShiftPatternCode as int;																	-- WO#3695

	Select @intShiftPatternCode = dbo.fnGetShiftPatternCode(@chrFacility, @dteTime, @vchWorkGroup);			-- WO#3695

	SELECT @intShiftNO = Case WHEN Displayable = 0 THEN 
							Case WHEN [Shift] - 1 = 0 THEN [ShiftsPerDay] ELSE [Shift] - 1 END
						 ELSE [SHIFT] END 
	FROM tblShift 
	WHERE Facility = @chrFacility AND workgroup = @vchWorkGroup
	 AND (
	 (FromTime <= ToTime AND CONVERT(varchar(8),@dteTime,14) BETWEEN CONVERT(varchar(8),FromTime,14) AND CONVERT(varchar(8),ToTime,14))
	 OR
	 (FromTime > ToTime AND (CONVERT(varchar(8),@dteTime,14) >= CONVERT(varchar(8),FromTime,14) OR CONVERT(varchar(8),@dteTime,14) <= CONVERT(varchar(8),ToTime,14)))
	 )
	 AND ShiftPatternCode = @intShiftPatternCode															-- WO#3695

	-- WO#2645 ADD Stop
	-- Return the result of the function
	RETURN @intShiftNo

END

GO

