
-- =============================================
-- Author:		Bong Lee
-- Create date: Sept. 21, 2006
-- Description:	Operation Staffing I/O Module
-- WO#359		Mar. 9,2012  Bong Lee
-- Description:	Add a parameter to filter by record status
-- WO#512		May. 9,2012  Bong Lee
-- Description:	Add a parameter to filter by WorkSubGroup
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PlantStaffing_Sel]
	-- Add the parameters for the stored procedure here
	@vchAction varchar(30),
	@vchWorkGroup varchar(10) = NULL,
--WO#359	@chrFacility char(3),
	@chrFacility char(3) = NULL,	--WO#359
	@vchStaffID varchar(10) = NULL
	,@chrActive char = NULL					--WO#359
	,@vchWorkSubGroup varchar(10) = NULL	--WO#512
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @bitActive as bit
	
	IF @chrActive is NULL
	    SET @bitActive  = 1
	ELSE 
		IF @chrActive = 'A'
			SET @bitActive  = NULL   
		ELSE
			SET @bitActive  = 0
			
	If @vchAction = 'ByID'
	BEGIN
		SELECT	*, RTRIM(FirstName) + ' ' + RTRIM(LastName) as FullName	
		FROM	 dbo.tblPlantStaff
--WO#359	WHERE	ActiveRecord = 1
--WO#359			AND Facility = @chrFacility 
--WO#359			AND (WorkGroup = @vchWorkGroup OR WorkGroup = 'ALL')
			WHERE	(@bitActive IS NULL OR ActiveRecord = @bitActive)	--WO#359
			AND StaffID = @vchStaffID
			AND (@chrFacility is NULL OR Facility = @chrFacility)		--WO#359 
			AND (@vchWorkGroup is NULL OR WorkGroup = @vchWorkGroup OR WorkGroup = 'ALL')	--WO#359
			AND (@vchWorkSubGroup is NULL OR WorkSubGroup = @vchWorkSubGroup)	--WO#512
			
	END
	ELSE
	BEGIN
		If @vchAction = 'ListAll'
		BEGIN
			SELECT	*, RTRIM(FirstName) + ' ' + RTRIM(LastName) as FullName	
			FROM	 dbo.tblPlantStaff
--WO#359	WHERE	ActiveRecord = 1 
--WO#359			AND Facility = @chrFacility
--WO#359			AND (WorkGroup = @vchWorkGroup OR WorkGroup = 'ALL')
			WHERE	(@bitActive IS NULL OR ActiveRecord = @bitActive)	--WO#359
				AND (@chrFacility is NULL OR Facility = @chrFacility)	--WO#359
				AND (@vchWorkSubGroup is NULL OR WorkSubGroup = @vchWorkSubGroup)	--WO#512
				AND (@vchWorkGroup is NULL OR WorkGroup = @vchWorkGroup OR WorkGroup = 'ALL')	--WO#359
				AND (@vchWorkSubGroup is NULL OR WorkSubGroup = @vchWorkSubGroup)	--WO#512
		END
		ELSE
		BEGIN
			If @vchAction = 'ByGroup_SortByName'
			BEGIN
				SELECT	*, RTRIM(FirstName) + ' ' + RTRIM(LastName) as FullName	
				FROM	 dbo.tblPlantStaff
--WO#359		WHERE	ActiveRecord = 1 
--WO#359				AND Facility = @chrFacility
--WO#359				AND (WorkGroup = @vchWorkGroup OR WorkGroup = 'ALL')
				WHERE	(@bitActive IS NULL OR ActiveRecord = @bitActive)	--WO#359
					AND (@chrFacility is NULL OR Facility = @chrFacility)	--WO#359 
					AND (@vchWorkGroup is NULL OR WorkGroup = @vchWorkGroup OR WorkGroup = 'ALL')	--WO#359
					AND (@vchWorkSubGroup is NULL OR WorkSubGroup = @vchWorkSubGroup)	--WO#512
				ORDER BY FirstName,LastName
			END
			ELSE
			BEGIN
				If @vchAction = 'AllGroup_SortByName'
				BEGIN
					SELECT	*, RTRIM(FirstName) + ' ' + RTRIM(LastName) as FullName	
					FROM	 dbo.tblPlantStaff
--WO#359			WHERE	ActiveRecord = 1 
--WO#359				AND Facility = @chrFacility
					WHERE	(@bitActive IS NULL OR ActiveRecord = @bitActive)	--WO#359						AND (@chrFacility is NULL OR Facility = @chrFacility)	--WO#359
					ORDER BY FirstName,LastName, WorkGroup
				END
				ELSE
				IF @vchAction = 'ListByNameWithBlank'
				BEGIN
					SELECT * From (
					SELECT	Staffid, WorkGroup, WorkSubGroup, RTRIM(FirstName) + ' ' + RTRIM(LastName) as FullName 	
					FROM	 dbo.tblPlantStaff
--WO#359			WHERE	ActiveRecord = 1 
--WO#359				AND Facility = @chrFacility
					WHERE	(@bitActive IS NULL OR ActiveRecord = @bitActive)	--WO#359
						AND (@chrFacility is NULL OR Facility = @chrFacility)	--WO#359
						AND (@vchWorkGroup is NULL OR WorkGroup = @vchWorkGroup OR WorkGroup = 'ALL')
						AND (@vchWorkSubGroup is NULL OR WorkSubGroup = @vchWorkSubGroup)	--WO#512
					Union
					SELECT NULL as Staffid,'*ALL' as WorkGroup, '*ALL' as WorkSubGroup, '*ALL' as FullName) t1
					ORDER BY FullName, WorkGroup
				END
			END
		END
	END			
		
END

GO

