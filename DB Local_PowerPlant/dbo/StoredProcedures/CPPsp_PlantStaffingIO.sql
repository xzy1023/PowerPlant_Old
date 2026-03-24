
-- =============================================
-- Author:		Bong Lee
-- Create date: Sept. 21, 2006
-- Description:	Operation Staffing I/O Module
-- WO#17432:	Aug. 16, 2018	Bong Lee
-- Description: Add the selection for Staff Class
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_PlantStaffingIO]
	-- Add the parameters for the stored procedure here
	@vchAction varchar(30) = NULL,
	@vchStaffID varchar(10) = NULL,
	@vchWorkGroup varchar(10) = NULL
	,@bitActiveRecord bit = NULL												-- WO#17432
	,@vchStaffClass varchar(50) = NULL											-- WO#17432

AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	If @vchAction = 'SelectAllFields'
	BEGIN
		SELECT	*, RTRIM(FirstName) + ' ' + RTRIM(LastName) as FullName	
		FROM	 dbo.tblPlantStaff
		-- WO#17432	WHERE	StaffID = @vchStaffID
			-- WO#17432	AND	(WorkGroup = @vchWorkGroup
			-- WO#17432	OR  WorkGroup = 'ALL')
		WHERE																			-- WO#17432
			StaffID = ISNULL(@vchStaffID,StaffID)										-- WO#17432
			AND (WorkGroup = ISNULL(@vchWorkGroup,WorkGroup ) OR WorkGroup = 'ALL')		-- WO#17432
			AND	(StaffClass	= ISNULL(@vchStaffClass,StaffClass))						-- WO#17432
			AND	(ActiveRecord = ISNULL(@bitActiveRecord,ActiveRecord))					-- WO#17432
	END
END

GO

