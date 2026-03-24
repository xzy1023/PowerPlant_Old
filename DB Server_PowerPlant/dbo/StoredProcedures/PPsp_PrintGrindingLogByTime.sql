
-- =============================================
-- Author:		Bong Lee
-- Create date: Nov16,2007
-- Description:	Select Grinding Log data
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PrintGrindingLogByTime]
	-- Add the parameters for the stored procedure here
	@chrFacility char(3),
	@dteStartTime datetime, 
	@dteEndTime datetime,
	@chrPkgLIne char(10) = null,
	@vchSortExpression varchar(50) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	DECLARE @vchSQLStmt varchar(2000);
	DECLARE @chrFmDateCYMD VARCHAR(19);
	DECLARE @chrToDateCYMD VARCHAR(19);
	SET @chrFmDateCYMD = CONVERT(VARCHAR,@dteStartTime,120)
	SET @chrToDateCYMD = CONVERT(VARCHAR,@dteEndTime,120)

	SET NOCOUNT ON;

	Set @vchSQLStmt = 'SELECT T1.ScheduleID, T1.Grinder, T1.Blend, T1.Grind, T1.EquipmentType, T1.Bin, T1.EquipmentID, ActualWgt, ISNULL(tOW.ReworkWgt,0) AS ReworkWgt, ' + 
				'ISNULL(tOW.SpillageWgt,0) AS SpillageWgt, T1.RejectedWgt, T1.StartTime, ' +
				'CASE WHEN T1.EndTime= ''12/13/9999'' THEN NULL ELSE T1.EndTime END AS EndTime, T1.MinColour, T1.TargetColour, T1.MaxColour, ' +
				'CAST(T5.AvgColour AS decimal(5, 2)) AS AvgColour, T1.Rejected, T2.UpdatedBy ' +
				'FROM (SELECT ScheduleID , Grinder, Blend, Grind, EquipmentType, Bin, EquipmentID, Shift, SUM(ActualWgt) AS ActualWgt, ' +
					'MIN(StartTime) AS StartTime, MAX(ISNULL(EndTime,''12/13/9999'')) AS EndTime, MinColour, TargetColour, MaxColour, ' +
					'MAX(CAST(Rejected AS integer)) AS Rejected, SUM(RejectedWgt) AS RejectedWgt ' +
					'FROM tblGrindingLog ' + 
					'WHERE Active = 1 AND Facility = ''' + @chrFacility + ''' and StartTime BETWEEN ''' + @chrFmDateCYMD + ''' AND ''' + @chrToDateCYMD + ''' ' +
					'GROUP BY ScheduleID, Grinder, Blend, Grind, EquipmentType, Bin, EquipmentID, Shift, MinColour, TargetColour, MaxColour) AS T1 ' +
				'LEFT OUTER JOIN ' +
					-- get the updatedby,comment from the last record of the schedule
					'(SELECT ScheduleID,UpdatedBy,Comment FROM tblgrindinglog WHERE GrindJobID IN ' +
						'(SELECT MAX(Grindjobid) as GrindJobID from tblGrindingLog ' +
							'WHERE Active = 1 AND Facility = ''' + @chrFacility + ''' AND (StartTime BETWEEN ''' + @chrFmDateCYMD + ''' AND ''' + @chrToDateCYMD + ''') ' +
							'GROUP BY ScheduleID)) AS T2 ' +
					'ON T1.ScheduleID = T2.ScheduleID ' +
				'LEFT OUTER JOIN tblOtherWeights As tOW ON T1.ScheduleID = tOW.ScheduleID ' +
				'LEFT OUTER JOIN ' +
					'(SELECT tGL.scheduleid, AVG(tGD.Colour) AS AvgColour FROM tblGrindingLog tGL ' +
					'INNER JOIN tblGrindData tGD on tGL.GrindJobID = tGD.GrindJobID ' +
					'WHERE tGL.Facility = ''' + @chrFacility + ''' AND (tGL.StartTime BETWEEN ''' + @chrFmDateCYMD + ''' AND ''' + @chrToDateCYMD + ''') ' +
					'GROUP BY tGL.ScheduleID) AS T5 ON T1.ScheduleID = T5.ScheduleID ' 
	
	IF @chrPkgLIne is not null
		Set @vchSQLStmt = @vchSQLStmt + ' WHERE T1.EquipmentID = ''' + @chrPkgLIne + ''' '

	IF @vchSortExpression <> 'StartTime'
		Set @vchSQLStmt = @vchSQLStmt + ' ORDER by ' + @vchSortExpression + ', T1.StartTIme'
	Else
		Set @vchSQLStmt = @vchSQLStmt + ' ORDER by T1.StartTime'
print @vchSQLStmt
	EXECUTE (@vchSQLStmt)

END

GO

