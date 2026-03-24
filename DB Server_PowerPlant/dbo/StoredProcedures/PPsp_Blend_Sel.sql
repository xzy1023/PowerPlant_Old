

-- =============================================
-- Author:		Bong Lee
-- Create date: Jan 28, 2008
-- Description:	Select Grind based on the different criteria
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_Blend_Sel] 
	@vchAction varchar(30) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	IF	@vchAction = 'BlankForAll'
		SELECT '' as Blend, 'All' as Description 
			Union
		SELECT Distinct Blend, Blend as Description FROM tblRoastColourSpec ORDER BY Blend
	IF	@vchAction = 'NullForAll'
		SELECT NULL as Blend, 'All' as Description 
			Union
		SELECT Distinct Blend, Blend as Description FROM tblRoastColourSpec ORDER BY Blend
	IF	@vchAction = 'withBlank'
		SELECT '' as Blend, '' as Description 
			Union
		SELECT Distinct Blend, Blend as Description FROM tblRoastColourSpec ORDER BY Blend
	ELSE
		SELECT Distinct Blend FROM tblRoastColourSpec ORDER BY Blend
END

GO

