
-- =============================================
-- Author:		Bong Lee
-- Create date: July 11, 2011
-- WO# 359:	Power Plant Reporting Phase 2
-- Description:	Get Pallet Adjustmest information
-- WO#755:		Jan 23, 2012	Bong Lee
-- Description:	Add AffectBPCS, AffectPowerPlant to the output table
-- WO#1297      April. 1, 2015   Bong Lee
-- Description: Modify for the new interface with MS Dynamics AX, change RRN to bigint
-- =============================================
CREATE FUNCTION [dbo].[tfnPalletAdjustment] 
(
	@vchAction varchar(50) = NULL, 
	@vchFacility varchar(3) = NULL,
	@intShopOrder int = NULL,
	@vchPkgLine varchar(10)= NULL,
	@vchOperator varchar(50) = NULL,
	@intPalletID int = NULL,
	@dteFromTransactionDate datetime = NULL,
	@dteToTransactiondate datetime = NULL,
	@vchTransactionReasonCode varchar(2) = NULL
)
RETURNS 
@tblPA TABLE 
(
-- WO#1297 	[RRN] [int]
	 [RRN] [bigint]				-- WO#1297 
	,[Facility] [varchar](3)
	,[ShopOrder] [int] 
 	,[MachineID] [char](10) 
	,[Operator] [varchar](10) 
	,[PalletID] [int]
	,[LotNumber] [varchar](25) 
	,[AdjustedQty] [decimal](11, 3) 
	,[TransactionReasonCode] [varchar](2)
	,[TransactionDate] [datetime]
	,[LatestReasonCode] [varchar](2)
	,[MachineName] varchar(30)
	,[OperatorName] [varchar](50)
	,[AffectBPCS] bit						--WO#755
	,[AffectPowerPlant] bit					--WO#755

)
AS
BEGIN

	BEGIN
		-- It returns adjustment with correction, Machine and Operator
		IF @vchAction = 'WithCorrection'
		BEGIN
			INSERT INTO @tblPA
			SELECT tPA.*
				,ISNULL(tPAC.TransactionReasonCode,tPA.TransactionReasonCode) as LatestReasonCode
				,tE.Description as Machine
				,tPS.FirstName + ' ' + tPS.LastName as OperatorName
				,tPATC.AffectBPCS									--WO#755
				,tPATC.AffectPowerPlant								--WO#755
			FROM dbo.tblPalletAdjustment tPA
			LEFT OUTER JOIN tblPalletAdjCorrection tPAC
				ON tPA.PalletID = tPAC.PalletID AND tPA.TransactionDate = tPAC.TransactionDate
			LEFT OUTER JOIN tblPlantStaff tPS
				On tPA.Facility = tPS.Facility and tPA.Operator = tPS.StaffID	
			LEFT OUTER JOIN tblEquipment tE
				On tPA.Facility = tE.facility AND tPA.MachineID = tE.EquipmentID
			LEFT OUTER JOIN tblPalletAdjustmentCode tPATC														--WO#755
				On ISNULL(tPAC.TransactionReasonCode,tPA.TransactionReasonCode) = tPATC.TransactionReasonCode	--WO#755
			WHERE (@vchFacility is NULL OR tPA.Facility = @vchFacility )										
				AND (@intShopOrder is NULL OR tPA.ShopOrder = @intShopOrder)
				AND (@vchPkgLine is NULL OR tPA.MachineID = @vchPkgLine )
				AND (@vchOperator is NULL OR tPA.Operator = @vchOperator)
				AND (@intPalletID is NULL OR tPA.PalletID = @intPalletID)
				AND (@dteFromTransactiondate is NULL OR @dteToTransactiondate is NULL OR tPA.Transactiondate Between @dteFromTransactiondate and @dteToTransactiondate)
				AND (@vchTransactionReasonCode is NULL OR ISNULL(tPAC.TransactionReasonCode,tPA.TransactionReasonCode) = @vchTransactionReasonCode)
		END
		ELSE
			-- It returns with Adjustment and no any other information.
			IF @vchAction = 'WithoutCorrection'
			BEGIN
				INSERT INTO @tblPA
				SELECT tPA.*
					,LatestReasonCode = ''
					,tE.Description as Machine,
					tPS.FirstName + ' ' + tPS.LastName as OperatorName
					,tPATC.AffectBPCS									--WO#755
					,tPATC.AffectPowerPlant 							--WO#755
				FROM dbo.tblPalletAdjustment tPA
				LEFT OUTER JOIN tblPlantStaff tPS
					On tPA.Facility = tPS.Facility and tPA.Operator = tPS.StaffID	
				LEFT OUTER JOIN tblEquipment tE
					On tPA.Facility = tE.facility AND tPA.MachineID = tE.EquipmentID
				LEFT OUTER JOIN tblPalletAdjustmentCode tPATC														--WO#755
					On tPA.TransactionReasonCode = tPATC.TransactionReasonCode		--WO#755
				WHERE (@vchFacility is NULL OR tPA.Facility = @vchFacility )
					AND (@intShopOrder is NULL OR tPA.ShopOrder = @intShopOrder)
					AND (@vchPkgLine is NULL OR tPA.MachineID = @vchPkgLine )
					AND (@vchOperator is NULL OR tPA.Operator = @vchOperator)
					AND (@intPalletID is NULL OR tPA.PalletID = @intPalletID)
					AND (@dteFromTransactiondate is NULL OR @dteToTransactiondate is NULL OR tPA.Transactiondate Between @dteFromTransactiondate and @dteToTransactiondate)
					AND (@vchTransactionReasonCode is NULL OR tPA.TransactionReasonCode = @vchTransactionReasonCode)
			END
			ELSE
				-- It returns adjustment with correction only
				IF @vchAction = 'AdjWithCorrectionOnly'
				BEGIN
					INSERT INTO @tblPA
					SELECT tPA.*
						,ISNULL(tPAC.TransactionReasonCode,tPA.TransactionReasonCode) as LatestReasonCode
						,'' as Machine
						,'' as OperatorName
						,tPATC.AffectBPCS									--WO#755
						,tPATC.AffectPowerPlant								--WO#755
					FROM dbo.tblPalletAdjustment tPA
					LEFT OUTER JOIN tblPalletAdjCorrection tPAC
						ON tPA.PalletID = tPAC.PalletID AND tPA.TransactionDate = tPAC.TransactionDate
					LEFT OUTER JOIN tblPalletAdjustmentCode tPATC														--WO#755
						On ISNULL(tPAC.TransactionReasonCode,tPA.TransactionReasonCode) = tPATC.TransactionReasonCode	--WO#755
					WHERE (@vchFacility is NULL OR tPA.Facility = @vchFacility )
						AND (@intShopOrder is NULL OR tPA.ShopOrder = @intShopOrder)
						AND (@vchPkgLine is NULL OR tPA.MachineID = @vchPkgLine )
						AND (@vchOperator is NULL OR tPA.Operator = @vchOperator)
						AND (@intPalletID is NULL OR tPA.PalletID = @intPalletID)
						AND (@dteFromTransactiondate is NULL OR @dteToTransactiondate is NULL OR tPA.Transactiondate Between @dteFromTransactiondate and @dteToTransactiondate)
						AND (@vchTransactionReasonCode is NULL OR ISNULL(tPAC.TransactionReasonCode,tPA.TransactionReasonCode) = @vchTransactionReasonCode)
				END

	
	END 
		
	RETURN 
END

GO

