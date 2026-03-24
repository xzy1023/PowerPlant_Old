
-- =============================================
-- Author:		Bong Lee
-- Create date: Oct. 4 2006
-- Description:	Convert Numeric time to DateTime
-- =============================================
CREATE FUNCTION [dbo].[fnCvtNumTimeToDateTime]
(
	-- Add the parameters for the function here
	@intTime int
)
RETURNS datetime
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result datetime;
	DECLARE @chrTime Char(6);
	SET @chrTime = dbo.fnFillLeadingZeros(6,@intTime)
	
	SELECT @Result = SUBSTRING(@chrTime,1,2) + ':' +
					 SUBSTRING(@chrTime,3,2) + ':' +
					 SUBSTRING(@chrTime,5,2) 

	-- Return the result of the function
	RETURN @Result

END

GO

