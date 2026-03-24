



-- =============================================
-- Author:		Bong Lee
-- Create date: Jun. 29 ,2010
-- Description:	Select Pallet Adjustment
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PalletAdjustment_Sel]
	@vchAction varchar(50) = NULL, 
	@vchFacility varchar(3) = NULL,
	@intShopOrder int = NULL,
	@vchPkgLine varchar(10)= NULL,
	@vchOperator varchar(50) = NULL,
	@intPalletID int = NULL,
	@dteFromTransactionDate datetime = NULL,
	@dteToTransactiondate datetime = NULL,
	@vchTransactionReasonCode varchar(2) = NULL

AS
BEGIN

	SET NOCOUNT ON;
	BEGIN TRY

--	BEGIN

	Select * from tfnPalletAdjustment (
		@vchAction 
		,@vchFacility
		,@intShopOrder
		,@vchPkgLine
		,@vchOperator
		,@intPalletID
		,@dteFromTransactionDate
		,@dteToTransactiondate
		,@vchTransactionReasonCode
	)
--		IF @vchAction = 'WithCorrection'
--		BEGIN
--			SELECT tPA.*, ISNULL(ISNULL(tPAC.TransactionReasonCode,tPAC2.TransactionReasonCode),tPA.TransactionReasonCode) as LatestReasonCode, tE.Description as Machine,
--				tPS.FirstName + ' ' + tPS.LastName as OperatorName, 'N' as Posted 
--			FROM dbo.tblPalletAdjustment tPA
--			LEFT OUTER JOIN tblPalletAdjCorrection tPAC
--				ON tPA.RRN = tPAC.AdjustmentRRN
--			LEFT OUTER JOIN tblPalletAdjCorrection tPAC2
--				ON tPA.PalletID = tPAC.PalletID
--			LEFT OUTER JOIN tblPlantStaff tPS
--				On tPA.Facility = tPS.Facility and tPA.Operator = tPS.StaffID	
--			LEFT OUTER JOIN tblEquipment tE
--				On tPA.Facility = tE.facility AND tPA.MachineID = tE.EquipmentID
--			WHERE (@vchFacility is NULL OR tPA.Facility = @vchFacility )
--				AND (@intShopOrder is NULL OR tPA.ShopOrder = @intShopOrder)
--				AND (@vchPkgLine is NULL OR tPA.MachineID = @vchPkgLine )
--				AND (@vchOperator is NULL OR tPA.Operator = @vchOperator)
--				AND (@intPalletID is NULL OR tPA.PalletID = @intPalletID)
--				AND (@dteFromTransactiondate is NULL OR @dteToTransactiondate is NULL OR tPA.Transactiondate Between @dteFromTransactiondate and @dteToTransactiondate)
--				AND (@vchTransactionReasonCode is NULL OR ISNULL(ISNULL(tPAC.TransactionReasonCode,tPAC2.TransactionReasonCode),tPA.TransactionReasonCode) = @vchTransactionReasonCode)
--		END
--		ELSE
--		IF @vchAction = 'WithoutCorrection'
--		BEGIN
--			SELECT tPA.*, tE.Description as Machine,
--				tPS.FirstName + ' ' + tPS.LastName as OperatorName, 'N' as Posted 
--			FROM dbo.tblPalletAdjustment tPA
--			LEFT OUTER JOIN tblPlantStaff tPS
--				On tPA.Facility = tPS.Facility and tPA.Operator = tPS.StaffID	
--			LEFT OUTER JOIN tblEquipment tE
--				On tPA.Facility = tE.facility AND tPA.MachineID = tE.EquipmentID
--			WHERE (@vchFacility is NULL OR tPA.Facility = @vchFacility )
--				AND (@intShopOrder is NULL OR tPA.ShopOrder = @intShopOrder)
--				AND (@vchPkgLine is NULL OR tPA.MachineID = @vchPkgLine )
--				AND (@vchOperator is NULL OR tPA.Operator = @vchOperator)
--				AND (@intPalletID is NULL OR tPA.PalletID = @intPalletID)
--				AND (@dteFromTransactiondate is NULL OR @dteToTransactiondate is NULL OR tPA.Transactiondate Between @dteFromTransactiondate and @dteToTransactiondate)
--				AND (@vchTransactionReasonCode is NULL OR tPA.TransactionReasonCode = @vchTransactionReasonCode)
--		END
--	
		
--	END 
		
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

