
-- =============================================
-- Author:		Bong Lee
-- Create date: April 15, 2008
-- Description:	Select Grinding Colour Test Data
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_GrindingColourTest_Sel]
(
	@vchAction as varchar(30),
	@intGrindJobID int = 0,
	@RelativeRecNo int = 0
)
AS
SET NOCOUNT ON;
	IF @vchAction = 'ByRRN'
	BEGIN
		SELECT     RelativeRecNo, GrindJobID, Colour, Operator, Inactive, Shift
				FROM         tblGrindData
				WHERE     ( RelativeRecNo = @RelativeRecNo)
	END
	ELSE
	IF @vchAction = 'ByGrindJobID'
	BEGIN
		SELECT     RelativeRecNo, GrindJobID, Colour, Operator, Inactive, Shift
		FROM         tblGrindData
		WHERE     (GrindJobID = @intGrindJobID)
	END

GO

