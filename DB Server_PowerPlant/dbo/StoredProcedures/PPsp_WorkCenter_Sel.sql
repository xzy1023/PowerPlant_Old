
-- =============================================
-- Author:		Bong Lee
-- Create date: Feb 15, 2012
-- Description:	Select Work Center
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_WorkCenter_Sel] 
	@vchAction varchar(30) = NULL
	,@vchFacility varchar(3) = NULL
	,@intWorkCenter int = NULL
	,@vchActive bit = NULL 
	,@vchSortBy varchar(50) = NULL 
AS
BEGIN
	SET NOCOUNT ON;

	IF	@vchAction = 'AllWithBlank'
		SELECT * FROM 
			(SELECT '' as Facility
				,NULL as WorkCenter
				,'*All' as Description 
				,'0' as Active
					Union
			  SELECT Facility, WorkCenter, Description, Active
				FROM   tblWorkCenter
				WHERE (@vchFacility IS NULL OR Facility = @vchFacility)
					AND (@intWorkCenter IS NULL OR WorkCenter = @intWorkCenter)
					AND (@vchActive IS NULL OR Active = @vchActive)
			) tWC
			ORDER BY CASE 
					 WHEN @vchSortBy is NULL OR @vchSortBy = 'DESC' THEN tWC.Description
					 ELSE CAST(tWC.WorkCenter as varchar(50))
					 END
	ELSE
		SELECT * FROM tblWorkCenter
		WHERE (@vchFacility IS NULL OR Facility = @vchFacility)
			AND (@intWorkCenter IS NULL OR WorkCenter = @intWorkCenter)
			AND (@vchActive IS NULL OR Active = @vchActive)
		ORDER BY CASE 
					 WHEN @vchSortBy is NULL OR @vchSortBy = 'DESC' THEN Description
					 ELSE CAST(WorkCenter as varchar(50))
				 END
		
END

GO

