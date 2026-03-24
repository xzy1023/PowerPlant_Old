
-- =============================================
-- WO#5370:     Sep. 19, 2017   Bong Lee
-- Descripton:	Update the processed record in tblUnitCountInbound
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_UnitCountInbound_Upd] 
	@vchFacility as varchar(3)
	,@intTxID as int
	,@intPalletID as int
	,@intProcessStatus as tinyint
AS
BEGIN
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
	DECLARE @ErrorProcedure nvarchar(200);
	DECLARE @ErrorLine int;
	DECLARE @ErrorNumber int;
	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @nvchSubject nvarchar(256);
	DECLARE @nvchBody nvarchar(MAX);

	SET NOCOUNT ON;

	BEGIN TRY
			UPDATE tblUnitCountInbound 
				SET LastUpdateTime = GETDATE()
					,PalletId = @intPalletId
					,ProcessingStatus = @intProcessStatus
				WHERE Facility = @vchFacility AND TxID = @intTxID 
  	END TRY
	BEGIN CATCH

		SELECT 
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ' Message: ' + CHAR(13) + ERROR_MESSAGE();

		BEGIN TRY
			IF (XACT_STATE()) = -1
				ROLLBACK TRANSACTION; 

			SELECT 
				@nvchSubject = @@ServerName + '.' + DB_NAME() + ' - ' + ISNULL(ERROR_PROCEDURE(),'') + N' fails.',
				@nvchBody = N'Error ' + CAST(@ErrorNumber as varchar(10)) +
							N', Level ' + CAST(@ErrorSeverity as varchar(10)) +
							N', State ' + CAST(@ErrorState as varchar(10)) + 
							N', Procedure ' + @ErrorProcedure +
							N', Line ' + CAST(@ErrorLine as varchar(10)) +
							N', Message:' + CHAR(13) + ERROR_MESSAGE()

			EXEC PPsp_SndMsgToOperator @nvchBody, @nvchSubject;

		END TRY
		BEGIN CATCH
		END CATCH

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine);
	END CATCH
END

GO

