
-- =============================================
-- Author:		Bong Lee
-- Create date: Sep. 16, 2008
-- Description:	Delete data to Roasting Log
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoastingLog_Del] 
	@chrFacility	char(3),
	@dteDateTest	datetime,
	@vchRoasterNo	varchar(10),
	@vchBlend	varchar(6)
 AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;
		DELETE from tblRoastingLog 
			WHERE Facility = @chrFacility and datetest = @dteDateTest 
				and RoasterNo =  @vchRoasterNo and blend = @vchBlend
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

