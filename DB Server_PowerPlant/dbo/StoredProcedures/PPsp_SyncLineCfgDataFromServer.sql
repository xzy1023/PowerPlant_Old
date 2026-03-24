
-- =============================================
-- Author:		Bong Lee
-- Create date: May 03, 2018
-- Description:	Synchonize packaging Line Configuration on IPC From Server
--				It replaces data in the tblComputerConfig and tblEquipment tables on IPC with the server.
--				Update the packaging id in tblSessionControl with the packaging line id from the tblComputerConfig
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_SyncLineCfgDataFromServer]
	@vchFacility as varchar(3)
	,@vchComputerName as varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @vchSQLStmt as varchar(1200)
	DECLARE @vchLinkServer as varchar(50)
	DECLARE @varDBName as varchar(50)
	DECLARE @varStagingDB as varchar(50)
	DECLARE @varLocalDB as varchar(50)
	DECLARE @varPackagingLine as varchar(50)
	DECLARE @intCount as int

	BEGIN TRY
		SET @intCOUNT = 0
		SELECT @varStagingDB = 'ImportData', @varLocalDB = 'LocalPowerPlant', @vchLinkServer = '[' + @vchComputerName + '\SQLEXPRESS]'

		-- Activate the selected computer in the server Computer Configuration table
		UPDATE tblComputerConfig SET RecordStatus = 1 WHERE Facility = @vchFacility AND ComputerName = @vchComputerName

		-- Sync. the same data in staging and local data base on IPC from server.
		WHILE @intCOUNT < 2
		BEGIN
			
			IF @intCOUNT = 0
				SET @varDBName = @varStagingDB
			ELSE
				SET @varDBName = @varLocalDB
			-- If the IPC is for Spare, the staging area holds muli-facility data otherwise only hold one facility which is defined in tblComputerconfig.
			/* Update tblComputerConfig for the particular computer*/
			SET @vchSQLStmt = 'DELETE ' +  @vchLinkServer + '.' + @varDBName + '.dbo.tblComputerConfig Where facility = ''' + @vchFacility + ''''

			SET @vchSQLStmt = @vchSQLStmt + '; INSERT INTO [' + @vchComputerName + '\SQLEXPRESS].' + @varDBName + '.dbo.tblComputerConfig' +
				' SELECT * FROM tblComputerConfig Where facility = ''' + @vchFacility + ''''
					print @vchSQLStmt
					EXEC(@vchSQLStmt) 

			/* Refresh Equiptment table from server to IPC */
			SET @vchSQLStmt = 'DELETE ' +  @vchLinkServer + '.' + @varDBName + '.dbo.tblEquipment Where facility = ''' + @vchFacility + ''''

			SET @vchSQLStmt = @vchSQLStmt + '; INSERT INTO [' + @vchComputerName + '\SQLEXPRESS].' + @varDBName + '.dbo.tblEquipment' +
			'(Active, facility, EquipmentID, [Type], SubType, [Description], IPCSharedGroup, WorkCenter) ' +
			'SELECT Active, facility, EquipmentID, [Type], SubType, [Description], IPCSharedGroup, WorkCenter ' +
				'FROM tblEquipment Where facility = ''' + @vchFacility + ''''
					print @vchSQLStmt
					EXEC(@vchSQLStmt)
		
			SET @intCOUNT = @intCOUNT + 1
		END 

		/* Update Session Control table on IPC */
			SELECT  @varPackagingLine = PackagingLine FROM tblComputerConfig WHERE Facility = @vchFacility AND ComputerName = @vchComputerName

			SET @vchSQLStmt = 'Update [' + @vchComputerName + '\SQLEXPRESS].' + @varLocalDB + '.dbo.tblSessionControl ' + 
			'SET DefaultPkgLine = ''' + @varPackagingLine + ''', ' +
			 'OverridePkgLine = ''' + @varPackagingLine + ''', ComputerName = ''' + @vchComputerName + ''''
		 			print @vchSQLStmt
					EXEC(@vchSQLStmt)
	
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

