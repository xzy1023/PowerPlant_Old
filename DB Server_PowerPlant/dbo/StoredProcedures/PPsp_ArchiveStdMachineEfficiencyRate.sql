
-- =============================================
-- Author:		Bong Lee
-- Create date: Oct 24, 2012
-- Description:	Archive Machine Efficiency data when the data is changed
/*
   Mod		Date		Author		Description
   WO#1297  08/19/2014	Bong Lee	Enhance error handling.
   WO#27470 01/30/2020	Bong Lee	To record the changes on machine rate and run operator correctly
									for machines having Override Machine Rate feature
*/
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ArchiveStdMachineEfficiencyRate]
	@chrFacility as char(3),
	@vchImportData as varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- WO#27470	DECLARE @vchSQLStmt varchar(1000)
	DECLARE @vchSQLStmt varchar(MAX)	-- WO#27470
	-- WO#1297	DECLARE @ErrorMessage NVARCHAR(4000);
	-- WO#1297	DECLARE @ErrorSeverity INT;
	-- WO#1297	DECLARE @ErrorState INT;
	SET NOCOUNT ON;
	BEGIN TRY

		-- Add a record if value(s) are changed or find a new rate
		SET @vchSQLStmt = 	
		'INSERT INTO tblStdMachineEfficiencyRateHst ' +
		-- WO#27470	'SELECT T1.*, GETDATE() as EffictiveTime ' +
		-- WO#27470 ADD Start
			'(Facility ' +
			  ',ItemNumber ' +
			  ',WorkCenter ' +
			  ',MachineID ' +
			  ',MachineHours ' +
			  ',BasisCode ' +
			  ',StdWorkCenterEfficiency ' +
			  ',RunOperators ' +
			  ',EffectiveTime) ' +
			'SELECT T1.Facility ' +
			  ',T1.ItemNumber ' +
			  ',T1.WorkCenter ' +
			  ',T1.MachineID ' +
			  ',ROUND(T1.MachineHours * COALESCE(t4.RateMultiplier, t3.RateMultiplier, 1),3) ' +
			  ',T1.BasisCode ' +
			  ',T1.StdWorkCenterEfficiency ' +
			  ',T1.RunOperators * COALESCE(t4.RunOperatorsMultiplier, t3.RunOperatorsMultiplier, 1)' +
			  ',GETDATE() as EffictiveTime ' +
		-- WO#27470 ADD Stop
 			'FROM ' + @vchImportData + '.dbo.tblStdMachineEfficiencyRate AS T1 ' +
		-- WO#27470	'INNER JOIN tblStdMachineEfficiencyRate AS T2 ' +
		-- WO#27470 ADD Start
			'INNER JOIN ' +
			'(SELECT * FROM ( ' +
				'SELECT *, ROW_NUMBER() OVER (PARTITION BY ItemNumber, WorkCenter, MachineID order by EffectiveTime desc) as RowNo ' +
				'FROM tblStdMachineEfficiencyRateHst ) T9 ' +
				'WHERE RowNo = 1) AS T2 ' +
		-- WO#27470 ADD Stop
				'ON T1.Facility = T2.Facility ' +
				'AND T1.ItemNumber = T2.ItemNumber ' +
				'AND T1.WorkCenter = T2.WorkCenter ' +
				'AND T1.MachineID = T2.MachineID ' +
	-- WO#27470 ADD Start
			'LEFT OUTER JOIN tblOverrideMachineEfficiencyRate AS T3 ' +
				'ON T1.Facility = T3.Facility ' +
				'AND T1.MachineID = T3.MachineID ' +	
				'AND T3.ItemNumber = '''' ' +																				
				'AND T3.Active = 1 ' +																													
			'LEFT OUTER JOIN tblOverrideMachineEfficiencyRate AS T4 ' +
				'ON T1.Facility = T4.Facility ' +
				'AND T1.MachineID = T4.MachineID ' +
				'AND T1.ItemNumber = T4.ItemNumber ' +	
				'AND T4.Active = 1 ' +																														
	-- WO#27470 ADD Stop
			-- If the current data in the standard machine rate is different from the imported data, save the imported data
			'WHERE T1.Facility = ''' + @chrFacility + ''' AND ' +
				 --WO#27470	'(T1.MachineHours <> T2.MachineHours ' +
				'(ROUND(T1.MachineHours * COALESCE(t4.RateMultiplier, t3.RateMultiplier, 1),3) <> T2.MachineHours ' +				-- WO#27470
				'OR T1.BasisCode <> T2.BasisCode ' +
				'OR T1.StdWorkCenterEfficiency <> T2.StdWorkCenterEfficiency ' +
				--WO#27470	'OR T1.RunOperators <> T2.RunOperators) ' +
				'OR T1.RunOperators * COALESCE(t4.RunOperatorsMultiplier, t3.RunOperatorsMultiplier, 1) <> T2.RunOperators) ' +		-- WO#27470
		'UNION ' + 
			-- If the imported rate is not found in the standard machine rate history, save the imported data
	-- WO#27470	'SELECT T1.*, GETDATE() as EffictiveTime ' + 
			'SELECT T1.Facility ' +
			  ',T1.ItemNumber ' +
			  ',T1.WorkCenter ' +
			  ',T1.MachineID ' +
			  ',ROUND(T1.MachineHours * COALESCE(t4.RateMultiplier, t3.RateMultiplier, 1),3) ' +
			  ',T1.BasisCode ' +
			  ',T1.StdWorkCenterEfficiency ' +
			  ',T1.RunOperators * COALESCE(t4.RunOperatorsMultiplier, t3.RunOperatorsMultiplier, 1)' +
			  ',GETDATE() as EffictiveTime ' +
	-- WO#27470 ADD Stop
			'FROM ' + @vchImportData + '.dbo.tblStdMachineEfficiencyRate As T1 ' +
	-- WO#27470		'LEFT OUTER JOIN tblStdMachineEfficiencyRate AS T2 ' +
			'LEFT OUTER JOIN tblStdMachineEfficiencyRateHst AS T2 ' +				-- WO#27470
				'ON T1.Facility = T2.Facility ' +
				'AND T1.ItemNumber = T2.ItemNumber ' +
				'AND T1.WorkCenter = T2.WorkCenter ' +
				'AND T1.MachineID = T2.MachineID ' +
	-- WO#27470 ADD Start
			'LEFT OUTER JOIN tblOverrideMachineEfficiencyRate AS T3 ' +
				'ON T1.Facility = T3.Facility ' +
				'AND T1.MachineID = T3.MachineID ' +		
				'AND T3.ItemNumber = '''' ' +																			
				'AND T3.Active = 1 ' +																													
			'LEFT OUTER JOIN tblOverrideMachineEfficiencyRate AS T4 ' +
				'ON T1.Facility = T4.Facility ' +
				'AND T1.MachineID = T4.MachineID ' +
				'AND T1.ItemNumber = T4.ItemNumber ' +	
				'AND T4.Active = 1 ' +																														
	-- WO#27470 ADD Stop
			'WHERE T1.Facility = ''' + @chrFacility + ''' AND T2.ItemNumber IS NULL' 
Print @vchSQLStmt
		Execute (@vchSQLStmt)

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

