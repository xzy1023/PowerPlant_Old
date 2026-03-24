



-- =============================================
-- Author:		Bong Lee
-- Create date: Oct 19, 2009
-- Description:	Down Time Summary by Line and shift
-- WO#194:		Sep. 9, 2010	Bong Lee	
-- Description:	If the time frame for the expected shift crosses mid-night
--				and the given time is prior to mid-night, the Shift Production 
--				Date will be the given time plus one day. Before this change the 	
--				Shift Production Date for this case was same as the given time but 
--				if the given time after mid-night, the result was given time 
--				minus one day.
-- WO#359		Sep. 06, 2011	Bong Lee
-- Description:	Sort by Down Time Duration in descending order
--				Add a new option to allow to filter by shift in the selected time range.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_DownTimeLogSummary]
	@vchAction varchar(50),					-- WO#359
	@vchFacility varchar(3),
	@vchMachineID varchar(10) = NULL,
	@dteFromProdDate as DateTime,
	@intFromShift as tinyint,
	@dteToProdDate as DateTime,
-- WO#359	@intToShift as tinyint,
	@intToShift as tinyint					-- WO#359
	,@intShiftNoFilter as tinyint = NULL	-- WO#359
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRY

-- WO#359 Del Start 
-- WO#194 Add Start 
--	/* Find the Work Shift type for each line - if line is duplicated, pick the active one */
--	Declare @tblComputerConfig Table
--	(Facility varchar(3) ,Packagingline varchar(10), WorkShiftType varchar(10))
--
--	INSERT INTO @tblComputerConfig
--	SELECT T1.Facility, T1.Packagingline, T1.WorkShiftType 
--	FROM tblcomputerconfig T1 
--	Left Outer Join 
--		(SELECT Facility, Packagingline 
--			FROM tblComputerconfig 
--		WHERE Packagingline <> 'SPARE'
--		Group By Facility, Packagingline
--		Having Count(*) > 1) T2
--	ON T1.Facility = T2.Facility AND T1.Packagingline = T2.Packagingline
--	WHERE (T2.Packagingline is null OR T1.RecordStatus = 1) 
--		AND T1.PackagingLine <> 'SPARE'
--	Group By T1.Facility, T1.Packagingline, T1.WorkShiftType
-- WO#194 Add Stop
-- WO#359 Del Stop

	BEGIN
		Select ROW_NUMBER() OVER (order by tDTL.ReasonCode)as RowNumber,	-- row number is used for showing the reason description in 3 columns.
			CASE WHEN @vchMachineID is Null THEN '' ELSE tDTL.MachineId END as MachineID, -- WO#194
			tDTL.ReasonCode, tRC.Description, count(*) as Occurances, Sum(DateDiff(Minute,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) as DownTime 
		From tbldowntimelog tDTL
-- WO#194 Add Start 
-- WO#359	Left Outer Join @tblComputerConfig tCC
			Left Outer Join dbo.vwLineWorkShiftType tCC			-- WO#359
			ON tDTL.Facility = tCC.Facility AND tDTL.MachineId =  tCC.PackagingLine
			Left Outer Join tblShift tS
			ON tDTL.Facility = tS.Facility AND tDTL.Shift = tS.Shift AND tCC.WorkShiftType  = tS.WorkGroup 
-- WO#194 Add End
		left outer join tblDTReasonCode tRC
		On tDTL.Facility = tRC.Facility AND tDTL.MachineType =  tRC.MachineType 
			And tDTL.MachineSubType =  tRC.MachineSubType And tDTL.ReasonCode =  tRC.ReasonCode
		Where tDTL.Facility = @vchFacility AND InActive = 0 AND (@vchMachineID is Null or tDTL.Machineid =  @vchMachineID)
			AND  tDTL.ReasonType <> 17
-- WO#194	AND Convert(varchar(8),tDTL.ShiftProductionDate,112) + Cast(tDTL.Shift as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1))
			AND Convert(varchar(8),tDTL.ShiftProductionDate,112) + Cast(tS.ShiftSequence as Char(1)) Between convert(varchar(8),@dteFromProdDate,112) + Cast(@intFromShift as char(1)) and convert(varchar(8),@dteToProdDate,112) + Cast(@intToShift as char(1)) -- WO#194
-- WO#194	Group by tDTL.MachineId, tDTL.ReasonCode, tRC.Description
-- WO#194 Add Start
			AND (@intShiftNoFilter is NULL OR tDTL.Shift = @intShiftNoFilter)	-- WO#359
		Group by CASE WHEN @vchMachineID is Null THEN '' ELSE tDTL.MachineId END, 
				tDTL.ReasonCode, tRC.Description
-- WO#359	Order By tDTL.ReasonCode
-- WO#359 Add Start
		Order By 
			CASE WHEN @vchAction = 'ByDuration' THEN
				Sum(DateDiff(Minute,tDTL.DownTimeBegin,tDTL.DownTimeEnd)) 
			END DESC,
			CASE WHEN @vchAction = 'ByOccurrences' THEN
				count(*)
			END DESC,
			CASE WHEN @vchAction = 'ByReasonCode' THEN
				ROW_NUMBER() OVER (order by tDTL.ReasonCode)	
			END
-- WO#359 Add Stop
	END
-- WO#194 Add End

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

