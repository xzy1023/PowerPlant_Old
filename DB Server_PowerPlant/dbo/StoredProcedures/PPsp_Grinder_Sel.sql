

-- =============================================
-- Author:		Bong Lee
-- Create date: Feb 06, 2008
-- Description:	Select Grinder 
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_Grinder_Sel] 
	@vchAction varchar(30),
	@chrFacility char(3)
AS
BEGIN
	SET NOCOUNT ON;
	IF @vchAction = 'AllWithBlank'
		SELECT '' as Description,NULL as Grinder 
		Union 
		SELECT Grinder as Description,Grinder FROM tblGrinderRate WHERE Facility = @chrFacility GROUP BY Grinder
	ELSE
		IF @vchAction = 'All'
			SELECT Grinder as Description,Grinder FROM tblGrinderRate WHERE Facility = @chrFacility GROUP BY Grinder
END

GO

