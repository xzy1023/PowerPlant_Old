




-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 3, 2008
-- Description:	Roasting Compliance By Blend
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoastingComplianceByBlend] 
	@dteFromDate datetime,
	@dteToDate datetime,
	@bitOperator bit,
	@bitRoaster bit
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
	SET  @dteToDate = Convert(datetime, Convert(varchar(10),@dteToDate, 101) + ' 23:59:59', 120);

	WITH cteRoastingCompl(Blend, Operator, RoasterNo, Compliance, Colour,SpecTarget, ColourVar,
			Moisture, TargetMoisture, MoistureVar, Rejected, NoOfMoistureData )
	AS (
    SELECT tRL.Blend, ISNULL(tPS.FirstName + ' ' + tPS.LastName, tRL.RoasterInit) as Operator,
		tRL.RoasterNo, 	
		CASE WHEN Colour > SpecMax OR Colour < SpecMin Then 0 ELSE 1 END AS Compliance,
		Colour,SpecTarget, (Colour-SpecTarget) as ColourVar,
		CASE WHEN Moisture > 1 Then Moisture ELSE 0 END AS Moisture,
		TargetMoisture,
		CASE WHEN Moisture > 1 Then (Moisture-TargetMoisture) ELSE 0 END AS MoistureVar,
		CASE WHEN Rejected = 'Y' Then 1 ELSE 0 END AS Rejected,
		CASE WHEN Moisture > 1 Then 1 ELSE 0 END AS NoOfMoistureData
	FROM tblRoastingLog tRL LEFT JOIN tblPlantStaff tPS ON tRL.Facility = tPS.Facility AND tRL.RoasterInit = tPS.StaffID 
	WHERE tRL.DateTest between @dteFromDate and @dteToDate 
	)
	SELECT Blend, 
			Case When @bitOperator = 1 Then Operator Else NULL End As Operator, 
			Case When @bitRoaster = 1 Then RoasterNo Else NULL End As RoasterNo,   
			Sum(Compliance)/CAST(COUNT(*) AS REAL) * 100 As [%Compliance], 
			Avg(Colour) As Colour,
			Avg(SpecTarget) As TargetColour, 
			Avg(ColourVar) As ColourVar,
			Sum(Moisture)/Sum(NoOfMoistureData) As Moisture, 
			Sum(TargetMoisture)/Sum(NoOfMoistureData) As TargetMoisture, 
			Sum(MoistureVar)/Sum(NoOfMoistureData) As AvgMoistureVar, 
			Sum(Rejected) as Rejects,
			Count(*) as NoOfReadings,
			Sum(NoOfMoistureData) as NoOfMoistureData
		From cteRoastingCompl
		Group By Blend, 
			Case When @bitOperator = 1 Then Operator Else NULL End,
			Case When @bitRoaster = 1 Then RoasterNo Else NULL End
		Order by Blend, 
			Case When @bitOperator = 1 Then Operator Else NULL End,
			Case When @bitRoaster = 1 Then RoasterNo Else NULL End

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

