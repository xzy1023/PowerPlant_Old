
-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 23,2008
-- Description:	Get Latest Roasting Specifications
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoastingSpecs_Sel] 
	-- Add the parameters for the stored procedure here
	@vchAction varchar(50), 
	@chrFacility char(3) = NULL,
	@vchBlend varchar(10),
	@vchRoaster varchar(10) = NULL, 
	@bitAcqColourSpec bit,
	@bitAcqMoistureSpec bit,
	@bitAcqFinalTempSpec bit	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ColourSpecMax as decimal(5, 2)
	DECLARE @ColourSpecTarget as decimal(5, 2)
	DECLARE @ColourSpecMin as decimal(5, 2)
	DECLARE @ColourTolerance as decimal(4, 2)
	DECLARE @MoistureSpecMax as decimal(4, 2)
	DECLARE @MoistureSpecTarget as decimal(4, 2)
	DECLARE @MoistureSpecMin as decimal(4, 2)
	DECLARE @FinalTempSpec as decimal(6, 2)
    -- Insert statements for procedure here
	BEGIN TRY

	IF @vchAction = 'LatestSpec'
	BEGIN
		IF @bitAcqColourSpec = '1'
		BEGIN
			SELECT @ColourSpecMax = SpecMax, @ColourSpecTarget= SpecTarg ,@ColourSpecMin = SpecMin 
				FROM vwLatestRoastColourSpec
				WHERE Blend = @vchBlend
			SELECT @ColourTolerance = Tolerance from tblColourTolerance 
				WHERE Facility = @chrFacility AND RoasterNo = @vchRoaster AND Blend = @vchBlend
		END

		IF @bitAcqMoistureSpec = '1'
			SELECT @MoistureSpecMax = MaxMoisture, @MoistureSpecTarget= TargetMoisture ,@MoistureSpecMin = MinMoisture
				FROM vwLatestMoistureSpec
				WHERE Blend = @vchBlend

		IF @bitAcqFinalTempSpec = '1'
			SELECT @FinalTempSpec = SpecFinalTemp
				FROM vwLatestFinalTempSpec
				WHERE Facility = @chrFacility AND RoasterNo = @vchRoaster AND Blend = @vchBlend

		Select ISNULL(@ColourSpecMax,0) as ColourSpecMax, ISNULL(@ColourSpecTarget,0) as ColourSpecTarget ,ISNULL(@ColourSpecMin,0) as ColourSpecMin,
			   ISNULL(@ColourTolerance,0) as ColourTolerance,
			   ISNULL(@MoistureSpecMax,0) as MoistureSpecMax, ISNULL(@MoistureSpecTarget,0) as MoistureSpecTarget ,ISNULL(@MoistureSpecMin,0) as MoistureSpecMin,
			   ISNULL(@FinalTempSpec,0) as FinalTempSpec
	END 

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

