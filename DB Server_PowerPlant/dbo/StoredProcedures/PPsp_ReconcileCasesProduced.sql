





-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 25,2009
-- Description:	Reconcile Cases Produced
-- WO#194:		Sep. 9, 2010	Bong Lee	
-- Description:	If the time frame for the expected shift crosses mid-night
--				and the given time is prior to mid-night, the Shift Production 
--				Date will be the given time plus one day. Before this change the 	
--				Shift Production Date for this case was same as the given time but 
--				if the given time after mid-night, the result was given time 
--				minus one day.
--				Add columns: Time Closed
-- WO#297:		Mar. 22, 2011	Bong Lee	
-- Description:	Use the latest shop order closing date from the pallet/pallethst table
--              if it is not found from the tblClosedShopOrderHist.
-- WO#359:		May 19, 2011	Bong Lee	
-- Description:	Make use of the generic session control history table function to extra data.
--              segreate the pallet adjustment by reason code.
--				Case Produced discrepancy if Diff + AdjPalletOnly - CarriedForwardCases <> 0
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ReconcileCasesProduced]
	@vchFacility varchar(3),
	@dteFromProdDate as DateTime,
	@intFromShift as tinyint,
	@dteToProdDate as DateTime,
	@intToShift as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRY;

-- WO#194 Add Start 
		/* Find the Work Shift type for each line - if line is duplicated, pick the active one */
-- WO#359 Del Start 
--		Declare @tblComputerConfig Table
--		(Facility varchar(3) ,Packagingline varchar(10), WorkShiftType varchar(10))
--		INSERT INTO @tblComputerConfig
--		SELECT T1.Facility, T1.Packagingline, T1.WorkShiftType 
--		FROM tblcomputerconfig T1 
--		Left Outer Join 
--			(SELECT Facility, Packagingline 
--				FROM tblComputerconfig 
--			WHERE Packagingline <> 'SPARE'
--			Group By Facility, Packagingline
--			Having Count(*) > 1) T2
--		ON T1.Facility = T2.Facility AND T1.Packagingline = T2.Packagingline
--		WHERE t2.Packagingline is null OR RecordStatus = 1
--		Group By T1.Facility, T1.Packagingline, T1.WorkShiftType
-- WO#359 Del Start 
-- WO#194 Add Stop

	-- Select all shop orders in the selected time period and facility.
	;With SOList(ShopOrder) As
	(
 		SELECT ShopOrder 
		FROM dbo.tfnSessionControlHstSummary('WithoutAdj',@vchFacility ,NULL,NULL,NULL,NULL,@dteFromProdDate, @intFromShift,	@dteToProdDate,	@intToShift) -- WO#359
-- WO#359 Del Start 
-- WO#194 From dbo.tblsessionControlHst

-- WO#194 Add Start 
--		FROM tblsessionControlHst tSCH
--		Left Outer Join @tblComputerConfig tCC
--		ON tSCH.Facility = tCC.Facility AND tSCH.OverridePkgLine =  tCC.PackagingLine
--		Left Outer Join tblShift tS
--		ON tSCH.Facility = tS.Facility AND tSCH.OverrideShiftNo = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
-- WO#194 Add Stop
-- WO#194	Where Facility = @vchFacility and ShiftProductionDate is not null 
-- WO#194	and Convert(varchar(8),ShiftProductionDate,112) + Cast(OverrideShiftNo as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
--		Where tSCH.Facility = @vchFacility and tSCH.ShiftProductionDate is not null		-- WO#194
--			and Convert(varchar(8),tSCH.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))	-- WO#194
		Group by ShopOrder
-- WO#359 Del Start 
	)

	SELECT tP_SC.Facility, tP_SC.ShopOrder
			,tP_SC.ItemNumber
			,tP_SC.CasesFromPallet
			,tP_SC.CasesProd
-- WO#359	,tP_SC.Diff  - tCF.CarriedForwardCases as Diff, 
			,tP_SC.Diff + ISNULL(tPA.AdjPalletOnly,0) + ISNULL(tPA.TopUp,0) - tCF.CarriedForwardCases as Diff -- WO#359
-- WO#194	tL.LooseCases, 
			,tCF.CarriedForwardCases
-- WO#359	,ISNULL(tPA.AdjQty,0) as AdjustedQty
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
		(Select tAP.Facility, tAP.ShopOrder, tAP.ItemNumber, Sum(ISNULL(tAP.Quantity,0)) as PQty
				, MAX(CreationDateTime) as LastPalletCreationTime	-- WO#194
		 From
			-- Include Pallets in history and in power plant that is ready to post to BPCS
			(Select tPH.* From dbo.tblPalletHst tPH
			Inner join SOList tSO
			on tPH.ShopOrder = tSO.ShopOrder
			Union
			Select tPt.* From dbo.tblPallet tPt
			Inner join SOList tSO
			on tPt.ShopOrder = tSO.ShopOrder) tAP 
		Group by tAP.Facility, tAP.ShopOrder,tAP.ItemNumber) tP
		FULL JOIN
		-- cases produced in session control history already includes loose cases
			(Select tSCH.Facility, tSCH.ShopOrder, tSCH.ItemNumber, Sum(tSCH.casesproduced) as CasesProd 
			From dbo.tblSessionControlHst tSCH
			Inner join SOlist tSO
			on tSCH.ShopOrder = tSO.ShopOrder
			Group by tSCH.Facility, tSCH.ShopOrder, tSCH.ItemNumber) tSC
		On tP.ShopOrder = tSC.ShopOrder
-- WO#194 Add Start
		LEFT OUTER JOIN
			(Select tPHst.ShopOrder, tPHst.CreationDateTime 
					From dbo.tblPalletHst tPHst
					Inner join SOList tSO
					On tPHst.ShopOrder = tSO.ShopOrder	
					Where tPHst.OrderComplete = 'Y' 
				Union
			 Select ShopOrder, CreationDateTime From tblPallet Where OrderComplete = 'Y'
			) tCP
		On tP.ShopOrder = tCP.ShopOrder AND tP.LastPalletCreationTime =  tCP.CreationDateTime
		LEFT OUTER JOIN
			(SELECT ShopOrder, Max(BPCSClosingTime) as ShopOrderClosingTime FROM  tblClosedShopOrderHst GROUP BY ShopOrder) tCSOH
		On tP.ShopOrder = tCSOH.ShopOrder
		LEFT OUTER JOIN
			(SELECT ShopOrder, Max(ClosingTime) as ShopOrderClosingTime FROM  tblToBeClosedShopOrder GROUP BY ShopOrder) tTBCSO
		On tP.ShopOrder = tTBCSO.ShopOrder
-- WO#194 Add Stop
	) tP_SC
-- WO#194 Del Start
--	Left Outer Join 
--	-- Get loose cases from the last record of the shop order in the session control history 
--		(Select tSCH1.ShopOrder, tSCH1.LooseCases From dbo.tblSessionControlHst tSCH1
--		Inner join 	
--		(Select tSCH.ShopOrder, Max(tSCH.StartTime) as LastStartTime
--		From dbo.tblSessionControlHst tSCH
--		Inner join SOlist tSO
--		on tSCH.ShopOrder = tSO.ShopOrder
--		Group By tSCH.ShopOrder) tSOL
--		On tSCH1.ShopOrder = tSOL.ShopOrder and tSCH1.StartTime = tSOL.LastStartTime) tL
--	On tP_SC.ShopOrder = tL.ShopOrder
-- WO#194 Del Stop
	Left Outer Join
	-- Get the cancelled cases of shop orders in BPCS from the Pallet Adjustment table
-- WO#359 Del Start
--		(Select tA.ShopOrder, Sum(tA.AdjustedQty) as AdjQty 
--		  From tblPalletAdjustment tA
--		  Inner join SOlist tSO 
--		  On tA.shoporder = tSO.ShopOrder
--		  Group by tA.ShopOrder) tPA
-- WO#359 Del Stop
-- WO#359 Add Start
		(SELECT ShopOrder, ISNULL([01],0) as AdjBoth, ISNULL([26],0) AS AdjPalletOnly, ISNULL([27],0) AS TopUp FROM
			(SELECT tA.ShopOrder, tA.LatestReasonCode AS TransactionReasonCode, tA.AdjustedQty	
				FROM tfnPalletAdjustment('AdjWithCorrectionOnly', @vchFacility, NULL, NULL, NULL, NULL, NULL, NULL, NULL ) tA
				Inner join SOlist tSO 
				On tA.shoporder = tSO.ShopOrder) tAJ
				PIVOT
				(
				SUM (AdjustedQty)
				FOR TransactionReasonCode IN
				( [01], [26], [27] )
			) AS pvt)  tPA
	On tP_SC.shoporder = tPA.ShopOrder
-- WO#359 Del Stop
	Left Outer Join
	-- Get carried forwrad cases from the last record of the shop order in the session control history 
		(Select tSCH1.ShopOrder, tSCH1.CarriedForwardCases From dbo.tblSessionControlHst tSCH1
		Inner join 	
		(Select tSCH.ShopOrder, Min(tSCH.StartTime) as FirstStartTime
			From dbo.tblSessionControlHst tSCH
			Inner join SOlist tSO
			on tSCH.ShopOrder = tSO.ShopOrder
			Group By tSCH.ShopOrder) tCFSOL
		On tSCH1.ShopOrder = tCFSOL.ShopOrder and tSCH1.StartTime = tCFSOL.FirstStartTime) tCF
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

