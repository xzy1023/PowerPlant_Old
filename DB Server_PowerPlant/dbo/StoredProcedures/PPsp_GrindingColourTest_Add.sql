

-- =============================================
-- Author:		Bong Lee
-- Create date: April 15, 2008
-- Description:	Insert Grinding Colour Test Data 
-- =============================================

CREATE PROCEDURE [dbo].[PPsp_GrindingColourTest_Add]
(
	@GrindJobID int,
	@Colour decimal(5, 2),
	@Operator varchar(50),
	@Inactive bit,
	@Shift tinyint
)
AS
	SET NOCOUNT OFF;
	INSERT INTO [tblGrindData] ([GrindJobID], [Colour], [Operator], [Inactive], [Shift]) VALUES (@GrindJobID, @Colour, @Operator, @Inactive, @Shift);
		
	SELECT RelativeRecNo, GrindJobID, Colour, Operator, Inactive, Shift FROM tblGrindData WHERE (RelativeRecNo = SCOPE_IDENTITY())

GO

