
-- =============================================
-- WO#1297      Jul. 4, 2014   Bong Lee
-- Description:	Post process for downloading MS Dynamics AX shop order related data to Power Plant for facility 01
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PostProcess_DownLoadShopOrder_01]

AS
BEGIN

	DECLARE	@vchFacility as varchar(3);
	DECLARE	@vchDownLoadLogFileName varchar(255);
	DECLARE	@vchText varchar(255);
	DECLARE	@vchPPDBName nvarchar(100); 
	DECLARE	@vchSQLStmt nvarchar(500);
	DECLARE	@vchParmDefinition nvarchar(500);

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		
		SELECT @vchFacility = Right(OBJECT_NAME(@@PROCID), Charindex('_', Reverse(OBJECT_NAME(@@PROCID)))-1);

		SELECT @vchDownLoadLogFileName = Value1  + Value2 + convert(varchar(8),getdate(),112) + '.txt' 
			FROM [tblControl] 
			WHERE [Key] = 'ERPDataXChgLogFolder' and [SubKey] = 'Interface';

		SELECT @vchPPDBName = Value1
			FROM [tblControl] 
			WHERE [Key] = 'PowerPlantDBName' and [SubKey] = 'General';

		--SET @vchText = ISNULL(OBJECT_NAME(@@PROCID),'') + ' ' + @@ServerName + '.' + DB_NAME() + ' - Started.';
		--EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;

		-- Call generic stored procedure to run post process of the shop order data Down Load for specific facility
		SET @vchSQLStmt =  N'EXEC ' + @vchPPDBName + N'.dbo.PPsp_PostProcess_DownLoadShopOrder @vchFacility';
		SET @vchParmDefinition = N'@vchFacility varchar(3)';

		EXECUTE sp_executesql @vchSQLStmt, @vchParmDefinition, @vchFacility;
		
		SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + ISNULL(OBJECT_NAME(@@PROCID),'') + ' - Completed.';
		EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;

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
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d, Message:' + CHAR(13) + ERROR_MESSAGE();

		BEGIN TRY
			SELECT 
				@nvchSubject = @@ServerName + '.' + DB_NAME() + ' - ' + ISNULL(ERROR_PROCEDURE(),'') + N' fails.',
				@nvchBody = N'Error ' + CAST(@ErrorNumber as varchar(10)) +
							N', Level ' + CAST(@ErrorSeverity as varchar(10)) +
							N', State ' + CAST(@ErrorState as varchar(10)) + 
							N', Procedure ' + @ErrorProcedure +
							N', Line ' + CAST(@ErrorLine as varchar(10)) +
							N', Message:' + CHAR(13) + ERROR_MESSAGE()
				 
			EXEC PPsp_SndMsgToOperator @nvchBody, @nvchSubject
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

