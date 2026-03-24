-- =============================================
-- Author:		Bong Lee
-- Create date: Oct. 05, 2016
-- Description:	Get Shift Pattern Color Code by Date
-- =============================================
CREATE FUNCTION [dbo].[fnGetShiftPatternCode] 
(
	@chrFacility char(3)
	,@dteGivenDate datetime
	,@vchWorkGroup varchar(10)
)
RETURNS int
AS
BEGIN
	Declare @dteRefDate as datetime
	Declare @intRemainingDays as int
	Declare @intShiftPatternCode as Tinyint
	Declare @intNoOfWeeks as int

	SELECT TOP 1 @intShiftPatternCode = ISNULL(ShiftPatternCode,0)
		FROM tblShift
		WHERE Facility = @chrFacility AND WorkGroup = @vchWorkGroup

	IF @intShiftPatternCode > 0
	BEGIN  
		SELECT TOP 1 @dteGivenDate = CASE WHEN CONVERT(char,@dteGivenDate,14) <= CONVERT(char,ToTime,14) THEN Dateadd(day, -1 , @dteGivenDate) ELSE  @dteGivenDate END
		FROM tblShift
		WHERE Facility = @chrFacility AND WorkGroup = @vchWorkGroup AND FromTime > ToTime

		Set @dteRefDate = '12-30-2004';  
		-- First pattern is 2004/12/31 - 2005/1/2(3 days) are green; 2005/1/03 - 1/4 (2 days) are pink;  2005/1/05 - 1/06(2 days) are green
		--					2005/01/07 - 2005/1/9(3 days) are pink;  2005/1/10 - 1/11(2 days) are green; 2005/1/12 - 1/13(2 days) are pink
		-- The subsequance days are repeating the above pattern. 

		select @intNoOfWeeks = FLOOR(datediff(d, @dteRefDate,@dteGivenDate) / 7)

		--'If the result is odd number, the first date is green else pink
		select @intRemainingDays = FLOOR(datediff(d, @dteRefDate, @dteGivenDate) % 7) 

		IF @intNoOfWeeks % 2 = 0
		BEGIN
	
			Select @intShiftPatternCode = Case 
				WHEN @intRemainingDays = 0 THEN 2		--'Pink' 
				WHEN @intRemainingDays < 4 OR @intRemainingDays > 5 THEN 1 ELSE 2 END
		END
		ELSE
		BEGIN
			Select @intShiftPatternCode = Case
				WHEN @intRemainingDays = 0 THEN 1		--'Green' 
				WHEN @intRemainingDays < 4 OR @intRemainingDays > 5 THEN 2 ELSE 1 END
		END
	END

	-- Results: if 1 equals to Green on user's calendar ; 2 equals to Pink.
	RETURN  @intShiftPatternCode

END

GO

