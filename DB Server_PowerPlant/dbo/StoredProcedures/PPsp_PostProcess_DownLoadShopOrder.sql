
-- =============================================
-- WO#1297      Jul. 9, 2014   Bong Lee
-- Description:	Post process for downloading MS Dynamics AX shop order related data to Power Plant
-- FX160726		Jul. 26, 2016	Bong Lee
-- Description:	Add logic for table tblProbatEquipment
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PostProcess_DownLoadShopOrder]
	@chrFacility as char(3)
AS
BEGIN

	DECLARE @vchImportData as varchar(50);
	DECLARE @vchSQLStmt as varchar(1000);
	--DECLARE @vchLogFileName nvarchar(200);
	DECLARE @vchDownLoadLogFileName nvarchar(200);
	DECLARE	@vchText varchar(255);
	DECLARE	@vchTableName varchar(100);
	DECLARE @vchCurrentDBName nvarchar(100);
	DECLARE @tblTemp as Table
	(
	 intRow int
	)

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		
		SELECT @vchImportData = Value2 
			FROM tblControl
			WHERE [Key] = 'StagingDBName' and SubKey = 'General';

		Select --@vchLogFileName = Value1  + 'DownLoadLog_' + convert(varchar(8),getdate(),112) + '.txt', 
				@vchDownLoadLogFileName =  Value1  + Value2 + convert(varchar(8),getdate(),112) + '.txt' 
			FROM [tblControl] 
			WHERE [Key] = 'ERPDataXChgLogFolder' and [SubKey] = 'Interface';

		SET  @vchCurrentDBName = DB_NAME();
		SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + ISNULL(OBJECT_NAME(@@PROCID),'') + ' was started at ' + convert(varchar(21), getdate(), 120) + '. Should '
		-- SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + OBJECT_NAME(@@PROCID) + ' - Started.'
		-- EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;
		-- Update imported AX shop order related data from staging DB to Local DB in Power plant server.

		-- refresh the Shop Order and Shop Order Weight Spec.(if any changes) table(s).
		SET @vchTableName = 'tblShopOrder';
		DELETE From @tblTemp
		SET @vchSQLStmt = 'SELECT top 1 1 FROM ' + @vchImportData + '.dbo.'+ @vchTableName + ';';
		Insert into @tblTemp EXECUTE (@vchSQLStmt);

		IF EXISTS (SELECT * FROM @tblTemp)
		BEGIN
			SET @vchText = @vchText + 'Import ' + @vchTableName + ', ';
			-- EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;
			EXECUTE PPsp_ImportSO @chrFacility, @vchImportData;
		END

		-- refresh the Bill of Material table
		SET @vchTableName = 'tblBillOfMaterials';
		DELETE From @tblTemp
		SET @vchSQLStmt = 'SELECT top 1 1 FROM ' + @vchImportData + '.dbo.'+ @vchTableName + ';';
		Insert into @tblTemp EXECUTE (@vchSQLStmt);

		IF EXISTS (SELECT * FROM @tblTemp)
		BEGIN
			SET @vchText = @vchText + 'Import ' + @vchTableName + ', ';
			-- EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;
			EXECUTE PPsp_ImportBOM @chrFacility, @vchImportData;
		END

		-- refresh the Import Notes table
		SET @vchTableName = 'tblItemNotes';
		DELETE From @tblTemp
		SET @vchSQLStmt = 'SELECT top 1 1 FROM ' + @vchImportData + '.dbo.'+ @vchTableName + ';';
		Insert into @tblTemp EXECUTE (@vchSQLStmt);

		IF EXISTS (SELECT * FROM @tblTemp)
		BEGIN
			SET @vchText = @vchText + 'Import ' + @vchTableName + ', ';
			-- EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;
			EXECUTE PPsp_ImportItemNotes @vchImportData;
		END
-- FX160726 ADD Start
		-- refresh the Import Probat Equipment X-Ref. table
		SET @vchTableName = 'tblProbatEquipment';
		DELETE From @tblTemp
		SET @vchSQLStmt = 'SELECT top 1 1 FROM ' + @vchImportData + '.dbo.'+ @vchTableName + ';';
		Insert into @tblTemp EXECUTE (@vchSQLStmt);

		IF EXISTS (SELECT * FROM @tblTemp)
		BEGIN
			SET @vchText = @vchText + 'Import ' + @vchTableName + ', ';
			-- EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;
			EXECUTE PPsp_ImportProbatEquipment @chrFacility, @vchImportData;
		END
-- FX160726 ADD Stop

		-- refresh the Standard Machine Efficiency Rate table. This table will not be pushed to IPC.
		SET @vchTableName = 'tblStdMachineEfficiencyRate';
		DELETE From @tblTemp
		SET @vchSQLStmt = 'SELECT top 1 1 FROM ' + @vchImportData + '.dbo.'+ @vchTableName + ';';
		Insert into @tblTemp EXECUTE (@vchSQLStmt);

		IF EXISTS (SELECT * FROM @tblTemp)
		BEGIN
			SET @vchText = @vchText + 'Import ' + @vchTableName + ', ';
			-- EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;
			EXECUTE PPsp_ImportStdMachineEfficiencyRate @chrFacility, @vchImportData;
		END
		-- Flag the Down Load Table List to idicate the table is ready to populate to IPCs
		UPDATE tblDownLoadTableList SET Active = 1 
			-- FX160726 ADD Start
			WHERE facility = @chrFacility AND TableName in ('tblBillOfMaterials', 'tblItemNotes'
			,'tblProbatEquipment'
			,'tblShopOrder');								
			-- FX160726 ADD Stop
			-- FX160726 WHERE facility = @chrFacility AND TableName in ('tblBillOfMaterials', 'tblItemNotes', 'tblShopOrder');

		-- Populate tables to IPCs based on the Down Load Table List
		-- EXECUTE PPsp_ExportDataToLocalDB @chrFacility

		--EXECUTE PPsp_ExportDataToIPCDB @chrFacility, @vchLogFileName, @vchCurrentDBName ;
		---EXECUTE PPsp_ExportDataToIPCDB @chrFacility, @vchDownLoadLogFileName, @vchCurrentDBName ; -- this was commented on jul 2 2025

		-- Clear imported tables after export data to IPCs
		/*
		SET @vchSQLStmt = 'Truncate table ' + @vchImportData + '.[dbo].[tblBillOfMaterials];' +
						  'Truncate table ' + @vchImportData + '.[dbo].[tblItemNotes];' +	
						  'Truncate table ' + @vchImportData + '.[dbo].[tblShopOrder];' +
						  'Truncate table ' + @vchImportData + '.[dbo].[tblStdMachineEfficiencyRate];' 
		EXECUTE  (@vchSQLStmt);
		*/
		EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;

		SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + OBJECT_NAME(@@PROCID) + ' - Completed.';
		EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;

	END TRY
	BEGIN CATCH
		
		BEGIN TRY
			EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;
			SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + OBJECT_NAME(@@PROCID) + ' - Failed.';
			EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;
		END TRY
		BEGIN CATCH
		END CATCH

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
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ' Message: ' + CHAR(13) + ERROR_MESSAGE();

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)
	END CATCH
END

GO

