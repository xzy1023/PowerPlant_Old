
-- =============================================
-- Author:		Bong Lee
-- Create date: Dec. 02, 2010
-- Description:	Select Session Control History 
-- WO#359:		Aug. 02, 2011	Bong Lee
--				make use of table function tfnSessionControlHstDetail to select record
-- WO#359:		May. 02, 2012	Bong Lee
--				Shop Order Adherenace Report - Add action for indicate the shop order has 
--				pallet Adjustment or not.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_SessionControlHst_Sel] 
	-- Add the parameters for the stored procedure here
	@vchAction VARCHAR(50),
	@chrPkgLine char(10) = NULL,
	@intShopOrder int = NULL,
	@intFromShiftSeq int = NULL,
	@intToShiftSeq int = NULL,
	@intShift int = NULL,
	@dteFromShiftProductionDate datetime = NULL,
	@dteToShiftProductionDate datetime = NULL,
	@dteDateTime datetime = NULL,
	@chrFacility char(3) = NULL,
	@vchOperator varchar(10) = NULL,
	@intRRN int = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @dteShiftStart as datetime
	DECLARE @dteShiftEnd as datetime
	DECLARE @dteLatestStartTime as datetime

	BEGIN TRY

		-- Select by Production Shift sequence
		IF @vchAction = 'ByShiftSeq'
		BEGIN
-- WO#359 Add Start
			SELECT tSCH.*, tIM.ItemDesc1 AS ItemDesc, tPS.FirstName + ' ' + tPS.LastName AS OperatorName
				FROM dbo.tfnSessionControlHstDetail (NULL, @chrFacility, @chrPkgLine, @vchOperator, @intShopOrder, NULL
				,@dteFromShiftProductionDate ,@intFromShiftSeq ,ISNULL(@dteToShiftProductionDate,@dteFromShiftProductionDate)
				,ISNULL(@intToShiftSeq,@intFromShiftSeq)) tSCH
-- WO#359 Add Stop				
-- WO#359 DEL Start
			--/* Find the Work Shift type for each line - if line is duplicated, pick the active one */
			--Declare @tblComputerConfig Table
			--(Facility varchar(3) ,Packagingline varchar(10), WorkShiftType varchar(10))

			--INSERT INTO @tblComputerConfig
			--SELECT T1.Facility, T1.Packagingline, T1.WorkShiftType 
			--FROM tblcomputerconfig T1 
			--Left Outer Join 
			--	(SELECT Facility, Packagingline 
			--		FROM tblComputerconfig 
			--	WHERE Packagingline <> 'SPARE'
			--	GROUP BY Facility, Packagingline
			--	HAVING Count(*) > 1) T2
			--ON T1.Facility = T2.Facility AND T1.Packagingline = T2.Packagingline
			--WHERE (T2.Packagingline is null OR T1.RecordStatus = 1) 
			--	AND T1.PackagingLine <> 'SPARE'
			--GROUP BY T1.Facility, T1.Packagingline, T1.WorkShiftType

			--SELECT tSCH.*, tIM.ItemDesc1 AS ItemDesc,
			--	tPS.FirstName + ' ' + tPS.LastName AS OperatorName
			--FROM tblSessionControlhst tSCH
			--Left Outer Join @tblComputerConfig tCC
			--ON tSCH.Facility = tCC.Facility AND tSCH.OverridePkgLine =  tCC.PackagingLine
			--Left Outer Join tblshift tS
			--ON tSCH.Facility = tS.Facility AND tSCH.OverrideShiftNo = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
-- WO#359 DEL Stop			
			LEFT OUTER JOIN dbo.tblItemMaster tIM
			ON tSCH.Facility = tIM.Facility and tSCH.ItemNumber = tIM.ItemNumber
			LEFT OUTER JOIN dbo.tblPlantStaff tPS
			ON tSCH.Facility = tPS.Facility and tSCH.Operator =tPS.StaffID
-- WO#359 DEL Start			
			--WHERE tSCH.facility = @chrFacility
			--	AND tS.ShiftSequence = @intFromShiftSeq
			--	AND tSCH.ShiftProductionDate = @dteFromShiftProductionDate
			--	AND (@chrPkgLine is null or tSCH.DefaultPkgLine = @chrPkgLine)
			--	AND (@vchOperator is null or tSCH.Operator = @vchOperator)
-- WO#359 DEL Stop			
			ORDER BY StartTime
		END
		ELSE
			-- Select by Relative Record Number
			IF @vchAction = 'ByRRN'
			BEGIN
				SELECT * FROM tblSessionControlhst 
					WHERE RRN = @intRRN
			END
			ELSE
				-- Select by Relative Record Number
				IF @vchAction = 'IndicateAdjustment'
				BEGIN
					;With cteADj as
					(
						Select  Facility, ShopOrder, MachineID, 'Y' AS HasAdjustment   from tfnPalletAdjustment (
							'WithoutCorrection'	--@vchAction
							,@chrFacility
							,@intShopOrder
							,@chrPkgLine
							,NULL			--@vchOperator
							,NULL			--@intPalletID
							,NULL			--@dteFromTransactionDate
							,NULL			--@dteToTransactiondate
							,NULL			--@vchTransactionReasonCod
						) GROUP BY Facility, ShopOrder, MachineID
					)
					
					SELECT tSCH.*, tIM.ItemDesc1 AS ItemDesc, tPS.FirstName + ' ' + tPS.LastName AS OperatorName, cteADj.HasAdjustment 
						FROM dbo.tfnSessionControlHstDetail (NULL, @chrFacility, @chrPkgLine, @vchOperator, @intShopOrder, NULL
							,@dteFromShiftProductionDate ,@intFromShiftSeq ,@dteToShiftProductionDate ,@intToShiftSeq) tSCH
					LEFT OUTER JOIN cteADj 
					ON tSCH.Facility = cteADj.Facility AND tSCH.ShopOrder = cteADj.ShopOrder AND tSCH.DefaultPkgLine = cteADj.MachineID
					LEFT OUTER JOIN dbo.tblItemMaster tIM
					ON tSCH.Facility = tIM.Facility and tSCH.ItemNumber = tIM.ItemNumber
					LEFT OUTER JOIN dbo.tblPlantStaff tPS
					ON tSCH.Facility = tPS.Facility and tSCH.Operator =tPS.StaffID
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

