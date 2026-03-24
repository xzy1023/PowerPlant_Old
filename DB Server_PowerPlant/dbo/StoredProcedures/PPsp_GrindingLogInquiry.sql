


-- =============================================
-- Author:		Bong Lee
-- Create date: May 28,2010
-- Description:	Grinding Log Inquiry
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_GrindingLogInquiry]
	-- Add the parameters for the stored procedure here
	@vchAction varchar(50) = NULL,
	@chrFacility char(3),
	@dteStartTime datetime, 
	@dteEndTime datetime,
	@chrPkgLIne char(10) = null,
	@bitRewkSplOnly bit = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	;With cteGL AS
	(
	SELECT tGL.ScheduleID , tGL.Grinder, tGL.Blend, tGL.Grind, tGL.EquipmentType, tGL.Bin, tGL.EquipmentID, tGL.Shift, SUM(tGL.ActualWgt) AS ActualWgt, MIN(tGL.StartTime) AS StartTime, 
				MAX(ISNULL(tGL.EndTime,'12/13/9999')) AS EndTime, tGL.MinColour, tGL.TargetColour, tGL.MaxColour, MAX(CAST(tGL.Rejected AS integer)) AS Rejected, SUM(tGL.RejectedWgt) AS RejectedWgt
				,AVG(tGD.Colour) AS AvgColour 
			FROM tblGrindingLog tGL
			LEFT OUTER JOIN tblGrindData tGD 
			On tGL.GrindJobID = tGD.GrindJobID 
			WHERE Active = 1 AND Facility = @chrFacility AND StartTime BETWEEN @dteStartTime AND @dteEndTime 
				AND (@chrPkgLIne is NULL OR EquipmentID = @chrPkgLIne)
			GROUP BY tGL.ScheduleID, tGL.Grinder, tGL.Blend, tGL.Grind, tGL.EquipmentType, tGL.Bin, tGL.EquipmentID, tGL.Shift, tGL.MinColour, tGL.TargetColour, tGL.MaxColour
	)
	
	Select cteGL.*, tLastInGrp.UpdatedBy, tLastInGrp.Comment, ISNULL(tOW.ReworkWgt,0) AS ReworkWgt, ISNULL(tOW.SpillageWgt,0) AS SpillageWgt  From cteGL
		LEFT OUTER JOIN (SELECT ScheduleID,UpdatedBy,Comment FROM tblgrindinglog 
			WHERE GrindJobID IN (
				SELECT MAX(Grindjobid) as GrindJobID 
				FROM tblGrindingLog 
				WHERE Active = 1 AND Facility = @chrFacility AND StartTime BETWEEN @dteStartTime AND @dteEndTime 
					AND (@chrPkgLIne is NULL OR EquipmentID = @chrPkgLIne)
				GROUP BY ScheduleID)) AS tLastInGrp 
	ON cteGL.ScheduleID = tLastInGrp.ScheduleID 
	LEFT OUTER JOIN tblOtherWeights As tOW 
	ON cteGL.ScheduleID = tOW.ScheduleID
	WHERE (ISNULL(tOW.ReworkWgt,0) > CASE WHEN @bitRewkSplOnly = 0 THEN -999999 ELSE 0 END) 
		OR (ISNULL(tOW.SpillageWgt,0)> CASE WHEN @bitRewkSplOnly = 0 THEN -999999 ELSE 0 END)
	ORDER BY cteGL.StartTime 

END

GO

