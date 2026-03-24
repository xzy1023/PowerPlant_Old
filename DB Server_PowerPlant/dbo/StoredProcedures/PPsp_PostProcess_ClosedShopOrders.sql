
-- =============================================
-- WO#1297      Aug. 4, 2014   Bong Lee
-- Description:	Post process for uploading closed shop orders to ERP system
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PostProcess_ClosedShopOrders] 
					
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
	
	DECLARE @vchLogFileName varchar(255);
	DECLARE @vchText varchar(255);
	DECLARE @intRRN int;
    DECLARE @chrFacility char(3);
	DECLARE @intShopOrder int;
	DECLARE @dteBPCSClosingTime as datetime;
	DECLARE @vchClosedShopOrders as varchar(1000);

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

		Select @vchLogFileName = Value1  + 'ClosedShopOrderLog_' + convert(varchar(8),getdate(),112) + '.txt' 
			FROM [tblControl] 
			WHERE [Key] = 'ERPDataXChgLogFolder' and [SubKey] = 'Interface';

		SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + ISNULL(OBJECT_NAME(@@PROCID),'') + ' - Started.'
		EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText;

		SELECT @vchClosedShopOrders = ''
		-- Select unposted closed shop order for which all its related pallet has been posted to ERP
		DECLARE csrClosedSOHst CURSOR FAST_FORWARD FOR
			SELECT tCSOH.RRN, tCSOH.Facility, tCSOH.ShopOrder FROM tblToBeClosedShopOrder tCSOH
			WHERE tCSOH.UpdatedToBPCS = '1' 
			ORDER BY tCSOH.Facility, tCSOH.ShopOrder, tCSOH.CreationTime

		OPEN csrClosedSOHst

		-- read a record from the tblClosedShopOrder and load the data to the variables
		FETCH NEXT FROM csrClosedSOHst INTO @intRRN, @chrFacility, @intShopOrder
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION Trn1;	
				BEGIN TRY
					INSERT INTO tblClosedShopOrderHst
						(Facility, ShopOrder, DefaultPkgLine, Operator, SessionStartTime, ClosingTime, UpdatedToBPCS, BPCSClosingTime, LastUpdated, CreationTime)
						SELECT Facility, ShopOrder, DefaultPkgLine, Operator, SessionStartTime, ClosingTime, UpdatedToBPCS, BPCSClosingTime, LastUpdated, CreationTime
						FROM tblToBeClosedShopOrder
						WHERE (RRN = @intRRN);
					DELETE FROM tblToBeClosedShopOrder WHERE RRN = @intRRN;
					SET @vchClosedShopOrders = @vchClosedShopOrders + RTRIM(cast(@intShopOrder as varchar(20))) + ','
				END TRY
				BEGIN CATCH

					IF (XACT_STATE()) = -1
						ROLLBACK TRANSACTION; 

					SET @vchText = 'Error on closing shop order ' + cast(@intShopOrder as varchar(20)) + ': ' + ERROR_MESSAGE()
					EXEC PPsp_AppendToTextFile @vchLogFileName,  @vchText
					
					SELECT 
						@ErrorSeverity = ERROR_SEVERITY(),
						@ErrorState = ERROR_STATE(),
						@ErrorProcedure = ERROR_PROCEDURE(),
						@ErrorLine  = ERROR_LINE(),
						@ErrorNumber  = ERROR_NUMBER(),
						@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ' Message: ' + CHAR(13) + ERROR_MESSAGE();
					
					SELECT
						@nvchSubject = @@ServerName + '.' + DB_NAME() + ' - ' + ISNULL(ERROR_PROCEDURE(),'') + N' fails.',
						@nvchBody = N'Error ' + CAST(@ErrorNumber as varchar(10)) +
									N', Level ' + CAST(@ErrorSeverity as varchar(10)) +
									N', State ' + CAST(@ErrorState as varchar(10)) + 
									N', Procedure ' + @ErrorProcedure +
									N', Line ' + CAST(@ErrorLine as varchar(10)) +
									N', Message:' + CHAR(13) + ERROR_MESSAGE();

						EXEC PPsp_SndMsgToOperator @nvchBody, @nvchSubject;
				END CATCH

			IF @@TRANCOUNT > 0
				COMMIT TRANSACTION Trn1;

			-- read a record from the tblClosedShopOrder and load the data to the variables
			FETCH NEXT FROM csrClosedSOHst INTO @intRRN, @chrFacility, @intShopOrder;
		END

		-- if the closed shop order list is not blank, take out the last comma
		IF @vchClosedShopOrders <> ''
		BEGIN
			SET @vchClosedShopOrders = LTRIM(LEFT(@vchClosedShopOrders,Len(@vchClosedShopOrders)-1));
		END

		SET  @vchText ='Closed Shop Orders Are: ' + CASE WHEN @vchClosedShopOrders = '' THEN 'Nothing.' ELSE @vchClosedShopOrders END;
		exec PPsp_AppendToTextFile @vchLogFileName, @vchText;
	
		-- Reset variable for actual process
		SET @vchClosedShopOrders = '';

		CLOSE csrClosedSOHst
		DEALLOCATE csrClosedSOHst

		SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + ISNULL(OBJECT_NAME(@@PROCID),'') + ' - Completed.';
		EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText;

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
			
			SELECT 
				@nvchSubject = @@ServerName + '.' + DB_NAME() + ' - ' + ISNULL(ERROR_PROCEDURE(),'') + N' fails.',
				@nvchBody = N'Error ' + CAST(@ErrorNumber as varchar(10)) +
							N', Level ' + CAST(@ErrorSeverity as varchar(10)) +
							N', State ' + CAST(@ErrorState as varchar(10)) + 
							N', Procedure ' + @ErrorProcedure +
							N', Line ' + CAST(@ErrorLine as varchar(10)) +
							N', Message:' + CHAR(13) + ERROR_MESSAGE()

			EXEC PPsp_SndMsgToOperator @nvchBody, @nvchSubject;
			SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + ISNULL(OBJECT_NAME(@@PROCID),'') + ' - Failed.';
			EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText;
		END TRY
		BEGIN CATCH
		END CATCH

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine);
	END CATCH
END

GO

