
-- =============================================
-- Author:		Bong Lee
-- Create date: Apr.06 2010
-- Description:	Work Center Efficency Summary
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ShopOrderAdherence]
	@vchFacility varchar(3)
	,@chrShowShift as char = 'Y'
	,@intShiftSeqFilter as tinyint = NULL
	,@dteFromProdDate as DateTime
	,@intFromShiftSeq as tinyint
	,@dteToProdDate as DateTime
	,@intToShiftSeq as tinyint
	,@intWorkCenter as int = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @varCompareBy as varchar(4)
	BEGIN TRY

/*
PPsp_ShopOrderAdherence	@vchFacility = '01',
	@chrShowShift ='Y',
	@intShiftFilter = NULL,
	@dteFromProdDate = '2012-02-03',
	@intFromShiftSeq = 1,
	@dteToProdDate='2012-02-03',
	@intToShiftSeq =3	
*/	
		
		SELECT @varCompareBy = Value1 FROM tblControl WHERE [Key] = 'ShopOrderAdherence' AND SubKey = 'Unit'

			-- Get shop odrer history for scheduled information
			;WITH cteSOH AS
			(
				SELECT tSOH.Facility
					,tSOH.ShopOrder
					,tSOH.ItemNumber
					,Cast(STUFF(STUFF(tSOH.StartDate,5,0,'/'),8,0,'/') + ' ' + convert(varchar(8),dbo.fnCvtNumTimeToDateTime(StartTime),8) as DateTime) as SchStartTime
					,Cast(STUFF(STUFF(tSOH.EndDate,5,0,'/'),8,0,'/') + ' ' + convert(varchar(8),dbo.fnCvtNumTimeToDateTime(EndTime),8) as DateTime) as SchEndTime
					,tSOH.PackagingLine
					,tSOH.OrderQty as TotalSchQty
					,(SELECT CONVERT(varchar(8),ProductionDate,112) + CAST(ShiftSequence as char(1)) 
						FROM dbo.fnShiftInfo(@vchFacility, STUFF(STUFF(StartDate,5,0,'/'),8,0,'/') + ' ' + convert(varchar(8),dbo.fnCvtNumTimeToDateTime(StartTime),8),0,NULL,tSOH.PackagingLine)) as ProdDate_ShiftSeq
				FROM tblShopOrderHst tSOH
				LEFT OUTER JOIN tblEquipment tEQ
				ON tSOH.Facility = tEQ.Facility AND tSOH.PackagingLine = tEQ.EquipmentID
				WHERE (@vchFacility is NULL OR tSOH.Facility = @vchFacility)
					AND (@intWorkCenter Is NULL OR ISNULL(tEQ.WorkCenter,Left(tSOH.PackagingLine,4)) = @intWorkCenter)
					AND (StartDate BETWEEN Convert(varchar(8), @dteFromProdDate,112) AND Convert(varchar(8), @dteToProdDate,112))
				--order by shoporder desc
			)
			-- Get the actual cases produced
			,cteCP as
			(	
				SELECT Facility, DefaultPkgLine, ShopOrder, SUM(CasesProduced) as CasesProduced, SUM(AdjustedQty) as AdjustedQty
				FROM tfnSessionControlHstSummary ('WithAdjByLineSO',@vchFacility,NULL,NULL,NULL,NULL,@dteFromProdDate,@intFromShiftSeq,@dteToProdDate,@intToShiftSeq)
				GROUP BY  Facility ,DefaultPkgLine ,ShopOrder
			)
			
			-- Get the scheduled cases, actual stat and end Time 
			,cteSCH as
			(
				SELECT Facility,DefaultPkgLine, ShopOrder, MIN(StartTime) as ActStartTime, MAX(StopTime) as ActStopTime, SUM(CasesScheduled) as CasesScheduled
				FROM tfnSessionControlHstDetail (NULL,@vchFacility,NULL,NULL,NULL,NULL,@dteFromProdDate,@intFromShiftSeq,@dteToProdDate,@intToShiftSeq)
				GROUP BY Facility ,DefaultPkgLine ,ShopOrder
			)
			
			SELECT cteSOH.*
				,Substring(cteSOH.ProdDate_ShiftSeq,9,1) as ShiftSequence
				,tEQ.[Description]
				,cteCP.CasesProduced, cteCP.AdjustedQty
				,cteSCH.ActStartTime, cteSCH.ActStopTime, cteSCH.CasesScheduled
				--,CASE WHEN cteSOH.TotalSchQty = 0 THEN 0 ELSE DATEADD(Second, DATEDIFF(Second, cteSOH.SchStartTime, cteSOH.SchEndTime) * cteSCH.CasesScheduled / CAST(cteSOH.TotalSchQty AS REAL), cteSOH.SchStartTime) END AS AdjSchEndTime 
				--,CASE WHEN cteSCH.CasesScheduled IS NOT NULL THEN 
				--		CASE WHEN cteSOH.TotalSchQty = 0 THEN 0 ELSE 
				--			CASE WHEN @varCompareBy = 'Time' THEN
				--				CASE WHEN ActStopTime < (DATEADD(Second, DATEDIFF(Second, cteSOH.SchStartTime, cteSOH.SchEndTime) * cteSCH.CasesScheduled / cteSOH.TotalSchQty,  cteSOH.SchStartTime)) THEN 1 ELSE 0 END
				--			ELSE
				--				CASE WHEN ActStopTime <= (DATEADD(Day, DATEDIFF(Day, cteSOH.SchStartTime, cteSOH.SchEndTime) * cteSCH.CasesScheduled / cteSOH.TotalSchQty,  cteSOH.SchStartTime)) THEN 1 ELSE 0 END
				--			END	
				--		END 
				--	  ELSE 0
				--END AS EndTimeAdh
				--,CASE WHEN cteSCH.CasesScheduled = 0 OR cteSCH.CasesScheduled IS NULL THEN 0 ELSE 1-(ABS(cteCP.CasesProduced + cteCP.AdjustedQty - cteSCH.CasesScheduled) / CAST(cteSCH.CasesScheduled as REAL)) END AS QtyAdh
				--,CASE WHEN cteSOH.TotalSchQty = 0 OR cteSCH.CasesScheduled IS NULL OR (DATEDIFF(Second, cteSOH.SchStartTime, DATEADD(Second, DATEDIFF(Second, cteSOH.SchStartTime, cteSOH.SchEndTime) * cteSCH.CasesScheduled / CAST(cteSOH.TotalSchQty AS REAL), cteSOH.SchStartTime))) = 0 THEN 0 ELSE 1 - ABS((DATEDIFF(Second, cteSOH.SchStartTime, DATEADD(Second, DATEDIFF(Second, cteSOH.SchStartTime, cteSOH.SchEndTime) * cteSCH.CasesScheduled / CAST(cteSOH.TotalSchQty AS REAL), cteSOH.SchStartTime))) - DATEDIFF(Second, ActStartTime, ActStopTime)) / CAST(DATEDIFF(Second, cteSOH.SchStartTime, DATEADD(Second, DATEDIFF(Second, cteSOH.SchStartTime, cteSOH.SchEndTime) * cteSCH.CasesScheduled / CAST(cteSOH.TotalSchQty AS REAL), cteSOH.SchStartTime)) AS REAL) END AS DurationAdh
				,CASE WHEN cteSOH.TotalSchQty = 0 THEN 0 ELSE DATEADD(Second, DATEDIFF(Second, cteSOH.SchStartTime, cteSOH.SchEndTime) * CAST(cteSCH.CasesScheduled AS REAL) / cteSOH.TotalSchQty, cteSOH.SchStartTime) END AS AdjSchEndTime 
				,CASE WHEN cteSCH.CasesScheduled IS NOT NULL THEN 
						CASE WHEN cteSOH.TotalSchQty = 0 THEN 0 ELSE 
							CASE WHEN @varCompareBy = 'Time' THEN
								CASE WHEN ActStopTime < (DATEADD(Second, DATEDIFF(Second, cteSOH.SchStartTime, cteSOH.SchEndTime) * CAST(cteSCH.CasesScheduled AS REAL) / cteSOH.TotalSchQty,  cteSOH.SchStartTime)) THEN 1 ELSE 0 END
							ELSE
								CASE WHEN ActStopTime <= (DATEADD(Day, DATEDIFF(Day, cteSOH.SchStartTime, cteSOH.SchEndTime) * CAST(cteSCH.CasesScheduled AS REAL) / cteSOH.TotalSchQty,  cteSOH.SchStartTime)) THEN 1 ELSE 0 END
							END	
						END 
					  ELSE 0
				END AS EndTimeAdh
				,CASE WHEN cteSCH.CasesScheduled = 0 OR cteSCH.CasesScheduled IS NULL THEN 0 ELSE 1-(ABS(cteCP.CasesProduced + cteCP.AdjustedQty - cteSCH.CasesScheduled) / CAST(cteSCH.CasesScheduled as REAL)) END AS QtyAdh
				,CASE WHEN cteSOH.TotalSchQty = 0 OR cteSCH.CasesScheduled IS NULL OR (DATEDIFF(Second, cteSOH.SchStartTime, DATEADD(Second, DATEDIFF(Second, cteSOH.SchStartTime, cteSOH.SchEndTime) * CAST(cteSCH.CasesScheduled AS REAL) / cteSOH.TotalSchQty, cteSOH.SchStartTime))) = 0 THEN 0 ELSE 1 - ABS((DATEDIFF(Second, cteSOH.SchStartTime, DATEADD(Second, DATEDIFF(Second, cteSOH.SchStartTime, cteSOH.SchEndTime) * CAST(cteSCH.CasesScheduled AS REAL) / cteSOH.TotalSchQty, cteSOH.SchStartTime))) - DATEDIFF(Second, ActStartTime, ActStopTime)) / CAST(DATEDIFF(Second, cteSOH.SchStartTime, DATEADD(Second, DATEDIFF(Second, cteSOH.SchStartTime, cteSOH.SchEndTime) * CAST(cteSCH.CasesScheduled AS REAL) / cteSOH.TotalSchQty, cteSOH.SchStartTime)) AS REAL) END AS DurationAdh
			FROM cteSOH
			LEFT OUTER JOIN cteCP
				ON cteSOH.Facility = cteCP.Facility AND cteSOH.PackagingLine = cteCP.DefaultPkgLine AND cteSOH.ShopOrder = cteCP.ShopOrder
			LEFT OUTER JOIN cteSCH
				ON cteSOH.Facility = cteSCH.Facility AND cteSOH.PackagingLine = cteSCH.DefaultPkgLine AND cteSOH.ShopOrder = cteSCH.ShopOrder
			-- LEFT OUTER JOIN tblEquipment tEQ
			INNER JOIN tblEquipment tEQ
				ON cteSOH.Facility = tEQ.Facility AND cteSOH.PackagingLine = tEQ.EquipmentID
			WHERE cteSOH.ProdDate_ShiftSeq BETWEEN CONVERT(varchar(8), @dteFromProdDate, 112) + CAST(@intFromShiftSeq as char(1)) AND CONVERT(varchar(8),@dteToProdDate, 112) + CAST(@intToShiftSeq as char(1))
			ORDER BY cteSOH.PackagingLine,cteSOH.SchStartTime

		
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

