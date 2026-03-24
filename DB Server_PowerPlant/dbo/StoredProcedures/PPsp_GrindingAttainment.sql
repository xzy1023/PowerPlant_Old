

-- =============================================
-- Author:		Bong Lee
-- Create date: May 22, 2008
-- Description:	Grinding Attainment
-- WO#0001		Other weights should be counted only once per schedule.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_GrindingAttainment]
	@vchAction varchar(50),
	@vchFacility as varchar(3),
	@dteFromDate as DateTime,
	@dteToDate as DateTime

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);

	DECLARE 	@vchFromDate as varchar(8),	@vchToDate as varchar(8);
	DECLARE @dteDate as DateTime

	BEGIN TRY

	SET @vchFromDate = CONVERT( varchar(8),@dteFromDate,112)
	SET @vchToDate = CONVERT( varchar(8),@dteToDate,112)

	-- Create Grinding Schedule work file
	CREATE TABLE #tblGS (SeqNo int, RcdCount int, Facility varchar(3), Grinder varchar(10), ScheduleID varchar(50), Blend varchar(6),
		Grind varchar(6), StartTime varchar(30), EndTime varchar(30), ToBin varchar(10), ToLine char(10),
		ProductionDate int, ScheduledWgt int)

	INSERT INTO #tblGS
	SELECT ROW_NUMBER() OVER(PARTITION BY GSCPDTE ORDER BY GSCSDTE, GSCSTIM) AS SeqNo,
		COUNT(*) OVER(PARTITION BY GSCPDTE ) AS Rcdcount, GSCFAC, GSCGRNDR, GSCACTI, GSCBLEND, GSCGRIND,
		STUFF(STUFF(CAST(GSCSDTE AS CHAR(8)),5,0,'-'),8,0,'-') + ' ' + CONVERT(CHAR(8),dbo.fnCvtNumTimeToDateTime(GSCSTIM),108),
		STUFF(STUFF(CAST(GSCEDTE AS CHAR(8)),5,0,'-'),8,0,'-') + ' ' + CONVERT(CHAR(8),dbo.fnCvtNumTimeToDateTime(GSCETIM),108),
		GSCTBIN, GSCPKLIN, GSCPDTE, SUM(GSCQTYI) AS GSCQTYI FROM dbo.tblGrindingSchedule tGS 
	WHERE tGS.GSCFAC = @vchFacility  AND tGS.GSCPDTE Between  @vchFromDate  AND  @vchToDate 
	GROUP BY GSCFAC,GSCGRNDR,GSCACTI,GSCBLEND,GSCGRIND,GSCSDTE,GSCSTIM,GSCEDTE,GSCETIM,GSCTBIN,GSCPKLIN,GSCPDTE
	ORDER BY tGS.GSCPDTE, tGS.GSCSDTE, tGS.GSCSTIM

--SELECT * FROM #tblGS
	-- Create Grinding Log work file
	CREATE TABLE #tblGL (ScheduleID varchar(50), Grinder varchar(10), Blend varchar(6),
			Grind varchar(6), EquipmentType char(1), ToBin varchar(10), ToLine char(10), Shift tinyint, 
			MinColour decimal(5,2), TargetColour decimal(5,2), MaxColour decimal(5,2), ActualWgt int,
			Rejected int, RejectedWgt int, StartTime DateTime, EndTime DateTime, OtherWgt int, TotalWgt int, Dropped char(1))

	INSERT INTO #tblGL
	SELECT T1.ScheduleID, T1.Grinder, T1.Blend, T1.Grind, T1.EquipmentType, T1.Bin, T1.EquipmentID, T1.Shift, 
		T1.MinColour, T1.TargetColour, T1.MaxColour, T1.ActualWgt, T1.Rejected, T1.RejectedWgt,
		--WO#0001 T1.StartTime, T1.EndTime, ISNULL(T3.ReworkWgt,0) + ISNULL(T3.SpillageWgt,0) AS OtherWgt, 
		--WO#0001 T1.ActualWgt + ISNULL(T3.ReworkWgt,0) + ISNULL(T3.SpillageWgt,0) - T1.RejectedWgt AS TotalWgt, ISNULL(T4.Dropped,'N') as Dropped
		T1.StartTime, T1.EndTime, CASE WHEN ROW_NUMBER() OVER(PARTITION BY T1.ScheduleID ORDER BY T1.StartTime DESC) = 1 THEN ISNULL(T3.ReworkWgt,0) + ISNULL(T3.SpillageWgt,0) ELSE 0 END AS OtherWgt,	-- WO#0001
		T1.ActualWgt + CASE WHEN ROW_NUMBER() OVER(PARTITION BY T1.ScheduleID ORDER BY T1.StartTime DESC) = 1 THEN ISNULL(T3.ReworkWgt,0) + ISNULL(T3.SpillageWgt,0) ELSE 0 END - T1.RejectedWgt AS TotalWgt, -- WO#0001
		ISNULL(T4.Dropped,'N') as Dropped		-- WO#0001
	FROM (SELECT  tGL.ScheduleID , tGS.ProductionDate, tGL.Grinder, tGL.Blend, tGL.Grind, tGL.EquipmentType, tGL.Bin, tGL.EquipmentID, tGL.Shift, SUM(ActualWgt) AS ActualWgt, 
			MIN(tGL.StartTime) AS StartTime, MAX(ISNULL(tGL.EndTime,'12/31/9999')) AS EndTime, tGL.MinColour, tGL.TargetColour, tGL.MaxColour, 
			MAX(CAST(Rejected AS integer)) AS Rejected, SUM(RejectedWgt) AS RejectedWgt
			FROM tblGrindingLog tGL
			INNER JOIN #tblGS tGS 
			ON tGL.ScheduleID = tGS.ScheduleID
			--WHERE tGS.Facility = @vchFacility AND tGS.ProductionDate Between  @vchFromDate  AND  @vchToDate 
			WHERE tGL.Active = 1
			--WHERE Active = 1 AND Facility = @vchFacility and StartTime BETWEEN @dteFmDateTime AND @dteToDateTime
			GROUP BY tGL.ScheduleID, tGS.ProductionDate, tGL.Grinder, tGL.Blend, tGL.Grind, tGL.EquipmentType, tGL.Bin, tGL.EquipmentID, tGL.Shift, tGL.MinColour, tGL.TargetColour, tGL.MaxColour) AS T1
	LEFT OUTER JOIN tblOtherWeights T3 ON T1.ScheduleID = t3.ScheduleID
	LEFT OUTER JOIN (SELECT ScheduleID, 'Y' as Dropped FROM tblGrindingLog Where Status = 'Dropped') T4 
		ON T1.ScheduleID = T4.ScheduleID 

--SELECT * FROM #tblGL
	-- Create a weight tolerent table for the reporting period
	CREATE TABLE #tblGT (EffectiveDate int ,PosTolerance Decimal(4,3), NegTolerance Decimal(4,3))

	SET @dteDate = @dteFromDate
	WHILE @dteDate <= @dteToDate
	BEGIN
		INSERT INTO #tblGT
		SELECT CAST(CONVERT(varchar(8),@dteDate,112) as int) ,PosTolerance, NegTolerance FROM dbo.tblGrindWeightTolerance 
			WHERE EffectiveDate = 			
		(SELECT MAX(EffectiveDate) FROM dbo.tblGrindWeightTolerance WHERE ACTIVE = 1
		AND EffectiveDate <= @dteDate)
		SET @dteDate = DATEADD(d,1,@dteDate)
	END

--SELECT * FROM #tblGT
	-- Create work file for out of sequence grinding process.
	CREATE TABLE #tblSeq (ScheduleID varchar(50), ActSeqNo int, OffSeq tinyint)
	DECLARE @vchGS_ScheduleID as varchar(50), @intGS_SeqNo as int, @intGL_SeqNo as int, @intGL_PreviousSeqNo as int
	DECLARE @chrOutOfSeq as Char(1) --, @intDropped as tinyint, @intLastDropped as tinyint
	DECLARE seq_cursor CURSOR FOR
		SELECT tGS.ScheduleID, tGS.SeqNo, ROW_NUMBER() OVER(PARTITION BY tGS.ProductionDate ORDER BY tGL.StartTime) AS 'SeqNo'--,tGL.Dropped
			FROM #tblGS tGS
			LEFT OUTER JOIN (SELECT ScheduleID, MIN(StartTime) as StartTime 
					--, Max(CASE WHEN status = 'Dropped' THEN 1 ELSE 0 END) as Dropped FROM tblGrindingLog WHERE Active = 1 AND StartTime IS NOT NULL
					FROM #tblGL WHERE StartTime IS NOT NULL
					GROUP BY ScheduleID) tGL
					ON tGS.ScheduleID = tGL.ScheduleID
			WHERE tGL.StartTime IS NOT NULL
			ORDER BY tGS.ProductionDate, tGS.SeqNo
	OPEN seq_cursor

	-- read the frist record
	FETCH NEXT FROM seq_cursor INTO @vchGS_ScheduleID, @intGS_SeqNo, @intGL_SeqNo --, @intDropped

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @intGS_SeqNo = 1 
			BEGIN
				IF @intGL_SeqNo IS NULL OR @intGL_SeqNo <> 1 
					SET @chrOutOfSeq = 1
				ELSE
					SET @chrOutOfSeq = 0
			END
		ELSE
			BEGIN
				IF @intGL_SeqNo IS NULL OR @intGL_SeqNo <> @intGL_PreviousSeqNo + 1
			
					SET @chrOutOfSeq = 1
				ELSE
					SET @chrOutOfSeq = 0
			END

		-- SET @intLastDropped = @intDropped 
			
		INSERT INTO [#tblSeq] (ScheduleID, ActSeqNo, OffSeq) VALUES (@vchGS_ScheduleID,@intGL_SeqNo,@chrOutOfSeq)
		SET @intGL_PreviousSeqNo = ISNULL(@intGL_SeqNo,0)

		FETCH NEXT FROM seq_cursor INTO @vchGS_ScheduleID, @intGS_SeqNo, @intGL_SeqNo --, @intDropped
	END

	CLOSE seq_cursor
	DEALLOCATE seq_cursor

--SELECT * FROM #tblSEQ

-- Calculate ToBin Changed
	CREATE TABLE #tblToBinChanged (ScheduleID varchar(50) ,ToBinChanged tinyint)
	INSERT INTO #tblToBinChanged
	SELECT tGS.ScheduleID,SUM(CASE WHEN tGL.Dropped = 'Y' OR (tGL.ToBin IS NOT NULL AND tGL.ToBin <> tGS.ToBin) THEN 1 ELSE 0 END) AS ToBinChanged
		FROM #tblGS tGS
		LEFT OUTER JOIN #tblGL tGL ON tGS.ScheduleID = tGL.ScheduleID
		GROUP BY tGS.ScheduleID
		ORDER BY tGS.ScheduleID
-- select * from #tblToBinChanged

-- Attain the Start Time, End Time and weight 
	CREATE TABLE #tblAttainOthers (ScheduleID varchar(50) ,StartedLate tinyInt,  EndedLate tinyInt, DurationFail tinyInt, QtyFail tinyInt)
	
	INSERT INTO #tblAttainOthers
	SELECT T1.ScheduleID,
		CASE WHEN T1.StartTime = '9999-12-31' OR T1.StartTime > tGS.StartTime THEN 1 ELSE 0 END AS StartedLate,
		CASE WHEN T1.EndTime > tGS.EndTime THEN 1 ELSE 0 END AS EndedLate,
		CASE WHEN T1.EndTime = '9999-12-31' OR DateDiff(mi,T1.StartTime,T1.EndTime) > DateDiff(mi,tGS.StartTime, tGS.EndTime) THEN 1 ELSE 0 END AS DurationFail,
		CASE WHEN (T1.ActualWgt + ISNULL(tOW.ReworkWgt,0) + ISNULL(tOW.SpillageWgt,0) - T1.RejectedWgt) - tGS.ScheduledWgt > tGT.posTolerance * tGS.ScheduledWgt OR tGS.ScheduledWgt - (T1.ActualWgt + ISNULL(tOW.ReworkWgt,0) + ISNULL(tOW.SpillageWgt,0) - T1.RejectedWgt) > tGT.NegTolerance  * tGS.ScheduledWgt THEN 1 ELSE 0 END AS QtyFail
		FROM #tblGS tGS
		LEFT OUTER JOIN tblOtherWeights tOW ON tGS.ScheduleID = tOW.ScheduleID
		LEFT OUTER JOIN #tblGT tGT ON tGS.ProductionDate = tGT.EffectiveDate
		LEFT OUTER JOIN
		(SELECT tGS.ScheduleID, SUM(CASE WHEN tGL.Status IS NULL OR tGL.Status='Dropped' THEN 0 ELSE tGL.ActualWgt END) AS ActualWgt, 
				MIN(CASE WHEN tGL.Status IS NULL OR tGL.Status='Dropped' THEN '12/31/9999' ELSE tGL.StartTime END) AS StartTime, MAX(ISNULL(tGL.EndTime,'12/31/9999')) AS EndTime, 
				SUM(tGL.RejectedWgt) AS RejectedWgt
				FROM #tblGS tGS 
				INNER JOIN tblGrindingLog tGL ON tGS.ScheduleID = tGL.ScheduleID
				WHERE tGL.Active = 1
				GROUP BY tGS.ScheduleID) T1 ON tGS.ScheduleID = T1.ScheduleID

--select * from #tblAttainOthers

	IF @vchAction = 'Detail'
--		SELECT TGS.ProductionDate, tGS.ScheduleID, tGL.Dropped, tGS.SeqNo, tGL.SeqNo AS ActSeqNo, tSEQ.OffSeq, tGS.Facility,tGS.Grinder,tGL.Grinder as ActGrinder, tGS.Blend, tGS.Grind, tGS.StartTime, tGL.StartTime as ActStartTime,
--			tGS.EndTime, tGL.EndTime as ActEndTime, tGS.ToBin, tGL.ToBin as ActToBin, tGS.ToLine, tGL.ToLine as ActToLine, tGS.ScheduledWgt,tGL.TotalWgt,
--			CASE WHEN tGL.Dropped IS NULL OR tGL.Dropped = 'Y' OR tGL.EndTime = '9999-12-31' OR DateDiff(mi,tGL.StartTime,tGL.EndTime) > DateDiff(mi,tGS.StartTime, tGS.EndTime) THEN 'Y' ELSE 'N' END AS DurationFail,
--			CASE WHEN tGL.Dropped IS NULL OR tGL.Dropped = 'Y' OR tGL.StartTime > tGS.StartTime THEN 'Y' ELSE 'N' END AS StartedLate,
--			CASE WHEN tGL.Dropped IS NULL OR tGL.Dropped = 'Y' OR tGL.EndTime > tGS.EndTime THEN 'Y' ELSE 'N' END AS EndedLate,
--			CASE WHEN tGL.Dropped IS NULL OR tGL.Dropped = 'Y' OR tGL.ToBin <> tGS.ToBin THEN 'Y' ELSE 'N' END AS ToBinChanged,
--			CASE WHEN tGL.Dropped IS NULL OR tGL.Dropped = 'Y' OR tGL.TotalWgt - tGS.ScheduledWgt > posTolerance * tGS.ScheduledWgt or tGS.ScheduledWgt - tGL.TotalWgt  > NegTolerance  * tGS.ScheduledWgt THEN 'Y' ELSE 'N' END AS QtyFail
--		FROM #tblGS tGS
--		LEFT OUTER JOIN #tblGL tGL ON tGS.ScheduleID = tGL.ScheduleID
--		LEFT OUTER JOIN #tblGT tGT ON tGS.ProductionDate = tGT.EffectiveDate
--		LEFT OUTer JOIN #tblSeq tSEQ ON tGS.ScheduleID = tSEQ.ScheduleID
		SELECT  TGS.ProductionDate, tGS.ScheduleID, tGL.Dropped , tGS.SeqNo, tSeq.ActSeqNo, tGS.Facility, tGS.Grinder, tGL.Grinder as ActGrinder,
			tGS.Blend, tGS.Grind, tGS.StartTime, CONVERT(varchar(21), tGL.StartTime,120) as ActStartTime,
			tGS.EndTime, CASE WHEN tGL.EndTime = '12/31/9999' THEN NULL ELSE CONVERT(varchar(21), tGL.EndTime,120) END as ActEndTime, tGS.ToBin, 
			tGL.ToBin as ActToBin, tGS.ToLine, tGL.ToLine as ActToLine, tGS.ScheduledWgt,tGL.TotalWgt,
			CASE WHEN DurationFail = 1 THEN 'Y' ELSE 'N' END AS DurationFail,
			CASE WHEN StartedLate = 1 THEN 'Y' ELSE 'N' END AS StartedLate,
			CASE WHEN EndedLate = 1 THEN 'Y' ELSE 'N' END AS EndedLate,
			CASE WHEN ToBinChanged > 0 THEN 'Y' ELSE 'N' END AS ToBinChanged,
			CASE WHEN QtyFail = 1 THEN 'Y' ELSE 'N' END AS QtyFail,
			CASE WHEN tGL.Dropped = 'Y' OR tSEQ.OffSeq = 1 THEN 'Y' ELSE 'N' END AS OffSeq
		FROM #tblGS tGS
		LEFT OUTER JOIN #tblGL tGL ON tGS.ScheduleID = tGL.ScheduleID
		LEFT OUTER JOIN #tblGT tGT ON tGS.ProductionDate = tGT.EffectiveDate
		LEFT OUTER JOIN #tblSeq tSEQ ON tGS.ScheduleID = tSEQ.ScheduleID
		LEFT OUTER JOIN #tblToBinChanged tTBC ON tGS.ScheduleID = tTBC.ScheduleID
		LEFT OUTER JOIN #tblAttainOthers tTAO ON tGS.ScheduleID = tTAO.ScheduleID 
	ELSE
		BEGIN
--			SELECT ProductionDate, count(*) as RcdCount, SUM(DurationFail) as DurationFail, SUM(StartedLate) as StartedLate, SUM(EndedLate) as EndedLate,  SUM(ToBinChanged) as ToBinChanged, SUM(QtyFail) as QtyFail,  Sum(OffSeq) as OffSeq From
--			(SELECT TGS.ProductionDate, tGS.ScheduleID, 
--				CASE WHEN tGL.Dropped IS NULL OR tGL.Dropped = 'Y' OR tGL.EndTime = '9999-12-31' OR DateDiff(mi,tGL.StartTime,tGL.EndTime) > DateDiff(mi,tGS.StartTime, tGS.EndTime) THEN 1 ELSE 0 END AS DurationFail,
--				CASE WHEN tGL.Dropped IS NULL OR tGL.Dropped = 'Y' OR tGL.StartTime > tGS.StartTime THEN 1 ELSE 0 END AS StartedLate,
--				CASE WHEN tGL.Dropped IS NULL OR tGL.Dropped = 'Y' OR tGL.EndTime > tGS.EndTime THEN 1 ELSE 0 END AS EndedLate,
--				CASE WHEN tGL.Dropped IS NULL OR tGL.Dropped = 'Y' OR tGL.ToBin <> tGS.ToBin THEN 1 ELSE 0 END AS ToBinChanged,
--				CASE WHEN tGL.Dropped IS NULL OR tGL.Dropped = 'Y' OR tGL.TotalWgt - tGS.ScheduledWgt > posTolerance * tGS.ScheduledWgt or tGS.ScheduledWgt - tGL.TotalWgt  > NegTolerance  * tGS.ScheduledWgt THEN 1 ELSE 0 END AS QtyFail,
--				CASE WHEN tGL.Dropped IS NULL OR tGL.Dropped = 'Y' OR tSEQ.OffSeq = 'Y' THEN 1 ELSE 0 END AS OffSeq
--			FROM #tblGS tGS
--			LEFT OUTER JOIN #tblGL tGL ON tGS.ScheduleID = tGL.ScheduleID
--			LEFT OUTER JOIN #tblGT tGT ON tGS.ProductionDate = tGT.EffectiveDate
--			LEFT OUTer JOIN #tblSeq tSEQ ON tGS.ScheduleID = tSEQ.ScheduleID) AS T1
--			Group By ProductionDate
			SELECT tGS.ProductionDate, count(*) as RcdCount, SUM(tAO.DurationFail) as DurationFail, SUM(tAO.StartedLate) as StartedLate, SUM(tAO.EndedLate) as EndedLate,
			  SUM(CASE WHEN tTBC.ToBinChanged > 1 Then 1 ELSE tTBC.ToBinChanged END) as ToBinChanged, SUM(tAO.QtyFail) as QtyFail, Sum(CASE WHEN tGL.Dropped = 'Y' Then 1 ELSE tSEQ.OffSeq END) as OffSeq
			FROM #tblGS tGS
			LEFT OUTER JOIN (SELECT ScheduleID,Dropped FROM #tblGL WHERE dropped = 'Y') tGL ON tGS.ScheduleID = tGL.ScheduleID
			LEFT OUTER JOIN #tblGT tGT ON tGS.ProductionDate = tGT.EffectiveDate
			LEFT OUTER JOIN #tblSeq tSEQ ON tGS.ScheduleID = tSEQ.ScheduleID
			LEFT OUTER JOIN #tblToBinChanged tTBC ON tGS.ScheduleID = tTBC.ScheduleID
			LEFT OUTER JOIN #tblAttainOthers tAO ON tGS.ScheduleID = tAO.ScheduleID
			Group By ProductionDate
		END

	Drop Table #tblSeq
	Drop Table #tblGT	
	Drop Table #tblGS
	Drop Table #tblGL
	Drop Table #tblToBinChanged
	Drop Table #tblAttainOthers

	END TRY

	BEGIN CATCH
		BEGIN TRY Drop Table #tblSeq END TRY BEGIN CATCH END CATCH
		BEGIN TRY Drop Table #tblGT END TRY BEGIN CATCH END CATCH
		BEGIN TRY Drop Table #tblGS END TRY BEGIN CATCH END CATCH
		BEGIN TRY Drop Table #tblGL END TRY BEGIN CATCH END CATCH
		BEGIN TRY Drop Table #tblToBinChanged END TRY BEGIN CATCH END CATCH
		BEGIN TRY Drop Table #tblAttainOthers END TRY BEGIN CATCH END CATCH
		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorNumber = ERROR_NUMBER(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');

		RAISERROR 
			(
			@ErrorMessage, 
			@ErrorSeverity, 
			@ErrorState,     -- parameter: original error state.               
			@ErrorNumber,    -- parameter: original error number.
			@ErrorSeverity,  -- parameter: original error severity.
			@ErrorProcedure, -- parameter: original error procedure name.
			@ErrorLine       -- parameter: original error line number.
			);
	END CATCH
END

GO

