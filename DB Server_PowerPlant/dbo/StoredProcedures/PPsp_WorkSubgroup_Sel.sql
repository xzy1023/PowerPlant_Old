


-- =============================================
-- Author:		Bong Lee
-- Create date: Mar 14, 2012
-- Description:	Select Work Subgroup
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_WorkSubgroup_Sel] 
	@vchAction varchar(50) = NULL
	,@vchActive bit = NULL 
	,@vchFacility varchar(3) = NULL
	,@vchWorkGroup varchar(10) = NULL
	,@vchWorkSubgroup varchar(10) = NULL
	,@vchSortBy varchar(50) = NULL 
AS
BEGIN
	SET NOCOUNT ON;

	IF	@vchAction = 'AllWithBlank'
		SELECT * FROM 
			(SELECT '0' as Active
				,'' as Facility
				,NULL as WorkGroup
				,NULL as WorkSubgroup
				,'*All' as [Description] 
				Union
			  SELECT Active, Facility, Workgroup, WorkSubgroup, [Description]
				FROM   tblWorkSubgroup
				WHERE (@vchFacility IS NULL OR Facility = @vchFacility)
					AND (@vchWorkGroup IS NULL OR WorkGroup = @vchWorkGroup)
					AND (@vchWorkSubgroup IS NULL OR WorkSubgroup = @vchWorkSubgroup)
					AND (@vchActive IS NULL OR Active = @vchActive)
			) tWSG
		ORDER BY CASE 
				 WHEN @vchSortBy is NULL OR @vchSortBy = 'DESC' THEN tWSG.[Description]
				 ELSE tWSG.WorkSubGroup 
				 END
	ELSE
		IF	@vchAction = 'DescList'
			SELECT [WorkSubGroup], [Description] FROM [tblWorkSubGroup] 
				GROUP BY [WorkSubGroup], [Description]
			ORDER BY [Description]
		ELSE
			IF	@vchAction = 'DescListWithBlank'
				SELECT NULL as WorkSubgroup, '' as [Description]
				UNION
				(SELECT [WorkSubGroup], [Description] FROM [tblWorkSubGroup] 
					GROUP BY [WorkSubGroup], [Description])
				ORDER BY [Description]
			ELSE
				IF	@vchAction = 'ByKey'
					SELECT * FROM tblWorkSubgroup
					WHERE  Facility = @vchFacility
						AND WorkGroup = @vchWorkGroup
						AND WorkSubgroup = @vchWorkSubgroup
						AND (@vchActive IS NULL OR Active = @vchActive)
				ELSE
					SELECT * FROM tblWorkSubgroup
					WHERE (@vchFacility IS NULL OR Facility = @vchFacility)
								AND (@vchWorkGroup IS NULL OR WorkGroup = @vchWorkGroup)
								AND (@vchWorkSubgroup IS NULL OR WorkSubgroup = @vchWorkSubgroup)
								AND (@vchActive IS NULL OR Active = @vchActive)
					ORDER BY CASE 
								 WHEN @vchSortBy is NULL OR @vchSortBy = 'DESC' THEN [Description]
								 ELSE WorkSubGroup 
							 END
			
END

GO

