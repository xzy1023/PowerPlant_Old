




-- =============================================
-- Author:		Bong Lee
-- Create date: Feb.06 2008
-- Description:	Add grinding log
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_GrindingLog_Add]
	@chrFacility char(3),
	@vchGrinder varchar(6),
	@vchBlend varchar(6),
	@vchGrind varchar(6),
	@vchBin varchar(10),
	@chrEquipmentType char(1),
	@chrEquipmentID char(10),
	@vchCreatedBy varchar(50),
	@intShift tinyint = 0,
	@intActualWgt int,
	@dteStartTime datetime = NULL,
	@dteEndTime datetime = NULL,
	@decMinColour decimal(5,2) = 0,
	@decTargetColour decimal(5,2)= 0,
	@decMaxColour decimal(5,2) = 0,
	@vchComment varchar(255),
	@bitRejected bit = '0',
	@intRejectedWgt int = 0,
	@bitFinalized bit = '0',
	@vchScheduleID varchar(50),
	@vchStatus varchar(10),
	@intGrindJobID_Out int OUTPUT
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
		INSERT INTO tblGrindingLog
               (Facility, Grinder, Blend, Grind, EquipmentType, Bin, EquipmentID, CreatedBy, Shift, ActualWgt, StartTime, EndTime, 
                MinColour, TargetColour, MaxColour, Comment, Rejected, RejectedWgt, Finalized, ScheduleID, UpdatedBy, UpdatedTime, Status)
		VALUES  (@chrFacility, @vchGrinder, @vchBlend, @vchGrind, @chrEquipmentType, @vchBin, @chrEquipmentID, @vchCreatedBy, @intShift, @intActualWgt,@dteStartTime, @dteEndTime,
				 @decMinColour, @decTargetColour, @decMaxColour, @vchComment, @bitRejected, @intRejectedWgt, @bitFinalized, @vchScheduleID, @vchCreatedBy, getdate(), @vchStatus)

		SET @intGrindJobID_Out = @@IDENTITY
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

