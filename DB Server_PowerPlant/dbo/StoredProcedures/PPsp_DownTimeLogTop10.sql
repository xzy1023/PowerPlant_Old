


-- =============================================
-- Author:		Bong Lee
-- Create date: Oct 19, 2009
-- Description:	Down Time Summary by Line and shift
-- WO#194:		Sep. 9, 2010	Bong Lee	
-- Description:	If the time frame for the expected shift crosses mid-night
--				and the given time is prior to mid-night, the Shift Production 
--				Date will be the given time plus one day. Before this change the 	
--				Shift Production Date for this case was same as the given time but 
--				if the given time after mid-night, the result was given time 
--				minus one day.
-- WO#359		Jun. 17, 2011	Bong Lee
-- Description:	Add *ALL opton on Report Type & Value to show Top 10 down time for over all.
--				Add Utility Tech. opton on Report Type & Value to show Top 10 down time for Utility Tech.
--				Add a new option to allow to filter by shift in the selected time range.
-- WO#3771		Sep. 22, 2016	Bong Lee
--				Add filter by work center
-- IC#9485		Add parameter @intTopCount to allow user to select nomber of "Top" downtime count.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_DownTimeLogTop10]
	@vchAction varchar(50),
	@vchFacility varchar(3),
--	@vchOperator varchar(10) = NULL,
--	@vchMachineID varchar(10) = NULL,
	-- @vchSelectedValue can be a Line or operator or Utility Tech.
	@vchSelectedValue varchar(10) = NULL,	-- WO#359 
	@dteFromProdDate as DateTime,
	@intFromShift as tinyint,
	@dteToProdDate as DateTime,
	@intToShift as tinyint,
	@intShiftNoFilter as tinyint = NULL,	-- WO#359
	@intTopCount as int					-- IC#9485
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRY

-- WO#359 Del Start 
-- WO#194 Add Start 
		/* Find the Work Shift type for each line - if line is duplicated, pick the active one */
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
--		WHERE (T2.Packagingline is null OR T1.RecordStatus = 1) 
--			AND T1.PackagingLine <> 'SPARE'
--		Group By T1.Facility, T1.Packagingline, T1.WorkShiftType
-- WO#194 Add Stop
-- WO#359 Del Stop

	IF @vchAction = 'ByLine'
	BEGIN
-- WO#194 Add Start
		;With cteSCH AS
		(
-- WO#359 Del Start
--			SELECT tSCH.Facility, tSCH.DefaultPkgLine, Round(SUM(ISNULL(tMPH.PaidHours, tSCH.ActRunTime)) * 60 ,2) as PaidRunTime 
--			FROM (
--				SELECT tSCH.Facility, DefaultPkgLine, ShopOrder, ItemNumber, Operator, ShiftProductionDate, OverrideShiftNo, sum(CasesProduced) as CasesProduced,
--					Round(Sum(Isnull(DateDiff(Hour,StartTime,StopTime),0)), 2) as ActRunTime
--				FROM tblSessionControlHst tSCH
--					Left Outer Join @tblComputerConfig tCC
--					ON tSCH.Facility = tCC.Facility AND tSCH.OverridePkgLine =  tCC.PackagingLine
--					Left Outer Join tblshift tS
--					ON tSCH.Facility = tS.Facility AND tSCH.OverrideShiftNo = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
--				WHERE tSCH.Facility = @vchFacility and Convert(varchar(8),ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1)) 	-- WO#194
--					AND ((tSCH.Facility not in (SELECT facility FROM dbo.tblFacility WHERE Region = '01') OR Len(Rtrim(Operator)) > 3) )
--					AND (@vchMachineID is NULL or @vchMachineID = DefaultPkgLine)
--				GROUP BY tSCH.Facility, DefaultPkgLine,ShopOrder,ItemNumber,Operator, ShiftProductionDate, OverrideShiftNo) tSCH
--				Left Outer Join tblMachinePaidHours tMPH
--			ON tSCH.Facility = tMPH.Facility AND tSCH.Operator = tMPH.Operator AND tSCH.DefaultPkgLine = tMPH.MachineID AND tSCH.ShopOrder = tMPH.ShopOrder 
--				AND tSCH.ShiftProductionDate = tMPH.ShiftProductionDate AND tSCH.OverrideShiftNo = tMPH.ShiftNo				
--			GROUP BY tSCH.Facility, tSCH.DefaultPkgLine
-- WO#359 Del Stop
-- WO#359 Add Start
			SELECT Facility, DefaultPkgLine, SUM(PaidRunTime * 60) as PaidRunTime
			FROM dbo.tfnSessionControlHstSummary('WithoutAdj', @vchFacility, @vchSelectedValue, NULL, NULL, NULL, @dteFromProdDate, @intFromShift,	@dteToProdDate,	@intToShift)
			WHERE (@intShiftNoFilter is NULL OR ShiftNo = @intShiftNoFilter)
			GROUP BY Facility, DefaultPkgLine
-- WO#359 Add Stop
		),

		cteDTL AS (
-- WO#194 Add End

		-- IC#9485 Select Top 10 tDTL.Facility, tDTL.MachineId as ID, tE.Description as Description, tDTL.ReasonCode, tRC.Description as RCDescription, count(*) as Occurances, Sum(DateDiff(Minute,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) as DownTime 
		Select Top (@intTopCount) tDTL.Facility, tDTL.MachineId as ID, tE.Description as Description, tDTL.ReasonCode, tRC.Description as RCDescription, count(*) as Occurances, Sum(DateDiff(Minute,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) as DownTime		-- IC#9485
		From tblDownTimeLog tDTL
-- WO#194 Add Start 
-- WO#359 Left Outer Join @tblComputerConfig tCC
		Left Outer Join dbo.vwLineWorkShiftType tCC		-- WO#359
		ON tDTL.Facility = tCC.Facility AND tDTL.MachineId =  tCC.PackagingLine
		Left Outer Join tblShift tS
-- WO#359	ON tDTL.Facility = tS.Facility AND tDTL.Shift = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
		ON tDTL.Facility = tS.Facility AND tDTL.Shift = tS.Shift AND ISNULL(tCC.WorkShiftType,'P')  = tS.WorkGroup	-- WO#359

-- WO#194 Add End
		left outer join tblDTReasonCode tRC
		On tDTL.Facility = tRC.Facility AND tDTL.MachineType =  tRC.MachineType 
			And tDTL.MachineSubType =  tRC.MachineSubType And tDTL.ReasonCode =  tRC.ReasonCode
		Left Outer Join tblEquipment tE
		On tDTL.Facility = tE.Facility AND tDTL.MachineID =  tE.EquipmentID 
		Where tDTL.Facility = @vchFacility 
			AND InActive = 0 
			AND (@vchSelectedValue is Null OR tDTL.MachineId =  @vchSelectedValue)	-- WO#359
-- WO#359	AND (@vchMachineID is Null OR tDTL.Machineid =  @vchMachineID)
-- WO#3771	AND  tDTL.ReasonType <> 17
			AND ((@vchFacility = '01' AND tDTL.ReasonType <> 17) OR (@vchFacility <>'01'))		-- WO#3771
			AND Convert(varchar(8),tDTL.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1)) -- WO#194
-- WO#194	AND Convert(varchar(8),tDTL.ShiftProductionDate,112) + Cast(tDTL.Shift as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
			AND (@intShiftNoFilter is NULL OR tDTL.Shift = @intShiftNoFilter)  -- WO#359
		Group by tDTL.Facility, tDTL.MachineId, tE.Description, tDTL.ReasonCode, tRC.Description
		Order by Sum(DateDiff(Minute,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) desc 
-- WO#194 Add Start 
		)

	SELECT cteDTL.* , cteSCH.PaidRunTime
		FROM cteDTL
		LEFT OUTER JOIN cteSCH
		ON cteDTL.Facility = cteSCH.Facility	-- WO#359
			AND cteDTL.Id = cteSCH.DefaultPkgLine
		ORDER BY DownTime Desc
-- WO#194 Add End

	END
	ELSE
	IF @vchAction = 'ByOperator'
	BEGIN
-- WO#194 Add Start
		;With cteSCH AS
		(
-- WO#359 Del Start
--			SELECT tSCH.Facility, tSCH.Operator, Round(SUM(ISNULL(tMPH.PaidHours, tSCH.ActRunTime)) * 60 ,2) as PaidRunTime 
--			FROM (
--				SELECT tSCH.Facility, DefaultPkgLine, ShopOrder, ItemNumber, Operator, ShiftProductionDate, OverrideShiftNo, sum(CasesProduced) as CasesProduced,
--					Round(Sum(Isnull(DateDiff(Hour,StartTime,StopTime),0)), 2) as ActRunTime
--				FROM tblSessionControlHst tSCH
--					Left Outer Join dbo.tblPlantStaff tPS
--					On tSCH.Facility = tPS.Facility AND tSCH.Operator =  tPS.StaffID
--					Left Outer Join @tblComputerConfig tCC
--					ON tSCH.Facility = tCC.Facility AND tSCH.OverridePkgLine =  tCC.PackagingLine
--					Left Outer Join tblshift tS
--					ON tSCH.Facility = tS.Facility AND tSCH.OverrideShiftNo = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
--				WHERE tSCH.Facility = @vchFacility and Convert(varchar(8),ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1)) 	-- WO#194
--					AND ((tSCH.Facility not in (SELECT facility FROM dbo.tblFacility WHERE Region = '01') OR Len(Rtrim(Operator)) > 3) )
--					AND (@vchOperator is Null OR tSCH.Operator =  @vchOperator) AND (tPS.WorkGroup = 'P' or tPS.WorkGroup = 'ALL')
--				GROUP BY tSCH.Facility, DefaultPkgLine,ShopOrder,ItemNumber,Operator, ShiftProductionDate, OverrideShiftNo) tSCH
--				Left Outer Join tblMachinePaidHours tMPH
--			ON tSCH.Facility = tMPH.Facility AND tSCH.Operator = tMPH.Operator AND tSCH.DefaultPkgLine = tMPH.MachineID AND tSCH.ShopOrder = tMPH.ShopOrder 
--				AND tSCH.ShiftProductionDate = tMPH.ShiftProductionDate AND tSCH.OverrideShiftNo = tMPH.ShiftNo				
--			GROUP BY tSCH.Facility, tSCH.Operator
-- WO#359 Del Stop
-- WO#359 Add Start
			SELECT Facility, Operator, SUM(PaidRunTime * 60) as PaidRunTime
			FROM dbo.tfnSessionControlHstSummary('WithoutAdj', @vchFacility, NULL, @vchSelectedValue, NULL, NULL, @dteFromProdDate, @intFromShift,	@dteToProdDate,	@intToShift)
			WHERE (@intShiftNoFilter is NULL OR ShiftNo = @intShiftNoFilter)
			GROUP BY Facility, Operator
-- WO#359 Add Stop
		),

		cteDTL AS (
-- WO#194 Add End
-- WO#359 Select Top 10 tDTL.Facility, tDTL.Operator as ID, tPS.FirstName + ' ' + tPS.LastName as Description, tDTL.ReasonCode, tRC.Description  as RCDescription, count(*) as Occurances, Sum(DateDiff(Minute,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) as DownTime 
-- WO#359 Add Start 
		-- IC#9485 Select Top 10 tDTL.Facility, tDTL.Operator as ID
		Select Top (@intTopCount) tDTL.Facility, tDTL.Operator as ID	-- IC#9485
			,ISNULL(tPS.FirstName + ' ' + tPS.LastName, '** Unknown **') as Description 
			,tDTL.ReasonCode, tRC.Description  as RCDescription, count(*) as Occurances
			, Sum(DateDiff(Minute,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) as DownTime 	
-- WO#359 Add Stop 
		From tbldowntimelog tDTL
-- WO#194 Add Start 
-- WO#359 Left Outer Join @tblComputerConfig tCC
		Left Outer Join dbo.vwLineWorkShiftType tCC		-- WO#359
		ON tDTL.Facility = tCC.Facility AND tDTL.MachineId =  tCC.PackagingLine
		Left Outer Join tblShift tS
-- WO#359	ON tDTL.Facility = tS.Facility AND tDTL.Shift = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 		
		ON tDTL.Facility = tS.Facility AND tDTL.Shift = tS.Shift AND ISNULL(tCC.WorkShiftType,'P') = tS.WorkGroup	--WO#359
-- WO#194 Add End
		left outer join tblDTReasonCode tRC
		On tDTL.Facility = tRC.Facility AND tDTL.MachineType =  tRC.MachineType 
			And tDTL.MachineSubType =  tRC.MachineSubType And tDTL.ReasonCode =  tRC.ReasonCode
		Left Outer Join dbo.tblPlantStaff tPS
		On tDTL.Facility = tPS.Facility AND tDTL.Operator =  tPS.StaffID
		Where tDTL.Facility = @vchFacility 
			AND InActive = 0 AND (@vchSelectedValue is Null OR tDTL.Operator = @vchSelectedValue) -- WO#359
-- WO#359	AND InActive = 0 AND (@vchOperator is Null OR tDTL.Operator =  @vchOperator) 
-- WO#359	AND (tPS.WorkGroup = 'P' OR tPS.WorkGroup = 'ALL')
-- WO#3771	AND  tDTL.ReasonType <> 17
			AND ((@vchFacility = '01' AND tDTL.ReasonType <> 17) OR (@vchFacility <>'01'))		-- WO#3771
			AND Convert(varchar(8),tDTL.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1)) -- WO#194
-- WO#194	AND Convert(varchar(8),tDTL.ShiftProductionDate,112) + Cast(tDTL.Shift as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
			AND (@intShiftNoFilter is NULL OR tDTL.Shift = @intShiftNoFilter)  -- WO#359
		Group by tDTL.Facility, tDTL.Operator, tPS.FirstName + ' ' + tPS.LastName, tDTL.ReasonCode, tRC.Description
		Order by Sum(DateDiff(Minute,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) desc 
-- WO#194 Add Start 
		)

	SELECT cteDTL.* , cteSCH.PaidRunTime
		FROM cteDTL
		LEFT OUTER JOIN cteSCH
		ON 	cteDTL.Facility = cteSCH.Facility	-- WO#359
			AND cteDTL.ID = cteSCH.Operator 
		ORDER BY DownTime Desc
-- WO#194 Add End 

	END
-- WO#359 Add Start 
	ELSE
	IF @vchAction = 'ByUT'	-- Utility Tech.
	BEGIN
		;With cteSCH AS
		(
			SELECT tSCH.Facility, tOS.StaffID as UtilityTech
				,Round(Sum(Isnull(DateDiff(Second, tSCH.StartTime, tSCH.StopTime),0)) / 60.00, 2) as PaidRunTime
			FROM dbo.tfnSessionControlHstDetail(NULL, @vchFacility, NULL, NULL, NULL, NULL, @dteFromProdDate, @intFromShift, @dteToProdDate,	@intToShift) tSCH
			Left Outer Join tblOperationStaffing tOS
			On tSCH.Facility = tOS.Facility AND tSCH.DefaultPkgLine = tOS.PackagingLine AND tSCH.StartTime = tOS.StartTime
			WHERE (@intShiftNoFilter is NULL OR tSCH.OverrideShiftNo = @intShiftNoFilter) 
				AND (@vchSelectedValue is NULL or tOS.StaffID = @vchSelectedValue)
			GROUP BY tSCH.Facility, tOS.StaffID
		)
		,cteDTL AS (
		-- IC#9485 Select Top 10 tDTL.Facility, tOS.StaffID as ID
		Select Top (@intTopCount) tDTL.Facility, tOS.StaffID as ID	-- IC#9485
			,ISNULL(tPS.FirstName + ' ' + tPS.LastName, '** Unknown **') as Description
			,tDTL.ReasonCode, tRC.Description  as RCDescription, count(*) as Occurances
			,Sum(DateDiff(Minute,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) as DownTime 
		From tbldowntimelog tDTL
		Inner Join tblOperationStaffing tOS
		On tDTL.Facility = tOS.Facility AND tDTL.MachineId = tOS.PackagingLine AND Cast(tDTL.EventID as datetime) =tOS.StartTime
		Left Outer Join dbo.vwLineWorkShiftType tCC	
		ON tDTL.Facility = tCC.Facility AND tDTL.MachineId =  tCC.PackagingLine
		Left Outer Join tblShift tS
		ON tDTL.Facility = tS.Facility AND tDTL.Shift = tS.Shift AND ISNULL(tCC.WorkShiftType,'P') = tS.WorkGroup
		left outer join tblDTReasonCode tRC
		On tDTL.Facility = tRC.Facility AND tDTL.MachineType =  tRC.MachineType 
			And tDTL.MachineSubType =  tRC.MachineSubType And tDTL.ReasonCode =  tRC.ReasonCode
		Left Outer Join dbo.tblPlantStaff tPS
		On tDTL.Facility = tPS.Facility AND tOS.StaffID =  tPS.StaffID
		Where tDTL.Facility = @vchFacility 
			AND InActive = 0 
-- WO#3771	AND  tDTL.ReasonType <> 17
			AND ((@vchFacility = '01' AND tDTL.ReasonType <> 17) OR (@vchFacility <>'01'))		-- WO#3771
			AND Convert(varchar(8),tDTL.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1)) 
			AND (@intShiftNoFilter is NULL OR tDTL.Shift = @intShiftNoFilter) 
			AND (@vchSelectedValue is NULL or tOS.StaffID = @vchSelectedValue)
		Group by tDTL.Facility, tOS.StaffID, tPS.FirstName + ' ' + tPS.LastName, tDTL.ReasonCode, tRC.Description
		Order by Sum(DateDiff(Minute,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) desc 
		)
		
		SELECT cteDTL.* , cteSCH.PaidRunTime
		FROM cteDTL
		LEFT OUTER JOIN cteSCH
		ON 	cteDTL.Facility = cteSCH.Facility AND cteDTL.ID = cteSCH.UtilityTech	
		ORDER BY DownTime Desc
	END
	ELSE
	IF @vchAction = 'OverAll'
	BEGIN
		;With cteSCH AS
		(
			SELECT Facility, SUM(PaidRunTime * 60) as PaidRunTime
			FROM dbo.tfnSessionControlHstSummary('WithoutAdj', @vchFacility, NULL, NULL, NULL, NULL, @dteFromProdDate, @intFromShift,	@dteToProdDate,	@intToShift)
			WHERE (@intShiftNoFilter is NULL OR ShiftNo = @intShiftNoFilter)
			GROUP BY Facility
		)
		,cteDTL AS (
		-- IC#9485 Select Top 10 tDTL.Facility, tDTL.ReasonCode
		Select Top (@intTopCount) tDTL.Facility, tDTL.ReasonCode	-- IC#9485
			,tRC.Description  as RCDescription, count(*) as Occurances
			,Sum(DateDiff(Minute,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) as DownTime 
		From tbldowntimelog tDTL
		Left Outer Join vwLineWorkShiftType tCC	
		ON tDTL.Facility = tCC.Facility AND tDTL.MachineId =  tCC.PackagingLine
		Left Outer Join tblShift tS
		ON tDTL.Facility = tS.Facility AND tDTL.Shift = tS.Shift AND ISNULL(tCC.WorkShiftType,'P') = tS.WorkGroup
		left outer join tblDTReasonCode tRC
		On tDTL.Facility = tRC.Facility AND tDTL.MachineType =  tRC.MachineType 
			And tDTL.MachineSubType =  tRC.MachineSubType And tDTL.ReasonCode =  tRC.ReasonCode
		Where tDTL.Facility = @vchFacility 
			AND InActive = 0 
-- WO#3771	AND  tDTL.ReasonType <> 17
			AND ((@vchFacility = '01' AND tDTL.ReasonType <> 17) OR (@vchFacility <>'01'))		-- WO#3771
			AND Convert(varchar(8),tDTL.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1)) 
			AND (@intShiftNoFilter is NULL OR tDTL.Shift = @intShiftNoFilter) 
		Group by tDTL.Facility, tDTL.ReasonCode, tRC.Description
		Order by Sum(DateDiff(Minute,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) desc 
		)
		
		SELECT cteDTL.* , cteSCH.PaidRunTime
		FROM cteDTL
		LEFT OUTER JOIN cteSCH
		ON 	cteDTL.Facility = cteSCH.Facility	
		ORDER BY DownTime Desc
	END

-- WO#359 Add Stop 
-- WO#3771 ADD Start
	ELSE
	IF @vchAction = 'ByWorkCenter'
	BEGIN
		;With cteSCH AS
		(
			SELECT tSCH.Facility, tEQ.WorkCenter, SUM(PaidRunTime * 60) as PaidRunTime
			FROM dbo.tfnSessionControlHstSummary('WithoutAdj', @vchFacility, @vchSelectedValue, NULL, NULL, NULL, @dteFromProdDate, @intFromShift,	@dteToProdDate,	@intToShift) tSCH
				left outer join [dbo].[tblEquipment] as tEQ
				on tSCH.Facility = tEQ.Facility and tSCH.DefaultPkgLine = tEQ.EquipmentID
			WHERE (@intShiftNoFilter is NULL OR ShiftNo = @intShiftNoFilter)
				AND (@vchSelectedValue is NULL or tEQ.WorkCenter = @vchSelectedValue)
			GROUP BY tSCH.Facility, tEQ.WorkCenter
		)
		,cteDTL AS (
		-- IC#9485 Select Top 10 tDTL.Facility, tE.WorkCenter as ID, tWC.Description as Description, tDTL.ReasonCode, tRC.Description as RCDescription, count(*) as Occurances, Sum(DateDiff(Minute,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) as DownTime 
		Select Top (@intTopCount) tDTL.Facility, tE.WorkCenter as ID, tWC.Description as Description, tDTL.ReasonCode, tRC.Description as RCDescription, count(*) as Occurances, Sum(DateDiff(Minute,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) as DownTime	-- IC#9485
		From tblDownTimeLog tDTL

		Left Outer Join dbo.vwLineWorkShiftType tCC
		ON tDTL.Facility = tCC.Facility AND tDTL.MachineId =  tCC.PackagingLine
		Left Outer Join tblShift tS
		ON tDTL.Facility = tS.Facility AND tDTL.Shift = tS.Shift AND ISNULL(tCC.WorkShiftType,'P')  = tS.WorkGroup
		left outer join tblDTReasonCode tRC
		On tDTL.Facility = tRC.Facility AND tDTL.MachineType =  tRC.MachineType 
			And tDTL.MachineSubType =  tRC.MachineSubType And tDTL.ReasonCode =  tRC.ReasonCode
		Left Outer Join tblEquipment tE
		On tDTL.Facility = tE.Facility AND tDTL.MachineID =  tE.EquipmentID 
		Left Outer Join tblWorkCenter tWC
		On tDTL.Facility = tWC.Facility AND tE.WorkCenter = tWC.WorkCenter
		Where tDTL.Facility = @vchFacility 
			AND InActive = 0 
			AND (@vchSelectedValue is Null OR tE.WorkCenter =  @vchSelectedValue)
			AND ((@vchFacility = '01' AND tDTL.ReasonType <> 17) OR (@vchFacility <>'01'))
			AND Convert(varchar(8),tDTL.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1)) -- WO#194
			AND (@intShiftNoFilter is NULL OR tDTL.Shift = @intShiftNoFilter) 
		Group by tDTL.Facility, tE.WorkCenter, tWC.Description, tDTL.ReasonCode, tRC.Description
		Order by Sum(DateDiff(Minute,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) desc 
		)

		SELECT cteDTL.* , cteSCH.PaidRunTime
		FROM cteDTL
		LEFT OUTER JOIN cteSCH
		ON cteDTL.Facility = cteSCH.Facility
			AND cteDTL.Id = cteSCH.WorkCenter
		ORDER BY DownTime Desc
	END
-- WO#3771 ADD Stop
	

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

--grant execute on object:: PPsp_DownTimeLogTop10 to ppuser

GO

