

-- =============================================
-- Author:		Bong Lee
-- Create date: Sep. 17 2008
-- Description:	Select Roasting Colour specification
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoastingColourSpec_Sel]
	@vchAction varChar(50),
	@vchBlend varchar(6)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		IF @vchAction = 'LatestByBlend'
			SELECT * FROM dbo.vwLatestRoastColourSpec WHERE blend = @vchBlend
		Else
			IF @vchAction = 'ByBlend'
				SELECT * FROM tblRoastColourSpec WHERE blend = @vchBlend
				ORDER BY EffectiveDate DESC
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

