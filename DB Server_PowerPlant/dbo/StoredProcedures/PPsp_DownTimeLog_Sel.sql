

-- =============================================
-- Author:		Bong Lee
-- Create date: July 10, 2009
-- Description:	Select Down Time
-- POAP 79: Down Time Entry Modification
--			Jan 11,2009	Bong Lee
-- POAP 74: Down Time Maintenance
--			Jan 11,2009	Bong Lee
-- POAP 103: Shift Report Modification
-- Description: allow to select by operator
--			Mar 29,2010	Bong Lee
-- WO#194:		Sep. 9, 2010	Bong Lee
-- Description:	If the time frame for the expected shift crosses mid-night
--				and the given time is prior to mid-night, the Shift Production 
--				Date will be the given time plus one day. Before this change the 	
--				Shift Production Date for this case was same as the given time but 
--				if the given time after mid-night, the result was given time 
--				minus one day.
-- WO#359:		Jul. 19, 2011	Bong Lee
-- Description: Add an input parameter for Shift Filter
--				If @vchAction = 'ByPrdDate_Shift_Line_Operator', the value of shift no. 
--				should be the shift sequence
-- WO#3771:		Sep. 12, 2016	Bong Lee
-- Description: Add an input parameter for Work Center
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_DownTimeLog_Sel] 
	@vchAction varchar(50) , 
	@chrFacility char(3) = Null,
	@vchPackagingLine varchar(10) = Null,
	@dteStartDownTime datetime = null,
	@vchOperator varchar(10) = NULL,
	@dteCurDateTime datetime = null,
	@vchActivityID varchar(50) = Null,
	@dteFromProdDate as DateTime = Null,	-- From Shift Production Date
	@intFromShift as tinyint = 0,			-- From Shift Sequence
	@dteToProdDate as DateTime = Null,		-- To Shift Production Date
	@intToShift as tinyint = 0,				-- From Shift Sequence
	@intReasonCode as smallint = NULL,
	@vchUtilityTech varchar(10) = NULL,
	@vchMachineType as varchar(3) = NULL,	-- POAP 79-Add: 
	@intRRN as int = NULL					-- POAP 74-Add:
	,@intShiftNoFilter as int = 0			-- Shift number, not sequence	-- WO$359
	,@vchWorkCenter as varchar(10) = NULL	-- WO#3771
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @dteProductionDate datetime,
			@intShiftNo int,
			@intShopOrder int
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
--		WHERE t2.Packagingline is null OR RecordStatus = 1
--		Group By T1.Facility, T1.Packagingline, T1.WorkShiftType
--		;
-- WO#194 Add Stop
-- WO#359 Del Stop

	--By Production Date/Shift/Line/Operator and Optional ActivityID
    If @vchAction = 'ByPrdDate_Shift_Line_Operator'
	Begin 

		If @intShiftNo is null and @dteFromProdDate is null
-- WO#359	Select @intShiftNo = Shift, @dteProductionDate = ProductionDate 
			Select @intShiftNo = ShiftSequence, @dteProductionDate = ProductionDate 
				From dbo.fnShiftInfo (@chrFacility, ISNULL(@dteCurDateTime,getdate()), 0,NULL, @vchPackagingLine)
		Else
			Select @dteProductionDate  = @dteFromProdDate, @intShiftNo = @intFromShift
		
--		Select ROW_NUMBER() OVER(PARTITION BY MachineID,DownTimeBegin,DownTimeEnd ORDER BY MachineID,DownTimeBegin,DownTimeEnd,tDTL.ReasonCode) AS SeqNo,
		Select Rank() OVER(PARTITION BY MachineID, Operator, DownTimeBegin ORDER BY MachineID, Operator, DownTimeBegin, DateDiff(minute,DownTimeBegin,DownTimeEnd) desc, creationdate) AS SeqNo,
				tDTL.*, IsNull(tPS.OperatorName,'** Unknown **') as OperatorName , DateDiff(minute,DownTimeBegin,DownTimeEnd) as Duration,
			 tDTC.Description as Reason, '' as UtilityTech, '' as UtilityTechName
			From tblDownTimeLog tDTL
-- WO#194 Add Start 
-- WO#359	Left Outer Join @tblComputerConfig tCC
			Left Outer Join dbo.vwLineWorkShiftType tCC		-- WO#359
			ON tDTL.Facility = tCC.Facility AND tDTL.MachineID =  tCC.PackagingLine
			Left Outer Join tblshift tS
-- WO#359	ON tDTL.Facility = tS.Facility AND tDTL.Shift = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
			ON tDTL.Facility = tS.Facility AND tDTL.Shift = tS.Shift AND ISNULL(tCC.WorkShiftType,'P')  = tS.WorkGroup	-- WO#359
-- WO#194 Add Stop
			Left outer join dbo.tblDTReasonCode tDTC
			On	tDTL.Facility = tDTC.Facility And
				tDTL.MachineType  = tDTC.MachineType And
				tDTL.MachineSubType = tDTC.MachineSubType And
				tDTL.ReasonCode = tDTC.ReasonCode
			Left Outer Join (Select Distinct StaffID, FirstName + ' ' + LastName as OperatorName 
				From tblPlantStaff
				Where Facility = @chrFacility ) tPS 
			ON tDTL.Operator = tPS.StaffID
			Where tDTL.Facility = @chrFacility and
				tDTL.ShiftProductionDate = @dteProductionDate And
-- WO#194		tDTL.Shift = @intShiftNo And
				tS.ShiftSequence = @intShiftNo And	-- WO#194
				(@vchMachineType is NULL OR tDTL.MachineType = @vchMachineType) And		-- POAP 79-Add: 
				(@vchPackagingLine is NULL OR tDTL.MachineID = @vchPackagingLine) And
				(@vchOperator is NULL OR tDTL.Operator = @vchOperator) And
				(@vchActivityID is null OR @vchActivityID ='' OR tDTL.ShopOrder = Cast(@vchActivityID as int)) And
				(@intShiftNoFilter = 0 OR tDTL.Shift = @intShiftNoFilter) AND 	--WO#359
				tDTL.Inactive = 0
		UNION
			-- this is for the active session selected to report the down time but has not been complete.
			SELECT 1 as SeqNo
			  ,0 as RRN
			  ,0 as InActive
			  ,@chrFacility
			  ,@vchActivityID as ShopOrder
			  ,'' as MachineType
			  ,'' as MachineSubType
			  ,@vchPackagingLine as MachineID
			  ,@vchOperator as Operator
			  ,'' as Technician
			  ,@dteStartDownTime as DownTimeBegin
			  ,ISNULL(@dteCurDateTime,getdate()) as DownTimeEnd
			  ,0 as Shift
			  ,0 as ReasonType
			  ,0 as ReasonCode
			  ,'' as Comment
			  ,'' as CreatedBy
			  ,getdate() as CreationDate
			  ,@vchOperator as UpdatedBy
			  ,getdate() as LastUpdated
			  ,getdate() as ShiftProductionDate
			  ,'' as eventID
			  ,(Select Distinct IsNull(FirstName + ' ' + LastName, '** Unknown **') From tblPlantStaff
				Where Facility = @chrFacility and StaffID = @vchOperator) as OperatorName 
			  ,DateDiff(minute,@dteStartDownTime,ISNULL(@dteCurDateTime,getdate())) as Duration
			  ,'** TBA **' as Reason
			  , '' as UtilityTech, '' as UtilityTechName	
			Where @dteStartDownTime is not null
-- WO#359	Order by DownTimeBegin
			Order by DownTimeBegin, CreationDate	-- WO#359
			
	End
	ELSE
		--By Production Date and Shift Range/Line/Operator and Optional ActivityID
    If @vchAction = 'ByPrdDateShiftRange_Line_Opt_ActivityID'
		Begin 
--			Select ROW_NUMBER() OVER(PARTITION BY MachineID,DownTimeBegin,DownTimeEnd ORDER BY MachineID,DownTimeBegin,DownTimeEnd,tDTL.ReasonCode) AS SeqNo,
-- WO#194	Select Rank() OVER(PARTITION BY MachineID, Operator,DownTimeBegin ORDER BY MachineID,Operator, DownTimeBegin, DateDiff(minute,DownTimeBegin,DownTimeEnd) desc, creationdate) AS SeqNo,
			Select Rank() OVER(PARTITION BY tDTL.MachineID, tDTL.Operator, tDTL.DownTimeBegin ORDER BY tDTL.MachineID, tDTL.Operator, 			-- WO#194
				 tDTL.DownTimeBegin, DateDiff(minute, tDTL.DownTimeBegin, tDTL.DownTimeEnd) desc, creationdate) AS SeqNo,			-- WO#194
				tDTL.*, IsNull(tPS.OperatorName,'** Unknown **') as OperatorName, DateDiff(minute,DownTimeBegin,DownTimeEnd) as Duration,
			 tDTC.Description as Reason, '' as UtilityTech, '' as UtilityTechName
			From tblDownTimeLog tDTL
-- WO#194 Add Start 
-- WO#359	Left Outer Join @tblComputerConfig tCC
			Left Outer Join dbo.vwLineWorkShiftType tCC		-- WO#359
			ON tDTL.Facility = tCC.Facility AND tDTL.MachineID =  tCC.PackagingLine
			Left Outer Join tblshift tS
-- WO#359	ON tDTL.Facility = tS.Facility AND tDTL.Shift = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup
			ON tDTL.Facility = tS.Facility AND tDTL.Shift = tS.Shift AND ISNULL(tCC.WorkShiftType,'P')  = tS.WorkGroup	-- WO#359
-- WO#194 Add Stop
			Left outer join dbo.tblDTReasonCode tDTC
			On	tDTL.Facility = tDTC.Facility And
				tDTL.MachineType  = tDTC.MachineType And
				tDTL.MachineSubType = tDTC.MachineSubType And
				tDTL.ReasonCode = tDTC.ReasonCode
			Left Outer Join (Select Distinct StaffID, FirstName + ' ' + LastName as OperatorName 
				From tblPlantStaff
				Where Facility = @chrFacility ) tPS 
			ON tDTL.Operator = tPS.StaffID
			Left Outer Join tblEquipment tE												--WO#3771
			On tDTL.Facility = tE.Facility And tDTL.MachineID = tE.EquipmentID			--WO#3771
			Where tDTL.Facility = @chrFacility and
-- WO#194		(Convert(varchar(8),tDTL.ShiftProductionDate,112) + Cast(tDTL.Shift as Char(1)) 
				(Convert(varchar(8),tDTL.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) -- WO#194
					Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) 
						and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))) And
				(@vchPackagingLine is NULL OR tDTL.MachineID = @vchPackagingLine) And			--WO103 Add	
--WO103 Del		tDTL.MachineID = @vchPackagingLine And
				(@vchOperator is NULL OR tDTL.Operator = @vchOperator) And
				(@vchActivityID is null OR @vchActivityID ='' OR tDTL.ShopOrder = Cast(@vchActivityID as int)) And
				(@intReasonCode is NULL OR @intReasonCode = tDTL.ReasonCode) And
				(@intShiftNoFilter = 0 OR tDTL.Shift = @intShiftNoFilter) AND 	--WO#359
				(@vchWorkCenter is NULL or tE.WorkCenter = @vchWorkCenter) AND			--WO#3771
				tDTL.Inactive = 0
			Order by DownTimeBegin,DownTimeEnd, ReasonCode
		End
		Else
		If @vchAction = 'ByPrdDateShiftRange_Line_UtilityTech_ActivityID'
		Begin 
			With Opr_UTech AS 
			(Select tSCH1.Facility, tSCH1.PackagingLine, tSCH1.ShopOrder, tSCH1.Operator, tOS.StaffID as UtilityTech From
/* WO#194 Del Stop 
			  (Select Facility, DefaultPkgLine as PackagingLine, StartTime, Operator, ShopOrder
					From tblSessionControlHst 
					Where Facility = @chrFacility 
					AND Convert(varchar(8),ShiftProductionDate,112) + Cast(OverrideShiftNo as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
					-- exclude Set-up Operators in Mississauga 
					AND ((Facility not in (Select facility from dbo.tblFacility Where Region = '01') OR Len(Rtrim(Operator)) > 3) ) 
					Group By Facility, DefaultPkgLine, StartTime, Operator, ShopOrder) tSCH1	
WO#194 Del Stop */
-- WO#194 Add Start 
				(SELECT tSCHst.Facility, tSCHst.DefaultPkgLine as PackagingLine, tSCHst.StartTime, tSCHst.Operator, tSCHst.ShopOrder
					FROM dbo.tfnSessionControlHstDetail(NULL,@chrFacility,NULL,NULL,NULL,NULL, @dteFromProdDate, @intFromShift, @dteToProdDate, @intToShift) tSCHst		-- WO#359
-- WO#359 Del Start
--				FROM tblSessionControlHst tSCHst
--				ON tSCHst.Facility = tCC.Facility AND tSCHst.OverridePkgLine =  tCC.PackagingLine
--				Left Outer Join tblshift tS
--				ON tSCHst.Facility = tS.Facility AND tSCHst.OverrideShiftNo = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
--				WHERE tSCHst.Facility = @chrFacility 
--				AND Convert(varchar(8),tSCHst.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
--				-- exclude Set-up Operators in Mississauga 
--				AND ((tSCHst.Facility not in (Select Facility from dbo.tblFacility where Region = '01') OR Len(Rtrim(Operator)) > 3) ) 
-- WO#359 Del Stop
					GROUP BY tSCHst.Facility, tSCHst.DefaultPkgLine, tSCHst.StartTime, tSCHst.Operator, tSCHst.ShopOrder) tSCH1
-- WO#194 Add Stop
			  Left Outer Join dbo.tblOperationStaffing tOS
			  On tSCH1.Facility = tOS.Facility and tSCH1.PackagingLine = tOS.PackagingLine And tSCH1.[StartTime] = tOS.[StartTime]
			  Where tOS.Rrn is not null	
			  Group By tSCH1.Facility, tSCH1.PackagingLine, tSCH1.ShopOrder, tSCH1.Operator, tOS.StaffID	
			)

--			Select ROW_NUMBER() OVER(PARTITION BY MachineID,DownTimeBegin,DownTimeEnd ORDER BY MachineID,DownTimeBegin,DownTimeEnd,tDTL.ReasonCode) AS SeqNo,
			Select Rank() OVER(PARTITION BY MachineID, tDTL.Operator, DownTimeBegin ORDER BY MachineID, tDTL.Operator, DateDiff(minute,DownTimeBegin,DownTimeEnd) desc, creationdate) AS SeqNo,
				tDTL.*, '' as OperatorName, DateDiff(minute,DownTimeBegin,DownTimeEnd) as Duration,
			 tDTC.Description as Reason, tOpr_UTec.UtilityTech, IsNull(tPS.UtilityTechName,'** Unknown **') as UtilityTechName
			From tblDownTimeLog tDTL
-- WO#194 Add Start 
-- WO#359	Left Outer Join @tblComputerConfig tCC
			Left Outer Join dbo.vwLineWorkShiftType tCC		-- WO#359
			ON tDTL.Facility = tCC.Facility AND tDTL.MachineID =  tCC.PackagingLine
			Left Outer Join tblshift tS
-- WO#359	ON tDTL.Facility = tS.Facility AND tDTL.Shift = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup
			ON tDTL.Facility = tS.Facility AND tDTL.Shift = tS.Shift AND ISNULL(tCC.WorkShiftType,'P')  = tS.WorkGroup	-- WO#359
-- WO#194 Add Stop
			Left outer join dbo.tblDTReasonCode tDTC
			On	tDTL.Facility = tDTC.Facility And
				tDTL.MachineType  = tDTC.MachineType And
				tDTL.MachineSubType = tDTC.MachineSubType And
				tDTL.ReasonCode = tDTC.ReasonCode
			Left Outer Join Opr_UTech as tOpr_UTec
			On tDTL.Facility = tOpr_UTec.Facility And tDTL.MachineID = tOpr_UTec.PackagingLine AND tDTL.ShopOrder = tOpr_UTec.ShopOrder AND tDTL.Operator = tOpr_UTec.Operator
			Left Outer Join (Select Distinct StaffID, FirstName + ' ' + LastName as UtilityTechName 
				From tblPlantStaff
				Where Facility = @chrFacility ) tPS 
			ON tOpr_UTec.UtilityTech = tPS.StaffID
			Left Outer Join tblEquipment tE												--WO#3771
			On tDTL.Facility = tE.Facility And tDTL.MachineID = tE.EquipmentID			--WO#3771
			Where tDTL.Facility = @chrFacility and
-- WO#194		(Convert(varchar(8),tDTL.ShiftProductionDate,112) + Cast(tDTL.Shift as Char(1)) 
				(Convert(varchar(8),tDTL.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) -- WO#194
					Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) 
						and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))) And
				(@vchPackagingLine is NULL OR tDTL.MachineID = @vchPackagingLine) And			--WO103 Add	
--WO103 Del		tDTL.MachineID = @vchPackagingLine And 
				tOpr_UTec.UtilityTech is not Null And
				(@vchUtilityTech is NULL OR tOpr_UTec.UtilityTech = @vchUtilityTech) And
				(@vchActivityID is null OR @vchActivityID ='' OR tDTL.ShopOrder = Cast(@vchActivityID as int)) And
				(@intReasonCode is NULL OR @intReasonCode = tDTL.ReasonCode) And
				(@intShiftNoFilter = 0 OR tDTL.Shift = @intShiftNoFilter) AND 	--WO#359
				(@vchWorkCenter is NULL or tE.WorkCenter = @vchWorkCenter) AND			--WO#3771
				tDTL.Inactive = 0
			Order by DownTimeBegin,DownTimeEnd, ReasonCode
		End
-- POAP 79-Add: Begin
		Else
			If @vchAction = 'ByRRN'
			Begin
				Select 0 as SqlNo, tDTL.*, IsNull(tPS.OperatorName,'** Unknown **') as OperatorName , DateDiff(minute,DownTimeBegin,DownTimeEnd) as Duration,
					tDTC.Description as Reason, '' as UtilityTech, '' as UtilityTechName
				From tblDownTimeLog tDTL
				Left outer join dbo.tblDTReasonCode tDTC
				On	tDTL.Facility = tDTC.Facility And
					tDTL.MachineType  = tDTC.MachineType And
					tDTL.MachineSubType = tDTC.MachineSubType And
					tDTL.ReasonCode = tDTC.ReasonCode
				Left Outer Join (Select Distinct StaffID, FirstName + ' ' + LastName as OperatorName 
					From tblPlantStaff
					Where Facility = @chrFacility ) tPS 
				ON tDTL.Operator = tPS.StaffID
				Where tDTL.RRN = @intRRN
			End
-- POAP 79-Add: End
	
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

