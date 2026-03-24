


-- =============================================
-- Author:		Bong Lee
-- Create date: Nov 14, 2006
-- Description:	Edit Component Scrap
-- WO#359		Jan. 25, 2012	Bong Lee
--              Add Action = 'ALLBySO+OtherScrap' to show Mix Scrap & Lab. Scrap
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_EditComponentScrap]
	-- Add the parameters for the stored procedure here
	@chrFacility char(3), 
	@intShopOrder int = 0,
	@dteStartTime datetime,
	@vchComponent varchar(35),
	@decQuantity  decimal(8,2) = 0, 
	@vchAction varchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @vchAction = 'ALLBySO'
-- WO#359 Del Start
--		SELECT   T1.Facility,T1.SequenceNo,T1.ComponentItem, T2.ItemDesc1,T1.UnitOfMeasure,
--					(SELECT T3.Quantity FROM dbo.tblComponentScrap AS T3
--						WHERE (T3.Facility = @chrFacility) AND (T3.ShopOrder = @intShopOrder) AND 
--							  (T3.StartTime = @dteStartTime) AND (T3.Component= T1.ComponentItem)) AS Quantity
--		FROM    tblBillOfMaterials AS T1 
--		LEFT OUTER JOIN tblItemMaster AS T2 
--		ON T1.Facility = T2.Facility AND T1.ComponentItem = T2.ItemNumber		
--		WHERE T1.ShopOrder =  @intShopOrder
--		ORDER BY T1.SequenceNo
-- WO#359 Del Stop
-- WO#359 Add Start
		SELECT T4.Facility,T4.SequenceNo,T4.ComponentItem,T4.ItemDesc1,T4.UnitOfMeasure, tCS.Quantity  FROM 
			(SELECT @chrFacility as Facility, 0 as SequenceNo, @intShopOrder as ShopOrder, 'LabScrap' as ComponentItem, 'Lab. Scrap' as ItemDesc1, 'LB' as UnitOfMeasure
			UNION
			SELECT tBOM.Facility,tBOM.SequenceNo,tBOM.ShopOrder, tBOM.ComponentItem,tIM.ItemDesc1,tBOM.UnitOfMeasure 
			FROM tblBillOfMaterials tBOM
			LEFT OUTER JOIN tblItemMaster AS tIM 
			ON tBOM.Facility = tIM.Facility AND tBOM.ComponentItem = tIM.ItemNumber	
			WHERE tBOM.Facility = @chrFacility AND tBOM.ShopOrder =  @intShopOrder) T4
		LEFT OUTER JOIN dbo.tblComponentScrap AS tCS
		ON (tCS.Facility = T4.Facility) AND (tCS.ShopOrder = T4.ShopOrder) AND 
		   (tCS.StartTime = @dteStartTime) AND (tCS.Component= T4.ComponentItem)
		ORDER BY T4.SequenceNo
-- WO#359 Add Stop
	ELSE
-- WO#359 Add Start
	IF @vchAction = 'ALLBySO+OtherScrap'
		SELECT T4.Facility,T4.SequenceNo,T4.ComponentItem,T4.ItemDesc1,T4.UnitOfMeasure, tCS.Quantity  FROM 
			(SELECT @chrFacility as Facility, -1 as SequenceNo, @intShopOrder as ShopOrder, 'MixScrap' as ComponentItem, 'Mixed Scrap' as ItemDesc1, 'LB' as UnitOfMeasure
			UNION
			SELECT @chrFacility as Facility, 0 as SequenceNo, @intShopOrder as ShopOrder, 'LabScrap' as ComponentItem, 'Lab. Scrap' as ItemDesc1, 'LB' as UnitOfMeasure
			UNION
			SELECT tBOM.Facility,tBOM.SequenceNo,tBOM.ShopOrder, tBOM.ComponentItem,tIM.ItemDesc1,tBOM.UnitOfMeasure 
			FROM tblBillOfMaterials tBOM
			LEFT OUTER JOIN tblItemMaster AS tIM 
			ON tBOM.Facility = tIM.Facility AND tBOM.ComponentItem = tIM.ItemNumber	
			WHERE tBOM.Facility = @chrFacility AND tBOM.ShopOrder =  @intShopOrder) T4
		LEFT OUTER JOIN dbo.tblComponentScrap AS tCS
		ON (tCS.Facility = T4.Facility) AND (tCS.ShopOrder = T4.ShopOrder) AND 
		  (tCS.StartTime = @dteStartTime) AND (tCS.Component= T4.ComponentItem)
		ORDER BY T4.SequenceNo
-- WO#359 Add Stop
	ELSE
	Begin
		If @vchAction = 'Edit'
	
			IF EXISTS (SELECT 1 from dbo.tblComponentScrap 
						WHERE (facility = @chrFacility) AND (ShopOrder = @intShopOrder) AND
							  (StartTime = @dteStartTime) AND (Component = @vchComponent))	
			Begin
				If @decQuantity = 0
					DELETE FROM dbo.tblComponentScrap
						WHERE (facility = @chrFacility) AND (ShopOrder = @intShopOrder) AND
							  (StartTime = @dteStartTime) AND (Component = @vchComponent)
				ELSE
					UPDATE dbo.tblComponentScrap SET Quantity = @decQuantity 
						WHERE (facility = @chrFacility) AND (ShopOrder = @intShopOrder) AND
							  (StartTime = @dteStartTime) AND (Component = @vchComponent)	
			End
			Else
			BEGIN
				If @decQuantity <> 0
				INSERT INTO dbo.tblComponentScrap
						   (facility, ShopOrder, StartTime, Component, Quantity)
					VALUES (@chrFacility,@intShopOrder,@dteStartTime,@vchComponent,@decQuantity)
			END	
	END
END

GO

