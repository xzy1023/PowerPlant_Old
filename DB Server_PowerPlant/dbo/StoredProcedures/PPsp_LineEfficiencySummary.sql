

-- =============================================
-- Author:		Bong Lee
-- Create date: Aug 13,2009
-- Description:	Line Efficency Summary
-- Task#6631	Aug. 31, 2015	Bong Lee
-- Description:	Use information from Dynamics AX
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_LineEfficiencySummary]
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

    DECLARE @vchProcEnv varchar(10)
	DECLARE @vchServerName as varchar(50);
	DECLARE @vchServerSQLStmt  varchar(1500);
	DECLARE @vchSQLStmt  varchar(1700);
	DECLARE @vchUserLib varchar(10);
	DECLARE @vchOriginalLib varchar(10);
	DECLARE @vchFromRow varchar(10);
	DECLARE @vchToRow varchar(10);
	DECLARE @vchFromDate as varchar(8), @vchToDate as varchar(8)

	BEGIN TRY
		/* Task#6631 DEL Start
		select @vchProcEnv = Value2 from tblcontrol Where Facility = @vchFacility and [Key] = 'CompanyName' and Subkey = 'General'

		If @vchProcEnv = 'PRD'
			BEGIN
				Select @vchServerName = value1 From tblControl Where [key] = 'iSeriesNames'
				Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibPRD'
			END
			ELSE
			BEGIN
				If @vchProcEnv = 'UA'
				BEGIN
					Select @vchServerName = value2 From tblControl Where [key] = 'iSeriesNames'
					Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibUA'
				END
				ELSE
				BEGIN
					Select @vchServerName = value2 From tblControl Where [key] = 'iSeriesNames'
					Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibDev'
				END
			END 
		Task#6631 DEL Stop */

		Select @vchFromDate = Convert(varchar(8),@dteFromProdDate - 1,112),
			   @vchToDate = Convert(varchar(8),@dteToProdDate + 10,112)

		-- StdUnitPerHr = Routing rate * (2 ** Base code)
		-- StdMachineHrEarnedInUnit = Cases Produced / StdUnitPerHr
		-- Units Produced = Cases Produced * Saleable Unit Per Case

		-- Actual Line Efficiency %= Case Produced / Acutal run time / StdUnitPerHr * 100%

		Select tSCH.Facility, tSCH.DefaultPkgLine, tSCH.ShopOrder, tSCH.ItemNumber, tSCH.Operator, tSCH.CasesProduced+ISNULL(tADJ.AdjustedQty,0) as CasesProduced, tSCH.ActRunTime,
			 tEQ.Description, Round(tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit / (Case When tIM.labelweightUOM = 'GM' Then 453.5924 Else 1 End),3) as NetWeight, 
			Round((tSCH.CasesProduced+ISNULL(tADJ.AdjustedQty,0)) * Round(tIM.labelweight * tIM.saleableunitpercase * tIM.PackagesPerSaleableUnit/ (Case When tIM.labelweightUOM = 'GM' Then 453.5924 Else 1 End),3),0) as PoundProduced, 
			CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0)) END as StdUnitPerHr,
			CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else (tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0))/ (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit,
			ISNULL(tSMER_ID.StdWorkCenterEfficiency,ISNULL(tSMER_WC.StdWorkCenterEfficiency,0)) as StdWCEfficiency,
			tPS.OperatorName, IsNull(tDTL.DownTime,0) as DownTime, ISNULL(AdjustedQty,0) as AdjustedCases,
			Round((tSCH.CasesProduced + ISNULL(tADJ.AdjustedQty,0)) * ISNULL(tIM.SaleableUnitPerCase,0) * ISNULL(tIM.PackagesPerSaleableUnit,0) ,0)  as UnitsProduced
		From (
			Select Facility,DefaultPkgLine,ShopOrder,ItemNumber,Operator,sum(CasesProduced) as CasesProduced,  Round(Sum(Isnull(DateDiff(minute,StartTime,StopTime),0)) / 60.00, 2) as ActRunTime
			From tblsessioncontrolhst with (nolock)
			Where Facility = @vchFacility and Convert(varchar(8),ShiftProductionDate,112) + Cast(OverrideShiftNo as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
				AND ((Facility not in (Select facility from dbo.tblFacility where Region = '01') OR Len(Rtrim(Operator)) > 3) )
			Group by Facility, DefaultPkgLine,ShopOrder,ItemNumber,Operator) tSCH
		Left outer join tblItemMaster tIM  with (nolock)
			On tSCH.Facility = tIM.Facility and tSCH.ItemNumber = tIM.ItemNumber
		LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_ID with (nolock)
			ON tSCH.Facility = tSMER_ID.Facility AND tIM.ItemNumber = tSMER_ID.ItemNumber AND tSCH.DefaultPkgLine = tSMER_ID.MachineID
		LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_WC with (nolock)
			ON tSCH.Facility = tSMER_WC.Facility AND tIM.ItemNumber = tSMER_WC.ItemNumber AND CAST(SUBSTRING(tSCH.DefaultPkgLine,1,4) as int) = tSMER_WC.WorkCenter and tSMER_WC.MachineID = ''
		Left Outer Join tblEquipment tEQ with (nolock)
			ON tSCH.Facility = tEQ.Facility and tSCH.DefaultPkgLine = tEQ.EquipmentID
		Left Outer Join (Select Distinct StaffID, FirstName + ' ' + LastName as OperatorName from tblPlantStaff with (nolock) Where Facility = @vchFacility ) tPS 
			ON tSCH.Operator = tPS.StaffID
		Left Outer Join (Select MachineID, ShopOrder, Operator, Sum(MaxDownTime)/60.00 as DownTime 
			from 
			(Select facility, MachineID, ShopOrder, Operator, DownTimeBegin, max(DateDiff(Second,DownTimeBegin,DownTimeEnd)) as MaxDownTime
				From dbo.tblDownTimeLog
				Where Facility = @vchFacility and InActive = 0 
					  AND Convert(varchar(8),ShiftProductionDate,112) + Cast(Shift as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
				Group by facility, MachineID, ShopOrder, Operator, DownTimeBegin) tDL	
			Group by MachineID, ShopOrder, Operator) tDTL
			On tSCH.DefaultPkgLine=tDTL.MachineID And tSCH.ShopOrder = tDTL.ShopOrder And tSCH.Operator = tDTL.Operator
		Left Outer Join (Select Facility,MachineID,ShopOrder,Operator, Sum(AdjustedQty) as AdjustedQty From tblPalletAdjustment with (nolock) 
			Where Facility = @vchFacility 
			Group by Facility,MachineID,ShopOrder,Operator) tADJ
			On tSCH.Facility = tADJ.Facility and tSCH.DefaultPkgLine=tADJ.MachineID And tSCH.ShopOrder = tADJ.ShopOrder And tSCH.Operator = tADJ.Operator
		Order by tSCH.DefaultPkgLine, tSCH.ShopOrder, tSCH.Operator

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

