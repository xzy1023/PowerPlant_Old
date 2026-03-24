


-- =============================================
-- Author:		Bong Lee
-- Create date: March 12, 2008
-- Description:	Edit Rework and Spillage Weights
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_OtherWeights_Edit] 
	@chrFacility char(3),
	@GSCACTI varchar(30),
	@ReworkWgt int,
	@SpillageWgt int,
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
		If exists (Select * from dbo.tblOtherWeights WHERE Facility= @chrFacility AND ScheduleID= @GSCACTI)
			Update dbo.tblOtherWeights
			Set ReworkWgt=@ReworkWgt, SpillageWgt=@SpillageWgt, UpdatedBy = @vchUpdatedBy, UpdatedAt = getdate()
			WHERE Facility= @chrFacility AND ScheduleID= @GSCACTI
		Else
			IF @ReworkWgt > 0 OR @SpillageWgt > 0
			BEGIN
				INSERT INTO tblOtherWeights
						  (Facility, ScheduleID, ReworkWgt, SpillageWgt, CreatedBy, UpdatedBy)
				VALUES     (@chrFacility, @GSCACTI, @ReworkWgt,@SpillageWgt,@vchUpdatedBy,@vchUpdatedBy)
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

grant execute on object::PPsp_OtherWeights_Edit to ppuser

GO

