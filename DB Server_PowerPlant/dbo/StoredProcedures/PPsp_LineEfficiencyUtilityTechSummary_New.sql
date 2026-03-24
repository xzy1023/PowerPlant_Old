

-- =============================================
-- Author:		Bong Lee
-- Create date: Oct 08,2009
-- Description:	Line Efficency Utitlity technician Summary
-- WO#194:		Sep. 9, 2010	Bong Lee	
-- Description:	If the time frame for the expected shift crosses mid-night
--				and the given time is prior to mid-night, the Shift Production 
--				Date will be the given time plus one day. Before this change the 	
--				Shift Production Date for this case was same as the given time but 
--				if the given time after mid-night, the result was given time 
--				minus one day.
--				Do not include pallet adjustment to cases produced
-- WO#359:		Jul. 22, 2011	Bong Lee	
-- Description:	Use standard table functions to replace some of the select statements
--				Add Pound Produced in the Overall and Individual sessions
-- WO#27470:		Nov. 4, 2019	Bong Lee
-- Description:	Routing rates are required to base on effective date
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_LineEfficiencyUtilityTechSummary_New]
	@vchFacility varchar(3),
	@dteFromProdDate as DateTime,
	@intFromShift as tinyint,
	@dteToProdDate as DateTime,
	@intToShift as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

-- WO#194	DECLARE @vchServerSQLStmt  varchar(1500);
-- WO#194	DECLARE @vchSQLStmt  varchar(1700);

	BEGIN TRY;
-- WO#359 Add Start 
		DECLARE @decGMtoLB as decimal(10,9)

		SELECT @decGMtoLB = Value2 FROM tblControl 
			WHERE [KEY] = 'WeightConversion' AND SubKey = 'General'
-- WO#359 Add Stop
-- WO#194 Add Start 
-- WO#359 Del Start 
--		/* Find the Work Shift type for each line - if line is duplicated, pick the active one */
--		Declare @tblComputerConfig Table
--		(Facility varchar(3) ,Packagingline varchar(10), WorkShiftType varchar(10))
--
--		-- Create temporary variable table to hold the shift no and shift sequence cross reference by line
--		INSERT INTO @tblComputerConfig
--		SELECT T1.Facility, T1.Packagingline, T1.WorkShiftType 
--		FROM tblcomputerconfig T1 
--		Left Outer Join 
--			(SELECT Facility, Packagingline 
--				FROM tblComputerconfig 
--			WHERE Packagingline <> 'SPARE'
--			Group By Facility, Packagingline
--			Having Count(*) > 1) T2
--		ON T1.Facility = T2.Facility AND T1.Packagingline = T2.Packagingline
--		WHERE t2.Packagingline is null OR RecordStatus = 1
--		Group By T1.Facility, T1.Packagingline, T1.WorkShiftType
-- WO#359 Del Stop
		-- Create temporary table to hold the selected Session COntrol History for using in multiple times 
		IF object_id('tempdb..#temp_SessionControlHst') is not null
			DROP TABLE #temp_SessionControlHst

		-- WO#27470  Select tSCHst.Facility, tSCHst.DefaultPkgLine as PackagingLine, tSCHst.StartTime, tSCHst.Operator, tSCHst.ShopOrder, tSCHst.ItemNumber, 
		Select tSCHst.Facility, tSCHst.ShiftProductionDate, tSCHst.DefaultPkgLine as PackagingLine, tSCHst.StartTime, tSCHst.Operator, tSCHst.ShopOrder, tSCHst.ItemNumber,			-- WO#27470
			Sum(tSCHst.CasesProduced) as CasesProduced, Sum(Isnull(DateDiff(second,tSCHst.StartTime,tSCHst.StopTime),0)) /3600.00 as RunTime
		INTO #temp_SessionControlHst
		FROM dbo.tfnSessionControlHstDetail(NULL,@vchFacility,NULL,NULL,NULL,NULL,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift) tSCHst	-- WO#359

-- WO#359 Del Start
--		From tblSessionControlHst tSCHst	
--		Left Outer Join @tblComputerConfig tCC
--		Left Outer Join dbo.vwLineWorkShiftType	tCC		
--		ON tSCHst.Facility = tCC.Facility AND tSCHst.DefaultPkgLine =  tCC.PackagingLine
--		Left Outer Join tblshift tS
--		ON tSCHst.Facility = tS.Facility AND tSCHst.OverrideShiftNo = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
--		Where tSCHst.Facility = @vchFacility 
--			AND Convert(varchar(8),tSCHst.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
--			-- exclude Set-up Operators in Mississauga 
--			AND ((tSCHst.Facility not in (Select facility from dbo.tblFacility where Region = '01') OR Len(Rtrim(Operator)) > 3) ) 
-- WO#359 Del Stop
		-- WO#27470  Group By tSCHst.Facility, tSCHst.DefaultPkgLine, tSCHst.StartTime, tSCHst.Operator, tSCHst.ShopOrder, tSCHst.ItemNumber
		Group By tSCHst.Facility, tSCHst.ShiftProductionDate, tSCHst.DefaultPkgLine, tSCHst.StartTime, tSCHst.Operator, tSCHst.ShopOrder, tSCHst.ItemNumber				-- WO#27470
/* WO#194 Del Start
		-- Create temporary table to hold the Pallet Adjustment for using in multiple times 
		IF object_id('tempdb..#temp_PalletAdjustment') is not null
			DROP TABLE #temp_PalletAdjustment

		SELECT tPA.Facility, tPA.MachineID, tPA.ShopOrder, tPA.Operator, 
				SUM(tPA.AdjustedQty) as AdjustedQty 
			INTO #temp_PalletAdjustment
		From tblPalletAdjustment tPA
		LEFT OUTER JOIN tblPalletHst tPH
		ON tPA.PalletID = tPH.PalletID
		LEFT OUTER JOIN @tblComputerConfig tCC
			ON tPH.Facility = tCC.Facility AND tPH.DefaultPkgLine =  tCC.PackagingLine
		LEFT OUTER JOIN tblshift tS
			ON tPH.Facility = tS.Facility AND tPH.ShiftNo = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup
		Where tPA.Facility = @vchFacility 
			AND Convert(varchar(8),tPH.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
		Group by tPA.Facility, tPA.MachineID, tPA.ShopOrder, tPA.Operator
WO#194 Del Stop */

		-- Create temporary table to hold the Down Time Log for using in multiple times 
		IF object_id('tempdb..#temp_DownTimeLog') is not null
			DROP TABLE #temp_DownTimeLog

		SELECT tD.*
			INTO #temp_DownTimeLog
		From dbo.tblDownTimeLog tD
--WO#359  Left Outer Join @tblComputerConfig tCC
		Left Outer Join dbo.vwLineWorkShiftType	tCC		--WO#359
		ON tD.Facility = tCC.Facility AND tD.MachineID = tCC.PackagingLine
		Left Outer Join tblshift tS
--WO$359 		ON tD.Facility = tS.Facility AND tD.Shift = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup		
		ON tD.Facility = tS.Facility AND tD.Shift = tS.Shift AND ISNULL(tCC.WorkShiftType,'P') = tS.WorkGroup	--WO#359
		Where tD.Facility = @vchFacility and InActive = 0 
			  AND Convert(varchar(8),tD.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))

		;
-- WO#194 Add Stop

		-- StdUnitPerHr = Routing rate * (2 ** Base code)
		-- StdMachineHrEarnedInUnit = Cases Produced / StdUnitPerHr
		-- Units Produced = Cases Produced * Saleable Unit Per Case
		-- Actual Line Efficiency % = Case Produced / Acutal run time / StdUnitPerHr * 100%
		-- Or
		-- Actual Line Efficiency % = StdMachineHrEarnedInUnit / Acutal run time * 100%

		------------------------------------------------
		-- Calculate Utility Tech. Performance by Line --
		------------------------------------------------
		With USCH_CTE
		AS
		(
-- WO#27470 ADD Start
		  SELECT tUSSCH.Facility , tUSSCH.UtilityTech,  tUSSCH.PackagingLine , tUSSCH.ShopOrder
			,SUM(tDTL.DownTime) as DownTime
			,SUM(tUSSCH.CasesProduced) as CasesProduced
			,SUM(tUSSCH.RunTime) as RunTime
			,SUM(tUSSCH.StdMachineHrEarnedInUnit) as StdMachineHrEarnedInUnit
			,AVG(tUSSCH.StdWCEfficiency) as StdWCEfficiency	
			,SUM(tUSSCH.PoundProd) as PoundProd
		 FROM
		 (
-- WO#27470 ADD Stop
-- WO#194 Select tSCH.Facility, tSCH.UtilityTech, tSCH.PackagingLine, tSCH.ShopOrder, (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0)) as CasesProduced, tSCH.RunTime, 
-- WO#194 CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0))/ (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit,
-- WO#359 Select tSCH.Facility, tSCH.UtilityTech, tSCH.PackagingLine, tSCH.ShopOrder, tSCH.CasesProduced,  tSCH.RunTime,		-- WO#194
-- WO#359	CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else tSCH.CasesProduced / (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit,	-- WO#194
/*WO#27470 DEL Start
- WO#359 Add Start
		  SELECT tSCH.Facility, tSCH.UtilityTech, tSCH.PackagingLine, tSCH.ShopOrder, (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0)) as CasesProduced, tSCH.RunTime,	 -- WO#359
		    CASE WHEN tSMER_ID.MachineHours IS NULL AND tSMER_WC.MachineHours IS Null 
				THEN 0 ELSE (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0))/ (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) END As StdMachineHrEarnedInUnit,	
-- WO#359 Add Stop
			ISNULL(tSMER_ID.StdWorkCenterEfficiency,ISNULL(tSMER_WC.StdWorkCenterEfficiency,0)) as StdWCEfficiency, tDTL.DownTime
			CASE When ISNULL(tfSMER.MachineHours,0)= 0 Then 0 Else (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0))/ (POWER(10,ISNULL(tfSMER.BasisCode,0)) / tfSMER.MachineHours) End As StdMachineHrEarnedInUnit		-- WO#27470
			,ISNULL(tfSMER.StdWorkCenterEfficiency,0) as StdWCEfficiency															-- WO#27470
			,tDTL.DownTime																											-- WO#27470
			,ROUND(ISNULL((tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0)) * tIM.LabelWeight * tIM.PackagesPerSaleableUnit * tIM.SaleableUnitPerCase * (CASE WHEN UPPER(tIM.LabelWeightUOM) = 'GM' THEN @decGMtoLB ELSE 1 END), 0),0) AS PoundProd  -- WO#359 
		  From 	
WO#27470 DEL Stop */
-- WO#27470	ADD Start
			  SELECT tSCH.Facility, tSCH.Operator, tSCH.UtilityTech, tSCH.PackagingLine, tSCH.ShopOrder
				,SUM((tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0))) as CasesProduced
				,SUM(tSCH.RunTime) as RunTime
  				,SUM(CASE When ISNULL(tfSMER.MachineHours,0)= 0 Then 0 Else (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0))/ (POWER(10,ISNULL(tfSMER.BasisCode,0)) / tfSMER.MachineHours) End) As StdMachineHrEarnedInUnit		
				,AVG(ISNULL(tfSMER.StdWorkCenterEfficiency,0)) as StdWCEfficiency
				,SUM(ROUND(ISNULL((tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0)) * tIM.LabelWeight * tIM.PackagesPerSaleableUnit * tIM.SaleableUnitPerCase * (CASE WHEN UPPER(tIM.LabelWeightUOM) = 'GM' THEN @decGMtoLB ELSE 1 END), 0),0)) AS PoundProd
			  FROM
-- WO#27470	ADD Stop
-- WO#359	(Select tSCH1.Facility, tSCH1.PackagingLine, tSCH1.Operator, tSCH1.ShopOrder, tSCH1.ItemNumber, tOS.StaffID as UtilityTech, Sum(tSCH1.CasesProduced) as CasesProduced, Sum(tSCH1.RunTime) as RunTime From
/* WO#194 Del Start
				(Select Facility, DefaultPkgLine as PackagingLine, StartTime, Operator, ShopOrder, ItemNumber, Sum(CasesProduced) as CasesProduced, Sum(Isnull(DateDiff(second,StartTime,StopTime),0)) /3600.00 as RunTime
					From tblSessionControlHst 
					Where Facility = @vchFacility 
					AND Convert(varchar(8),ShiftProductionDate,112) + Cast(OverrideShiftNo as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
					-- exclude Set-up Operators in Mississauga 
					AND ((Facility not in (Select facility from dbo.tblFacility where Region = '01') OR Len(Rtrim(Operator)) > 3) ) 
					Group By Facility, DefaultPkgLine, StartTime, Operator, ShopOrder, ItemNumber) tSCH1
WO#194 Del Stop */
-- WO#359 Add Start
			-- WO#27470 (Select tSCH1.Facility, tSCH1.PackagingLine, tSCH1.Operator, tSCH1.ShopOrder, tSCH1.ItemNumber
					(Select tSCH1.Facility, tSCH1.ShiftProductionDate, tSCH1.PackagingLine, tSCH1.Operator, tSCH1.ShopOrder, tSCH1.ItemNumber			-- WO#27470
						,tOS.StaffID as UtilityTech, SUM(tSCH1.CasesProduced) as CasesProduced, SUM(tSCH1.RunTime) as RunTime 
-- WO#359 Add Stop
					  FROM #temp_SessionControlHst tSCH1
						Left Outer Join [tblOperationStaffing] tOS
						On tSCH1.Facility = tOS.Facility and tSCH1.PackagingLine = tOS.PackagingLine And tSCH1.[StartTime] = tOS.[StartTime]
					  Where tOS.Rrn is not null 
					  Group By tSCH1.Facility, tSCH1.ShiftProductionDate, tSCH1.PackagingLine, tSCH1.Operator, tSCH1.ShopOrder, tSCH1.ItemNumber, tOS.StaffID) tSCH		-- WO#27470
					  -- WO#27470  Group By tSCH1.Facility, tSCH1.PackagingLine, tSCH1.Operator, tSCH1.ShopOrder, tSCH1.ItemNumber, tOS.StaffID) tSCH
				LEFT OUTER JOIN
-- WO#359 Add Start
		-- Add Pallet Adjustment
/* WO#27470 Del Start
			(SELECT Facility, DefaultPkgLine, Operator, ShopOrder, SUM(AdjustedQty) as AdjustedQty
				FROM tfnSessionControlHstSummary ('WithAdjByOpr',@vchFacility,NULL,NULL,NULL,NULL,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift)
				GROUP BY Facility, DefaultPkgLine, ShopOrder, Operator) tADJ
		  ON tSCH.Facility = tADJ.Facility and tSCH.PackagingLine = tADJ.DefaultPkgLine and tSCH.Operator = tADJ.Operator And tSCH.ShopOrder = tADJ.ShopOrder
WO#27470 Del Stop */
-- WO#27470 Add Start
				(SELECT Facility, ShiftProductionDate, DefaultPkgLine, Operator, ShopOrder, SUM(AdjustedQty) as AdjustedQty
					FROM tfnSessionControlHstSummary ('WithAdjByOpr',@vchFacility,NULL,NULL,NULL,NULL,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift)
					GROUP BY Facility, ShiftProductionDate, DefaultPkgLine, ShopOrder, Operator) tADJ
				ON tSCH.Facility = tADJ.Facility 
				and tSCH.ShiftProductionDate = tADJ.ShiftProductionDate
				and tSCH.PackagingLine = tADJ.DefaultPkgLine 
				and tSCH.Operator = tADJ.Operator 
				And tSCH.ShopOrder = tADJ.ShopOrder
-- WO#27470 Add Stop
		  -- WO#27470 LEFT OUTER JOIN	
-- WO#359 Add Stop

/* WO#194 Del Start
			(Select Facility,Operator,MachineID,ShopOrder, Sum(AdjustedQty) as AdjustedQty 
				From tblPalletAdjustment
				Where Facility = @vchFacility 
				Group by Facility,Operator,MachineID,ShopOrder) tADJ
		  #temp_PalletAdjustment tADJ
		  On tSCH.Facility = tADJ.Facility and tSCH.Operator = tADJ.Operator and tSCH.PackagingLine = tADJ.MachineID And tSCH.ShopOrder = tADJ.ShopOrder
		  Left Outer Join
			(Select Facility, Operator, MachineID, ShopOrder, Sum(DateDiff(minute,DownTimeBegin,DownTimeEnd)) as DownTime
				From dbo.tblDownTimeLog
				Where Facility = @vchFacility and InActive = 0 
					  AND Convert(varchar(8),ShiftProductionDate,112) + Cast(Shift as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
				Group by Facility,Operator,MachineID,ShopOrder) tDTL
WO#194 Del Stop */

/* WO#27470 Del Start
-- WO#194 Add Start 
		  (SELECT tDL.Facility, tDL.Operator, tDL.MachineID, tDL.ShopOrder, Sum(tDL.MaxDownTime)/60.00 as DownTime
		  FROM
			(Select tD.Facility, tD.Operator, tD.MachineID, tD.ShopOrder, tD.DownTimeBegin
				,MAX(DateDiff(second, tD.DownTimeBegin, tD.DownTimeEnd)) as MaxDownTime
				From #temp_DownTimeLog tD
				Group by tD.Facility, tD.Operator, tD.MachineID, tD.ShopOrder, tD.DownTimeBegin) tDL
		  Group by tDL.Facility, tDL.Operator, tDL.MachineID, tDL.ShopOrder) tDTL
-- WO#194 Add End
		  On tSCH.Facility = tDTL.Facility and tSCH.Operator = tDTL.Operator and tSCH.PackagingLine = tDTL.MachineID And tSCH.ShopOrder = tDTL.ShopOrder
		  LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_ID
			ON tSCH.Facility = tSMER_ID.Facility AND tSCH.ItemNumber = tSMER_ID.ItemNumber AND tSCH.PackagingLine = tSMER_ID.MachineID
		  LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_WC
			ON tSCH.Facility = tSMER_WC.Facility AND tSCH.ItemNumber = tSMER_WC.ItemNumber AND CAST(SUBSTRING(tSCH.PackagingLine,1,4) as int) = tSMER_WC.WorkCenter and tSMER_WC.MachineID = ''	
WO#27470 Del Stop */
-- WO#359 Add Start 
			  LEFT OUTER JOIN tblItemMaster tIM
				ON tSCH.Facility = tIM.Facility AND tSCH.ItemNumber = tIM.ItemNumber
-- WO#359 Add Stop 
-- WO#27470 ADD Start
			  OUTER APPLY [dbo].[tfnStdMachineEfficiencyRate] (tSCH.Facility, tSCH.ItemNumber, tSCH.PackagingLine, LEFT(tSCH.PackagingLine,4), tSCH.ShiftProductionDate) as tfSMER			-- WO#27470
			  Group by tSCH.Facility, tSCH.Operator, tSCH.UtilityTech, tSCH.PackagingLine, tSCH.ShopOrder
			) tUSSCH
			LEFT OUTER JOIN
			  (SELECT tDL.Facility, tDL.Operator, tDL.MachineID, tDL.ShopOrder, Sum(tDL.MaxDownTime)/60.00 as DownTime
			  FROM
				(Select tD.Facility, tD.Operator, tD.MachineID, tD.ShopOrder, tD.DownTimeBegin
					,MAX(DateDiff(second, tD.DownTimeBegin, tD.DownTimeEnd)) as MaxDownTime
					From #temp_DownTimeLog tD
					Group by tD.Facility, tD.Operator, tD.MachineID, tD.ShopOrder, tD.DownTimeBegin) tDL
			  Group by tDL.Facility, tDL.Operator, tDL.MachineID, tDL.ShopOrder) tDTL
			  ON tUSSCH.Facility = tDTL.Facility and tUSSCH.Operator = tDTL.Operator and tUSSCH.PackagingLine = tDTL.MachineID And tUSSCH.ShopOrder = tDTL.ShopOrder

			GROUP by tUSSCH.Facility , tUSSCH.UtilityTech,  tUSSCH.PackagingLine , tUSSCH.ShopOrder
-- WO#27470 ADD Stop
		),

		---------------------------------------------------------------
		-- Calculate Overall Line performance for each operated line --
		---------------------------------------------------------------

		-- Scheduled Adher = Qty Produced/Qty Scheduled * 100%
		-- Scheduled Attainment = (Qty Scheduled - Absolute(Qty Scheduled - Qty Produced))/Qty Scheduled * 100%
		--						= 1 - (Absolute(Qty Scheduled - Qty Produced) / Qty Scheduled) * 100%
		-- Line Efficiency = Case Produced / Acutal run time / StdUnitPerHr(from table) * 100% 
		-- Budget Line Effeciency = Line Effeciency / Std Line Efficiency(from table)  * 100% 
		SCH_CTE
		AS
		(
-- WO#194 Select tSCH.Facility, tSCH.PackagingLine, tSCH.ShopOrder, (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0)) as CasesProduced, tSCH.RunTime, tSOH.OrderQty,
-- WO#194	CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0))/ (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit,
-- WO#359 Select tSCH.Facility, tSCH.PackagingLine, tSCH.ShopOrder, tSCH.CasesProduced, tSCH.RunTime, tSOH.OrderQty,	-- WO#194
-- WO#359	CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else tSCH.CasesProduced / (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit,	-- WO#194
/* WO#27470 DEL Start
-- WO#359 Add Start
		SELECT tSCH.Facility, tSCH.PackagingLine, tSCH.ShopOrder, (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0)) AS CasesProduced, tSCH.RunTime, tSOH.OrderQty,
			CASE WHEN tSMER_ID.MachineHours IS NULL AND tSMER_WC.MachineHours IS NULL THEN 0 ELSE (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0))/ (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) END AS StdMachineHrEarnedInUnit,
-- WO#359 Add Stop
			ISNULL(tSMER_ID.StdWorkCenterEfficiency,ISNULL(tSMER_WC.StdWorkCenterEfficiency,0)) as StdWCEfficiency
			,ROUND(ISNULL((tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0)) * tIM.LabelWeight * tIM.PackagesPerSaleableUnit * tIM.SaleableUnitPerCase * (CASE WHEN UPPER(tIM.LabelWeightUOM) = 'GM' Then @decGMtoLB ELSE 1 END), 0),0) AS PoundProd  -- WO#359 
		  From 
WO#27470 DEL Stop */
-- WO#27470 Add Start
		 SELECT tSSCH.Facility, tSSCH.PackagingLine, tSSCH.ShopOrder
			,SUM(tSSCH.CasesProduced) as CasesProduced
			,SUM(tSSCH.RunTime) as RunTIme
			,SUM(tSSCH.StdMachineHrEarnedInUnit) as StdMachineHrEarnedInUnit
			,AVG(tSSCH.StdWCEfficiency) as StdWCEfficiency
			,SUM(tSSCH.PoundProd) as PoundProd
			,SUM(tSOH.OrderQty) as OrderQty
		 FROM
		 (
			SELECT tSCH.Facility, tSCH.PackagingLine, tSCH.ShopOrder
				,SUM(tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0)) AS CasesProduced
				,SUM(tSCH.RunTime) as RunTIme
				,SUM(CASE WHEN ISNULL(tfSMER.MachineHours,0) = 0 THEN 0 ELSE tSCH.CasesProduced/ (POWER(10,ISNULL(tfSMER.BasisCode,0)) / tfSMER.MachineHours) END) AS StdMachineHrEarnedInUnit		-- WO27470
				,AVG(ISNULL(tfSMER.StdWorkCenterEfficiency,0)) as StdWCEfficiency																												-- WO27470
				,SUM(ROUND(ISNULL((tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0)) * tIM.LabelWeight * tIM.PackagesPerSaleableUnit * tIM.SaleableUnitPerCase * (CASE WHEN UPPER(tIM.LabelWeightUOM) = 'GM' Then @decGMtoLB ELSE 1 END), 0),0)) AS PoundProd  -- WO#359 
			FROM
-- WO#27470 Add Stop
/* WO#194 Del Start
			(Select Facility, DefaultPkgLine as PackagingLine, ShopOrder, ItemNumber, Sum(CasesProduced)   as CasesProduced, Sum(Isnull(DateDiff(second,StartTime,StopTime),0)) /3600.00 as RunTime
			  From tblSessionControlHst
			  Where Facility = @vchFacility 
				AND Convert(varchar(8),ShiftProductionDate,112) + Cast(OverrideShiftNo as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
				AND ((Facility not in (Select facility from dbo.tblFacility where Region = '01') OR Len(Rtrim(Operator)) > 3) ) 
			Group By Facility, DefaultPkgLine, ShopOrder, ItemNumber) tSCH
WO#194 Del Stop */
-- WO#194 Add Start 
			-- WO27470	(Select tSCHst.Facility, tSCHst.PackagingLine, tSCHst.ShopOrder, tSCHst.ItemNumber, 
			 (Select tSCHst.Facility, tSCHst.ShiftProductionDate, PackagingLine, tSCHst.ShopOrder, tSCHst.ItemNumber,					-- WO27470
				Sum(tSCHst.CasesProduced) as CasesProduced, Sum(RunTime) as RunTime
			  From #temp_SessionControlHst tSCHst	
			  Group By tSCHst.Facility, tSCHst.ShiftProductionDate, tSCHst.PackagingLine, tSCHst.ShopOrder, tSCHst.ItemNumber) tSCH		-- WO27470
			-- WO27470	Group By tSCHst.Facility, tSCHst.PackagingLine, tSCHst.ShopOrder, tSCHst.ItemNumber) tSCH
-- WO#194 Add Stop
		  -- WO27470 Left Outer Join dbo.tblShopOrderHst as tSOH
		  -- WO27470 On tSCH.Facility = tSOH.Facility and tSCH.ShopOrder = tSOH.ShopOrder
-- WO#359 Add Start
		-- Add Pallet Adjustment
		  LEFT OUTER JOIN	
/* WO#27470 Del Start
			(SELECT Facility, DefaultPkgLine, ShopOrder, SUM(AdjustedQty) as AdjustedQty
				FROM tfnSessionControlHstSummary ('WithAdjByLineSO',@vchFacility,NULL,NULL,NULL,NULL,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift)
				GROUP BY Facility, DefaultPkgLine, ShopOrder, Operator) tADJ
		  ON tSCH.Facility = tADJ.Facility AND tSCH.PackagingLine = tADJ.DefaultPkgLine AND tSCH.ShopOrder = tADJ.ShopOrder
WO#27470 Del Stop */
-- WO#27470 Add Start
			(SELECT Facility, ShiftProductionDate, DefaultPkgLine, ShopOrder, SUM(AdjustedQty) as AdjustedQty
				FROM tfnSessionControlHstSummary ('WithAdjByLineSO',@vchFacility,NULL,NULL,NULL,NULL,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift)
				GROUP BY Facility, ShiftProductionDate, DefaultPkgLine, ShopOrder, Operator) tADJ
				ON tSCH.Facility = tADJ.Facility 
				AND tSCH.ShiftProductionDate = tADJ.ShiftProductionDate 
				AND tSCH.PackagingLine = tADJ.DefaultPkgLine
				AND tSCH.ShopOrder = tADJ.ShopOrder
-- WO#27470 Add Stop
-- WO#359 Add Stop
/* WO#194 Del Start
		  Left Outer Join
		  (Select Facility,MachineID,ShopOrder, Sum(AdjustedQty) as AdjustedQty 
				From tblPalletAdjustment 
				Where Facility = @vchFacility 
				Group by Facility,MachineID,ShopOrder) tADJ
		  #temp_PalletAdjustment tADJ
		  On tSCH.Facility = tADJ.Facility and tSCH.PackagingLine = tADJ.MachineID And tSCH.ShopOrder = tADJ.ShopOrder
WO#194 Del Stop */
/* WO#27470 Del Start
		  LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_ID
			ON tSCH.Facility = tSMER_ID.Facility AND tSCH.ItemNumber = tSMER_ID.ItemNumber AND tSCH.PackagingLine = tSMER_ID.MachineID
		  LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_WC
			ON tSCH.Facility = tSMER_WC.Facility AND tSCH.ItemNumber = tSMER_WC.ItemNumber AND CAST(SUBSTRING(tSCH.PackagingLine,1,4) as int) = tSMER_WC.WorkCenter and tSMER_WC.MachineID = ''	
WO#27470 Del Stop */
		  OUTER APPLY [dbo].[tfnStdMachineEfficiencyRate] (tSCH.Facility, tSCH.ItemNumber, tSCH.PackagingLine, LEFT(tSCH.PackagingLine,4), tSCH.ShiftProductionDate) as tfSMER			-- WO27470
-- WO#359 Add Start 
		  LEFT Outer Join tblItemMaster tIM
		  ON tSCH.Facility = tIM.Facility AND tSCH.ItemNumber = tIM.ItemNumber
-- WO#359 Add Stop 
		  GROUP BY tSCH.Facility, tSCH.PackagingLine, tSCH.ShopOrder
		 ) tSSCH
-- WO27470 ADD Start
		  LEFT OUTER JOIN dbo.tblShopOrderHst as tSOH
		 On tSSCH.Facility = tSOH.Facility and tSSCH.ShopOrder = tSOH.ShopOrder
-- WO27470 ADD Stop
		 GROUP BY tSSCH.Facility, tSSCH.PackagingLine, tSSCH.ShopOrder
		)

		Select tOSCH.*, tLSCH.LineRunTime, tLSCH.LineQtySched, tLSCH.LineQtyProd, tLSCH.LineSchedAdher, tLSCH.LineSchedAttmnt, tLSCH.LineEff, 
				tLSCH.LineBudgetEff, tLSCH.LineStdWCEfficiency, tDTL.DownTime as LineDownTime, isnull(tPS.UtilityTechName,'** Unknown **') as UtilityTechName, 
				tE.Description
				,tLSCH.LinePoundProd	--WO#359
			 From 
			(Select USCH_CTE.Facility, USCH_CTE.UtilityTech, USCH_CTE.PackagingLine, Sum(USCH_CTE.RunTime) as RunTime, 
				Sum(USCH_CTE.CasesProduced) as QtyProd,
				Case When Sum(USCH_CTE.RunTime) = 0 Then 0 Else Sum(USCH_CTE.StdMachineHrEarnedInUnit) / Sum(USCH_CTE.RunTime)  End as OLineEff,
				Case When (Sum(USCH_CTE.RunTime) = 0 OR AVG(USCH_CTE.StdWCEfficiency) = 0) Then 0 Else Sum(USCH_CTE.StdMachineHrEarnedInUnit) / Sum(USCH_CTE.RunTime) / AVG(USCH_CTE.StdWCEfficiency) End as BudgetEff,
				AVG(USCH_CTE.StdWCEfficiency) as StdWCEfficiency,
				Sum(USCH_CTE.StdMachineHrEarnedInUnit) as StdMachineHrEarnedInUnit,
				Sum(USCH_CTE.DownTime) as DownTime
				,Sum(USCH_CTE.PoundProd) as PoundProd	--WO#359
			From USCH_CTE
			Group by USCH_CTE.Facility, USCH_CTE.UtilityTech, USCH_CTE.PackagingLine) tOSCH
		Left outer join
			(Select tSCH_CTE.Facility, tSCH_CTE.PackagingLine, Sum(tSCH_CTE.RunTime) as LineRunTime, Sum(tSCH_CTE.OrderQty) as LineQtySched, Sum(tSCH_CTE.CasesProduced) as LineQtyProd,
				Case When Sum(tSCH_CTE.OrderQty) = 0 Then 0 Else Sum(tSCH_CTE.CasesProduced)/Sum(tSCH_CTE.OrderQty) End as LineSchedAdher,
				Case When Sum(tSCH_CTE.OrderQty) = 0 Then 0 Else (1 - (Sum(ABS(tSCH_CTE.OrderQty - tSCH_CTE.CasesProduced)) / Sum(tSCH_CTE.OrderQty))) End as LineSchedAttmnt,
				Case When Sum(tSCH_CTE.RunTime) = 0 Then 0 Else Sum(tSCH_CTE.StdMachineHrEarnedInUnit) / Sum(tSCH_CTE.RunTime) End as LineEff,
				Case When (Sum(tSCH_CTE.RunTime) = 0 OR AVG(tSCH_CTE.StdWCEfficiency) = 0) Then 0 Else Sum(tSCH_CTE.StdMachineHrEarnedInUnit) / Sum(tSCH_CTE.RunTime) / AVG(tSCH_CTE.StdWCEfficiency) End as LineBudgetEff,
				AVG(tSCH_CTE.StdWCEfficiency) as LineStdWCEfficiency
				,Sum(tSCH_CTE.PoundProd) as LinePoundProd	--WO#359
			From SCH_CTE as tSCH_CTE
			Group by tSCH_CTE.Facility, tSCH_CTE.PackagingLine) tLSCH
			On tOSCH.Facility = tLSCH.Facility and tOSCH.PackagingLine = tLSCH.PackagingLine
		Left Outer Join (Select Distinct StaffID, FirstName + ' ' + LastName as UtilityTechName 
			From tblPlantStaff
			Where Facility = @vchFacility ) tPS 
			ON tOSCH.UtilityTech = tPS.StaffID
		Left Outer Join tblEquipment tE
			On tOSCH.Facility = tE.Facility and tOSCH.PackagingLine = tE.EquipmentID
		Left Outer Join
		  (Select Facility, MachineID, Sum(MaxDownTime)/60.00 as DownTime
			  From	
/* WO#194 Del Start
				(Select facility, MachineID, DownTimeBegin, max(DateDiff(Second,DownTimeBegin,DownTimeEnd)) as MaxDownTime
				From dbo.tblDownTimeLog
				Where Facility = @vchFacility and InActive = 0 
					  AND Convert(varchar(8),ShiftProductionDate,112) + Cast(Shift as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
				Group by facility,MachineID,DownTimeBegin) tDL
WO#194 Del Stop */
-- WO#194 Add Start
				(Select tD.Facility, tD.MachineID, tD.DownTimeBegin, MAX(DateDiff(Second,tD.DownTimeBegin,tD.DownTimeEnd)) as MaxDownTime
					From #temp_DownTimeLog tD
					Group by tD.Facility,tD.MachineID,tD.DownTimeBegin) tDL
-- WO#194 Add Stop
			  Group by Facility,MachineID) tDTL
		  On tOSCH.Facility = tDTL.Facility and tOSCH.PackagingLine = tDTL.MachineID    	   

		IF object_id('tempdb..#temp_SessionControlHst') is not null
			DROP TABLE #temp_SessionControlHst

-- WO#194 IF object_id('tempdb..#temp_PalletAdjustment') is not null
-- WO#194	DROP TABLE #temp_PalletAdjustment

		IF object_id('tempdb..#temp_DownTimeLog') is not null
			DROP TABLE #temp_DownTimeLog

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		BEGIN TRY
		IF object_id('tempdb..#temp_SessionControlHst') is not null
			DROP TABLE #temp_SessionControlHst
		END TRY
		BEGIN CATCH
		END CATCH

/* WO#194 Del Start
		BEGIN TRY
		IF object_id('tempdb..#temp_PalletAdjustment') is not null
			DROP TABLE #temp_PalletAdjustment
		END TRY
		BEGIN CATCH
		END CATCH
WO#194 Del Stop */

		BEGIN TRY
		IF object_id('tempdb..#temp_DownTimeLog') is not null
			DROP TABLE #temp_DownTimeLog
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

