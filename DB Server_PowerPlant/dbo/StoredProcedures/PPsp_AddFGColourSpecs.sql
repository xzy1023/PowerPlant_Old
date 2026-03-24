



-- =============================================
-- Author:		Bong Lee
-- Create date: 9/14/2006
-- Description:	Update Darkening Factor and Finshed Goods Colour Spec.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_AddFGColourSpecs]
	-- Add the parameters for the stored procedure here
	@chrBlend char(6), 
	@chrGrind char(6) = '',
	@SpecMin decimal(5,2),
	@SpecTarg decimal(5,2),
	@SpecMax decimal(5,2),
	@varCreatedBy varchar(50),
	@dteEffectiveDate datetime = NULL
AS
BEGIN
	DECLARE @dteToday DATETIME
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

    SET @dteToday = GETDATE()
	IF @dteEffectiveDate IS NULL
		Set @dteEffectiveDate = @dteToday

	INSERT INTO tblFGColourSpec
	  (EffectiveDate, Blend, Grind, SpecMin, SpecMax, SpecTarg, CreationDate, CreatedBy)
	VALUES     (@dteEffectiveDate, @chrBlend,@chrGrind, @SpecMin, @SpecMax, @SpecTarg, @dteToday, @varCreatedBy)

END

GO

