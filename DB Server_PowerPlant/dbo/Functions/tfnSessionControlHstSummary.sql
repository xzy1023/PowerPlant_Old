

-- =============================================
-- Author:		Bong Lee
-- Create date: May 9, 2011
-- WO# 359:	Power Plant Reporting Phase 2
-- Description:	Get Session Control History Summary inforamtion
-- =============================================
/*	-- Test --

SELECT Shiftproductiondate,shiftno, count(*), sum(PaidRunTime) FROM tfnSessionControlHstSummary ('WithoutAdj','07 ',NULL,NULL,NULL,NULL,'6/6/2011',1,'6/7/2011',3)
group by Shiftproductiondate,shiftno
order by Shiftproductiondate,shiftno

2011-06-06 00:00:00.000	1	35	103.10
2011-06-06 00:00:00.000	2	19	86.00
2011-06-06 00:00:00.000	3	8	24.00
2011-06-07 00:00:00.000	1	38	103.31
2011-06-07 00:00:00.000	2	21	72.50
2011-06-07 00:00:00.000	3	15	46.00

SELECT Shiftproductiondate,shiftno, count(*), sum(PaidRunTime) FROM tfnSessionControlHstSummary ('WithoutAdj', '07 ',NULL,NULL,NULL,NULL,'6/6/2011',3,'6/7/2011',3)
group by Shiftproductiondate,shiftno
order by Shiftproductiondate,shiftno	

2011-06-06 00:00:00.000	2	19	86.00
2011-06-07 00:00:00.000	1	38	103.31
2011-06-07 00:00:00.000	2	21	72.50
2011-06-07 00:00:00.000	3	15	46.00

*/
CREATE FUNCTION [dbo].[tfnSessionControlHstSummary] 
(
	@vchAction varchar(50)
	,@vchFacility varchar(3)
	,@vchMachineID varchar(10) = NULL
	,@vchOperator varchar(10) = NULL
	,@intShopOrder int = NULL
	,@vchItemNumber varchar(35) = NULL
	,@dteFromProdDate DateTime
	,@intFromShift char(1) = NULL
	,@dteToProdDate DateTime
	,@intToShift char(1) = NULL
)
RETURNS 
@tblSCH TABLE 
(
	Facility varchar(3)
	,ShiftProductiondate datetime
	,ShiftSequence tinyint
	,ShiftNo char(1) 
	,DefaultPkgLine varchar(10) 
	,ShopOrder int 
	,ItemNumber varchar(35) 
	,Operator varchar(10) 
	,CasesProduced int			-- Adjustment is not included.
	,ActRunTime decimal(6,2) 
	,PaidRunTime decimal(6,2)	-- Included this logic: ISNULL(PaidHours,ActRunTime)
	,AdjustedQty int
	,ReworkWgt decimal(8,2)
)
AS
BEGIN
	DECLARE @MaxShiftSequence int,
			@MinShiftSequence int;

	-- Find out the first and last shift sequence of a day
	SELECT @MaxShiftSequence = Max(ShiftSequence) , @MinShiftSequence = Min(ShiftSequence) 
		FROM tblShift WHERE Facility = @vchFacility AND Workgroup = 'P'
	
	If @vchAction = 'WithoutAdj'
	BEGIN
		-- If the From or To Shifts are not specified or (From Shift is not first shift and To shift is not last shift of a day)
		-- then compare the Shift Producton Date only.
		If  @intFromShift IS NULL 
			OR @intToShift IS NULL
			OR (@intFromShift = @MinShiftSequence AND @intToShift = @MaxShiftSequence)
		BEGIN
			INSERT INTO @tblSCH
			SELECT tSCHSum.Facility, tSCHSum.ShiftProductionDate, tS.ShiftSequence, tSCHSum.OverrideShiftNo, tSCHSum.DefaultPkgLine, tSCHSum.ShopOrder, tSCHSum.ItemNumber 
					,tSCHSum.Operator, SUM(tSCHSum.CasesProduced) as CasesProduced, SUM(ActRunTime) as ActRunTime
					,Round(SUM(ISNULL(tMPH.PaidHours, tSCHSum.ActRunTime)) ,2) as PaidRunTime 
					,0 as AdjustedQty
					,SUM(tSCHSum.ReworkWgt) as ReworkWgt
			FROM (
				SELECT tSCH.Facility, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo, tSCH.DefaultPkgLine, tSCH.ShopOrder 
					,tSCH.ItemNumber, tSCH.Operator, Sum(CasesProduced) as CasesProduced
					,Round(Sum(Isnull(DateDiff(Second, tSCH.StartTime, tSCH.StopTime),0)) / 3600.00, 2) as ActRunTime
					,SUM(tSCH.ReworkWgt) as ReworkWgt
				FROM tblSessionControlHst tSCH
				LEFT OUTER JOIN tblPlantStaff tPS
				ON tSCH.Operator = tPS.StaffID AND tSCH.Facility = tPS.Facility
				WHERE (@vchFacility IS NULL OR tSCH.Facility = @vchFacility)
					AND (@vchMachineID IS NULL OR tSCH.DefaultPkgLine = @vchMachineID)
					AND (@vchOperator IS NULL OR tSCH.Operator  = @vchOperator)
					AND (@intShopOrder IS NULL OR tSCH.ShopOrder = @intShopOrder)
					AND (@vchItemNumber IS NULL OR tSCH.ItemNumber = @vchItemNumber)
					AND ShiftProductionDate	BETWEEN @dteFromProdDate AND @dteToProdDate
					--AND tPS.WorkGroup <> 'SetUp'
					AND tPS.WorkSubGroup <> 'SetUp'
				GROUP BY tSCH.Facility, tSCH.DefaultPkgLine, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo) tSCHSum
			LEFT OUTER JOIN vwLineWorkShiftType vLWT
				ON tSCHSum.Facility = vLWT.Facility AND tSCHSum.DefaultPkgLine =  vLWT.PackagingLine
			LEFT OUTER JOIN tblshift tS
				ON tSCHSum.Facility = tS.Facility AND tSCHSum.OverrideShiftNo = tS.Shift AND ISNULL(vLWT.WorkShiftType,'P') = tS.WorkGroup 
			LEFT OUTER JOIN tblMachinePaidHours tMPH
			ON tSCHSum.Facility = tMPH.Facility AND tSCHSum.Operator = tMPH.Operator AND tSCHSUM.DefaultPkgLine = tMPH.MachineID AND tSCHSum.ShopOrder = tMPH.ShopOrder 
				AND tSCHSum.ShiftProductionDate = tMPH.ShiftProductionDate AND tSCHSum.OverrideShiftNo = tMPH.ShiftNo				
			GROUP BY tSCHSum.Facility, tSCHSum.ShiftProductionDate, tS.ShiftSequence, tSCHSum.OverrideShiftNo, tSCHSum.DefaultPkgLine, tSCHSum.ShopOrder, tSCHSum.ItemNumber, tSCHSum.Operator
		END
		ELSE	-- Compare shift production date and shift
		BEGIN
			INSERT INTO @tblSCH
			SELECT tSCHSum.Facility, tSCHSum.ShiftProductionDate, tSCHSum.ShiftSequence, tSCHSum.OverrideShiftNo, tSCHSum.DefaultPkgLine, tSCHSum.ShopOrder, tSCHSum.ItemNumber 
					,tSCHSum.Operator, SUM(tSCHSum.CasesProduced) as CasesProduced, SUM(ActRunTime) as ActRunTime
					,Round(SUM(ISNULL(tMPH.PaidHours, tSCHSum.ActRunTime)) ,2) as PaidRunTime
					,0 as AdjustedQty
					,SUM(tSCHSum.ReworkWgt) as ReworkWgt 
			FROM (
				SELECT tSCH.Facility, tSCH.ShiftProductionDate, tS.ShiftSequence, tSCH.OverrideShiftNo, tSCH.DefaultPkgLine, tSCH.ShopOrder 
					,tSCH.ItemNumber, tSCH.Operator, Sum(CasesProduced) as CasesProduced
					,Round(Sum(Isnull(DateDiff(Second, tSCH.StartTime, tSCH.StopTime),0)) / 3600.00, 2) as ActRunTime
					,SUM(tSCH.ReworkWgt) as ReworkWgt
				FROM tblSessionControlHst tSCH
				LEFT OUTER JOIN tblPlantStaff tPS
					ON tSCH.Operator = tPS.StaffID AND tSCH.Facility = tPS.Facility
				LEFT OUTER JOIN vwLineWorkShiftType vLWT
					ON tSCH.Facility = vLWT.Facility AND tSCH.DefaultPkgLine =  vLWT.PackagingLine
				LEFT OUTER JOIN tblshift tS
					ON tSCH.Facility = tS.Facility AND tSCH.OverrideShiftNo = tS.Shift AND ISNULL(vLWT.WorkShiftType,'P') = tS.WorkGroup 
				WHERE (@vchFacility IS NULL OR tSCH.Facility = @vchFacility)
					AND (@vchMachineID IS NULL OR tSCH.DefaultPkgLine = @vchMachineID)
					AND (@vchOperator IS NULL OR tSCH.Operator  = @vchOperator)
					AND (@intShopOrder IS NULL OR tSCH.ShopOrder = @intShopOrder)
					AND (@vchItemNumber IS NULL OR tSCH.ItemNumber = @vchItemNumber)
					AND Convert(varchar(8),ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) 
						BETWEEN convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) AND convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
					--AND tPS.WorkGroup <> 'SetUp'
					AND tPS.WorkSubGroup <> 'SetUp'
				GROUP BY tSCH.Facility, tSCH.ShiftProductionDate, tS.ShiftSequence, tSCH.OverrideShiftNo, tSCH.DefaultPkgLine, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator) tSCHSum
			LEFT OUTER JOIN tblMachinePaidHours tMPH
			ON tSCHSum.Facility = tMPH.Facility AND tSCHSum.Operator = tMPH.Operator AND tSCHSUM.DefaultPkgLine = tMPH.MachineID AND tSCHSum.ShopOrder = tMPH.ShopOrder 
				AND tSCHSum.ShiftProductionDate = tMPH.ShiftProductionDate AND tSCHSum.OverrideShiftNo = tMPH.ShiftNo				
			GROUP BY tSCHSum.Facility, tSCHSum.ShiftProductionDate, tSCHSum.ShiftSequence, tSCHSum.OverrideShiftNo, tSCHSum.DefaultPkgLine, tSCHSum.ShopOrder, tSCHSum.ItemNumber, tSCHSum.Operator
		END
	END
	ELSE
	If @vchAction = 'WithAdjByOpr'
	BEGIN
	-- If the From or To Shifts are not specified or (From Shift is not first shift and To shift is not last shift of a day)
	-- then compare the Shift Producton Date only.
		If  @intFromShift IS NULL 
			OR @intToShift IS NULL
			OR (@intFromShift = @MinShiftSequence AND @intToShift = @MaxShiftSequence)
		BEGIN
			-- use Pallet History to get the pallet adjustment on the requested date range
			;With cteAdj as
			(	
				-- Get adjustments for Line operator created pallets
				-- ShiftProduction date and shift is based the session history not from Pallet history
				-- (Note: currently the ShiftProduction date and shift of the pallet is calculated based on server system time)
				SELECT tSCH.Facility, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo as ShiftNo, tSCH.ShopOrder, tSCH.DefaultPkgLine, tSCH.Operator, SUM(ISNULL(tPA.AdjustedQty,0)) as AdjustedQty 
				FROM tfnSessionControlHstDetail(NULL, @vchFacility, @vchMachineID, @vchOperator, @intShopOrder, @vchItemNumber, @dteFromProdDate, @intFromShift, @dteToProdDate, @intToShift) tSCH
				INNER JOIN tblPalletHst tPH
				ON tSCH.Facility = tPH.Facility AND tSCH.ShopOrder = tPH.ShopOrder AND tSCH.DefaultPkgLine = tPH.DefaultPkgLine AND tSCH.Operator = tPH.Operator AND tSCH.StartTime = tPH.StartTime
				INNER JOIN (SELECT PalletID, SUM(AdjustedQty) as AdjustedQty FROM dbo.tfnPalletAdjustment('AdjWithCorrectionOnly',@vchFacility, @intShopOrder, @vchMachineID, @vchOperator, NULL, NULL, NULL, NULL) 
							WHERE LatestReasonCode = '01'
							GROUP BY PalletID) tPA
				ON 	tPH.PalletID = tPA.PalletID
				GROUP BY tSCH.Facility, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo, tSCH.ShopOrder, tSCH.DefaultPkgLine, tSCH.Operator
				--
				UNION
				--
				-- Get adjustments for Pallet Station created pallets
				-- Session Control does not log Pallet Station sessions so the ShiftProductionDate of the pallet is calculated based on the server system time
				SELECT tPH.Facility, tPH.ShiftProductionDate, tPH.ShiftNo, tPH.ShopOrder, tPH.DefaultPkgLine, tPH.Operator, SUM(ISNULL(tPA.AdjustedQty,0)) as AdjustedQty
				FROM tblPalletHst tPH
				INNER JOIN (SELECT PalletID, SUM(AdjustedQty) as AdjustedQty FROM dbo.tfnPalletAdjustment('AdjWithCorrectionOnly', @vchFacility, @intShopOrder,@vchMachineID, @vchOperator, NULL, NULL, NULL, NULL)
								WHERE LatestReasonCode = '01'
								GROUP BY PalletID) tPA
				ON 	tPH.PalletID = tPA.PalletID
				INNER JOIN tblPlantStaff tPS
--WO#359		ON tPH.Operator = tPS.staffID
				ON tPH.Facility = tPS.Facility AND tPH.Operator = tPS.StaffID	--WO#359
				WHERE tPS.WorkGroup = 'PS'
					AND (@vchFacility IS NULL OR tPH.Facility = @vchFacility)
					AND (@vchMachineID IS NULL OR tPH.DefaultPkgLine = @vchMachineID)
					AND (@vchOperator IS NULL OR tPH.Operator  = @vchOperator)
					AND (@intShopOrder IS NULL OR tPH.ShopOrder = @intShopOrder)
					AND (@vchItemNumber IS NULL OR tPH.ItemNumber = @vchItemNumber)
					AND tPH.ShiftProductionDate	BETWEEN @dteFromProdDate AND @dteToProdDate
				GROUP BY tPH.Facility, tPH.ShiftProductionDate, tPH.ShiftNo, tPH.ShopOrder, tPH.DefaultPkgLine, tPH.Operator	
			)
	
			INSERT INTO @tblSCH
			SELECT tSCHSum.Facility, tSCHSum.ShiftProductionDate, tS.ShiftSequence, tSCHSum.OverrideShiftNo, tSCHSum.DefaultPkgLine, tSCHSum.ShopOrder, tSCHSum.ItemNumber 
					,tSCHSum.Operator, SUM(tSCHSum.CasesProduced) as CasesProduced, SUM(ActRunTime) as ActRunTime
					,Round(SUM(ISNULL(tMPH.PaidHours, tSCHSum.ActRunTime)) ,2) as PaidRunTime 
					,CAST(Round(SUM(ISNULL(cteAdj.AdjustedQty,0)),0) as int)
					,SUM(tSCHSum.ReworkWgt) as ReworkWgt
			FROM (
				SELECT tSCH.Facility, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo, tSCH.DefaultPkgLine, tSCH.ShopOrder 
					,tSCH.ItemNumber, tSCH.Operator, Sum(CasesProduced) as CasesProduced
					,Round(Sum(Isnull(DateDiff(Second, tSCH.StartTime, tSCH.StopTime),0)) / 3600.00, 2) as ActRunTime
					,SUM(tSCH.ReworkWgt) as ReworkWgt
				FROM tblSessionControlHst tSCH
				LEFT OUTER JOIN tblPlantStaff tPS
					ON tSCH.Operator = tPS.StaffID AND tSCH.Facility = tPS.Facility
				WHERE (@vchFacility IS NULL OR tSCH.Facility = @vchFacility)
					AND (@vchMachineID IS NULL OR tSCH.DefaultPkgLine = @vchMachineID)
					AND (@vchOperator IS NULL OR tSCH.Operator  = @vchOperator)
					AND (@intShopOrder IS NULL OR tSCH.ShopOrder = @intShopOrder)
					AND (@vchItemNumber IS NULL OR tSCH.ItemNumber = @vchItemNumber)
					AND ShiftProductionDate	BETWEEN @dteFromProdDate AND @dteToProdDate
					--AND tPS.WorkGroup <> 'SetUp'
					AND tPS.WorkSubGroup <> 'SetUp'
				GROUP BY tSCH.Facility, tSCH.DefaultPkgLine, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo) tSCHSum
			LEFT OUTER JOIN vwLineWorkShiftType vLWT
				ON tSCHSum.Facility = vLWT.Facility AND tSCHSum.DefaultPkgLine =  vLWT.PackagingLine
			LEFT OUTER JOIN tblshift tS
				ON tSCHSum.Facility = tS.Facility AND tSCHSum.OverrideShiftNo = tS.Shift AND ISNULL(vLWT.WorkShiftType,'P') = tS.WorkGroup 
			LEFT OUTER JOIN tblMachinePaidHours tMPH
			ON tSCHSum.Facility = tMPH.Facility AND tSCHSum.Operator = tMPH.Operator AND tSCHSUM.DefaultPkgLine = tMPH.MachineID AND tSCHSum.ShopOrder = tMPH.ShopOrder 
				AND tSCHSum.ShiftProductionDate = tMPH.ShiftProductionDate AND tSCHSum.OverrideShiftNo = tMPH.ShiftNo				
			LEFT OUTER JOIN cteAdj 
			ON tSCHSum.Facility = cteAdj.Facility AND tSCHSum.ShiftProductionDate = cteAdj.ShiftProductionDate AND tSCHSum.OverrideShiftNo = cteAdj.ShiftNo 
				AND tSCHSum.Operator = cteAdj.Operator AND tSCHSum.DefaultPkgLine = cteAdj.DefaultPkgLine And tSCHSum.ShopOrder = cteAdj.ShopOrder		
			GROUP BY tSCHSum.Facility, tSCHSum.ShiftProductionDate, tS.ShiftSequence, tSCHSum.OverrideShiftNo, tSCHSum.DefaultPkgLine, tSCHSum.ShopOrder, tSCHSum.ItemNumber, tSCHSum.Operator
		END
		ELSE
		BEGIN
			;With cteAdj as
			(	
				-- Get adjustments for Line operator created pallets
				-- ShiftProduction date and shift is based the session history not from Pallet history
				-- (Note: currently the ShiftProduction date and shift of the pallet is calculated based on server system time)
				SELECT tSCH.Facility, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo as ShiftNo, tSCH.ShopOrder, tSCH.DefaultPkgLine, tSCH.Operator, SUM(ISNULL(tPA.AdjustedQty,0)) as AdjustedQty 
				FROM tfnSessionControlHstDetail(NULL, @vchFacility, @vchMachineID, @vchOperator, @intShopOrder, @vchItemNumber, @dteFromProdDate, @intFromShift, @dteToProdDate, @intToShift) tSCH
				INNER JOIN tblPalletHst tPH
				ON tSCH.Facility = tPH.Facility AND tSCH.ShopOrder = tPH.ShopOrder AND tSCH.DefaultPkgLine = tPH.DefaultPkgLine AND tSCH.Operator = tPH.Operator AND tSCH.StartTime = tPH.StartTime
				INNER JOIN (SELECT PalletID, SUM(AdjustedQty) as AdjustedQty FROM dbo.tfnPalletAdjustment('AdjWithCorrectionOnly',@vchFacility, @intShopOrder, @vchMachineID, @vchOperator, NULL, NULL, NULL, NULL) 
							WHERE LatestReasonCode = '01'
							GROUP BY PalletID) tPA
				ON 	tPH.PalletID = tPA.PalletID
				GROUP BY tSCH.Facility, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo, tSCH.ShopOrder, tSCH.DefaultPkgLine, tSCH.Operator
				--
				UNION
				--
				-- Get adjustments for Pallet Station created pallets
				-- Session Control does not log Pallet Station sessions so the ShiftProductionDate of the pallet is calculated based on the server system time
				SELECT tPH.Facility, tPH.ShiftProductionDate, tPH.ShiftNo, tPH.ShopOrder, tPH.DefaultPkgLine, tPH.Operator, SUM(ISNULL(tPA.AdjustedQty,0)) as AdjustedQty
				FROM tblPalletHst tPH
				INNER JOIN (SELECT PalletID, SUM(AdjustedQty) as AdjustedQty FROM dbo.tfnPalletAdjustment('AdjWithCorrectionOnly', @vchFacility, @intShopOrder,@vchMachineID, @vchOperator, NULL, NULL, NULL, NULL)
								WHERE LatestReasonCode = '01'
								GROUP BY PalletID) tPA
				ON 	tPH.PalletID = tPA.PalletID
				INNER JOIN tblPlantStaff tPS
--WO#359		ON tPH.Operator = tPS.staffID
				ON tPH.Facility = tPS.Facility AND tPH.Operator = tPS.StaffID	--WO#359
				LEFT OUTER JOIN vwLineWorkShiftType vLWT
					ON tPH.Facility = vLWT.Facility AND tPH.DefaultPkgLine =  vLWT.PackagingLine
				LEFT OUTER JOIN tblshift tS
					ON tPH.Facility = tS.Facility AND tPH.ShiftNo = tS.Shift AND ISNULL(vLWT.WorkShiftType,'P') = tS.WorkGroup 
				WHERE tPS.WorkGroup = 'PS'
					AND (@vchFacility IS NULL OR tPH.Facility = @vchFacility)
					AND (@vchMachineID IS NULL OR tPH.DefaultPkgLine = @vchMachineID)
					AND (@vchOperator IS NULL OR tPH.Operator  = @vchOperator)
					AND (@intShopOrder IS NULL OR tPH.ShopOrder = @intShopOrder)
					AND (@vchItemNumber IS NULL OR tPH.ItemNumber = @vchItemNumber)
					AND Convert(varchar(8),tPH.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) 
						BETWEEN convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
				GROUP BY tPH.Facility, tPH.ShiftProductionDate, tPH.ShiftNo, tPH.ShopOrder, tPH.DefaultPkgLine, tPH.Operator	
			)

			INSERT INTO @tblSCH
			SELECT tSCHSum.Facility, tSCHSum.ShiftProductionDate, tSCHSum.ShiftSequence, tSCHSum.OverrideShiftNo, tSCHSum.DefaultPkgLine, tSCHSum.ShopOrder, tSCHSum.ItemNumber 
					,tSCHSum.Operator, SUM(tSCHSum.CasesProduced) as CasesProduced, SUM(ActRunTime) as ActRunTime
					,Round(SUM(ISNULL(tMPH.PaidHours, tSCHSum.ActRunTime)) ,2) as PaidRunTime
					,CAST(Round(SUM(ISNULL(cteAdj.AdjustedQty,0)),0) as int)
					,SUM(tSCHSum.ReworkWgt) as ReworkWgt
			FROM (
				SELECT tSCH.Facility, tSCH.ShiftProductionDate, tS.ShiftSequence, tSCH.OverrideShiftNo, tSCH.DefaultPkgLine, tSCH.ShopOrder 
					,tSCH.ItemNumber, tSCH.Operator, Sum(CasesProduced) as CasesProduced
					,Round(Sum(Isnull(DateDiff(Second, tSCH.StartTime, tSCH.StopTime),0)) / 3600.00, 2) as ActRunTime
					,SUM(tSCH.ReworkWgt) as ReworkWgt
				FROM tblSessionControlHst tSCH
				LEFT OUTER JOIN tblPlantStaff tPS
					ON tSCH.Operator = tPS.StaffID AND tSCH.Facility = tPS.Facility
				LEFT OUTER JOIN vwLineWorkShiftType vLWT
					ON tSCH.Facility = vLWT.Facility AND tSCH.DefaultPkgLine =  vLWT.PackagingLine
				LEFT OUTER JOIN tblshift tS
					ON tSCH.Facility = tS.Facility AND tSCH.OverrideShiftNo = tS.Shift AND vLWT.WorkShiftType = tS.WorkGroup 
				WHERE (@vchFacility IS NULL OR tSCH.Facility = @vchFacility)
					AND (@vchMachineID IS NULL OR tSCH.DefaultPkgLine = @vchMachineID)
					AND (@vchOperator IS NULL OR tSCH.Operator  = @vchOperator)
					AND (@intShopOrder IS NULL OR tSCH.ShopOrder = @intShopOrder)
					AND (@vchItemNumber IS NULL OR tSCH.ItemNumber = @vchItemNumber)
					AND Convert(varchar(8),ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) 
					BETWEEN convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) AND convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
					--AND tPS.WorkGroup <> 'SetUp'
					AND tPS.WorkSubGroup <> 'SetUp'
				GROUP BY tSCH.Facility, tSCH.ShiftProductionDate, tS.ShiftSequence, tSCH.OverrideShiftNo, tSCH.DefaultPkgLine, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator) tSCHSum
			LEFT OUTER JOIN tblMachinePaidHours tMPH
			ON tSCHSum.Facility = tMPH.Facility AND tSCHSum.Operator = tMPH.Operator AND tSCHSUM.DefaultPkgLine = tMPH.MachineID AND tSCHSum.ShopOrder = tMPH.ShopOrder 
				AND tSCHSum.ShiftProductionDate = tMPH.ShiftProductionDate AND tSCHSum.OverrideShiftNo = tMPH.ShiftNo				
			LEFT OUTER JOIN cteAdj 
			ON tSCHSum.Facility = cteAdj.Facility AND tSCHSum.ShiftProductionDate = cteAdj.ShiftProductionDate AND tSCHSum.OverrideShiftNo = cteAdj.ShiftNo 
				AND tSCHSum.Operator = cteAdj.Operator AND tSCHSum.DefaultPkgLine = cteAdj.DefaultPkgLine And tSCHSum.ShopOrder = cteAdj.ShopOrder		
			GROUP BY tSCHSum.Facility, tSCHSum.ShiftProductionDate, tSCHSum.ShiftSequence, tSCHSum.OverrideShiftNo, tSCHSum.DefaultPkgLine, tSCHSum.ShopOrder, tSCHSum.ItemNumber, tSCHSum.Operator
		END

	END
	----
		ELSE
		If @vchAction = 'WithAdjByLineSO'
		BEGIN
		-- If the From or To Shifts are not specified or (From Shift is not first shift and To shift is not last shift of a day)
		-- then compare the Shift Producton Date only.
			If  @intFromShift IS NULL 
				OR @intToShift IS NULL
				OR (@intFromShift = @MinShiftSequence AND @intToShift = @MaxShiftSequence)
			BEGIN
				-- use Pallet History to get the pallet adjustment on the requested date range
				;With cteAdj as
				(	
					-- Get adjustments for Line operator created pallets
					-- ShiftProduction date and shift is based the session history not from Pallet history
					-- (Note: currently the ShiftProduction date and shift of the pallet is calculated based on server system time)
					SELECT tSCH.Facility, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo as ShiftNo, tSCH.ShopOrder, tSCH.DefaultPkgLine, SUM(ISNULL(tPA.AdjustedQty,0)) as AdjustedQty 
					FROM tfnSessionControlHstDetail(NULL, @vchFacility, @vchMachineID, NULL, @intShopOrder, @vchItemNumber, @dteFromProdDate, @intFromShift, @dteToProdDate, @intToShift) tSCH
					INNER JOIN tblPalletHst tPH
					ON tSCH.Facility = tPH.Facility AND tSCH.ShopOrder = tPH.ShopOrder AND tSCH.DefaultPkgLine = tPH.DefaultPkgLine AND tSCH.Operator = tPH.Operator AND tSCH.StartTime = tPH.StartTime
					INNER JOIN (SELECT PalletID, SUM(AdjustedQty) as AdjustedQty FROM dbo.tfnPalletAdjustment('AdjWithCorrectionOnly',@vchFacility, @intShopOrder, @vchMachineID, @vchOperator, NULL, NULL, NULL, NULL) 
								WHERE LatestReasonCode = '01'
								GROUP BY PalletID) tPA
					ON 	tPH.PalletID = tPA.PalletID
					GROUP BY tSCH.Facility, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo, tSCH.ShopOrder, tSCH.DefaultPkgLine
					--
					UNION
					--
					-- Get adjustments for Pallet Station created pallets
					-- Session Control does not log Pallet Station sessions so the ShiftProductionDate of the pallet is calculated based on the server system time
					SELECT tPH.Facility, tPH.ShiftProductionDate, tPH.ShiftNo, tPH.ShopOrder, tPH.DefaultPkgLine, SUM(ISNULL(tPA.AdjustedQty,0)) as AdjustedQty
					FROM tblPalletHst tPH
					INNER JOIN (SELECT PalletID, SUM(AdjustedQty) as AdjustedQty FROM dbo.tfnPalletAdjustment('AdjWithCorrectionOnly', @vchFacility, @intShopOrder,@vchMachineID, NULL, NULL, NULL, NULL, NULL)
									WHERE LatestReasonCode = '01'
									GROUP BY PalletID) tPA
					ON 	tPH.PalletID = tPA.PalletID
					INNER JOIN tblPlantStaff tPS
--WO#359			ON tPH.Operator = tPS.staffID
					ON tPH.Facility = tPS.Facility AND tPH.Operator = tPS.StaffID	--WO#359
					WHERE tPS.WorkGroup = 'PS'
						AND (@vchFacility IS NULL OR tPH.Facility = @vchFacility)
						AND (@vchMachineID IS NULL OR tPH.DefaultPkgLine = @vchMachineID)
						AND (@intShopOrder IS NULL OR tPH.ShopOrder = @intShopOrder)
						AND (@vchItemNumber IS NULL OR tPH.ItemNumber = @vchItemNumber)
						AND tPH.ShiftProductionDate	BETWEEN @dteFromProdDate AND @dteToProdDate
					GROUP BY tPH.Facility, tPH.ShiftProductionDate, tPH.ShiftNo, tPH.ShopOrder, tPH.DefaultPkgLine	
				)
		
				INSERT INTO @tblSCH
				SELECT tSCHSum.Facility, tSCHSum.ShiftProductionDate, tS.ShiftSequence, tSCHSum.OverrideShiftNo, tSCHSum.DefaultPkgLine, tSCHSum.ShopOrder, tSCHSum.ItemNumber 
						,'' as Operator, SUM(tSCHSum.CasesProduced) as CasesProduced, SUM(ActRunTime) as ActRunTime
						,Round(SUM(ISNULL(tMPH.PaidHours, tSCHSum.ActRunTime)) ,2) as PaidRunTime 
						,CAST(Round(SUM(ISNULL(cteAdj.AdjustedQty,0)),0) as int)
						,SUM(tSCHSum.ReworkWgt) as ReworkWgt
				FROM (
					SELECT tSCH.Facility, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo, tSCH.DefaultPkgLine, tSCH.ShopOrder 
						,tSCH.ItemNumber, Sum(CasesProduced) as CasesProduced
						,Round(Sum(Isnull(DateDiff(Second, tSCH.StartTime, tSCH.StopTime),0)) / 3600.00, 2) as ActRunTime
						,SUM(tSCH.ReworkWgt) as ReworkWgt
					FROM tblSessionControlHst tSCH
					LEFT OUTER JOIN tblPlantStaff tPS
						ON tSCH.Operator = tPS.StaffID AND tSCH.Facility = tPS.Facility
					WHERE (@vchFacility IS NULL OR tSCH.Facility = @vchFacility)
						AND (@vchMachineID IS NULL OR tSCH.DefaultPkgLine = @vchMachineID)
						AND (@intShopOrder IS NULL OR tSCH.ShopOrder = @intShopOrder)
						AND (@vchItemNumber IS NULL OR tSCH.ItemNumber = @vchItemNumber)
						AND ShiftProductionDate	BETWEEN @dteFromProdDate AND @dteToProdDate
						--AND tPS.WorkGroup <> 'SetUp'
						AND tPS.WorkSubGroup <> 'SetUp'
					GROUP BY tSCH.Facility, tSCH.DefaultPkgLine, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo) tSCHSum
				LEFT OUTER JOIN vwLineWorkShiftType vLWT
					ON tSCHSum.Facility = vLWT.Facility AND tSCHSum.DefaultPkgLine =  vLWT.PackagingLine
				LEFT OUTER JOIN tblshift tS
					ON tSCHSum.Facility = tS.Facility AND tSCHSum.OverrideShiftNo = tS.Shift AND ISNULL(vLWT.WorkShiftType,'P') = tS.WorkGroup 
				LEFT OUTER JOIN (SELECT Facility, MachineID, ShopOrder, ShiftProductionDate, ShiftNo, SUM(PaidHours) as PaidHours
									FROM tblMachinePaidHours
									GROUP BY Facility, MachineID, ShopOrder, ShiftProductionDate, ShiftNo) tMPH
				ON tSCHSum.Facility = tMPH.Facility AND tSCHSUM.DefaultPkgLine = tMPH.MachineID AND tSCHSum.ShopOrder = tMPH.ShopOrder 
					AND tSCHSum.ShiftProductionDate = tMPH.ShiftProductionDate AND tSCHSum.OverrideShiftNo = tMPH.ShiftNo				
				LEFT OUTER JOIN cteAdj 
				ON tSCHSum.Facility = cteAdj.Facility AND tSCHSum.ShiftProductionDate = cteAdj.ShiftProductionDate AND tSCHSum.OverrideShiftNo = cteAdj.ShiftNo 
					AND tSCHSum.DefaultPkgLine = cteAdj.DefaultPkgLine And tSCHSum.ShopOrder = cteAdj.ShopOrder		
				GROUP BY tSCHSum.Facility, tSCHSum.ShiftProductionDate, tS.ShiftSequence, tSCHSum.OverrideShiftNo, tSCHSum.DefaultPkgLine, tSCHSum.ShopOrder, tSCHSum.ItemNumber
			END
			ELSE
			BEGIN
				;With cteAdj as
				(	
					-- Get adjustments for Line operator created pallets
					-- ShiftProduction date and shift is based the session history not from Pallet history
					-- (Note: currently the ShiftProduction date and shift of the pallet is calculated based on server system time)
					SELECT tSCH.Facility, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo as ShiftNo, tSCH.ShopOrder, tSCH.DefaultPkgLine, SUM(ISNULL(tPA.AdjustedQty,0)) as AdjustedQty 
					FROM tfnSessionControlHstDetail(NULL, @vchFacility, @vchMachineID, NULL, @intShopOrder, @vchItemNumber, @dteFromProdDate, @intFromShift, @dteToProdDate, @intToShift) tSCH
					INNER JOIN tblPalletHst tPH
					ON tSCH.Facility = tPH.Facility AND tSCH.ShopOrder = tPH.ShopOrder AND tSCH.DefaultPkgLine = tPH.DefaultPkgLine AND tSCH.Operator = tPH.Operator AND tSCH.StartTime = tPH.StartTime
					INNER JOIN (SELECT PalletID, SUM(AdjustedQty) as AdjustedQty FROM dbo.tfnPalletAdjustment('AdjWithCorrectionOnly',@vchFacility, @intShopOrder, @vchMachineID, NULL, NULL, NULL, NULL, NULL) 
								WHERE LatestReasonCode = '01'
								GROUP BY PalletID) tPA
					ON 	tPH.PalletID = tPA.PalletID
					GROUP BY tSCH.Facility, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo, tSCH.ShopOrder, tSCH.DefaultPkgLine
					--
					UNION
					--
					-- Get adjustments for Pallet Station created pallets
					-- Session Control does not log Pallet Station sessions so the ShiftProductionDate of the pallet is calculated based on the server system time
					SELECT tPH.Facility, tPH.ShiftProductionDate, tPH.ShiftNo, tPH.ShopOrder, tPH.DefaultPkgLine, SUM(ISNULL(tPA.AdjustedQty,0)) as AdjustedQty
					FROM tblPalletHst tPH
					INNER JOIN (SELECT PalletID, SUM(AdjustedQty) as AdjustedQty FROM dbo.tfnPalletAdjustment('AdjWithCorrectionOnly', @vchFacility, @intShopOrder,@vchMachineID, NULL, NULL, NULL, NULL, NULL)
									WHERE LatestReasonCode = '01'
									GROUP BY PalletID) tPA
					ON 	tPH.PalletID = tPA.PalletID
					INNER JOIN tblPlantStaff tPS
--WO#359			ON tPH.Operator = tPS.staffID
					ON tPH.Facility = tPS.Facility AND tPH.Operator = tPS.StaffID	--WO#359
					LEFT OUTER JOIN vwLineWorkShiftType vLWT
						ON tPH.Facility = vLWT.Facility AND tPH.DefaultPkgLine =  vLWT.PackagingLine
					LEFT OUTER JOIN tblshift tS
						ON tPH.Facility = tS.Facility AND tPH.ShiftNo = tS.Shift AND ISNULL(vLWT.WorkShiftType,'P') = tS.WorkGroup 
					WHERE tPS.WorkGroup = 'PS'
						AND (@vchFacility IS NULL OR tPH.Facility = @vchFacility)
						AND (@vchMachineID IS NULL OR tPH.DefaultPkgLine = @vchMachineID)
						AND (@intShopOrder IS NULL OR tPH.ShopOrder = @intShopOrder)
						AND (@vchItemNumber IS NULL OR tPH.ItemNumber = @vchItemNumber)
						AND Convert(varchar(8),tPH.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) 
							BETWEEN convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
					GROUP BY tPH.Facility, tPH.ShiftProductionDate, tPH.ShiftNo, tPH.ShopOrder, tPH.DefaultPkgLine	
				)

				INSERT INTO @tblSCH
				SELECT tSCHSum.Facility, tSCHSum.ShiftProductionDate, tSCHSum.ShiftSequence, tSCHSum.OverrideShiftNo, tSCHSum.DefaultPkgLine, tSCHSum.ShopOrder, tSCHSum.ItemNumber 
						,'' as Operator, SUM(tSCHSum.CasesProduced) as CasesProduced, SUM(ActRunTime) as ActRunTime
						,Round(SUM(ISNULL(tMPH.PaidHours, tSCHSum.ActRunTime)) ,2) as PaidRunTime
						,CAST(Round(SUM(ISNULL(cteAdj.AdjustedQty,0)),0) as int)
						,SUM(tSCHSum.ReworkWgt) as ReworkWgt
				FROM (
					SELECT tSCH.Facility, tSCH.ShiftProductionDate, tS.ShiftSequence, tSCH.OverrideShiftNo, tSCH.DefaultPkgLine, tSCH.ShopOrder 
						,tSCH.ItemNumber, Sum(CasesProduced) as CasesProduced
						,Round(Sum(Isnull(DateDiff(Second, tSCH.StartTime, tSCH.StopTime),0)) / 3600.00, 2) as ActRunTime
						,SUM(tSCH.ReworkWgt) as ReworkWgt
					FROM tblSessionControlHst tSCH
					LEFT OUTER JOIN tblPlantStaff tPS
						ON tSCH.Operator = tPS.StaffID AND tSCH.Facility = tPS.Facility
					LEFT OUTER JOIN vwLineWorkShiftType vLWT
						ON tSCH.Facility = vLWT.Facility AND tSCH.DefaultPkgLine =  vLWT.PackagingLine
					LEFT OUTER JOIN tblshift tS
						ON tSCH.Facility = tS.Facility AND tSCH.OverrideShiftNo = tS.Shift AND ISNULL(vLWT.WorkShiftType,'P') = tS.WorkGroup 
					WHERE (@vchFacility IS NULL OR tSCH.Facility = @vchFacility)
						AND (@vchMachineID IS NULL OR tSCH.DefaultPkgLine = @vchMachineID)
						AND (@intShopOrder IS NULL OR tSCH.ShopOrder = @intShopOrder)
						AND (@vchItemNumber IS NULL OR tSCH.ItemNumber = @vchItemNumber)
						AND Convert(varchar(8),ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) 
						BETWEEN convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) AND convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
						--AND tPS.WorkGroup <> 'SetUp'
						AND tPS.WorkSubGroup <> 'SetUp'
					GROUP BY tSCH.Facility, tSCH.ShiftProductionDate, tS.ShiftSequence, tSCH.OverrideShiftNo, tSCH.DefaultPkgLine, tSCH.ShopOrder, tSCH.ItemNumber) tSCHSum
				LEFT OUTER JOIN (SELECT Facility, MachineID, ShopOrder, ShiftProductionDate, ShiftNo, SUM(PaidHours) as PaidHours
									FROM tblMachinePaidHours
									GROUP BY Facility, MachineID, ShopOrder, ShiftProductionDate, ShiftNo) tMPH
				ON tSCHSum.Facility = tMPH.Facility AND tSCHSUM.DefaultPkgLine = tMPH.MachineID AND tSCHSum.ShopOrder = tMPH.ShopOrder 
					AND tSCHSum.ShiftProductionDate = tMPH.ShiftProductionDate AND tSCHSum.OverrideShiftNo = tMPH.ShiftNo				
				LEFT OUTER JOIN cteAdj 
				ON tSCHSum.Facility = cteAdj.Facility AND tSCHSum.ShiftProductionDate = cteAdj.ShiftProductionDate AND tSCHSum.OverrideShiftNo = cteAdj.ShiftNo 
					AND tSCHSum.DefaultPkgLine = cteAdj.DefaultPkgLine AND tSCHSum.ShopOrder = cteAdj.ShopOrder		
				GROUP BY tSCHSum.Facility, tSCHSum.ShiftProductionDate, tSCHSum.ShiftSequence, tSCHSum.OverrideShiftNo, tSCHSum.DefaultPkgLine, tSCHSum.ShopOrder, tSCHSum.ItemNumber
			END

	END


	RETURN 
END

GO

