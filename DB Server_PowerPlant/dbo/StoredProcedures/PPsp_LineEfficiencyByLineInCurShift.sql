
-- =============================================
-- Author:		Bong Lee
-- Create date: May 6,2009
-- Description:	Line Efficency by Line
-- Ticket: 6923 May. 17, 2010   Bong Lee
-- Description: If the line is not connected to the network, the information of the line
--				was obtained from the previous line
-- WO#359		Jul.05 2011		Bong Lee
-- Description: Add a new column, Shift Progress(hr).
--				Order by line id in Descending sequence
-- WO#5370		Dec.15 2017		Bong Lee
-- Description:	Show efficiency for all Sarong lines
-- ID#7042		Feb.12 2021		Zhiyuan Xiao
-- Description:	Modify Status column with new ServerCnn Added
-- ID#7042_		Aug.30 2021		Bong Lee
-- Description: Accept to select mutiple packaging lines
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_LineEfficiencyByLineInCurShift] 
	@vchFacility as varchar(3),
	--ID#7042_	@vchPackagingLine as varchar(10) = NULL
	@vchPackagingLines as varchar(MAX) = NULL				--ID#7042_
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @sc_LogOnTime datetime ,
		@sc_ItemNumber varchar(35),
		@sc_ShopOrder int,
		@sc_StopTime as datetime,
		@sc_StartTime as datetime,
		@sc_StartDownTime as datetime,
		@sc_DefaultPkgLine as varchar(10),
		@sc_OverridePkgLine as varchar(10),
		@sc_OverrideShiftNo char(1),
		@sc_CasesScheduled int,
		@sc_CasesProduced int,
		@sc_PalletsCreated int,
		@sc_DefaultShiftNo char(1),
		@sc_Operator varchar(10),
		@sc_LooseCases int ,
		@sc_ProductionDate datetime ,
		@sc_CarriedForwardCases int,
		@sc_ShiftProductionDate datetime
		,@sc_Act_Duration decimal(10,2)		-- Actual Shop order in Shift Duration	WO#359
		,@sc_ServerCnn bit					-- Add ServerCnn ID#7042

	DECLARE	@return_value int,
		@so_Act_Produced int,
		@so_Act_CsPerHour int,
		@so_Sch_CsPerHour int,
		@so_Efficiency decimal(10, 2),
		@so_Alert bit,
		@so_Progress int,
		@so_Act_Duration decimal(10,2),		-- WO#359
		@sft_Act_Produced int,
		@sft_Act_CsPerHour int,
		@sft_Sch_Produced int,
		@sft_Sch_CsPerHour int,
		@sft_Efficiency decimal(10, 2),
		@sft_Alert bit,
		@sft_Progress int
		,@sft_Progress_Hr decimal(6,2)		-- WO#359
		,@sft_Act_Duration decimal(10,2) 	-- Actual Shift Duration	WO#359

	DECLARE @vchComputerName varchar(50)
	DECLARE @cc_PackagingLine varchar(10)	-- WO#359
	DECLARE @vchSQLStmt varchar(1000)
	DECLARE @dteCurDateTime datetime
	DECLARE	@intCurShiftNo int
	DECLARE @dteCurShiftProductionDate datetime
	DECLARE @dteShiftProductionDate datetime
	DECLARE @int_OperatorDownTimeShiftTotal int
	DECLARE @int_OperatorSODownTimeShiftTotal int
	DECLARE @intCurrentDownTime int
	
	Declare @vchOperatorName as varchar(100)
	Declare @vchLineDesc as varchar(30)
	Declare @dteShiftStartTime as datetime
	Declare @bitForgotLogOff as bit

	DECLARE @tblSC Table (
		LogOnTime datetime,
		StartTime datetime,
		DefaultPkgLine varchar(10),
		OverridePkgLine varchar(10),
		ShopOrder int,
		ItemNumber varchar(35),
		Operator varchar(10),
		DefaultShiftNo char(1),
		OverrideShiftNo char(1),
		CasesScheduled int,
		CasesProduced int,
		PalletsCreated int,
		LooseCases int ,
		ProductionDate datetime ,
		CarriedForwardCases int,
		ShiftProductionDate datetime,
-- ID#7042	StartDownTime datetime)
		StartDownTime datetime		-- ID#7042
		,ServerCnn bit				-- ID#7042
		)							-- ID#7042

	DECLARE @tblEfficiency Table (
		Active bit,
		ServerCnn bit,				-- ID#7042
		PackagingLine varchar(10),
		PkgLineDesc varchar(30),
		Operator varchar(10),
		OperatorName varchar(100),
		ForgotLogOff bit,
		ShopOrder int,
		ItemNumber varchar(35),
		SOStartTime datetime,
		LogOnTime datetime,
		StartDownTime datetime,
		CurServerTime datetime,
		OperatorSODownTimeShiftTotal int,
		OperatorDownTimeShiftTotal int,
		so_Act_Produced int,
		so_Act_CsPerHour int,
		so_CasesScheduled int,
		so_Sch_CsPerHour int,
		so_Efficiency decimal(10, 2),
		so_Alert bit,
		so_Progress int,
		so_Act_Duration decimal(10, 2),	-- WO#359
		sft_Act_Produced int,
		sft_Act_CsPerHour int,
		sft_Sch_Produced int,
		sft_Sch_CsPerHour int,
		sft_Efficiency decimal(10, 2),
		sft_Alert bit,
		sft_Progress int
		,sft_Progress_Hr decimal(6,2)		-- WO#359
		,sft_Act_Duration decimal(10, 2)	-- WO#359

	)	

	BEGIN TRY
		
		
		DECLARE table_cursor CURSOR FOR	
-- WO#359	Select T1.ComputerName, T2.Description from tblComputerConfig T1
		Select T1.ComputerName, T1.PackagingLine, T2.Description from tblComputerConfig T1	-- WO#359
			Left outer join tblEquipment T2 
			On T1.Facility = T2.Facility and T1.PackagingLine = t2.EquipmentID
			-- WO#37864 ADD Start
			Where T1.facility = @vchFacility 
			and RecordStatus = 1 
			-- ID#7042_	and (@vchPackagingLine = 'ALL' OR @vchPackagingLine = T1.PackagingLine)
			and (@vchPackagingLines = 'ALL' OR T1.PackagingLine IN (SELECT value FROM STRING_SPLIT(@vchPackagingLines, ',')))		-- ID#7042_
			and T1.PackagingLine <> 'Spare      '
			and (T1.ShowEfficiency = 1 or T1.PkgLineType in ('Sarong','590','Optima'))
			-- WO#37864 ADD StOP
		
			-- WO#37864	Where T1.facility = @vchFacility and recordstatus = 1 and (@vchPackagingLine = 'ALL' OR @vchPackagingLine = T1.PackagingLine)
			-- WO#37864	and T1.PackagingLine <> 'Spare      ' 	and (T1.ShowEfficiency = 1 or T1.PkgLineType = 'Sarong')		-- WO#5370			
			--WO#5370	and T1.PackagingLine <> 'Spare      ' 	and T1.ShowEfficiency = 1
-- WO#359		Order by Description

		OPEN table_cursor
-- WO#359	FETCH NEXT FROM table_cursor INTO 	@vchComputerName, @vchLineDesc
			FETCH NEXT FROM table_cursor INTO @vchComputerName,@cc_PackagingLine, @vchLineDesc	-- WO#359
			WHILE @@FETCH_STATUS = 0
			BEGIN

			Select 
-- ticket#6923 Add Begin
						@vchOperatorName = null ,
						@sc_ShopOrder = null,
						@sc_ItemNumber = null,
						@sc_StartTime = null,
						@sc_LogOnTime = null, @so_Act_Produced = 0,
						@so_Act_CsPerHour = 0,
						@sc_CasesScheduled = 0,
						@so_Sch_CsPerHour = 0,
						@so_Efficiency = 0,
						@so_Alert = 0,
						@so_Progress = 0, 
-- ticket# 6923 Add End
				@sft_Act_Produced = 0,
				@sft_Act_CsPerHour = 0,	@sft_Sch_Produced = 0, @sft_Sch_CsPerHour = 0,
				@sft_Efficiency  = 0, @sft_Alert = 0, @sft_Progress = 0,
				@int_OperatorDownTimeShiftTotal = 0,	@int_OperatorSODownTimeShiftTotal = 0
				,@sft_Progress_Hr = 0	-- WO#359

				Set @vchSQLStmt = 'Select LogOnTime, StartTime, DefaultPkgLine, OverridePkgLine, ShopOrder, ItemNumber, ' +
					'Operator, DefaultShiftNo, OverrideShiftNo, CasesScheduled, CasesProduced, PalletsCreated, ' +
					-- ID#7042	'LooseCases, ProductionDate, CarriedForwardCases, ShiftProductionDate, StartDownTime ' +
					'LooseCases, ProductionDate, CarriedForwardCases, ShiftProductionDate, StartDownTime, ServerCnnIsOk ' +   -- ID#7042
					'from [' + @vchComputerName + '\sqlexpress].LocalPowerPlant.dbo.tblSessioncontrol'
				Begin try 
print @vchSQLStmt 
					Insert into @tblSC	
					exec (@vchSQLStmt)

					Select @sc_LogOnTime = LogOnTime, @sc_StartTime = StartTime, @sc_DefaultPkgLine = DefaultPkgLine, @sc_OverridePkgLine = OverridePkgLine, 
							@sc_ShopOrder = ShopOrder, @sc_ItemNumber = ItemNumber, 
							@sc_Operator = Operator, @sc_DefaultShiftNo = DefaultShiftNo, @sc_OverrideShiftNo = OverrideShiftNo, 
							@sc_CasesScheduled = CasesScheduled, @sc_CasesProduced = CasesProduced, @sc_PalletsCreated = PalletsCreated, 
							@sc_LooseCases = LooseCases, @sc_ProductionDate = ProductionDate, @sc_CarriedForwardCases = CarriedForwardCases,
							@sc_ShiftProductionDate = ShiftProductionDate, @sc_StartDownTime = StartDownTime
							,@sc_ServerCnn = ServerCnn	-- ID#7042
								From @tblSC
--select * from @tblSC
					-- clear the table
					Delete from @tblSC
					
					-- initialize work fields
					Select   @so_Act_Produced = 0, @so_Act_CsPerHour =0, @so_Sch_CsPerHour = 0, @so_Efficiency = 0, @so_Alert = 0, @so_Progress = 0,
						@sft_Act_Produced = 0, @sft_Act_CsPerHour = 0, @sft_Sch_Produced = 0, @sft_Sch_CsPerHour = 0, @sft_Efficiency = 0, 
						@sft_Alert = 0, @sft_Progress = 0
						,@sft_Progress_Hr = 0	-- WO#359

					-- get current shift no.
					Select @dteCurDateTime = getdate()
					Select @intCurShiftNo = Shift, @dteShiftStartTime = ShiftStartTime, @dteCurShiftProductionDate = ProductionDate 
						From dbo.fnShiftInfo (@vchFacility, @dteCurDateTime, 0,NULL, @sc_DefaultPkgLine)

/* For debugging purpose */
/* use it to copy and paste to the test sql statement */
--print '@chrFacility = ''' + @vchFacility + 
--	''', @chrPkgLine = ''' + @sc_DefaultPkgLine + 
--	''', @vchStaffID = ''' + @sc_Operator +
--	''', @intShift = ' + cast(@intCurShiftNo as char(1)) +
--	', @intShopOrder = ' +  cast(@sc_ShopOrder as varchar(10)) +
--	', @dteDateTime = ''' +  cast(@dteCurDateTime as varchar(21)) +
--	''', @sc_StartTime = ''' +  cast(@sc_StartTime as varchar(21)) +
--	''', @sc_DefaultPkgLine = ''' +  @sc_DefaultPkgLine +
--	''', @sc_OverridePkgLine = ''' +  @sc_OverridePkgLine +
--	''', @sc_ShopOrder = ' +  cast(@sc_ShopOrder as varchar(10)) +
--	', @sc_ItemNumber = ''' +  @sc_ItemNumber +
--	''', @sc_Operator = ''' +  @sc_Operator +
--	''', @sc_DefaultShiftNo = ' +  cast(@sc_DefaultShiftNo as varchar(1)) +
--	', @sc_OverrideShiftNo = ' +  cast(@sc_OverrideShiftNo as varchar(1)) +
--	', @sc_CasesScheduled = ' +  cast(@sc_CasesScheduled as varchar(10)) +
--	', @sc_CasesProduced = ' +  cast(@sc_CasesProduced as varchar(10)) +
--	', @sc_PalletsCreated = ' +  cast(@sc_PalletsCreated as varchar(10)) +
--	', @sc_LooseCases = ' +  cast(@sc_LooseCases as varchar(10)) +
--	', @sc_ProductionDate = ''' +  cast(@sc_ProductionDate as varchar(21)) +
--	''', @sc_CarriedForwardCases = ' +  cast(@sc_CarriedForwardCases as varchar(10)) +
--	', @sc_ShiftProductionDate = ''' +  cast(@sc_ShiftProductionDate as varchar(21)) + ''''

					IF @sc_ShopOrder > 0 AND @sc_OverrideShiftNo = @intCurShiftNo AND @sc_ShiftProductionDate = @dteCurShiftProductionDate
						EXEC	@return_value = [dbo].[PPsp_LineEfficiency]
								@vchFacility,
								@sc_DefaultPkgLine,
								@sc_Operator,
								@intCurShiftNo,
								@sc_ShopOrder,
								@dteCurDateTime,
								@sc_StartTime,
								@sc_DefaultPkgLine,
								@sc_OverridePkgLine,
								@sc_ShopOrder,
								@sc_ItemNumber,
								@sc_Operator,
								@sc_DefaultShiftNo,
								@sc_OverrideShiftNo,
								@sc_CasesScheduled,
								@sc_CasesProduced,
								@sc_PalletsCreated,
								@sc_LooseCases,
								@sc_ProductionDate,
								@sc_CarriedForwardCases,
								@sc_ShiftProductionDate,
								@so_Act_Produced OUTPUT,
								@so_Act_CsPerHour OUTPUT,
								@so_Sch_CsPerHour OUTPUT,
								@so_Efficiency OUTPUT,
								@so_Alert OUTPUT,
								@so_Progress OUTPUT,
								@so_Act_Duration OUTPUT,	-- WO#359
								@sft_Act_Produced OUTPUT,
								@sft_Act_CsPerHour OUTPUT,
								@sft_Sch_Produced OUTPUT,
								@sft_Sch_CsPerHour OUTPUT,
								@sft_Efficiency OUTPUT,
								@sft_Alert OUTPUT,
								@sft_Progress OUTPUT
								,@sft_Progress_Hr OUTPUT	-- WO#359
								,@sft_Act_Duration OUTPUT	-- WO#359

					Set @bitForgotLogOff = 0

					-- If shop order is not started in the line, blank out some output fields.
					IF @sc_ShopOrder > 0 
					BEGIN
						-- Get Operator's name
						Select top 1 @vchOperatorName = FirstName + ' ' + LastName From dbo.tblPlantStaff 
							Where Facility = @vchFacility and StaffID = @sc_Operator
						
						-- Did operator forgot to log off?
						IF DateDiff(minute,@sc_LogOnTime, @dteShiftStartTime) > 15 
							Set @bitForgotLogOff = 1

						Set @dteShiftProductionDate = @sc_ShiftProductionDate

						/* Calculate operator down time	*/

						-- If the line current is down, calulate the down time up-to now from the beginning of current down time
						If @sc_StartDownTime is not Null
							Set @intCurrentDownTime = DateDiff(minute,@sc_StartDownTime, @dteCurDateTime)
						ELSE
							Set @intCurrentDownTime = 0

						-- Total down time of the packaging line for the shop order which is packed by the operator in the shift 
						Select @int_OperatorSODownTimeShiftTotal = ISNULL(Sum(ISNULL(MaxDownTime,0)),0) + @intCurrentDownTime 
							From 
								(Select facility, MachineID, ShopOrder, Operator, DownTimeBegin,max(DateDiff(Minute,DownTimeBegin,DownTimeEnd)) as MaxDownTime
									From dbo.tblDownTimeLog
									Where Facility = @vchFacility and InActive = 0 
										  AND shiftProductionDate = @dteShiftProductionDate and shift = @intCurShiftNo and MachineID = @sc_DefaultPkgLine and Operator = @sc_Operator and ShopOrder = @sc_ShopOrder
									Group by facility,MachineID, ShopOrder, Operator, DownTimeBegin) tDL	

						-- Total down time of the packaging line for the operator in the shift 
						Select @int_OperatorDownTimeShiftTotal = ISNULL(Sum(ISNULL(MaxDownTime,0)),0) + @intCurrentDownTime 
							From 
								(Select facility, MachineID, Operator, DownTimeBegin, max(DateDiff(Minute,DownTimeBegin,DownTimeEnd)) as MaxDownTime
									From dbo.tblDownTimeLog
									Where Facility = @vchFacility and InActive = 0 
										  AND shiftProductionDate = @dteShiftProductionDate and shift = @intCurShiftNo and MachineID = @sc_DefaultPkgLine and Operator = @sc_Operator
									Group by facility,MachineID, Operator, DownTimeBegin) tDL				

					END
					ELSE  -- Shop order stopped or packaging line stops packing, zeroized some result fields
					BEGIN
						Select @sc_ShopOrder = NULL, @sc_StartTime = NULL, @sc_CasesScheduled = 0,	@vchOperatorName = NULL,
							@so_Act_Produced = 0, @so_Act_CsPerHour =0, @so_Sch_CsPerHour = 0, @so_Efficiency = 0, @so_Alert = 0, 
							@so_Progress = 0, @int_OperatorDownTimeShiftTotal = 0,	@int_OperatorSODownTimeShiftTotal = 0

						Set @dteShiftProductionDate = @dteCurShiftProductionDate
					END

					-- get description of packaging line from equiptment table
					Select @vchLineDesc = Description from dbo.tblEquipment 
						Where facility = @vchFacility and EquipmentID = @sc_DefaultPkgLine
					
					INSERT INTO @tblEfficiency
					VALUES (1,
						@sc_ServerCnn,					-- ID#7042
						@sc_DefaultPkgLine,
						@vchLineDesc,
						@sc_Operator,
						@vchOperatorName,
						@bitForgotLogOff,
						@sc_ShopOrder,
						@sc_ItemNumber,
						@sc_StartTime,
						@sc_LogOnTime,
						@sc_StartDownTime,
						@dteCurDateTime,
						@int_OperatorSODownTimeShiftTotal,
						@int_OperatorDownTimeShiftTotal,
						ISNULL(@so_Act_Produced,0),
						ISNULL(@so_Act_CsPerHour,0),
						ISNULL(@sc_CasesScheduled,0),
						ISNULL(@so_Sch_CsPerHour,0),
						ISNULL(@so_Efficiency,0),
						@so_Alert,
						ISNULL(@so_Progress,0),
						ISNULL(@so_Act_Duration,0),		-- WO#359
						ISNULL(@sft_Act_Produced,0),
						ISNULL(@sft_Act_CsPerHour,0),
						ISNULL(@sft_Sch_Produced,0),
						ISNULL(@sft_Sch_CsPerHour,0),
						ISNULL(@sft_Efficiency,0),
						@sft_Alert,
-- WO#359				ISNULL(@sft_Progress,0))
						ISNULL(@sft_Progress,0)			-- WO#359
						,ISNULL(@sft_Progress_Hr,0)		-- WO#359
						,ISNULL(@sft_Act_Duration,0)	-- WO#359
						)								-- WO#359
				End try
				Begin catch

					INSERT INTO @tblEfficiency
					VALUES (0,
			-- WO#359   @sc_DefaultPkgLine,
						@sc_ServerCnn,					-- ID#7042
						@cc_PackagingLine,				-- WO#359
						@vchLineDesc,
						@sc_Operator,
						@vchOperatorName,
						@bitForgotLogOff,
						@sc_ShopOrder,
						@sc_ItemNumber,
						@sc_StartTime,
						@sc_LogOnTime,
						@sc_StartDownTime,
						@dteCurDateTime,
						@int_OperatorSODownTimeShiftTotal,
						@int_OperatorDownTimeShiftTotal,
						@so_Act_Produced,
						@so_Act_CsPerHour,
						@sc_CasesScheduled,
						@so_Sch_CsPerHour,
						@so_Efficiency,
						@so_Alert,
						@so_Progress,
						@so_Act_Duration,			-- WO#359
						@sft_Act_Produced ,
						@sft_Act_CsPerHour,
						@sft_Sch_Produced,
						@sft_Sch_CsPerHour,
						@sft_Efficiency,
						@sft_Alert,
-- WO#35				@sft_Progress)
						@sft_Progress				-- WO#359
						,@sft_Progress_Hr			-- WO#359
						,@sft_Act_Duration			-- WO#359
						)							-- WO#359
				End catch

-- WO#359	FETCH NEXT FROM table_cursor INTO 	@vchComputerName, @vchLineDesc
			FETCH NEXT FROM table_cursor INTO @vchComputerName,@cc_PackagingLine, @vchLineDesc	-- WO#359
		END

		CLOSE table_cursor
		DEALLOCATE table_cursor

-- WO#359	Select * From @tblEfficiency Order by PkgLineDesc
		Select * From @tblEfficiency		-- WO#359
			Order by PackagingLine			-- WO#359

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

