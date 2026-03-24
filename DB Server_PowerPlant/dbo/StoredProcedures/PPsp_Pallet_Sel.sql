

-- =============================================
-- Author:		Bong Lee
-- Create date: Mar. 11 ,2010
-- Description:	Select Pallet
-- WO#297:		Mar 29, 2011	Bong Lee	
-- Description:	Add PalletOnly & PalletOnly options
-- WO#359:		May 11, 2011	Bong Lee
-- Description:	Added action 'AllPalletsWithAdj'
-- WO#755:		Jan 23, 2012	Bong Lee
-- Description:	Add AffectBPCS, AffectPowerPlant
-- WO#17432		Bong Lee	Create date: Jul 05, 2018
-- Description:	Add Action Pallet_LastCreated
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_Pallet_Sel]
	@vchAction varchar(50) = NULL, 
	@chrFacility char(3) = NULL,
	@intPalletID int = NULL,
	@chrPkgLine char(10)= NULL,
	@intShopOrder int = NULL,
	@vchShiftProductionDate datetime = NULL,
	@intShift int = NULL,
	@vchStaffID varchar(50) = NULL
AS
BEGIN

	SET NOCOUNT ON;
	BEGIN TRY

	-- WO#359 ADD Start
	If @vchAction = 'AllPalletsWithReasonAdj'
	BEGIN
		-- Assuming only one negative PR transaction reason code per pallet
		SELECT tP.*, '' as TransactionReasonCode,
			'' as AffectBPCS,								--WO#755
			'' as AffectPowerPlant,							--WO#755
			0 as AdjustedQty,
			Case WHEN not (tPS.WorkGroup = 'P' or tPS.WorkGroup = 'ALL') OR tPS.WorkGroup Is null Then 'NA' Else tPS.WorkGroup End as WorkGroup,
			tPS.FirstName + ' ' + tPS.LastName as OperatorName, 'N' as Posted 
		FROM dbo.tblPallet tP
		LEFT OUTER JOIN tblPlantStaff tPS
			On tP.Facility = tPS.Facility and tP.Operator = tPS.StaffID	
		WHERE (@chrFacility is NULL OR tP.Facility = @chrFacility )
			AND (@intPalletID is NULL OR tP.PalletID = @intPalletID)
			AND (@chrPkgLine is NULL OR tP.defaultPkgLine = @chrPkgLine )
			AND (@intShopOrder is NULL OR tP.ShopOrder = @intShopOrder)
			AND (@vchShiftProductionDate is NULL OR tP.ShiftProductionDate = @vchShiftProductionDate)
			AND (@intShift is NULL OR tP.ShiftNo = @intShift)
			AND (@vchStaffID is NULL OR tP.Operator = @vchStaffID)
		UNION ALL
		-- WO#755 SELECT tPH.*, tPA.latestReasonCode as TransactionReasonCode, 
		SELECT tPH.*, ISNULL(tPA.latestReasonCode,'') as TransactionReasonCode,		-- WO#755
			ISNULL(tPA.AffectBPCS,'') as AffectBPCS,								--WO#755
			ISNULL(tPA.AffectPowerPlant,'') as AffectPowerPlant,					--WO#755
			ISNULL(AdjustedQty,0),
			Case WHEN not (tPS.WorkGroup = 'P' or tPS.WorkGroup = 'ALL') OR tPS.WorkGroup Is null Then 'NA' Else tPS.WorkGroup End as WorkGroup,
			tPS.FirstName + ' ' + tPS.LastName as OperatorName, 'Y' as Posted  
		 FROM tblPalletHst tPH
		 LEFT OUTER JOIN (SELECT PalletID, LatestReasonCode
		 	,AffectBPCS								--WO#755
			,AffectPowerPlant						--WO#755
			,SUM(AdjustedQty) as AdjustedQty 
			FROM tfnPalletAdjustment ('AdjWithCorrectionOnly', @chrFacility, @intShopOrder, @chrPkgLine, @vchStaffID, @intPalletID, NULL, NULL, NULL)
			GROUP BY PalletID, LatestReasonCode, AffectBPCS, AffectPowerPlant) tPA	--WO#755
			--WO#755 GROUP BY PalletID, LatestReasonCode) tPA 
		 ON tPA.PalletID = tPH.PalletID 
		 LEFT OUTER JOIN tblPlantStaff tPS
			On tPH.Facility = tPS.Facility and tPH.Operator = tPS.StaffID	
		 WHERE (@chrFacility is NULL OR tPH.Facility = @chrFacility )
				AND (@intPalletID is NULL OR tPH.PalletID = @intPalletID)
				AND (@chrPkgLine is NULL OR tPH.defaultPkgLine = @chrPkgLine )
				AND (@intShopOrder is NULL OR tPH.ShopOrder = @intShopOrder)
				AND (@vchShiftProductionDate is NULL OR tPH.ShiftProductionDate = @vchShiftProductionDate)
				AND (@intShift is NULL OR tPH.ShiftNo = @intShift)
				AND (@vchStaffID is NULL OR tPH.Operator = @vchStaffID)
	END 
	ELSE
	  If @vchAction = 'AllPalletsWithAdj'
	  BEGIN
		SELECT tP.*,
			0 as AdjustedQty, 
			Case WHEN not (tPS.WorkGroup = 'P' or tPS.WorkGroup = 'ALL') OR tPS.WorkGroup Is null Then 'NA' Else tPS.WorkGroup End as WorkGroup,
			tPS.FirstName + ' ' + tPS.LastName as OperatorName, 'N' as Posted 
		FROM dbo.tblPallet tP
		LEFT OUTER JOIN tblPlantStaff tPS
			On tP.Facility = tPS.Facility and tP.Operator = tPS.StaffID	
		WHERE (@chrFacility is NULL OR tP.Facility = @chrFacility )
			AND (@intPalletID is NULL OR tP.PalletID = @intPalletID)
			AND (@chrPkgLine is NULL OR tP.defaultPkgLine = @chrPkgLine )
			AND (@intShopOrder is NULL OR tP.ShopOrder = @intShopOrder)
			AND (@vchShiftProductionDate is NULL OR tP.ShiftProductionDate = @vchShiftProductionDate)
			AND (@intShift is NULL OR tP.ShiftNo = @intShift)
			AND (@vchStaffID is NULL OR tP.Operator = @vchStaffID)
		UNION ALL
		SELECT tPH.*, 
			ISNULL((SELECT SUM(tPA.AdjustedQty) 
			FROM tfnPalletAdjustment ('AdjWithCorrectionOnly', @chrFacility, @intShopOrder, @chrPkgLine, @vchStaffID, @intPalletID, NULL, NULL, NULL) tPA 
			WHERE tPA.PalletID = tPH.PalletID 
			GROUP BY tPA.PalletID),0) as AdjustedQty,
			Case WHEN not (tPS.WorkGroup = 'P' or tPS.WorkGroup = 'ALL') OR tPS.WorkGroup Is null Then 'NA' Else tPS.WorkGroup End as WorkGroup,
			tPS.FirstName + ' ' + tPS.LastName as OperatorName, 'Y' as Posted  
		 FROM tblPalletHst tPH
		 LEFT OUTER JOIN tblPlantStaff tPS
			On tPH.Facility = tPS.Facility and tPH.Operator = tPS.StaffID	
		 WHERE (@chrFacility is NULL OR tPH.Facility = @chrFacility )
				AND (@intPalletID is NULL OR tPH.PalletID = @intPalletID)
				AND (@chrPkgLine is NULL OR tPH.defaultPkgLine = @chrPkgLine )
				AND (@intShopOrder is NULL OR tPH.ShopOrder = @intShopOrder)
				AND (@vchShiftProductionDate is NULL OR tPH.ShiftProductionDate = @vchShiftProductionDate)
				AND (@intShift is NULL OR tPH.ShiftNo = @intShift)
				AND (@vchStaffID is NULL OR tPH.Operator = @vchStaffID)
	  END 
	-- WO#359 ADD Stop
	  ELSE
		IF @vchAction = 'AllPalletsOrderByID'
		BEGIN
			SELECT * FROM dbo.tblPallet 
				WHERE (@chrFacility is NULL OR Facility = @chrFacility )
					AND (@intPalletID is NULL OR PalletID = @intPalletID)
					AND (@chrPkgLine is NULL OR defaultPkgLine = @chrPkgLine )
					AND (@intShopOrder is NULL OR ShopOrder = @intShopOrder)
					AND (@vchShiftProductionDate is NULL OR ShiftProductionDate = @vchShiftProductionDate)
					AND (@intShift is NULL OR ShiftNo = @intShift)
					AND (@vchStaffID is NULL OR Operator = @vchStaffID)
				UNION ALL
			SELECT * FROM dbo.tblPallethst 
				WHERE (@chrFacility is NULL OR Facility = @chrFacility )
					AND (@intPalletID is NULL OR PalletID = @intPalletID)
					AND (@chrPkgLine is NULL OR defaultPkgLine = @chrPkgLine )
					AND (@intShopOrder is NULL OR ShopOrder = @intShopOrder)
					AND (@vchShiftProductionDate is NULL OR ShiftProductionDate = @vchShiftProductionDate)
					AND (@intShift is NULL OR ShiftNo = @intShift)
					AND (@vchStaffID is NULL OR Operator = @vchStaffID)
			Order by PalletID
		END

-- WO#297 ADD Start
		ELSE
		IF @vchAction = 'PalletOnly'
		BEGIN
			SELECT * FROM dbo.tblPallet 
				WHERE (@chrFacility is NULL OR Facility = @chrFacility )
					AND (@intPalletID is NULL OR PalletID = @intPalletID)
					AND (@chrPkgLine is NULL OR defaultPkgLine = @chrPkgLine )
					AND (@intShopOrder is NULL OR ShopOrder = @intShopOrder)
					AND (@vchShiftProductionDate is NULL OR ShiftProductionDate = @vchShiftProductionDate)
					AND (@intShift is NULL OR ShiftNo = @intShift)
					AND (@vchStaffID is NULL OR Operator = @vchStaffID)
		END
		ELSE
			IF @vchAction = 'PalletHstOnly'
			BEGIN
				SELECT * FROM dbo.tblPalletHst 
					WHERE (@chrFacility is NULL OR Facility = @chrFacility )
						AND (@intPalletID is NULL OR PalletID = @intPalletID)
						AND (@chrPkgLine is NULL OR defaultPkgLine = @chrPkgLine )
						AND (@intShopOrder is NULL OR ShopOrder = @intShopOrder)
						AND (@vchShiftProductionDate is NULL OR ShiftProductionDate = @vchShiftProductionDate)
						AND (@intShift is NULL OR ShiftNo = @intShift)
						AND (@vchStaffID is NULL OR Operator = @vchStaffID)
			END
-- WO#297 ADD Stop
-- WO#17432	ADD Start
			ELSE
				IF @vchAction = 'Pallet_LastCreated'
				BEGIN
					SELECT TOP 1 * FROM
					(SELECT * FROM dbo.tblPallet 
						WHERE (@chrFacility is NULL OR Facility = @chrFacility )
							AND (@intPalletID is NULL OR PalletID = @intPalletID)
							AND (@chrPkgLine is NULL OR defaultPkgLine = @chrPkgLine )
							AND (@intShopOrder is NULL OR ShopOrder = @intShopOrder)
							AND (@vchShiftProductionDate is NULL OR ShiftProductionDate = @vchShiftProductionDate)
							AND (@intShift is NULL OR ShiftNo = @intShift)
							AND (@vchStaffID is NULL OR Operator = @vchStaffID)
						UNION ALL
					SELECT * FROM dbo.tblPallethst 
						WHERE (@chrFacility is NULL OR Facility = @chrFacility )
							AND (@intPalletID is NULL OR PalletID = @intPalletID)
							AND (@chrPkgLine is NULL OR defaultPkgLine = @chrPkgLine )
							AND (@intShopOrder is NULL OR ShopOrder = @intShopOrder)
							AND (@vchShiftProductionDate is NULL OR ShiftProductionDate = @vchShiftProductionDate)
							AND (@intShift is NULL OR ShiftNo = @intShift)
							AND (@vchStaffID is NULL OR Operator = @vchStaffID)
					) as t1
					Order by PalletID DESC
				END
-- WO#17432	ADD Stop
		
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

