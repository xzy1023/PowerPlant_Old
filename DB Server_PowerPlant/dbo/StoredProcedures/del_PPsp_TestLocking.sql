
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[del_PPsp_TestLocking]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BEGIN TRANSACTION
	Update tblCurrentPalletNo set CurrentPalletNo = CurrentPalletNo + 1
	waitfor DELAY '00:00:8'
	select * from tblCurrentPalletNo 
	COMMIT TRANSACTION

END

GO

