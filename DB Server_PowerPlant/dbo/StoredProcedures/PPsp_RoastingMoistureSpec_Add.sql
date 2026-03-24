
-- =============================================
-- Author:		Bong Lee
-- Create date: Oct. 22 2008
-- Description:	Add New Roasting Moisture Specification
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoastingMoistureSpec_Add]
(
	@dteEffectiveDate datetime,
	@vchBlend varchar(6),
	@decMinMoisture decimal(3, 2),
	@decTargetMoisture decimal(3, 2),
	@decMaxMoisture decimal(3, 2),
	@vchCreatedBy varchar(50)
)
AS
BEGIN

	SET NOCOUNT OFF;
	BEGIN TRY
		INSERT INTO tblMoistureSpec
			([EffectiveDate], [Blend], [MinMoisture], [TargetMoisture], [MaxMoisture], [CreatedBy])
			 VALUES (@dteEffectiveDate, @vchBlend, @decMinMoisture, @decTargetMoisture, @decMaxMoisture, @vchCreatedBy);
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

