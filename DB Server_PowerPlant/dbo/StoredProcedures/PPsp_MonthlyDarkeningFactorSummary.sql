


-- =====================================================================
-- Author:		Bong Lee
-- Create date: Dec. 29, 2005
-- Description:	Calculate Darkening Factor Summary by Month 
-- =====================================================================

CREATE PROCEDURE [dbo].[PPsp_MonthlyDarkeningFactorSummary]
	-- Add the parameters for the stored procedure here
	@dteFmDate Datetime = null,
	@dteToDate Datetime = null,
	@chrBlend nchar(6) = '',
	@chrGrind nchar(6) ='' 
AS
BEGIN
	DECLARE @dteFmDateMDCY DATETIME
	DECLARE @dteToDateMDCY DATETIME

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	-- Convert the input date formats are MMDDCCYY to ensure the date format
	SET @dteFmDateMDCY = CONVERT(DATETIME,@dteFmDate,102)
	-- set the ToDate to the 11:59:59 PM of the date.
	SET @dteToDateMDCY = DATEADD(SECOND,-1,DATEADD(DAY,1,CONVERT(DATETIME,@dteToDate,102)))
	
	-- Write average roasting colour by month and blend into a local temporary file,
	-- #RoastingLogColour. (Including barblends)
  
	-- Get non-BarBlend average roasting colour by month
	SELECT     YEAR(DateTest) * 100 + MONTH(DateTest) AS TestMonth, 
			   ROUND(AVG(Colour), 2) AS RoastingColour,
			   ROUND(AVG(SpecTarget), 2) AS RoastingTargetColour
	INTO        #RoastingLogColour
	FROM         tblRoastingLog
	WHERE     (DateTest BETWEEN @dteFmDateMDCY AND @dteToDateMDCY)
				AND (Blend = @chrBlend) 
				AND (Rejected <> N'Y' OR Rejected IS NULL)
	GROUP BY YEAR(DateTest) * 100 + MONTH(DateTest)
	
	-- Get BarBlend average roasting colour by month
	Union
	(SELECT     F4.TestMonth,
			   ROUND(SUM(F4.Expr1 * F3.Percentage),2) AS RoastingColour,   
			   ROUND(SUM(F4.Expr2 * F3.Percentage),2) AS RoastingTargetColour
		FROM
         (SELECT	YEAR(F1.DateTest) * 100 + MONTH(F1.DateTest) AS TestMonth,
					F2.BarBlend, F2.Blend,
					AVG(F1.Colour) AS Expr1,
					AVG(F1.SpecTarget) AS Expr2
			   FROM		tblRoastingLog AS F1 INNER JOIN
						tblBarBlend AS F2 
			   ON	F1.Blend = F2.Blend
			   WHERE (F1.DateTest BETWEEN @dteFmDateMDCY AND @dteToDateMDCY)
					AND (F2.barBlend = @chrBlend)
					AND (Rejected <> N'Y' OR Rejected IS NULL)
			   GROUP BY YEAR(F1.DateTest) * 100 + MONTH(F1.DateTest), F2.BarBlend, F2.Blend) AS F4 
		INNER JOIN	tblBarBlend AS F3 ON F4.BarBlend = F3.BarBlend AND F4.Blend = F3.Blend
	GROUP BY F4.TestMonth)
	ORDER BY TestMonth

	-- Write average finished goods colour by month and blend into a local temporary file,
	-- #FinishedGoodsLogColour.
	SELECT	YEAR(F1.DateTest) * 100 + MONTH(F1.DateTest) as TestMonth,
			ROUND(AVG(F1.Colour), 2) AS FGColour,
			ROUND(AVG(F2.SpecTarg), 2) AS FGTargetColour,
			AVG(F3.DarkeningFactor) AS DarkeningFactor 
	INTO	#FinishedGoodsLogColour
	FROM	tblFGColourLog F1 
		LEFT OUTER JOIN dbo.vwLatestFGColourSpec AS F2 
	ON F1.Blend = F2.Blend AND F1.Grind = F2.Grind	
		LEFT OUTER JOIN	dbo.vwLatestCurrentDarkeningFactor AS F3
	ON F1.Blend = F3.Blend AND F1.Grind = F3.Grind		
	WHERE     (F1.DateTest BETWEEN @dteFmDateMDCY AND @dteToDateMDCY)
				AND F1.Blend = @chrBlend and F1.Grind = @chrGrind
	GROUP BY YEAR(F1.DateTest) * 100 + MONTH(F1.DateTest)
	ORDER BY TestMonth

	-- Join 2 temporary files to get the result set
	SELECT F1.TestMonth, F2.RoastingColour, F2.RoastingTargetColour, F1.FGColour,
			F1.FGTargetColour, F1.DarkeningFactor AS CurDF,
			(F1.FGColour - F2.RoastingColour) AS CalDF,
			F1.DarkeningFactor - (F1.FGColour - F2.RoastingColour) AS DFDiff	
	FROM         #FinishedGoodsLogColour F1 
		LEFT OUTER JOIN	 #RoastingLogColour F2
	ON		F1.TestMonth = F2.TestMonth
	ORDER BY F1.TestMonth
END

GO

