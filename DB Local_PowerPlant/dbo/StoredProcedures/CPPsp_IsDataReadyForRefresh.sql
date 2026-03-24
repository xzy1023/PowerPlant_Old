

-- =============================================================================
-- Author:		Bong Lee
-- Create date: Mar. 09, 2012
-- Description:	Is ready to refresh data in the local DB from the Staging DB
-- ==============================================================================
CREATE PROCEDURE [dbo].[CPPsp_IsDataReadyForRefresh] 
	@vchComputerName varchar(50) = NULL
	,@bitReady bit = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRY
		SELECT @bitReady = ReadyForDownLoad 
			FROM importdata.dbo.tblComputerConfig 
			WHERE ComputerName = @vchComputerName
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

