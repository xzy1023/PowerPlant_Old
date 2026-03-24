
-- =============================================
-- Author:		<Bong Lee>
-- Create date: <10/5/2007>
-- Description:	Update Roasting Colour Specifications 
-- =============================================

Create PROCEDURE [dbo].[PPsp_AddRoastColourSpecs] 
	-- Add the parameters for the stored procedure here
	@chrBlend nchar(6),
	@SpecMin decimal(5,2),
	@SpecTarg decimal(5,2),
	@SpecMax decimal(5,2),
	@dteEffectiveDate datetime = NULL
AS
BEGIN
	DECLARE @dteToday DATETIME
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON 

	SET @dteToday = GETDATE()
	if @dteEffectiveDate IS NULL
		Set @dteEffectiveDate = @dteToday

    INSERT INTO tblRoastColourSpec
		(EffectiveDate, Blend,  SpecMin, SpecMax, SpecTarg, CreationDate)
	VALUES     (@dteEffectiveDate, @chrBlend, @SpecMin, @SpecMax, @SpecTarg, @dteToday)

END

GO

