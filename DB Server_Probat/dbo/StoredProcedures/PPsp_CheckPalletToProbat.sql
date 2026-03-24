
-- =============================================
-- WO#871	    Jun. 5, 2015   Bong Lee
-- Description:	Check no. of pallet records that will be sent to Power Plant/Probat interface server
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_CheckPalletToProbat]
	@intIdleTimeThreshold as int			-- in minutes
AS
BEGIN

	DECLARE @vchServerName as varchar(50);
	DECLARE @vchSQLInstance as varchar(50);
	DECLARE @vchDBName as varchar(50);
	DECLARE @bitConnected as bit;
	DECLARE @vchSQLStmt as nvarchar(500);
	DECLARE @ParmDefinition as nvarchar(500);
	DECLARE @intMaxIdleTime as int;

	DECLARE @nvchEmailSubject nvarchar(256);
	DECLARE @nvchEmailBody nvarchar(MAX);

	SET @ParmDefinition = N'@MaxIdleTime int OUTPUT'

	BEGIN TRY

		SELECT @vchSQLInstance = [Value1], @vchDBName = [Value2]
				,@vchServerName = CASE WHEN CHARINDEX('\',[Value1]) > 0 
								THEN LEFT([Value1], CHARINDEX('\',[Value1])-1) 
								ELSE [Value1] END
			FROM [dbo].[tblControl] 
			WHERE [key]='Probat' AND [SubKey] = 'InterfaceFileServer'

		EXEC PPsp_IsDeviceConnected @vchServerName, @bitConnected OUTPUT
	
		IF @bitConnected = 1
		BEGIN
			SELECT  @vchSQLStmt = 'SELECT TOP 1 @MaxIdleTime = Datediff(minute,[CREATE_TIMESTAMP],getdate()) FROM [' + @vchSQLInstance + '].' + @vchDBName + 
					'.[dbo].[PRO_IMP_PALLET] WHERE [TRANSFERED] = 0 ORDER BY CREATE_TIMESTAMP ';
			EXECUTE sp_executesql @vchSQLStmt, @ParmDefinition, @MaxIdleTime = @intMaxIdleTime OUTPUT;
			IF @intMaxIdleTime IS NOT NULL and @intMaxIdleTime > @intIdleTimeThreshold
			BEGIN
				SELECT @nvchEmailBody = 'Probat has not picked up pallet records from the Power Plant/Probat interface table for ' + cast(@intMaxIdleTime as varchar(4)) + ' minutes.'
			END
		END
		ELSE
		BEGIN
			SELECT @nvchEmailBody = 'Can not connect to the Power Plant/Probat interface computer, ' + @vchServerName + '.' 
		END
		
		IF @nvchEmailBody <> ''
		BEGIN
			SET @nvchEmailSubject = 'Warning! Communication disturbed between Power Plant and Probat. SQL instance - ' + @vchSQLInstance + ', DB - ' + @vchDBName
			EXEC PPsp_SndMsgToOperator @nvchEmailBody, @nvchEmailSubject,NULL,N'Probat Alert Notification List';
		END

	END TRY
	BEGIN CATCH
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure nvarchar(200);
		DECLARE @ErrorLine int;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorMessage NVARCHAR(4000);

		DECLARE @nvchSubject nvarchar(256);
		DECLARE @nvchBody nvarchar(MAX);

		SELECT 
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ISNULL(ERROR_PROCEDURE(),''),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d, Message:' + CHAR(13) + ERROR_MESSAGE();

		BEGIN TRY
			SELECT 
				@nvchSubject = @@ServerName + '.' + DB_NAME() + ' - ' + @ErrorProcedure + N' fails.',
				@nvchBody = N'Error ' + CAST(@ErrorNumber as varchar(10)) +
							N', Level ' + CAST(@ErrorSeverity as varchar(10)) +
							N', State ' + CAST(@ErrorState as varchar(10)) + 
							N', Procedure ' + @ErrorProcedure +
							N', Line ' + CAST(@ErrorLine as varchar(10)) +
							N', Message:' + CHAR(13) + ERROR_MESSAGE();

			EXEC PPsp_SndMsgToOperator @nvchBody, @nvchSubject;

		END TRY
		BEGIN CATCH
		END CATCH
		-- Use RAISERROR inside the CATCH block to return error
		-- information about the original error that caused
		-- execution to jump to the CATCH block.
		RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)
	END CATCH
END

GO

