
-- =============================================
-- Author:		Bong Lee
-- Create date: Apr.06 2010
-- Description:	Work Center Efficency Summary
-- WO#194:		Sep. 9, 2010	Bong Lee	
-- Description:	If the time frame for the expected shift crosses mid-night
--				and the given time is prior to mid-night, the Shift Production 
--				Date will be the given time plus one day. Before this change the 	
--				Shift Production Date for this case was same as the given time but 
--				if the given time after mid-night, the result was given time 
--				minus one day.
--				Do not include pallet adjustment to cases produced
-- WO#359:		Aug. 02, 2011	Bong Lee
--				Re-Write
-- WO#27470:		Aug. 27, 2019	Bong Lee
-- Description:	Routing rates are required to base on effective date
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_WorkCenterEfficiencySummary]
	@vchFacility varchar(3)
	,@chrShowShift as char = 'Y'
	,@intShiftFilter as tinyint
	,@dteFromProdDate as DateTime
	,@intFromShift as tinyint
	,@dteToProdDate as DateTime
	,@intToShift as tinyint
	-- WO#27470	,@intWorkCenter as int = NULL
	,@vchWorkCenters as varchar(MAX) = NULL
	,@vchItemNumber as varchar(35) = NULL						-- WO#27470
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRY
		
		DECLARE @tWgt TABLE
			(
			 Facility varchar(3)
			,OverrideShiftNo int
			,DefaultPkgLine varchar(10)
			,ShopOrder int
			,Operator varchar(10) 
			,TargetWeight decimal(10,4)
			-- WO#27470	,AvgOverPackWgt decimal(10,4
			,OverPackWgt decimal(10,4)		-- WO#27470
			,WgtRcdCnt int					-- WO#27470
			,Unique Clustered (Facility,OverrideShiftNo,DefaultPkgLine,ShopOrder,Operator)
			)

		DECLARE @tDTL TABLE
			(Facility varchar(3), 
			OverrideShiftNo int,
			MachineID varchar(10), 
			ShopOrder int,
			Operator varchar(10), 
			DownTime decimal(10,2)
			Unique Clustered (Facility,OverrideShiftNo,MachineID,ShopOrder,Operator))
		
		DECLARE @decLBtoGM as decimal(10,7)
		
		-- Get the Pound to Gram conversion rate
		SELECT @decLBtoGM = Value1 FROM tblControl 
			WHERE [KEY] = 'WeightConversion' AND SubKey = 'General'

/*
		StdUnitPerHr = Routing rate * (2 ** Base code)
		StdMachineHrEarnedInUnit = Cases Produced / StdUnitPerHr
		Units Produced = Cases Produced * Saleable Unit Per Case
		Actual Line Efficiency % = Case Produced / Acutal run time / StdUnitPerHr * 100%
*/
--		;With cteWgt --(Facility, PackagingLine, ShopOrder, Operator, AvgWgt, StdevWgt, TargetWgt, MinWgt, MaxWgt) 
--		AS (
		INSERT INTO @tWgt	
			SELECT tSCH.Facility, CASE WHEN @chrShowShift = 'Y' THEN tSCH.OverrideShiftNo ELSE ISNULL(@intShiftFilter, 0) END as OverrideShiftNo,  tSCH.DefaultPkgLine, tSCH.ShopOrder, tSCH.Operator
				-- WO#27470	,Avg(TargetWeight) as TargetWeight, AVG(ActualWeight-TargetWeight) AS AvgOverPackWgt
				,Max(TargetWeight) as TargetWeight, SUM(ActualWeight-TargetWeight) AS OverPackWgt, count(*) AS WgtRcdCnt 	-- WO#27470
			FROM dbo.tblWeightLog tWL
			-- WO#27470	INNER JOIN dbo.tfnSessionControlHstDetail(NULL,@vchFacility,NULL,NULL,NULL,NULL,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift) tSCH
			INNER JOIN dbo.tfnSessionControlHstDetail(NULL,@vchFacility,NULL,NULL,NULL,@vchItemNumber,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift) tSCH		-- WO#27470
			ON tWL.Facility = tSCH.Facility AND tWL.ShopOrder = tSCH.ShopOrder 
				AND	tWL.PackagingLine = tSCH.DefaultPkgLine AND tWL.ShopOrderStartTIme = tSCH.StartTIme
			LEFT OUTER JOIN tblEquipment tEQ														-- WO#27470
				ON tWL.Facility  = tEQ.Facility and tWL.PackagingLine = tEQ.EquipmentID				-- WO#27470
			WHERE tWL.WeightSource <> 2	-- exlcluded tareweight records	
				AND (@intShiftFilter IS NULL OR tSCH.OverrideShiftNo = @intShiftFilter)
				AND tWL.ItemNumber = ISNULL(@vchItemNumber, tWL.ItemNumber)							-- WO#27470
				-- WO#27470	AND tEQ.WorkCenter = ISNULL(@intWorkCenter, tEQ.WorkCenter)							-- WO#27470
				AND (@vchWorkCenters IS NULL OR tEQ.WorkCenter IN (SELECT value FROM STRING_SPLIT(@vchWorkCenters, ','))	)	-- WO#27470
			GROUP BY tSCH.Facility, CASE WHEN @chrShowShift = 'Y' THEN tSCH.OverrideShiftNo ELSE ISNULL(@intShiftFilter, 0) END, tSCH.DefaultPkgLine, tSCH.ShopOrder, tSCH.Operator
--		),

--		cteDTL AS
--		(
--			SELECT Facility, OverrideShiftNo, MachineID, ShopOrder, Operator, Sum(MaxDownTime)/60.00 as DownTime 
--			FROM
--				(SELECT tDTL.Facility, CASE WHEN @chrShowShift = 'Y' THEN tSCH.OverrideShiftNo ELSE 0 END as OverrideShiftNo, tDTL.MachineID, tDTL.ShopOrder, tDTL.Operator
--					,tDTL.DownTimeBegin	,MAX(DateDiff(Second,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) as MaxDownTime
--				FROM dbo.tblDownTimeLog tDTL
--				Inner join dbo.tfnSessionControlHstDetail(NULL,@vchFacility,NULL,NULL,NULL,NULL,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift) tSCH
--				ON tDTL.Facility = tSCH.Facility AND tDTL.ShopOrder = tSCH.ShopOrder 
--					AND tDTL.MachineID = tSCH.DefaultPkgLine AND tDTL.EventID = tSCH.StartTIme
--				WHERE Facility = @vchFacility AND InActive = 0 
--					AND (@intShiftFilter IS NULL OR tSCH.OverrideShiftNo = @intShiftFilter)
--				GROUP BY tDTL.Facility, CASE WHEN @chrShowShift = 'Y' THEN tSCH.OverrideShiftNo ELSE 0 END, tDTL.MachineID, tDTL.ShopOrder, tDTL.Operator, DownTimeBegin) tDL
--			GROUP BY Facility, OverrideShiftNo, MachineID, ShopOrder, Operator
		INSERT INTO @tDTL
			SELECT Facility, Shift as OverrideShiftNo, MachineID, ShopOrder, Operator, Sum(MaxDownTime)/60.00 as DownTime 
			FROM
				-- WO#27470	(SELECT tDTL.Facility, CASE WHEN @chrShowShift = 'Y' THEN tDTL.Shift ELSE ISNULL(@intShiftFilter, 0)  END as Shift, MachineID, tDTL.ShopOrder, Operator
				-- WO#27470		,tDTL.DownTimeBegin	,MAX(DateDiff(Second, tDTL.DownTimeBegin, tDTL.DownTimeEnd)) as MaxDownTime
				-- WO#27470 ADD Start
				(SELECT tDTL.Facility, CASE WHEN @chrShowShift = 'Y' THEN tDTL.Shift ELSE ISNULL(@intShiftFilter, 0)  END as Shift, tDTL.MachineID, tDTL.ShopOrder, tDTL.Operator	
					,tDTL.DownTimeBegin	, DateDiff(Second, tDTL.DownTimeBegin, tDTL.DownTimeEnd) as MaxDownTime										
					,Rank() OVER(PARTITION BY tDTL.MachineID, tDTL.Operator, tDTL.DownTimeBegin 
								ORDER BY tDTL.MachineID, tDTL.Operator,	tDTL.DownTimeBegin, DateDiff(minute, tDTL.DownTimeBegin, tDTL.DownTimeEnd) DESC, creationdate) AS SeqNo
				-- WO#27470	ADD Stop
				FROM tblDownTimeLog tDTL
				LEFT OUTER JOIN vwLineWorkShiftType vLWT
					ON tDTL.Facility = vLWT.Facility AND tDTL.MachineID =  vLWT.PackagingLine
				LEFT OUTER JOIN tblshift tS
					ON tDTL.Facility = tS.Facility AND tDTL.Shift = tS.Shift AND ISNULL(vLWT.WorkShiftType,'P') = tS.WorkGroup 
				LEFT OUTER JOIN tblEquipment tEQ															-- WO#27470
					ON tDTL.Facility  = tEQ.Facility and tDTL.MachineID = tEQ.EquipmentID					-- WO#27470
				LEFT OUTER JOIN tblShopOrderHst tSOH														-- WO#27470
					ON tDTL.Facility  = tSOH.Facility and tDTL.ShopOrder = tSOH.ShopOrder					-- WO#27470
				WHERE tDTL.Facility = @vchFacility AND tDTL.InActive = 0 
					AND (@intShiftFilter IS NULL OR tDTL.Shift = @intShiftFilter)
					AND Convert(varchar(8),ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) 
						BETWEEN convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) AND convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
					-- WO#27470		AND tEQ.WorkCenter = ISNULL(@intWorkCenter, tEQ.WorkCenter)	
					AND (@vchWorkCenters IS NULL OR tEQ.WorkCenter IN (SELECT value FROM STRING_SPLIT(@vchWorkCenters, ','))	)	-- WO#27470							-- WO#27470
					AND (@vchItemNumber is NULL OR @vchItemNumber = tSOH.ItemNumber)						-- WO#27470
				-- WO#27470	GROUP BY tDTL.Facility, CASE WHEN @chrShowShift = 'Y' THEN tDTL.Shift ELSE ISNULL(@intShiftFilter, 0) END, tDTL.MachineID, tDTL.ShopOrder, tDTL.Operator, tDTL.DownTimeBegin) tDL
				) tDL					-- WO#27470
				WHERE SeqNo = 1			-- WO#27470
			GROUP BY Facility, Shift, MachineID, ShopOrder, Operator
--		),

--		cteSCH AS
		;With cteSCH AS
		(
			-- WO#27470 SELECT tSCH.Facility, CASE WHEN @chrShowShift = 'Y' THEN tSCH.ShiftSequence ELSE 0 END AS ShiftSequence, CASE WHEN @chrShowShift = 'Y' THEN tSCH.ShiftNo ELSE ISNULL(@intShiftFilter, 0) END AS ShiftNo
			SELECT tSCH.Facility																					-- WO#27470
				,tSCH.ShiftProductionDate																			-- WO#27470
				,CASE WHEN @chrShowShift = 'Y' THEN tSCH.ShiftSequence ELSE 0 END AS ShiftSequence					-- WO#27470
				,CASE WHEN @chrShowShift = 'Y' THEN tSCH.ShiftNo ELSE ISNULL(@intShiftFilter, 0) END AS ShiftNo		-- WO#27470
				,tSCH.DefaultPkgLine, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator
				,SUM(tSCH.CasesProduced) as CasesProduced 
				,SUM(tSCH.AdjustedQty) as AdjustedQty
				,SUM(tSCH.ActRunTime) as ActRunTime
				,SUM(tSCH.PaidRunTime) as PaidRunTime
			-- WO#27470	FROM tfnSessionControlHstSummary ('WithAdjByOpr',@vchFacility,NULL,NULL,NULL,NULL,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift) as tSCH
			FROM tfnSessionControlHstSummary ('WithAdjByOpr',@vchFacility,NULL,NULL,NULL,@vchItemNumber,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift) as tSCH		-- WO#27470
			LEFT OUTER JOIN tblEquipment tEQ
			ON tSCH.Facility = tEQ.Facility and tSCH.DefaultPkgLine = tEQ.EquipmentID
			WHERE(@intShiftFilter IS NULL OR ShiftNo = @intShiftFilter)
				-- WO#27470	AND (@intWorkCenter Is NULL OR ISNULL(tEQ.WorkCenter,Left(tSCH.DefaultPkgLine,4)) = @intWorkCenter)
				-- WO#27470	GROUP BY tSCH.Facility, CASE WHEN @chrShowShift = 'Y' THEN tSCH.ShiftSequence ELSE 0 END, CASE WHEN @chrShowShift = 'Y' THEN tSCH.ShiftNo ELSE ISNULL(@intShiftFilter, 0) END, tSCH.DefaultPkgLine, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator
				-- WO#27470 ADD Start
				-- WO#27470		AND (@intWorkCenter Is NULL OR ISNULL(tEQ.WorkCenter,Left(tSCH.DefaultPkgLine,4)) = Cast(@intWorkCenter as varchar(10)))	-- WO#27470	
					AND (@vchWorkCenters Is NULL OR CAST(ISNULL(tEQ.WorkCenter,Left(tSCH.DefaultPkgLine,4)) as int) IN (SELECT value FROM STRING_SPLIT(@vchWorkCenters, ',')))	-- WO#27470	
			GROUP BY tSCH.Facility
				,tSCH.ShiftProductionDate
				,CASE WHEN @chrShowShift = 'Y' THEN tSCH.ShiftSequence ELSE 0 END
				,CASE WHEN @chrShowShift = 'Y' THEN tSCH.ShiftNo ELSE ISNULL(@intShiftFilter, 0) END
				,tSCH.DefaultPkgLine
				,tSCH.ShopOrder
				,tSCH.ItemNumber
				,tSCH.Operator
				-- WO#27470 ADD Stop
		)
/* WO#27470 DEL Start
		-- WO#27470	SELECT tSCH.Facility, tSCH.ShiftSequence, tSCH.ShiftNo, tSCH.DefaultPkgLine, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator, (tSCH.CasesProduced + ISNULL(tSCH.AdjustedQty,0)) as CasesProduced
		-- WO#27470	,tSCH.ActRunTime, tSCH.PaidRunTime, tEQ.Description, Round(tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit / (Case When tIM.labelweightUOM = 'GM' Then @decLBtoGM Else 1 End),3) as NetWeight 
			-- WO#27470 ,Round((tSCH.CasesProduced + ISNULL(tSCH.AdjustedQty,0)) * Round(tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit/ (Case When tIM.labelweightUOM = 'GM' Then @decLBtoGM Else 1 End),3),0) as PoundProduced 
			-- WO#27470	,CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0)) END as StdUnitPerHr
			-- WO#27470	,CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else (tSCH.CasesProduced + ISNULL(tSCH.AdjustedQty,0))/ (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit
			-- WO#27470	,ISNULL(tSMER_ID.StdWorkCenterEfficiency,ISNULL(tSMER_WC.StdWorkCenterEfficiency,0)) as StdWCEfficiency
			-- WO#27470	,tPS.OperatorName, IsNull(tDTL.DownTime,0) as DownTime, ISNULL(tSCH.AdjustedQty,0) as AdjustedCases
			-- WO#27470	,Round((tSCH.CasesProduced + ISNULL(tSCH.AdjustedQty,0)) * ISNULL(tIM.SaleableUnitPerCase,0) * ISNULL(tIM.PackagesPerSaleableUnit,0) ,0) AS UnitsProduced
			-- WO#27470,ISNULL(tEQ.WorkCenter,Left(tSCH.DefaultPkgLine,4)) as WorkCenter, tWC.Description as WorkCenterDesc, cteWgt.TargetWeight
			-- WO#27470	,cteWgt.AvgOverPackWgt
WO#27470 DEL Stop */
		-- WO#27470 ADD Start
		SELECT 
			ISNULL(tSCH.Facility,tDTL.Facility) as Facility
			,CASE WHEN @chrShowShift = 'Y' THEN ISNULL(tSCH.ShiftSequence,tS.ShiftSequence) ELSE 0 END AS ShiftSequence
			,ISNULL(tSCH.ShiftNo, tDTL.OverrideShiftNo) as ShiftNo				
			,ISNULL(tSCH.DefaultPkgLine, tDTL.MachineID) as DefaultPkgLine		
			,tSCH.ShopOrder
			,tSCH.ItemNumber
			,ISNULL(tSCH.Operator, tDTL.Operator) as Operator					
			,ISNULL((tSCH.CasesProduced + ISNULL(tSCH.AdjustedQty,0)),0) as CasesProduced
			,ISNULL(tSCH.ActRunTime,0.00) as ActRunTime
			,ISNULL(tSCH.PaidRunTime,0.00) as PaidRunTime
			,tEQ.Description
			,ISNULL(Round(tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit / (Case When tIM.labelweightUOM = 'GM' Then @decLBtoGM Else 1 End),3) ,0) as NetWeight 
			,ISNULL(Round((tSCH.CasesProduced + ISNULL(tSCH.AdjustedQty,0)) * Round(tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit/ (Case When tIM.labelweightUOM = 'GM' Then @decLBtoGM Else 1 End),3),0),0) as PoundProduced 
			,tSCH.StdUnitPerHr  
			,ISNULL(tSCH.StdMachineHrEarnedInUnit,0) as StdMachineHrEarnedInUnit
			,ISNULL(tSCH.StdWCEfficiency,0) as StdWCEfficiency
			,tPS.OperatorName
			,IsNull(tDTL.DownTime,0) as DownTime
			,ISNULL(tSCH.AdjustedQty,0) as AdjustedCases
			,ISNULL(Round((tSCH.CasesProduced + ISNULL(tSCH.AdjustedQty,0)) * ISNULL(tIM.SaleableUnitPerCase,0) * ISNULL(tIM.PackagesPerSaleableUnit,0) ,0),0) AS UnitsProduced
			,ISNULL(tEQ.WorkCenter, Left(ISNULL(tSCH.DefaultPkgLine,tDTL.MachineID),4)) as WorkCenter
			,tWC.Description WorkCenterDesc	
			,ISNULL(cteWgt.TargetWeight,0) as TargetWeight
			,ISNULL(cteWgt.OverPackWgt,0) as OverPackWgt
			,ISNULL(cteWgt.WgtRcdCnt,0)	as WgtRcdCnt
		-- WO#27470 ADD Stop
			,tIM.ItemDesc1 + ' ' + LTRIM(tIM.itemDesc2) as ItemDesc
		-- WO#27470 ADD Start
		FROM (
			SELECT 
				tSSCH.Facility
				,tSSCH.ShiftSequence
				,tSSCH.ShiftNo				
				,tSSCH.DefaultPkgLine		
				,tSSCH.ShopOrder
				,tSSCH.ItemNumber
				,tSSCH.Operator					
				,SUM(tSSCH.CasesProduced) as CasesProduced
				,SUM(tSSCH.AdjustedQty) as AdjustedQty
				,SUM(tSSCH.ActRunTime) as ActRunTime
				,SUM(tSSCH.PaidRunTime) as PaidRunTime
				,AVG(CASE When ISNULL(tfSMER.MachineHours,0)= 0 Then 0 Else POWER(10,ISNULL(tfSMER.BasisCode,0)) / tfSMER.MachineHours END) as StdUnitPerHr
				,SUM(CASE When ISNULL(tfSMER.MachineHours,0)= 0 Then 0 Else (tSSCH.CasesProduced + ISNULL(tSSCH.AdjustedQty,0))/ (POWER(10,ISNULL(tfSMER.BasisCode,0)) / tfSMER.MachineHours) End) As StdMachineHrEarnedInUnit
				,AVG(IIF(tfSMER.StdWorkCenterEfficiency=0,NULL,tfSMER.StdWorkCenterEfficiency)) as StdWCEfficiency
			FROM cteSCH tSSCH	
				OUTER APPLY [dbo].[tfnStdMachineEfficiencyRate] (tSSCH.Facility, tSSCH.ItemNumber, tSSCH.DefaultPkgLine, LEFT(tSSCH.DefaultPkgLine,4), tSSCH.ShiftProductionDate) as tfSMER
			GROUP BY tSSCH.Facility
				,tSSCH.ShiftSequence
				,tSSCH.ShiftNo
				,tSSCH.DefaultPkgLine
				,tSSCH.ShopOrder
				,tSSCH.ItemNumber
				,tSSCH.Operator
		) tSCH
		-- WO#27470 ADD Stop
		LEFT OUTER JOIN tblItemMaster tIM
			On tSCH.Facility = tIM.Facility and tSCH.ItemNumber = tIM.ItemNumber
/* WO#27470 DEL Start
		LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_ID
			ON tSCH.Facility = tSMER_ID.Facility AND tIM.ItemNumber = tSMER_ID.ItemNumber AND tSCH.DefaultPkgLine = tSMER_ID.MachineID
		LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_WC
			ON tSCH.Facility = tSMER_WC.Facility AND tIM.ItemNumber = tSMER_WC.ItemNumber AND CAST(SUBSTRING(tSCH.DefaultPkgLine,1,4) as int) = tSMER_WC.WorkCenter and tSMER_WC.MachineID = ''
		LEFT OUTER JOIN tblEquipment tEQ
			ON tSCH.Facility = tEQ.Facility and tSCH.DefaultPkgLine = tEQ.EquipmentID
		--LEFT OUTER JOIN (Select Distinct StaffID, FirstName + ' ' + LastName as OperatorName from tblPlantStaff with (nolock) Where Facility = @vchFacility ) tPS 
		--	ON tSCH.Operator = tPS.StaffID 
		-- WO#27470		LEFT OUTER JOIN @tDTL tDTL
		LEFT OUTER JOIN tblWorkCenter tWC
			ON tSCH.Facility = tWC.Facility AND LEFT(tSCH.DefaultPkgLine,4) = tWC.WorkCenter
WO#27470 DEL Stop */
-- WO#27470 ADD Start
		FULL JOIN @tDTL tDTL						-- WO#27470	
			On tSCH.Facility = tDTL.Facility AND tSCH.ShiftNo = tDTL.OverrideShiftNo AND tSCH.DefaultPkgLine = tDTL.MachineID 
				AND tSCH.ShopOrder = tDTL.ShopOrder AND tSCH.Operator = tDTL.Operator
		LEFT OUTER JOIN tblEquipment tEQ
			ON tEQ.Facility =ISNULL(tSCH.Facility,tDTL.Facility) and  tEQ.EquipmentID = ISNULL(tSCH.DefaultPkgLine, tDTL.MachineID)

		LEFT OUTER JOIN (Select Distinct StaffID, FirstName + ' ' + LastName as OperatorName from tblPlantStaff with (nolock) Where Facility = @vchFacility ) tPS 
			on tPS.StaffID = ISNULL(tSCH.Operator, tDTL.Operator)

		LEFT OUTER JOIN tblWorkCenter tWC
			ON  tWC.Facility = ISNULL(tSCH.Facility,tDTL.Facility)  AND tWC.WorkCenter = tEQ.WorkCenter					
		LEFT OUTER JOIN tblShift tS																			
		ON tDTL.Facility = tS.facility AND tDTL.OverrideShiftNo = tS.Shift AND  tS.WorkGroup = 
			(Select vWST.WorkShiftType from vwLineWorkShiftType vWST where tDTL.Facility =vWST.facility AND tDTL.MachineID = vWST.PackagingLine)
			--(Select tCC.WorkShiftType from tblComputerConfig tCC where tDTL.Facility = tCC.facility AND tDTL.MachineID = tCC.PackagingLine)
-- WO#27470 ADD Stop
		LEFT OUTER JOIN @tWgt cteWgt
			On tSCH.Facility = cteWgt.Facility AND tSCH.ShiftNo = cteWgt.OverrideShiftNo AND tSCH.DefaultPkgLine = cteWgt.DefaultPkgLine 
				AND tSCH.ShopOrder = cteWgt.ShopOrder And tSCH.Operator = cteWgt.Operator
		-- WO#27470	WHERE (@intShiftFilter IS NULL OR tSCH.ShiftNo = @intShiftFilter)
		WHERE (@intShiftFilter IS NULL OR ISNULL(tSCH.ShiftNo,tDTL.OverrideShiftNo) = @intShiftFilter)		-- WO#27470	

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

