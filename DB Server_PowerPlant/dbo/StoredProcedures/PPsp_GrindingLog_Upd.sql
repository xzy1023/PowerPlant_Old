


-- =============================================
-- Author:		Bong Lee
-- Create date: Mar.21 2006
-- Description:	Update grinding log
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_GrindingLog_Upd]
	-- Add the parameters for the stored procedure here
	@vchAction varchar(30),
	@intGrindJobID int, 
	@vchGrinder varchar(6) = NULL,
	@vchBlend varchar(6) = NULL,
	@vchGrind varchar(6) = NULL,
	@vchBin varchar(10) = NULL,
	@chrEquipmentType char(1) = NULL,
	@chrEquipmentID char(10) = NULL,
	@vchUpdatedBy varchar(50),
	@intShift tinyint = 0,
	@intActualWgt int = 0,
	@dteStartTime datetime = NULL,
	@dteEndTime datetime = NULL ,
	@decMinColour decimal(5,2) = 0,
	@decTargetColour decimal(5,2)= 0,
	@decMaxColour decimal(5,2) = 0,
	@vchComment varchar(255) = NULL,
	@bitRejected bit = '0',
	@intRejectedWgt int = 0,
	@bitFinalized bit = '0'
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @vchStatus as varchar(10);
	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);

	-- Update the record
	BEGIN

		BEGIN TRY
			IF @vchAction = 'ControlRoom'
			BEGIN
				UPDATE    tblGrindingLog
				SET	 Grinder = @vchGrinder, Blend = @vchBlend, Grind = @vchGrind, EquipmentType = @chrEquipmentType, Bin = @vchBin, EquipmentID = @chrEquipmentID, UpdatedBy = @vchUpdatedBy,
					ActualWgt = @intActualWgt, Comment = @vchComment, UpdatedTime = getdate(), Finalized = @bitFinalized
				WHERE	GrindJobID = @intGrindJobID
			END
			ELSE
				IF @vchAction = 'ControlRoom_EndGrindJob'
				BEGIN				
					UPDATE    tblGrindingLog
					SET EndTime = getdate(), UpdatedBy = @vchUpdatedBy, Status = 'Done', Finalized = @bitFinalized
					WHERE	GrindJobID = @intGrindJobID
				END
				ELSE
				BEGIN
					-- grind job start
					IF @vchAction = 'GrindRoom_Start'
					BEGIN
						UPDATE  tblGrindingLog
						SET  UpdatedBy = @vchUpdatedBy, Shift = @intShift, StartTime = @dteStartTime, EndTime = @dteEndTime, MinColour = @decMinColour, TargetColour = @decTargetColour, MaxColour = @decMaxColour, Comment = @vchComment, 
							 UpdatedTime = getdate(), Status = 'Started'
						WHERE	GrindJobID = @intGrindJobID
					END
					ELSE 
					-- grind job end
					IF @vchAction = 'GrindRoom_End'
						BEGIN
							UPDATE  tblGrindingLog
							SET UpdatedBy = @vchUpdatedBy, Shift = @intShift, StartTime = @dteStartTime,  EndTime = @dteEndTime, Comment = @vchComment, Rejected = @bitRejected, RejectedWgt = @intRejectedWgt, 
								 UpdatedTime = getdate(), Status = 'Done'
							WHERE	GrindJobID = @intGrindJobID
						END
					ELSE
						-- Update Comment
						IF @vchAction = 'CommentOnly'
							BEGIN
								UPDATE  tblGrindingLog
								SET UpdatedBy = @vchUpdatedBy, Comment = @vchComment, UpdatedTime = getdate()
								WHERE	GrindJobID = @intGrindJobID
							END
						ELSE
							-- Update RejectedWeight
							IF @vchAction = 'RejectedWeightOnly'
								BEGIN
									UPDATE  tblGrindingLog
									SET UpdatedBy = @vchUpdatedBy, Rejected = @bitRejected, RejectedWgt = @intRejectedWgt, UpdatedTime = getdate()
									WHERE	GrindJobID = @intGrindJobID
								END
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
END

GO

