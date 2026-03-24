

-- =======================================================================================================================================
-- WO#25925		May 29, 2019		Bong Lee
-- Description: (Rewrite the stored procedure) The data source is changed from WorkCentral database directly to customized local DB 
--				that was constructed by periodically download data from the Workforce Dimension on the cloud.
-- ID#5539		May 11, 2021		Bong Lee
-- Description: Missing the records for shift that uses shift end date for shift production date. Find out does any shifts have 
--				UseSEDateForShiftPD = 1. Fix the ambiguous of the column, shiftNo. Select time card records with cost center is numeric.
-- =======================================================================================================================================

CREATE PROCEDURE [dbo].[PPsp_KronosPaidHours_Sel]
	@vchAction as varchar(50) = NULL,
	@dteFromTime as datetime,
	@dteToTime as datetime,
	@vchFacility as varchar(3),
	@vchCountry as varchar(10) = NULL
	
AS
BEGIN
	DECLARE	@nvchKRONOSServerName as nvarchar(50)
			,@nvchSQLStmt as nvarchar(MAX)
			,@nvchWhereClause as nvarchar(MAX)
			,@nvchPayCodeID nvarchar(100)
			,@nvchPayCodeIDNew nvarchar(100)
			,@nvchADayPriorFromTime nvarchar(10)
			,@nvchADayPriorToTime nvarchar(10)
			,@nchrAllShiftsInOneDay as char(1)
			,@nvchFromTime nvarchar(10)
			,@nvchToTime nvarchar(10)
			,@bitUseSEDateForShiftPD bit
			,@intMethod tinyint	
			,@nvchShiftless as char(1)
			,@chrOverNightShihtNo char(1)
			,@nvchImplementionDate datetime
			,@nvchWFCLastDate nvarchar(10)

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRY

		SELECT @nvchFromTime = CONVERT(nvarchar(10),@dteFromTime,111)
			,@nvchToTime = CONVERT(nvarchar(10),@dteToTime,111)
			,@nvchADayPriorFromTime = CONVERT(nvarchar(10),DateAdd(day,-1, @dteFromTime) ,111)	
			,@nvchADayPriorToTime = CONVERT(nvarchar(10),DateAdd(day,-1, @dteToTime) ,111)	

		SELECT @nvchKRONOSServerName = Value1, @vchCountry = ISNULL(@vchCountry, Value2) 
			FROM tblControl WHERE [KEY] = 'KRONOSSETTINGS' AND [SUBKEY]= 'KRONOS'

		SELECT @nvchWFCLastDate = Value1
			FROM tblControl WHERE [KEY] = 'WFCLastDate' AND [SUBKEY]= 'KRONOS'

		SELECT @nvchShiftless = Value1, @nchrAllShiftsInOneDay = Value2  
			FROM tblControl WHERE [KEY] = 'Shiftless' AND [SUBKEY]= 'LaborEfficiency'

		SELECT Top 1 @bitUseSEDateForShiftPD = [UseSEDateForShiftPD] , @intMethod = [Method]
			FROM tblShift	WHERE Facility = @vchFacility AND [WorkGroup] = 'P'
			ORDER BY UseSEDateForShiftPD DESC										-- ID#5539

		SELECT @nvchPayCodeID = Value1 
			FROM tblControl WHERE [KEY] = 'PayCodes' AND [SUBKEY]= 'KRONOS'

		SELECT @nvchPayCodeIDNew = Value1 
			FROM tblControl WHERE [KEY] = 'PayCodesNew' AND [SUBKEY]= 'KRONOS'

		IF @nvchShiftless = 'Y' OR @bitUseSEDateForShiftPD = 0				
		BEGIN
			SET @nvchWhereClause =  +
				'WHERE tFTC.Facility = ''''' + @vchFacility + ''''' AND ApplyDate BETWEEN ''''' + @nvchFromTime + ''''' AND ''''' + @nvchToTime + ''''' '  
		END
		ELSE
		BEGIN
			-- Find the shift no. that crosses mid-night.
			SELECT Top 1 @chrOverNightShihtNo = Cast([Shift] as char(1)) FROM tblShift 
					WHERE Facility = @vchFacility AND [WorkGroup] = 'P' AND FromTime > ToTime  
			
			-- construct the where clause of SQL statement
			SET @nvchWhereClause = +		
				'WHERE tFTC.Facility = ''''' + @vchFacility + ''''' AND ((ApplyDate BETWEEN ''''' + @nvchFromTime + ''''' AND ''''' + @nvchToTime + ''''' '  +
				-- ID#5539	'AND (Shift <> ''''' + @chrOverNightShihtNo + ''''' )) ' +	
				'AND (tSX.ShiftNo <> ''''' + @chrOverNightShihtNo + ''''' )) ' +					-- ID#5539 														
				'OR (ApplyDate BETWEEN ''''' + @nvchADayPriorFromTime + ''''' AND ''''' + @nvchADayPriorToTime + ''''' ' +	
				-- ID#5539	'AND Shift = ''''' + @chrOverNightShihtNo + ''''' )) '	
				'AND tSX.ShiftNo = ''''' + @chrOverNightShihtNo + ''''' )) '						-- ID#5539
		END	

		SELECT @nvchSQLStmt = N'SELECT tKr.FullName, tKr.PersonNum, tWC.WorkCenter, tKr.Shift, tKr.PayCodeName, ' +	
			'CASE WHEN tS.FromTime > tS.ToTime AND UseSEDateForShiftPD = 1 AND ''' + @nvchShiftless + ' ''= ''N'' ' +
			'THEN DATEADD(day,1, tKr.ApplyDate) ELSE tKr.ApplyDate END AS ApplyDate ' +
			', tKr.LaborHoursWorked' +
			',tS.ShiftSequence FROM OPENQUERY(' + @nvchKRONOSServerName + ',' 

		-- If the selected from date is prior or equal to the last date of Work Force Central, query the historical data.
		IF convert(datetime,@nvchFromTime,111)<= convert(datetime,@nvchWFCLastDate,111)
		BEGIN
			SELECT @nvchSQLStmt = @nvchSQLStmt +
			'''SELECT FullName, PersonNum, WorkCenter, Shift, PAYCODENAME, ApplyDate, LaborHoursWorked FROM tblPaidHoursHistory tFTC ' +
			-- ID#5539	@nvchWhereClause + 'AND PAYCODE IN (' + @nvchPayCodeID + ') ' +
			Replace(@nvchWhereClause,'tSX.ShiftNo', 'Shift') + 'AND PAYCODE IN (' + @nvchPayCodeID + ') ' +		-- ID#5539
			'UNION ALL ' 
		END
		ELSE
		BEGIN
			SELECT @nvchSQLStmt = @nvchSQLStmt + ''''
		END

		-- Select data from the appending download data from Work Force Dimension.
		SELECT @nvchSQLStmt = @nvchSQLStmt +
		'SELECT ISNULL(LastName,'''''''') + '''', '''' + ISNULL(FirstName,'''''''') + ISNULL(MiddleName,'''''''') as FullName, ' + 
		'tFTC.PersonNum, tFTC.CostCenter as WorkCenter, ' +
		'CASE WHEN tFTC.Shift IS NULL THEN 0 ELSE tSX.ShiftNo END as Shift, ' +
		'tDPC.NAME as PAYCODENAME, ApplyDate, LaborHoursWorked ' + 
		'FROM FactTimeCard tFTC ' +
		'LEFT OUTER JOIN tblShiftXRef tSX on tFTC.shift = tSX.ShiftDesc ' +
		'LEFT OUTER JOIN DimEmployee tDE on tFTC.PersonNum = tDE.PersonNum AND tFTC.Facility = tDE.Facility ' +
		'LEFT OUTER JOIN DimPayCode tDPC on tFTC.PayCode = tDPC.PayCodeID ' +
		@nvchWhereClause + 'AND PAYCODE IN (' + @nvchPayCodeIDNew + ') AND tFTC.TransactionSource = ''''Totals'''' ' +
		'AND ISNUMERIC(tFTC.CostCenter) = 1 ' +									-- ID#5539
		''') as tKr ' +
		'LEFT OUTER JOIN (SELECT * FROM tblShift WHERE Facility = ''' + @vchFacility + ''' AND WorkGroup = ''P'') tS ' + 
		'ON tKr.Shift = tS.Shift ' +
		'Cross JOIN (SELECT DISTINCT tEQ.WorkCenter,tKCCX.KronosCostCentre FROM tblEquipment tEQ ' +
			'LEFT OUTER JOIN tblComputerConfig tCC ' +
			'ON tEQ.Facility = tCC.Facility and tEQ.EquipmentID = tCC.packagingline ' +
			'LEFT OUTER JOIN tblKronosCostCenterXref tKCCX ' +											
			'ON tEQ.Facility = tKCCX.Facility AND tEQ.WorkCenter = tKCCX.PPWorkCenter  ' +					
			'WHERE tEQ.Facility = ''' + @vchFacility + ''' AND tEQ.[Type] = ''P'' ' + 		
			'AND tKCCX.Active = 1 ' +	
			'AND tCC.PalletStation <> 1 AND tEQ.WorkCenter is not NULL) as tWC ' +
		'WHERE tKr.WorkCenter = tWC.WorkCenter OR tKr.WorkCenter =tWC.KronosCostCentre ' 	

		IF @vchAction = 'DtlByWC_Shift_Date'
			Set @nvchSQLStmt = @nvchSQLStmt + 'Order by tKR.WorkCenter, tS.ShiftSequence, APPLYDATE, tKR.FULLNAME'

		print @nvchSQLStmt
		EXEC sp_executesql @nvchSQLStmt

	END TRY
	BEGIN CATCH
	
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure nvarchar(200);
		DECLARE @ErrorLine int;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorMessage NVARCHAR(4000);

		SELECT 
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ', Message: ' + CHAR(13) + ERROR_MESSAGE();

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)
	END CATCH
END

GO

