


-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 19, 2008
-- Description:	Top Customer Compliance
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_TopCustomersRoastingCompliance_Summary] 
	@dteFromDate datetime,
	@dteToDate datetime
AS
BEGIN

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

	--Total no. of readings and avg. colour variance
	SELECT tTC.Customer,
		CAST(ROUND(SUM(Case When tRL.Colour <= [specMax] And tRL.Colour >= [specMin] Then 1 Else 0 End) * 100.00 / Count(*),2) As Decimal(5,2)) As [%ComplianceCount], 
		CAST(ROUND(SUM(Case When tRL.Colour > [specMax] Then 1 Else 0 End) * 100.00 / Count(*),2) As Decimal(5,2)) As [%LightCount],
		CAST(ROUND(SUM(Case When tRL.Colour < [specMin] Then 1 Else 0 End) * 100.00 / Count(*),2) As Decimal(5,2)) As [%DarkCount],
		CAST(ROUND(Avg(([Colour]-[SpecTarget])),2) As Decimal(4,2)) AS avgVar,
		SUM(Case When tRL.rejected= 'Y' Then 1 Else 0 End) As RejectedCount,
		Count(*) AS Readings 
		FROM tblTopCustomers tTC
		INNER JOIN tblRoastingLog tRL ON tTC.Blend = tRL.Blend 
		WHERE tRL.DateTest BETWEEN @dtefromDate And @dteToDate
		GROUP BY tTC.Customer

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

