

-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 19 2008
-- Description:	Update Roasting Colour Tolerance Specification
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoastingColourTolerance_Upd]
(
	@chrFacility char(3),
	@vchRoasterNo varchar(50),
	@vchBlend varchar(6),
	@decTolerance decimal(4, 2)
)
AS
BEGIN

	SET NOCOUNT OFF;
	BEGIN TRY
		Update tblColourTolerance SET Tolerance = @decTolerance
            WHERE Facility = @chrFacility AND RoasterNo = @vchRoasterNo AND Blend = @vchBlend
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

