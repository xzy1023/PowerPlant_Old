



-- =============================================
-- Author:		Bong Lee
-- Create date: Aug. 01 ,2008
-- Description:	Line Efficiency
-- Mod.			Date			Author
-- WO#103		Apr.20 2010		Bong Lee
-- Description: Fix the Actual Duration. It should be filtered by shop order and operator.
-- Mod.			Date			Author
-- WO#359		Jul.05 2011		Bong Lee
-- Description: Add a new column, Shift Progress(hr).
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_LineEfficiency] 
	@chrFacility char(3), 
	@chrPkgLine char(10),
	@vchStaffID varchar(10),
	@intShift int,
	@intShopOrder int = NULL,
	@dteDateTime as DateTime,
	
	--@sc_ComputerName varchar(50) = NULL,
	@sc_StartTime datetime = NULL,
	--@sc_StopTime datetime = NULL,
	@sc_DefaultPkgLine char(10) = NULL,
	@sc_OverridePkgLine char(10) = NULL,
	@sc_ShopOrder int = NULL,
	@sc_ItemNumber varchar(35) = NULL,
	@sc_Operator varchar(10) = NULL,
	@sc_DefaultShiftNo char(1) = NULL,
	@sc_OverrideShiftNo char(1) = NULL,
	@sc_CasesScheduled int = NULL,
	@sc_CasesProduced int = NULL,
	@sc_PalletsCreated int = NULL,
	--@sc_ReworkWgt decimal(8,2) = NULL,
	@sc_LooseCases int = NULL,
	@sc_ProductionDate datetime = NULL,
	--@sc_LogOnTime datetime = NULL,
	@sc_CarriedForwardCases int = NULL,
	@sc_ShiftProductionDate datetime = NULL,

	-- Output for the efficiency of the shop order
	@so_Act_Produced int OUTPUT,			-- Actual cases produced
	@so_Act_CsPerHour int OUTPUT,			-- Actual cases/Hour
	@so_Sch_CsPerHour smallint OUTPUT,		-- Scheduled cases/Hour
	@so_Efficiency decimal(10,2) OUTPUT,	-- Actual Line Efficiency
	@so_Alert bit OUTPUT,					-- Line Efficiency Alert
	@so_Progress int OUTPUT,				-- Actual SO Progress
	@so_Act_Duration decimal(10,2) = NULL OUTPUT,	-- Actual SO Duration	WO#359

	-- Output for the efficiency of the shift
	@sft_Act_Produced int OUTPUT,			-- Actual cases produced
	@sft_Act_CsPerHour int OUTPUT,			-- Actual cases/Hour
	@sft_Sch_Produced int OUTPUT,			-- Scheduled cases produced
	@sft_Sch_CsPerHour int OUTPUT,			-- Scheduled cases/Hour
	@sft_Efficiency decimal(10,2) OUTPUT,	-- Actual Line Efficiency
	@sft_Alert bit OUTPUT,					-- Line Efficiency Alert
	@sft_Progress int OUTPUT				-- Actual Shift Progress
	,@sft_Progress_Hr decimal(10,2) = NULL OUTPUT	-- Shift Progress in Hours	WO#359
	,@sft_Act_Duration decimal(10,2) = NULL OUTPUT	-- Actual Shift Duration	WO#359
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets FROM interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @dteToDay as DateTime,
			@sc_StopTime datetime,
			@vchWorkShiftType as varchar(10),
			@dteShiftStart as DateTime,
			@dteShiftEnd as DateTime,
			@intTotalShiftTime as int,	--in seconds
			@intCurLooseCases as int,
			@intPrvLooseCases as int,
			@vchShiftProductionDate varchar(8),
			@vchWorkGroup varchar(10);

	BEGIN TRY

	-------------------------------------------------------------------------------------
	-- Calculate the start and end time of given shift, and the duration of the shift. --
	-------------------------------------------------------------------------------------
	SELECT @dteShiftStart = ShiftStartTime, @dteShiftEnd = ShiftEndTime , @intTotalShiftTime = ShiftTotalTime
		From dbo.fnShiftInfo(@chrFacility,@sc_StartTime,@intShift,NULL,@chrPkgLine )

	SELECT @vchShiftProductionDate = convert(varchar(8), [dbo].[fnGetProdDateByShift] (@chrFacility,  @intShift, @sc_StartTime, PackagingLine, WorkShiftType),112) from tblComputerConfig 
		Where Facility = @chrFacility AND PackagingLine = @chrPkgLine 

print '1 - ' + cast(@dteShiftStart as varchar(21))
print '2 - ' + cast(@dteShiftEnd as varchar(21))
print '3 - ' + @chrPkgLine;
print '4 - ' + cast(@intTotalShiftTime as varchar(10))

	-------------------------------------------------------------------------------------
 	-- Create session control work file to hold the last 5 days and prior 5 days from  --
	-- the provided date															   --	
 	-------------------------------------------------------------------------------------	
	if object_id('tempdb..#temp_SessionControlHst') is not null
		DROP TABLE #temp_SessionControlHst

	CREATE TABLE #temp_SessionControlHst(
	[Facility] [char](3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	--[ComputerName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[StartTime] [datetime] NULL,
	[StopTime] [datetime] NULL,
	[DefaultPkgLine] [char](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[OverridePkgLine] [char](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ShopOrder] [int] NULL,
	[ItemNumber] [varchar](35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Operator] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	--[LogOnTime] [datetime] NULL,
	[DefaultShiftNo] [tinyint] NULL,
	[OverrideShiftNo] [tinyint] NULL,
	[CasesScheduled] [int] NULL,
	[CasesProduced] [int] NULL,
	[PalletsCreated] [int] NULL,
	--[ReworkWgt] [decimal](8, 2) NULL,
	[LooseCases] [int] NULL,
	[ProductionDate] [datetime] NULL,
	CarriedForwardCases [int] NULL,
	[ShiftProductionDate] [datetime] NULL
	) ON [PRIMARY]

	-- Merge Session Control and Session Control History to a temporary table	
	INSERT INTO #temp_SessionControlHst
	SELECT Facility, StartTime, StopTime, DefaultPkgLine, OverridePkgLine, ShopOrder, ItemNumber, Operator, DefaultShiftNo,
		   OverrideShiftNo, CasesScheduled, CasesProduced, PalletsCreated, LooseCases, ProductionDate, CarriedForwardCases, ShiftProductionDate
	FROM   tblSessionControlHst
	WHERE	Facility = @chrFacility 
			AND DefaultPkgLine = @chrPkgLine 
			AND ShiftProductionDate = @vchShiftProductionDate
			AND OverrideShiftNo = @intShift

	IF @sc_ShopOrder <> 0 
	BEGIN
		-- if there is at least 1 pallet was created in the current session, update the stop time of the session contol temporary file. It simulates a stop shop order record.
		-- So the loose cases for the shop order will be based on the value in the current session control record. It should be zero because the loose cases should be consumed by the pallet.
		IF @sc_PalletsCreated > 0 
			SET @sc_StopTime = @dteDateTime
		ELSE
			SET @sc_StopTime = NULL

		INSERT INTO #temp_SessionControlHst
		SELECT @chrFacility, @sc_StartTime, @sc_StopTime, @sc_DefaultPkgLine, @sc_OverridePkgLine, @sc_ShopOrder, @sc_ItemNumber, @sc_Operator, @sc_DefaultShiftNo,
			  @sc_OverrideShiftNo, @sc_CasesScheduled, @sc_CasesProduced, @sc_PalletsCreated, @sc_LooseCases, @sc_ProductionDate, @sc_CarriedForwardCases, @sc_ShiftProductionDate
	END
print '#temp_SessionControlHst'
-- select 'SCH' as tableName,* from #temp_SessionControlHst order by starttime	-- Debug point

------------------------------------------------------------------------------------------
	--	selected pallets
------------------------------------------------------------------------------------------
	IF object_id('tempdb..#temp_PalletHst') is not null
		DROP TABLE #temp_PalletHst

	CREATE TABLE #temp_PalletHst (
	[Facility] [char](3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PkgLine] [char](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ShopOrder] [int] NOT NULL,
	[ItemNumber] [varchar](35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SOStartTime] [datetime] NOT NULL,
	[Operator] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Quantity] [int] NOT NULL,
	[OrderComplete] [char](1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PalletCrtTime] [datetime] NULL)
-- ,
--	[StartTime] [datetime] NOT NULL,
--	[StopTime] [datetime] NOT NULL,
--	[ActualDuration] [decimal](7,2)  )

	INSERT INTO #temp_PalletHst 
		SELECT Facility, PkgLine, ShopOrder,ItemNumber, StartTime as SOStartTime, Operator, Quantity, OrderComplete, PalletCrtTime
--,
--				Min(StartTime) as StartTime, Max(StopTime) as StopTime,
--				Case
--					When Min(StartTime) is NULL Then 0
--					ELSE Sum(DateDiff(second,StartTime,ISNULL(StopTime,@dteDateTime)))
--				End /3600.00 as ActualDuration
			From (
				-- select created pallets detail records  
			Select tPH.Facility, tPH.DefaultPkgLine as PkgLine, tPH.ShopOrder, tPH.ItemNumber,tPH.Operator, tPH.Quantity, 
				 tPH.OrderComplete, tPH.CreationDateTime as PalletCrtTime, tSCH.CasesScheduled, tSCH.StartTime
--, tSCH.StopTime
				FROM (
					SELECT * FROM dbo.tblPallet 
					WHERE Facility = @chrFacility 
						AND defaultPkgLine = @chrPkgLine 
						AND ShiftProductionDate = @vchShiftProductionDate
						AND ShiftNo = @intShift
						AND Operator = @vchStaffID
					UNION ALL
					SELECT * FROM dbo.tblPallethst 
					WHERE Facility = @chrFacility 
						AND defaultPkgLine = @chrPkgLine 
						AND ShiftProductionDate = @vchShiftProductionDate
						AND ShiftNo = @intShift
						AND Operator = @vchStaffID
					) tPH 
				INNER JOIN #temp_SessionControlHst tSCH
				On 	tPH.ShopOrder = tSCH .ShopOrder 
						AND tPH.StartTime = tSCH.StartTime) T1
		Group By  Facility, PkgLine, ShopOrder,ItemNumber, StartTime, Operator, Quantity, OrderComplete, PalletCrtTime

	-- If there is no pallet created, check any loose cases created during the stop shop order in the shift
	
	If (Select count(*) from #temp_PalletHst) = 0 
	Begin
		Insert into #temp_PalletHst
		Select Facility, OverridePkgLine,shoporder, ItemNumber, Max(StartTime),Operator,0,'N',NULL
--, Min(StartTime), ISNULL(Max(StopTime),@dteDateTime),
--			sum(DateDiff(second,StartTime,ISNULL(StopTime,@dteDateTime)))/3600.00 
		from #temp_SessionControlHst 
		Where OverrideShiftNo = @intShift
			And ShiftProductionDate = @vchShiftProductionDate
			And Operator =  @vchStaffID
		Group by Facility, OverridePkgLine, shoporder, ItemNumber, Operator
	End


	print '#temp_PalletHst'
--	Select 'PH' as tableName,* from #temp_PalletHst	-- Debug point

-------------------------------------------------------------------------------------------
	-- Calculate # of loose cases from last stop of the shop order in different shift or operator
	-- and		 # of loose cases from current stop of the shop order
-------------------------------------------------------------------------------------------
	DECLARE @vchOperator as varchar(10),
			@bitFirst as bit,			-- indicates first time
			@intLooseCases as int,
			@dteStartTime as datetime, 
			@dteStopTime as datetime, 
			@intOverrideShiftNo as int,
			@intSavLooseCase as int,
		    @intSavShopOrder as int,
			@vchSavOperator as varchar(10),
			@intPalletCreated as int,
			@intCarriedForwardCases as int,
			@intRRN as int,
			@dteSavStartTime as datetime,
			@dteSavStopTime as datetime;

	-- Initialization
	if object_id('tempdb..#temp_LooseCases') is not null
		DROP TABLE #temp_LooseCases

	CREATE TABLE #temp_LooseCases(
		RRN int NULL,
		ShopOrder int NULL,
		Operator varchar(10) NULL,
		CurLooseCases int NULL,
		PrvLooseCases int NULL,
		StartTime DateTime NULL,
		StopTime DateTime NULL
	) ON [PRIMARY]

	Select @bitFirst = 1, @intRRN = 1	

	DECLARE object_cursor CURSOR FOR	
		SELECT tSCH.ShopOrder, tSCH.StartTime ,tSCH.StopTime, tSCH.Operator, tSCH.LooseCases, tSCH.CarriedForwardcases, ISNULL(PalletCreated,0) as PalletCreated
		FROM #temp_SessionControlHst tSCH
		Left Outer Join (Select ShopOrder, SOStartTime, count(*) as PalletCreated From #temp_PalletHst Group by ShopOrder, SOStartTime) tPH 
			On tSCH.ShopOrder = tPH.ShopOrder
			AND tSCH.StartTime = tPH.SOStartTime
		WHERE tSCH.DefaultPkgLine = @chrPkgLine
			AND ShiftProductionDate = @vchShiftProductionDate
			AND OverrideShiftNo = @intShift
			AND tSCH.Facility = @chrFacility
		ORDER by tSCH.StartTime 

	OPEN object_cursor
--
--	-- Read the frist record from the cursor and put their values to the variables
	FETCH NEXT FROM object_cursor INTO 	
		@intShopOrder, @dteStartTime ,@dteStopTime, @vchOperator, @intLooseCases, @intCarriedForwardCases, @intPalletCreated
	WHILE @@FETCH_STATUS = 0
	BEGIN

--Print 'In cursor read loop'

		IF @bitFirst = 1
		BEGIN
			Select @bitFirst = 0, @intSavShopOrder = @intShopOrder, @vchSavOperator = @vchOperator, @dteSavStartTime = @dteStartTime,
				@dteSavStopTime = @dteStopTime

			IF @dteStopTime is Not NULL Or @intPalletCreated > 0
				Select @intPrvLooseCases = @intCarriedForwardCases
		END

		IF  @intSavShopOrder <> @intShopOrder OR @vchSavOperator <> @vchOperator
		BEGIN
--			IF @vchSavOperator = @vchStaffID
--			BEGIN
				Insert into #temp_LooseCases
				Values(@intRRN , @intSavShopOrder, @vchSavOperator, @intCurLooseCases, @intPrvLooseCases, @dteSavStartTime, @dteSavStopTime)
				Select @intRRN = @intRRN + 1
--			END
			IF (Select Top 1 Operator from  #temp_LooseCases Where ShopOrder = @intShopOrder order by rrn desc) = @vchOperator
				Set @intPrvLooseCases = 0
			ELSE
				Set @intPrvLooseCases = @intCarriedForwardCases

			Select @intSavShopOrder = @intShopOrder, @vchSavOperator = @vchOperator
		END 

--print 'loose C= ' + cast(@intLooseCases as varchar(5))
--print 'Pallet C= ' + cast(@intPalletCreated as varchar(5))

--		IF @intPalletCreated > 0 
--			SELECT @intCurLooseCases = 0
--		ELSE
			SELECT @intCurLooseCases = @intLooseCases, @dteSavStartTime = @dteStartTime, @dteSavStopTime = @dteStopTime
--
	FETCH NEXT FROM object_cursor INTO 	
		@intShopOrder, @dteStartTime ,@dteStopTime, @vchOperator, @intLooseCases, @intCarriedForwardCases, @intPalletCreated

	END
--	
--	If @vchOperator = @vchStaffID
	Insert into #temp_LooseCases
		Values(@intRRN , @intSavShopOrder, @vchSavOperator, @intCurLooseCases, @intPrvLooseCases, @dteSavStartTime, @dteSavStopTime)
--	
	CLOSE object_cursor
	DEALLOCATE object_cursor


	Print '#temp_LooseCases'
--	Select 'LC' as tableName,* from #temp_LooseCases -- Debug point

	-- Use the latest record if the shop order appears more than once
	IF object_id('tempdb..#temp_CFandLooseCases') is not null
		DROP TABLE #temp_CFandLooseCases

	Select ShopOrder, sum(CurLooseCases) as CurLooseCases ,sum(PrvLooseCases) as PrvLooseCases
		Into #temp_CFandLooseCases from #temp_loosecases
	WHERE Operator = @vchStaffID
	Group by ShopOrder
	Order by ShopOrder

Print '#temp_CFandLooseCases'
--	Select 'CF_LC' as tableName,* from #temp_CFandLooseCases	-- Debug point
-------------------------------------------------------------------------------------------
	-- Create Line Efficiency work File by SO
-------------------------------------------------------------------------------------------
print 'Create Line Efficiency work File by SO'

	IF object_id('tempdb..#tmp_LineEff_SO') is not null
		DROP TABLE #tmp_LineEff_SO
		
	SELECT  tPH.Facility, tPH.PkgLine, tPH.ShopOrder, tPH.ItemNumber, tPH.Operator,
		CASE WHEN Sum(Quantity) + Max(ISNULL(tLC.CurLooseCases,0)) - Max(ISNULL(tLC.PrvLooseCases,0)) < 0 THEN 0 
			ELSE Sum(Quantity) + Max(ISNULL(tLC.CurLooseCases,0)) - Max(ISNULL(tLC.PrvLooseCases,0)) END as ActQty, 
--		CASE When Min(StartTime) > @dteShiftStart Then Min(StartTime) Else @dteShiftStart End as ActualStartTime,
		Max(PalletCrtTime) as LatestPalletCrtTime, 
		(Select Sum(Datediff(Second,StartTime,IsNull(StopTime,@dteDateTime))) From #Temp_SessionControlHst 
			Where shopOrder = tPH.shoporder AND operator = tPH.Operator) / 3600.00 as ActualDuration,	--WO#103
--		Max(tLC.ActualDuration ) as  ActualDuration,
--		Case
--			When Min(StartTime) is NULL Then 0
--			When Sum(Quantity)= 0 AND Max(PalletCrtTime) is not NULL Then datediff(second,Min(StartTime), @dteDateTime )
--			When max(OrderComplete)= 'Y' Then Datediff(second,Min(StartTime), Max(StopTime))
----			-- If the start time is less than shift start time (i.e. last operator forgot log off), use the shift start time
----			Else Datediff(second, CASE When Min(StartTime) > @dteShiftStart Then Min(StartTime) Else @dteShiftStart End,
--			Else Datediff(second, Min(StartTime),
--				CASE
--					-- When Max(StopTime) is NULL Then (Case When @dteDateTime > @dteShiftEnd Then @dteShiftEnd Else @dteDateTime END)
--					When Max(StopTime) is NULL THEN @dteDateTime
--					When Max(PalletCrtTime) > Max(StopTime) Then Max(PalletCrtTime) 
--					ELSE Max(StopTime) 
--				End )
--		End /3600.00 as ActualDuration,
--		CASE When Max(PalletCrtTime) > Max(StopTime)
--			 Then Max(PalletCrtTime) 
--			 ELSE Max(StopTime) 
--		End As ActualStopTime,

		CASE When MAX(tSMER_ID.MachineHours) is NULL and MAX(tSMER_WC.MachineHours) is Null Then 0 else MAX(POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0)))) / MAX(ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) END as StdUnitPerHr,
		--CASE When MAX(tSMER_ID.MachineHours) is NULL and MAX(tSMER_WC.MachineHours) is Null Then 0 ELSE MAX(POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) END as StdUnitPerHr,
		-- Std. Machine Hours Earned: Units Processed * Standard Hours per unit
		CASE When (MAX(tSMER_ID.MachineHours) is NULL and MAX(tSMER_WC.MachineHours) is Null) OR (SUM(quantity) +  MAX(tLC.CurLooseCases) - MAX(tLC.PrvLooseCases) < 0) Then 0 
			Else (sum(quantity) +  MAX(tLC.CurLooseCases) - MAX(tLC.PrvLooseCases)) / (MAX(POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0)))) / Max(ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0)))) End As StdMachineHrEarned,
-- 0 As StdMachineHrEarned,
		MAX(ISNULL(tSMER_WC.StdWorkCenterEfficiency,0)) as StdWCEfficiency
		--ISNULL(Avg(T1.pkgRate),0) as pkgRate,
		--Min(SchStartTime) as SchStartTime, Max(SchEndTime) as SchEndTime ,
		--Datediff(mi,Min(SchStartTime), Max(SchEndTime)) as SchDuration
	INTO #tmp_LineEff_SO
	FROM #temp_PalletHst tPH
	LEFT OUTER JOIN #temp_CFandLooseCases tLC
		ON tPH.ShopOrder = tLC.ShopOrder
	LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_ID 
		ON tPH.Facility = tSMER_ID.Facility AND tPH.ItemNumber = tSMER_ID.ItemNumber AND tPH.PkgLine = tSMER_ID.MachineID
	LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_WC 
		ON tPH.Facility = tSMER_WC.Facility AND tPH.ItemNumber = tSMER_WC.ItemNumber AND CAST(SUBSTRING(tPH.PkgLine,1,4) as int) = tSMER_WC.WorkCenter and tSMER_WC.MachineID = ''
	GROUP BY tPH.Facility,tPH.PkgLine,tPH.ShopOrder,tPH.itemnumber,tPH.Operator
	ORDER BY tPH.Facility,tPH.PkgLine,tPH.ShopOrder,tPH.itemnumber,tPH.Operator

--	select 'LF_SO' as tablename,* from #tmp_LineEff_SO -- Debug point

-------------------------------------------------------------------------------------------
	-- Line Efficiency by Shop order
-------------------------------------------------------------------------------------------

print 'Line Efficiency by Shop order'

	Select @so_Act_Produced = ISNULL(ActQty,0), 
		   -- Actual Cases/Hour: Actual Cases Produced divided by the hours spent in the Shop Order Period
		   @so_Act_CsPerHour = ISNULL(Case When ActualDuration = 0 Then 0 Else ActQty / ActualDuration * 1.00 END,0),
		   -- Scheduled Cases/Hour: It is the Std. Cases/Hrs from the Routing Table.
		   @so_Sch_CsPerHour = ROUND(ISNULL(tLE.StdUnitPerHr,0),0),
		   -- Efficiency Rate:  Use Actual Cases Produced /Ran Hours divided by Std. Cases/Hrs * 100%. 
		   @so_Efficiency =  ISNULL(Case When tLE.StdUnitPerHr = 0 Then 0 Else (Case When ActualDuration = 0 Then 0 Else ActQty / ActualDuration END) / tLE.StdUnitPerHr * 100.00 END,0),
		   -- If the efficiency Rate is less than Budget Line Efficiency then set alert to '1' 
		   @so_Alert = ISNULL(Case When tLE.StdUnitPerHr = 0 Then 0 Else (Case When (Case When ActualDuration = 0 Then 0 Else ActQty / ActualDuration END) / tLE.StdUnitPerHr < tLE.StdWCEfficiency THEN '1' Else '0' END ) END,0),
		   -- Progress (Cases/Hr.):  Actual Cases produced per Hour – Std. Cases/Hrs.  
		   @so_Progress = ISNULL(Case When ActualDuration = 0 Then 0 Else ActQty  - (tLE.StdUnitPerHr * ActualDuration)  END,0)
		  ,@so_Act_Duration = ActualDuration -- Actual duration for the operator for the shop order		-- WO#359
	From #tmp_LineEff_SO tLE
	WHERE tLE.PkgLine = @chrPkgLine
		AND tLE.ShopOrder = @intShopOrder
		AND tLE.Operator = @vchStaffID
		AND tLE.Facility = @chrFacility 

	-- Find scheduled case per hour if the shop order started but has not created any pallet.
	IF ISNULL(@so_Sch_CsPerHour,0) = 0 and @sc_ShopOrder <> 0 
	BEGIN
		DECLARE @decMachineHours decimal(8,3),
				@chrBasisCode char(1) ,
				@decStdWCEfficiency as decimal(5,4);

		SET @decMachineHours = 0

		-- Try to use machine id to search first. If not found use work center of the machine to search
		EXEC [dbo].[PPsp_StdMachineEfficiencyRate_Sel]
			@vchAction = 'GetStdMachineEfficiencyRates',
			@chrFacility = @chrFacility,
			@vchItemNumber = @sc_ItemNumber,
			@chrPkgLine = @chrPkgLine,
			@decMachineHours = @decMachineHours OUTPUT,
			@chrBasisCode = @chrBasisCode OUTPUT,
			@decStdWCEfficiency = @decStdWCEfficiency OUTPUT

		Select @so_Sch_CsPerHour = Case When @decMachineHours = 0 THEN 0 ELSE Round(POWER(10, Cast(@chrBasisCode as int)) / @decMachineHours,0) END
	END

--select @so_Act_Produced as so_Act_Produced,
--	@so_Act_CsPerHour as so_Act_CsPerHour,
--	@so_Sch_CsPerHour as so_Sch_CsPerHour, 
--	@so_Efficiency as so_Efficiency,
--	@so_Alert as so_Alert,
--	@so_Progress as so_Progress

-------------------------------------------------------------------------------------------
	-- Create Line Efficiency Work File by Shift
-------------------------------------------------------------------------------------------
Print 'Create Line Efficiency Work File by Shift'

--	DECLARE @decSOActualDuration as real
--	SET @decSOActualDuration = (DateDiff(Second, ActualStartTime, ISNULL(ActualStopTime, @dteDateTime)) / 3600.00)

	-- Scheduled Cases Produced = Sum of ((Actual Shop order End time - Start time) *  standard. case/hour rate from Routing)) of all packaged Shop orders in the shift.
	Select @sft_Sch_Produced = ISNULL(SUM(Case When tLE.StdUnitPerHr = 0 Then 0 Else (ActualDuration * tLE.StdUnitPerHr) END),0), -- as ScheduledCasesProduced,
	-- Scheduled Cases/Hour = Scheduled Cases Produced / sum of Actual Duration of shop orders
		@sft_Sch_CsPerHour = ISNULL(Case When Sum(ActualDuration) = 0 Then 0 Else SUM(Case When tLE.StdUnitPerHr = 0 Then 0 Else ActualDuration * ROUND(ISNULL(tLE.StdUnitPerHr,0),0) END) / SUM(ActualDuration) END,0), -- as Sch_CsPerHour,
		-- Actual Cases Produced: Sum of the cases produced of all the SOs (Excluding the loose cases from previous operator or shift and including loose cases from current operator or shift)
		@sft_Act_Produced = ISNULL(Sum(ActQty),0), -- as ActualQty,
		-- Actual Cases/Hour: Actual Cases Produced divided by the hours in Shift Period
		@sft_Act_CsPerHour =ISNULL(Case When Sum(ActualDuration) = 0 Then 0 Else Sum(ActQty)/ SUM(ActualDuration) End ,0), -- as Act_CsPerHour,
--		-- Std. Machine Hours Earned: Units Processed * Standard Hours per unit
--		Sum(Case When tLE.StdUnitPerHr = 0 Then 0 Else (ActQty + tLC.CurLooseCases - tLC.PrvLooseCases)/ tLE.StdUnitPerHr End) As StdMachineHrEarned,
		-- Efficiency Rate: Sum of Std. Machine Hours Earned of each shop order / Sum of Run Hours in the Shift Period. 
--		Sum(Case When tLE.StdUnitPerHr = 0 Then 0 Else ActQty / tLE.StdUnitPerHr End) / SUM(ActualDuration) * 100.00 as Efficiency,
		@sft_Efficiency = ISNULL(Case When Sum(ActualDuration) = 0 Then 0 Else Sum(StdMachineHrEarned) / SUM(ActualDuration) * 100.00 End ,0), -- as Efficiency,
		-- If the efficiency Rate less than the Budget Line Efficiency, set the Alert to '1'. 
--		CASE When (Sum(Case When tLE.StdUnitPerHr = 0 Then 0 Else ActQty/ tLE.StdUnitPerHr End) / SUM(ActualDuration)) < Max(ISNULL(tLE.StdWCEfficiency,0)) THEN '1' Else '0' END as Alert,
		@sft_Alert = ISNULL(CASE When (Case When Sum(ActualDuration) = 0 Then 0 Else Sum(StdMachineHrEarned) / SUM(ActualDuration) End) < Max(ISNULL(tLE.StdWCEfficiency,0)) THEN '1' Else '0' END,0), -- as Alert,
		-- Progress (Cases/Hr.): Sum of (Actual Cases produced per Hour– Standard Unit per Hour of the shop orders) 
		@sft_Progress = ISNULL(Sum(ActQty  - tLE.StdUnitPerHr * ActualDuration),0) -- as Progress
-- WO#359 Add Start
		-- Shift Progress (Hr.): Sum of (Cases produced per Hour– Standard Unit per Hour of the shop orders) 
		,@sft_Progress_Hr = ISNULL(Sum(ActQty / tLE.StdUnitPerHr - ActualDuration),0) -- as Shift Progress
		,@sft_Act_Duration = ISNULL(SUM(ActualDuration),0) -- Actual duration for the operator in the shift
-- WO#359 Add Stop
	From #tmp_LineEff_SO tLE
--	LEFT OUTER JOIN #temp_LooseCases tLC
--		ON tLE.ShopOrder = tLC.ShopOrder
	WHERE tLE.Facility = @chrFacility 
		AND tLE.PkgLine = @chrPkgLine
		AND tLE.Operator = @vchStaffID

print '@sc_CasesScheduled =' + cast(@sc_CasesScheduled as varchar(10))
		IF @sc_ShopOrder <> 0  and ISNULL(@sft_Sch_CsPerHour,0) = 0 and  ISNULL(@sft_Sch_Produced,0) =0
		BEGIN
			SELECT @sft_Sch_Produced = ISNULL(@sc_CasesScheduled,0),	@sft_Sch_CsPerHour = ISNULL(@so_Sch_CsPerHour,0)
		END

--select @sft_Act_Produced as sft_Act_Produced,			-- Actual cases produced
--	@sft_Act_CsPerHour as sft_Act_CsPerHour,			-- Actual cases/Hour
--	@sft_Sch_Produced as sft_Sch_Produced ,
--	@sft_Sch_CsPerHour as sft_Sch_CsPerHour,			-- Scheduled cases/Hour
--	@sft_Efficiency as sft_Efficiency,	-- Actual Line Efficiency
--	@sft_Alert as sft_Alert,					-- Line Efficiency Alert
--	@sft_Progress as sft_Progress

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

Print 'Error: ' + ERROR_MESSAGE()		
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

