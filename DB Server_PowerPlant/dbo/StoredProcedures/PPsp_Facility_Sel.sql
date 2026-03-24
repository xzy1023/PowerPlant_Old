

-- =============================================
-- Author:		Bong Lee
-- Create date: Feb 06, 2008
-- Description:	Select Facility 
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_Facility_Sel] 
	@vchAction varchar(30)= NULL,
	@vchOrderBy varchar(30) =NULL
AS
BEGIN
	
	SET NOCOUNT ON;
	DECLARE @vchSQLStmt as varchar(500);
	
	IF @vchAction = 'SelByRegion'
		SET @vchSQLStmt = 'SELECT tblFacility.* FROM tblFacility INNER JOIN
			tblControl ON tblFacility.Region = tblControl.Value1
			WHERE tblControl.[Key] =''Facility'' AND tblControl.SubKey = ''General'''
	ELSE
		SET @vchSQLStmt = 'SELECT * FROM tblFacility'

	IF @vchOrderBy = 'Desc' 
		SET @vchSQLStmt = @vchSQLStmt + ' ORDER BY Description'
	ELSE
		IF @vchOrderBy = 'ShortDesc'
			SET @vchSQLStmt = @vchSQLStmt + ' ORDER BY ShortDescription'
	ELSE
		SET @vchSQLStmt = @vchSQLStmt + ' ORDER BY Facility'

	EXECUTE (@vchSQLStmt)
END

GO

