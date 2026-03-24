
-- =============================================
-- Author:		Bong Lee
-- Create date: Jun 12, 2014
-- Description:	Select Date Format Code
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_DateFormatCode_Sel] 
	@vchAction as varchar(30) = NULL
	,@vchFacility varchar(3)
	,@bitActive bit = NULL
AS
BEGIN
	SET NOCOUNT ON;
	IF @vchAction = 'LookUp'
		SELECT DateCode
			,Rtrim(DateCode) + ' - ' + RTrim(DateFormat) + ' - ' + ISNULL([dbo].[fnConvertDate](getdate(),Datecode, @vchFacility, 1), 'N/A') as FmtDescription
		FROM tbldatefmtcode 
		WHERE (Active = @bitActive or @bitActive is NULL)
		Union
		SELECT '  ', 'Please select ...';
	ELSE
		SELECT *
		FROM tbldatefmtcode 
		WHERE (Active = @bitActive or @bitActive is NULL);

END

GO

