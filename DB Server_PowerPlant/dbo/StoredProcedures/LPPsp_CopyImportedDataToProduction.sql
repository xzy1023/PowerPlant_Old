
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
    @vchComputerName varchar(50)
AS
BEGIN
	DECLARE @vchTableName as varchar(50); 
	DECLARE @vchSQLStmt as varchar(512);
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	BEGIN TRY
		if exists (Select ReadyForDownLoad from importdata.dbo.tblComputerConfig WHERE ComputerName = @vchComputerName AND ReadyForDownLoad = 1)
		BEGIN
			--SELECT [name] as TableName into #tblDownLoadTableList
			 --FROM importdata..sysobjects WHERE [type] = 'U' AND [name] <> 'tblDownLoadTableList'

				-- Declare scroll cursor for tblDownLoadTableList
			DECLARE DownLoadTableList_cursor CURSOR FOR
			SELECT TableName FROM importdata.dbo.tblDownLoadTableList WHERE Active = '1' 

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

				SET @vchSQLStmt = 'INSERT INTO ' + @vchTableName + ' SELECT * From importdata.dbo.' + @vchTableName
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
			--drop table #tblDownLoadTableList

			-- turn off the Ready to Download flag.
			--Set @vchComputerName = REPLACE(@@SERVERNAME,'\SQLEXPRESS','')
			UPDATE ImportData.dbo.tblComputerConfig 
				SET ReadyForDownLoad = 0 
				WHERE ComputerName = @vchComputerName
		END
	END TRY
	BEGIN CATCH
		PRINT 'Error in LPPsp_CopyImportedDataToProduction - ' + ERROR_MESSAGE()
	END CATCH
END

GO

