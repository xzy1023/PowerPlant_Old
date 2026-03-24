



-- =============================================
-- Author:		<Bong Lee>
-- Create date: <10/5/2007>
-- Description:	Add Darkening Factor to Specifications 
-- =============================================

CREATE PROCEDURE [dbo].[PPsp_AddDarkeningFactor] 
	-- Add the parameters for the stored procedure here
	@chrBlend char(6),
	@chrGrind char(6),
	@decDarkeningFactor decimal(4,2),
	@varCreatedBy varchar(50),
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

    INSERT INTO dbo.tblDarkeningFactor
		(EffectiveDate, Blend, Grind, DarkeningFactor, CreationDate, CreatedBy)
	VALUES     (@dteEffectiveDate, @chrBlend, @chrGrind, @decDarkeningFactor, @dteToday, @varCreatedBy)

END

GO

