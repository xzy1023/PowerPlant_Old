
-- =============================================
-- WO#1297      Aug. 4, 2014   Bong Lee
-- Description:	Post process for uploading Power Plant Pallets to MS Dynamics AX for posting
-- WO#2563	  : Sep 25, 2015	Bong Lee
-- Description:	Add output location to tblPallet and tblPalletHst.
-- ALM#11828  : Apr 05, 2016	Bong Lee
-- Description:	Add DestinationShopOrder to tblPalletHst.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PostProcess_PostedPallets] 
					
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	DECLARE @vchSQLStmt varchar(512);
	DECLARE @vchLogFileName varchar(255);
    DECLARE @chrFacility char(3);
	DECLARE @intPalletID int;
	DECLARE @intQtyPerPallet int;
	DECLARE @intQuantity int;
	DECLARE @vchItemNumber varchar(35);
	DECLARE @chrDefaultPkgLine char(10);
	DECLARE @vchOperator varchar(10);
	DECLARE @chrCreationDate char(8);
	DECLARE @chrCreationTime char(6);
	DECLARE @chrOrderComplete char(1);
	DECLARE @vchLotID  varchar(25);
	DECLARE @intShopOrder int;
	DECLARE @dteStartTime datetime;
	DECLARE @chrProductionDate char(8);
	DECLARE @chrExpiryDate char(8);
	DECLARE @dteCreationDateTime datetime;
	DECLARE @dteShiftProductionDate datetime;
	DECLARE @intShiftNo Tinyint;
	DECLARE @vchOutputLocation varchar(10);				-- WO#2563
	DECLARE @vchDestinationShopOrder int;				-- ALM#11828
	DECLARE @dteLastUpdate datetime;
	DECLARE @vchBatchNumber varchar(20);

	-- WO#543 Add Start	
	DECLARE @vchText varchar(255);
	DECLARE @vchPalletIDs varchar(1000);
	DECLARE @intCount int;

	DECLARE @tblPalletsToBeArchived as TABLE
	(
	  PalletID int
	)

	DECLARE @tblPalletsArchived as TABLE
	(
	  PalletID int
	)

	SET NOCOUNT ON;
	BEGIN TRY

		Select @vchLogFileName = Value1  + 'UpLoadLog_' + convert(varchar(8),getdate(),112) + '.txt' 
			FROM [tblControl] 
			WHERE [Key] = 'ERPDataXChgLogFolder' and [SubKey] = 'Interface';

		SET  @vchText = '*** Archive pallet information Begin.' ;
		EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText;

		SET @vchPalletIDs = '';
	
		DECLARE csrPallet CURSOR SCROLL FOR	
			SELECT Facility, PalletID, QtyPerPallet, Quantity, ItemNumber, DefaultPkgLine, Operator,   
					CreationDate, CreationTime, OrderComplete, LotID, ShopOrder, StartTime, 
					ProductionDate, ExpiryDate, CreationDateTime, ShiftProductionDate, ShiftNo, LastUpdate
					,OutputLocation						-- WO#2563
					,DestinationShopOrder				-- ALM#11828
					,BatchNumber
				FROM tblPallet 
				WHERE PrintStatus = 3 
				ORDER BY RRN

		OPEN csrPallet

		-- Get the list of pallet ids to write to the log.
		-- read a record from the tblPallet and load the data to the variables
		FETCH NEXT FROM csrPallet INTO  @chrFacility, @intPalletID, @intQtyPerPallet, @intQuantity, @vchItemNumber, 
				@chrDefaultPkgLine, @vchOperator, @chrCreationDate, @chrCreationTime, @chrOrderComplete, @vchLotID, 
				@intShopOrder, @dteStartTime, @chrProductionDate, @chrExpiryDate, @dteCreationDateTime, 
				@dteShiftProductionDate, @intShiftNo, @dteLastUpdate
				,@vchOutputLocation						-- WO#2563
				,@vchDestinationShopOrder				-- ALM#11828
				,@vchBatchNumber
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT INTO @tblPalletsToBeArchived (PalletID) VALUES(@intPalletID)
			SET @vchPalletIDs = @vchPalletIDs + ' ' + Cast(@intPalletID as varchar(20))

			-- read a record from the tblPallet and load the data to the variables
			FETCH NEXT FROM csrPallet INTO  @chrFacility, @intPalletID, @intQtyPerPallet, @intQuantity, @vchItemNumber, 
					@chrDefaultPkgLine, @vchOperator, @chrCreationDate, @chrCreationTime, @chrOrderComplete, @vchLotID, 
					@intShopOrder, @dteStartTime, @chrProductionDate, @chrExpiryDate, @dteCreationDateTime, 
					@dteShiftProductionDate, @intShiftNo, @dteLastUpdate
					,@vchOutputLocation					-- WO#2563
					,@vchDestinationShopOrder			-- ALM#11828
					,@vchBatchNumber
		END

		SET  @vchText ='Pallets To Be Archived: ' + CASE WHEN @vchPalletIDs = '' THEN 'Nothing.' ELSE LTRIM(@vchPalletIDs) END;
		exec PPsp_AppendToTextFile @vchLogFileName, @vchText;
	
		-- Reset variable for actual process
		SET @vchPalletIDs = '';


		-- Reset file pointer, read a record from beginning of the tblPallet and load the data to the variables
		FETCH FIRST FROM csrPallet INTO  @chrFacility, @intPalletID, @intQtyPerPallet, @intQuantity, @vchItemNumber,
				@chrDefaultPkgLine, @vchOperator, @chrCreationDate, @chrCreationTime, @chrOrderComplete, @vchLotID, 
				@intShopOrder, @dteStartTime, @chrProductionDate, @chrExpiryDate, @dteCreationDateTime, 
				@dteShiftProductionDate, @intShiftNo, @dteLastUpdate
				,@vchOutputLocation										-- WO#2563
				,@vchDestinationShopOrder								-- ALM#11828
				,@vchBatchNumber
		
		WHILE @@FETCH_STATUS = 0
		BEGIN

			IF NOT EXISTS (SELECT 1 from tblPalletHst WHERE PalletID = @intPalletID)
			BEGIN
				BEGIN TRY

					SET @vchPalletIDs = Cast(@intPalletID as varchar(20));	
	
						SET @vchText = 'Pallet will be archived to Power Plant System: ' + 
								CASE WHEN @vchPalletIDs = '' THEN 'Nothing.' ELSE @vchPalletIDs END	;
						EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText;	 

						BEGIN TRANSACTION

							INSERT INTO tblPalletHst
									(Facility, PalletID, QtyPerPallet, Quantity, ItemNumber, DefaultPkgLine, Operator, 
									CreationDate, CreationTime, OrderComplete, LotID, ShopOrder,StartTime, ProductionDate,
									-- WO#2563	ExpiryDate, CreationDateTime, ShiftProductionDate, shiftNo, LastUpdate)
									ExpiryDate, CreationDateTime, ShiftProductionDate, shiftNo, LastUpdate
									,OutputLocation								-- WO#2563
									,DestinationShopOrder						-- ALM#11828
									,BatchNumber
									)											-- WO#2563
							VALUES ( @chrFacility, @intPalletID, @intQtyPerPallet, @intQuantity, @vchItemNumber, 
									@chrDefaultPkgLine, @vchOperator, @chrCreationDate, @chrCreationTime, @chrOrderComplete, @vchLotID, 
									@intShopOrder, @dteStartTime, @chrProductionDate, @chrExpiryDate, @dteCreationDateTime, 
									-- WO#2563	@dteShiftProductionDate, @intShiftNo, @dteLastUpdate);
									@dteShiftProductionDate, @intShiftNo, @dteLastUpdate	
									,@vchOutputLocation							-- WO#2563
									,@vchDestinationShopOrder					-- ALM#11828
									,@vchBatchNumber
									);											-- WO#2563

							-- record archived pallet id
							INSERT INTO @tblPalletsArchived (PalletID) VALUES(@intPalletID);

							SET @vchText = 'Pallet has been archived: ' + 
									CASE WHEN @vchPalletIDs = '' THEN 'Nothing.' ELSE @vchPalletIDs END;	
							EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText;	 

							DELETE FROM tblPallet WHERE CURRENT OF csrPallet;
					
						COMMIT TRANSACTION
				END TRY
				BEGIN CATCH
					IF (XACT_STATE()) = -1
						ROLLBACK TRANSACTION 
					PRINT ERROR_MESSAGE();
					SET @vchText = 'Error on posting ' + @vchPalletIDs + ': ' + ERROR_MESSAGE();		
					EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText;
				END CATCH
			END	
			ELSE
				DELETE FROM tblPallet WHERE CURRENT OF csrPallet;

			FETCH NEXT FROM csrPallet INTO @chrFacility, @intPalletID, @intQtyPerPallet, @intQuantity, @vchItemNumber, 
				@chrDefaultPkgLine, @vchOperator, @chrCreationDate, @chrCreationTime, @chrOrderComplete, @vchLotID, 
				@intShopOrder, @dteStartTime, @chrProductionDate, @chrExpiryDate, @dteCreationDateTime, 
				@dteShiftProductionDate, @intShiftNo, @dteLastUpdate
				,@vchOutputLocation										-- WO#2563
				,@vchDestinationShopOrder								-- ALM#11828
				,@vchBatchNumber
		END

		SELECT @intCount = count(*) FROM  @tblPalletsToBeArchived T1 
			left outer join  @tblPalletsArchived T2
			ON T1.PalletID = T2.PalletID
			WHERE T2.PalletID IS NULL;
	
		IF @intCount = 0
			SET @vchText = 'All pallets are archived successfully.';	
		ELSE
			SET @vchText = Cast(@intCount as varchar(10)) + ' Pallets did not archive. $$$$';
		EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText;

		CLOSE csrPallet;
		DEALLOCATE csrPallet;

		SET  @vchText = '*** Archive pallet information Completed.' ;
		EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText;
	END TRY
	BEGIN CATCH
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure nvarchar(200);
		DECLARE @ErrorLine int;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorMessage NVARCHAR(4000);
		
		DECLARE @nvchSubject nvarchar(256);
		DECLARE @nvchBody nvarchar(MAX);

		SELECT 
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ' Message: ' + CHAR(13) + ERROR_MESSAGE();

		BEGIN TRY
			SELECT 
				@nvchSubject = @@ServerName + '.' + DB_NAME() + ' - ' + ISNULL(ERROR_PROCEDURE(),'') + N' fails.',
				@nvchBody = N'Error ' + CAST(@ErrorNumber as varchar(10)) +
							N', Level ' + CAST(@ErrorSeverity as varchar(10)) +
							N', State ' + CAST(@ErrorState as varchar(10)) + 
							N', Procedure ' + @ErrorProcedure +
							N', Line ' + CAST(@ErrorLine as varchar(10)) +
							N', Message:' + CHAR(13) + ERROR_MESSAGE()

			EXEC PPsp_SndMsgToOperator @nvchBody, @nvchSubject;
		END TRY
		BEGIN CATCH
		END CATCH
			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine);
	END CATCH
END

GO

