-- =============================================
-- Author:		Bong Lee
-- Create date: 2/5/2016
-- Description:	Reassign existing packaging line ID To a specific IPC
-- =============================================
CREATE PROCEDURE [dbo].[Usp_ReassignLineToIPC] 

	@vchToLineID char(10),
	@vchIPCNameToBeUsed varchar(50)
AS
BEGIN
Print @vchIPCNameToBeUsed
	--SET XACT_ABORT ON;
	DECLARE @vchIPCNameOfToLine as varchar(50)
	DECLARE @chrFacility as char(3)
	--DECLARE @vchToLineID as char(10)
	DECLARE @vchSQLStmt as varchar(1200)
	DECLARE @vchLineDesc as varchar(50)
	BEGIN TRY

		SELECT @chrFacility = value1 from tblcontrol
			WHERE [KEY] = 'Facility' and SubKey = 'General'

		--Select @vchLineDesc = [Description] from [dbo].[tblEquipment]
		--	Where [EquipmentID] = @vchToLineID

		SELECT  top 1 @vchIPCNameOfToLine = ComputerName
		FROM            tblComputerConfig
		WHERE        (PackagingLine = @vchToLineID)
	
		IF @vchIPCNameToBeUsed <> @vchIPCNameOfToLine
		BEGIN
			BEGIN TRANSACTION
				update tblcomputerconfig set ComputerName= Reverse(@vchIPCNameToBeUsed) where ComputerName = @vchIPCNameToBeUsed;
				update tblcomputerconfig set ComputerName= @vchIPCNameToBeUsed,[RecordStatus] = 1 where ComputerName =  @vchIPCNameOfToLine;
				update tblcomputerconfig set ComputerName= @vchIPCNameOfToLine, RecordStatus=0 where ComputerName = Reverse(@vchIPCNameToBeUsed);
			COMMIT TRANSACTION

			--importdata
			SET @vchSQLStmt = 'update [' + @vchIPCNameToBeUsed + '\sqlexpress].importdata.dbo.tblcomputerconfig set ComputerName = ''' + Reverse(@vchIPCNameToBeUsed) + ''' where ComputerName = ''' + @vchIPCNameToBeUsed + ''';' +
				'update [' + @vchIPCNameToBeUsed + '\sqlexpress].importdata.dbo.tblcomputerconfig set ComputerName = ''' + @vchIPCNameToBeUsed + ''' ,[RecordStatus] = 1 where ComputerName = ''' +  @vchIPCNameOfToLine + ''';' +
				'update [' + @vchIPCNameToBeUsed + '\sqlexpress].importdata.dbo.tblcomputerconfig set ComputerName = ''' + @vchIPCNameOfToLine + ''', RecordStatus=0 where ComputerName = ''' + Reverse(@vchIPCNameToBeUsed) + '''' 
			print @vchSQLStmt
			EXEC(@vchSQLStmt) 

			--LocalPowerPlant
			--SET @vchSQLStmt = 'update [' + @vchIPCNameToBeUsed + '\sqlexpress].LocalPowerPlant.dbo.tblcomputerconfig set recordstatus = 1, ReadyForDownLoad = 1,  packagingline = ''' + @vchToLineID + ''' ,Facility = ''' + @chrFacility + ''' ,[Description] = ''' + @vchLineDesc + ''' where ComputerName = ''' + @vchIPCName + ''''
			SET @vchSQLStmt = 'update [' + @vchIPCNameToBeUsed + '\sqlexpress].LocalPowerPlant.dbo.tblcomputerconfig set ComputerName = ''' + Reverse(@vchIPCNameToBeUsed) + ''' where ComputerName = ''' + @vchIPCNameToBeUsed + ''';' +
				'update [' + @vchIPCNameToBeUsed + '\sqlexpress].LocalPowerPlant.dbo.tblcomputerconfig set ComputerName = ''' + @vchIPCNameToBeUsed + ''' ,[RecordStatus] = 1 where ComputerName = ''' +  @vchIPCNameOfToLine + ''';' +
				'update [' + @vchIPCNameToBeUsed + '\sqlexpress].LocalPowerPlant.dbo.tblcomputerconfig set ComputerName = ''' + @vchIPCNameOfToLine + ''', RecordStatus=0 where ComputerName = ''' + Reverse(@vchIPCNameToBeUsed) + '''' 
					print @vchSQLStmt
			EXEC(@vchSQLStmt)  

			SET @vchSQLStmt = 'update [' + @vchIPCNameToBeUsed + '\sqlexpress].LocalPowerPlant.dbo.tblSessionControl set facility = ''' + @chrFacility + ''', computername =  ''' + @vchIPCNameToBeUsed + ''', DefaultPkgLine = ''' + @vchToLineID + ''', OverridePkgLine = ''' + @vchToLineID + ''', ShopOrder = 0'
			print @vchSQLStmt
			EXEC(@vchSQLStmt) 
		END

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		-- WO#817 Add Start		
		IF (XACT_STATE()) = -1
			ROLLBACK TRANSACTION 
		-- WO#817 Add End
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

