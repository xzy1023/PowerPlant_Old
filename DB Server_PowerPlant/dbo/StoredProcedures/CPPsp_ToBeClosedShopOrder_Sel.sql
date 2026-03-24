
-- =============================================
-- Author:		Bong Lee
-- Create date: Mar. 28, 2011
-- Description:	Select Closed Shop Order History 
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_ToBeClosedShopOrder_Sel] 
	@vchAction varchar(50),
	@Facility char(3) = NULL, 
	@ShopOrder int = NULL,
	@DefaultPkgLine char(10) = NULL,
	@Operator varchar(10) = NULL,
	@SessionStartTime datetime = NULL,
	@ClosingTime datetime = NULL,
	@UpdatedToBPCS bit = NULL
AS
BEGIN

	SET NOCOUNT ON;

    IF @vchAction = 'ALL'
	BEGIN
		SELECT * FROM tblToBeClosedShopOrder 
		WHERE (@Facility is NULL OR Facility = @Facility)
			AND (@ShopOrder IS NULL OR ShopOrder = @ShopOrder)
			AND (@DefaultPkgLine IS NULL OR DefaultPkgLine = @DefaultPkgLine)
			AND (@Operator IS NULL OR Operator = @Operator)
			AND (@SessionStartTime IS NULL OR SessionStartTime = @SessionStartTime)
			AND (@ClosingTime IS NULL OR ClosingTime = @ClosingTime)
			AND (@UpdatedToBPCS IS NULL OR UpdatedToBPCS = @UpdatedToBPCS)
	END
END

Grant execute on object:: PPsp_CloseBPCSShopOrders to dataimporter

GO

