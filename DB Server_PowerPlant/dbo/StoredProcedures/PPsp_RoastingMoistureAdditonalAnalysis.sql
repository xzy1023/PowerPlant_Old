
-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 20, 2008
-- Description:	Roasting Colour Moisture Analysis
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoastingMoistureAdditonalAnalysis] 

	@chrFacility char(3), 
	@dteFromDate datetime,
	@dteToDate datetime,
	@vchRoaster varchar(10) = NULL,
	@vchBlend varchar(6) = NULL

AS
BEGIN
	
	DECLARE @vchToDate as varchar(10)
	DECLARE @LastestTargetMoisture as decimal(4,2)
	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);
	BEGIN TRY
	SET  @dteToDate = Convert(datetime, Convert(varchar(10),@dteToDate, 101) + ' 23:59:59', 120)
	
	SELECT top 1 @LastestTargetMoisture = TargetMoisture
	FROM tblroastinglog 
	WHERE DateTest Between @dteFromDate And @dteToDate
		AND (@vchRoaster is NULL OR RoasterNo = @vchRoaster)
		AND (@vchBlend is NULL OR Blend = @vchBlend)
		AND Moisture > 1
	ORDER BY DateTest desc

	SELECT Count(Moisture) AS CountOfMoisture, 
		ROUND(Avg(Moisture),2) AS AvgOfMoisture, 
        @LastestTargetMoisture AS LastOfTargetMoisture, 
		ROUND(StDev(Moisture),2) AS StDevOfMoisture, 
        Max(Moisture) AS MaxOfMoisture, 
		Min(Moisture) AS MinOfMoisture
        FROM tblroastinglog 
		WHERE DateTest Between @dteFromDate And @dteToDate
			AND (@vchRoaster is NULL OR RoasterNo = @vchRoaster)
			AND (@vchBlend is NULL OR Blend = @vchBlend)
			AND Moisture > 1
	END TRY

	BEGIN CATCH

		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorNumber = ERROR_NUMBER(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');

		RAISERROR 
			(
			@ErrorMessage, 
			@ErrorSeverity, 
			@ErrorState,     -- parameter: original error state.               
			@ErrorNumber,    -- parameter: original error number.
			@ErrorSeverity,  -- parameter: original error severity.
			@ErrorProcedure, -- parameter: original error procedure name.
			@ErrorLine       -- parameter: original error line number.
			);
	END CATCH
END

GO

