
-- =============================================
-- Author:		Bong Lee
-- Create date: Aug 13,2009
-- Description:	Shop Order Case Produced Variance
-- WO#194:		Sep. 9, 2010	Bong Lee	
-- Description:	If the time frame for the expected shift crosses mid-night
--				and the given time is prior to mid-night, the Shift Production 
--				Date will be the given time plus one day. Before this change the 	
--				Shift Production Date for this case was same as the given time but 
--				if the given time after mid-night, the result was given time 
--				minus one day.
--				Add columns: Time Closed
-- WO#359:		Sep. 29, 2011	Bong Lee
-- Description:	Use standard table functions to replace some of the select statements
--				Apply pallet adjustment to cases produced
-- Task#6631:	Sep. 14, 2015	Bong Lee
-- Description:	Use PPsp_ShopOrderClosingTime to get the shop order closing time.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ShopOrderCasesProducedVariance]
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

--Task#6631 Add Start
	DECLARE @intFmShopOrder as int;
	DECLARE @intToShopOrder as int;

	IF object_id('tempdb..#tSOCD') is not null
		DROP TABLE #tSOCD

	CREATE TABLE #tSOCD
		(
			[Facility] [varchar](3)
			,[ShopOrder] [int]
			,ClosingDate datetime 
		);

	IF object_id('tempdb..#tSOList') is not null
		  DROP TABLE #tSOList
	CREATE TABLE #tSOList
		(
			[Facility] [varchar](3)
			,[ShopOrder] [int]
		);

	Insert into #tSOList
	SELECT Facility, ShopOrder
			FROM tfnSessionControlHstDetail (NULL,@vchFacility, NULL, NULL, NULL, NULL, @dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift)
			GROUP BY Facility, ShopOrder

	SELECT @intFmShopOrder =  Min(ShopOrder), @intToShopOrder =  Max(ShopOrder)
			FROM #tSOList

	Insert into #tSOCD
	EXEC PPsp_ShopOrderClosingDate @vchFacility, @intFmShopOrder, @intToShopOrder
--Task#6631 Add Stop

-- WO#194 Add Start 
		/* Find the Work Shift type for each line - if line is duplicated, pick the active one */
-- WO#359 Del Start 
--		Declare @tblComputerConfig TABLE
--		(Facility varchar(3) ,Packagingline varchar(10), WorkShiftType varchar(10))
--
--
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
--		WHERE (T2.Packagingline is null OR T1.RecordStatus = 1) 
--			AND T1.PackagingLine <> 'SPARE'
--		Group By T1.Facility, T1.Packagingline, T1.WorkShiftType;
-- WO#359 Del Stop 
-- Select all shop orders in the selected time period and facility.
--WO#xxx		;With cteSOList As
--WO#xxx		(
--WO#xxx			SELECT tSCH.Facility, tSCH.ShopOrder
--WO#xxx			FROM tfnSessionControlHstSummary ('WithAdjByLineSO',@vchFacility, NULL, NULL, NULL, NULL, @dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift) tSCH -- WO#359
-- WO#359 Del Start 
--			FROM tblSessionControlHst tSCH
--			Left Outer Join @tblComputerConfig tCC
--			ON tSCH.Facility = tCC.Facility AND tSCH.OverridePkgLine =  tCC.PackagingLine
--			Left Outer Join tblShift tS
--			ON tSCH.Facility = tS.Facility AND tSCH.OverrideShiftNo = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
--			WHERE tSCH.Facility = @vchFacility and tSCH.ShiftProductionDate is not null		-- WO#194
--				AND Convert(varchar(8),tSCH.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) AND convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
-- WO#359 Del Stop
		--Task#6631	GROUP BY tSCH.Facility, tSCH.ShopOrder
		--Task#6631)
 
-- WO#194 Add Start 
/* WO#194 Del Start 
		With cteSCH (Facility,ShopOrder,ItemNumber,CasesProduced) 
		AS (
			Select tSCH.Facility, tSCH.ShopOrder, tSCH.ItemNumber, Sum(tSCH.CasesProduced) as CasesProduced
			From tblSessionControlHst tSCH
			Inner join
			(Select Facility, ShopOrder
				From tblSessionControlHst 
				Where Facility = @vchFacility and 
				Convert(varchar(8),ShiftProductionDate,112) + Cast(OverrideShiftNo as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1)) AND
				(Facility not in (Select facility from dbo.tblFacility where Region = '01') OR Len(Rtrim(Operator)) > 3) 
				Group by Facility, ShopOrder
				 ) tSO
			On tSCH.Facility = tSO.Facility AND tSCH.ShopOrder = tSO.ShopOrder
			Group by tSCH.Facility, tSCH.ShopOrder, tSCH.ItemNumber 
		)
WO#194 Del Start */

		SELECT cteSCH.Facility, cteSCH.ShopOrder, cteSCH.ItemNumber, cteSCH.CasesProduced, tSOH.OrderQty
			,tCP.LastPalletCreationTime		-- WO#194 
		FROM 
-- WO#194 Add Start 
			(SELECT tSCHst.Facility, tSCHst.ShopOrder, tSCHst.ItemNumber, Sum(tSCHst.CasesProduced) as CasesProduced
				FROM tblSessionControlHst tSCHst
				INNER JOIN #tSOList tSOList
				ON tSCHst.Facility = tSOList.Facility AND tSCHst.ShopOrder = tSOList.ShopOrder
				--INNER JOIN cteSOList
				--ON tSCHst.Facility = cteSOList.Facility AND tSCHst.ShopOrder = cteSOList.ShopOrder
			GROUP BY tSCHst.Facility, tSCHst.ShopOrder, tSCHst.ItemNumber
			) cteSCH
		LEFT OUTER JOIN
			(SELECT  Facility, ShopOrder, ClosingDate as LastPalletCreationTime FROM #tSOCD) tCP	--Task#6631
			--Task#6631 DEL Start
			--(SELECT  tAP.Facility, tAP.ShopOrder, OrderComplete, MAX(tAP.CreationDateTime) as LastPalletCreationTime
			--FROM
			--	(SELECT tPH.* FROM dbo.tblPalletHst tPH
			--		INNER JOIN cteSOList					
			--		ON tPH.Facility = cteSOList.Facility AND tPH.ShopOrder = cteSOList.ShopOrder
			--	UNION
			--		SELECT tPt.* FROM dbo.tblPallet tPt
			--		) tAP 
			--GROUP BY tAP.Facility, tAP.ShopOrder, tAP.OrderComplete
			--HAVING tAP.OrderComplete = 'Y'
			-- ) tCP
			--Task#6631 DEL Stop
		ON cteSCH.Facility = tCP.Facility AND cteSCH.ShopOrder = tCP.ShopOrder
-- WO#194 Add End 
-- WO#194	From cteSCH tblSessionControlHst
		Left outer join tblShopOrderhst tSOH
		ON cteSCH.Facility = tSOH.Facility AND cteSCH.ShopOrder = tSOH.ShopOrder
		ORDER BY cteSCH.ShopOrder

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		--Task#6631 Add Start
		DECLARE @ErrorProcedure nvarchar(200);
		DECLARE @ErrorLine int;
		DECLARE @ErrorNumber int;
		--Task#6631 Add Stop

		SELECT 

			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			--Task#6631 @ErrorState = ERROR_STATE();
			--Task#6631 Add Start
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ' Message: ' + CHAR(13) + ERROR_MESSAGE();
			--Task#6631 Add Stop

		-- Use RAISERROR inside the CATCH block to return error
		-- information about the original error that caused
		-- execution to jump to the CATCH block.
		/* Task#6631 Add Start
		RAISERROR (@ErrorMessage, -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
		Task#6631 Add Start */
		RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)
	END CATCH;
END

GO

