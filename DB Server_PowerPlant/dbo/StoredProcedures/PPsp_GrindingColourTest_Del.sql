
-- =============================================
-- Author:		Bong Lee
-- Create date: April 15, 2008
-- Description:	Delete Grinding Colour Test Data
-- =============================================

CREATE PROCEDURE [dbo].[PPsp_GrindingColourTest_Del]
(
	@Original_RelativeRecNo int,
	@Original_GrindJobID int,
	@Original_Colour decimal(5, 2),
	@Original_Operator varchar(50),
	@Original_Inactive bit,
	@IsNull_Shift tinyint,
	@Original_Shift tinyint
)
AS
SET NOCOUNT OFF;
	DELETE FROM [tblGrindData] WHERE (([RelativeRecNo] = @Original_RelativeRecNo) AND ([GrindJobID] = @Original_GrindJobID) AND ([Colour] = @Original_Colour) AND ([Operator] = @Original_Operator) AND ([Inactive] = @Original_Inactive) AND ((@IsNull_Shift = 1 AND [Shift] IS NULL) OR ([Shift] = @Original_Shift)))

GO

