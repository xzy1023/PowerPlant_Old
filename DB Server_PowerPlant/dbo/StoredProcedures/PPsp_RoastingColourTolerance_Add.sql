
-- =============================================
-- Author:		Bong Lee
-- Create date: Oct. 22 2008
-- Description:	Add New Roasting Colour Tolerance Specification
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoastingColourTolerance_Add]
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
		INSERT INTO tblColourTolerance
            (Facility, RoasterNo, Blend, Tolerance)
			 VALUES (@chrFacility, @vchRoasterNo, @vchBlend,@decTolerance);
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

