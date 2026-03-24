


-- =====================================================================
-- Author:		Bong Lee
-- Create date: Dec. 29, 2005
-- Description:	Calculate Darkening Factor Chart Data 
-- =====================================================================

CREATE PROCEDURE [dbo].[PPsp_MonthlyDarkeningFactorChart]
	-- Add the parameters for the stored procedure here
	@dteFmDate Datetime = null,
	@dteToDate Datetime = null,
	@chrBlend nchar(6) = '',
	@chrGrind nchar(6) = '',
	@chrExclusion varchar(200) = ''
AS
BEGIN
	DECLARE @dteFmDateMDCY DATETIME
	DECLARE @dteToDateMDCY DATETIME
	DECLARE @CurrentDF DECIMAL(3,1)
	DECLARE @MaxDiff DECIMAL(4,2)
	DECLARE @MinDiff DECIMAL(4,2)
	DECLARE @chrSQLStmt VARCHAR(512)
	DECLARE @bitBarBlend BIT

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	-- Convert the input date formats are MMDDCCYY to ensure the date format
	SET @dteFmDateMDCY = CONVERT(DATETIME,@dteFmDate,102)
	-- set the ToDate to the 11:59:59 PM of the date.
	SET @dteToDateMDCY = DATEADD(SECOND,-1,DATEADD(DAY,1,CONVERT(DATETIME,@dteToDate,102)))
	
	-- Finished goods colour information
	SELECT	YEAR(F1.DateTest) * 100 + MONTH(F1.DateTest) AS TestMonth, ROUND(AVG(Colour), 2) AS Colour,
			Avg((SpecMax - SpecTarg)) AS MaxDiff, Avg((SpecTarg - SpecMin)) AS MinDiff,
			Avg(DarkeningFactor) as CurrentDF
	INTO    #FinsihedGoodsColour
	FROM    tblFGColourLog AS F1
		LEFT OUTER JOIN  vwLatestFGColourSpec F2
	ON		(F1.Blend = F2.BLend) AND (F1.Grind = F2.Grind)
		LEFT OUTER JOIN vwLatestCurrentDarkeningFactor F3
	ON		(F1.Blend = F3.BLend) AND (F1.Grind = F3.Grind)
	WHERE	(F1.DateTest BETWEEN @dteFmDateMDCY AND @dteToDateMDCY) AND (F1.Blend = @chrBlend) AND (F1.Grind = @chrGrind)
	GROUP BY YEAR(F1.DateTest) * 100 + MONTH(F1.DateTest)
	ORDER BY TestMonth

	-- Write average roasting colour by month and blend into a local temporary file,
	-- #RoastingLogColour. (Including barblends)
	
	IF NOT EXISTS (SELECT * FROM tblBarBlend WHERE BarBlend = @chrBlend) 
	BEGIN
		SET		@bitBarBlend = 0
	-- Get non-BarBlend average roasting colour by month
		SELECT	YEAR(DateTest) * 100 + MONTH(DateTest) AS TestMonth, 
				ROUND(AVG(Colour), 2) AS RoastingColour,
				ROUND(AVG(SpecMax), 2) AS RoastingSpecMax,
				ROUND(AVG(SpecTarget), 2) AS RoastingSpecTarget,
				ROUND(AVG(SpecMin), 2) AS RoastingSpecMin
		INTO    #NormalBlends
		FROM    tblRoastingLog
		WHERE   (DateTest BETWEEN @dteFmDateMDCY AND @dteToDateMDCY)
					AND (Blend = @chrBlend) 
					AND (Rejected <> N'Y' OR Rejected IS NULL)
		GROUP BY YEAR(DateTest) * 100 + MONTH(DateTest)
		ORDER BY TestMonth
	
		-- Join Finished goods and Roasting colour info. to get result set
		SET @chrSQLStmt = 'Select * from #FinsihedGoodsColour F1 INNER JOIN #NormalBlends F2 ON	F1.TestMonth = F2.TestMonth'
	END
	ELSE

	-- Get BarBlend average roasting colour by month
	BEGIN
		SET		@bitBarBlend = 1
		(SELECT F4.TestMonth,
				ROUND(SUM(F4.Colour * F3.Percentage),2) AS RoastingColour,   
				ROUND(SUM(F4.SpecMax * F3.Percentage),2) AS RoastingSpecMax,
				ROUND(SUM(F4.SpecTarget * F3.Percentage),2) AS RoastingSpecTarget,
				ROUND(SUM(F4.SpecMin * F3.Percentage),2) AS RoastingSpecMin
		 INTO   #BarBlends
		 FROM
         (SELECT	YEAR(F1.DateTest) * 100 + MONTH(F1.DateTest) AS TestMonth,
					F2.BarBlend, F2.Blend,
					AVG(F1.Colour) AS Colour,
					AVG(F1.SpecMax) AS SpecMax,
					AVG(F1.SpecTarget) AS SpecTarget,
					AVG(F1.SpecMin) AS SpecMin
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
		
		-- Join Finished goods and Roasting colour info. to get result set
		SET @chrSQLStmt = 'SELECT F1.TestMonth,  F1.Colour, F1.MaxDiff, F1.MinDiff, F1.CurrentDF, F2.RoastingColour, F2.RoastingSpecMax, F2.RoastingSpecTarget, F2.RoastingSpecMin 
							FROM #FinsihedGoodsColour F1 INNER JOIN #BarBlends F2 ON F1.TestMonth = F2.TestMonth'
	END
					
	IF (@chrExclusion <> '')
	BEGIN
		SET @chrSQLStmt = RTRIM(@chrSQLStmt) + ' WHERE F1.TestMonth NOT IN (' +  RTRIM(@chrExclusion) + ')'	
	END

	EXEC (@chrSQLStmt)		

END

GO

