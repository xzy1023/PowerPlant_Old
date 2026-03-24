


-- =============================================
-- Author:		Bong Lee
-- Create date: 2010-7-21
-- Description:	WO# 228 - Labor Efficiency Summary 
-- WO#359:		Aug. 31, 2011	Bong Lee	
-- Description:	Use standard table functions to replace some of the select statements
--				Apply pallet adjustment to quantity produced in the Overall and Individual sessions
-- Ticket 5138: % Efficiency is different in Work Center Efficiency report vs. Labor and Machine Efficiency
-- WO#27470:	Aug. 27, 2019	Bong Lee
-- Description:	Routing rates are required to base on effective date.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_LaborEfficiency]
	@vchAction as varchar(50) = NULL,
	@dteFromDate as datetime,
	@dteToDate as datetime,
	@vchFacility as varchar(3),
	@vchHourType as varchar(15) = 'Labor'
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @chrShiftLess as Char
	
	BEGIN TRY
		IF Object_id('tempdb..#tblKPH') IS NOT NULL
			DROP TABLE #tblKPH

		CREATE TABLE #tblKPH(
			FULLNAME	[varchar](64),
			PERSONNUM	[varchar](15),
			WorkCenter	[varchar](50),
			Shift		[varchar](50),
			PAYCODENAME	[varchar](50),
			ApplyDate	[datetime],
			LaborHoursWorked	[decimal](10,2),
			ShiftSequence tinyint
		) ON [PRIMARY]

		-- Get Labour hours from Kronos's HR system
		INSERT INTO #tblKPH
		EXEC	[PPsp_KronosPaidHours_Sel] 
				'DtlByWC_Shift_Date',
				@dteFromDate ,
				@dteToDate ,
				@vchFacility 

--select * from #tblKPH

		IF @vchAction = 'LaborEfficiency'
		BEGIN
			SELECT @chrShiftLess = Value1 FROM  tblControl WHERE Facility = @vchFacility AND [Key] = 'Shiftless' and SubKey = 'LaborEfficiency'

			-- The difference between @vchHourType = 'Labor' and 'Production' is 
			-- 'Labor' uses Kronos table, #tblKPH as Primary table to join
			-- 'Production' uses Power Plant table cteSCH as Primary table to join
			-- so only difference is 'Production' uses LEFT OUTER JOIN and 'Labor' uses RIGHT OUTER JOIN
			IF @vchHourType = 'Production'
			BEGIN

				;with cteSCH AS
				(
-- WO#359 Add Start
					SELECT tSCH.Facility, tSCH.DefaultPkgLine, CASE WHEN @chrShiftLess = 'Y' THEN 0 ELSE tSCH.ShiftNo END as Shift,
						tSCH.ShiftProductionDate, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator, SUM(tSCH.CasesProduced + tSCH.AdjustedQty) as CasesProduced, 
						SUM(ActRunTime) as ActRunTime, SUM(PaidRunTime) as PaidRunTime
					FROM dbo.tfnSessionControlHstSummary('WithAdjByOpr',@vchFacility,NULL,NULL,NULL,NULL,@dteFromDate,NULL,@dteToDate,NULL) tSCH
					GROUP BY tSCH.Facility, tSCH.DefaultPkgLine, CASE WHEN @chrShiftLess = 'Y' THEN 0 ELSE tSCH.ShiftNo END, tSCH.ShiftProductionDate, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator
-- WO#359 Add Stop
/* WO#359 Del Start
					SELECT tSCH.Facility, tSCH.DefaultPkgLine, CASE WHEN @chrShiftLess = 'Y' THEN 0 ELSE tSCH.Shift END as Shift,
						tSCH.ShiftProductionDate, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator, SUM(tSCH.CasesProduced) as CasesProduced, SUM(ActRunTime) as ActRunTime,
						ROUND(SUM(ISNULL(tMPH.PaidHours, tSCH.ActRunTime)) ,2) as PaidRunTime 
					FROM (
						SELECT Facility, DefaultPkgLine, ShopOrder, ItemNumber, Operator, ShiftProductionDate, OverrideShiftNo as Shift, sum(CasesProduced) as CasesProduced,
							Round(Sum(Isnull(DateDiff(Second,StartTime,StopTime),0)) / 3600.00, 2) as ActRunTime
						FROM tblSessionControlHst 
						WHERE Facility = @vchFacility 
							AND ShiftProductionDate Between @dteFromDate and @dteToDate
							AND ((Facility not in (SELECT facility FROM dbo.tblFacility WHERE Region = '01') OR Len(Rtrim(Operator)) > 3) )
						GROUP BY Facility, DefaultPkgLine, ShopOrder, ItemNumber, Operator, ShiftProductionDate, OverrideShiftNo) tSCH
					Left Outer Join tblMachinePaidHours tMPH
					ON tSCH.Facility = tMPH.Facility 
						AND tSCH.Operator = tMPH.Operator 
						AND tSCH.DefaultPkgLine = tMPH.MachineID 
						AND tSCH.ShopOrder = tMPH.ShopOrder 
						AND tSCH.ShiftProductionDate = tMPH.ShiftProductionDate 
						AND tSCH.Shift = tMPH.ShiftNo	
					GROUP BY tSCH.Facility, tSCH.DefaultPkgLine, CASE WHEN @chrShiftLess = 'Y' THEN 0 ELSE tSCH.Shift END, tSCH.ShiftProductionDate, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator
WO#359 Del Stop */
				)

				SELECT 	tLES.WorkCenter, tLES.Shift, tLES.ShiftProductionDate, tLES.CasesProduced, 
					tLES.PaidRunTime, tLES.PoundProduced, tLES.UnitsProduced, tLES.StdLaborHrEarned,
					ISNULL(tLES.StdMachineHrEarned,0) as StdMachineHrEarned,
					tLES.stdLabor,tLES.StdLbsPerLaborHr, ISNULL(tLES.StdWCBgtEfficiency,0) as StdWCBgtEfficiency,
					tLHW.LaborHoursWorked as ActLaborWorkHr, tS.ShiftSequence
					,tLES.StdHr	-- Ticket 5138
				 FROM ( 
					SELECT WorkCenter, Shift, ShiftProductionDate, 
						SUM(CasesProduced) as CasesProduced, SUM(PaidRunTime) as PaidRunTime, 
						SUM(PoundProduced) as PoundProduced, 
						SUM(UnitsProduced) as UnitsProduced, SUM(StdLaborHrEarned) as StdLaborHrEarned,
						SUM(StdMachineHrEarnedInUnit) as StdMachineHrEarned,
						SUM(stdLabor) as stdLabor,
						CASE WHEN Sum(RunOperators * StdMachineHrEarnedInUnit * NetWeight) = 0 THEN 0 ELSE SUM(PoundProduced) /SUM(RunOperators * StdMachineHrEarnedInUnit * NetWeight) END as StdLbsPerLaborHr,
						--Ticket 5138  Max(StdWCBgtEfficiency) as StdWCBgtEfficiency
						Avg(StdWCBgtEfficiency) as StdWCBgtEfficiency	--Ticket 5138
						,SUM(CASE WHEN StdWCBgtEfficiency = 0 THEN 0 ELSE StdMachineHrEarnedInUnit/StdWCBgtEfficiency END) as StdHr --Ticket 5138
					FROM (
						SELECT tEQ.WorkCenter, cteSCH.Shift, cteSCH.ShiftProductionDate, cteSCH.CasesProduced as CasesProduced,
							cteSCH.ActRunTime, cteSCH.PaidRunTime,
							Round(tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit / (Case When tIM.labelweightUOM = 'GM' Then 453.5924 Else 1 End),3) as NetWeight, 
							Round((cteSCH.CasesProduced) * Round(tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit/ (Case When tIM.labelweightUOM = 'GM' Then 453.5924 Else 1 End),3),0) as PoundProduced, 
/* WO#27470 DEL Start
							CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0)) END as StdUnitPerHr,
							CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else (cteSCH.CasesProduced)/ (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit,
							CASE When (tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null) OR (tSMER_ID.StdWorkCenterEfficiency = 0) or (tSMER_ID.StdWorkCenterEfficiency is NULL AND tSMER_WC.StdWorkCenterEfficiency = 0) Then 0 Else (cteSCH.CasesProduced)/ (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) * ISNULL(tSMER_ID.RunOperators,ISNULL(tSMER_WC.RunOperators,1)) / (ISNULL(tSMER_ID.StdWorkCenterEfficiency,ISNULL(tSMER_WC.StdWorkCenterEfficiency,0))) End As StdLaborHrEarned,
							ISNULL(tSMER_ID.StdWorkCenterEfficiency,ISNULL(tSMER_WC.StdWorkCenterEfficiency,0)) as StdWCBGTEfficiency,
							cteSCH.PaidRunTime * ISNULL(tSMER_ID.RunOperators,ISNULL(tSMER_WC.RunOperators,1)) as StdLabor,
							ISNULL(tSMER_ID.RunOperators,ISNULL(tSMER_WC.RunOperators,1)) as RunOperators,
WO#27470 DEL Stop */
--WO#27470 ADD Start
							CASE When ISNULL(tfSMER.MachineHours,0)= 0 Then 0 Else POWER(10,ISNULL(tfSMER.BasisCode,0)) / tfSMER.MachineHours END  as StdUnitPerHr,
							CASE When ISNULL(tfSMER.MachineHours,0)= 0 Then 0 Else cteSCH.CasesProduced / (POWER(10,ISNULL(tfSMER.BasisCode,0)) / tfSMER.MachineHours) End As StdMachineHrEarnedInUnit,
							CASE When ISNULL(tfSMER.MachineHours,0) = 0 OR tfSMER.StdWorkCenterEfficiency = 0 Then 0 Else cteSCH.CasesProduced/ (POWER(10,ISNULL(tfSMER.BasisCode,0)) / ISNULL(tfSMER.MachineHours,0)) * ISNULL(tfSMER.RunOperators,1) / ISNULL(tfSMER.StdWorkCenterEfficiency,0) End As StdLaborHrEarned,
							CASE When tfSMER.StdWorkCenterEfficiency = 0 THEN NULL ELSE tfSMER.StdWorkCenterEfficiency END as StdWCBgtEfficiency,	
							cteSCH.PaidRunTime * ISNULL(tfSMER.RunOperators,1) as StdLabor,
							ISNULL(tfSMER.RunOperators,1) as RunOperators,
--WO#27470 ADD STop
							Round((cteSCH.CasesProduced) * ISNULL(tIM.SaleableUnitPerCase,0) * ISNULL(tIM.PackagesPerSaleableUnit,0) ,0)  as UnitsProduced
						FROM cteSCH
						LEFT OUTER JOIN tblItemMaster tIM
						On cteSCH.Facility = tIM.Facility and cteSCH.ItemNumber = tIM.ItemNumber
/* WO#27470 DEL Start
						LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_ID
						ON cteSCH.Facility = tSMER_ID.Facility AND tIM.ItemNumber = tSMER_ID.ItemNumber AND cteSCH.DefaultPkgLine = tSMER_ID.MachineID
						LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_WC
						ON cteSCH.Facility = tSMER_WC.Facility AND tIM.ItemNumber = tSMER_WC.ItemNumber AND CAST(SUBSTRING(cteSCH.DefaultPkgLine,1,4) as int) = tSMER_WC.WorkCenter and tSMER_WC.MachineID = ''
WO#27470 DEL Stop */
						LEFT OUTER JOIN tblEquipment tEQ
						ON cteSCH.Facility = tEQ.Facility and cteSCH.DefaultPkgLine = tEQ.EquipmentID
						OUTER APPLY [dbo].[tfnStdMachineEfficiencyRate] (cteSCH.Facility, cteSCH.ItemNumber, cteSCH.DefaultPkgLine, LEFT(cteSCH.DefaultPkgLine,4), cteSCH.ShiftProductionDate) as tfSMER		--WO#27470 
--						LEFT OUTER JOIN (SELECT Facility, MachineID, ShopOrder, Operator, Sum(AdjustedQty) as AdjustedQty 
--							From tblPalletAdjustment
--							Where Facility = @vchFacility 
--							Group by Facility, MachineID, ShopOrder, Operator) tADJ
--							On cteSCH.Facility = tADJ.Facility and cteSCH.DefaultPkgLine=tADJ.MachineID And cteSCH.ShopOrder = tADJ.ShopOrder And cteSCH.Operator = tADJ.Operator
					) tLPFM
					GROUP BY WorkCenter, Shift, ShiftProductionDate
				) tLES
				-- "Based on Production Hours" uses LEFT OUTER JOIN	
				LEFT OUTER JOIN (
					SELECT WorkCenter, Shift, ApplyDate, Sum(LaborHoursWorked) as LaborHoursWorked
						FROM #tblKPH 
						GROUP BY WorkCenter, Shift, ApplyDate) tLHW
				ON tLES.WorkCenter = tLHW.WorkCenter 
					AND tLES.Shift = tLHW.Shift
					AND tLES.ShiftProductionDate = tLHW.ApplyDate
				LEFT OUTER JOIN (SELECT * FROM tblShift WHERE Facility = @vchFacility and WorkGroup = 'P') tS
				ON tLES.Shift = tS.Shift
			END
			ELSE
			BEGIN
				;with cteSCH AS
				(
-- WO#359 Add Start
					SELECT tSCH.Facility, tSCH.DefaultPkgLine, CASE WHEN @chrShiftLess = 'Y' THEN 0 ELSE tSCH.ShiftNo END as Shift,
						tSCH.ShiftProductionDate, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator, SUM(tSCH.CasesProduced + tSCH.AdjustedQty) as CasesProduced, 
						SUM(ActRunTime) as ActRunTime, SUM(PaidRunTime) as PaidRunTime
					FROM dbo.tfnSessionControlHstSummary('WithAdjByOpr',@vchFacility,NULL,NULL,NULL,NULL,@dteFromDate,NULL,@dteToDate,NULL) tSCH
					GROUP BY tSCH.Facility, tSCH.DefaultPkgLine, CASE WHEN @chrShiftLess = 'Y' THEN 0 ELSE tSCH.ShiftNo END, tSCH.ShiftProductionDate, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator
-- WO#359 Add Stop
/* WO#359 Del Start
					SELECT tSCH.Facility, tSCH.DefaultPkgLine, CASE WHEN @chrShiftLess = 'Y' THEN 0 ELSE tSCH.Shift END as Shift, tSCH.ShiftProductionDate, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator, SUM(tSCH.CasesProduced) as CasesProduced, SUM(ActRunTime) as ActRunTime,
						ROUND(SUM(ISNULL(tMPH.PaidHours, tSCH.ActRunTime)) ,2) as PaidRunTime 
					FROM (
						SELECT Facility, DefaultPkgLine, ShopOrder, ItemNumber, Operator, ShiftProductionDate, OverrideShiftNo as Shift, sum(CasesProduced) as CasesProduced,
							Round(Sum(Isnull(DateDiff(Second,StartTime,StopTime),0)) / 3600.00, 2) as ActRunTime
						FROM tblsessioncontrolhst 
						WHERE Facility = @vchFacility 
							AND ShiftProductionDate Between @dteFromDate and @dteToDate
							AND ((Facility not in (SELECT facility FROM dbo.tblFacility WHERE Region = '01') OR Len(Rtrim(Operator)) > 3) )
						GROUP BY Facility, DefaultPkgLine, ShopOrder, ItemNumber, Operator, ShiftProductionDate, OverrideShiftNo) tSCH
					Left Outer Join tblMachinePaidHours tMPH
					ON tSCH.Facility = tMPH.Facility 
						AND tSCH.Operator = tMPH.Operator 
						AND tSCH.DefaultPkgLine = tMPH.MachineID 
						AND tSCH.ShopOrder = tMPH.ShopOrder 
						AND tSCH.ShiftProductionDate = tMPH.ShiftProductionDate 
						AND tSCH.Shift = tMPH.ShiftNo	
					GROUP BY tSCH.Facility, tSCH.DefaultPkgLine, CASE WHEN @chrShiftLess = 'Y' THEN 0 ELSE tSCH.Shift END, tSCH.ShiftProductionDate, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator
WO#359 Del Stop */
				)

				SELECT tLHW.WorkCenter, tLHW.Shift, tLHW.ApplyDate as ShiftProductionDate, tLHW.ShiftSequence,
				    tLES.CasesProduced, tLES.PaidRunTime, tLES.PoundProduced, tLES.UnitsProduced, tLES.StdLaborHrEarned,
					ISNULL(tLES.StdMachineHrEarned,0) as StdMachineHrEarned, tLES.stdLabor, tLES.StdLbsPerLaborHr,
					ISNULL(tLES.StdWCBgtEfficiency,0) as StdWCBgtEfficiency, tLHW.LaborHoursWorked as ActLaborWorkHr
					,tLES.StdHr	-- Ticket 5138
				 FROM ( 
					SELECT WorkCenter, Shift, ShiftProductionDate, 
						SUM(CasesProduced) as CasesProduced, SUM(PaidRunTime) as PaidRunTime, 
						SUM(PoundProduced) as PoundProduced, 
						SUM(UnitsProduced) as UnitsProduced, SUM(StdLaborHrEarned) as StdLaborHrEarned,
						SUM(StdMachineHrEarnedInUnit) as StdMachineHrEarned,
						SUM(stdLabor) as stdLabor,
						CASE WHEN SUM(RunOperators * StdMachineHrEarnedInUnit * NetWeight) = 0 THEN 0 ELSE SUM(PoundProduced) /SUM(RunOperators * StdMachineHrEarnedInUnit * NetWeight) END as StdLbsPerLaborHr,
						--Ticket 5138	Max(StdWCBgtEfficiency) as StdWCBgtEfficiency
						Avg(StdWCBgtEfficiency) as StdWCBgtEfficiency	--Ticket 5138
						,SUM(CASE WHEN StdWCBgtEfficiency = 0 THEN 0 ELSE StdMachineHrEarnedInUnit/StdWCBgtEfficiency END) as StdHr --Ticket 5138
					FROM (
						SELECT tEQ.WorkCenter, cteSCH.Shift, cteSCH.ShiftProductionDate, cteSCH.CasesProduced as CasesProduced,
							cteSCH.ActRunTime, cteSCH.PaidRunTime,
							Round(tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit / (Case When tIM.labelweightUOM = 'GM' Then 453.5924 Else 1 End),3) as NetWeight, 
							Round((cteSCH.CasesProduced) * Round(tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit/ (Case When tIM.labelweightUOM = 'GM' Then 453.5924 Else 1 End),3),0) as PoundProduced, 
/* WO#27470 DEL Start
							CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0)) END as StdUnitPerHr,
							CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else (cteSCH.CasesProduced)/ (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit,
							CASE When (tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null) OR (tSMER_ID.StdWorkCenterEfficiency = 0) or (tSMER_ID.StdWorkCenterEfficiency is NULL AND tSMER_WC.StdWorkCenterEfficiency = 0) Then 0 Else (cteSCH.CasesProduced)/ (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) * ISNULL(tSMER_ID.RunOperators,ISNULL(tSMER_WC.RunOperators,1)) / (ISNULL(tSMER_ID.StdWorkCenterEfficiency,ISNULL(tSMER_WC.StdWorkCenterEfficiency,0))) End As StdLaborHrEarned,
							ISNULL(tSMER_ID.StdWorkCenterEfficiency,ISNULL(tSMER_WC.StdWorkCenterEfficiency,0)) as StdWCBgtEfficiency,
							cteSCH.PaidRunTime * ISNULL(tSMER_ID.RunOperators,ISNULL(tSMER_WC.RunOperators,1)) as StdLabor,
							ISNULL(tSMER_ID.RunOperators,ISNULL(tSMER_WC.RunOperators,1)) as RunOperators,
WO#27470 DEL Stop */
--WO#27470 ADD Start
							CASE When ISNULL(tfSMER.MachineHours,0)= 0 Then 0 Else POWER(10,ISNULL(tfSMER.BasisCode,0)) / tfSMER.MachineHours END  as StdUnitPerHr,
							CASE When ISNULL(tfSMER.MachineHours,0)= 0 Then 0 Else cteSCH.CasesProduced / (POWER(10,ISNULL(tfSMER.BasisCode,0)) / tfSMER.MachineHours) End As StdMachineHrEarnedInUnit,
							CASE When ISNULL(tfSMER.MachineHours,0) = 0 OR tfSMER.StdWorkCenterEfficiency = 0 Then 0 Else cteSCH.CasesProduced/ (POWER(10,ISNULL(tfSMER.BasisCode,0)) / ISNULL(tfSMER.MachineHours,0)) * ISNULL(tfSMER.RunOperators,1) / ISNULL(tfSMER.StdWorkCenterEfficiency,0) End As StdLaborHrEarned,
							CASE When tfSMER.StdWorkCenterEfficiency = 0 THEN NULL ELSE tfSMER.StdWorkCenterEfficiency END as StdWCBgtEfficiency,	
							cteSCH.PaidRunTime * ISNULL(tfSMER.RunOperators,1) as StdLabor,
							ISNULL(tfSMER.RunOperators,1) as RunOperators,
--WO#27470 ADD Stop
							Round((cteSCH.CasesProduced) * ISNULL(tIM.SaleableUnitPerCase,0) * ISNULL(tIM.PackagesPerSaleableUnit,0) ,0)  as UnitsProduced
						FROM cteSCH
						LEFT OUTER JOIN tblItemMaster tIM
						On cteSCH.Facility = tIM.Facility and cteSCH.ItemNumber = tIM.ItemNumber
/* WO#27470 DEL Start
						LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_ID
						ON cteSCH.Facility = tSMER_ID.Facility AND tIM.ItemNumber = tSMER_ID.ItemNumber AND cteSCH.DefaultPkgLine = tSMER_ID.MachineID
						LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_WC
						ON cteSCH.Facility = tSMER_WC.Facility AND tIM.ItemNumber = tSMER_WC.ItemNumber AND CAST(SUBSTRING(cteSCH.DefaultPkgLine,1,4) as int) = tSMER_WC.WorkCenter and tSMER_WC.MachineID = ''
WO#27470 DEL Stop */
						LEFT OUTER JOIN tblEquipment tEQ
						ON cteSCH.Facility = tEQ.Facility and cteSCH.DefaultPkgLine = tEQ.EquipmentID
						OUTER APPLY [dbo].[tfnStdMachineEfficiencyRate] (cteSCH.Facility, cteSCH.ItemNumber, cteSCH.DefaultPkgLine, LEFT(cteSCH.DefaultPkgLine,4), cteSCH.ShiftProductionDate) as tfSMER		--WO#27470 
--						LEFT OUTER JOIN (SELECT Facility, MachineID, ShopOrder, Operator, Sum(AdjustedQty) as AdjustedQty 
--							From tblPalletAdjustment
--							Where Facility = @vchFacility 
--							Group by Facility, MachineID, ShopOrder, Operator) tADJ
--							On cteSCH.Facility = tADJ.Facility and cteSCH.DefaultPkgLine=tADJ.MachineID And cteSCH.ShopOrder = tADJ.ShopOrder And cteSCH.Operator = tADJ.Operator
					) tLPFM
					GROUP BY WorkCenter, Shift, ShiftProductionDate
				) tLES
				-- "Based on Labor hours" uses LEFT OUTER JOIN
				RIGHT OUTER JOIN (
					SELECT WorkCenter, Shift, ApplyDate, ShiftSequence, Sum(LaborHoursWorked) as LaborHoursWorked
						FROM #tblKPH 
						GROUP BY WorkCenter, Shift, ApplyDate, ShiftSequence) tLHW
				ON tLES.WorkCenter = tLHW.WorkCenter 
					AND tLES.Shift = tLHW.Shift
					AND tLES.ShiftProductionDate = tLHW.ApplyDate
			END
		END
--		ELSE
--			IF @vchAction = 'LaborHrByShift'
--			BEGIN
--				;with cteSCH AS
--				(
--					SELECT tSCH.Facility, tSCH.DefaultPkgLine, tSCH.Shift, tSCH.ShiftProductionDate, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator, SUM(tSCH.CasesProduced) as CasesProduced, SUM(ActRunTime) as ActRunTime,
--						ROUND(SUM(ISNULL(tMPH.PaidHours, tSCH.ActRunTime)) ,2) as PaidRunTime 
--					FROM (
--						SELECT Facility, DefaultPkgLine, ShopOrder, ItemNumber, Operator, ShiftProductionDate, OverrideShiftNo as Shift, sum(CasesProduced) as CasesProduced,
--							Round(Sum(Isnull(DateDiff(Second,StartTime,StopTime),0)) / 3600.00, 2) as ActRunTime
--						FROM tblsessioncontrolhst 
--						WHERE Facility = @vchFacility 
--							AND ShiftProductionDate Between @dteFromDate and @dteToDate
--							AND ((Facility not in (SELECT facility FROM dbo.tblFacility WHERE Region = '01') OR Len(Rtrim(Operator)) > 3) )
--						GROUP BY Facility, DefaultPkgLine, ShopOrder, ItemNumber, Operator, ShiftProductionDate, OverrideShiftNo) tSCH
--					Left Outer Join tblMachinePaidHours tMPH
--					ON tSCH.Facility = tMPH.Facility 
--						AND tSCH.Operator = tMPH.Operator 
--						AND tSCH.DefaultPkgLine = tMPH.MachineID 
--						AND tSCH.ShopOrder = tMPH.ShopOrder 
--						AND tSCH.ShiftProductionDate = tMPH.ShiftProductionDate 
--						AND tSCH.Shift = tMPH.ShiftNo	
--					GROUP BY tSCH.Facility, tSCH.DefaultPkgLine, tSCH.Shift, tSCH.ShiftProductionDate, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator
--				)
--
--				Select WorkCenter, Shift, ShiftProductionDate, SUM(CasesProduced) as CasesProduced, SUM(PaidRunTime) as PaidRunTime, SUM(PoundProduced) as PoundProduced, 
--					SUM(UnitsProduced) as UnitsProduced, Sum(LaborHoursWorked) as ActLaborWorkHr, SUM(StdMachineHrEarnedInUnit) as StdMachineHrEarned,
--					SUM(PoundProduced) /SUM(RunOperators * StdMachineHrEarnedInUnit * NetWeight) as StdLbsPerLaborHr
--				FROM (
--					Select tEQ.WorkCenter, cteSCH.Shift, cteSCH.ShiftProductionDate, cteSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0) as CasesProduced, cteSCH.ActRunTime, cteSCH.PaidRunTime,
--						Round(tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit / (Case When tIM.labelweightUOM = 'GM' Then 453.5924 Else 1 End),3) as NetWeight, 
--						Round((cteSCH.CasesProduced+ISNULL(tADJ.AdjustedQty,0)) * Round(tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit/ (Case When tIM.labelweightUOM = 'GM' Then 453.5924 Else 1 End),3),0) as PoundProduced, 
--						CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0)) END as StdUnitPerHr,
--						CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else (cteSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0))/ (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit,
--						ISNULL(tSMER_ID.StdWorkCenterEfficiency,ISNULL(tSMER_WC.StdWorkCenterEfficiency,0)) as StdWCBGTEfficiency,
--						ISNULL(tSMER_ID.RunOperators,ISNULL(tSMER_WC.RunOperators,1)) as RunOperators,
--						Round((cteSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0)) * ISNULL(tIM.SaleableUnitPerCase,0) * ISNULL(tIM.PackagesPerSaleableUnit,0) ,0)  as UnitsProduced,
--						tLHW.LaborHoursWorked
--					From cteSCH
--					LEFT OUTER JOIN tblItemMaster tIM
--					On cteSCH.Facility = tIM.Facility and cteSCH.ItemNumber = tIM.ItemNumber
--					LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_ID
--					ON cteSCH.Facility = tSMER_ID.Facility AND tIM.ItemNumber = tSMER_ID.ItemNumber AND cteSCH.DefaultPkgLine = tSMER_ID.MachineID
--					LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_WC
--					ON cteSCH.Facility = tSMER_WC.Facility AND tIM.ItemNumber = tSMER_WC.ItemNumber AND CAST(SUBSTRING(cteSCH.DefaultPkgLine,1,4) as int) = tSMER_WC.WorkCenter and tSMER_WC.MachineID = ''
--					LEFT OUTER JOIN tblEquipment tEQ
--					ON cteSCH.Facility = tEQ.Facility and cteSCH.DefaultPkgLine = tEQ.EquipmentID
--					LEFT OUTER JOIN (Select Facility, MachineID, ShopOrder, Operator, Sum(AdjustedQty) as AdjustedQty From tblPalletAdjustment
--						Where Facility = @vchFacility 
--						Group by Facility, MachineID, ShopOrder, Operator) tADJ
--						On cteSCH.Facility = tADJ.Facility and cteSCH.DefaultPkgLine=tADJ.MachineID And cteSCH.ShopOrder = tADJ.ShopOrder And cteSCH.Operator = tADJ.Operator
--					LEFT OUTER JOIN (
--						SELECT WorkCenter, Shift, ApplyDate, Sum(LaborHoursWorked) as LaborHoursWorked
--						FROM #tblKPH 
--						GROUP BY WorkCenter, Shift, ApplyDate) tLHW
--					ON tEQ.WorkCenter = tLHW.WorkCenter 
--						AND cteSCH.Shift = tLHW.Shift
--						AND cteSCH.ShiftProductionDate = tLHW.ApplyDate
--				) tLES
--				GROUP BY Shift, WorkCenter, ShiftProductionDate
--			END	
		IF Object_id('tempdb..#tblKPH') IS NOT NULL
			DROP TABLE #tblKPH
	
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		BEGIN TRY
		IF Object_id('tempdb..#tblKPH') IS NOT NULL
			DROP TABLE #tblKPH
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

