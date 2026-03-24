


-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 20, 2008
-- Description:	Roasting Colour Moisture Analysis
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoastingColourMoistureAnalysis] 
	-- Add the parameters for the stored procedure here
	@vchAction varchar(50),	
	@chrFacility char(3), 
	@dteFromDate datetime,
	@dteToDate datetime,
	@vchRoaster varchar(10) = NULL,
	@vchBlend varchar(6) = NULL

AS
BEGIN
	
	DECLARE @vchToDate as varchar(10)
	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);
	BEGIN TRY

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	SET  @dteToDate = Convert(datetime, Convert(varchar(10),@dteToDate, 101) + ' 23:59:59', 120)

	IF @vchAction = 'COLOUR'
		SELECT CONVERT(varchar(5),DateTest, 01) as [DateTest],
			Round(Avg(tblRoastingLog.Colour),2) AS [DataAvg],
			Round(Avg(tblroastinglog.SpecTarget),2) AS [Spec Target], 
			Round(Avg(tblroastinglog.SpecMin),2) AS [Spec Min], 
			Round(Avg(tblroastinglog.SpecMax),2) AS [Spec Max]
		FROM tblroastinglog
		WHERE DateTest Between @dteFromDate And @dteToDate
			AND (@vchRoaster is NULL OR RoasterNo = @vchRoaster)
			AND (@vchBlend is NULL OR Blend = @vchBlend)
		GROUP BY CONVERT(varchar(5),DateTest, 01)
		ORDER BY CONVERT(varchar(5),DateTest, 01)
	ELSE
		SELECT CONVERT(varchar(5),DateTest, 01) as [DateTest],
			Round(Avg(tblRoastingLog.Moisture),2) AS [DataAvg],
			Round(Avg(tblRoastingLog.targetMoisture),2) AS [Spec Target], 
			Round(Avg(tblRoastingLog.MinMoisture),2) AS [Spec Min], 
			Round(Avg(tblRoastingLog.MaxMoisture),2) AS [Spec Max] 
		FROM tblroastinglog
		WHERE DateTest Between @dteFromDate And @dteToDate
			AND (@vchRoaster is NULL OR RoasterNo = @vchRoaster)
			AND (@vchBlend is NULL OR Blend = @vchBlend)
			AND tblroastinglog.Moisture > 1
		GROUP BY CONVERT(varchar(5),DateTest, 01)
		ORDER BY CONVERT(varchar(5),DateTest, 01)
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

