

-- =============================================
-- Author:		Bong Lee
-- Create date: Feb. 20, 2008
-- Description:	Alter table for each IPC
-- =============================================
CREATE PROCEDURE [dbo].[del_UTsp_AlterIPCTables] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @vchRemoteServerName as varchar(50);
    Declare @vchServerName as varChar(50)
	Declare @vchFileName as varChar(50)
	Declare @vchSQLStmt as varChar(1000)
	Declare @vchFinalSQLStmt as nvarchar(1200)

	BEGIN TRY
	--Set @vchServerName = N'[MPHOIPC15\SQLEXPRESS]'
	
	-- Declare cursor for tblComputerConfig
	DECLARE LinkServer_cursor CURSOR FOR
		SELECT ComputerName FROM dbo.tblComputerConfig
		WHERE RecordStatus = 1
		ORDER BY ComputerName
	OPEN LinkServer_cursor
	-- Read computer Name (i.e. remote server name)
	FETCH NEXT FROM LinkServer_cursor INTO @vchRemoteServerName

	WHILE @@FETCH_STATUS = 0
	BEGIN
		Begin Try
			-- Construct Remote SQL Server Name from Computer Configuration Table
			SET @vchServerName = '['+ @vchRemoteServerName + '\SQLEXPRESS]'
			Set @vchFileName = 'ImportData.dbo.tblDownLoadTableList'

			Set @vchSQLStmt = 'ALTER TABLE ' + @vchFileName + ' ADD  Facility char(3);'
			Set @vchFinalSQLStmt = N'EXEC (''' + @vchSQLStmt + ''') At ' + @vchServerName
			EXEC sp_executesql @vchFinalSQLStmt

			Set @vchSQLStmt = @vchServerName + '.master.dbo.sp_executesql N' +
				'''Update ' + @vchFileName + ' set Facility = ''''01 '''';' +
				'ALTER TABLE ' + @vchFileName + ' Alter column Facility char(3) NOT NULL;' +
				'Alter TABLE ' + @vchFileName + ' drop CONSTRAINT [PK_tblLocalTableList];' +
				'Alter TABLE ' + @vchFileName + ' ADD CONSTRAINT [PK_tblDownLoadTableList] PRIMARY KEY CLUSTERED ([Facility] ASC,[TableName] ASC)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]'''		
			EXECUTE  (@vchSQLStmt)

		--	Change Down Time Reason Code table

			Set @vchFileName = 'ImportData.dbo.tblDTReasonCode'

			Begin Try
				Set @vchSQLStmt = @vchServerName + '.master.dbo.sp_executesql N' +
					'''Alter TABLE ' + @vchFileName + ' drop CONSTRAINT [PK_tblDTReasonCode_1];''' 
				EXECUTE  (@vchSQLStmt)
			end try
			Begin Catch
				PRINT @vchRemoteServerName + '-(1)-' + ERROR_MESSAGE()
			End catch

			Begin Try
				Set @vchSQLStmt = @vchServerName + '.master.dbo.sp_executesql N' +
					'''Alter TABLE ' + @vchFileName + ' drop CONSTRAINT [PK_tblDTReasonCode];''' 
				EXECUTE  (@vchSQLStmt)
			End Try
			Begin Catch
				print @vchRemoteServerName + '-(2)-' + error_message()
			End catch

			Set @vchSQLStmt = @vchServerName + '.master.dbo.sp_executesql N' +
				'''Alter TABLE ' + @vchFileName + ' ADD CONSTRAINT [PK_tblDTReasonCode] PRIMARY KEY CLUSTERED ([Facility] ASC,[MachineType] ASC,[MachineSubType] ASC,[ReasonType] ASC,[ReasonCode] ASC) WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]'''		
			EXECUTE  (@vchSQLStmt)
		End Try
		Begin Catch
			PRINT @vchRemoteServerName + '-(3)-' + ERROR_MESSAGE()
		End Catch

		-- Read next computer name from table
			FETCH NEXT FROM LinkServer_cursor INTO @vchRemoteServerName
		END

		CLOSE LinkServer_cursor
		DEALLOCATE LinkServer_cursor

	END TRY
	BEGIN CATCH
		CLOSE LinkServer_cursor
		DEALLOCATE LinkServer_cursor
		PRINT @vchRemoteServerName + '-(4)-' + ERROR_MESSAGE()
	END CATCH

END

GO

