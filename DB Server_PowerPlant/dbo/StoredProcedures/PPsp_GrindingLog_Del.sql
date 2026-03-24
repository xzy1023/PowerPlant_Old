





-- =============================================
-- Author:		Bong Lee
-- Create date: Feb.06 2008
-- Description:	Delete Grinding Log Record
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_GrindingLog_Del]
	@GrindJobID as int,
	@vchUpdatedBy varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);

	BEGIN TRY
		UPDATE dbo.tblGrindingLog SET Active = 0,  UpdatedBy = @vchUpdatedBy, UpdatedTime = getdate()
		WHERE GrindJobID = @GrindJobID
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

