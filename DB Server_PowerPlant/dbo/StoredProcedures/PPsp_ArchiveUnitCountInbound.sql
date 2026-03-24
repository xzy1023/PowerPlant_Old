
-- =============================================
-- Author:		Bong Lee
-- Create date: Oct. 16, 2007
-- Description:	Save unit count inbound interface table for given retention days
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ArchiveUnitCountInbound]
	@vchFacility as varchar(3),
	@intRetentionDays as tinyInt = 2
AS
BEGIN

	-- variables for error handler.
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
	DECLARE @ErrorProcedure nvarchar(200);
	DECLARE @ErrorLine int;
	DECLARE @ErrorNumber int;
	DECLARE @ErrorMessage NVARCHAR(4000);

	-- variables for sending error message in email.
	DECLARE @nvchSubject nvarchar(256);
	DECLARE @nvchBody nvarchar(MAX);
	SET XACT_ABORT ON;

	BEGIN TRY
		IF EXISTS (SELECT TxID 
				FROM tblUnitCountInbound 
				WHERE DATEDIFF(d,CreationTime,getdate()) < @intRetentionDays AND Facility = @vchFacility)
		BEGIN
			BEGIN TRANSACTION

			INSERT INTO [dbo].[tblUnitCountInboundHst]
			SELECT *
			FROM tblUnitCountInbound 
			WHERE ProcessingStatus = 2 and DATEDIFF(d,CreationTime,getdate()) >=  @intRetentionDays AND Facility = @vchFacility

			DELETE tblUnitCountInbound 
			WHERE ProcessingStatus = 2 and DATEDIFF(d,CreationTime,getdate()) >=  @intRetentionDays  AND Facility = @vchFacility
			COMMIT TRANSACTION
		END
	END TRY
	BEGIN CATCH

		IF (XACT_STATE()) = -1
			ROLLBACK TRANSACTION 

		SELECT 
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ' Message: ' + CHAR(13) + ERROR_MESSAGE();

		BEGIN TRY
			
			SELECT 
				@nvchSubject = @@ServerName + '.' + DB_NAME() + ' - ' + ISNULL(ERROR_PROCEDURE(),'') + N' fails.',
				@nvchBody = N'Error ' + CAST(@ErrorNumber as varchar(10)) +
							N', Level ' + CAST(@ErrorSeverity as varchar(10)) +
							N', State ' + CAST(@ErrorState as varchar(10)) + 
							N', Procedure ' + ISNULL(@ErrorProcedure,'') +
							N', Line ' + CAST(@ErrorLine as varchar(10)) +
							N', Message:' + CHAR(13) + ERROR_MESSAGE()

		END TRY
		BEGIN CATCH
		END CATCH

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine);
	END CATCH
END

GO

