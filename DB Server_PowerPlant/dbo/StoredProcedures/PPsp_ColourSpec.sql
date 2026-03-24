
-- =============================================
-- Author:		Bong Lee
-- Create date: Sep. 22 2008
-- Description:	Select Latest Colour specification
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ColourSpec]
	@bitActiveSpec bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF @bitActiveSpec = 1
				SELECT   v1.Blend, v2.Grind, v3.SpecMin AS RstSpecMin, v3.SpecTarg AS RstSpecTarg, v3.SpecMax AS RstSpecMax, v2.DarkeningFactor, 
				 v1.SpecMin AS FGSpecMin, v1.SpecTarg AS FGSpecTarg, v1.SpecMax AS FGSpecMax
			FROM         vwLatestFGColourSpec AS v1
			LEFT OUTER JOIN		  vwLatestCurrentDarkeningFactor AS v2 
				ON v1.Blend = v2.Blend AND v1.Grind = v2.Grind 
			LEFT OUTER JOIN	  vwLatestRoastColourSpec AS v3 
				ON v1.Blend = v3.Blend 
		WHERE     (v1.Active = 1)
		ORDER BY v1.Blend, v1.Grind
	Else
		SELECT   v1.Blend, v2.Grind, v3.SpecMin AS RstSpecMin, v3.SpecTarg AS RstSpecTarg, v3.SpecMax AS RstSpecMax, v2.DarkeningFactor, 
				 v1.SpecMin AS FGSpecMin, v1.SpecTarg AS FGSpecTarg, v1.SpecMax AS FGSpecMax
			FROM         vwLatestFGColourSpec AS v1
			LEFT OUTER JOIN		  vwLatestCurrentDarkeningFactor AS v2 
				ON v1.Blend = v2.Blend AND v1.Grind = v2.Grind 
			LEFT OUTER JOIN	  vwLatestRoastColourSpec AS v3 
				ON v1.Blend = v3.Blend 
			ORDER BY v1.Blend, v1.Grind
END

GO

