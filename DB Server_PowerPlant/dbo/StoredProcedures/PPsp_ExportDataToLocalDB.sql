




-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 04, 2006
-- Description:	Export Server Tables To Local DataBase
-- WO#21        Apr. 21, 2010   Bong Lee
-- Description: export a new column IPCSharedGroup to dbo.tblEquipment
-- WO#359       Feb. 22, 2012   Bong Lee
-- Description: Do not send error message if it is a spare IPC
--			    Write current time to tblDownLoadTableList
-- FIX20160401	Apr. 01, 2016	Bong Lee
--				Fix data conversion error
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ExportDataToLocalDB]
	@chrFacility as char(3)
AS
BEGIN
	DECLARE @vchSQLServerName as varchar(50); 
	DECLARE @vchIPCName as varchar(50);
	DECLARE @vchTableName as varchar(50); 
	DECLARE @vchRemoteTableName as varchar(100); 
	DECLARE @vchSQLStmt as varchar(1000);
	DECLARE @vchIPCImportData as varchar(50);
	DECLARE @chrPackagingLine as char(10);
	DECLARE @bitAtLeast1TableOnList as bit;
	DECLARE @bitHasFacilityColumn as bit;

	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		
		-- Local IPCs only have production environment
		Set @vchIPCImportData = 'ImportData'
		Set @bitAtLeast1TableOnList = 0 

		-- Declare scroll cursor for tblDownLoadTableList
		DECLARE DownLoadTableList_cursor CURSOR SCROLL STATIC READ_ONLY FOR
			SELECT TableName FROM dbo.tblDownLoadTableList
			WHERE Active = '1' and facility = @chrFacility
			ORDER BY TableName
		OPEN DownLoadTableList_cursor
		
		-- Reset the ready-to-download flags to 0 (i.e. not ready) for each active computer in the Coomputre Configuration table 
		Update tblComputerConfig SET ReadyForDownLoad = 0 WHERE facility = @chrFacility AND RecordStatus = 1

		-- Declare cursor for tblComputerConfig
		DECLARE LinkServer_cursor CURSOR FOR
			SELECT ComputerName,PackagingLine FROM dbo.tblComputerConfig
			WHERE RecordStatus = 1 and (facility = @chrFacility OR PackagingLine = 'Spare')
			ORDER BY ComputerName
		OPEN LinkServer_cursor

		-- Read industrial PC Name 
		FETCH NEXT FROM LinkServer_cursor INTO @vchIPCName,@chrPackagingLine

		WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRY
			-- Construct Remote SQL Server Name from Computer Configuration Table
			SET @vchSQLServerName = '['+ @vchIPCName + '\SQLEXPRESS]'
			
			-- Verify the connectivity of the IPC by setting Ready_For_DownLoad status to No in the remote SQL server
				SET @vchSQLStmt ='Update ' + @vchSQLServerName  + '.' + @vchIPCImportData + '.dbo.tblComputerConfig SET ReadyForDownLoad = 0 WHERE ComputerName = ''' + @vchIPCName + ''''
				EXECUTE  (@vchSQLStmt)

				-- Read the frist active table name from the local download list for download
				FETCH FIRST FROM DownLoadTableList_cursor INTO @vchTableName
				WHILE @@FETCH_STATUS = 0
				BEGIN
					BEGIN TRY

					-- Clear the "ready-to-download" table in the remote SQL server staging database
					SET @vchRemoteTableName = @vchSQLServerName + '.' + @vchIPCImportData + '.dbo.' + @vchTableName

					IF exists(select 1 from information_schema.columns where table_name = @vchTableName and column_name = 'facility')					
						Set @bitHasFacilityColumn = 1
					ELSE
						Set @bitHasFacilityColumn = 0

					-- If the IPC is for Spare, the staging area holds muli-facility data otherwise only hold one facility which is defined in tblComputerconfig.
					IF @chrPackagingLine = 'SPARE' AND @bitHasFacilityColumn = 1
						SET @vchSQLStmt = 'DELETE ' + @vchRemoteTableName + ' Where facility = ''' + @chrFacility + ''''
					ELSE
						SET @vchSQLStmt = @vchSQLServerName + '.master.dbo.sp_executesql N''truncate table ' + @vchIPCImportData + '.dbo.' + @vchTableName + ''''

					-- Copy data from local SQL server to remote SQL server staging database
					IF @vchTableName = 'tblEquipment' 
						SET @vchSQLStmt = @vchSQLStmt + ';INSERT INTO ' + @vchRemoteTableName + ' (Active,facility,EquipmentID,[Type] ,SubType, Description,IPCSharedGroup) SELECT Active,facility, ' +
							'EquipmentID,[Type] ,SubType,Description, IPCSharedGroup FROM ' + @vchTableName + ' Where facility = ''' + @chrFacility + ''''
					ELSE
						IF @bitHasFacilityColumn = 1
							SET @vchSQLStmt = @vchSQLStmt + ';INSERT INTO ' + @vchRemoteTableName + ' SELECT * FROM ' + @vchTableName + ' Where facility = ''' + @chrFacility + ''''
						ELSE
							SET @vchSQLStmt = @vchSQLStmt + ';INSERT INTO ' + @vchRemoteTableName + ' SELECT * FROM ' + @vchTableName 

					-- In the DownloadTablelist Table, update the download status flag = 1 (i.e. success) and set the record Active to indicate the table needed to be download.
--WO#359			SET @vchSQLStmt = @vchSQLStmt + ';Update ' + @vchSQLServerName + '.' + @vchIPCImportData + '.dbo.tblDownLoadTableList set DownLoadStatus = 1, Active = 1 WHERE TableName =''' + 
--WO#359				@vchTableName + ''''
					SET @vchSQLStmt = @vchSQLStmt + ';Update ' + @vchSQLServerName + '.' + @vchIPCImportData + '.dbo.tblDownLoadTableList set DownLoadStatus = 1, Active = 1, LastDownLoad = ''' + CONVERT(varchar(23), GETDATE(),121) +	--FIX20160401
--FIX20160401		SET @vchSQLStmt = @vchSQLStmt + ';Update ' + @vchSQLServerName + '.' + @vchIPCImportData + '.dbo.tblDownLoadTableList set DownLoadStatus = 1, Active = 1, LastDownLoad = ''' + GETDATE() +	--WO#359
						'''  WHERE TableName =''' + @vchTableName + ''''	--WO#359
--print @vchSQLStmt		
						EXECUTE  (@vchSQLStmt)

					-- At least one table is required to be download.
					Set @bitAtLeast1TableOnList = 1 
	
					END TRY
					BEGIN CATCH
						IF @chrPackagingLine <> 'SPARE'		--WO#359
						BEGIN								--WO#359
							SET @ErrorMessage = 'Error in PPsp_ExportDataToLocalDB - (IPC: ' + @vchIPCName + ' Table:' + @vchRemoteTableName + ') - ' + ERROR_MESSAGE()
							EXEC PPsp_SndMsgForSupport @ErrorMessage
							BREAK
						END									--WO#359	
					END CATCH

					FETCH NEXT FROM DownLoadTableList_cursor INTO @vchTableName
				END

				IF @bitAtLeast1TableOnList = 1
				BEGIN
					-- Set Ready_For_DownLoad status to Yes in the Local SQL server for audit purpose
					Update dbo.tblComputerConfig set ReadyForDownLoad = 1 WHERE CURRENT OF LinkServer_cursor

					-- Set Ready_For_DownLoad status to Yes in the remote SQL server
					SET @vchSQLStmt ='Update ' + @vchSQLServerName  + '.' + @vchIPCImportData + '.dbo.tblComputerConfig SET ReadyForDownLoad = 1 WHERE ComputerName = ''' + @vchIPCName + ''''
--					print @vchSQLStmt
					EXECUTE  (@vchSQLStmt)
				END

			END TRY
			BEGIN CATCH
				IF @chrPackagingLine <> 'SPARE'		--WO#359
				BEGIN								--WO#359
					SET @ErrorMessage = 'Error in PPsp_ExportDataToLocalDB - (IPC:' + @vchIPCName + ') - ' + ERROR_MESSAGE()
					EXEC PPsp_SndMsgForSupport @ErrorMessage
				END									--WO#359
			END CATCH

			-- Read next computer name from table
			FETCH NEXT FROM LinkServer_cursor INTO @vchIPCName,@chrPackagingLine
		END

		CLOSE DownLoadTableList_cursor
		DEALLOCATE DownLoadTableList_cursor

		CLOSE LinkServer_cursor
		DEALLOCATE LinkServer_cursor

		Update tblDownLoadTableList set Active = 0 Where facility = @chrFacility 
	END TRY
	BEGIN CATCH
		CLOSE DownLoadTableList_cursor
		DEALLOCATE DownLoadTableList_cursor

		CLOSE LinkServer_cursor
		DEALLOCATE LinkServer_cursor


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
	END CATCH
END

GO

