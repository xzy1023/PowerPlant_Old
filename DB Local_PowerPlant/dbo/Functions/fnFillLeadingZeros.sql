
-- =============================================
-- Author:		Bong Lee
-- Create date: Feb. 9 2006
-- Description:	Fill in Leading Zeros to a string
-- =============================================
CREATE FUNCTION [dbo].[fnFillLeadingZeros] 
	(
	-- Total length of the output string
	@intPrmReqLen smallint = 0,
	-- The string to be filled with leading zeros
	@strPrmString nVarChar (10) 
	)
RETURNS nVarChar(10)
AS
	BEGIN
		DECLARE @strWkField nVarChar(10) 
		DECLARE @intActualLen smallint
		DECLARE @intStartAt smallint
		
		SET @intActualLen = LEN(@strPrmString)
		SET @strWkField = REPLICATE('0', @intPrmReqLen)
		SET @intStartAt = @intPrmReqLen - @intActualLen + 1	
		IF @intActualLen = 0 or @intStartAt = 0 
			SET @strWkField = @strPrmString 
		ELSE
			SET @strWkField = STUFF(@strWkField, @intStartAt, @intActualLen, @strPrmString)
		RETURN @strWkField 
	END

GO

