
-- =============================================
-- Author:		Bong Lee
-- Create date: Dec 08,2011
-- Description:	Reconcile Cases Produced by Date Period
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ReconcileCasesProducedByDate]
	@vchFacility varchar(3),
	@dteFromProdDate as DateTime,
	@intFromShift as tinyint,
	@dteToProdDate as DateTime,
	@intToShift as tinyint
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRY
	
	;With cte_SCH 
	AS
	(
		SELECT * FROM dbo.tfnSessionControlHstDetail(NULL, @vchFacility, NULL, NULL, NULL, NULL, @dteFromProdDate, CAST(@intFromShift as char(1)), @dteToProdDate, CAST(@intToShift as char(1)))
	),
	cte_PH
	AS
	(
		SELECT * FROM dbo.tfnPalletHstDetail(NULL, @vchFacility, NULL, NULL, NULL, NULL, @dteFromProdDate, CAST(@intFromShift as char(1)), @dteToProdDate, CAST(@intToShift as char(1)))
	)

	SELECT tP_SC.Facility, tP_SC.ShopOrder
			,tP_SC.ItemNumber
			,tP_SC.CasesFromPallet
			,tP_SC.CasesProd
			,tP_SC.Diff + ISNULL(tPA.AdjPalletOnly,0) + ISNULL(tPA.TopUp,0) - tCF.CarriedForwardCases as Diff -- WO#359
			,tCF.CarriedForwardCases
			,ISNULL(tPA.AdjBoth,0) as AdjBoth				-- WO#359
			,ISNULL(tPA.AdjPalletOnly,0) as AdjPalletOnly	-- WO#359
			,ISNULL(tPA.TopUp,0) as TopUp					-- WO#359
			,tIM.ItemDesc1 + ' ' + tIM.ItemDesc2 as Description 
			,ShopOrderClosedTime	-- WO#194
	FROM
	(
		-- Calculate their cases produced and difference from pallet history and session control history
		Select ISNULL(tP.Facility, tSC.Facility) as Facility, ISNULL(tP.ShopOrder, tSC.ShopOrder) as ShopOrder,
			   ISNULL(tP.ItemNumber, tSC.ItemNumber) as ItemNumber, tP.PQty as CasesFromPallet, tSC.CasesProd, tP.PQty-tSC.CasesProd as Diff,
			   ISNULL(tTBCSO.ShopOrderClosingTime ,ISNULL(tCSOH.ShopOrderClosingTime, tCP.CreationDateTime)) as ShopOrderClosedTime	-- WO#194
		From 
		-- Include Pallets in history and in power plant that is ready to post to BPCS
		(Select tPH.Facility, tPH.ShopOrder, tPH.ItemNumber, Sum(ISNULL(tPH.Quantity,0)) as PQty
				, MAX(tPH.CreationDateTime) as LastPalletCreationTime
		 From cte_PH tPH
			Group by tPH.Facility, tPH.ShopOrder,tPH.ItemNumber) tP
		FULL JOIN
		-- cases produced in session control history already includes loose cases
		(Select tSCH.Facility, tSCH.ShopOrder, tSCH.ItemNumber, Sum(tSCH.casesproduced) as CasesProd 
			From cte_SCH tSCH
			Group by tSCH.Facility, tSCH.ShopOrder, tSCH.ItemNumber) tSC
		On tP.ShopOrder = tSC.ShopOrder
		-- Get the creatation date of order complete of the shop order from pallet files
		LEFT OUTER JOIN
		(Select tPHst.ShopOrder, tPHst.CreationDateTime 
				From cte_PH tPHst
				Where tPHst.OrderComplete = 'Y' 
			) tCP
		On tP.ShopOrder = tCP.ShopOrder AND tP.LastPalletCreationTime =  tCP.CreationDateTime
		LEFT OUTER JOIN
			(SELECT ShopOrder, Max(BPCSClosingTime) as ShopOrderClosingTime FROM  tblClosedShopOrderHst GROUP BY ShopOrder) tCSOH
		On tP.ShopOrder = tCSOH.ShopOrder
		LEFT OUTER JOIN
			(SELECT ShopOrder, Max(ClosingTime) as ShopOrderClosingTime FROM  tblToBeClosedShopOrder GROUP BY ShopOrder) tTBCSO
		On tP.ShopOrder = tTBCSO.ShopOrder

	) tP_SC
	Left Outer Join
		(SELECT ShopOrder, ISNULL([01],0) as AdjBoth, ISNULL([26],0) AS AdjPalletOnly, ISNULL([27],0) AS TopUp FROM
			(SELECT tA.ShopOrder, tA.LatestReasonCode AS TransactionReasonCode, tA.AdjustedQty	
				FROM tfnPalletAdjustment('AdjWithCorrectionOnly', @vchFacility, NULL, NULL, NULL, NULL, NULL, NULL, NULL ) tA
				Inner join cte_PH tPH
				On tA.PalletID = tPH.PalletID) tAJ
				PIVOT
				(
				SUM (AdjustedQty)
				FOR TransactionReasonCode IN
				( [01], [26], [27] )
			) AS pvt)  tPA
	On tP_SC.shoporder = tPA.ShopOrder

	Left Outer Join
	-- Get carried forwrad cases from the first record of the shop order in the session control history 
	(Select tCFByLine.ShopOrder, Sum(tCFByLine.CarriedForwardCases) as CarriedForwardCases 
		FROM 
		(Select tSCH1.ShopOrder, tSCH1.DefaultPkgLine, tSCH1.CarriedForwardCases 
			From cte_SCH tSCH1
		Inner join 	
		(Select tSCH.ShopOrder, tSCH.DefaultPkgLine, Min(tSCH.StartTime) as FirstStartTime
			From cte_SCH tSCH
			Group By tSCH.ShopOrder, tSCH.DefaultPkgLine) tCFSOL
		On tSCH1.ShopOrder = tCFSOL.ShopOrder and 
			tSCH1.DefaultPkgLine = tCFSOL.DefaultPkgLine and 
			tSCH1.StartTime = tCFSOL.FirstStartTime) tCFByLine
		group by tCFByLine.ShopOrder)  tCF
	On tP_SC.ShopOrder = tCF.ShopOrder
	-- Get item description from item master table
	Left outer join	tblItemMaster tIM 
	On tP_SC.Facility = tIM.Facility and tP_SC.ItemNumber = tIM.ItemNumber
	Where tP_SC.Diff + ISNULL(tPA.AdjPalletOnly,0)  + ISNULL(tPA.TopUp,0) - tCF.CarriedForwardCases <> 0 -- WO#359
-- WO#359	Where tP_SC.Diff - tCF.CarriedForwardCases <> 0	-- WO#194
-- WO#194	Where tP_SC.Diff + tL.LooseCases - tCF.CarriedForwardCases <> 0
	-- Pallet history does not have the loose cases, so add the loose cases to pallet history to calculate the diff.
	-- Carried forward cases do not produced in the session but in the pallet so it needs to subtract the CF cases from the diff.

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();

		-- Use RAISERROR inside the CATCH block to return error
		-- information about the original error that caused
		-- execution to jump to the CATCH block.
		RAISERROR (@ErrorMessage, -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH;
END

GO

