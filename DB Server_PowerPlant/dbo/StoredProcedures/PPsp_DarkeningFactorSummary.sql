



-- =====================================================================
-- Author:		Bong Lee
-- Create date: Dec. 29, 2005
-- Description:	Calculate Darkening Factor Summary
-- =====================================================================

CREATE PROCEDURE [dbo].[PPsp_DarkeningFactorSummary]
	-- Add the parameters for the stored procedure here
	@dteFmDate Datetime,
	@dteToDate Datetime,
	@chrBlend nchar(6) = null,
	@chrGrind nchar(6) = null,
	@chrExclusion varchar(200) = null 
	 
AS
BEGIN
	DECLARE @dteFmDateCYMD DATETIME
	DECLARE @dteToDateCYMD DATETIME
	DECLARE @chrSQLStmt VARCHAR(1024)
	DECLARE @chrFmDateCYMD CHAR(20)
	DECLARE @chrToDateCYMD CHAR(20)

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	-- Convert the input date formats are MMDDCCYY to ensure the date format
	SET @dteFmDateCYMD = CONVERT(DATETIME,@dteFmDate,120)
	-- set the ToDate to the 11:59:59 PM of the date.
	SET @dteToDateCYMD = DATEADD(SECOND,-1,DATEADD(DAY,1,CONVERT(DATETIME,@dteToDate,120)))
print '@chrBlend = ' + isnull(@chrBlend,'*')
print '@chrGrind = ' + isnull(@chrGrind,'+')
	-- if Blend and grind are not passed from the calling procedure
	If @chrBlend is Null AND @chrGrind is Null
	BEGIN
print '@chrBlend'
	-- Write average roasting colour by blend into a local temporary file,#RoastingLog.  
	-- Get non-BarBlend average colour
		SELECT      Blend, ROUND(AVG(Colour),2) AS RoastingColour
		INTO        #RoastingLog
			FROM         tblRoastingLog
			WHERE     (DateTest BETWEEN @dteFmDateCYMD AND @dteToDateCYMD)
					   AND (Rejected <> N'Y' OR Rejected IS NULL)
			GROUP BY  Blend
		
		-- Get BarBlend average colour
		Union
		SELECT     F4.BarBlend as Blend, ROUND(SUM(F4.Expr1 * F3.Percentage),2) AS RoastingColour
			FROM
			 (SELECT	F2.BarBlend, F1.Blend, AVG(F1.Colour) AS Expr1
				   FROM		tblRoastingLog AS F1 INNER JOIN
							tblBarBlend AS F2 ON F1.Blend = F2.Blend
				   WHERE (F1.DateTest BETWEEN @dteFmDateCYMD AND @dteToDateCYMD)
						AND (Rejected <> N'Y' OR Rejected IS NULL)
				   GROUP BY F2.BarBlend, F1.Blend) AS F4 
			INNER JOIN	tblBarBlend AS F3 ON F4.BarBlend = F3.BarBlend AND F4.Blend = F3.Blend
		GROUP BY F4.BarBlend
		ORDER BY Blend
	
		-- Write average finished colour by blend into a local temporary file, #FinishedGoodsLog.
		SELECT     Blend, Grind, ROUND(AVG(Colour), 2) AS FGColour
		INTO	#FinishedGoodsLog
			FROM	tblFGColourLog
			WHERE     (DateTest between @dteFmDateCYMD AND @dteToDateCYMD)
			GROUP BY Blend, Grind
			ORDER BY Blend, Grind
		
		-- Join 2 temporary files to get the result set
		SELECT F1.Blend, F1.Grind, DarkeningFactor as CurDF, (FGColour - RoastingColour) as CalDF,
			DarkeningFactor - (FGColour - RoastingColour) as DFDiff,	
			F4.SpecTarg as FGColourTarget, F5.SpecTarg as RoastColourTarget	
			FROM         #FinishedGoodsLog F1 
			LEFT OUTER JOIN	 vwLatestCurrentDarkeningFactor F2
				ON		F1.Blend = F2.Blend and F1.Grind = F2.Grind
			LEFT OUTER JOIN	 #RoastingLog F3
				ON		F1.Blend = F3.Blend
			LEFT OUTER JOIN dbo.vwLatestFGColourSpec F4
				ON		F1.Blend = F4.Blend AND F1.Grind = F4.Grind
			LEFT OUTER JOIN dbo.vwLatestRoastColourSpec F5
				ON		F1.Blend = F5.Blend
			ORDER BY F1.Blend, F1.Grind
	END
	ELSE
	-- if Blend and grind are passed from the calling procedure, do below
	BEGIN
		IF @chrExclusion = '' or @chrExclusion is null
		BEGIN
			-- Write average finished colour by blend and grind into a local temporary file, #FinishedGoodsLog1.
			SELECT     Blend, Grind, ROUND(AVG(Colour), 2) AS FGColour
			INTO	#FinishedGoodsLog1
				FROM	tblFGColourLog
				WHERE     (DateTest between @dteFmDateCYMD AND @dteToDateCYMD)
						AND Blend = @chrBlend AND Grind = @chrGrind	
				GROUP BY Blend, Grind
				
			-- passed blend is a barblend?
			IF NOT EXISTS (SELECT * FROM tblBarBlend WHERE BarBlend = @chrBlend) 
			-- Get non-BarBlend average roasting colour
			BEGIN
				SELECT      Blend, ROUND(AVG(Colour),2) AS RoastingColour
					INTO        #RoastingLog1
					FROM         tblRoastingLog
					WHERE     (DateTest BETWEEN @dteFmDateCYMD AND @dteToDateCYMD)
							   AND Blend = @chrBlend
							   AND (Rejected <> N'Y' OR Rejected IS NULL)
					GROUP BY  Blend
				-- Join 2 temporary files to get the result set	
				SELECT F1.Blend, F1.Grind, DarkeningFactor as CurDF, (FGColour - RoastingColour) as CalDF,
					DarkeningFactor - (FGColour - RoastingColour) as DFDiff,	
					F4.SpecTarg as FGColourTarget, F5.SpecTarg as RoastColourTarget		
					FROM         #FinishedGoodsLog1 F1 
					LEFT OUTER JOIN	 vwLatestCurrentDarkeningFactor F2
						ON		F1.Blend = F2.Blend and F1.Grind = F2.Grind
					LEFT OUTER JOIN	 #RoastingLog1 F3
						ON		F1.Blend = F3.Blend
					LEFT OUTER JOIN dbo.vwLatestFGColourSpec F4
						ON		F1.Blend = F4.Blend AND F1.Grind = F4.Grind
					LEFT OUTER JOIN dbo.vwLatestRoastColourSpec F5
					ON		F1.Blend = F5.Blend
					ORDER BY F1.Blend, F1.Grind
			END
			ELSE
			BEGIN
				-- Get BarBlend average roasting colour
				SELECT     F4.BarBlend as Blend, ROUND(SUM(F4.Expr1 * F3.Percentage),2) AS RoastingColour
					INTO        #RoastingLog2
					FROM
					 (SELECT	F2.BarBlend, F1.Blend, AVG(F1.Colour) AS Expr1
						   FROM		tblRoastingLog AS F1 INNER JOIN
									tblBarBlend AS F2 ON F1.Blend = F2.Blend
						   WHERE (F1.DateTest BETWEEN @dteFmDateCYMD AND @dteToDateCYMD)
								AND (Rejected <> N'Y' OR Rejected IS NULL)
								AND F2.BarBlend = @chrBlend
						   GROUP BY F2.BarBlend, F1.Blend) AS F4 
					INNER JOIN	tblBarBlend AS F3 ON F4.BarBlend = F3.BarBlend AND F4.Blend = F3.Blend
				GROUP BY F4.BarBlend
				
				-- Join 2 temporary files to get the result set
				SELECT F1.Blend, F1.Grind, DarkeningFactor as CurDF, (FGColour - RoastingColour) as CalDF,
					DarkeningFactor - (FGColour - RoastingColour) as DFDiff,	
					F4.SpecTarg as FGColourTarget, F5.SpecTarg as RoastColourTarget
					FROM         #FinishedGoodsLog1 F1 
					LEFT OUTER JOIN	 vwLatestCurrentDarkeningFactor F2
						ON		F1.Blend = F2.Blend and F1.Grind = F2.Grind
					LEFT OUTER JOIN	 #RoastingLog2 F3
						ON		F1.Blend = F3.Blend
					LEFT OUTER JOIN dbo.vwLatestFGColourSpec F4
					ON		F1.Blend = F4.Blend AND F1.Grind = F4.Grind
					LEFT OUTER JOIN dbo.vwLatestRoastColourSpec F5
					ON		F1.Blend = F5.Blend
					ORDER BY F1.Blend, F1.Grind
			END
		END
		ELSE
		BEGIN
			SET @chrFmDateCYMD = CONVERT(CHAR(20),@dteFmDateCYMD,120)
			SET @chrToDateCYMD = CONVERT(CHAR(20),@dteToDateCYMD,120)

			CREATE TABLE [dbo].[#FinishedGoodsLog2](
				[Blend] [nchar](6) NOT NULL DEFAULT (('')),
				[Grind] [nchar](6) NULL DEFAULT (('')),
				[FGColour] [decimal](5, 2) NULL  DEFAULT ((0))
			) ON [PRIMARY]

			CREATE TABLE [dbo].[#RoastingLog3](
				[Blend] [nchar](6) NOT NULL DEFAULT (('')),
				[RoastingColour] [decimal](5, 2) NULL  DEFAULT ((0))
			) ON [PRIMARY]

			-- Write average finished colour by blend and grind into a local temporary file, #FinishedGoodsLog1.
			SET @chrSQLStmt = 'INSERT INTO #FinishedGoodsLog2
			SELECT Blend, Grind, ROUND(AVG(Colour), 2) AS FGColour
				FROM	tblFGColourLog
				WHERE     (DateTest between ''' + @chrFmDateCYMD + ''' AND ''' + @chrToDateCYMD + ''' )
						AND Blend = ''' + @chrBlend + ''' AND Grind = ''' + @chrGrind	+
						''' AND (YEAR(DateTest) * 100 + MONTH(DateTest)) NOT IN (' + @chrExclusion + 
				') GROUP BY Blend, Grind'
			-- print @chrSQLStmt
			exec (@chrSQLStmt)

			-- passed blend is a barblend?
			IF NOT EXISTS (SELECT * FROM tblBarBlend WHERE BarBlend = @chrBlend) 
			-- Get non-BarBlend average roasting colour
			BEGIN
				SET @chrSQLStmt = 'INSERT INTO #RoastingLog3
					SELECT Blend, ROUND(AVG(Colour),2) AS RoastingColour
					FROM         tblRoastingLog
					WHERE     (DateTest BETWEEN ''' + @chrFmDateCYMD + ''' AND ''' + @chrToDateCYMD + ''')
							   AND (YEAR(DateTest) * 100 + MONTH(DateTest)) NOT IN (' + @chrExclusion + 
							  ')	AND Blend = ''' + @chrBlend + 
							  ''' AND (Rejected <> N''Y'' OR Rejected IS NULL)
					GROUP BY  Blend'
				-- print @chrSQLStmt
				EXEC (@chrSQLStmt)
			END
			ELSE
			BEGIN
				-- Get BarBlend average roasting colour
				SET @chrSQLStmt = 'INSERT INTO #RoastingLog3
					SELECT F4.BarBlend as Blend, ROUND(SUM(F4.Expr1 * F3.Percentage),2) AS RoastingColour
					FROM
					 (SELECT	F2.BarBlend, F1.Blend, AVG(F1.Colour) AS Expr1
						   FROM		tblRoastingLog AS F1 INNER JOIN
									tblBarBlend AS F2 ON F1.Blend = F2.Blend
						   WHERE (F1.DateTest ''' + @chrFmDateCYMD + ''' AND ''' + @chrToDateCYMD + ''' )
								AND YEAR(DateTest) * 100 + MONTH(DateTest)) NOT IN (' + @chrExclusion + 
								') AND (Rejected <> N''Y'' OR Rejected IS NULL)
								AND F2.BarBlend = ''' + @chrBlend + 
						   ''' GROUP BY F2.BarBlend, F1.Blend) AS F4 
					INNER JOIN	tblBarBlend AS F3 ON F4.BarBlend = F3.BarBlend AND F4.Blend = F3.Blend
				GROUP BY F4.BarBlend'
				EXEC (@chrSQLStmt)
			END
		
			-- Join 2 temporary files to get the result set	
			SELECT F1.Blend, F1.Grind, DarkeningFactor as CurDF, (FGColour - RoastingColour) as CalDF,
				DarkeningFactor - (FGColour - RoastingColour) as DFDiff,	
				F4.SpecTarg as FGColourTarget, F5.SpecTarg as RoastColourTarget
				FROM         #FinishedGoodsLog2 F1 
				LEFT OUTER JOIN	 vwLatestCurrentDarkeningFactor F2
					ON		F1.Blend = F2.Blend and F1.Grind = F2.Grind
				LEFT OUTER JOIN	 #RoastingLog3 F3
					ON		F1.Blend = F3.Blend
				LEFT OUTER JOIN dbo.vwLatestFGColourSpec F4
					ON		F1.Blend = F4.Blend AND F1.Grind = F4.Grind
				LEFT OUTER JOIN dbo.vwLatestRoastColourSpec F5
					ON		F1.Blend = F5.Blend
				ORDER BY F1.Blend, F1.Grind
		END	
	END
END

GO

