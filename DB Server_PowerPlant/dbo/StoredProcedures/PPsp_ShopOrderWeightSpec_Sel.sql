-- =============================================
-- Author:		Bong Lee
-- Create date: Sep. 14, 2009
-- Description:	Select Shop Order Weight Specification
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ShopOrderWeightSpec_Sel] 
	-- Add the parameters for the stored procedure here
	@vchAction varchar(50) , 
	@vchFacility varchar(3),
	@intShopOrder int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	If @vchAction = 'BySO'
	Begin
	SELECT * From dbo.tblShopOrderWeightSpecHst Where Facility = @vchFacility And ShopOrder = @intShopOrder
	End
END

GO

