
-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 20, 2008
-- Description:	Roasting Moisture is out of Target Specification
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoastingMoistureOffSpec] 
	@vchFacility varchar(100), 
	@dteFromDate datetime,
	@dteToDate datetime
AS
BEGIN
	
	DECLARE @vchToDate as varchar(10)
	DECLARE @decMoistureTolerance as decimal(3,2)
	DECLARE @LastestTargetMoisture as decimal(4,2)
	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);

	BEGIN TRY
		--SELECT @decMoistureTolerance = cast(Value1 as decimal(3,2)) FROM tblControl WHERE [Key] = 'MoistureTolerance' and [SubKey] = 'frmRoastingLog'

		SELECT T1.Blend, T1.RoasterNo, T2.TargetMoisture, T1.Moisture 
		FROM tblroastinglog T1
		LEFT OUTER JOIN 
		(SELECT Blend, RoasterNo, Avg(TargetMoisture) as TargetMoisture
			FROM tblroastinglog
			WHERE Facility IN (Select Value from dbo.fnSplit(@vchFacility,','))
			AND DateTest Between @dteFromDate And @dteToDate
			AND Moisture > 1		
		GROUP BY Blend, Roasterno) AS T2
		ON T1.Blend = T2.Blend AND T1.Roasterno = T2.Roasterno

		WHERE T1.Facility IN (Select Value from dbo.fnSplit(@vchFacility,','))
			AND T1.DateTest Between @dteFromDate And @dteToDate
			AND T1.Moisture > 1		
		ORDER BY T1.Blend, T1.Roasterno

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

