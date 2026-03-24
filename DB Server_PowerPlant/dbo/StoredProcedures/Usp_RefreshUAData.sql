
-- =============================================
-- Author:		Bong Lee
-- Create date: July. 2 2013
-- Description:	Refresh Data 
-- =============================================
CREATE PROCEDURE [dbo].[Usp_RefreshUAData]
	-- Add the parameters for the stored procedure here
	@vchFromServer varchar(50 ),
	@vchFromDB varchar(50 ),
	@vchToServer varchar(50 ),
	@vchToDB varchar(50 ),
	@intRefreshListNo int = 0,
	@vchFacilityFilter varchar(3 ) = NULL,
	@intStartFrom int = 0,
	@intEndAt int = 9999999
AS
BEGIN
	DECLARE @vchTableName as varchar(100);
	DECLARE @vchColumnName as varchar(100); 
	DECLARE @vchColumnNames as varchar(3000);
	DECLARE @bitIs_Identity as bit;
	DECLARE @bitHasIdentity as bit;
	DECLARE @bitHasFacility as bit;
	DECLARE @vchSQLStmt as varchar(5000);
	DECLARE @vchMigrationID as varchar(50);
	DECLARE @intRecordCount_From as int;
	DECLARE @vchWhere as varchar(100);
	DECLARE @nvchSQLStmt as nvarchar(500);
	DECLARE @ParmDefinition as NVARCHAR(500);
	DECLARE @intSystem_Type_id as int;
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 
	
	BEGIN TRY

		Select @vchFromServer = Case when @vchFromServer is null or @vchFromServer = 'localhost' then '' else '[' + RTRIM(@vchFromServer) + '].' end 
		Select @vchToServer = Case when @vchToServer is null or @vchToServer = 'localhost' then '' else '[' + RTRIM(@vchToServer) + '].' end 
		Select @vchMigrationID = 'ID' + replace(convert(varchar(50), getdate(), 120),' ','T')

		IF object_id('tempdb.dbo.#tblTableNames') is not null
			DROP TABLE #tblTableNames 
	
		Create table #tblTableNames
		(
			TableName varchar(255),
		);

		IF object_id('tempdb.dbo.#tblColumnList') is not null
			DROP TABLE #tblColumnList 
	
		Create table #tblColumnList
		(
			ColumnName varchar(255),
			is_Identity bit,
			system_type_id int
		);

		declare @sqlstatement nvarchar(4000)

		set @sqlstatement = 'INSERT INTO #tblTableNames SELECT [TableName] 
			FROM [dbo].tblTablesRefreshList 
			WHERE Sequence between @intStartFrom and @intEndAt and
					''1'' = case @intRefreshListNo
						 when 1 then [Active] 
						 when 2 then [Refresh1]
						 when 3 then [Refresh2]
						 else [Refresh3]
					  end	
			ORDER by [Sequence]'

		EXECUTE SP_EXECUTESQL @sqlstatement, N'@intStartFrom int, @intEndAt int, @intRefreshListNo int', 
			@intStartFrom = @intStartFrom,
			@intEndAt = @intEndAt,
			@intRefreshListNo = @intRefreshListNo


		--DECLARE object_cursor CURSOR FOR	
		--	SELECT [TableName] 
		--	FROM [dbo].tblTablesRefreshList 
		--	WHERE [Active] = 1 and Sequence between @intStartFrom and @intEndAt 
		--	ORDER by [Sequence]

		DECLARE object_cursor CURSOR FAST_FORWARD FOR 
			SELECT [TableName] FROM #tblTableNames

		OPEN object_cursor
		
		-- read the frist table name from the Table for copy
		FETCH NEXT FROM object_cursor INTO @vchTableName

		SELECT @bitHasIdentity = '0', @bitHasFacility = '0'

		WHILE @@FETCH_STATUS = 0
		BEGIN

			SET @nvchSQLStmt = 'SELECT @intRecordCount = count(*) FROM ' + @vchfromServer + @vchFromDB + '.dbo.' + @vchTableName 
			SET @ParmDefinition=N'@intRecordCount int OUTPUT'
			EXEC  sp_executesql @nvchSQLStmt , @ParmDefinition, @intRecordCount = @intRecordCount_From OUTPUT
--		print  @intRecordCount_From

			INSERT INTO [dbo].tblTablesRefreshLog 
			(MigrationID, TableName, [FromServer], [FromDataBase], [ToServer], [ToDataBase], [FacilityFilter], RecordCount_From)
			Values(@vchMigrationID
				,@vchTableName
				,replace(@vchFromServer,'.','')
				,@vchFromDB
				,replace(CASE WHEN @vchToServer = '' THEN @@SERVERNAME ELSE @vchToServer END,'.','')
				,@vchtoDB
				,@vchFacilityFilter
				,@intRecordCount_From)

			SET @vchSQLStmt = 'USE ' + @vchFromDB + '; INSERT INTO #tblColumnList ' + 
					'SELECT [Name], is_identity, system_type_id FROM sys.columns ' +
					'WHERE object_name(object_id) = ''' + RTRIM(@vchTableName) + ''''
			EXEC (@vchSQLStmt)

			DECLARE Columns_cursor CURSOR FOR	
				SELECT * FROM #tblColumnList 

			OPEN Columns_cursor
			SELECT  @vchColumnNames = '', @bitHasIdentity = '0' , @bitHasFacility = '0'

			-- read the  column names from the Table
			FETCH NEXT FROM Columns_cursor INTO @vchColumnName, @bitIs_Identity, @intSystem_Type_id
			WHILE @@FETCH_STATUS = 0
			BEGIN
				IF  @intSystem_Type_id <> 189  -- if the column is not timestamp data type
					SELECT @vchColumnNames = @vchColumnNames + '[' + @vchColumnName + '],'

				IF @bitIs_Identity = '1' 
					SET @bitHasIdentity = '1'

				IF  RTRIM(@vchColumnName) = 'Facility'
					SET @bitHasFacility = '1'

			FETCH NEXT FROM Columns_cursor INTO @vchColumnName, @bitIs_Identity, @intSystem_Type_id
			END

			CLOSE Columns_cursor
			DEALLOCATE Columns_cursor

			SET @vchSQLStmt = 'TRUNCATE table ' +  @vchToServer + @vchToDB + '.dbo.' + @vchTableName + '; '

			SELECT @vchWhere = ''
			IF @bitHasFacility = '1' and @vchFacilityFilter is not null
				SELECT @vchWhere = ' Where Facility = ''' + @vchFacilityFilter + ''''

			IF @bitHasIdentity = '1'
			BEGIN
				SELECT @vchColumnNames = SUBSTRING(@vchColumnNames,1, len(@vchColumnNames)-1)
				SELECT @vchSQLStmt = @vchSQLStmt + 'SET IDENTITY_INSERT ' + @vchToServer + @vchToDB + '.dbo.' + 
							  @vchTableName + ' ON;' + 'INSERT INTO ' + @vchToServer + @vchToDB + '.dbo.' + @vchTableName +
							   '(' + @vchColumnNames + ') SELECT ' + @vchColumnNames + ' FROM ' +
							   @vchfromServer + @vchFromDB + '.dbo.' + @vchTableName + @vchWhere
			END
			ELSE
			BEGIN
				SELECT @vchSQLStmt = @vchSQLStmt + 'INSERT INTO ' + @vchToServer + @vchToDB + 
						'.dbo.' + @vchTableName + ' SELECT * FROM ' + @vchfromServer + @vchFromDB + '.dbo.' + @vchTableName + @vchWhere
			END
		
			print @vchSQLStmt

			EXECUTE  (@vchSQLStmt)

			UPDATE [dbo].tblTablesRefreshLog 
				SET RecordCount_to = cast(@@rowcount as varchar(10)), LastUpdate = getdate()
				Where MigrationID = @vchMigrationID and TableName = @vchTableName

			DELETE FROM #tblColumnList

			FETCH NEXT FROM object_cursor INTO @vchTableName
			END
		CLOSE object_cursor;
		DEALLOCATE object_cursor;

		DROP TABLE #tblColumnList;

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

