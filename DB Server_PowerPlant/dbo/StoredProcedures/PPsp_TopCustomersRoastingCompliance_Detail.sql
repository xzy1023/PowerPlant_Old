



-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 19, 2008
-- Description:	Top Customer Compliance
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_TopCustomersRoastingCompliance_Detail] 
	@dteFromDate datetime,
	@dteToDate datetime,
	@vchCustomer varchar(50)
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
print @vchCustomer
	--Total no. of readings and avg. colour variance
	SELECT tTC.Customer, tTC.Blend,
		CAST(ROUND(SUM(Case When tRL.Colour <= [specMax] And tRL.Colour >= [specMin] Then 1 Else 0 End) * 100.00 / Count(*),2) As Decimal(5,2)) As [%ComplianceCount], 
		CAST(ROUND(AVG(tRL.Colour),2) AS Decimal(5,2)) AS AvgColour,
		CAST(ROUND(AVG(tRL.SpecTarget),2)AS Decimal(5,2)) AS AvgSpecTarget,
		CAST(ROUND(AVG((tRL.Colour-[SpecTarget])),2) As Decimal(4,2)) AS avgVar,
		CAST(ROUND(AVG(tRL.Moisture),2) AS Decimal(5,2)) AS AvgMoisture,
		CAST(ROUND(AVG(tRL.TargetMoisture),2)AS Decimal(5,2)) AS AvgTargetMoisture,
		CAST(ROUND(AVG((tRL.Moisture-TargetMoisture)),2) As Decimal(4,2)) AS avgMoistureVar,
		SUM(Case When tRL.rejected= 'Y' Then 1 Else 0 End) As RejectedCount,
		Count(*) AS Readings 
		FROM tblTopCustomers tTC
		INNER JOIN tblRoastingLog tRL ON tTC.Blend = tRL.Blend 
		WHERE (tRL.DateTest BETWEEN @dtefromDate AND @dteToDate)
				AND tTC.Customer = @vchCustomer 
		GROUP BY tTC.Customer, tTC.Blend
		ORDER BY tTC.Customer, tTC.Blend

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

