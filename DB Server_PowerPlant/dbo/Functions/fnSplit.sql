
-- =============================================
-- Author:		Bong Lee
-- Create date: Nov., 14, 2008
-- Description:	It takes a comma-delimited string and splits it into a recordset representation.
--				This functionality is similar to the Split function in various programming languages,
--				which splits a delimited string into a string array
-- =============================================
CREATE FUNCTION [dbo].[fnSplit] 
(	
	@vchList varchar(2000),
	@chrSplitOn char(1)
)
RETURNS @RtnValue TABLE 
(
	Id int identity(1,1),
	Value varchar(100)
)
AS
BEGIN
	While (Charindex(@chrSplitOn,@vchList)>0)
	BEGIN
		Insert Into @RtnValue (value)
		Select Value = ltrim(rtrim(Substring(@vchList,1,Charindex(@chrSplitOn,@vchList)-1)))
		Set @vchList = Substring(@vchList,Charindex(@chrSplitOn,@vchList)+len(@chrSplitOn),len(@vchList))
	END
	Insert Into @RtnValue (Value)
    Select Value = ltrim(rtrim(@vchList))
    Return
END

GO

