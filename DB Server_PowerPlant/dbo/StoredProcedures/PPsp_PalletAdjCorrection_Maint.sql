

-- =============================================
-- Author:		Bong Lee
-- Create date: Jul. 06 ,2011
-- Description:	Pallet Adjustment Correction Table Maintenance
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PalletAdjCorrection_Maint]
	@vchAction varchar(50) = NULL, 
	@chrFacility char(3),
	@intPalletID int,
	@dteTransactionDate datetime,
	@vchTransactionReasonCode varchar(2),
	@dteCreationDate datetime,
	@vchCreatedBy varchar(50),
	@vchResult varchar(10) OUTPUT,
	@LatestReasonCode varchar(2)

AS
BEGIN
	SET XACT_ABORT ON

	SET NOCOUNT ON;
	
	BEGIN TRY
		
		BEGIN TRANSACTION 
		SET @vchResult = ''

		IF EXISTS (SELECT * FROM tblPalletAdjCorrection WHERE PalletID = @intPalletID AND TransactionDate = @dteTransactionDate)
		BEGIN
			UPDATE tblPalletAdjCorrection SET TransactionReasonCode = @vchTransactionReasonCode 
			OUTPUT 'Update', @vchCreatedBy, Getdate(), Deleted.*
			INTO tblPalletAdjCorrectionAudit
			WHERE PalletID = @intPalletID AND TransactionDate = @dteTransactionDate
		END
		ELSE
		BEGIN
			INSERT INTO tblPalletAdjCorrection
                (Facility, PalletID, TransactionDate, TransactionReasonCode, CreationDate, CreatedBy)
			OUTPUT 'Insert', @vchCreatedBy, Getdate(), Inserted.*
			INTO tblPalletAdjCorrectionAudit
			VALUES (@chrFacility, @intPalletID, @dteTransactionDate, @vchTransactionReasonCode, @dteCreationDate, 
				RIGHT(@vchCreatedBy, LEN(@vchCreatedBy) - CHARINDEX('\',@vchCreatedBy)))
		END 

	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		SET @vchResult = 'Failure'
		ROLLBACK TRANSACTION 

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

