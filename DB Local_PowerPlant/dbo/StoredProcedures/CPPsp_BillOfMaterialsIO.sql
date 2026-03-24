
-- =============================================
-- Author:		Bong Lee
-- Create date: Nov 06, 2006
-- Description:	Bill Of Material IO module
-- WO#871:		Mar 19, 2014
-- Description:	Add an Component Item pararmeter to allow selecting by component
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_BillOfMaterialsIO]
	@intShopOrder int,
	@vchAction varchar(30) = NULL,
	@intCaseQty int = 0
	,@vchComponentItem varchar(35) = NULL		--WO#871
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	If @vchAction = 'ALL'
		SELECT     *
		FROM    tblBillOfMaterials AS T1 
		LEFT OUTER JOIN tblItemMaster AS T2 ON T1.Facility = T2.Facility AND T1.ComponentItem = T2.ItemNumber
		WHERE T1.ShopOrder =  @intShopOrder
			AND (T1.ComponentItem = @vchComponentItem OR @vchComponentItem Is NULL)		--WO#871
		ORDER BY T1.SequenceNo
	Else
		-- The quantity will be BOM qty * # of cases
		IF @vchAction = 'TotalBOM'
			SELECT     T1.Facility, T1.ShopOrder, T1.SequenceNo, T1.ComponentItem, T1.Quantity * @intCaseQty as Quantity, T1.UnitOfMeasure, T2.ItemDesc1
		FROM    tblBillOfMaterials AS T1 
		LEFT OUTER JOIN tblItemMaster AS T2 ON T1.Facility = T2.Facility AND T1.ComponentItem = T2.ItemNumber
		WHERE T1.ShopOrder =  @intShopOrder
			AND (T1.ComponentItem = @vchComponentItem OR @vchComponentItem Is NULL)		--WO#871
		ORDER BY T1.SequenceNo
END

GO

