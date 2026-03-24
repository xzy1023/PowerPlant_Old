


-- =============================================
-- Author:		Bong Lee
-- Create date: Feb 2, 2008
-- Description:	Add AdHoc Grinding Schedule
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_AdHocGrindingSchedule_Add] 
	-- Add the parameters for the stored procedure here
	@chrFacility char(3),
	@vchGrinder varchar(10),
	@vchBlend char(6),
	@vchGrind char(6),
	@intProductionDate int,
	@vchFromTank varchar(10),
	@vchToBin varchar(10),
	@chrEquipmentID char(10),
	@intScheduledWgt int,
	@intStartDate int,
	@intStartTime int,
	@intEndDate int,
	@intEndTime int,
	@vchCreatedBy varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @vchScheduleID varchar(30);
	DECLARE @dteTime datetime;
	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);
	
	BEGIN TRY
	SELECT @dteTime = getdate()
	SELECT @vchScheduleID = '#' + RTRIM(EquipmentID) + convert(varchar(8),@dteTime,112) + 
		[dbo].[fnFillLeadingZeros](2,CONVERT(varchar(2),DATEPART(Hour ,@dteTime))) +
		[dbo].[fnFillLeadingZeros](2,CONVERT(varchar(2),DATEPART(Minute, @dteTime))) + 
		[dbo].[fnFillLeadingZeros](2,CONVERT(varchar(2),DATEPART(Second,@dteTime))) + 
		[dbo].[fnFillLeadingZeros](3,CONVERT(varchar(3),DATEPART(millisecond ,@dteTime)))
		FROM dbo.tblEquipment WHERE facility = @chrFacility and ProbatID = @vchGrinder and [Type] = 'G'

	INSERT INTO tblAdHocGrindingSchedule
       (Facility, ScheduleID, Grinder, Blend, Grind, ProductionDate, FromTank, ToBin, EquipmentID, ScheduledWgt, StartDate, StartTime, EndDate, EndTime, 
        CreatedBy, UpdatedBy, UpdatedAt)
	VALUES (@chrFacility,@vchScheduleID,@vchGrinder,@vchBlend,@vchGrind,@intProductionDate,@vchFromTank,@vchToBin,@chrEquipmentID,@intScheduledWgt,@intStartDate,
			@intStartTime,@intEndDate,@intEndTime,@vchCreatedBy,@vchCreatedBy,@dteTime) 
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

