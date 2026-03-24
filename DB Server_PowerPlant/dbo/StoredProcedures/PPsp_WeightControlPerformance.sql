



-- =============================================
-- Author:		<Bong Lee>
-- Create date: <March 7, 2006>
-- Description:	<Finished Package Weight Control Report>
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_WeightControlPerformance]
	-- Add the parameters for the stored procedure here
	@dteFmDate Datetime = '11/1/2005',
	@dteToDate Datetime = '11/30/2005',
	@chrGroupBy NChar(2) = 'PL',
	@ChrWeightUnit NChar(2) = 'KG'
AS
BEGIN
	DECLARE @dteFmDateMDCY DATETIME
	DECLARE @dteToDateMDCY DATETIME
	DECLARE @chrFmDateMDCY CHAR(20)
	DECLARE @chrToDateMDCY CHAR(20)
	DECLARE @chrGrpByFieldName varchar(20)
	DECLARE @chrGrpByFieldName2 varchar(20)
	DECLARE @chrDescField varchar(50)
	DECLARE @chrJoinFile varchar(150)
	DECLARE @chrSQLStmt varchar(3000)
	DECLARE @chrSubQueryStmt varchar(1000)
	DECLARE @chrGroupByStmt varchar(500)
    DECLARE @chrJoinStmt varchar(500)
	DECLARE @decWgtMultiplier decimal(9,8)
	DECLARE @chrWgtMultiplier varchar(10)

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Convert the input date formats are MMDDCCYY to ensure the date format
	SET @dteFmDateMDCY = CONVERT(DATETIME,@dteFmDate,120)
	-- set the ToDate to the 11:59:59 PM of the date.
	SET @dteToDateMDCY = DATEADD(SECOND,-1,DATEADD(DAY,1,CONVERT(DATETIME,@dteToDate,120)))
	
	IF EXISTS (SELECT * FROM tblPalletCount WHERE BatchStartTime BETWEEN @dteFmDateMDCY AND @dteToDateMDCY) 
	
	SET @chrFmDateMDCY = CONVERT(CHAR(20),@dteFmDateMDCY,120)
	SET @chrToDateMDCY = CONVERT(CHAR(20),@dteToDateMDCY,120)
	
	BEGIN
		SET @chrJoinFile = ''

		IF @ChrWeightUnit = 'KG'
			SET @decWgtmultiplier = 0.001
		ELSE -- in LBs
			SET @decWgtmultiplier = 0.0022046

		SET @chrWgtMultiplier = CONVERT(CHAR(10),@decWgtMultiplier) 

		IF @chrGroupBy = 'SO'			-- Group by Shop Order 
		BEGIN		 
			SET @chrGrpByFieldName = 'ShopOrder'
			SET @chrGrpByFieldName2 = 'ShopOrder,'
			SET @chrDescField = 'F1.ShopOrder'
			SET @chrJoinStmt = '(F1.' + RTRIM(@chrGrpByFieldName) + ' = F2.' + RTRIM(@chrGrpByFieldName) + ') and '
			SET @chrJoinFile = ''
		END
		ELSE IF @chrGroupBy = 'PL'		-- Group by Package Line
		BEGIN	
			SET @chrGrpByFieldName  ='ProductionLine'
			SET @chrGrpByFieldName2 = ''
			SET @chrDescField = 'F1.ProductionLine'
			SET @chrJoinStmt = ''
			SET @chrJoinFile = ''
			
		END
		ELSE IF @chrGroupBy = 'OP'		-- Group by Operator
		BEGIN
			SET @chrGrpByFieldName = 'CreatedBy'
			SET @chrGrpByFieldName2 = 'CreatedBy,'
			SET @chrDescField = 'F4.[First Name] + '' '' + F4.[Last Name]'
			SET @chrJoinStmt = 'F1.' + RTRIM(@chrGrpByFieldName) + ' = F2.OperatorInitial and '
			SET @chrJoinFile = 'LEFT OUTER JOIN tblUserProfile F4 ON F1.' + @chrGrpByFieldName + '= F4.Initial'
		END	

		IF @chrGroupBy <> 'PS'
		BEGIN		-- Process for selecting other than by package size 		

			SET @chrSubQueryStmt = 
				'(SELECT ' + RTRIM(@chrGrpByFieldName) + ', ProductionLine AS ProdLine,
					SUM(Cast(([PalletsPacked]*[EnteredUnitsPerPallet]+[CasesPacked])*[saleableUnitsPerCase] AS REAL)) as SaleableUnitsPacked
					FROM tblPalletCount
					WHERE (BatchStartTime BETWEEN ''' + @chrFmDateMDCY + ''' AND ''' + @chrToDateMDCY + ''') 
						AND ([EnteredUnitsPerPallet] > 0 OR [CasesPacked] > 0)
					GROUP BY ' + RTRIM(@chrGrpByFieldName2) + ' ProductionLine) F1 
				INNER JOIN tblWCWeightLog F2 
				ON ' + RTRIM(@chrJoinStmt) + ' F2.ProductionLine = F1.ProdLine     
				LEFT OUTER JOIN tblEquipment F3 ON F1.ProdLine = F3.EquipmentID ' + @chrJoinFile +
				' WHERE (F2.BatchStartTime BETWEEN ''' + @chrFmDateMDCY + ''' AND ''' + @chrToDateMDCY + ''') AND F2.Inactive = 0' 

			SET @chrGroupByStmt = ' GROUP BY  F3.Description, ' + RTRIM(@chrDescField) + 
								   ' ORDER BY  F3.Description, ' + RTRIM(@chrDescField)

			SET @chrSQLStmt = 
			  'SELECT F3.Description AS PackageLine,' + RTRIM(@chrDescField) + ' AS Description, 
				Cast(Avg([NetWeight]) * ' + @chrWgtmultiplier + ' as Decimal(7,3)) as AvgWeight,
				Cast(Avg([NetWeight]-[TargetWeight]) as Decimal(7,3)) AS AvgWgtVariance,
				Cast(Avg(([NetWeight]-[TargetWeight]) /[TargetWeight]) * 100 as decimal(5,2)) AS [wgtVariancePct],
				Cast(StDev(NetWeight) AS Decimal(5,2)) AS StdWeight,
				Cast(Round(Avg([NetWeight]-[TargetWeight]) * Avg(SaleableUnitsPacked) * ' + @chrWgtmultiplier + ' ,0) as bigint) AS OverPackedWgt,
				Cast(Sum(CASE WHEN [NetWeight]>[LowerControlLimit] THEN 1 ELSE 0 END) * 100.0 / Count(*) AS Decimal(5,2)) AS PctLegalCompliance,
				Cast(Sum(CASE WHEN [NetWeight]<[MinWeight] THEN 1 ELSE 0 END) * 100.0 / Count(*) AS Decimal(5,2)) AS PctBelowLimit,
				Cast(Sum(CASE WHEN [NetWeight]>[MaxWeight] THEN 1 ELSE 0 END) * 100.0 / Count(*) AS Decimal(5,2)) AS PctAboveLimit,
				Cast(Sum(CASE WHEN [NetWeight]<=[MaxWeight] and [NetWeight]>=[MinWeight] THEN 1 ELSE 0 END) * 100.0 / Count(*) AS Decimal(5,2)) AS PctInSpec,
				Cast(Min(NetWeight) * ' + @chrWgtmultiplier + ' AS Decimal(7,4)) AS MiniWeight, 
				Cast(Max(NetWeight) * ' + @chrWgtmultiplier + ' AS Decimal(7,4)) AS MaxiWeight,
				Count(NetWeight) as NoOfSamples FROM '  + 
				RTrim(@chrSubQueryStmt) + RTrim(@chrGroupByStmt)

			-- print @chrsqlstmt
			EXEC (@chrSQLStmt)
		END
		ELSE
		BEGIN		-- Process for selecting by Package Size
			SELECT F5.Description AS PackageLine, 			
				Cast(F4.TargetWeight * @chrWgtmultiplier AS Decimal(7,5)) AS Description,
				Cast(Avg([NetWeight]) * @chrWgtmultiplier AS Decimal(7,4)) AS AvgWeight,
				Cast(Avg([NetWeight]-F4.[TargetWeight]) AS Decimal(7,4)) AS AvgWgtVariance,
				Cast(Avg(([NetWeight]-F4.[TargetWeight]) /F4.[TargetWeight]) * 100 AS Decimal(5,2)) AS [wgtVariancePct],
				Cast(StDev(NetWeight) AS Decimal(5,2)) AS StdWeight,
				Cast(Round(Avg([NetWeight]-F4.[TargetWeight]) * Avg(F3.SaleableUnitsPacked) * @decWgtmultiplier ,0) as int) AS OverPackedWgt,
				Cast(Sum(CASE WHEN [NetWeight]>[LowerControlLimit] THEN 1 ELSE 0 END) * 100.0 / Count(*) AS Decimal(5,2)) AS PctLegalCompliance,
				Cast(Sum(CASE WHEN [NetWeight]<[MinWeight] THEN 1 ELSE 0 END) * 100.0 / Count(*) AS Decimal(5,2)) AS PctBelowLimit,
				Cast(Sum(CASE WHEN [NetWeight]>[MaxWeight] THEN 1 ELSE 0 END) * 100.0 / Count(*) AS Decimal(5,2)) AS PctAboveLimit,
				Cast(Sum(CASE WHEN [NetWeight]<=[MaxWeight] and [NetWeight]>=[MinWeight] THEN 1 ELSE 0 END) * 100.0 / Count(*) AS Decimal(5,2)) AS PctInSpec,
				Cast(Min(NetWeight) * @chrWgtmultiplier AS Decimal(7,4)) AS MiniWeight,
				Cast(Max(NetWeight) * @chrWgtmultiplier AS Decimal(7,4)) AS MaxiWeight,
				Count(NetWeight) AS NoOfSamples
			FROM tblWCWeightLog F4 INNER JOIN 
				(SELECT f2.TargetWeight, F2.ProductionLine, SUM(cast(([PalletsPacked]*[EnteredUnitsPerPallet]+[CasesPacked])*[saleableUnitsPerCase] as real)) as SaleableUnitsPacked
				FROM tblPalletCount F1 INNER JOIN
					(SELECT DISTINCT tblWCWeightLog.TargetWeight,ProductionLine,batchstarttime FROM tblWCWeightLog 
					  WHERE BatchStartTime between @dteFmDateMDCY and @dteToDateMDCY AND InActive = 0) F2
				ON F1.BatchStartTime = F2.BatchStartTime and F1.ProductionLine = F2.ProductionLine
				WHERE ([EnteredUnitsPerPallet] > 0 OR [CasesPacked] > 0) 
				GROUP BY f2.TargetWeight,F2.ProductionLine) F3
			ON F4.TargetWeight = F3.TargetWeight and F4.ProductionLine = F3.ProductionLine
			LEFT JOIN tblEquipment F5
			ON F4.ProductionLine = F5.EquipmentID
			WHERE  (F4.BatchStartTime between @dteFmDateMDCY and @dteToDateMDCY) AND F4.InActive = 0
			GROUP BY F4.TargetWeight,F5.Description
			ORDER BY F4.TargetWeight,F5.Description
		END
	END	
END

GO

