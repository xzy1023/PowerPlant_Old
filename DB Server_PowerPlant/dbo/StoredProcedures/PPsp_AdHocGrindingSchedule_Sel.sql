

-- =============================================
-- Author:		Bong Lee
-- Create date: Feb 2, 2008
-- Description:	Select Ad-Hoc Grinding Schedule
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_AdHocGrindingSchedule_Sel] 
	-- Add the parameters for the stored procedure here
	@vchAction varchar(30),
	@chrfacility char(3),
	@vchFromDate varchar(8) = NULL,
	@vchToDate varchar(8) = NULL,
	@vchScheduleID varchar(30) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @intFromDate AS Int
	DECLARE @intToDate AS Int

	IF @vchAction = 'ByProductionDate'
	BEGIN
		Set @intFromDate = CAST(@vchFromDate AS Int)
		Set @intToDate = CAST(@vchToDate AS Int)
		SELECT * from dbo.tblAdHocGrindingSchedule where Facility = @chrfacility and ProductionDate between @intFromDate and @intToDate
			ORDER BY ProductionDate,StartDate,StartTime
	End
	ELSE 
		IF @vchAction = 'ByScheduleID'
			SELECT * from dbo.tblAdHocGrindingSchedule where Facility = @chrfacility and ScheduleID = @vchScheduleID		
END

GO

