


-- =============================================
-- Author:		Bong Lee
-- Create date: Sept. 05, 2006
-- Description:	Create Label Data 
-- Mod.			Date			Author
-- #0001		Jul.13 2007		Bong Lee
-- Description: Update the production date and its related field every time creating 
--				case label to provide actual production date on labels.
--				Add a new input parameter to facilitate date code 36 which requires
--				the shift no.
-- WO#21        Mar. 23, 2010   Bong Lee
-- Description: Add a column DefaultPkgLine in tblDynamicData to identify the records
--				that were created from the default packaging line for clearing.
-- WO#650       Mar. 19, 2012   Bong Lee
-- Description: Add an optional input parameter Expiry Date. If the parameter is not null, 
--				it will be used instead of using calculated one.
-- WO#1096      Jun. 27, 2014   Bong Lee
-- Description: The ProductionDateDesc and ExpiryDateDesc were changed to varchar(30) from varchar(10)
--				in tblItemMaster, so the length of related columns have to be adjusted accordingly
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_CreateLabelData_20241125] 
	-- Add the parameters for the stored procedure here
	@chrLabelType char(1),
	@chrFacility char(3),
	@chrDefaultPkgLine char(10) = NULL,		-- WO#21
	@chrPkgLine char(10) = NULL, 
	@intShopOrder int = 0,
	@vchItemNo varchar(35) = NULL,
	@intPalletID int = 0,
	@intQuantity int = 0,
	@vchOperator varchar(10) = NULL,
	@vchJobName varchar(50) = NULL,
	@vchLotID varchar(25) = NULL,
	@vchOvrdProductionDate varchar(10) = NULL,
	@intShift tinyint = 0		-- ADD#0001
	,@vchExpiryDate varchar(10) = NULL	--WO#650
AS
BEGIN
	DECLARE @vchLabelKey AS varchar(50);
	--DECLARE @dteNow AS datetime; 
	DECLARE @dteProductionDate as datetime;
	DECLARE @dteExpiryDate as datetime;
	DECLARE @chrLblFmtCode as char(2);
	DECLARE @intShelfLife as int;
--WO#1096	DECLARE @vchProductionDateDesc as varchar(10);
--WO#1096	DECLARE @vchExpiryDateDesc as varchar(10);
	DECLARE @vchProductionDateDesc as varchar(30);			--WO#1096
	DECLARE @vchExpiryDateDesc as varchar(30);				--WO#1096
	DECLARE @chrDateToPrintFlag as char(1);
--WO#1096	DECLARE @vchPreFmtProductionDate as varchar(30);
--WO#1096	DECLARE @vchPreFmtExpiryDate as varchar(30);
	DECLARE @vchPreFmtProductionDate as varchar(50);		--WO#1096
	DECLARE @vchPreFmtExpiryDate as varchar(50);			--WO#1096
	
	-- SET NOCOUNT ON added to prevent extra result sets from

	SET NOCOUNT ON;
	BEGIN TRY

		-- Calculate the production date if it is not passed as parameter
		-- If the current time is greater than or equal to the from time of the shift no 1 (i.e. morning shift)
		--   Production Date = date part of the current time
		-- Else Production Date = date part of the current time - 1 (i.e. the production date of night shift still belongs to the same date of the morning shift)  

		--SET @dteNow = getdate();	-- format to mm/dd/yyyy
		--IF @vchOvrdProductionDate = '' OR @vchOvrdProductionDate IS NULL
		--	SET @dteProductionDate = (SELECT CASE WHEN @dteNow > tblShift.FromTime
		--						 THEN @dteNow ELSE DATEADD(day,-1,@dteNow) END
		--						 FROM tblShift
		--						 WHERE (Facility = @chrFacility) AND (WorkGroup = 'P') AND (Shift = 1))
		--ELSE
			SET @dteProductionDate = CONVERT(datetime,@vchOvrdProductionDate, 101);  -- format to mm/dd/yyyy Datetime data type

		-- Get information from item master
		SELECT @chrLblFmtCode = LabelDateFmtCode, @intShelfLife = ProductionShelfLifeDays, 
			   @vchProductionDateDesc = ProductionDateDesc, @vchExpiryDateDesc = ExpiryDateDesc,
			   @chrDateToPrintFlag = DateToPrintFlag
			FROM dbo.tblItemMaster WHERE facility = @chrFacility AND ItemNumber = @vchItemNo;

		IF @vchProductionDateDesc = NULL
			SET @vchProductionDateDesc = ' ' + @vchProductionDateDesc
		IF @vchExpiryDateDesc = NULL
			SET @vchExpiryDateDesc = ' ' + @vchExpiryDateDesc

		-- '0' = None; '1'= Print Expiry Date; '2'= Print Production Date; '3'= Print Expiry & Prodcution Date		
		IF @chrDateToPrintFlag = '3'
			BEGIN
				SET @vchPreFmtProductionDate = @vchProductionDateDesc + ' ' + dbo.fnConvertDate(@dteProductionDate, @chrLblFmtCode, @chrFacility, @intShift)
				IF @vchExpiryDate is NULL or @vchExpiryDate = ''	--WO#650
					SET @vchPreFmtExpiryDate = @vchExpiryDateDesc + ' ' + dbo.fnConvertDate(DATEADD(day,@intShelfLife,@dteProductionDate), @chrLblFmtCode, @chrFacility, @intShift)
				ELSE	--WO#650
					SET @vchPreFmtExpiryDate = @vchExpiryDateDesc + ' ' + dbo.fnConvertDate(CONVERT(datetime,@vchExpiryDate, 101), @chrLblFmtCode, @chrFacility, @intShift) --WO#650
			END
		ELSE
			IF @chrDateToPrintFlag = '2'
				BEGIN
					SET @vchPreFmtProductionDate = @vchProductionDateDesc + ' ' + dbo.fnConvertDate(@dteProductionDate, @chrLblFmtCode, @chrFacility, @intShift)
					SET @vchPreFmtExpiryDate = NULL
				END
			ELSE
				IF @chrDateToPrintFlag = '1'
					BEGIN
						SET @vchPreFmtProductionDate = NULL
						IF @vchExpiryDate is NULL or @vchExpiryDate = '' --WO#650
							SET @vchPreFmtExpiryDate = @vchExpiryDateDesc + ' ' + dbo.fnConvertDate(DATEADD(day,@intShelfLife,@dteProductionDate), @chrLblFmtCode, @chrFacility, @intShift)
						ELSE	--WO#650
							SET @vchPreFmtExpiryDate = @vchExpiryDateDesc + ' ' + dbo.fnConvertDate(CONVERT(datetime,@vchExpiryDate, 101), @chrLblFmtCode, @chrFacility, @intShift) --WO#650
					END
				ELSE
					IF @chrDateToPrintFlag = '0'
						BEGIN
							SET @vchPreFmtProductionDate = NULL
							SET @vchPreFmtExpiryDate = NULL
						END
		SET @vchPreFmtProductionDate = LTRIM(@vchPreFmtProductionDate)
		SET @vchPreFmtExpiryDate = LTRIM(@vchPreFmtExpiryDate)
	-- Case Label
		IF @chrLabelType <> 'P'
			BEGIN
			-- If label record is not already existed, create one
			--	IF @chrLabelType = 'C'
			--		SET @vchLabelKey = @chrPkgLine + CAST(@intShopOrder AS VARCHAR(20))	
			--	ELSE
					SET @vchLabelKey = @vchJobName

				IF NOT EXISTS (Select 1 from tblDynamicLabelData where LabelKey = @vchLabelKey)
				INSERT INTO tblDynamicLabelData
					(Facility, RecordType, LabelKey, PackagingLine, ShopOrder, ItemNumber, MPProductionDate,
					 ProductionDate, PreFmtProductionDate, PreFmtExpiryDate, Operator, LotID, DefaultPkgLine)
					VALUES (@chrFacility, @chrLabelType, @vchLabelKey, @chrPkgLine, @intShopOrder, @vchItemNo, dbo.fnConvertDate(@dteProductionDate, '01', @chrFacility, @intShift),
							@dteProductionDate, @vchPreFmtProductionDate, @vchPreFmtExpiryDate, @vchOperator, @vchLotID, @chrDefaultPkgLine)
				ELSE
-- Add#0001 begin
				  UPDATE tblDynamicLabelData 
					SET MPProductionDate = dbo.fnConvertDate(@dteProductionDate, '01', @chrFacility, @intShift),
						ProductionDate = @dteProductionDate, PreFmtProductionDate = @vchPreFmtProductionDate, PreFmtExpiryDate = @vchPreFmtExpiryDate,
						Operator = @vchOperator, LastUpdated = getdate()  --WO#650
--WO#650				@vchOperator = @vchOperator 
					WHERE LabelKey = @vchLabelKey
-- Add#0001 End
			END
		ELSE
			BEGIN
			-- Pallet Label
				SET @vchLabelKey = @chrPkgLine + CAST(@intPalletID AS varChar(25));

				--SET @chrProductionDate = (SELECT  CASE WHEN CONVERT(char(8),@dteNow,14) > CONVERT(char(8),FromTime,14) 
				--							THEN SUBSTRING(CONVERT(char(6),@dteNow,12),2,5)
				--							ELSE SUBSTRING(CONVERT(char(6),DATEADD(day,-1,@dteNow),12),2,5) END
				IF NOT EXISTS (Select 1 from tblDynamicLabelData where LabelKey = @vchLabelKey)
				INSERT INTO tblDynamicLabelData
					(Facility, RecordType, LabelKey, PackagingLine, ShopOrder, ItemNumber, PalletID, Quantity,
					  MPProductionDate, ProductionDate, PreFmtProductionDate,  Operator, LotID, DefaultPkgLine)
					VALUES (@chrFacility, @chrLabelType, @vchLabelKey, @chrPkgLine, @intShopOrder, @vchItemNo, @intPalletID, @intQuantity,
							dbo.fnConvertDate(@dteProductionDate, '01', @chrFacility, @intShift),@dteProductionDate, dbo.fnConvertDate(@dteProductionDate, @chrLblFmtCode, @chrFacility, @intShift), @vchOperator, @vchLotID, @chrDefaultPkgLine)	
			END
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
	END CATCH
END

GO

