


-- =============================================
-- Author:		Bong Lee
-- Create date: Aug.31, 2010
-- Description: WO#262 - Show give away coffee in dollar (based on Std. Blend Cost)
-- WO#359:		Aug. 25, 2011	Bong Lee	
-- Description:	Use standard table functions to replace some of the select statements
--				Apply pallet adjustment to cases produced
-- IC#4397:		Jan. 16, 2014	Bong Lee
-- Description: Divided by zero error	
-- WO#1297:		Jul. 29, 2011	Bong Lee	
-- Description:	Use standard cost from the Item Master Table
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_GiveAwayCoffee] 
	-- Add the parameters for the stored procedure here
	@vchAction varchar(50) = NULL,
	@vchFacility varchar(3) = NULL,
	@intPercentTolerance int = NULL,
	@vchPackagingLine varchar(10) = NULL,
	@vchItemNumber varchar(35) = NULL,	
	@intShopOrder int = NULL,
	@vchOperator varchar(10) = NULL,
	@chrShowShift as char = 'Y',
	@dteFromProdDate as DateTime,
	@intFromShift as tinyint,
	@dteToProdDate as DateTime,
	@intToShift as tinyint

AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		IF @vchAction is NULL
		BEGIN
			/* WO#1297 Del Start 
			DECLARE @chrEnvironment as char(1);
			DECLARE @vchSQLStmt as nvarchar(4000);
			DECLARE @vchISeriesSQLStmt as nvarchar(3000);
			DECLARE @iSeriesName as nvarchar(10);
			DECLARE @vchUserLib varchar(10);
			DECLARE @vchOriginalLib varchar(10);
			WO#1297 Del Stop */
			DECLARE @vchFromDate varchar(8);
			DECLARE @vchToDate varchar(8);

-- WO#359 Add Start 
		DECLARE @decGMtoLB as decimal(10,9)
		DECLARE @decLBtoGM as decimal(10,7)

		SELECT @decLBtoGM = Value1, @decGMtoLB = Value2 FROM tblControl 
			WHERE [KEY] = 'WeightConversion' AND SubKey = 'General'
-- WO#359 Add Stop
			/* WO#1297 Del Start 
			SELECT @chrEnvironment = UPPER(SUBSTRING(Value2,1,1)) from tblControl Where [Key] = 'Facility' and SubKey = 'General'
			SELECT @iSeriesName = Case When @chrEnvironment = 'P' Then Value1 Else Value2 END from tblControl Where [Key] = 'iSeriesNames' and SubKey = 'ServerNames'

		 -- Check the processing environment. Is production or UA?
			If @chrEnvironment = 'P'
			BEGIN
				Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibPrd'
			END
			ELSE
			BEGIN
				If @chrEnvironment = 'U'
				BEGIN
					Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibUA'
				END
				ELSE
				BEGIN
					Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibDev'
				END
			END 
			WO#1297 Del Stop */

			-- Minus one day to the entered From Shift Production Date and add one more day to the To Shift Production Date to ensure
			-- the date range will be passed to BPCS can cover all shop orders that requested.
			SELECT @vchFromDate = CONVERT(varchar(8),DATEADD(day,-1,@dteFromProdDate),112), @vchToDate = CONVERT(varchar(8),DATEADD(day,1, @dteToProdDate),112)

			-- Obtain Standard Costs of the components in $/lb from BPCS.
	/* WO#1297 Del Start  
--IC#4397	SET	@vchISeriesSQLStmt = N'SELECT tIIPI.shoporder, SUM(IFNULL(tITH.TSCST,0) * tITH.THQUM)/SUM(IFNULL(tITH.THQUM,0)) as StdBlendCostPerLB FROM( ' +
			SET	@vchISeriesSQLStmt = N'SELECT tIIPI.shoporder,  ' + --IC#4397
					'CASE WHEN SUM(IFNULL(tITH.THQUM,0)) = 0 THEN 0 ELSE ' + --IC#4397
					'SUM(IFNULL(tITH.TSCST,0) * tITH.THQUM)/SUM(IFNULL(tITH.THQUM,0)) END ' + --IC#4397
					'as StdBlendCostPerLB FROM( ' +
					'SELECT IPREF as ShopOrder, MIN(IPPAL) as PalletID ' + 
					'FROM ' + @vchUserLib + '.iipi$ WHERE IPFAC = ''''' + @vchFacility + ''''' AND ' +
					'IPPCDT Between ' + @vchFromDate + ' And ' + @vchToDate + ' '
			
			IF @vchPackagingLine IS NOT NULL 
				SET @vchISeriesSQLStmt = @vchISeriesSQLStmt + N'And IPMACH = ''''' + @vchPackagingLine + ''''' '

			IF @vchItemNumber IS NOT NULL 
				SET @vchISeriesSQLStmt = @vchISeriesSQLStmt + N'And IPPROD = ''''' + @vchItemNumber + ''''' '

			IF @intShopOrder IS NOT NULL
				SET @vchISeriesSQLStmt = @vchISeriesSQLStmt + N'And IPREF = ' + Cast(@intShopOrder as varchar(10)) + ' '
	
			IF @vchOperator IS NOT NULL
				SET @vchISeriesSQLStmt = @vchISeriesSQLStmt + N'And IPEMP = ''''' + @vchOperator + ''''' '
	
			SET @vchISeriesSQLStmt = @vchISeriesSQLStmt + N'Group by IPREF) tIIPI ' +                         
					'LEFT JOIN ' + @vchOriginalLib + '.ith tITH ' +                     
					'ON tIIPI.ShopOrder = tITH.TREF And tIIPI.PalletID= tITH.TCOM ' +
					'WHERE SUBSTRING(tITH.TCLAS,2,1) = ''''B'''' GROUP BY ShopOrder'
			
			-- If the From Shift Production date is beyond the current BPCS Inventory Transaction History file (153 days from current day
			-- i.e. about 5 months but file is checked and updated at month end)
			IF @dteFromProdDate < dateadd(day,-150, getDate()) 
			BEGIN
				-- SET	@vchISeriesSQLStmt2 = REPLACE(@vchISeriesSQLStmt,@vchOriginalLib + '.ith ',@vchUserLib + '.ith$A')
				SET @vchISeriesSQLStmt = @vchISeriesSQLStmt + N' UNION ' + REPLACE(@vchISeriesSQLStmt,@vchOriginalLib + '.ith ',@vchUserLib + '.ith$A ')
				SET @vchISeriesSQLStmt = N'Select ShopOrder, max(StdBlendCostPerLB) as StdBlendCostPerLB FROM (' + @vchISeriesSQLStmt + ') tA GROUP BY ShopOrder'
			END
			Set @vchSQLStmt = N'Select * from openquery(' + @iSeriesName + ',''' + @vchISeriesSQLStmt + ''')'
	PRINT '@vchSQLStmt = ' + @vchSQLStmt 
			
			If object_id('tempdb..#tblStdBlendCost') is not null
			DROP TABLE #tblStdBlendCost

			CREATE TABLE #tblStdBlendCost(
			ShopOrder int,
			StdBlendCostPerLB decimal(15,5)
			) ON [PRIMARY]

			INSERT INTO #tblStdBlendCost
			EXEC sp_ExecuteSQL @vchSQLStmt

			CREATE CLUSTERED INDEX IDX_#tblStdBlendCost ON #tblStdBlendCost(ShopOrder)	-- IC#4397
	WO#1297 Del Stop  */
				
	--			select * from #tblStdBlendCost
-- WO#359 Del Start 
--			/* Find the Work Shift type for each line - if line is duplicated, pick the active one */
--			DECLARE @tblComputerConfig Table
--			(Facility varchar(3) ,Packagingline varchar(10), WorkShiftType varchar(10))
--
--			INSERT INTO @tblComputerConfig
--			SELECT T1.Facility, T1.Packagingline, T1.WorkShiftType 
--			FROM tblcomputerconfig T1 
--			Left Outer Join 
--				(SELECT Facility, Packagingline 
--					FROM tblComputerconfig 
--				WHERE Packagingline <> 'SPARE'
--				GROUP BY Facility, Packagingline
--				HAVING Count(*) > 1) T2
--			ON T1.Facility = T2.Facility AND T1.Packagingline = T2.Packagingline
--			WHERE (T2.Packagingline is null OR T1.RecordStatus = 1) 
--				AND T1.PackagingLine <> 'SPARE'
--			GROUP BY T1.Facility, T1.Packagingline, T1.WorkShiftType
-- WO#359 Del Stop

			-- Select and summarize the Session Control History record
			If object_id('tempdb..#tblSCH') is not null
			DROP TABLE #tblSCH

			SELECT tSCH.Facility, tSCH.DefaultPkgLine , tSCH.OverrideShiftNo, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.CasesProduced,
-- WO#1297 		tSCH.StartTIme, tSBC.StdBlendCostPerLb
				tSCH.StartTIme, tIM.StdCostPerLB as StdBlendCostPerLb		-- WO#xxxx 
			INTO #tblSCH
			FROM dbo.tfnSessionControlHstDetail(NULL,@vchFacility,@vchPackagingLine,@vchOperator,@intShopOrder,@vchItemNumber,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift) tSCH -- WO#359
			LEFT OUTER JOIN tblItemMaster tIM		-- WO#1297 
			ON tSCH.ItemNumber = tIM.ItemNumber		-- WO#1297
-- WO#1297		LEFT OUTER JOIN #tblStdBlendCost tSBC
-- WO#1297		ON tSCH.ShopOrder = tSBC.ShopOrder
			CREATE CLUSTERED INDEX IDX_#tblSCH ON #tblSCH(Facility,DefaultPkgLine,ShopOrder,StartTIme)	-- IC#4397	
-- WO#359 Del Start 
--			FROM tblSessionControlHst tSCH
--			Left Outer Join @tblComputerConfig tCC
--				ON tSCH.Facility = tCC.Facility AND tSCH.DefaultPkgLine =  tCC.PackagingLine
--			Left Outer Join tblshift tS
--				ON tSCH.Facility = tS.Facility AND tSCH.OverrideShiftNo = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
--			LEFT OUTER JOIN #tblStdBlendCost tSBC
--				ON tSCH.ShopOrder = tSBC.ShopOrder
--			WHERE (@vchFacility is NULL OR tSCH.Facility = @vchFacility)
--				AND Convert(varchar(8),tSCH.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
--				AND (@vchPackagingLine is NULL OR tSCH.DefaultPkgLine = @vchPackagingLine)
--				AND (@intShopOrder is NULL OR tSCH.ShopOrder = @intShopOrder)
--				AND (@vchItemNumber is NULL OR tSCH.ItemNumber = @vchItemNumber)
--				AND (@vchOperator is NULL OR tSCH.Operator = @vchOperator)
-- WO#359 Del Stop 

			-- Get Weight information based on the Session Control History
			If object_id('tempdb..#tblWgtLog') is not null
			DROP TABLE #tblWgtLog

			SELECT tWL.Facility, tSCH.OverrideShiftNo, tWL.PackagingLine, tSCH.ItemNumber, tWL.ShopOrder, 
				   tWL.ActualWeight, CASE WHEN tWL.LabelWeight = 0 THEN 0 ELSE tIM.LabelWeight END as LabelWeight, tWL.TargetWeight, tWL.MinWeight, tWL.MaxWeight,
				   CASE WHEN tWL.ActualWeight > (CASE WHEN tWL.LabelWeight = 0 THEN 0 ELSE tIM.LabelWeight END) THEN 1 ELSE 0 END as OverPackCount,
				   CASE WHEN tWL.ActualWeight < (CASE WHEN tWL.LabelWeight = 0 THEN 0 ELSE tIM.LabelWeight END) THEN 1 ELSE 0 END as UnderPackCount	
			INTO #tblWgtLog
			FROM dbo.tblWeightLog tWL
			Inner join #tblSCH tSCH
			ON tWL.Facility = tSCH.Facility 
				AND tWL.PackagingLine = tSCH.DefaultPkgLine 
				AND tWL.ShopOrder = tSCH.ShopOrder
				AND tWL.ShopOrderStartTIme = tSCH.StartTIme
			LEFT OUTER JOIN tblItemMaster tIM
			ON tWL.Facility = tIM.Facility and tWL.ItemNumber = tIM.ItemNumber
			WHERE 
				 tWL.WeightSource <> 2	-- exlcluded tareweight records	
				AND (@intPercentTolerance is NULL OR ABS(tWL.ActualWeight - tWL.TargetWeight) <= (@intPercentTolerance * tWL.TargetWeight/100))
				AND Inactive = 0

			CREATE CLUSTERED INDEX IDX_#tblWgtLog ON #tblWgtLog(Facility,OverrideShiftNo,PackagingLine,ItemNumber,ShopOrder)	-- IC#4397
			/*  Calculate other values for different grouping levels */
			-- By Facility\Shift\Line\Item\Shop Order Totals
			;With cteTotals AS 
			(
-- WO#359	SELECT tSCH.Facility, CASE WHEN @chrShowShift = 'Y' THEN tSCH.OverrideShiftNo ELSE NULL END as OverrideShiftNo , tSCH.DefaultPkgLine, tSCH.ItemNumber, tSCH.ShopOrder, tSCH.CasesProduced,
			SELECT tSCH.Facility, CASE WHEN @chrShowShift = 'Y' THEN tSCH.OverrideShiftNo ELSE NULL END as OverrideShiftNo , tSCH.DefaultPkgLine, tSCH.ItemNumber, tSCH.ShopOrder	-- WO#359
				, tSCH.CasesProduced + tADJ.AdjustedQty as CasesProduced,																															-- WO#359
				tWL.AvgActWgt, tWL.NoOfCount, tWL.LabelWeight, tWL.StDevWgt, tWL.TargetWeight, tWL.MinWeight, tWL.MaxWeight, 
-- WO#359		Case When tIM.labelweightUOM = 'GM' Then tSCH.StdBlendCostPerLB / 453.5924 Else tSCH.StdBlendCostPerLB END as StdBlendCostPerLB,
-- WO#359	    (tWL.AvgActWgt - tWL.LabelWeight) * tIM.SaleableUnitPerCase * tIM.PackagesPerSaleableUnit * tSCH.CasesProduced as GiveAwayWeight,
				Case When tIM.labelweightUOM = 'GM' Then tSCH.StdBlendCostPerLB / @decLBtoGM Else tSCH.StdBlendCostPerLB END as StdBlendCostPerLB,	-- WO#359
				(tWL.AvgActWgt - tWL.LabelWeight) * tIM.SaleableUnitPerCase * tIM.PackagesPerSaleableUnit * (tSCH.CasesProduced + tADJ.AdjustedQty) as GiveAwayWeight, -- WO#359
				tIM.labelweightUOM,
-- WO#359		(tWL.AvgActWgt - tWL.LabelWeight) * Case When tIM.labelweightUOM = 'GM' Then tSCH.StdBlendCostPerLB / 453.5924 Else tSCH.StdBlendCostPerLB End
-- WO#359		* tIM.SaleableUnitPerCase * tIM.PackagesPerSaleableUnit * tSCH.CasesProduced as GiveAwayDollars,
-- WO#359		tIM.LabelWeight * Case When tIM.labelweightUOM = 'GM' Then tSCH.StdBlendCostPerLB / 453.5924 Else tSCH.StdBlendCostPerLB End
-- WO#359		* tIM.SaleableUnitPerCase * tIM.PackagesPerSaleableUnit * tSCH.CasesProduced as CoffeeCost,
				(tWL.AvgActWgt - tWL.LabelWeight) * Case When tIM.labelweightUOM = 'GM' Then tSCH.StdBlendCostPerLB / @decLBtoGM Else tSCH.StdBlendCostPerLB End	-- WO#359
				* tIM.SaleableUnitPerCase * tIM.PackagesPerSaleableUnit * (tSCH.CasesProduced + tADJ.AdjustedQty) as GiveAwayDollars,				-- WO#359
				tIM.LabelWeight * Case When tIM.labelweightUOM = 'GM' Then tSCH.StdBlendCostPerLB / @decLBtoGM Else tSCH.StdBlendCostPerLB End		-- WO#359
				* tIM.SaleableUnitPerCase * tIM.PackagesPerSaleableUnit * (tSCH.CasesProduced + tADJ.AdjustedQty) as CoffeeCost,					-- WO#359
				OverPackCount, UnderPackCount
			FROM
				(SELECT Facility, CASE WHEN @chrShowShift = 'Y' THEN OverrideShiftNo ELSE 0 END as OverrideShiftNo, DefaultPkgLine, ItemNumber, ShopOrder, StdBlendCostPerLB, SUM(CasesProduced) as CasesProduced
				FROM #tblSCH 
				GROUP BY Facility, CASE WHEN @chrShowShift = 'Y' THEN OverrideShiftNo ELSE 0 END, DefaultPkgLine, ItemNumber, ShopOrder, StdBlendCostPerLB) tSCH
			LEFT OUTER JOIN
				(SELECT Facility, CASE WHEN @chrShowShift = 'Y' THEN OverrideShiftNo ELSE 0 END as OverrideShiftNo, PackagingLine, ItemNumber, ShopOrder,
					LabelWeight, Round(stdev(ActualWeight),1) as StDevWgt, Avg(TargetWeight) as TargetWeight, Min(ActualWeight) as MinWeight, Max(ActualWeight) as MaxWeight, 
					Avg(ActualWeight) as AvgActWgt, Count(*) as NoOfCount, 
					SUM(OverPackCount) as OverPackCount, SUM(UnderPackCount) as UnderPackCount
				FROM #tblWgtLog 
				GROUP BY Facility, CASE WHEN @chrShowShift = 'Y' THEN OverrideShiftNo ELSE 0 END, PackagingLine, ItemNumber, ShopOrder, LabelWeight) tWL
			ON tSCH.Facility = tWL.Facility
				AND tSCH.OverrideShiftNo = tWL.OverrideShiftNo
				AND tSCH.DefaultPkgLine = tWL.PackagingLine
				AND tSCH.ItemNumber = tWL.ItemNumber
				AND tSCH.ShopOrder = tWL.ShopOrder
			LEFT OUTER JOIN tblItemMaster tIM
			ON tSCH.Facility = tIM.Facility and tSCH.ItemNumber = tIM.ItemNumber
-- WO#359 Add Start
		  LEFT OUTER JOIN
		-- Add Pallet Adjustment
			(SELECT Facility, CASE WHEN @chrShowShift = 'Y' THEN ShiftNo ELSE 0 END as ShiftNo, DefaultPkgLine, Operator, ShopOrder, SUM(AdjustedQty) as AdjustedQty
				FROM tfnSessionControlHstSummary ('WithAdjByLineSO',@vchFacility,@vchPackagingLine,@vchOperator,@intShopOrder,@vchItemNumber,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift)
				GROUP BY Facility, CASE WHEN @chrShowShift = 'Y' THEN ShiftNo ELSE 0 END, DefaultPkgLine, ShopOrder, Operator) tADJ
			ON tSCH.Facility = tADJ.Facility AND tSCH.OverrideShiftNo = tADJ.ShiftNo AND tSCH.DefaultPkgLine = tADJ.DefaultPkgLine 
				AND tSCH.ShopOrder = tADJ.ShopOrder
-- WO#359 Add Stop
			)

			-- order by shift reporting sequence
			SELECT cteTotals.* from cteTotals
--WO#359	Left Outer Join @tblComputerConfig tCC
			Left Outer Join dbo.vwLineWorkShiftType	tCC		--WO#359
			ON cteTotals.Facility = tCC.Facility AND cteTotals.DefaultPkgLine =  tCC.PackagingLine
			LEFT OUTER JOIN tblShift tS
			ON cteTotals.Facility = tS.Facility and ISNULL(tCC.WorkShiftType,'P') = tS.WorkGroup and cteTotals.OverrideShiftNo = tS.Shift
			ORDER BY cteTotals.Facility, tS.ShiftSequence, cteTotals.DefaultPkgLine, cteTotals.ItemNumber, cteTotals.ShopOrder

--WO#1297	If object_id('tempdb..#tblStdBlendCost') is not null
--WO#1297	DROP TABLE #tblStdBlendCost

			If object_id('tempdb..#tblWgtLog') is not null
			DROP TABLE #tblWgtLog

			If object_id('tempdb..#tblSCH') is not null
			DROP TABLE #tblSCH

		END
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		BEGIN TRY
			If object_id('tempdb..#tblStdBlendCost') is not null
			DROP TABLE #tblStdBlendCost
		END TRY
		BEGIN CATCH
		END CATCH

		BEGIN TRY
			If object_id('tempdb..#tblWgtLog') is not null
			DROP TABLE #tblWgtLog
		END TRY
		BEGIN CATCH
		END CATCH

		BEGIN TRY
			If object_id('tempdb..#tblSCH') is not null
			DROP TABLE #tblSCH
		END TRY
		BEGIN CATCH
		END CATCH

		SELECT 
				@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();

			-- Use RAISERROR inside the CATCH block to return error
			-- information about the original error that caused
			-- execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, -- Message text.
					   @ErrorSeverity, -- Severity.
					   @ErrorState -- State.
					   );
	END CATCH;	
END

GO

