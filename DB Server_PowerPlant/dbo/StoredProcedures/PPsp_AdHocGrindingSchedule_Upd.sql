

-- =============================================
-- Author:		Bong Lee
-- Create date: Feb.06 2008
-- Description:	Update Ad-Hoc Grinding Schedule
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_AdHocGrindingSchedule_Upd]
	@vchScheduleID varchar(30),
	@chrFacility char(3),
	@vchGrinder varchar(10),
	@vchBlend varchar(6),
	@vchGrind varchar(6),
	@intProductionDate int,
	@vchFromTank varchar(10),
	@vchToBin varchar(10),
	@chrEquipmentID char(10),
	@vchUpdatedBy varchar(50),
	@intScheduledWgt int,
	@intStartDate int,
	@intStartTime int,
	@intEndDate int,
	@intEndTime int
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
		UPDATE [dbo].[tblAdHocGrindingSchedule]
           SET [Grinder] = @vchGrinder
           ,[Blend] = @vchBlend
           ,[Grind] = @vchGrind
           ,[ProductionDate] = @intProductionDate
		   ,[FromTank] = @vchFromTank
           ,[ToBin] = @vchToBin
           ,[EquipmentID] = @chrEquipmentID
           ,[ScheduledWgt] = @intScheduledWgt
           ,[StartDate] = @intStartDate
           ,[StartTime] = @intStartTime
           ,[EndDate] = @intEndDate
           ,[EndTime] = @intEndTime
           ,[UpdatedBy] = @vchUpdatedBy
           ,[UpdatedAt] = getdate()
		WHERE [Facility]= @chrFacility AND [ScheduleID] = @vchScheduleID
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

