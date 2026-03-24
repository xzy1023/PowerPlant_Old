






-- =============================================
-- Author:		Bong Lee
-- Create date: Feb 06, 2008
-- Description:	Select Grind 
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_Grind_Sel] 
	@vchAction varchar(30) = NULL,
	@chrFacility char(3) = NULL,
	@vchBlend varchar(6),
	@vchGrinder varchar(6) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	IF @vchAction = 'ByBlend'
			SELECT Grind, CASE Grind WHEN '' THEN 'WB' ELSE Grind End as Descripton FROM vwLatestFGColourSpec 
			WHERE Blend = @vchBlend
			ORDER BY Grind
	ELse	-- @vchAction = 'ByGrinderBlend'
		SELECT T1.Grind ,CASE T1.Grind WHEN '' THEN 'WB' ELSE T1.Grind End as Descripton FROM vwLatestFGColourSpec T1 
		INNER JOIN tblGrinderRate T2 
		ON T1.Grind = T2.Grind 
		WHERE (T1.Blend = @vchBlend) AND (T2.Facility = @chrFacility) 
		AND (T2.Grinder = @vchGrinder) ORDER BY T1.Grind
		
END

GO

