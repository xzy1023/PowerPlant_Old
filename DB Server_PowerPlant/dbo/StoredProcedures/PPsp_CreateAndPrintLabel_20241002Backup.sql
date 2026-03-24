


-- =============================================
-- Author:		Bong Lee
-- Create date: Oct. 04, 2006
-- Description:	Create And Print Label
-- Mod#01		Jul.13,2007		Bong Lee
-- Description: Add a new input parameter for Shift
--				Handle clear buffer for native printer driver
-- WO#21        Mar. 23, 2010   Bong Lee
-- Description: Print Case Label with Shop Order Number
--				Add a column DefaultPkgLine in tblDynamicData
-- WO#650       Mar. 19, 2012   Bong Lee
-- Description: Add an optional input parameter, Expiry Date, for PPsp_CreateLabelData 
--				to allow override.
-- WO#512       Mar. 26, 2012   Bong Lee
-- Description: Track who printed the UPI label. Add an optional input parameter, Requestor, 
--				for PPsp_CreatePrintRequest
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_CreateAndPrintLabel_20241002Backup] 

	@chrLabelType char(1),
	@chrFacility char(3),
	@chrDftPkgLine char(10) = NULL, 
	@chrOvrPkgLine char(10) = NULL,		-- WO#21
	@intShopOrder int = 0,
	@vchItemNo varchar(35) = NULL,
	@intQuantity int = 0,
	@vchOperator varchar(10) = NULL,
	@chrDeviceType char(1) = NULL,
	@dteStartTime datetime = NULL,
	@vchJobName varchar(50) = NULL,
	@bitSbmFromPalletStation bit = 0,
	@vchLotID varchar(25) = NULL,
	@vchProductionDate varchar(10) = NULL,
	@intPalletID int = 0,
	@intCopies int = 1,
	@intShift Tinyint = 1	--Mod#01
	,@vchExpiryDate varchar(10) = NULL	--WO#650
	,@vchRequestor varchar(10) = NULL	--WO#512

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	DECLARE @chrPkgLineSubmit varchar(10);

	SET NOCOUNT ON;
	-- SET XACT_ABORT ON

	BEGIN TRY
	-- BEGIN TRANSACTION   
	-- Create label data to data source
			EXEC PPsp_CreateLabelData 
				@chrLabelType, 
				@chrFacility,
				@chrDftPkgLine, 
				@chrOvrPkgLine,			-- WO#21 
				@intShopOrder,
				@vchItemNo,
				@intPalletID,
				@intQuantity,
				@vchOperator,
				@vchjobName, 
				@vchLotID,
				@vchProductionDate,
				@intShift				-- Mod#01
				,@vchExpiryDate			--WO#650

			EXEC PPsp_CreatePrintRequest
				@chrLabelType,
				@chrFacility,
				@chrDftPkgLine, 
				@chrDeviceType,
				@dteStartTime,
				@vchJobName,
				@bitSbmFromPalletStation,	-- Mod#01
				@intCopies
				,@vchRequestor				--WO#512
				

	--	COMMIT TRANSACTION		
	END TRY
	BEGIN CATCH
	--	ROLLBACK TRANSACTION 
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

