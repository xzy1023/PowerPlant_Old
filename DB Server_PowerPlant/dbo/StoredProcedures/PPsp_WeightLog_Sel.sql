



-- =============================================
-- Author:		Bong Lee
-- Create date: Sep. 28 2010
-- Description:	 Select Weight Log
-- WO#262:		Dec. 17, 2010	Bong Lee	
-- Description:	Accept an internally assigned weight source 99 that
--				means excluding tareweight (i.e. <> 02), the change is
--				actually not related to the WO but will be implemented with.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_WeightLog_Sel] 

	@vchAction varchar(50),
	@vchFacility varchar(3),
	@intPercentTolerence int = 100,
	@vchPackagingLine varchar(10) = NULL,
	@decLabelWeight decimal(6,1) = NULL,
	@intShopOrder int = NULL,
	@vchOperator varchar(10) = NULL,
	@vchItemNumber varchar(35) = NULL,	
	@dteFromProdDate as DateTime,
	@intFromShift as tinyint,
	@dteToProdDate as DateTime,
	@intToShift as tinyint,
-- WO#262 @intWgtSource as tinyint = 0	-- default source from Auto
	@intWgtSource as tinyint = 99	-- default source is all weights exclude tareweight

AS
BEGIN

	SET NOCOUNT ON;

	/* Find the Work Shift type for each line - if line is duplicated, pick the active one */
	Declare @tblComputerConfig TABLE
	(Facility varchar(3) ,Packagingline varchar(10), WorkShiftType varchar(10))

	INSERT INTO @tblComputerConfig
	SELECT T1.Facility, T1.Packagingline, T1.WorkShiftType 
	FROM tblcomputerconfig T1 
	Left Outer Join 
		(SELECT Facility, Packagingline 
			FROM tblComputerconfig 
		WHERE Packagingline <> 'SPARE'
		Group By Facility, Packagingline
		Having Count(*) > 1) T2
	ON T1.Facility = T2.Facility AND T1.Packagingline = T2.Packagingline
	WHERE (T2.Packagingline is null OR T1.RecordStatus = 1) 
		AND T1.PackagingLine <> 'SPARE'
	GROUP BY T1.Facility, T1.Packagingline, T1.WorkShiftType;
	
	IF @vchAction = 'Detail'
		SELECT tEQ.Description, tWL.PackagingLine, tWL.ShopOrder, Case When tSOH.ItemNumber is NULL Then tSCH.ItemNumber ELSE tSOH.ItemNumber END as ItemNumber, tWL.TimeTest, 
			tSCH.Operator, CASE WHEN tPS.StaffID is NULL THEN tSCH.Operator ELSE RTrim(tPS.FirstName) + ' ' + LTrim(tPS.LastName) END as OprName, tWL.LabelWeight, 
			tWL.TargetWeight, tWL.ActualWeight, tWL.ActualWeight-tWL.LabelWeight as OverPackLabel, tWL.ActualWeight-tWL.targetWeight as OverPackTarget
		FROM dbo.tblWeightLog tWL
		Left Outer Join tblSessionControlHst tSCH ON tWL.Facility = tSCH.Facility and tWL.ShopOrderStartTIme = tSCH.StartTime and tWL.PackagingLine = tSCH.DefaultPkgLine
		Left Outer Join @tblComputerConfig tCC
		ON tSCH.Facility = tCC.Facility AND tSCH.OverridePkgLine =  tCC.PackagingLine
		Left Outer Join tblShift tS
		ON tSCH.Facility = tS.Facility AND tSCH.OverrideShiftNo = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
		Left Outer Join tblequipment tEQ ON tWL.Facility = tEQ.Facility and tWL.PackagingLine = tEQ.EquipmentID
		Left Outer Join tblShopOrderHst tSOH ON tWL.ShopOrder = tSOH.ShopOrder
		Left Outer Join tblPlantStaff tPS ON tWL.Facility = tPS.Facility AND tSCH.Operator = tPS.StaffID
		WHERE 
			tWL.Facility = @vchFacility 
			AND	Convert(varchar(8),tSCH.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))	-- WO#194
			AND (@vchPackagingLine IS NULL OR tWL.PackagingLine = @vchPackagingLine) 
			AND (@decLabelWeight IS NULL OR tWL.LabelWeight = @decLabelWeight)
			AND ABS(ActualWeight-TargetWeight) <= (@intPercentTolerence * TargetWeight/100)
			AND (tPS.WorkGroup = 'P' or tPS.WorkGroup = 'ALL')
			AND (@intShopOrder is NULL OR tWL.ShopOrder = @intShopOrder)
			AND (@vchOperator is NULL or tSCH.Operator = @vchOperator)
			AND (@vchItemNumber is NULL or tSCH.ItemNumber = @vchItemNumber) -- WO#194
-- WO#262	AND tWL.WeightSource = @intWgtSource
			AND (tWL.WeightSource = case when @intWgtSource = 99 then 0 else @intWgtSource end		-- WO#262
				OR tWL.WeightSource = case when @intWgtSource = 99 then 1 else @intWgtSource end)	-- WO#262
		ORDER BY tWL.TimeTest
	
END

GO

