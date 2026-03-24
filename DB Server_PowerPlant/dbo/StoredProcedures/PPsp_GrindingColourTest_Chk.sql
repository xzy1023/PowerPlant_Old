


-- =============================================
-- Author:		Bong Lee
-- Create date: April 15, 2008
-- Description:	Check Grinding Colour Test Data
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_GrindingColourTest_Chk]
(
	@vchAction as varchar(30),
	@chrFacility as char(3),
	@vchScheduleID as varchar(50),
	@vchMessage as varchar(256) OUTPUT
)	
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @RcdCount as smallint
	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);
	
	BEGIN TRY
		SELECT @vchMessage = ''

		IF @vchAction = 'IsTestRequired'
		BEGIN
			SELECT @RcdCount = COUNT(*) FROM dbo.tblGrindingLog AS T1
			LEFT OUTER JOIN tblGrindData AS T2 
			ON T1.GrindJobID = T2.GrindJobID
			WHERE T1.Facility = @chrFacility AND T1.ScheduleID = @vchScheduleID and T2.inactive = 0
			IF @RcdCount = 0 
				SELECT @vchMessage = 'Colour test is required.'
--			ELSE
--			BEGIN	
--				SELECT @RcdCount = COUNT(*) from dbo.tblGrindingLog AS T1
--				LEFT OUTER JOIN tblGrindData AS T2 
--				ON T1.GrindJobID = T2.GrindJobID
--				WHERE T1.Facility = @chrFacility AND T1.ScheduleID = @vchScheduleID and T2.inactive = 0 AND TestResult = 1
--				IF @RcdCount = 0 
--				SELECT @vchMessage = 'Colour test is required as none of tests passed.'
--			END 
		END
		
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

