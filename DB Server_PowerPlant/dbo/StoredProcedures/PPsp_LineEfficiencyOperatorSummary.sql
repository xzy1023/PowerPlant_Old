

-- =============================================
-- Author:		Bong Lee
-- Create date: Oct 08,2009
-- Description:	Line Efficency Operator Summary
-- Mod.			Date			Author
-- WO#103		Apr.20 2010		Bong Lee
-- Description: Use Entered Line Paid Hour to override actual run time if exist,
--				Down Time %: (Down Time / Paid Run Time) * 100 %, 
--				Dow Time % accurary: (DT%)/1-Eff)
--				Pound Produced: Cases Produced * Packages Per Saleable Unit * Saleable Unit Per Case
--								If the UOM is gram,convert it to pound with 1 gm = (0.0352736 / 16) pound = 0.002204624 pound
--								(453.592 GM / pound from BPCS - SYS03-Table ID:CVTTOLBS)
-- WO#194:		Sep. 9, 2010	Bong Lee	
-- Description:	If the time frame for the expected shift crosses mid-night
--				and the given time is prior to mid-night, the Shift Production 
--				Date will be the given time plus one day. Before this change the 	
--				Shift Production Date for this case was same as the given time but 
--				if the given time after mid-night, the result was given time 
--				minus one day.
--				Do not include pallet adjustment to cases produced
-- WO#359:		Aug. 25, 2011	Bong Lee	
-- Description:	Use standard table functions to replace some of the select statements
--				Apply pallet adjustment to quantity produced in the Overall and Individual sessions
-- WO#27470:		Aug. 27, 2019	Bong Lee
-- Description:	Routing rates are required to base on effective date
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_LineEfficiencyOperatorSummary]
	@vchFacility varchar(3),
	@dteFromProdDate as DateTime,
	@intFromShift as tinyint,
	@dteToProdDate as DateTime,
	@intToShift as tinyint
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
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
-- WO#359 ADD Start 
		SELECT * INTO #temp_SessionControlHst
		FROM dbo.tfnSessionControlHstSummary('WithAdjByOpr',@vchFacility,NULL,NULL,NULL,NULL,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift) 
-- WO#359 ADD Stop 
-- WO#359 Del Start 
--		SELECT tSCH.*, tMPH.PaidHours INTO #temp_SessionControlHst
--			FROM
--			(SELECT tSCHst.Facility, tSCHst.DefaultPkgLine, tSCHst.ShopOrder,tSCHst.Operator, tSCHst.ShiftProductionDate, tSCHst.OverrideShiftNo, tSCHst.ItemNumber,
--					SUM(CasesProduced) as CasesProduced,
--					SUM(IsNull(DateDiff(Second,StartTime,StopTime),0)) / 3600.00 as ActRunTime
--				FROM tblSessionControlHst tSCHst
--					Left Outer Join @tblComputerConfig tCC
--					ON tSCHst.Facility = tCC.Facility AND tSCHst.DefaultPkgLine =  tCC.PackagingLine
--					Left Outer Join tblshift tS
--					ON tSCHst.Facility = tS.Facility AND tSCHst.OverrideShiftNo = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
--				WHERE tSCHst.Facility = @vchFacility and Convert(varchar(8),ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1)) 	-- WO#194
--					AND ((tSCHst.Facility not in (SELECT Facility FROM dbo.tblFacility WHERE Region = '01') OR Len(Rtrim(Operator)) > 3) )
--				GROUP BY tSCHst.Facility, tSCHst.DefaultPkgLine, tSCHst.ShopOrder,tSCHst.Operator, tSCHst.ShiftProductionDate, tSCHst.OverrideShiftNo, tSCHst.ItemNumber
--			) tSCH
--		LEFT OUTER JOIN tblMachinePaidHours tMPH
--		ON tSCH.Facility = tMPH.Facility AND tSCH.DefaultPkgLine = tMPH.MachineID AND
--		   tSCH.ShopOrder = tMPH.ShopOrder AND tSCH.Operator = tMPH.Operator AND
--		   tSCH.ShiftProductionDate = tMPH.ShiftProductionDate AND tSCH.OverrideShiftNo = tMPH.ShiftNo
-- WO#359 Del Stop	
/* WO#194 DEL Start
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
WO#194 DEL Stop */

		-- Create temporary table to hold the Down Time Log for using in multiple times 
		IF object_id('tempdb..#temp_DownTimeLog') is not null
			DROP TABLE #temp_DownTimeLog

--WO#27470		SELECT tD.*
		SELECT tDTL.*
			INTO #temp_DownTimeLog
--WO#27470		FROM dbo.tblDownTimeLog tD
--WO#359  Left Outer Join @tblComputerConfig tCC
--WO#27470 ADD Start
		FROM
		(SELECT tD.* 
		,DateDiff(Second, tD.DownTimeBegin, tD.DownTimeEnd) as DTDuration
		,Rank() OVER(PARTITION BY tD.MachineID, tD.Operator, tD.DownTimeBegin 
								ORDER BY tD.MachineID, tD.Operator,	tD.DownTimeBegin, DateDiff(minute, tD.DownTimeBegin, tD.DownTimeEnd) DESC, tD.CreationDate) AS SeqNo
--WO#27470 ADD Stop
		FROM dbo.tblDownTimeLog tD
		Left Outer Join dbo.vwLineWorkShiftType	tCC		--WO#359
		ON tD.Facility = tCC.Facility AND tD.MachineID =  tCC.PackagingLine
		Left Outer Join tblshift tS
		ON tD.Facility = tS.Facility AND tD.Shift = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
		WHERE tD.Facility = @vchFacility and InActive = 0 
			  AND Convert(varchar(8),tD.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
		) tDTL						--WO#27470
		WHERE SeqNo = 1				--WO#27470
-- WO#194 Add Stop

		-- Get weight information
		IF object_id('tempdb..#temp_cteWgt') is not null
			DROP TABLE #temp_cteWgt

--		;With cteWgt 
--		AS (
			Select Facility, PackagingLine, Operator, AVG(AvgWgt) as AvgWgt,AVG(StdevWgt) as AvgStdevWgt, AVG(TargetWeight) as TargetWeight,
					AVG(MinWeight) as MinWeight, AVG(MaxWeight) as MaxWeight 
				INTO #temp_cteWgt
				From (
				Select tWL.Facility, tWL.PackagingLine, tSCH.Operator, tWL.LabelWeight, Avg(ActualWeight) as AvgWgt,
					Round(stdev(ActualWeight),1) as stdevWgt, Avg(TargetWeight) as TargetWeight,
					Avg(MinWeight) as MinWeight, Avg(MaxWeight) as MaxWeight
				From dbo.tblWeightLog tWL
				Inner join 
					dbo.tfnSessionControlHstDetail(NULL,@vchFacility,NULL,NULL,NULL,NULL,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift) tSCH	-- WO#359
-- WO#359 Del Start 
--					(Select tSCHst.Facility, tSCHst.DefaultPkgLine, ShopOrder, Operator, tSCHst.StartTIme
--						From tblSessionControlHst tSCHst
---- WO#194 Add Start 
--						Left Outer Join @tblComputerConfig tCC
--						ON tSCHst.Facility = tCC.Facility AND tSCHst.DefaultPkgLine =  tCC.PackagingLine
--						Left Outer Join tblshift tS
--						ON tSCHst.Facility = tS.Facility AND tSCHst.OverrideShiftNo = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
---- WO#194 Add End
--						Where tSCHst.Facility = @vchFacility and 
---- WO#194				Convert(varchar(8),tSCHst.ShiftProductionDate,112) + Cast(tSCHst.OverrideShiftNo as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1)) AND
--						Convert(varchar(8),tSCHst.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1)) AND	-- WO#194
--						(tSCHst.Facility not in (Select facility from dbo.tblFacility where Region = '01') OR Len(Rtrim(tSCHst.Operator)) > 3) 
--						Group by tSCHst.Facility, tSCHst.DefaultPkgLine, tSCHst.ShopOrder, tSCHst.Operator, tSCHst.StartTIme
--					 ) tSCH
-- WO#359 Del Stop 
				On tWL.Facility = tSCH.Facility AND tWL.ShopOrder = tSCH.ShopOrder AND
					tWL.PackagingLine = tSCH.DefaultPkgLine AND tWL.ShopOrderStartTIme = tSCH.StartTIme
				Where tWL.WeightSource <> 2	-- exlcluded tareweight records	
				Group by tWL.Facility, tWL.PackagingLine, tSCH.Operator, tWL.LabelWeight
				) tWgt
				Group By Facility, PackagingLine, Operator
--		),
		-- StdUnitPerHr = Routing rate * (2 ** Base code)
		-- StdMachineHrEarnedInUnit = Cases Produced / StdUnitPerHr
		-- Units Produced = Cases Produced * Saleable Unit Per Case
		-- Actual Line Efficiency % = Case Produced / Acutal run time / StdUnitPerHr * 100%
		-- Or
		-- Actual Line Efficiency % = StdMachineHrEarnedInUnit / Acutal run time * 100%

		--------------------------------------------
		-- Calculate Operator Performance by Line --
		--------------------------------------------
	;With OSCH_CTE
		AS
		(
-- WO#194 Select tSCH.Facility, tSCH.Operator, tSCH.PackagingLine, tSCH.ShopOrder, tSCH.ItemNumber, (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0)) as CasesProduced, tSCH.ActRunTime, tSCH.PaidHours as PaidHours, 
-- WO#194 CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0))/ (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit,
		  Select tSCH.Facility, tSCH.Operator, tSCH.PackagingLine, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.CasesProduced, tSCH.ActRunTime, tSCH.PaidHours as PaidHours,	-- WO#194
			-- WO#27470	CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else tSCH.CasesProduced/ (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit, -- WO#194
			-- WO#27470	ISNULL(tSMER_ID.StdWorkCenterEfficiency,ISNULL(tSMER_WC.StdWorkCenterEfficiency,0)) as StdWCEfficiency
			CASE When ISNULL(tfSMER.MachineHours,0)= 0 Then 0 Else tSCH.CasesProduced/ (POWER(10,ISNULL(tfSMER.BasisCode,0)) / tfSMER.MachineHours) End As StdMachineHrEarnedInUnit	-- WO#27470
			,ISNULL(tfSMER.StdWorkCenterEfficiency,0) as StdWCEfficiency																												-- WO#27470
		  From 	
-- WO#194 Add Start
			-- WO#27470 (SELECT tSCH#.Facility, tSCH#.DefaultPkgLine as PackagingLine, tSCH#.ShopOrder, tSCH#.ItemNumber, tSCH#.Operator, 
			(SELECT tSCH#.Facility, tSCH#.ShiftProductionDate, tSCH#.DefaultPkgLine as PackagingLine, tSCH#.ShopOrder, tSCH#.ItemNumber, tSCH#.Operator,								-- WO#27470
-- WO#359			SUM(tSCH#.CasesProduced) as CasesProduced, Round(SUM(tSCH#.ActRunTime),2) as ActRunTime, Round(SUM(ISNULL(tSCH#.PaidHours, tSCH#.ActRunTime)) ,2) as PaidHours
					SUM(tSCH#.CasesProduced + ISNULL(tSCH#.AdjustedQty,0)) as CasesProduced, Round(SUM(tSCH#.ActRunTime),2) as ActRunTime, Round(SUM(ISNULL(tSCH#.PaidRunTime, tSCH#.ActRunTime)) ,2) as PaidHours	-- WO#359
				FROM #Temp_SessionControlHst tSCH#
				-- WO#27470	GROUP BY tSCH#.Facility, tSCH#.DefaultPkgLine, tSCH#.ShopOrder, tSCH#.ItemNumber, tSCH#.Operator
				GROUP BY tSCH#.Facility, tSCH#.ShiftProductionDate, tSCH#.DefaultPkgLine, tSCH#.ShopOrder, tSCH#.ItemNumber, tSCH#.Operator												-- WO#27470
			) tSCH
-- WO#194 Add Stop
/* WO#194 Del Start
			(SELECT tSCH1.Facility, tSCH1.Operator, tSCH1.DefaultPkgLine as PackagingLine, tSCH1.ShopOrder, tSCH1.ItemNumber,  SUM(tSCH1.CasesProduced) as CasesProduced, SUM(ActRunTime) as ActRunTime, Round(SUM(ISNULL(tMPH.PaidHours, tSCH1.ActRunTime)) ,2) as PaidHours
				FROM (
				SELECT tSCH1.Facility, DefaultPkgLine, ShopOrder, ItemNumber, Operator, ShiftProductionDate, OverrideShiftNo, sum(CasesProduced) as CasesProduced,
					Round(Sum(Isnull(DateDiff(Second,StartTime,StopTime),0)) / 3600.00, 2) as ActRunTime
				FROM tblSessionControlHst tSCH1
				WHERE tSCH1.Facility = @vchFacility and Convert(varchar(8),ShiftProductionDate,112) + Cast(OverrideShiftNo as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
					AND ((tSCH1.Facility not in (SELECT facility FROM dbo.tblFacility WHERE Region = '01') OR Len(Rtrim(Operator)) > 3) )
				GROUP BY tSCH1.Facility, DefaultPkgLine,ShopOrder,ItemNumber,Operator, ShiftProductionDate, OverrideShiftNo) tSCH1
				Left Outer Join tblMachinePaidHours tMPH
				ON tSCH1.Facility = tMPH.Facility AND tSCH1.Operator = tMPH.Operator AND tSCH1.DefaultPkgLine = tMPH.MachineID AND tSCH1.ShopOrder = tMPH.ShopOrder 
					AND tSCH1.ShiftProductionDate = tMPH.ShiftProductionDate AND tSCH1.OverrideShiftNo = tMPH.ShiftNo				
				GROUP BY tSCH1.Facility, tSCH1.DefaultPkgLine, tSCH1.ShopOrder, tSCH1.ItemNumber, tSCH1.Operator) tSCH
			  Left Outer Join
				(Select Facility,Operator,MachineID,ShopOrder, Sum(AdjustedQty) as AdjustedQty 
					From tblPalletAdjustment
					Where Facility = @vchFacility 
					Group by Facility,Operator,MachineID,ShopOrder) tADJ
			  On tSCH.Facility = tADJ.Facility and tSCH.Operator = tADJ.Operator and tSCH.PackagingLine = tADJ.MachineID And tSCH.ShopOrder = tADJ.ShopOrder
WO#194 Del Stop */
/* WO#27470 Del Start
			  LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_ID
				ON tSCH.Facility = tSMER_ID.Facility AND tSCH.ItemNumber = tSMER_ID.ItemNumber AND tSCH.PackagingLine = tSMER_ID.MachineID
			  LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_WC
				ON tSCH.Facility = tSMER_WC.Facility AND tSCH.ItemNumber = tSMER_WC.ItemNumber AND CAST(SUBSTRING(tSCH.PackagingLine,1,4) as int) = tSMER_WC.WorkCenter and tSMER_WC.MachineID = ''	
WO#27470	Del Stop */
			  OUTER APPLY [dbo].[tfnStdMachineEfficiencyRate] (tSCH.Facility, tSCH.ItemNumber, tSCH.PackagingLine, LEFT(tSCH.PackagingLine,4), tSCH.ShiftProductionDate) as tfSMER			-- WO#27470
		),

		--------------------------------------------------
		-- Calculate overall operated lines performance --
		--------------------------------------------------

		-- Scheduled Adher = Qty Produced/Qty Scheduled * 100%
		-- Scheduled Attainment = (Qty Scheduled - Absolute(Qty Scheduled - Qty Produced))/Qty Scheduled * 100%
		--						= 1 - (Absolute(Qty Scheduled - Qty Produced) / Qty Scheduled) * 100%
		-- Line Efficiency = Case Produced / Acutal run time / StdUnitPerHr(from table) * 100% 
		-- Budget Line Effeciency = Line Effeciency / Std Line Efficiency(from table)  * 100% 
		SCH_CTE
		AS
		(
--WO#194  Select tSCH.Facility, tSCH.PackagingLine, tSCH.ShopOrder, tSCH.ItemNumber, (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0)) as CasesProduced, tSCH.ActRunTime, tSCH.PaidHours as PaidHours, tSOH.OrderQty,
--WO#194	CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0))/ (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit,
		  -- WO#27470 Select tSCH.Facility, tSCH.PackagingLine, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.CasesProduced, tSCH.ActRunTime, tSCH.PaidHours as PaidHours, tSOH.OrderQty,	--WO#194
			-- WO#27470	CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else tSCH.CasesProduced / (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit, --WO#194
			-- WO#27470	ISNULL(tSMER_ID.StdWorkCenterEfficiency,ISNULL(tSMER_WC.StdWorkCenterEfficiency,0)) as StdWCEfficiency
-- WO#27470 ADD Start
 		Select tSSCH.Facility, tSSCH.PackagingLine, tSSCH.ShopOrder, tSSCH.ItemNumber, SUM(tSSCH.CasesProduced) as CasesProduced, SUM(tSSCH.ActRunTime) as ActRunTime						
		,SUM(tSSCH.PaidHours) as PaidHours, SUM(tSSCH.StdMachineHrEarnedInUnit) as StdMachineHrEarnedInUnit ,AVG(tSSCH.StdWCEfficiency)	as StdWCEfficiency 
		,SUM(tSOH.OrderQty) as OrderQty								
		FROM																																															
			(
			  Select tSCH.Facility, tSCH.PackagingLine, tSCH.ShopOrder, tSCH.ItemNumber
			  ,SUM(tSCH.CasesProduced) as CasesProduced, SUM(tSCH.ActRunTime) as ActRunTime, SUM(tSCH.PaidHours) as PaidHours									
			  ,SUM(CASE When ISNULL(tfSMER.MachineHours,0)= 0 Then 0 Else tSCH.CasesProduced/ (POWER(10,ISNULL(tfSMER.BasisCode,0)) / tfSMER.MachineHours) End) As StdMachineHrEarnedInUnit		
			  ,AVG(ISNULL(tfSMER.StdWorkCenterEfficiency,0)) as StdWCEfficiency																												
-- WO#27470 ADD Stop
			  From 	
-- WO#194 Add Start
			-- WO#27470	(SELECT tSCH#.Facility, tSCH#.DefaultPkgLine as PackagingLine, tSCH#.ShopOrder, tSCH#.ItemNumber,
				(SELECT tSCH#.Facility, tSCH#.ShiftProductionDate, tSCH#.DefaultPkgLine as PackagingLine, tSCH#.ShopOrder, tSCH#.ItemNumber,	-- WO#27470
-- WO#359			SUM(tSCH#.CasesProduced) as CasesProduced, Round(SUM(tSCH#.ActRunTime),2) as ActRunTime, Round(SUM(ISNULL(tSCH#.PaidHours, tSCH#.ActRunTime)) ,2) as PaidHours
					SUM(tSCH#.CasesProduced + ISNULL(tSCH#.AdjustedQty,0)) as CasesProduced, Round(SUM(tSCH#.ActRunTime),2) as ActRunTime, Round(SUM(ISNULL(tSCH#.PaidRunTime, tSCH#.ActRunTime)) ,2) as PaidHours  -- WO#359
				FROM #Temp_SessionControlHst tSCH#
				-- WO#27470	GROUP BY tSCH#.Facility, tSCH#.DefaultPkgLine, tSCH#.ShopOrder, tSCH#.ItemNumber
				GROUP BY tSCH#.Facility, tSCH#.ShiftProductionDate, tSCH#.DefaultPkgLine, tSCH#.ShopOrder, tSCH#.ItemNumber		-- WO#27470
			) tSCH
-- WO#194 Add Stop
/* WO#194 Del Start
			(Select tPLH.Facility, tPLH.DefaultPkgLine as PackagingLine, tPLH.ShopOrder, tPLH.ItemNumber, Sum(tPLH.CasesProduced) as CasesProduced, 
			  Round(SUM(tPLH.ActRunTime),2) as ActRunTime, Round(SUM(ISNULL(tMPH.PaidHours, tPLH.ActRunTime)) ,2) as PaidHours	-- WO#194
			  Sum(Isnull(DateDiff(second,StartTime,StopTime),0)) /3600.00 as ActRunTime,
			  Sum(ISNULL(tMPH.PaidHours * 3600.00, Isnull(DateDiff(second,tPLH.StartTime,tPLH.StopTime),0))) /3600.00 as PaidHours
			From tblSessionControlHst tPLH
				Left Outer Join tblMachinePaidHours tMPH
				On tPLH.Facility = tMPH.Facility AND tPLH.Operator = tMPH.Operator AND tPLH.DefaultPkgLine = tMPH.MachineID AND tPLH.ShopOrder = tMPH.ShopOrder 
					AND tPLH.ShiftProductionDate = tMPH.ShiftProductionDate AND tPLH.OverrideShiftNo = tMPH.ShiftNo
				Where tPLH.Facility = @vchFacility 
				AND Convert(varchar(8),tPLH.ShiftProductionDate,112) + Cast(tPLH.OverrideShiftNo as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
				AND ((tPLH.Facility not in (Select facility from dbo.tblFacility where Region = '01') OR Len(Rtrim(tPLH.Operator)) > 3) ) 
			Group By tPLH.Facility, tPLH.DefaultPkgLine, tPLH.ShopOrder, tPLH.ItemNumber
			) tSCH
WO#194 Del Stop */
		  -- WO#27470	Left Outer Join dbo.tblShopOrderHst as tSOH
		  -- WO#27470	On tSCH.Facility = tSOH.Facility and tSCH.ShopOrder = tSOH.ShopOrder
/* WO#194 Del Start
		  LEFT OUTER JOIN
		  (Select Facility,MachineID,ShopOrder, Sum(AdjustedQty) as AdjustedQty 
				From tblPalletAdjustment 
				Where Facility = @vchFacility 
				Group by Facility,MachineID,ShopOrder) tADJ
		  On tSCH.Facility = tADJ.Facility and tSCH.PackagingLine = tADJ.MachineID And tSCH.ShopOrder = tADJ.ShopOrder
WO#194 Del Stop */
/* WO#27470 Del Start
		  LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_ID
			ON tSCH.Facility = tSMER_ID.Facility AND tSCH.ItemNumber = tSMER_ID.ItemNumber AND tSCH.PackagingLine = tSMER_ID.MachineID
		  LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_WC
			ON tSCH.Facility = tSMER_WC.Facility AND tSCH.ItemNumber = tSMER_WC.ItemNumber AND CAST(SUBSTRING(tSCH.PackagingLine,1,4) as int) = tSMER_WC.WorkCenter and tSMER_WC.MachineID = ''	
WO#27470 Del Stop */
			OUTER APPLY [dbo].[tfnStdMachineEfficiencyRate] (tSCH.Facility, tSCH.ItemNumber, tSCH.PackagingLine, LEFT(tSCH.PackagingLine,4), tSCH.ShiftProductionDate) as tfSMER			-- WO#27470
--WO#27470 )
			GROUP BY tSCH.Facility, tSCH.PackagingLine, tSCH.ShopOrder, tSCH.ItemNumber				-- WO#27470

			) tSSCH
			Left Outer Join dbo.tblShopOrderHst as tSOH												-- WO#27470
			On tSSCH.Facility = tSOH.Facility and tSSCH.ShopOrder = tSOH.ShopOrder					-- WO#27470
			GROUP BY tSSCH.Facility, tSSCH.PackagingLine, tSSCH.ShopOrder, tSSCH.ItemNumber			-- WO#27470
		)																							-- WO#27470

		Select tOSCH.*,
				tLSCH.LineActRunTime, tLSCH.LinePaidHours, tLSCH.LineQtySched, tLSCH.LineQtyProd, tLSCH.LinePoundProd,
				tLSCH.LineSchedAdher, tLSCH.LineSchedAttmnt, tLSCH.LineEff, 
				tLSCH.LineBudgetEff, tLSCH.LineStdWCEfficiency, tOPRDTL.OprDownTime as DownTime, tDTL.DownTime as LineDownTime, isnull(tPS.OperatorName,'** Unknown **') as OperatorName, 
				tE.Description, cteWgt.AvgStdevWgt
			 From 
			(Select OSCH_CTE.Facility, OSCH_CTE.Operator, OSCH_CTE.PackagingLine,  Sum(OSCH_CTE.ActRunTime) as ActRunTime, Sum(OSCH_CTE.PaidHours) as PaidHours, 
				Sum(OSCH_CTE.CasesProduced) as QtyProd,
-- WO#194		Sum(Round(OSCH_CTE.CasesProduced * tIM.LabelWeight * tIM.PackagesPerSaleableUnit * tIM.SaleableUnitPerCase * (Case When upper(tIM.LabelWeightUOM) = 'GM' Then 0.0022046 Else 1 END), 0)) as PoundProd,
-- WO#359 		Round(Sum(ISNULL(OSCH_CTE.CasesProduced * tIM.LabelWeight * tIM.PackagesPerSaleableUnit * tIM.SaleableUnitPerCase * (Case When upper(tIM.LabelWeightUOM) = 'GM' Then 0.002204624 Else 1 END), 0)),0) as PoundProd,  -- WO#194 
				Round(Sum(ISNULL(OSCH_CTE.CasesProduced * tIM.LabelWeight * tIM.PackagesPerSaleableUnit * tIM.SaleableUnitPerCase * (Case When upper(tIM.LabelWeightUOM) = 'GM' Then @decGMtoLB Else 1 END), 0)),0) as PoundProd,  -- WO#359 
				Case When Sum(OSCH_CTE.PaidHours) = 0 Then 0 Else Sum(OSCH_CTE.StdMachineHrEarnedInUnit) / Sum(OSCH_CTE.PaidHours)  End as OLineEff,
				Case When (Sum(OSCH_CTE.PaidHours) = 0 OR AVG(OSCH_CTE.StdWCEfficiency) = 0) Then 0 Else Sum(OSCH_CTE.StdMachineHrEarnedInUnit) / Sum(OSCH_CTE.PaidHours) / AVG(OSCH_CTE.StdWCEfficiency) End as BudgetEff,
				AVG(OSCH_CTE.StdWCEfficiency) as StdWCEfficiency,
				Sum(OSCH_CTE.StdMachineHrEarnedInUnit) as StdMachineHrEarnedInUnit
			From OSCH_CTE
			Left Outer Join tblItemMaster tIM
				On OSCH_CTE.Facility = tIM.Facility and OSCH_CTE.ItemNumber = tIM.ItemNumber
			Group by OSCH_CTE.Facility, OSCH_CTE.Operator, OSCH_CTE.PackagingLine) tOSCH
		Left outer join
			(Select tSCH_CTE.Facility, tSCH_CTE.PackagingLine, Sum(tSCH_CTE.ActRunTime) as LineActRunTime, Sum(tSCH_CTE.PaidHours) as LinePaidHours, Sum(tSCH_CTE.OrderQty) as LineQtySched, Sum(tSCH_CTE.CasesProduced) as LineQtyProd,
-- WO#359 		Round(Sum(tSCH_CTE.CasesProduced * tIM.LabelWeight * tIM.PackagesPerSaleableUnit * tIM.SaleableUnitPerCase * (Case When upper(tIM.LabelWeightUOM) = 'GM' Then 0.002204624 Else 1 END)), 0) as LinePoundProd, 
				Round(Sum(tSCH_CTE.CasesProduced * tIM.LabelWeight * tIM.PackagesPerSaleableUnit * tIM.SaleableUnitPerCase * (Case When upper(tIM.LabelWeightUOM) = 'GM' Then @decGMtoLB Else 1 END)), 0) as LinePoundProd, -- WO#359 
				Case When Sum(tSCH_CTE.OrderQty) = 0 Then 0 Else Sum(tSCH_CTE.CasesProduced)/Sum(tSCH_CTE.OrderQty) End as LineSchedAdher,
				Case When Sum(tSCH_CTE.OrderQty) = 0 Then 0 Else (1 - (Sum(ABS(tSCH_CTE.OrderQty - tSCH_CTE.CasesProduced)) / Sum(tSCH_CTE.OrderQty))) End as LineSchedAttmnt,
				Case When Sum(tSCH_CTE.PaidHours) = 0 Then 0 Else Sum(tSCH_CTE.StdMachineHrEarnedInUnit) / Sum(tSCH_CTE.PaidHours) End as LineEff,
				Case When (Sum(tSCH_CTE.PaidHours) = 0 OR AVG(tSCH_CTE.StdWCEfficiency) = 0) Then 0 Else Sum(tSCH_CTE.StdMachineHrEarnedInUnit) / Sum(tSCH_CTE.PaidHours) / AVG(tSCH_CTE.StdWCEfficiency) End as LineBudgetEff,
				AVG(tSCH_CTE.StdWCEfficiency) as LineStdWCEfficiency
			From SCH_CTE as tSCH_CTE
			Left Outer Join tblItemMaster tIM
				On tSCH_CTE.Facility = tIM.Facility and tSCH_CTE.ItemNumber = tIM.ItemNumber
			Group by tSCH_CTE.Facility, tSCH_CTE.PackagingLine) tLSCH
			On tOSCH.Facility = tLSCH.Facility and tOSCH.PackagingLine = tLSCH.PackagingLine
		Left Outer Join (Select Distinct StaffID, FirstName + ' ' + LastName as OperatorName 
			From tblPlantStaff
			Where Facility = @vchFacility ) tPS 
			ON tOSCH.Operator = tPS.StaffID
		Left Outer Join tblEquipment tE
			On tOSCH.Facility = tE.Facility and tOSCH.PackagingLine = tE.EquipmentID
		Left Outer Join
-- WO#194	(Select Facility, MachineID, Sum(MaxDownTime)/60.00 as DownTime
			-- WO#27470	(Select Facility, MachineID, Sum(MaxDownTime)/60.00 as DownTime -- WO#194
			(Select Facility, MachineID, Sum(DTDuration)/60.00 as DownTime				-- WO#27470
			  From	
-- WO#194		(Select facility, MachineID, DownTimeBegin, max(DateDiff(Second,DownTimeBegin,DownTimeEnd)) as MaxDownTime
-- WO#194			From dbo.tblDownTimeLog
-- WO#194				Where Facility = @vchFacility and InActive = 0 
-- WO#194					  AND Convert(varchar(8),ShiftProductionDate,112) + Cast(Shift as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
-- WO#194			Group by facility,MachineID,DownTimeBegin) tDL
-- WO#194 Add Start 
				 #temp_DownTimeLog			--WO#27470
/* WO#27470 DEL Start
			(Select tDTL.Facility, tDTL.MachineID, tDTL.DownTimeBegin, MAX(DateDiff(Second, tDTL.DownTimeBegin, tDTL.DownTimeEnd)) as MaxDownTime
				From #temp_DownTimeLog tDTL
				Group by tDTL.facility, tDTL.MachineID, tDTL.DownTimeBegin) tDL
WO#27470 DEL Stop */
-- WO#194 Add End
			  Group by Facility,MachineID) tDTL
			On tOSCH.Facility = tDTL.Facility and tOSCH.PackagingLine = tDTL.MachineID 
		Left Outer Join
			-- WO#27470	(Select Facility, Operator, MachineID, Sum(MaxDownTime)/60.00 as OprDownTime
			(Select Facility, Operator, MachineID, Sum(DTDuration)/60.00 as OprDownTime
			  From
-- WO#194		(Select facility, Operator, MachineID, DownTimeBegin, max(DateDiff(Second,DownTimeBegin,DownTimeEnd)) as MaxDownTime	
				
-- WO#194		From dbo.tblDownTimeLog tDTL
-- WO#194		Where Facility = @vchFacility and InActive = 0 
-- WO#194			AND Convert(varchar(8),ShiftProductionDate,112) + Cast(Shift as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
-- WO#194		Group by Facility, Operator, MachineID, DownTimeBegin) tOPRDL
-- WO#194 Add Start 
				#temp_DownTimeLog		--WO#27470
/* WO#27470 DEL Start
				(Select tDTL.facility, tDTL.Operator, tDTL.MachineID, tDTL.DownTimeBegin, MAX(DateDiff(Second, tDTL.DownTimeBegin, tDTL.DownTimeEnd)) as MaxDownTime
				From #temp_DownTimeLog tDTL
				Group by tDTL.Facility, tDTL.Operator, tDTL.MachineID, tDTL.DownTimeBegin) tOPRDL
WO#27470 DEL Stop */
-- WO#194 Add End
			Group by Facility, Operator, MachineID) tOPRDTL
			On tOSCH.Facility = tOPRDTL.Facility and tOSCH.Operator = tOPRDTL.Operator and tOSCH.PackagingLine = tOPRDTL.MachineID
		Left Outer Join #temp_cteWgt cteWgt  
			On tOSCH.Facility = cteWgt.Facility and tOSCH.PackagingLine=cteWgt.PackagingLine And tOSCH.Operator = cteWgt.Operator

		IF object_id('tempdb..#temp_SessionControlHst') is not null
			DROP TABLE #temp_SessionControlHst

		IF object_id('tempdb..#temp_cteWgt') is not null
			DROP TABLE #temp_cteWgt

/* WO#194 Del Start
		IF object_id('tempdb..#temp_PalletAdjustment') is not null
			DROP TABLE #temp_PalletAdjustment

		IF object_id('tempdb..#temp_DownTimeLog') is not null
			DROP TABLE #temp_DownTimeLog
WO#194 Del Stop */

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

		BEGIN TRY
		IF object_id('tempdb..#temp_cteWgt') is not null
			DROP TABLE #temp_cteWgt
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

