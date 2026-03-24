CREATE VIEW [dbo].[vwLatestFGColourSpec] AS SELECT TOP (100) PERCENT F1.Blend, F1.Grind, F1.SpecMin, F1.SpecMax, F1.SpecTarg, F1.Active FROM dbo.tblFGColourSpec AS F1 INNER JOIN (SELECT Blend, Grind, MAX(EffectiveDate) AS EffectiveAt FROM dbo.tblFGColourSpec AS F2 GROUP BY Blend, Grind) AS F3 ON F1.EffectiveDate = F3.EffectiveAt AND F1.Blend = F3.Blend AND F1.Grind = F3.Grind ORDER BY F1.Blend, F1.Grind

GO

