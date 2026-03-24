
-- =============================================
-- WO#1297      Oct 20, 2014   Bong Lee
-- Description:	Post process for uploading pallets created for Probat to ERP
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PostProcess_ProbatPallet] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @vchLogFileName varchar(255);
	DECLARE @vchISeriesFile varchar(10);
	DECLARE @vchSQLStmt varchar(512);
	DECLARE @vchText varchar(255);
	DECLARE @chrEnvironment char(1);
	DECLARE @vchFacility varchar(3);
	DECLARE @vchPalletIDs varchar(1000);
	DECLARE @intCount int;
	DECLARE @intCountPosted int;

	DECLARE @intCUSTOMER_ID int;
	DECLARE @intTRANSFERED int;
	DECLARE @dteTRANSFERED_TIMESTAMP datetime;
	DECLARE @chrACTIVITY	char(1);
	DECLARE @vchPALLET_ID varchar(20);
	DECLARE @dteCREATE_TIMESTAMP datetime;
	DECLARE @vchRECEIVING_STATION varchar(20);
	DECLARE @vchCUSTOMER_CODE varchar(20);
	DECLARE @intAMOUNT int;
	DECLARE @vchORDER_NAME varchar(20);
	DECLARE @intORDER_TYP int;
	DECLARE @dteSTART_TIMESTAMP datetime;
	DECLARE @chrORDER_COMPLETE	char(1);
	DECLARE @intShopOrder	int;
	DECLARE @vchPalletID_PXPAL	varchar(9);
	DECLARE @decQtyOnPallet	decimal(11,3);
	DECLARE @intTransactionSeq	int;
	DECLARE @intCreationDate	int;
	DECLARE @intCreationTime	int;
	DECLARE @vchCreatedBy	varchar(10);
	DECLARE @intMaintenanceDate	int;
	DECLARE @intMaintenanceTime	int;
	DECLARE @vchMaintainedBy	varchar(10);
	
	BEGIN TRY
		SELECT @intCount = 0, @intCountPosted = 0, @vchPalletIDs = ''
		SELECT     @chrEnvironment = LEFT(Value2,1)
		FROM         tblControl
		WHERE     [Key] = N'CompanyName' AND SubKey = 'General';


		Select @vchLogFileName = Value1  + 'UploadLog_' + convert(varchar(8),getdate(),112) + '.txt' 
			FROM [tblControl] 
			WHERE [Key] = 'ERPDataXChgLogFolder' and [SubKey] = 'Interface';

		SET @vchText = @@ServerName + '.' + DB_NAME() + ' ' + ISNULL(OBJECT_NAME(@@PROCID),'') + ' - Started.'
		EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText;
	

		DECLARE csrPallet CURSOR SCROLL FOR	
			SELECT [CUSTOMER_ID]
					,[TRANSFERED]
					,[TRANSFERED_TIMESTAMP]
					,[ACTIVITY]
					,[PALLET_ID]
					,[CREATE_TIMESTAMP]
					,[RECEIVING_STATION]
					,[CUSTOMER_CODE]
					,[AMOUNT]
					,[ORDER_NAME]
					,[ORDER_TYP]
					,[START_TIMESTAMP]
					,ORDER_COMPLETE
					,ShopOrder
					,PalletID_PXPAL
					,QtyOnPallet
					,TransactionSeq
					,CreationDate
					,CreationTime
					,CreatedBy
					,MaintenanceDate
					,MaintenanceTime
					,MaintainedBy
					,Facility
				FROM tblProbatPallet
				WHERE TRANSFERED = '1' 
				ORDER BY CUSTOMER_ID;

		OPEN csrPallet;

		FETCH NEXT FROM csrPallet INTO  
			@intCUSTOMER_ID
			,@intTRANSFERED
			,@dteTRANSFERED_TIMESTAMP
			,@chrACTIVITY
			,@vchPALLET_ID
			,@dteCREATE_TIMESTAMP
			,@vchRECEIVING_STATION
			,@vchCUSTOMER_CODE
			,@intAMOUNT
			,@vchORDER_NAME
			,@intORDER_TYP
			,@dteSTART_TIMESTAMP
			,@chrORDER_COMPLETE
			,@intShopOrder
			,@vchPalletID_PXPAL
			,@decQtyOnPallet
			,@intTransactionSeq
			,@intCreationDate
			,@intCreationTime
			,@vchCreatedBy
			,@intMaintenanceDate
			,@intMaintenanceTime
			,@vchMaintainedBy
			,@vchFacility
			;
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SELECT @vchPalletIDs = @vchPalletIDs + ' ' + @vchPALLET_ID, @intCount = @intCount + 1;

			FETCH NEXT FROM csrPallet INTO  
				@intCUSTOMER_ID
				,@intTRANSFERED
				,@dteTRANSFERED_TIMESTAMP
				,@chrACTIVITY
				,@vchPALLET_ID
				,@dteCREATE_TIMESTAMP
				,@vchRECEIVING_STATION
				,@vchCUSTOMER_CODE
				,@intAMOUNT
				,@vchORDER_NAME
				,@intORDER_TYP
				,@dteSTART_TIMESTAMP
				,@chrORDER_COMPLETE
				,@intShopOrder
				,@vchPalletID_PXPAL
				,@decQtyOnPallet
				,@intTransactionSeq
				,@intCreationDate
				,@intCreationTime
				,@vchCreatedBy
				,@intMaintenanceDate
				,@intMaintenanceTime
				,@vchMaintainedBy
				,@vchFacility
				;
		END

		SET  @vchText ='Probat Pallet(s) have Been Posted to ERP: ' + CASE WHEN @vchPalletIDs = '' THEN 'Nothing.' ELSE LTRIM(@vchPalletIDs) END;
		EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText;
	
		FETCH FIRST FROM csrPallet INTO  
			@intCUSTOMER_ID
			,@intTRANSFERED
			,@dteTRANSFERED_TIMESTAMP
			,@chrACTIVITY
			,@vchPALLET_ID
			,@dteCREATE_TIMESTAMP
			,@vchRECEIVING_STATION
			,@vchCUSTOMER_CODE
			,@intAMOUNT
			,@vchORDER_NAME
			,@intORDER_TYP
			,@dteSTART_TIMESTAMP
			,@chrORDER_COMPLETE
			,@intShopOrder
			,@vchPalletID_PXPAL
			,@decQtyOnPallet
			,@intTransactionSeq
			,@intCreationDate
			,@intCreationTime
			,@vchCreatedBy
			,@intMaintenanceDate
			,@intMaintenanceTime
			,@vchMaintainedBy
			,@vchFacility
			;

		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS (SELECT 1 from tblProbatPalletHst WHERE PALLET_ID = @vchPallet_ID) 
			BEGIN
				SET @vchText = 'Pallet for Probat has been Posted to ERP: ' + 
						CASE WHEN @vchPALLET_ID = '' THEN 'Nothing.' ELSE @vchPALLET_ID END;	
				EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText;	
				SELECT  @intCountPosted = @intCountPosted + 1; 

				BEGIN TRANSACTION

					INSERT INTO tblProbatPalletHst
							([CUSTOMER_ID]
							,[TRANSFERED]
							,[TRANSFERED_TIMESTAMP]
							,[ACTIVITY]
							,[PALLET_ID]
							,[CREATE_TIMESTAMP]
							,[RECEIVING_STATION]
							,[CUSTOMER_CODE]
							,[AMOUNT]
							,[ORDER_NAME]
							,[ORDER_TYP]
							,[START_TIMESTAMP]
							,ORDER_COMPLETE
							,ShopOrder
							,PalletID_PXPAL
							,QtyOnPallet
							,TransactionSeq
							,CreationDate
							,CreationTime
							,CreatedBy
							,MaintenanceDate
							,MaintenanceTime
							,MaintainedBy
							,Facility
							)
					VALUES ( @intCUSTOMER_ID 
							,@intTRANSFERED
							,@dteTRANSFERED_TIMESTAMP
							,@chrACTIVITY
							,@vchPALLET_ID
							,@dteCREATE_TIMESTAMP
							,@vchRECEIVING_STATION
							,@vchCUSTOMER_CODE
							,@intAMOUNT
							,@vchORDER_NAME
							,@intORDER_TYP
							,@dteSTART_TIMESTAMP
							,@chrORDER_COMPLETE
							,@intShopOrder
							,@vchPalletID_PXPAL
							,@decQtyOnPallet
							,@intTransactionSeq
							,@intCreationDate
							,@intCreationTime
							,@vchCreatedBy
							,@intMaintenanceDate
							,@intMaintenanceTime
							,@vchMaintainedBy
							,@vchFacility
							);


					SET @vchText = 'Pallet for Probat has been archived to PP: ' + 
							CASE WHEN @vchPALLET_ID = '' THEN 'Nothing.' ELSE @vchPALLET_ID END;	
					EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText;	 

					DELETE FROM tblProbatPallet WHERE CURRENT OF csrPallet;
					
				COMMIT TRANSACTION
			END
			ELSE
				DELETE FROM tblProbatPallet WHERE CURRENT OF csrPallet;

			FETCH NEXT FROM csrPallet INTO 
				 @intCUSTOMER_ID
				,@intTRANSFERED
				,@dteTRANSFERED_TIMESTAMP
				,@chrACTIVITY
				,@vchPALLET_ID
				,@dteCREATE_TIMESTAMP
				,@vchRECEIVING_STATION
				,@vchCUSTOMER_CODE
				,@intAMOUNT
				,@vchORDER_NAME
				,@intORDER_TYP
				,@dteSTART_TIMESTAMP
				,@chrORDER_COMPLETE
				,@intShopOrder
				,@vchPalletID_PXPAL
				,@decQtyOnPallet
				,@intTransactionSeq
				,@intCreationDate
				,@intCreationTime
				,@vchCreatedBy
				,@intMaintenanceDate
				,@intMaintenanceTime
				,@vchMaintainedBy
				,@vchFacility
				;
		END
	
		CLOSE csrPallet;
		DEALLOCATE csrPallet;

		IF @intCountPosted - @intCount = 0
			SET @vchText = 'All pallets for Probat have been archived successfully.'	
		ELSE
			SET @vchText = Cast((@intCountPosted - @intCount) as varchar(10)) + ' pallets for Probat did not archived. $$$$'

		EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText 

	END TRY
	BEGIN CATCH
		IF (XACT_STATE()) = -1
			ROLLBACK TRANSACTION;

		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure nvarchar(200);
		DECLARE @ErrorLine int;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorMessage NVARCHAR(4000);
		
		DECLARE @nvchSubject nvarchar(256);
		DECLARE @nvchBody nvarchar(MAX);

		SET @ErrorMessage = @ErrorMessage + 'Error on archaving ' + @vchPallet_ID + ': '
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

