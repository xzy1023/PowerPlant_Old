
-- =============================================
-- Author:		Bong Lee
-- Create date: April 15, 2008
-- Description:	Update Grinding Colour Test Data
-- =============================================

CREATE PROCEDURE [dbo].[PPsp_GrindingColourTest_Upd]
(
	@GrindJobID int,
	@Colour decimal(5, 2),
	@Operator varchar(50),
	@Inactive bit,
	@Shift tinyint,
	@Original_RelativeRecNo int,
	@Original_GrindJobID int,
	@Original_Colour decimal(5, 2),
	@Original_Operator varchar(50),
	@Original_Inactive bit,
	@IsNull_Shift tinyint,
	@Original_Shift tinyint,
	@RelativeRecNo int
)
AS
SET NOCOUNT OFF;
UPDATE [tblGrindData] SET [GrindJobID] = @GrindJobID, [Colour] = @Colour, [Operator] = @Operator, [Inactive] = @Inactive, [Shift] = @Shift WHERE (([RelativeRecNo] = @Original_RelativeRecNo) AND ([GrindJobID] = @Original_GrindJobID) AND ([Colour] = @Original_Colour) AND ([Operator] = @Original_Operator) AND ([Inactive] = @Original_Inactive) AND ((@IsNull_Shift = 1 AND [Shift] IS NULL) OR ([Shift] = @Original_Shift)));
	
SELECT RelativeRecNo, GrindJobID, Colour, Operator, Inactive, Shift FROM tblGrindData WHERE (RelativeRecNo = @RelativeRecNo)

GO

