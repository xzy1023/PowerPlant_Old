CREATE VIEW [dbo].[@del_vwLastestCurrentDarkeningFactor]
AS
SELECT     TOP (100) PERCENT F1.Blend, F1.Grind, F1.DarkeningFactor
FROM         dbo.tblDarkeningFactor AS F1 INNER JOIN
                          (SELECT     MAX(EffectiveDate) AS EffectiveAt, Blend, Grind
                            FROM          dbo.tblDarkeningFactor
                            GROUP BY Blend, Grind) AS F2 ON F1.EffectiveDate = F2.EffectiveAt AND F1.Blend = F2.Blend AND F1.Grind = F2.Grind
ORDER BY F2.Blend, F2.Grind

GO

