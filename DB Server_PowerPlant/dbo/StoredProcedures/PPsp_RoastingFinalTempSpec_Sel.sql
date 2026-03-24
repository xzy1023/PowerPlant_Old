

-- =============================================
-- Author:		Bong Lee
-- Create date: Sep. 30 2008
-- Description:	Select Latest Roasting Final Temperature Specification
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoastingFinalTempSpec_Sel]
	@vchAction varChar(50),
	@chrFacility char(3),
	@vchRoasterNo varchar(6),
	@vchBlend varchar(6)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		IF @vchAction = 'ByRoasterNoBlend'
			select * from dbo.tblFinalTempSpec where Facility = @chrFacility AND RoasterNo =@vchRoasterNo AND blend = @vchBlend
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

