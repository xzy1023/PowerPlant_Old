

-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 25, 2007
-- Description:	Ground Cofee Grinding Activity Selection
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_GrindingLog_Sel]
	-- Add the parameters for the stored procedure here
	@vchAction as varchar(30),
	@vchStatus as varchar(10) = NULL,
	@chrFacility char(3),
	@vchScheduleID varchar(50) = NULL,
	@vchGrinder varchar(10) = '',
	@intGrindJobID int = 0 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	DECLARE @vchSQLStmt  varchar(1000);
	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);

	BEGIN TRY
		IF @vchAction = 'ByJobID_NoOtherWgts'
			SELECT *, 0 as ReworkWgt, 0 as SpillageWgt 
				FROM tblGrindingLog 
				WHERE GrindJobID = @intGrindJobID AND Active = 1
		ELSE
		BEGIN
			SET @vchSQLStmt = 'SELECT  T1.*, ISNULL(T2.ReworkWgt,0) as ReworkWgt, ISNULL(T2.SpillageWgt,0) as SpillageWgt ' +
					'FROM tblGrindingLog T1 ' +
					'LEFT OUTER JOIN tblOtherWeights T2 ' +
					'ON T1.Facility = T2.Facility AND T1.ScheduleID = T2.ScheduleID ' +
					'WHERE T1.Facility = ''' + @chrFacility + ''' AND (T1.Active = 1) '
			
			IF @vchAction = 'ByScheduleID'
				SET @vchSQLStmt = @vchSQLStmt + 'AND (T1.ScheduleID = ''' + @vchScheduleID + ''') ' +
						'ORDER BY T1.StartTime '
			Else 
				IF @vchAction = 'ByStatus'
				BEGIN	
					IF @vchStatus = 'ALL'
					BEGIN
						If @vchGrinder <> ''
							SET @vchSQLStmt = @vchSQLStmt + ' AND T1.grinder = ''' + @vchGrinder + ''' ' +
								'ORDER BY T1.StartTime '
					END
					ELSE
					BEGIN
						If @vchGrinder <> ''
							SET @vchSQLStmt = @vchSQLStmt + 'AND ISNULL(T1.Status,''Ready'') = ''' + @vchStatus + ''' AND T1.grinder = ''' + @vchGrinder + ''' '
						ELSE
							SET @vchSQLStmt = @vchSQLStmt + 'AND ISNULL(T1.Status,''Ready'') = ''' + @vchStatus + ''' '
						SET @vchSQLStmt = @vchSQLStmt + 'ORDER BY T1.StartTime '
					END
				END
				ELSE
					IF @vchAction = 'FIFOByStatus'
					BEGIN
						SET @vchSQLStmt = @vchSQLStmt + ' AND T1.GrindJobID IN ' +	
							'(SELECT MIN(GrindJobID) as GrindJobID From tblGrindingLog ' +
							'WHERE Facility = ''' + @chrFacility + ''' AND ISNULL(Status,''Ready'') = ''' + @vchStatus + ''' AND Active = 1'
						If @vchGrinder <> ''
							SET @vchSQLStmt = @vchSQLStmt + ' and grinder = ''' + @vchGrinder + ''')'
						ELSE
							SET @vchSQLStmt = @vchSQLStmt + ')'
					END
					ELSE
						IF @vchAction = 'NextJobInSameSchedule'
							SET @vchSQLStmt = @vchSQLStmt + 'AND (T1.ScheduleID = ''' + @vchScheduleID + ''') ' + ' AND T1.GrindJobID = ' +
								'(SELECT MIN(GrindJobID) as GrindJobID From tblGrindingLog ' +
								'WHERE Facility = ''' + @chrFacility + ''' AND ScheduleID = ''' + @vchScheduleID + ''' AND Active = 1 AND Status = ''' + @vchStatus + ''')'
						ELSE
						IF @vchAction = 'ByJobID'
							SET @vchSQLStmt = @vchSQLStmt + ' AND T1.GrindJobID = ' + CAST(@intGrindJobID as varchar)
						ELSE
							IF @vchAction = 'ByScheduleIDWithTestData' 
								SET @vchSQLStmt = 'SELECT  T1.*, ISNULL(T2.ReworkWgt,0) as ReworkWgt, ISNULL(T2.SpillageWgt,0) as SpillageWgt, T3.AvgColour ' +
									'FROM tblGrindingLog T1 ' +
									'LEFT OUTER JOIN tblOtherWeights T2 ' +
									'ON T1.Facility = T2.Facility AND T1.ScheduleID = T2.ScheduleID ' +
									'LEFT OUTER JOIN (SELECT tGD.GrindJobID, CAST(AVG(tGD.Colour) AS Decimal(5,2)) AS AvgColour FROM tblGrindingLog tGL ' +
											'INNER JOIN tblGrindData tGD on tGL.GrindJobID = tGD.GrindJobID ' +
											'WHERE tGL.Facility = ''' + @chrFacility + ''' AND TGL.ScheduleID = ''' + @vchScheduleID + ''' ' +
											'GROUP BY tGD.GrindJobID) AS T3 ON T1.GrindJobID = T3.GrindJobID ' +
									'WHERE T1.Facility = ''' + @chrFacility + ''' AND (T1.Active = 1) AND (T1.ScheduleID = ''' + @vchScheduleID + ''') ' +
									'ORDER BY T1.StartTime '
		
			PRINT @vchSQLStmt
			EXEC (@vchSQLStmt)
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

