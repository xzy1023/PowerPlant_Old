
-- =============================================
-- Author:		Bong Lee
-- Create date: Dec 10, 2006
-- Description:	Upload Pallet to iSeries
-- WO#297:		Bong Lee	Apr 15, 2011
-- Description:	Add column LastUpdate
-- WO#543:		Bong Lee	Sep. 15, 2011
-- Description:	Write log to a text file
-- ITR#2539:	Bong Lee	Jul. 24, 2013
-- Description:	handle 7 digit shop order number
-- WO#871:		Bong Lee	Mar. 16, 2014
-- Description:	Execute stored procedure at the end of process to upload the pallet inforamtion to BPCS for Probat
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_UploadPalletToISeries] 
		@vchISeriesLibrary varchar(10),
		@vchISeriesName varchar(10),
		@vchLogFileName varchar(255)			-- WO#543	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	
	DECLARE @vchISeriesFile varchar(10)
	DECLARE @vchSQLStmt varchar(512)

    DECLARE @chrFacility char(3)
	DECLARE @intPalletID int
	DECLARE @intQtyPerPallet int
	DECLARE @intQuantity int
	DECLARE @vchItemNumber varchar(35)
	DECLARE @chrDefaultPkgLine char(10)
	DECLARE @vchOperator varchar(10)
	DECLARE @chrCreationDate char(8)
	DECLARE @chrCreationTime char(6)
	DECLARE @chrOrderComplete char(1)
	DECLARE @vchLotID  varchar(25)
	DECLARE @intShopOrder int
	DECLARE @dteStartTime datetime
	DECLARE @chrProductionDate char(8)
	DECLARE @chrExpiryDate char(8)
	DECLARE @dteCreationDateTime datetime
	DECLARE @dteShiftProductionDate datetime
	DECLARE @intShiftNo Tinyint
	DECLARE @dteLastUpdate datetime

	-- WO#543 Add Start	
	DECLARE @vchText varchar(255)
	DECLARE @vchPalletIDs varchar(1000)
	DECLARE @intCount int

	DECLARE @tblPalletsToBePosted as TABLE
	(
	  PalletID int
	)

	DECLARE @tblPalletsPosted as TABLE
	(
	  PalletID int
	)
	
	-- Can not write to the same log file, so write to different file.
	SET @vchLogFileName = REPLACE(@vchLogFileName,'.txt','x.txt')

	SET  @vchText = '*** Upload pallet information to iSeries Begin.' 
	EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText
	-- WO#543 Add Stop

	-- Get iSeries library and file names for the Pallet File
	SELECT     @vchISeriesFile = Value2
	FROM         tblControl
	WHERE     ([Key] = N'PalletFileName')	

	-- WO#543 Add Start	
	SET @vchPalletIDs = ''
	
	DECLARE csrPallet CURSOR SCROLL FOR	
		SELECT Facility, PalletID, QtyPerPallet, Quantity, ItemNumber, DefaultPkgLine, Operator,   
				CreationDate, CreationTime, OrderComplete, LotID, ShopOrder, StartTime, 
				ProductionDate, ExpiryDate, CreationDateTime, ShiftProductionDate, ShiftNo, LastUpdate
			FROM tblPallet 
			WHERE PrintStatus = 2 
			ORDER BY RRN

	OPEN csrPallet

	-- read a record from the tblPallet and load the data to the variables
	FETCH NEXT FROM csrPallet INTO  @chrFacility, @intPalletID, @intQtyPerPallet, @intQuantity, @vchItemNumber, 
			@chrDefaultPkgLine, @vchOperator, @chrCreationDate, @chrCreationTime, @chrOrderComplete, @vchLotID, 
			@intShopOrder, @dteStartTime, @chrProductionDate, @chrExpiryDate, @dteCreationDateTime, 
			@dteShiftProductionDate, @intShiftNo, @dteLastUpdate
		
	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT INTO @tblPalletsToBePosted (PalletID) VALUES(@intPalletID)
		SET @vchPalletIDs = @vchPalletIDs + ' ' + Cast(@intPalletID as varchar(20))

		-- read a record from the tblPallet and load the data to the variables
		FETCH NEXT FROM csrPallet INTO  @chrFacility, @intPalletID, @intQtyPerPallet, @intQuantity, @vchItemNumber, 
				@chrDefaultPkgLine, @vchOperator, @chrCreationDate, @chrCreationTime, @chrOrderComplete, @vchLotID, 
				@intShopOrder, @dteStartTime, @chrProductionDate, @chrExpiryDate, @dteCreationDateTime, 
				@dteShiftProductionDate, @intShiftNo, @dteLastUpdate
	END

	SET  @vchText ='Pallets To Be Posted: ' + CASE WHEN @vchPalletIDs = '' THEN 'Nothing.' ELSE LTRIM(@vchPalletIDs) END
	exec PPsp_AppendToTextFile @vchLogFileName, @vchText
	
	-- Reset variable for actual process
	SET @vchPalletIDs = ''
	-- WO#543 Add Stop

	-- Reset file pointer, read a record from beginning of the tblPallet and load the data to the variables
-- WO#543	FETCH NEXT FROM csrPallet INTO  @chrFacility, @intPalletID, @intQtyPerPallet, @intQuantity, @vchItemNumber, 
	FETCH FIRST FROM csrPallet INTO  @chrFacility, @intPalletID, @intQtyPerPallet, @intQuantity, @vchItemNumber,			-- WO#543 
			@chrDefaultPkgLine, @vchOperator, @chrCreationDate, @chrCreationTime, @chrOrderComplete, @vchLotID, 
			@intShopOrder, @dteStartTime, @chrProductionDate, @chrExpiryDate, @dteCreationDateTime, 
			@dteShiftProductionDate, @intShiftNo, @dteLastUpdate
		
	WHILE @@FETCH_STATUS = 0
	BEGIN

		IF NOT EXISTS (SELECT 1 from tblPalletHst WHERE PalletID = @intPalletID)
		BEGIN
			BEGIN TRY

				SET @vchPalletIDs = Cast(@intPalletID as varchar(20))	-- WO#543

				SET @vchSQLStmt = 'INSERT INTO ' + @vchISeriesLibrary + '.' + @vchISeriesFile + 
							  ' (PIFAC, PIPAL, PIQTY, PIPROD, PIMACH, PIEMP, PIPDTE, PIEDTE, PICDTE, PICTIM, PICMP ,PILOT, PISORD)
								values(''' + @chrFacility + ''',' +
								CAST(@intPalletID AS varchar(9)) + ',' +
								CAST(@intQuantity AS varchar(6)) + ',''' +
								@vchItemNumber + ''',''' +
								@chrDefaultPkgLine + ''',''' +
								@vchOperator + ''',' +
								@chrProductionDate + ',' +
								@chrExpiryDate + ',' +
								@chrCreationDate + ',' +
								@chrCreationTime + ',''' +
								@chrOrderComplete + ''',''' +
								@vchLotID + ''',' +
								CAST(@intShopOrder AS varchar(10)) + ')' 
								--CAST(@intShopOrder AS varchar(6)) + ')' 

				IF @vchSQLStmt IS NOT NULL		-- WO#543
				BEGIN							-- WO#543

					IF @vchISeriesName = 'S105HF5M'
						EXEC (@vchSQLStmt) at S105HF5M
					ELSE
						IF @vchISeriesName = 'S10A8379'
							EXEC (@vchSQLStmt) at S10A8379
					
					-- WO#543 Add Start			
					SET @vchText = 'Pallet has been Posted to BPCS: ' + 
							CASE WHEN @vchPalletIDs = '' THEN 'Nothing.' ELSE @vchPalletIDs END	
					EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText	 
					-- WO#543 Add Stop

					BEGIN TRANSACTION

						INSERT INTO tblPalletHst
								(Facility, PalletID, QtyPerPallet, Quantity, ItemNumber, DefaultPkgLine, Operator, 
								CreationDate, CreationTime, OrderComplete, LotID, ShopOrder,StartTime, ProductionDate,
								ExpiryDate, CreationDateTime, ShiftProductionDate, shiftNo, LastUpdate)
						VALUES ( @chrFacility, @intPalletID, @intQtyPerPallet, @intQuantity, @vchItemNumber, 
								@chrDefaultPkgLine, @vchOperator, @chrCreationDate, @chrCreationTime, @chrOrderComplete, @vchLotID, 
								@intShopOrder, @dteStartTime, @chrProductionDate, @chrExpiryDate, @dteCreationDateTime, 
								@dteShiftProductionDate, @intShiftNo, @dteLastUpdate)

						-- WO#543 Add Start			
						-- record posted pallet id
						INSERT INTO @tblPalletsPosted (PalletID) VALUES(@intPalletID)

						SET @vchText = 'Pallet has been Posted to PP: ' + 
								CASE WHEN @vchPalletIDs = '' THEN 'Nothing.' ELSE @vchPalletIDs END	
						EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText	 
						-- WO#543 Add Stop

						DELETE FROM tblPallet WHERE CURRENT OF csrPallet
					
					COMMIT TRANSACTION
				END		-- WO#543

			END TRY
				BEGIN CATCH
					IF (XACT_STATE()) = -1
						ROLLBACK TRANSACTION 
					PRINT ERROR_MESSAGE()
					SET @vchText = 'Error on posting ' + @vchPalletIDs + ': ' + ERROR_MESSAGE()		-- WO#543
					EXEC PPsp_AppendToTextFile @vchLogFileName,  @vchText								-- WO#543
				END CATCH
		END	
		ELSE
			DELETE FROM tblPallet WHERE CURRENT OF csrPallet

		FETCH NEXT FROM csrPallet INTO @chrFacility, @intPalletID, @intQtyPerPallet, @intQuantity, @vchItemNumber, 
			@chrDefaultPkgLine, @vchOperator, @chrCreationDate, @chrCreationTime, @chrOrderComplete, @vchLotID, 
			@intShopOrder, @dteStartTime, @chrProductionDate, @chrExpiryDate, @dteCreationDateTime, 
			@dteShiftProductionDate, @intShiftNo, @dteLastUpdate
	END

	SELECT @intCount = count(*) FROM  @tblPalletsToBePosted T1 
		left outer join  @tblPalletsPosted T2
		ON T1.PalletID = T2.PalletID
		WHERE T2.PalletID IS NULL
	
	-- WO#543 Add Start
	IF @intCount = 0
		SET @vchText = 'All pallets posted successfully.'	
	ELSE
		SET @vchText = Cast(@intCount as varchar(10)) + ' Pallets did not post. $$$$'
	EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText 
	-- WO#543 Add Stop

	CLOSE csrPallet
	DEALLOCATE csrPallet

	-- WO#871 Add Start
	SET  @vchText = '*** Upload pallet information for Probat to iSeries Begin.' 
	EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText

	Execute PPsp_UploadProbatPalletToISeries @vchISeriesLibrary, @vchISeriesName, @vchLogFileName

	SET  @vchText = '*** Upload pallet information for Probat to iSeries Complete.' 
	EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText

	-- WO#871 Add Stop

	SET  @vchText = '*** Upload pallet information to iSeries Completed.' 
	EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText

END

GO

