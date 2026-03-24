
-- =============================================
-- Author:		Bong Lee
-- Create date: Dec 29, 2008
-- Description:	Copy Machine Efficiency data from staging area to production area
/*
	Mod		 Date		Author		Description
	P228	 7/19/2010	B. Lee		Add new column, RunOperators
	WO359	 10/24/2012	B. Lee		Achive data change to the history table
    WO#1297  08/19/2014 B. Lee		Enhance error handling.
	Bug11838 03/31/2016	B. Lee		Description: Override the machine rates based on the table, tblOverrideMachineEfficiencyRate	
	WO#4047	 10/20/2016	B. Lee		Description: Add item number on table tblOverrideMachineEfficiencyRate to check item then line.
	WO#5178	 03/28/2017	B. Lee		Description: Add Run Operator Multiplier to allow override Run Operators.
	WO#6391	 11/15/2017	B. Lee		Description: Active records in tblOverrideMachineEfficiencyRate are included only.
			 01/23/2020	B. Lee		Description: Rename PPsp_AchiveStdMachineEfficiencyRate to PPsp_ArchiveStdMachineEfficiencyRate
*/
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ImportStdMachineEfficiencyRate]
	@chrFacility as char(3),
	@vchImportData as varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- WO#4047	DECLARE @vchSQLStmt varchar(1000)
	DECLARE @vchSQLStmt varchar(1500);				-- WO#4047
-- WO#1297	DECLARE @ErrorMessage NVARCHAR(4000);
-- WO#1297	DECLARE @ErrorSeverity INT;
-- WO#1297	DECLARE @ErrorState INT;
	SET NOCOUNT ON;

	BEGIN TRANSACTION [Tran1]		-- WO#1297
	BEGIN TRY
	--SET @chrFacility = (SELECT value1 FROM tblControl WHERE ([Key] = 'Facility'))
	
		-- Achive data change to the history table								-- WO#359
		-- 01/23/2020	EXEC PPsp_AchiveStdMachineEfficiencyRate @chrFacility, @vchImportData	-- WO#359
		EXEC PPsp_ArchiveStdMachineEfficiencyRate @chrFacility, @vchImportData					-- 01/23/2020

		-- Update record if value(s) are changed
		SET @vchSQLStmt = 	
		'Update dbo.tblStdMachineEfficiencyRate ' + 
-- Bug11838	'SET MachineHours = T2.MachineHours,' +
		'SET MachineHoursOriginal = T2.MachineHours,' +									-- Bug11838
-- WO#4047		'MachineHours = T2.MachineHours * COALESCE(T3.RateMultiplier,1),' +				-- Bug11838
		'MachineHours = T2.MachineHours * COALESCE(T4.RateMultiplier, T3.RateMultiplier,1),' +				-- WO#4047
		'BasisCode = T2.BasisCode, ' +
		-- WO#4047	'RunOperators = T2.RunOperators, ' +
		'RunOperatorsOriginal = T2.RunOperators,' +																			-- WO#5178	
		'RunOperators = T2.RunOperators * COALESCE(T4.RunOperatorsMultiplier, T3.RunOperatorsMultiplier,1),' +				-- WO#5178
		'StdWorkCenterEfficiency = T2.StdWorkCenterEfficiency ' +
		'FROM tblStdMachineEfficiencyRate AS T1 ' +
		'INNER JOIN ' + @vchImportData + '.dbo.tblStdMachineEfficiencyRate AS T2 ' +
		'ON T1.Facility = T2.Facility ' +
		'AND T1.ItemNumber = T2.ItemNumber ' +
		'AND T1.WorkCenter = T2.WorkCenter ' +
		'AND T1.MachineID = T2.MachineID ' +
-- Bug11838 ADD Start
		'LEFT OUTER JOIN tblOverrideMachineEfficiencyRate AS T3 ' +
		'ON T1.Facility = T3.Facility ' +
		'AND T1.MachineID = T3.MachineID ' +																-- WO#4047
		'AND '''' = T3.ItemNumber ' +	
		'AND T3.Active = 1 ' +																				-- WO#6391										
-- Bug11838 ADD Stop
-- WO#4047 ADD Start
		'LEFT OUTER JOIN tblOverrideMachineEfficiencyRate AS T4 ' +
		'ON T1.Facility = T4.Facility ' +
		'AND T1.MachineID = T4.MachineID ' +
		'AND T1.ItemNumber = T4.ItemNumber ' +	
		'AND T4.Active = 1 ' +																				-- WO#6391											
-- WO#4047 ADD Stop
		'WHERE T1.Facility = ''' + @chrFacility + ''' AND ' +
-- Bug11838	  '(T1.MachineHours <> T2.MachineHours ' +
		'(T1.MachineHoursOriginal <> T2.MachineHours ' +								-- Bug11838
		'OR T1.MachineHours <> T2.MachineHours * COALESCE(T4.RateMultiplier, T3.RateMultiplier,1) ' +		-- WO#4047
		'OR T1.RunOperators <> T2.RunOperators * COALESCE(T4.RunOperatorsMultiplier, T3.RunOperatorsMultiplier,1) ' +		-- WO#5178
		'OR T1.BasisCode <> T2.BasisCode ' +
		'OR T1.StdWorkCenterEfficiency <> T2.StdWorkCenterEfficiency ' +
		'OR T1.RunOperators <> T2.RunOperators) '
	Print @vchSQLStmt
		execute (@vchSQLStmt)

		--Add New Records
		SET @vchSQLStmt = 
		'INSERT INTO tblStdMachineEfficiencyRate ' +
-- Bug11838 ADD Start
		-- WO#5178	'(Facility, ItemNumber, WorkCenter, MachineID, MachineHoursOriginal, BasisCode, StdWorkCenterEfficiency, RunOperators, MachineHours) ' +
		-- WO#5178 ADD Start
		'(Facility, ItemNumber, WorkCenter, MachineID, MachineHoursOriginal, BasisCode, StdWorkCenterEfficiency, ' +
		'RunOperatorsOriginal, MachineHours, RunOperators) ' +	
		-- WO#5178 ADD Stop
-- WO#4047		'SELECT T1.*, T1.MachineHours * COALESCE(T3.RateMultiplier,1) ' +
		'SELECT T1.*, T1.MachineHours * COALESCE(T4.RateMultiplier, T3.RateMultiplier,1) ' +				-- WO#4047
		',T1.RunOperators * COALESCE(T4.RunOperatorsMultiplier, T3.RunOperatorsMultiplier, 1) ' +			-- WO#5178
		'FROM ' + @vchImportData + '.dbo.tblStdMachineEfficiencyRate As T1  ' +
-- Bug11838 ADD Stop
-- Bug11838	'SELECT T1.* FROM ' + @vchImportData + '.dbo.tblStdMachineEfficiencyRate As T1  ' +
		'LEFT OUTER JOIN tblStdMachineEfficiencyRate AS T2 ' +
		'ON T1.Facility = T2.Facility ' +
		'AND T1.ItemNumber = T2.ItemNumber ' +
		'AND T1.WorkCenter = T2.WorkCenter ' +
		'AND T1.MachineID = T2.MachineID ' +
-- Bug11838 ADD Start
		'LEFT OUTER JOIN tblOverrideMachineEfficiencyRate AS T3 ' +
		'ON T1.Facility = T3.Facility ' +
		'AND T1.MachineID = T3.MachineID ' +
		'AND '''' = T3.ItemNumber ' +																		-- WO#4047
-- Bug11838 ADD Stop
-- WO#4047 ADD Start
		'LEFT OUTER JOIN tblOverrideMachineEfficiencyRate AS T4 ' +
		'ON T1.Facility = T4.Facility ' +
		'AND T1.MachineID = T4.MachineID ' +
		'AND T1.ItemNumber = T4.ItemNumber ' +											
-- WO#4047 ADD Stop												-- WO#4047								
		'WHERE T1.Facility = ''' + @chrFacility + ''' AND T2.ItemNumber IS NULL' 
Print @vchSQLStmt
		execute (@vchSQLStmt)

		--Delete Records if record is not found in ImportData
		SET @vchSQLStmt = 
		'DELETE tblStdMachineEfficiencyRate ' +
		'FROM tblStdMachineEfficiencyRate AS T1 ' +
		'LEFT OUTER JOIN ' + @vchImportData + '.dbo.tblStdMachineEfficiencyRate AS T2 ' +
		'ON T1.Facility = T2.Facility ' +
		'AND T1.ItemNumber = T2.ItemNumber ' +
		'AND T1.WorkCenter = T2.WorkCenter ' +
		'AND T1.MachineID = T2.MachineID ' +
		'WHERE T1.Facility = ''' + @chrFacility + ''' AND T2.itemNumber IS NULL' 
Print @vchSQLStmt
		execute (@vchSQLStmt)

		COMMIT TRANSACTION [Tran1]	-- WO#1297	

	END TRY
	BEGIN CATCH
	/* WO#1297 DEL Start
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
	WO#1297 DEL Stop */
	-- WO#1297 ADD Start
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION [Tran1]

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
	-- WO#1297 ADD Stop
	END CATCH
END

GO

