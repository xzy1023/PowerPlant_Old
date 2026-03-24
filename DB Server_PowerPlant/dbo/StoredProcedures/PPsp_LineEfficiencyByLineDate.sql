

-- =============================================
-- Author:		Bong Lee
-- Create date: Oct 21,2009
-- Description:	Line Efficency By Line and Date
-- WO#194:		Sep. 9, 2010	Bong Lee	
-- Description:	If the time frame for the expected shift crosses mid-night
--				and the given time is prior to mid-night, the Shift Production 
--				Date will be the given time plus one day. Before this change the 	
--				Shift Production Date for this case was same as the given time but 
--				if the given time after mid-night, the result was given time 
--				minus one day.
-- WO#359:		Aug. 25, 2011	Bong Lee	
-- Description:	Use standard table functions to replace some of the select statements
--				Apply pallet adjustment to quantity produced in the Overall and Individual sessions
--				Include the records that RunTime > 0 and tSCH_CTE.CasesProduced > 0
-- WO#5103:		Mar. 03, 2017	Bong Lee	
-- Description:	Allow to filter by shift
-- WO#27470:	Aug. 27, 2019	Bong Lee
-- Description:	Routing rates are required to base on effective date.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_LineEfficiencyByLineDate]
	@vchFacility varchar(3),
	@vchMachineID varchar(10),
	@dteFromProdDate as DateTime,
	@intFromShift as tinyint,
	@dteToProdDate as DateTime,
	@intToShift as tinyint
	,@intInclOnlyShift as tinyint = NULL		-- WO#5103
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRY;
/* WO#359 Del Start 
-- WO#194 Add Start 
		/* Find the Work Shift type for each line - if line is duplicated, pick the active one */
		Declare @tblComputerConfig Table
		(Facility varchar(3) ,Packagingline varchar(10), WorkShiftType varchar(10))

		INSERT INTO @tblComputerConfig
		SELECT T1.Facility, T1.Packagingline, T1.WorkShiftType 
		FROM tblcomputerconfig T1 
		Left Outer Join 
			(SELECT Facility, Packagingline 
				FROM tblComputerconfig 
			WHERE Packagingline <> 'SPARE'
			Group By Facility, Packagingline
			Having Count(*) > 1) T2
		ON T1.Facility = T2.Facility AND T1.Packagingline = T2.Packagingline
		WHERE t2.Packagingline is null OR RecordStatus = 1
		Group By T1.Facility, T1.Packagingline, T1.WorkShiftType;
-- WO#194 Add Stop

		-- StdUnitPerHr = Routing rate * (2 ** Base code)
		-- StdMachineHrEarnedInUnit = Cases Produced / StdUnitPerHr
		-- Units Produced = Cases Produced * Saleable Unit Per Case
		-- Actual Line Efficiency % = Case Produced / Acutal run time / StdUnitPerHr * 100%
		-- Or
		-- Actual Line Efficiency % = StdMachineHrEarnedInUnit / Acutal run time * 100%
		With SCH_CTE
		AS
		(
		  Select tSCH.Facility, tSCH.PackagingLine, tSCH.ShiftProductionDate, tSCH.ShopOrder, (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0)) as CasesProduced, tSCH.RunTime, 
			CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0))/ (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit,
			ISNULL(tSMER_ID.StdWorkCenterEfficiency,ISNULL(tSMER_WC.StdWorkCenterEfficiency,0)) as StdWCEfficiency
		  From 	
/* WO#194 Del Start
			(Select Facility, DefaultPkgLine as PackagingLine, ShiftProductionDate, ShopOrder, ItemNumber, Sum(CasesProduced) as CasesProduced, 
				Sum(Isnull(DateDiff(second,StartTime,StopTime),0)) /3600.00 as RunTime
			  From tblSessionControlHst
			  Where Facility = @vchFacility AND DefaultPkgLine = @vchMachineID 
				AND Convert(varchar(8),ShiftProductionDate,112) + Cast(OverrideShiftNo as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
				AND ((Facility not in (Select facility from dbo.tblFacility where Region = '01') OR Len(Rtrim(Operator)) > 3) ) 
			Group By Facility, DefaultPkgLine, ShiftProductionDate, ShopOrder, ItemNumber) tSCH
 WO#194 Del Stop */
-- WO#194 Add Start
			-- Group data to desired way to join with other tables
			(Select tSCHst.Facility, tSCHst.DefaultPkgLine as PackagingLine, tSCHst.ShiftProductionDate, tSCHst.ShopOrder, tSCHst.ItemNumber, 
				SUM(tSCHst.CasesProduced) as CasesProduced, SUM(ISNULL(tMPH.PaidHours, ActRunTime)) as RunTime
			 FROM
				-- Select records based on filter and group them for joining Machine Paid Hours
 				(SELECT tSCH1.Facility, tSCH1.DefaultPkgLine, tSCH1.ShopOrder, tSCH1.ItemNumber, tSCH1.Operator, tSCH1.ShiftProductionDate, tSCH1.OverrideShiftNo, 
					SUM(tSCH1.CasesProduced) as CasesProduced, Round(SUM(Isnull(DateDiff(Second,tSCH1.StartTime,tSCH1.StopTime),0)) / 3600.00, 2) as ActRunTime
				FROM tblSessionControlHst tSCH1
				LEFT OUTER JOIN @tblComputerConfig tCC
					ON tSCH1.Facility = tCC.Facility AND tSCH1.OverridePkgLine =  tCC.PackagingLine
				LEFT OUTER JOIN tblshift tS
					ON tSCH1.Facility = tS.Facility AND tSCH1.OverrideShiftNo = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
				WHERE tSCH1.Facility = @vchFacility AND tSCH1.DefaultPkgLine = @vchMachineID 
					AND Convert(varchar(8),tSCH1.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
					AND ((tSCH1.Facility not in (Select Facility from dbo.tblFacility where Region = '01') OR Len(Rtrim(tSCH1.Operator)) > 3) ) 
				GROUP BY tSCH1.Facility, tSCH1.DefaultPkgLine, tSCH1.ShopOrder, tSCH1.ItemNumber, tSCH1.Operator, tSCH1.ShiftProductionDate, tSCH1.OverrideShiftNo) tSCHst
			LEFT OUTER JOIN tblMachinePaidHours tMPH
				ON tSCHst.Facility = tMPH.Facility AND tSCHst.Operator = tMPH.Operator AND tSCHst.DefaultPkgLine = tMPH.MachineID AND tSCHst.ShopOrder = tMPH.ShopOrder 
					AND tSCHst.ShiftProductionDate = tMPH.ShiftProductionDate AND tSCHst.OverrideShiftNo = tMPH.ShiftNo
			Group By tSCHst.Facility, tSCHst.DefaultPkgLine, tSCHst.ShiftProductionDate, tSCHst.ShopOrder, tSCHst.ItemNumber) tSCH
-- WO#194 Add Stop
		  Left Outer Join
/* WO#194 Del Start
		  (Select Facility, MachineID, ShopOrder, SUM(AdjustedQty) as AdjustedQty 
				From tblPalletAdjustment 
				Where Facility = @vchFacility AND MachineID = @vchMachineID 
				Group by Facility,MachineID,ShopOrder) tADJ
WO#194 Del Stop */
-- WO#194 Add Start
		  (Select tPA.Facility, tPA.MachineID, tPA.ShopOrder, SUM(tPA.AdjustedQty) as AdjustedQty 
				From tblPalletAdjustment tPA
				LEFT OUTER JOIN tblPalletHst tPH
				ON tPA.PalletID = tPH.PalletID
				LEFT OUTER JOIN @tblComputerConfig tCC
					ON tPH.Facility = tCC.Facility AND tPH.DefaultPkgLine =  tCC.PackagingLine
				LEFT OUTER JOIN tblshift tS
					ON tPH.Facility = tS.Facility AND tPH.ShiftNo = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup
				Where tPA.Facility = @vchFacility AND tPA.MachineID = @vchMachineID 
					AND Convert(varchar(8),tPH.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
				Group by tPA.Facility,tPA.MachineID,tPA.ShopOrder) tADJ
-- WO#194 Add Stop
		  On tSCH.Facility = tADJ.Facility and tSCH.PackagingLine = tADJ.MachineID And tSCH.ShopOrder = tADJ.ShopOrder
		  LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_ID
			ON tSCH.Facility = tSMER_ID.Facility AND tSCH.ItemNumber = tSMER_ID.ItemNumber AND tSCH.PackagingLine = tSMER_ID.MachineID
		  LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_WC
			ON tSCH.Facility = tSMER_WC.Facility AND tSCH.ItemNumber = tSMER_WC.ItemNumber AND CAST(SUBSTRING(tSCH.PackagingLine,1,4) as int) = tSMER_WC.WorkCenter and tSMER_WC.MachineID = ''	
		)

		Select tSCH_CTE.Facility, tSCH_CTE.PackagingLine, tSCH_CTE.ShiftProductionDate, 
			Case When Sum(tSCH_CTE.RunTime) = 0 Then 0 Else Sum(tSCH_CTE.StdMachineHrEarnedInUnit) / Sum(tSCH_CTE.RunTime) End as LineEff,
			Max(tSCH_CTE.StdWCEfficiency) as StdWCEfficiency
		From SCH_CTE as tSCH_CTE
		Group by tSCH_CTE.Facility, tSCH_CTE.PackagingLine, tSCH_CTE.ShiftProductionDate
WO#359 Del Stop */

--WO#359 Add Start
		With SCH_CTE
		AS
		(
		  -- WO#27470	SELECT tSCH.Facility, tSCH.DefaultPkgLine as PackagingLine, tSCH.ShiftProductionDate, tSCH.ShopOrder, (tSCH.CasesProduced + tSCH.AdjustedQty) as CasesProduced,
			-- WO#27470	tSCH.PaidRunTime as RunTime, 
			-- WO#27470	CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else (tSCH.CasesProduced + tSCH.AdjustedQty) / (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit,
			-- WO#27470	ISNULL(tSMER_ID.StdWorkCenterEfficiency,ISNULL(tSMER_WC.StdWorkCenterEfficiency,0)) as StdWCEfficiency
-- WO#27470 ADD Start
		  SELECT tSCH.Facility, tSCH.DefaultPkgLine as PackagingLine, tSCH.ShiftProductionDate
			,(tSCH.CasesProduced + tSCH.AdjustedQty) as CasesProduced
			,tSCH.PaidRunTime as RunTime 
			,CASE When ISNULL(tfSMER.MachineHours,0)= 0 Then 0 Else (tSCH.CasesProduced + ISNULL(tSCH.AdjustedQty,0))/ (POWER(10,ISNULL(tfSMER.BasisCode,0)) / tfSMER.MachineHours) End As StdMachineHrEarnedInUnit	
			,IIF(tfSMER.StdWorkCenterEfficiency=0,NULL,tfSMER.StdWorkCenterEfficiency) as StdWCEfficiency
-- WO#27470 ADD Stop
		  FROM dbo.tfnSessionControlHstSummary('WithAdjByLineSO',@vchFacility,@vchMachineID,NULL,NULL,NULL,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift) tSCH
/* WO#27470 DEL Start
		  LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_ID
		  ON tSCH.Facility = tSMER_ID.Facility AND tSCH.ItemNumber = tSMER_ID.ItemNumber AND tSCH.DefaultPkgLine = tSMER_ID.MachineID
		  LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_WC
		  ON tSCH.Facility = tSMER_WC.Facility AND tSCH.ItemNumber = tSMER_WC.ItemNumber AND CAST(SUBSTRING(tSCH.DefaultPkgLine,1,4) as int) = tSMER_WC.WorkCenter and tSMER_WC.MachineID = ''	
WO#27470 DEL Stop */
		  OUTER APPLY [dbo].[tfnStdMachineEfficiencyRate] (tSCH.Facility, tSCH.ItemNumber, tSCH.DefaultPkgLine, LEFT(tSCH.DefaultPkgLine,4), tSCH.ShiftProductionDate) as tfSMER	-- WO#27470
		  WHERE tSCH.ShiftNo = ISNULL(@intInclOnlyShift, tSCH.ShiftNo)					-- WO#5103
		)

		Select tSCH_CTE.Facility, tSCH_CTE.PackagingLine, tSCH_CTE.ShiftProductionDate, 
			Case When Sum(tSCH_CTE.RunTime) = 0 Then 0 Else Sum(tSCH_CTE.StdMachineHrEarnedInUnit) / Sum(tSCH_CTE.RunTime) End as LineEff,
			-- WO#27470	Max(tSCH_CTE.StdWCEfficiency) as StdWCEfficiency
			AVG(tSCH_CTE.StdWCEfficiency) as StdWCEfficiency	-- WO#27470
		FROM SCH_CTE as tSCH_CTE 
			WHERE tSCH_CTE.RunTime > 0 and tSCH_CTE.CasesProduced > 0
		Group by tSCH_CTE.Facility, tSCH_CTE.PackagingLine, tSCH_CTE.ShiftProductionDate
--WO#359 Add Stop

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

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

