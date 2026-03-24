

-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 17, 2007
-- Description:	Grinding Schedule Selection
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_GrindingSchedule_Sel]
	@vchAction varchar(50),
	@vchGrinder varchar(6) = NULL,
	@vchFacility as varchar(3),
	@vchFromDate as varchar(8),
	@vchToDate as varchar(8),
	@vchGrindingStatus as varchar(10),
	@vchScheduleID as varchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @vchGrindingSchdleSQLStmt varchar(2400);
	DECLARE @vchSQLStmt  varchar(2400);
	DECLARE @vchGLogSQLStmt varchar(500);
	DECLARE @vchADHocSchSQLStmt  varchar(500);

	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);

	BEGIN TRY
		IF @vchAction = 'Consolidated'
		BEGIN
		-- Select grinding schedule from iSeries
			Set @vchGrindingSchdleSQLStmt ='SELECT T5.*, ISNULL(T6.GSCFBIN,''Multi'') AS GSCFBIN FROM (SELECT GSCFAC, GSCGRNDR, GSCACTI, GSCBLEND, GSCGRIND, SUM(GSCQTYI) AS GSCQTYI, ' + 
				'SUBSTRING(CAST(GSCSDTE AS CHAR(8)),3,6) AS GSCSDTE, GSCSTIM/100 AS GSCSTIM, ' + 
				'SUBSTRING(CAST(GSCEDTE AS CHAR(8)),3,6) AS GSCEDTE, GSCETIM/100 AS GSCETIM, GSCTBIN, GSCPKLIN, GSCPDTE ' +
				'FROM tblGrindingSchedule WHERE ' +
				'GSCFAC = ''' + @vchFacility + ''' AND GSCPDTE Between ' +  @vchFromDate + ' AND ' + @vchToDate +
				' GROUP BY GSCFAC,GSCGRNDR,GSCACTI,GSCBLEND,GSCGRIND,GSCSDTE,GSCSTIM,GSCEDTE,GSCETIM,GSCTBIN,GSCPKLIN,GSCPDTE) as T5 ' +
				' LEFT OUTER JOIN ' +
				'(SELECT GSCFAC,GSCACTI,MIN(GSCFBIN) AS GSCFBIN FROM tblGrindingSchedule ' +
				'WHERE GSCFAC = ''' + @vchFacility + ''' AND GSCPDTE Between ' +  @vchFromDate + ' AND ' + @vchToDate +
				' GROUP BY GSCFAC,GSCACTI, GSCTBIN having count(*) = 1) as T6 ' +
				'ON T5.GSCFAC = T6.GSCFAC and T5.GSCACTI = T6.GSCACTI '
			-- WHERE Condition for grinding schedule from iSeries
			IF Not( @vchGrinder is NULL )
				Set @vchGrindingSchdleSQLStmt =  @vchGrindingSchdleSQLStmt + ' WHERE GSCGRNDR = ''' +  @vchGrinder + ''''
			-- SORT sequence for grinding schedule from iSeries
	--		SET @vchGrindingSchdleSQLStmt = @vchGrindingSchdleSQLStmt + ' Order by GSCPDTE, GSCSDTE, GSCSTIM'

			-- Select Grinding Log 
			IF @vchGrindingStatus <> 'Ready' 
			BEGIN
				SET @vchGLogSQLStmt = 'SELECT Status, Finalized, T4.* from ' +
					'(SELECT Facility, ScheduleID, MAX(GrindJobID) as GrindJobID, SUM(ActualWgt) as TotalActualWgt, ' + 
					' SUM(RejectedWgt) as TotalRejectedWgt from dbo.tblGrindingLog '
				-- WHERE Condition for Grinding Log 
				SET @vchGLogSQLStmt =  @vchGLogSQLStmt + ' WHERE Active = 1 AND Facility = ''' + @vchFacility + ''''
				--IF @vchGrindingStatus <> 'All'
				--	SET @vchGLogSQLStmt =  @vchGLogSQLStmt + ' AND Status = ''' + @vchGrindingStatus + ''''
--				IF Not( @vchGrinder is NULL )
--					Set @vchGLogSQLStmt =  @vchGLogSQLStmt + ' AND Grinder = ''' + @vchGrinder  + ''''
				-- Grouping for Grinding Log 
				SET @vchGLogSQLStmt = @vchGLogSQLStmt + ' group by Facility, ScheduleID) T4 ' +
					'LEFT OUTER JOIN tblGrindingLog T5 on T4.GrindJobID = T5.GrindJobID '
			END

			-- Select Ad-Hoc schedule 
			SET @vchADHocSchSQLStmt = 'SELECT Facility, Grinder, ScheduleID, BLEND, GRIND, ScheduledWgt, ' +
							'SUBSTRING(CAST(StartDate AS CHAR(8)),3,6) as StartDate, ' + 
							'StartTime/100 AS StartTime, ' + 
							'SUBSTRING(CAST(EndDate AS CHAR(8)),3,6) as EndDate, ' +
							'EndTime/100 AS EndTime , ToBin, EquipmentID, ProductionDate, FromTank ' +
							'FROM dbo.tblAdHocGrindingSchedule ' +
							'WHERE Facility = ''' + @vchFacility + ''' AND ProductionDate Between ' +  @vchFromDate + ' AND ' +  @vchToDate 
			IF Not( @vchGrinder is NULL )
				Set @vchADHocSchSQLStmt =  @vchADHocSchSQLStmt + ' AND Grinder = ''' +  @vchGrinder + ''''

			IF @vchGrindingStatus <> 'Ready'
				Set @vchSQLStmt = 'SELECT T3.Status, isnull(T3.Finalized,0) as Finalized, isnull(T3.TotalActualWgt,0) as ActualWgt,isnull(T4.ReworkWgt,0) as ReworkWgt, isnull(T4.SpillageWgt,0) as SpillageWgt, ' +
						'T1.GSCQTYI-isnull(T3.TotalActualWgt,0)-isnull(T4.ReworkWgt,0)-isnull(T4.SpillageWgt,0) + isnull(T3.TotalRejectedWgt,0) as RemainWgt, T1.*, ' + 
						'T5.Description	As LineDesc ' +
						'FROM (' + RTRIM(@vchGrindingSchdleSQLStmt) +
						' UNION ' +
						@vchADHocSchSQLStmt + ' ) AS T1 ' +
	--				 'left outer join tblGrindingLog T2 on T1.GSCACTI = T2.ScheduleID ' +
						'LEFT OUTER JOIN (' + @vchGLogSQLStmt + ') as T3 on T1.GSCFAC = T3.Facility AND T1.GSCACTI = T3.ScheduleID ' +
						'LEFT OUTER JOIN tblOtherWeights T4 ON T1.GSCFAC = T4.Facility AND T1.GSCACTI = T4.ScheduleID '	+
						'LEFT OUTER JOIN tblEquipment T5 ON T1.GSCFAC = T5.Facility AND T1.GSCPKLIN = T5.EquipmentID '	
			ELSE
				Set @vchSQLStmt = 'SELECT ''Ready'' as Status, 0 as Finalized, 0 as ActualWgt, ISNULL(T3.ReworkWgt,0) AS ReworkWgt, isnull(T3.SpillageWgt,0) AS SpillageWgt, ' + 
						'T1.GSCQTYI-isnull(T3.ReworkWgt,0)-isnull(T3.SpillageWgt,0) as RemainWgt, T1.*, ' + 
						'T5.Description	 As LineDesc ' +
						'FROM (' + RTRIM(@vchGrindingSchdleSQLStmt) +
						' UNION ' +
						@vchADHocSchSQLStmt + ' ) AS T1 ' +
						'LEFT OUTER JOIN tblGrindingLog T2 ON T1.GSCFAC = T2.Facility AND T1.GSCACTI = T2.ScheduleID ' +
						'LEFT OUTER JOIN tblOtherWeights T3 ON T1.GSCFAC = T3.Facility AND T1.GSCACTI = T3.ScheduleID ' +
						'LEFT OUTER JOIN tblEquipment T5 ON T1.GSCFAC = T5.Facility AND T1.GSCPKLIN = T5.EquipmentID ' +
						'WHERE T2.ScheduleID IS NULL '

			IF  @vchGrindingStatus <> 'All'
				IF @vchGrindingStatus <> 'Ready'
					SET @vchSQLStmt = @vchSQLStmt + 'WHERE T3.Status = ''' + @vchGrindingStatus + ''' '
			
			SET @vchSQLStmt = @vchSQLStmt + 'Order by GSCPDTE, GSCSDTE, GSCSTIM'
			
			PRINT 'vchSQLStmt = ' + @vchSQLStmt
				EXEC (@vchSQLStmt)
		END
		ELSE
			IF @vchAction = 'GrindingScheduleByID/FmBin'
			BEGIN
				Select GSCFAC,GSCGRNDR,GSCACTI,GSCBLEND,GSCGRIND,GSCQTYI,GSCSDTE,GSCSTIM,GSCEDTE,GSCETIM,GSCTBIN,GSCPKLIN,GSCPDTE,GSCFBIN 
				FROM tblGrindingSchedule
				WHERE GSCFAC = @vchFacility and GSCACTI = @vchScheduleID
				ORDER BY GSCFBIN 
			END
			 
	END TRY

	BEGIN CATCH
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

