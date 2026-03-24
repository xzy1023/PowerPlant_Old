

-- =============================================
-- Author:		Bong Lee
-- Create date: Jan. 10, 2008
-- Description:	Swap the value of the facility of the IPC tables
 
-- =============================================
CREATE PROCEDURE [dbo].[LPPsp_SwapIPCFacility] 
	-- Add the parameters for the stored procedure here
	@vchIPCToBeReplaced varchar(50) = NULL,
	@chrFromFacility Char(3)=NULL,
	@chrToFacility Char(3)=NULL
AS
BEGIN
	
	DECLARE @vchTableName as varchar(50);
	DECLARE @vchSQLStmt as varchar(100);
	DECLARE @vchSpareIPC as varchar(50);
	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	SELECT @vchSpareIPC = convert(varchar(50), SERVERPROPERTY('MachineName'))	-- i.e. the computer runs this stored procedure


	--If the facility parameters are not specified,
	--the "To" facility will be picked up from the facility of the To-Be-Replaced IPC in the tblComputerConfig.  
	--and the "From" facility will be based on the facility of the Spare IPC.
	--So the spare IPC facility will be changed to same as the To-Be-replaced IPC.

	IF @chrToFacility is NULL
		SELECT @chrToFacility = Facility from importdata.dbo.tblComputerConfig WHERE ComputerName= @vchIPCToBeReplaced

	IF @chrFromFacility is NULL
		SELECT @chrFromFacility = Facility from importdata.dbo.tblComputerConfig WHERE ComputerName = @vchSpareIPC

	IF @chrFromFacility <> @chrToFacility
	Begin
		BEGIN TRY
			--UPDATE importdata.dbo.tblDownLoadTableList SET Active = 1 WHERE facility = @chrToFacility
			UPDATE importdata.dbo.tblComputerConfig SET ReadyForDownLoad = 1 WHERE ComputerName = @vchSpareIPC
			
			DECLARE object_cursor CURSOR FOR	
					SELECT TableName FROM importdata.dbo.tblDownLoadTableList WHERE facility = @chrToFacility

			OPEN object_cursor

			-- read the frist table name from the Download Table List
			FETCH NEXT FROM object_cursor INTO @vchTableName
			WHILE @@FETCH_STATUS = 0
				BEGIN
					IF 	@vchTableName <> 'tblComputerConfig'
					BEGIN
						SET @vchSQLStmt = 'TRUNCATE TABLE ' + @vchTableName
						EXECUTE (@vchSQLStmt)
						UPDATE importdata.dbo.tblDownLoadTableList set active = 1 WHERE CURRENT OF object_cursor
					END
					FETCH NEXT FROM object_cursor INTO @vchTableName
				END

			CLOSE object_cursor
			DEALLOCATE object_cursor

			EXEC LPPsp_CopyImportedDataToProduction @vchSpareIPC, @chrToFacility
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
	
END

GO

