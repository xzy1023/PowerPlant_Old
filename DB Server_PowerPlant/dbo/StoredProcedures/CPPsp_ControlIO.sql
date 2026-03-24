


-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 1, 2006
-- Description:	Control Table I/O Module
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_ControlIO]
	-- Add the parameters for the stored procedure here
	@vchKey varchar(50), 
	@vchSubKey varchar(50) = NULL

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT     [Key], SubKey, Description, Value1, Value2
	FROM         tblControl
	WHERE     ([Key] = @vchKey) AND (SubKey = @vchSubKey)
END

GO

