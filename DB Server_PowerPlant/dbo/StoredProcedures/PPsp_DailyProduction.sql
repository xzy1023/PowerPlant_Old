
-- =============================================
-- WO#xxxx      Oct. 4, 2014   Bong Lee
-- Description:	Daily Production
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_DailyProduction]
	@dteFromShiftProductionDate datetime
	,@dteToShiftProductionDate  datetime
	,@vchFacility varchar(3)

AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		SELECT T1.Facility, [Date], [WorkCenter], DefaultPkgLine as Line, T1.ItemNumber,
		 [ItemDesc1] + ' ' + [ItemDesc2] as Description
		 ,Cast(SUM(T1.AdjustedQty) as Int) as Cases 
		 ,ROUND(SUM(T1.AdjustedQty * tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit/ (Case When tIM.labelweightUOM = 'GM' Then 453.5924 Else 1 End)),2) as Pound
		 ,Cast(SUM(T1.AdjustedQty * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit) as Int) as Units
		FROM
			(SELECT tPH.Facility, tPH.shiftProductiondate as [Date], tPH.DefaultPkgLine, tPH.ItemNumber, tPH.quantity +  
						ISNULL((SELECT SUM(tPA.AdjustedQty) 
						FROM tfnPalletAdjustment ('AdjWithCorrectionOnly', @vchFacility,  NULL,  NULL,  NULL,  NULL, NULL, NULL, NULL) tPA 
						WHERE tPA.PalletID = tPH.PalletID 
						GROUP BY tPA.PalletID),0) as AdjustedQty
					 FROM tblPalletHst tPH
					 WHERE (tPH.Facility = @vchFacility )
							AND (tPH.ShiftProductionDate between @dteFromShiftProductionDate and @dteToShiftProductionDate)
			) T1
			LEFT OUTER JOIN tblItemMaster tIM
			on T1.Facility = tIM.Facility and T1.ItemNumber = tIM.ItemNumber
			LEFT OUTER JOIN tblEquipment tEQ
			on T1.Facility = tIM.Facility and T1.DefaultPkgLine = tEQ.[EquipmentID]
			group by T1.Facility, [Date], [WorkCenter], DefaultPkgLine, T1.ItemNumber, ItemDesc1 + ' ' + [ItemDesc2]
			order by T1.Facility, [Date], [WorkCenter],  DefaultPkgLine, T1.ItemNumber
	END TRY
	BEGIN CATCH
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure nvarchar(200);
		DECLARE @ErrorLine int;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorMessage NVARCHAR(4000);
		
		DECLARE @nvchSubject nvarchar(256);
		DECLARE @nvchBody nvarchar(MAX);

		SELECT 
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ' Message: ' + CHAR(13) + ERROR_MESSAGE();

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine);
	END CATCH
END

GO

