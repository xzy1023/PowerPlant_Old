


-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 3, 2008
-- Description:	Roaster Conformance
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoasterOperatorConformance] 
	@vchFacility varchar(100), 
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
	SET  @dteToDate = Convert(datetime, Convert(varchar(10),@dteToDate, 101) + ' 23:59:59', 120);

	WITH cteRoasterOprConf(Facility, RoasterNo, RoasterInit, ColourInSpec, ColourInTolerance, MoistureVar, Rejected, NoOfMoistureData )
	AS (
    SELECT tRL.Facility, tRL.RoasterNo,RoasterInit,
		CASE WHEN Colour > SpecMax OR Colour < SpecMin Then 0 ELSE 1 END AS ColourInSpec,
		CASE WHEN Colour > (SpecMax + Tolerance) OR Colour < (SpecMin - Tolerance) Then 0 ELSE 1 END ColourInTolerance,
		CASE WHEN Moisture > 1 THEN ([Moisture] - [TargetMoisture]) ELSE 0 END AS MoistureVar,
		CASE WHEN Rejected = 'Y' Then 1 ELSE 0 END AS Rejected,
		CASE WHEN Moisture > 1 Then 1 ELSE 0 END AS NoOfMoistureData
	 FROM tblRoastingLog tRL Left Outer Join tblColourTolerance AS tCT
	  ON tRL.Facility = tCT.Facility AND tRL.RoasterNo = tCT.RoasterNo AND tRL.Blend = tCT.Blend
	WHERE tRL.Facility in (Select Value from dbo.fnSplit(@vchFacility,',')) AND tRL.DateTest between @dteFromDate and @dteToDate 
	)
    SELECT T1.Facility, T2.Description, ISNULL(T3.FirstName + ' ' + T3.LastName, T1.RoasterInit) AS Operator, T1.RoasterInit,
		 T1.RoasterNo,  COUNT(*) AS Readings, 
		ROUND(SUM(T1.ColourInSpec) / CAST(COUNT(*) AS REAL) * 100, 2) AS [%InSpec], 
		ROUND(SUM(T1.ColourInTolerance) / CAST(COUNT(*) AS REAL) * 100, 2) AS [%InTolerance], 
		ROUND(SUM(T1.MoistureVar) / CAST(SUM(NoOfMoistureData) AS REAL), 2) AS AvgMoistureVar,
		ROUND(SUM(T1.Rejected) / CAST(COUNT(*) AS REAL) * 100, 2) AS [%Rejected] 
     FROM         cteRoasterOprConf AS T1 
		LEFT OUTER JOIN tblFacility AS T2 ON T1.Facility = T2.Facility
		LEFT JOIN tblPlantStaff T3 ON T1.Facility = T3.Facility AND T1.RoasterInit = T3.StaffID 
     GROUP BY T1.Facility, T2.Description, ISNULL(T3.FirstName + ' ' + T3.LastName, T1.RoasterInit), T1.RoasterInit, T1.RoasterNo
     ORDER BY T1.Facility, T2.Description, ISNULL(T3.FirstName + ' ' + T3.LastName, T1.RoasterInit), T1.RoasterInit, T1.RoasterNo

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

grant execute on object::PPsp_RoasterOperatorConformance to ppuser

GO

