

-- =============================================
-- Author:		Bong Lee
-- Create date: March 05, 2010
-- Description:	Select Shop Order History Table
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ShopOrderHst_Sel] 
	-- Add the parameters for the stored procedure here
	@vchAction varchar(50) , 
	@vchFacility varchar(3),
	@intShopOrder int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	If @vchAction = 'BySO'
	Begin
	SELECT *,
		(CONVERT(datetime, (CONVERT(char(10),tSOH.StartDate)),112) + dbo.fnCvtNumTimeToDateTime(tSOH.StartTime)) AS StartDateTime,
		(CONVERT(datetime, (CONVERT(char(10),tSOH.EndDate)),112) + dbo.fnCvtNumTimeToDateTime(tSOH.EndTime)) AS EndDateTime
		From dbo.tblShopOrderHst AS tSOH 
		LEFT OUTER JOIN tblItemMaster AS tIM 
		ON tSOH .ItemNumber = tIM.ItemNumber
		Where tSOH.Facility = @vchFacility And tSOH.ShopOrder = @intShopOrder
	End

END

GO

