-- =============================================
-- Author:		Bong Lee
-- Create date: Sep. 30 2008
-- Description:	Add New Roasting Final Temperature Specification
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoastingFinalTempSpec_Add]
(
	@chrFacility char(3),
	@dteEffectiveDate datetime,
	@vchRoasterNo varchar(10),
	@vchBlend varchar(6),
	@decSpecFinalTemp decimal(5, 1),
	@vchCreatedBy varchar(50)
)
AS
BEGIN

	SET NOCOUNT OFF;
	BEGIN TRY
		INSERT INTO [tblFinalTempSpec] 
			([Facility], [EffectiveDate], [RoasterNo], [Blend], [SpecFinalTemp], [CreatedBy])
			 VALUES (@chrFacility, @dteEffectiveDate, @vchRoasterNo, @vchBlend, @decSpecFinalTemp, @vchCreatedBy);
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

