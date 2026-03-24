

-- ===============================================================
-- Author:		Bong Lee
-- Create date: Nov. 04, 2006
-- Description:	Copy Imported Data to Production DataBase in Remote server
--
-- Mod.	001: Bong Lee	2007-03-08
--		When import SO data, reserve the value of the Column "Closed" to
--		prevent reopening the locally closed shop orders.  
-- ===============================================================
CREATE PROCEDURE [dbo].[LPPsp_CopyImportedDataToProduction]
    @vchComputerName varchar(50),
	@chrFacility as char(3) = NULL
AS
BEGIN
	DECLARE @vchTableName as varchar(50); 
	DECLARE @vchSQLStmt as varchar(512);
	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	BEGIN TRY
		-- Check whether the data is ready for download to the computer
		if exists (Select ReadyForDownLoad from importdata.dbo.tblComputerConfig WHERE ComputerName = @vchComputerName AND ReadyForDownLoad = 1)
		BEGIN
			if @chrFacility is NULL
				Select @chrFacility = facility from importdata.dbo.tblComputerConfig where ComputerName  = @vchComputerName

			-- Declare scroll cursor for tblDownLoadTableList
			DECLARE DownLoadTableList_cursor CURSOR FOR
			SELECT TableName FROM importdata.dbo.tblDownLoadTableList WHERE Active = '1' AND facility = @chrFacility

			OPEN DownLoadTableList_cursor

			-- read the frist table name from the Table for download
			FETCH NEXT FROM DownLoadTableList_cursor INTO @vchTableName
			WHILE @@FETCH_STATUS = 0
			BEGIN
				IF @vchTableName = 'tblShopOrder'
				BEGIN
					TRUNCATE Table tblClosedShopOrder
					INSERT INTO tblClosedShopOrder
					SELECT ShopOrder from tblShopOrder Where Closed = 1
				END	

				SET @vchSQLStmt = 'TRUNCATE Table ' +  @vchTableName
				EXECUTE  (@vchSQLStmt)

				IF @vchTableName = 'tblItemNotes'
					SET @vchSQLStmt = 'INSERT INTO ' + @vchTableName + ' SELECT * From importdata.dbo.' + @vchTableName
				ELSE
						SET @vchSQLStmt = 'INSERT INTO ' + @vchTableName + ' SELECT * FROM importdata.dbo.' + @vchTableName + ' Where facility = ''' + @chrFacility + ''''
Print @vchSQLStmt
				EXECUTE  (@vchSQLStmt)

				IF @vchTableName = 'tblShopOrder'
				BEGIN
					UPDATE tblShopOrder SET Closed = 1 WHERE ShopOrder IN 
					(SELECT ShopOrder from tblClosedShopOrder)
				END	
				-- Set Active flag back to '0' when the copy is done.
				Update importdata.dbo.tblDownLoadTableList set Active = 0 WHERE CURRENT OF DownLoadTableList_cursor
				FETCH NEXT FROM DownLoadTableList_cursor INTO @vchTableName
			END

			CLOSE DownLoadTableList_cursor
			DEALLOCATE DownLoadTableList_cursor

			-- turn off the Ready to Download flag.
			UPDATE ImportData.dbo.tblComputerConfig 
				SET ReadyForDownLoad = 0 
				WHERE ComputerName = @vchComputerName
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

