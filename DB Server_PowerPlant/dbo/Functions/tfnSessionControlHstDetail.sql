

-- =============================================
-- Author:		Bong Lee
-- Create date: May 9, 2011
-- WO# 359:	Power Plant Reporting Phase 2
-- Description:	Get Session Control History Detail inforamtion
-- WO#755:		May 06, 2013	Bong Lee
-- Description:	Select specific columns to return to exclude case count
-- =============================================
/* -- Test --
	SELECT overrideshiftno, count(*) FROM tfnSessionControlHstDetail (NULL, '07 ',NULL,NULL,NULL,NULL,'6/6/2011',NULL,'6/6/2011',NULL)
	group by overrideshiftno

	Shift	Count
	1		44
	2		25
	3		10
	Total	79

	SELECT Shiftproductiondate, overrideshiftno,count(*) FROM tfnSessionControlHstDetail ('07 ',NULL,NULL,NULL,NULL,'6/6/2011',3,'6/7/2011',3)
	group by Shiftproductiondate,overrideshiftno
	order by Shiftproductiondate,overrideshiftno

	ShiftProductionDate		Shift#	Count	ShiftSeq
	2011-06-06 00:00:00.000	2		25		3
	2011-06-07 00:00:00.000	1		47		2
	2011-06-07 00:00:00.000	2		22		3
	2011-06-07 00:00:00.000	3		16		1

*/

CREATE FUNCTION [dbo].[tfnSessionControlHstDetail] 
(
	@vchAction varchar(50) = NULL
	,@vchFacility varchar(3) = NULL
	,@vchMachineID varchar(10) = NULL
	,@vchOperator varchar(10) = NULL
	,@intShopOrder int = NULL
	,@vchItemNumber varchar(35) = NULL
	,@dteFromProdDate DateTime = NULL
	,@intFromShift char(1) = NULL
	,@dteToProdDate DateTime = NULL
	,@intToShift as char(1) =NULL
)
RETURNS 
@tblSCH TABLE 
(
	[RRN] [int]
	,[Facility] [varchar](3)
	,[ComputerName] [varchar](50) 
	,[StartTime] [datetime] 
	,[StopTime] [datetime] 
	,[DefaultPkgLine] [char](10) 
	,[OverridePkgLine] [char](10) 
	,[ShopOrder] [int] 
	,[ItemNumber] [varchar](35) 
	,[Operator] [varchar](10) 
	,[LogOnTime] [datetime] 
	,[DefaultShiftNo] [char](1) 
	,[OverrideShiftNo] [char](1) 
	,[CasesScheduled] [int] 
	,[CasesProduced] [int] 
	,[PalletsCreated] [int] 
	,[BagLengthUsed] [decimal](4, 2) 
	,[ReworkWgt] [decimal](8, 2) 
	,[LooseCases] [int] 
	,[ProductionDate] [datetime] 
	,[ServerCnnIsOk] [bit] 
	,[CarriedForwardCases] [int] 
	,[ShiftProductionDate] [datetime] 
	,[StartDownTime] [datetime]
	,ShiftSequence [smallint]
	,ShiftDesc [varchar] (50) 
)
AS
BEGIN

	DECLARE @MaxShiftSequence int,
			@MinShiftSequence int;

	SELECT @MaxShiftSequence = Max(ShiftSequence) , @MinShiftSequence = Min(ShiftSequence) 
		FROM tblShift WHERE Facility = @vchFacility AND Workgroup = 'P'
	
	-- If the From or To Shifts are not specified or (From Shift is not first shift and To shift is not last shift of a day)
	-- then compare the Shift Producton Date only.
	If  @intFromShift IS NULL 
		OR @intToShift IS NULL
		OR (@intFromShift = @MinShiftSequence AND @intToShift = @MaxShiftSequence)
	BEGIN
		INSERT INTO @tblSCH
			-- WO#755 SELECT tSCH.*, tS.ShiftSequence, tS.Description as ShiftDesc
			-- WO#755 Add Start
			SELECT
				tSCH.RRN
				,tSCH.Facility
				,tSCH.ComputerName
				,tSCH.StartTime
				,tSCH.StopTime
				,tSCH.DefaultPkgLine
				,tSCH.OverridePkgLine
				,tSCH.ShopOrder
				,tSCH.ItemNumber
				,tSCH.Operator
				,tSCH.LogOnTime
				,tSCH.DefaultShiftNo
				,tSCH.OverrideShiftNo
				,tSCH.CasesScheduled
				,tSCH.CasesProduced
				,tSCH.PalletsCreated
				,tSCH.BagLengthUsed
				,tSCH.ReworkWgt
				,tSCH.LooseCases
				,tSCH.ProductionDate
				,tSCH.ServerCnnIsOk
				,tSCH.CarriedForwardCases
				,tSCH.ShiftProductionDate
				,tSCH.StartDownTime
				,tS.ShiftSequence
				,tS.[Description] as ShiftDesc
			-- WO#755 Add Stop
			FROM tblSessionControlHst tSCH
			LEFT OUTER JOIN tblPlantStaff tPS
			ON tSCH.Operator = tPS.StaffID AND tSCH.Facility = tPS.Facility
			LEFT OUTER JOIN vwLineWorkShiftType vLWT
			ON tSCH.Facility = vLWT.Facility AND tSCH.DefaultPkgLine =  vLWT.PackagingLine
			LEFT OUTER JOIN tblshift tS
			ON tSCH.Facility = tS.Facility AND tSCH.OverrideShiftNo = tS.Shift AND ISNULL(vLWT.WorkShiftType,'P') = tS.WorkGroup 
				WHERE (@vchFacility IS NULL OR tSCH.Facility = @vchFacility) 
				AND (@vchMachineID IS NULL OR tSCH.DefaultPkgLine = @vchMachineID)
				AND (@vchOperator IS NULL OR tSCH.Operator  = @vchOperator)
				AND (@intShopOrder IS NULL OR tSCH.ShopOrder = @intShopOrder)
				AND (@vchItemNumber IS NULL OR tSCH.ItemNumber = @vchItemNumber)
				AND ((@dteFromProdDate IS NULL AND @dteToProdDate IS NULL )OR ShiftProductionDate BETWEEN @dteFromProdDate AND @dteToProdDate)
				AND tPS.WorkSubGroup <> 'SetUp'
	END
	ELSE
	BEGIN
		IF 	(@dteFromProdDate IS NULL OR @dteToProdDate IS NULL)
		BEGIN
			INSERT INTO @tblSCH
			-- WO#755 SELECT tSCH.*, tS.ShiftSequence, tS.Description as ShiftDesc
			-- WO#755 Add Start
			SELECT
				tSCH.RRN
				,tSCH.Facility
				,tSCH.ComputerName
				,tSCH.StartTime
				,tSCH.StopTime
				,tSCH.DefaultPkgLine
				,tSCH.OverridePkgLine
				,tSCH.ShopOrder
				,tSCH.ItemNumber
				,tSCH.Operator
				,tSCH.LogOnTime
				,tSCH.DefaultShiftNo
				,tSCH.OverrideShiftNo
				,tSCH.CasesScheduled
				,tSCH.CasesProduced
				,tSCH.PalletsCreated
				,tSCH.BagLengthUsed
				,tSCH.ReworkWgt
				,tSCH.LooseCases
				,tSCH.ProductionDate
				,tSCH.ServerCnnIsOk
				,tSCH.CarriedForwardCases
				,tSCH.ShiftProductionDate
				,tSCH.StartDownTime
				,tS.ShiftSequence
				,tS.[Description] as ShiftDesc
			-- WO#755 Add Stop
			FROM tblSessionControlHst tSCH
			LEFT OUTER JOIN tblPlantStaff tPS
				ON tSCH.Operator = tPS.StaffID AND tSCH.Facility = tPS.Facility
			LEFT OUTER JOIN vwLineWorkShiftType vLWT
				ON tSCH.Facility = vLWT.Facility AND tSCH.DefaultPkgLine =  vLWT.PackagingLine
			LEFT OUTER JOIN tblshift tS
				ON tSCH.Facility = tS.Facility AND tSCH.OverrideShiftNo = tS.Shift AND ISNULL(vLWT.WorkShiftType,'P') = tS.WorkGroup 
			WHERE (@vchFacility IS NULL OR tSCH.Facility = @vchFacility) 
				AND (@vchMachineID IS NULL OR tSCH.DefaultPkgLine = @vchMachineID)
				AND (@vchOperator IS NULL OR tSCH.Operator  = @vchOperator)
				AND (@intShopOrder IS NULL OR tSCH.ShopOrder = @intShopOrder)
				AND (@vchItemNumber IS NULL OR tSCH.ItemNumber = @vchItemNumber)
				AND tS.ShiftSequence BETWEEN @intFromShift AND @intToShift
				AND tPS.WorkSubGroup <> 'SetUp'
		END
		ELSE
		BEGIN		
			INSERT INTO @tblSCH
			-- WO#755 SELECT tSCH.*, tS.ShiftSequence, tS.Description as ShiftDesc
			-- WO#755 Add Start
			SELECT
				tSCH.RRN
				,tSCH.Facility
				,tSCH.ComputerName
				,tSCH.StartTime
				,tSCH.StopTime
				,tSCH.DefaultPkgLine
				,tSCH.OverridePkgLine
				,tSCH.ShopOrder
				,tSCH.ItemNumber
				,tSCH.Operator
				,tSCH.LogOnTime
				,tSCH.DefaultShiftNo
				,tSCH.OverrideShiftNo
				,tSCH.CasesScheduled
				,tSCH.CasesProduced
				,tSCH.PalletsCreated
				,tSCH.BagLengthUsed
				,tSCH.ReworkWgt
				,tSCH.LooseCases
				,tSCH.ProductionDate
				,tSCH.ServerCnnIsOk
				,tSCH.CarriedForwardCases
				,tSCH.ShiftProductionDate
				,tSCH.StartDownTime
				,tS.ShiftSequence
				,tS.[Description] as ShiftDesc
			-- WO#755 Add Stop
			FROM tblSessionControlHst tSCH
			LEFT OUTER JOIN tblPlantStaff tPS
				ON tSCH.Operator = tPS.StaffID AND tSCH.Facility = tPS.Facility
			LEFT OUTER JOIN vwLineWorkShiftType vLWT
				ON tSCH.Facility = vLWT.Facility AND tSCH.DefaultPkgLine =  vLWT.PackagingLine
			LEFT OUTER JOIN tblshift tS
				ON tSCH.Facility = tS.Facility AND tSCH.OverrideShiftNo = tS.Shift AND ISNULL(vLWT.WorkShiftType,'P') = tS.WorkGroup 
			WHERE (@vchFacility IS NULL OR tSCH.Facility = @vchFacility) 
				AND (@vchMachineID IS NULL OR tSCH.DefaultPkgLine = @vchMachineID)
				AND (@vchOperator IS NULL OR tSCH.Operator  = @vchOperator)
				AND (@intShopOrder IS NULL OR tSCH.ShopOrder = @intShopOrder)
				AND (@vchItemNumber IS NULL OR tSCH.ItemNumber = @vchItemNumber)
				AND Convert(varchar(8),ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) 
					BETWEEN convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) AND convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
				AND tPS.WorkSubGroup <> 'SetUp'
		  END
	END

	RETURN 
END

GO

