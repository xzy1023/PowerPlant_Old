

-- =============================================
-- Author:		Bong Lee
-- Create date: Mar 9, 2010
-- Description:	Select Machine Paid Hours
-- WO#359:		April. 4, 2012	Bong Lee
--				Use standard table functiont to retrieve packaging log
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_MachinePaidHours_Sel]
	@vchAction varchar(50),
	@vchFacility varchar(3),
	@vchPkgLine varchar(10) = NULL,
	@dteShiftProductionDate as DateTime,
	@intShiftNo as tinyint
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRY
		--WO#359 ADD Start
		-- Convert Shift no. to Shift Sequence
		If @vchPkgLine IS NULL
			SELECT @intShiftNo = ShiftSequence from tblShift Where Facility = @vchFacility AND Shift = @intShiftNo AND WorkGroup = 'P' 
		ELSE
			SELECT @intShiftNo = ShiftSequence from tblShift 
				Where Facility = @vchFacility AND Shift = @intShiftNo 
					AND WorkGroup = (Select WorkShiftType FROM dbo.vwLineWorkShiftType Where Facility = @vchFacility AND PackagingLine = @vchPkgLine)
		--WO#359 ADD Stop
		
		IF @vchAction = 'GrpByLine_SO_Opr'
			BEGIN
			SELECT tART.*, isnull(tMRH.PaidHours,tART.ActRunTime) as PaidHours FROM (
				SELECT	tSCH.Facility, tSCH.DefaultPkgLine as PkgLine, tSCH.ShopOrder, tSCH.Operator, tPS.FirstName + ' ' + tPS.LastName AS OprName, 
						SUM(ROUND(DATEDIFF(second, tSCH.StartTime, tSCH.StopTime)/ 3600.00, 2)) AS ActRunTime, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo
				FROM	dbo.tfnSessionControlHstDetail(NULL,@vchFacility, @vchPkgLine,NULL,NULL, NULL,@dteShiftProductionDate,@intShiftNo,@dteShiftProductionDate,@intShiftNo ) AS tSCH --WO#359
--WO#359		FROM	tblSessionControlHst AS tSCH 
				LEFT OUTER JOIN tblPlantStaff AS tPS 
						ON tSCH.Facility = tPS.Facility AND tSCH.Operator = tPS.StaffID
--WO#359		WHERE	(tSCH.Facility = @vchFacility) AND (tSCH.ShiftProductionDate = @dteShiftProductionDate) AND (tSCH.OverrideShiftNo = @intShiftNo) 
--WO#359				AND (@vchPkgLine is null OR tSCH.DefaultPkgLine = @vchPkgLine)
				GROUP BY tSCH.Facility, tSCH.DefaultPkgLine, tSCH.ShopOrder, tSCH.Operator, tPS.FirstName + ' ' + tPS.LastName, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo) tART
			LEFT OUTER JOIN	tblMachinePaidHours AS tMRH 
					ON tART.Facility = tMRH.Facility AND tART.PkgLine = tMRH.MachineID AND tART.ShopOrder = tMRH.ShopOrder AND tART.Operator = tMRH.Operator 
						AND tART.ShiftProductiondate = tMRH.ShiftProductionDate AND tART.OverrideShiftNo = tMRH.ShiftNo
			END
		ELSE
		  IF @vchAction = 'GrpByLine'
		  BEGIN

			SELECT tART.PkgLine, tEQ.Description, Sum(tART.ActRunTime) as ActRunTime, Sum(isnull(tMRH.PaidHours,tART.ActRunTime)) as PaidHours FROM (
				SELECT	tSCH.Facility, tSCH.DefaultPkgLine as PkgLine, tSCH.ShopOrder, tSCH.Operator, tPS.FirstName + ' ' + tPS.LastName AS OprName, 
						SUM(ROUND(DATEDIFF(second, tSCH.StartTime, tSCH.StopTime)/ 3600.00, 2)) AS ActRunTime, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo
				FROM	dbo.tfnSessionControlHstDetail(NULL,@vchFacility, @vchPkgLine,NULL,NULL, NULL,@dteShiftProductionDate,@intShiftNo,@dteShiftProductionDate,@intShiftNo ) AS tSCH --WO#359
--WO#359		FROM	tblSessionControlHst AS tSCH 
				LEFT OUTER JOIN tblPlantStaff AS tPS 
						ON tSCH.Facility = tPS.Facility AND tSCH.Operator = tPS.StaffID
--WO#359		WHERE	(tSCH.Facility = @vchFacility) AND (tSCH.ShiftProductionDate = @dteShiftProductionDate) AND (tSCH.OverrideShiftNo = @intShiftNo) 
--WO#359				AND (@vchPkgLine is null OR tSCH.DefaultPkgLine = @vchPkgLine)
				GROUP BY tSCH.Facility, tSCH.DefaultPkgLine, tSCH.ShopOrder, tSCH.Operator, tPS.FirstName + ' ' + tPS.LastName, tSCH.ShiftProductionDate, tSCH.OverrideShiftNo) tART
			LEFT OUTER JOIN	tblMachinePaidHours AS tMRH 
					ON tART.Facility = tMRH.Facility AND tART.PkgLine = tMRH.MachineID AND tART.ShopOrder = tMRH.ShopOrder AND tART.Operator = tMRH.Operator 
						AND tART.ShiftProductiondate = tMRH.ShiftProductiondate AND tART.OverrideShiftNo = tMRH.ShiftNo
			LEFT OUTER JOIN tblEquipment AS tEQ 
					ON tART.Facility = tEQ.Facility AND tART.PkgLine = tEQ.EquipmentID
			WHERE tART.Facility = @vchFacility
			GROUP BY  tART.PkgLine, tEQ.Description
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
	END CATCH;
END

GO

