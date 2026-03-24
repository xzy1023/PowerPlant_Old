
CREATE PROCEDURE [dbo].[PPsp_PrintGrindingLog]
	@chrFacility CHAR(3),
	@vchReportDate VARCHAR(10),
	@intShift CHAR(1)
AS
	BEGIN
	DECLARE @dteReportDate Datetime
	DECLARE @chrFmDateCYMD VARCHAR(10)
	DECLARE @chrToDateCYMD VARCHAR(10)
	DECLARE @chrFmTime VARCHAR(8)
	DECLARE @chrToTime VARCHAR(8)
	
	DECLARE @dteFmDateTime DATETIME
	DECLARE @dteToDateTime DATETIME

	-- Find the time range for the specified shift from the Shift Table
--	SELECT  @chrFmTime = cast(datepart(hh,FromTime) as varchar(2)) + ':' + cast(datepart(mi,fromTime) as varchar(2)) + ':' + cast(datepart(ss,fromTime) as varchar(2)),
--			@chrToTime = cast(datepart(hh,ToTime) as varchar(2)) + ':' + cast(datepart(mi,ToTime) as varchar(2)) + ':' + cast(datepart(ss,toTime) as varchar(2))
	SELECT @chrFmTime = convert(varchar(8),FromTime,14), @chrToTime = convert(varchar(8),ToTime,14)
	FROM         tblShift
	WHERE     (facility = @chrFacility) AND (WorkGroup = 'G') AND (Shift = @intShift) 

	--SET @dteReportDate = CONVERT(SMALLDATETIME,@vchReportDate,120)
	SET @dteReportDate = CONVERT(DATETIME,@vchReportDate,120)
	
	-- Convert the input date formats or Report Date to yyyy-mm-dd hh:mi:ss(24h) format in character
	SET @chrFmDateCYMD = CONVERT(VARCHAR,@dteReportDate,120)
	
	-- If it is night shift, assuming the input parameter is Yesterday.
	-- The From Date will be Yesterday,and To Date is today's date. 	
	IF @intShift = 3
		SET @dteReportDate = DATEADD(d,1,@dteReportDate)
		
	SET @chrToDateCYMD = CONVERT(VARCHAR,@dteReportDate,120)
	
	-- Convert the from and to time of the shift from character to DateTime data type
	SET @dteFmDateTime = CONVERT(DATETIME,@chrFmDateCYMD + ' ' + @chrFmTime,120)
	SET @dteToDateTime = CONVERT(DATETIME,@chrToDateCYMD + ' ' + @chrToTime,120)

	print @chrFmDateCYMD + ' ' + @chrFmTime
	print @chrToDateCYMD + ' ' + @chrToTime
	print @dteFmDateTime
	print @dteToDateTime

	SELECT T4.GroupID, T1.ScheduleID, T1.Grinder, T1.Blend, T1.Grind, T1.EquipmentType, T1.Bin, T1.EquipmentID, T4.Description, T1.Shift, 
		T1.MinColour, T1.TargetColour, T1.MaxColour, T1.ActualWgt, T1.Rejected, T1.RejectedWgt,
		SUBSTRING(CONVERT(varchar(16),T1.StartTime, 120), 6, 11) AS StartTime, 
		CASE WHEN EndTime = '12/31/9999' THEN '** TBD **' ELSE SUBSTRING(CONVERT(varchar(16),T1.EndTime, 120), 6, 11) END AS EndTime, 
		T2.UpdatedBy, CASE WHEN CHARINDEX('//',T2.Comment) >0 THEN LTRIM(SUBSTRING(T2.Comment, CHARINDEX('//', T2.Comment)+2, LEN(T2.Comment))) END as Comment, ISNULL(T3.ReworkWgt,0) + ISNULL(T3.SpillageWgt,0) AS OtherWgt, 
		T1.ActualWgt + ISNULL(T3.ReworkWgt,0) + ISNULL(T3.SpillageWgt,0) - T1.RejectedWgt AS TotalWgt,
		CAST(T5.AvgColour as decimal(5, 2)) AS AvgColour
	FROM (SELECT   ScheduleID , Grinder, Blend, Grind, EquipmentType, Bin, EquipmentID, Shift, SUM(ActualWgt) AS ActualWgt, 
			MIN(StartTime) AS StartTime, MAX(ISNULL(EndTime,'12/31/9999')) AS EndTime, MinColour, TargetColour, MaxColour, 
			MAX(CAST(Rejected AS integer)) AS Rejected, SUM(RejectedWgt) AS RejectedWgt
			FROM tblGrindingLog
			WHERE Active = 1 AND Facility = @chrFacility and StartTime BETWEEN @dteFmDateTime AND @dteToDateTime
			GROUP BY ScheduleID, Grinder, Blend, Grind, EquipmentType, Bin, EquipmentID, Shift, MinColour, TargetColour, MaxColour) AS T1
	LEFT OUTER JOIN
	-- get the updatedby,comment from the last record of the schedule
	(SELECT ScheduleID,UpdatedBy,Comment FROM tblgrindinglog WHERE GrindJobID IN 
		(SELECT MAX(Grindjobid) as GrindJobID from tblGrindingLog 
			WHERE Active = 1 AND Facility = @chrFacility AND StartTime BETWEEN @dteFmDateTime AND @dteToDateTime
			GROUP BY ScheduleID)
	) AS T2 
	ON T1.ScheduleID = T2.ScheduleID
	LEFT OUTER JOIN tblOtherWeights T3 on t1.ScheduleID = t3.ScheduleID
	LEFT OUTER JOIN tblEquipment AS T4 ON T1.EquipmentID = T4.EquipmentID
	-- get average colour by schedule ID
	LEFT OUTER JOIN (SELECT tGL.scheduleid, AVG(tGD.Colour) AS AvgColour FROM tblGrindingLog tGL
			INNER JOIN tblGrindData tGD on tGL.GrindJobID = tGD.GrindJobID
			WHERE tGL.Facility = @chrFacility AND (tGL.StartTime BETWEEN @dteFmDateTime AND @dteToDateTime)
			GROUP BY tGL.ScheduleID) AS T5 ON T1.ScheduleID = T5.ScheduleID
	-- WHERE     (T1.StartTime BETWEEN @dteFmDateTime AND @dteToDateTime)
	ORDER BY T4.GroupID, T4.EquipmentID, T1.EndTime
		
--	SELECT T2.GroupID, T2.Description AS Machine, T1.Grinder, T1.UpdatedBy, T1.Bin, T1.Blend, T1.Grind, T1.ActualWgt, 
--		  ISNULL(T4.ReworkWgt,0) + ISNULL(T4.SpillageWgt,0) AS OtherWgt, 
--		  T1.ActualWgt + ISNULL(T4.ReworkWgt,0) + ISNULL(T4.SpillageWgt,0) - T1.RejectedWgt AS TotalWgt, T1.RejectedWgt, 
--		  SUBSTRING(CONVERT(NVarchar(16), StartTime, 120), 6, 11) AS StartTime, SUBSTRING(CONVERT(NVarchar(16), EndTime, 120), 6, 11) AS EndTime, 
--		  Comment, CAST(T3.AvgColour AS decimal(5, 2)) AS AvgColour, T1.MinColour, T1.MaxColour, T1.Finalized
--		FROM  tblGrindingLog_NEW AS T1 
--		LEFT OUTER JOIN tblOtherWeights_New As T4 ON T1.Facility = T4.facility AND T1.ScheduleID = T4.ScheduleID
--		LEFT OUTER JOIN tblEquipment AS T2 ON T1.Facility = T2.facility AND T1.EquipmentID = T2.EquipmentID
--		LEFT OUTER JOIN (SELECT  GrindJobID, AVG(Colour) AS AvgColour
--				FROM tblGrindData
--				WHERE (Inactive = '0')
--				GROUP BY GrindJobID) AS T3 ON T1.GrindJobID = T3.GrindJobID
--		WHERE     (T1.StartTime BETWEEN @dteFmDateTime AND @dteToDateTime)
--		ORDER BY T2.GroupID, Machine, T1.EndTime
	
	RETURN
	END

GO

