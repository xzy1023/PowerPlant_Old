
-- =============================================
-- Author:		Bong Lee
-- Create date: March 10, 2009
-- Description:	Add Record to Pallet Table
-- WO#359:		Feb. 07, 2012	Bong Lee
-- Description: Write in Shift Production date in yyyy/mm/dd 00:00:00 format
-- WO#871	  : March 17, 2014	Bong Lee
-- Description:	Add pallet record to Probat interface Table and
--				Add the similar pallet record to BPCS for Probat Enabled lines
-- WO#1096	  : Dec 13, 2014	Bong Lee
-- Description:	Use column MachineID in tblProbatEquipment since the column name renamed.
-- WO#2563	  : Sep 25, 2015	Bong Lee
-- Description:	Add output location to tblPallet.
-- FIX20160309: Mar 29, 2016	Bong Lee
-- Description:	Use the default Packaging line (Session Control) instead of packaging line in the shop order table 
--				to get the receiving station.
-- ALM#11828  : Apr 05, 2016	Bong Lee
-- Description:	Add DestinationShopOrder to tblPallet.
-- FIX20170106: Jan 07, 2017	Bong Lee
-- Description:	Error occur when it runs on IPC instead of server.
-- FIX20170502: May 02, 2017	Bong Lee
-- Description:	Excluding the multiple raw material components with zero quantity in the BOM records.
-- V7.01:		Jnn 05, 2025 Zhi & Saga
-- Description:	Add Batch Number Generation Logic
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_Pallet_Add] 
	-- Add the parameters for the stored procedure here
	@chrFacility char(3), 
	@intPalletID int,
	@intShopOrder int,
	@vchItemNumber varchar(35),
	@intQuantity int, 
	@dteStartTime datetime, 
	@chrDefaultPkgLine char(10), 
	@vchOperator varchar(10), 
	@chrOrderComplete char(1), 
-- ALM#11828	@bitPrintStatus bit, 
	@intPrintStatus smallint,			-- ALM#11828
	@intQtyPerPallet int, 
	@vchLotID varchar(25),
	@dteProductionDate datetime, 
--	@chrProductionDate char(8), 
	@chrExpiryDate char(8), 
	@intShiftNo tinyint,
	@bitIsPalletStation bit
	-- WO#817 Add Start
	,@bitProbatEnabled bit
	-- WO#817 Add End
	,@vchOutputLocation varchar(10)	-- WO#2563
	,@intDestinationShopOrder int	-- ALM#11828
AS
BEGIN

	SET NOCOUNT ON;
	-- WO#817 Add Start
	SET XACT_ABORT ON;			
	DECLARE @vchShopOrderName varchar(20);
	DECLARE @intShopOrderType int;
	DECLARE @dteSOScheduledStartTime datetime;
	DECLARE @vchRecStation varchar(20);
	DECLARE @intRawMaterialUsed int				-- (1 decimal place)
	DECLARE @dteCreationTime as datetime;
	DECLARE @vchComponentItem as varchar(35)
	DECLARE @vchPalletID as varchar(20);
	DECLARE @vchCreatedBy as varchar(10)
	DECLARE @intCreationDate as int;
	DECLARE @intCreationTime as int;
	DECLARE @chrCreationDate as char(8);
	DECLARE @chrCreationTime as char(6);
	DECLARE @vchCreateionDateTime as varchar(25);
	-- FIX20170106	DECLARE @vchInterfaceServerName as varchar(255);	
	DECLARE @vchProbatDBName as varchar(255); 
	DECLARE @vchSQLStmt varchar(1024);
	-- WO#817 Add End
	DECLARE @vchBatchNumber varchar(20);

	Declare @dteShiftProductionDate as datetime
	BEGIN TRY
		-- WO#817 Add Start
		SET @dteCreationTime = GetDate();
		SELECT @chrCreationDate = CONVERT([char](8),@dteCreationTime,(112))
			  ,@chrCreationTime = REPLACE(CONVERT(char(8), @dteCreationTime,108),':','')
			  ,@vchCreateionDateTime = CONVERT([varchar](25),@dteCreationTime,(121));

		IF @bitProbatEnabled = 1
		BEGIN

			SELECT @vchPalletID = CAST(@intPalletID as varchar(20))
				,@intCreationDate = CAST(@chrCreationDate as int)
				,@intCreationTime = CAST(@chrCreationTime as int)

			IF CHARINDEX('\SQLEXPRESS',@@servername) > 0 
				-- FIX20170106 SELECT @vchInterfaceServerName = '[.\SQLEXPRESS].', @vchCreatedBy = '.\SQLEXPRESS', @vchProbatDBName = db_name()
				SELECT @vchCreatedBy = '.\SQLEXPRESS', @vchProbatDBName = db_name()	-- FIX20170106
			ELSE
				-- FIX20170106	SELECT @vchInterfaceServerName = '', @vchCreatedBy = Value1, @vchProbatDBName = Value2
				SELECT @vchCreatedBy = Value1, @vchProbatDBName = Value2						-- FIX20170106
				FROM tblControl
				WHERE ([Key] = N'ProbatInterfaceDB') AND (SubKey = N'Probat')

			SELECT @vchComponentItem = tSO.ComponentItem
				,@vchShopOrderName = tSO.ProbatOrderName
				,@intShopOrderType = tSO.OrderType
				,@dteSOScheduledStartTime = CONVERT(datetime, (CONVERT(char(10),tSO.StartDate)),112) + dbo.fnCvtNumTimeToDateTime(tSO.StartTime)
				,@vchRecStation = CASE WHEN tPEQ.ProbatEqID = '' 
									   THEN 
										  CASE WHEN tSO.FlavoredCoffee = 'Y' THEN tPEQ.MachineFlavor 
												WHEN tSO.OrderType = 3 THEN tPEQ.MachineWholeBean
												WHEN tSO.OrderType = 5 THEN tPEQ.MachineGround
												ELSE '' 
										  END
										ELSE tPEQ.ProbatEqID
								  END
			FROM tblShopOrder tSO
			LEFT OUTER JOIN tblProbatEquipment tPEQ 
			ON	tSO.Facility = tPEQ.Facility 
				AND	@chrDefaultPkgLine = tPEQ.MachineID			-- FIX20160309
-- FIX20160309	AND tSO.PackagingLine = tPEQ.MachineID			-- WO#1096 
-- WO#1096  	AND tSO.PackagingLine = tPEQ.BPCSMachineID
			WHERE tSO.ShopOrder = @intShopOrder
				AND tPEQ.Type = 'P'

			-- Raw material component consumption is in lb. and 1 decimal place for Probat
			SET @intRawMaterialUsed = 0;
			IF @vchComponentItem <> ''
				-- FIX20170502	SELECT @intRawMaterialUsed = ROUND(ISNULL([Quantity],0) * @intQuantity, 1 ) * 10 from [dbo].[tblBillOfMaterials]
				SELECT top 1 @intRawMaterialUsed = ROUND(ISNULL([Quantity],0) * @intQuantity, 1 ) * 10 from [dbo].[tblBillOfMaterials]			-- FIX20170502
				WHERE ShopOrder = @intShopOrder
					AND ComponentItem = @vchComponentItem
					AND ISNULL([Quantity],0) <> 0									-- FIX20170502
		END

		SET @vchBatchNumber = CAST(@intShopOrder AS VARCHAR) + '-' + CONVERT(VARCHAR(6), @dteProductionDate, 12)		-- V7.01

		BEGIN TRANSACTION

			IF @chrOrderComplete = 'N' OR @intQuantity > 0 
			BEGIN
			-- WO#817 Add End

				If @bitIsPalletStation = 1
		--			Select @dteShiftProductionDate = Convert(datetime,STUFF(STUFF(@chrProductionDate, 5, 0, '/'),8,0,'/'),111)
		-- WO#359	Select @dteShiftProductionDate = Convert(datetime,@dteProductionDate,111)
					Select @dteShiftProductionDate = CONVERT(datetime,CONVERT(CHAR(10),@dteProductionDate,111),111)  --WO#359
				Else
					select @dteShiftProductionDate = dbo.fnGetProdDateByShift(@chrFacility,@intShiftNo,@dteProductionDate,@chrDefaultPkgLine,NULL)
		--			select @dteShiftProductionDate = dbo.fnGetProdDateByShift(@chrFacility,@intShiftNo,getdate(),@chrDefaultPkgLine,NULL)
			
				INSERT INTO dbo.tblPallet 
					 (facility, PalletID, ShopOrder, ItemNumber, Quantity,  StartTime, DefaultPkgLine, Operator,
					  CreationDate, CreationTime,	-- WO#817
					  -- WO#2563	OrderComplete, PrintStatus, QtyPerPallet, LotID, ProductionDate, ExpiryDate, ShiftProductionDate, ShiftNo)
					  -- WO#2563 ADD Start
					  OrderComplete, PrintStatus, QtyPerPallet, LotID, ProductionDate, ExpiryDate, ShiftProductionDate, ShiftNo
					  ,OutputLocation
					  ,DestinationShopOrder   -- ALM#11828
					  ,[BatchNumber]
					  )	
					  -- WO#2563 ADD Stop
					 VALUES (@chrFacility, @intPalletID, @intShopOrder, @vchItemNumber, @intQuantity, @dteStartTime, @chrDefaultPkgLine,
		-- WO#817	  @vchOperator, @chrOrderComplete, @bitPrintStatus, 
		-- ALM#11828  @vchOperator, @chrCreationDate, @chrCreationTime, @chrOrderComplete, @bitPrintStatus, -- WO#817
					  @vchOperator, @chrCreationDate, @chrCreationTime, @chrOrderComplete, @intPrintStatus, -- ALM#11828
					  -- WO#2563 @intQtyPerPallet, @vchLotID, Convert(varchar(8),@dteProductionDate,112), @chrExpiryDate, @dteShiftProductionDate, @intShiftNo )
					  -- WO#2563 ADD Start
					  @intQtyPerPallet, @vchLotID, Convert(varchar(8),@dteProductionDate,112), @chrExpiryDate, @dteShiftProductionDate, @intShiftNo 
					  ,@vchOutputLocation
					  ,@intDestinationShopOrder   -- ALM#11828
					  ,@vchBatchNumber
					  )
					  -- WO#2563 ADD Stop

			-- WO#817 Add Start	
			END

			IF @bitProbatEnabled = 1 and @vchComponentItem <> ''
			BEGIN

				--Save the probat pallet interface record for BPCS			
				INSERT INTO tblProbatPallet
					(CUSTOMER_ID, TRANSFERED, TRANSFERED_TIMESTAMP, ACTIVITY, PALLET_ID, CREATE_TIMESTAMP, RECEIVING_STATION, CUSTOMER_CODE, AMOUNT, 
					ORDER_NAME, ORDER_TYP, START_TIMESTAMP, ORDER_COMPLETE, ShopOrder, PalletID_PXPAL, QtyOnPallet, TransactionSeq, 
					CreationDate, CreationTime, CreatedBy, MaintenanceDate, MaintenanceTime, MaintainedBy, Facility)
				VALUES (@vchPalletID, 0, @dteCreationTime, 'I', @vchPalletID, @dteCreationTime, @vchRecStation, @vchComponentItem, @intRawMaterialUsed 
					,@vchShopOrderName, @intShopOrderType, @dteSOScheduledStartTime, @chrOrderComplete, @intShopOrder, @vchPalletID, @intQuantity, @intPalletID
					,@intCreationDate, @intCreationTime, @vchCreatedBy, @intCreationDate, @intCreationTime, @vchCreatedBy, @chrFacility)
		
				--Write the probat pallet interface record to Probat
				-- FIX20170106	SET @vchSQLStmt = 'INSERT INTO ' + @vchInterfaceServerName + @vchProbatDBName + '.dbo.PRO_IMP_PALLET ' +
				SET @vchSQLStmt = 'INSERT INTO ' + @vchProbatDBName + '.dbo.PRO_IMP_PALLET ' +			-- FIX20170106
									'(CUSTOMER_ID, TRANSFERED, TRANSFERED_TIMESTAMP, ACTIVITY, PALLET_ID, CREATE_TIMESTAMP, RECEIVING_STATION, ' +
									'CUSTOMER_CODE, AMOUNT, ORDER_NAME, ORDER_TYP, START_TIMESTAMP, ORDER_COMPLETE ' +
									')' +
									'VALUES(''' + @vchPalletID + ''', 0,''' +
									@vchCreateionDateTime + ''',''I'',''' +
									@vchPalletID + ''',''' +
									@vchCreateionDateTime + ''',''' +
									@vchRecStation + ''',''' +
									@vchComponentItem + ''',' +
									CAST(@intRawMaterialUsed AS varchar(9)) + ',''' +
									@vchShopOrderName + ''',' +
									CAST(@intShopOrderType AS varchar(3)) + ',''' +
									CONVERT(varchar(25),@dteSOScheduledStartTime,120) + ''',''' +
									@chrOrderComplete + '''' +
									')' 

				IF @vchSQLStmt IS NOT NULL		
				BEGIN
				  print @vchSQLStmt
				  EXEC (@vchSQLStmt)								
				END
			END
		COMMIT TRANSACTION
		-- WO#817 Add End
		
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		-- WO#817 Add Start		
		IF (XACT_STATE()) = -1
			ROLLBACK TRANSACTION 
		-- WO#817 Add End
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

