



-- =============================================
-- Author:		Bong Lee
-- Create date: Sep. 17, 2008
-- Description:	Control Table I/O Module
-- POAP # 74: Down Time Log Maintenance
--			  Jan 19,2010 Bong Lee
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_Control_Sel]
	-- Add the parameters for the stored procedure here
	@vchKey varchar(50) =NULL, 
	@vchSubKey varchar(50) = NULL,
	@vchAction varchar(50) = NULL

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @vchAction = 'BySubKey'
		Select * from tblControl where subKey = @vchSubKey Order by [Key]
	ELSE
		IF @vchAction = 'ByKey'
			SELECT * from tblControl where [Key] = @vchKey 
		ELSE
			SELECT *
		--	SELECT     [Key], SubKey, Description, Value1, Value2
			FROM         tblControl
			WHERE     ([Key] = @vchKey) AND							-- POAP#74
					 (@vchSubKey is NULL OR SubKey = @vchSubKey)	-- POAP#74
-- POAP#74 			WHERE     ([Key] = @vchKey) AND (SubKey = @vchSubKey)
END

GO

