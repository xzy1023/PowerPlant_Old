

-- =============================================
-- Author:		Bong Lee
-- Create date: Mar. 28, 2012
-- Description:	Add data to PalletDeletion table
-- WO#2563	  : Sep 25, 2015	Bong Lee
-- Description:	Add output location to tblPalletDeletion table
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PalletDeletion_Add] 
	@intPalletID	int
	,@dteDeletionTime	datetime
	,@vchDeletedBy	varchar(10)
	
 AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;
		INSERT INTO tblPalletDeletion
		-- WO#2563	SELECT *, @dteDeletionTime, @vchDeletedBy 
		SELECT @dteDeletionTime, @vchDeletedBy, * -- WO#2563
			FROM tblPallet
			WHERE PalletID = @intPalletID

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

