
-- =============================================
-- Author:		Bong Lee
-- Create date: Oct 17, 2008
-- Description:	Weight Log Inquiry
-- Mod.			Date			Author
-- WO#103		Apr.06 2010		Bong Lee
-- Description: To allow to filter by shop order, operator and weight source type
-- WO#194:		Sep. 9, 2010	Bong Lee	
-- Description:	If the time frame for the expected shift crosses mid-night
--				and the given time is prior to mid-night, the Shift Production 
--				Date will be the given time plus one day. Before this change the 	
--				Shift Production Date for this case was same as the given time but 
--				if the given time after mid-night, the result was given time 
--				minus one day.
--				Add new parameter: Item Number
-- WO#262:		Dec. 17, 2010	Bong Lee	
-- Description:	Accept an internally assigned weight source 99 that
--				means excluding tareweight (i.e. <> 02), the change is
--				actually not related to the WO but will be implemented with.
-- WO#359:		Jan. 30, 2011	Bong Lee
-- Description: use the standard function tfnSessionControlHstDetail to filter transactions
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_WeightLog] 
	-- Add the parameters for the stored procedure here
	@vchAction varchar(50),
	@vchFacility varchar(3),
	@intPercentTolerence int = 100,
	@vchPackagingLine varchar(10) = NULL,
	@decLabelWeight decimal(6,1) = NULL,
	@intShopOrder int = NULL,
	@vchOperator varchar(10) = NULL,
	@vchItemNumber varchar(35) = NULL,	-- WO#194
	@dteFromProdDate as DateTime,
	@intFromShift as tinyint,
	@dteToProdDate as DateTime,
	@intToShift as tinyint,
-- WO#262 @intWgtSource as tinyint = 0	-- default source from Auto
	@intWgtSource as tinyint = 99	-- default source is all weights exclude tareweight

AS
BEGIN

	SET NOCOUNT ON;

-- WO#194 Add Start 
	/* Find the Work Shift type for each line - if line is duplicated, pick the active one */
	Declare @tblComputerConfig Table
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
	Group By T1.Facility, T1.Packagingline, T1.WorkShiftType;
-- WO#194 Add Stop

	IF @vchAction = 'Summary'
		SELECT tEQ.Description, tWL.PackagingLine, tWL.LabelWeight, tWL.TargetWeight, count(*) as SampleCounts, avg(ActualWeight) as Mean ,avg(tWL.ActualWeight-tWL.LabelWeight) as OverPackLabel, 
			avg(tWL.ActualWeight-tWL.TargetWeight) as OverPackTarget, min(tWL.ActualWeight) as minWgt, max(tWL.ActualWeight) as maxWgt, stdev(tWL.actualweight) as StdDev
		FROM dbo.tblWeightLog tWL
		INNER JOIN dbo.tfnSessionControlHstDetail(NULL, @vchFacility, @vchPackagingLine, @vchOperator, @intShopOrder, @vchItemNumber, @dteFromProdDate, @intFromShift,@dteToProdDate, @intToShift) tSCH
-- WO#359 left outer join tblSessionControlHst tSCH 
			on tWL.Facility = tSCH.Facility and tWL.ShopOrderStartTIme = tSCH.StartTime and tWL.PackagingLine = tSCH.DefaultPkgLine
		left outer join tblequipment tEQ 
			on tWL.Facility = tEQ.Facility and tWL.PackagingLine = tEQ.EquipmentID
-- WO#359 DEL Start
-- WO#194 Add Start 
		--Left Outer Join @tblComputerConfig tCC
		--ON tSCH.Facility = tCC.Facility AND tSCH.OverridePkgLine =  tCC.PackagingLine
		--Left Outer Join tblShift tS
		--ON tSCH.Facility = tS.Facility AND tSCH.OverrideShiftNo = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
-- WO#194 Add End
-- WO#359 DEL End
		WHERE -- tWL.TimeTest Between @dteFromTime and @dteToTime 
-- WO#359	tWL.Facility = @vchFacility 
-- WO#359	AND	Convert(varchar(8),tSCH.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
-- WO#194	AND	Convert(varchar(8),tSCH.ShiftProductionDate,112) + Cast(tSCH.OverrideShiftNo as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
			ABS(tWL.ActualWeight - tWL.TargetWeight) <= (@intPercentTolerence * tWL.TargetWeight/100)
-- WO#359	AND ABS(tWL.ActualWeight - tWL.TargetWeight) <= (@intPercentTolerence * tWL.TargetWeight/100)
-- WO#359	AND (@vchPackagingLine IS NULL OR tWL.PackagingLine = @vchPackagingLine) 
			AND (@decLabelWeight IS NULL OR tWL.LabelWeight = @decLabelWeight)
-- WO#359	AND (@intShopOrder is NULL OR tWL.ShopOrder = @intShopOrder)
-- WO#359	AND (@vchOperator is NULL or tSCH.Operator = @vchOperator)
-- WO#359	AND (@vchItemNumber is NULL or tSCH.ItemNumber = @vchItemNumber)	-- WO#194
-- WO#262	AND tWL.WeightSource = @intWgtSource
			AND (tWL.WeightSource = case when @intWgtSource = 99 then 0 else @intWgtSource end		-- WO#262
				OR tWL.WeightSource = case when @intWgtSource = 99 then 1 else @intWgtSource end)	-- WO#262
		GROUP BY tEQ.Description, tWL.PackagingLine, tWL.LabelWeight, tWL.TargetWeight
		ORDER BY tEQ.Description
	
END

GO

