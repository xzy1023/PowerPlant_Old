
-- =============================================
-- WO#1297      Oct. 10, 2014   Bong Lee
-- Description:	Pre process for downloading MS Dynamics AX Probat Equipment cross reference to Power Plant
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PreProcess_ProbatEquipment]

AS
BEGIN

	DECLARE		@vchFacility as char(3);
	DECLARE 	@vchImportData as varchar(50);
	DECLARE		@vchTableName as varchar(100);
	DECLARE		@vchDownLoadLogFileName varchar(255);
	DECLARE		@vchText varchar(255);

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		SELECT @vchDownLoadLogFileName = Value1  + Value2 + convert(varchar(8),getdate(),112) + '.txt' 
			FROM [tblControl] 
			WHERE [Key] = 'ERPDataXChgLogFolder' and [SubKey] = 'Interface';

		SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + ISNULL(OBJECT_NAME(@@PROCID),'') + ' - Started.'
		EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;

		SELECT @vchTableName = 'tblProbatEquipment',  @vchFacility = NULL;

		-- Call generic stored procedure to run post process of the Item Master Down Load for specific facility
		EXECUTE PPsp_ClearATable @vchTableName, @vchFacility

		SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + ISNULL(OBJECT_NAME(@@PROCID),'') + ' - Completed.'
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

			SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + ISNULL(OBJECT_NAME(@@PROCID),'') + ' - Failed.'
			EXEC PPsp_AppendToTextFile @vchDownLoadLogFileName, @vchText;
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

