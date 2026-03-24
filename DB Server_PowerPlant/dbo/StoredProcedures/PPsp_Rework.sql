
-- =============================================
-- Author:		Bong Lee
-- Create date: Jan. 20, 2012
-- Description:	Rework by Shift Production Date & Shift

/* -- To Test --
EXEC	[PPsp_Rework]
		@vchAction = N'ByOperator',
		@vchFacility = N'01',
		@dteFromDate = N'7/14/2011',
		@intFromShift = 1,
		@dteToDate = N'7/14/2011',
		@intToShift = 3,
		@intShift = NULL,
		@vchWorker = NULL
*/
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_Rework]
	@vchAction varchar(50) = NULL,
	@vchFacility varchar(3),
	@dteFromDate datetime , 
	@intFromShift int, 
	@dteToDate datetime,
	@intToShift int, 
	@intShift int = NULL,
	@vchWorker varchar(10) = NULL
AS
BEGIN
	DECLARE @chrEnvironment as char(1)
	DECLARE @vchSQLStmt as nvarchar(3000)
	DECLARE @vchISeriesSQLStmt as nvarchar(300)
	DECLARE @vchISeriesSQLStmt_1 as nvarchar(600)
	DECLARE @vchOrderBy as nvarchar(200)
	DECLARE @iSeriesName as nvarchar(10)
	DECLARE @vchUserLib varchar(10)
	DECLARE @vchOriginalLib varchar(10)
	DECLARE @vchFromShopOrder varchar(25)
	DECLARE @vchToShopOrder varchar(25)

	DECLARE @tIIM Table (
		IPROD varchar(35)
	)

	DECLARE @tIQty Table (
		ShopOrder int,
		Item varchar(35),
		QtyIssued decimal(13,3)
	)

	SET NOCOUNT ON;
	BEGIN TRY

		-- Get the Pound to Gram conversion rate
		DECLARE @decLBtoGM as decimal(10,7)
		
		SELECT @decLBtoGM = Value1 FROM tblControl 
			WHERE [KEY] = 'WeightConversion' AND SubKey = 'General'

		IF @vchAction = 'ByOperator'
		BEGIN
			SELECT tSCH.ItemNumber, tIM.ItemDesc1 + ' ' + tIM.ItemDesc2 as ItemDesc, tSCH.ShopOrder, tSCH.DefaultPkgLine, tEqt.Description, tSCH.ShiftProductionDate
				,tS.Description as ShiftDesc, tSCH.ReworkWgt, tSCH.Operator as Worker, tPS.FirstName + ' ' + tPS.LastName as WorkerName
				,Round((tSCH.CasesProduced) * Round(tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit/ (Case When tIM.labelweightUOM = 'GM' Then @decLBtoGM Else 1 End),3),0) as QtyIssued 
			FROM tfnSessionControlHstSummary('WithAdjByOpr', @vchFacility, NULL, @vchWorker, NULL, NULL, @dteFromDate, @intFromShift,@dteToDate, @intToShift) tSCH
			LEFT OUTER JOIN tblItemMaster tIM
			ON tSCH.Facility = tIM.Facility AND tSCH.ItemNumber = tIM.ItemNumber
			LEFT OUTER JOIN dbo.tblPlantStaff tPS 
			On tSCH.Facility = tPS.Facility AND tSCH.Operator = tPS.StaffID 
			LEFT OUTER JOIN dbo.tblEquipment tEqt
			ON tSCH.Facility = tEqt.Facility AND tSCH.DefaultPkgLine = tEqt.EquipmentID 
			LEFT OUTER JOIN vwLineWorkShiftType vLWT
			ON tSCH.Facility = vLWT.Facility AND tSCH.DefaultPkgLine = vLWT.PackagingLine
			LEFT OUTER JOIN tblShift tS
			ON tSCH.Facility = tS.Facility AND ISNULL(vLWT.WorkShiftType,'P') = tS.WorkGroup AND  tSCH.ShiftNo = tS.Shift
			WHERE (@intShift IS NULL OR tSCH.ShiftNo = @intShift) and ReworkWgt <> 0
		END
		ELSE
		IF @vchAction = 'ByUtilityTech'
		BEGIN
			SELECT tSCH.ItemNumber, tIM.ItemDesc1 + ' ' + tIM.ItemDesc2 as ItemDesc, tSCH.ShopOrder, tSCH.DefaultPkgLine, tEqt.Description, tSCH.ShiftProductionDate
				,tS.Description as ShiftDesc, tSCH.ReworkWgt, tOS.StaffID as Worker, tPS.FirstName + ' ' + tPS.LastName as WorkerName
				,Round((tSCH.CasesProduced) * Round(tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit/ (Case When tIM.labelweightUOM = 'GM' Then @decLBtoGM Else 1 End),3),0) as QtyIssued
			FROM tfnSessionControlHstDetail(NULL, @vchFacility, NULL, @vchWorker, NULL, NULL, @dteFromDate, @intFromShift,@dteToDate, @intToShift) tSCH
			INNER JOIN [tblOperationStaffing] tOS
			On tSCH.Facility = tOS.Facility and tSCH.DefaultPkgLine = tOS.PackagingLine And tSCH.[StartTime] = tOS.[StartTime]
			LEFT OUTER JOIN tblItemMaster tIM
			ON tSCH.Facility = tIM.Facility AND tSCH.ItemNumber = tIM.ItemNumber
			LEFT OUTER JOIN dbo.tblPlantStaff tPS 
			On tSCH.facility = tPS.facility AND tOS.StaffID = tPS.StaffID 
			LEFT OUTER JOIN dbo.tblEquipment tEqt
			ON tSCH.facility = tEqt.facility AND tSCH.DefaultPkgLine = tEqt.EquipmentID 
			LEFT OUTER JOIN vwLineWorkShiftType vLWT
			ON tSCH.Facility = vLWT.Facility AND tSCH.DefaultPkgLine = vLWT.PackagingLine
			LEFT OUTER JOIN tblShift tS
			ON tSCH.Facility = tS.Facility AND  ISNULL(vLWT.WorkShiftType,'P') = tS.WorkGroup AND  tSCH.OverrideShiftNo = tS.Shift
			WHERE (@intShift IS NULL OR tSCH.OverrideShiftNo = @intShift) and ReworkWgt <> 0
		END	

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
	END CATCH
END

GO

