


-- =============================================
-- Author:		Bong Lee
-- Create date: Dec. 8, 2011
-- WO# 359:	Power Plant Reporting Phase 2
-- Description:	Get Pallet Detail inforamtion
-- WO#2563:	Apr. 22, 2016	Bong Lee	
-- Description:	New field 'OutputLocation' is added to tblPalletHst. 
--				The corresponding process should be changed accordingly
-- ALM#11828:	Apr. 22, 2016	Bong Lee	
-- Description:	New field 'DestinationShopOrder' is added to tblPalletHst. 
--				The corresponding process should be changed accordingly
-- =============================================
/* -- Test --
	SELECT overrideshiftno, count(*) FROM tfnPalletHstDetail ('07 ',NULL,NULL,NULL,NULL,'6/6/2011',NULL,'6/6/2011',NULL)
	group by overrideshiftno
*/

CREATE FUNCTION [dbo].[tfnPalletHstDetail] 
(
	@vchAction varchar(50) = NULL
	,@vchFacility varchar(3)
	,@vchMachineID varchar(10) = NULL
	,@vchOperator varchar(10) = NULL
	,@intShopOrder int = NULL
	,@vchItemNumber varchar(35) = NULL
	,@dteFromProdDate DateTime
	,@intFromShift char(1) = NULL
	,@dteToProdDate DateTime
	,@intToShift as char(1) =NULL
)
RETURNS 
@tblPalletHst TABLE 
(
	rrn int
	,Facility	char(3)
	,PalletID	int
	,QtyPerPallet	int
	,Quantity	int
	,ItemNumber	varchar(35)
	,DefaultPkgLine	char(10)
	,Operator	varchar(10)
	,CreationDate	char(8)
	,CreationTime	char(6)
	,OrderComplete	char(1)
	,LotID	varchar(25)
	,ShopOrder	int
	,StartTime	datetime
	,ProductionDate	char(8)
	,ExpiryDate	char(8)
	,PrintStatus	smallint
	,CreationDateTime	datetime
	,ShiftProductionDate	datetime
	,ShiftNo	tinyint
	,LastUpdate	datetime
	,OutputLocation varchar(10)			-- WO#2563
	,DestinationShopOrder int			-- ALM#11828
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
		INSERT INTO @tblPalletHst
			SELECT tPH.*
			FROM tblPalletHst tPH
			LEFT OUTER JOIN tblPlantStaff tPS
			ON tPH.Operator = tPS.StaffID AND tPH.Facility = tPS.Facility
				WHERE (@vchFacility IS NULL OR tPH.Facility = @vchFacility) 
				AND (@vchMachineID IS NULL OR tPH.DefaultPkgLine = @vchMachineID)
				AND (@vchOperator IS NULL OR tPH.Operator = @vchOperator)
				AND (@intShopOrder IS NULL OR tPH.ShopOrder = @intShopOrder)
				AND (@vchItemNumber IS NULL OR tPH.ItemNumber = @vchItemNumber)
				AND tPH.ShiftProductionDate BETWEEN @dteFromProdDate AND @dteToProdDate
				AND tPS.StaffClass <> 'SetUp'
			UNION
			SELECT tP.*
			FROM tblPallet tP
			LEFT OUTER JOIN tblPlantStaff tPS
			ON tP.Operator = tPS.StaffID AND tP.Facility = tPS.Facility
				WHERE (@vchFacility IS NULL OR tP.Facility = @vchFacility) 
				AND (@vchMachineID IS NULL OR tP.DefaultPkgLine = @vchMachineID)
				AND (@vchOperator IS NULL OR tP.Operator = @vchOperator)
				AND (@intShopOrder IS NULL OR tP.ShopOrder = @intShopOrder)
				AND (@vchItemNumber IS NULL OR tP.ItemNumber = @vchItemNumber)
				AND tP.ShiftProductionDate BETWEEN @dteFromProdDate AND @dteToProdDate
				AND tPS.StaffClass <> 'SetUp'

	END
	ELSE
	BEGIN
			
		INSERT INTO @tblPalletHst
		SELECT tPH.*
		FROM tblPalletHst tPH
		LEFT OUTER JOIN tblPlantStaff tPS
			ON tPH.Operator = tPS.StaffID AND tPH.Facility = tPS.Facility
		LEFT OUTER JOIN vwLineWorkShiftType vLWT
			ON tPH.Facility = vLWT.Facility AND tPH.DefaultPkgLine =  vLWT.PackagingLine
		LEFT OUTER JOIN tblshift tS
			ON tPH.Facility = tS.Facility AND tPH.ShiftNo = tS.Shift AND ISNULL(vLWT.WorkShiftType,'P') = tS.WorkGroup 
		WHERE (@vchFacility IS NULL OR tPH.Facility = @vchFacility) 
			AND (@vchMachineID IS NULL OR tPH.DefaultPkgLine = @vchMachineID)
			AND (@vchOperator IS NULL OR tPH.Operator  = @vchOperator)
			AND (@intShopOrder IS NULL OR tPH.ShopOrder = @intShopOrder)
			AND (@vchItemNumber IS NULL OR tPH.ItemNumber = @vchItemNumber)
			AND Convert(varchar(8),tPH.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) 
				BETWEEN convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) AND convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
			AND tPS.StaffClass <> 'SetUp'
		UNION
		SELECT tP.*
		FROM tblPallet tP
		LEFT OUTER JOIN tblPlantStaff tPS
			ON tP.Operator = tPS.StaffID AND tP.Facility = tPS.Facility
		LEFT OUTER JOIN vwLineWorkShiftType vLWT
			ON tP.Facility = vLWT.Facility AND tP.DefaultPkgLine =  vLWT.PackagingLine
		LEFT OUTER JOIN tblshift tS
			ON tP.Facility = tS.Facility AND tP.ShiftNo = tS.Shift AND ISNULL(vLWT.WorkShiftType,'P') = tS.WorkGroup 
		WHERE (@vchFacility IS NULL OR tP.Facility = @vchFacility) 
			AND (@vchMachineID IS NULL OR tP.DefaultPkgLine = @vchMachineID)
			AND (@vchOperator IS NULL OR tP.Operator  = @vchOperator)
			AND (@intShopOrder IS NULL OR tP.ShopOrder = @intShopOrder)
			AND (@vchItemNumber IS NULL OR tP.ItemNumber = @vchItemNumber)
			AND Convert(varchar(8),tP.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) 
				BETWEEN convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) AND convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
			AND tPS.StaffClass <> 'SetUp'

	END

	RETURN 
END

GO

