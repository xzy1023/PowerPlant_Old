-- =============================================
-- Author:		Bong Lee
-- Create date: Sep.8 2006
-- Description:	Update download table
-- =============================================
CREATE PROCEDURE [ppsp_UpDownloadLog] 
	-- Add the parameters for the stored procedure here
	@strAction nvarchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if @strAction = 'Start'
	Begin
	INSERT INTO tblDB2ToSQLDownloadLog (StartTime) VALUES (GETDATE())
    End                  
			
END

GO

