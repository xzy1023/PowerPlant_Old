
-- =============================================
-- Author:		Bong Lee
-- Create date: Mar 14, 2014	WO#871
-- Description:	Upload Pallets Created for Probat to iSeries
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_UploadProbatPalletToISeries] 
		@vchISeriesLibrary varchar(10)
		,@vchISeriesName varchar(10)
		,@vchLogFileName varchar(255)		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

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
	
	SELECT @intCount = 0, @intCountPosted = 0, @vchPalletIDs = ''
	SELECT     @chrEnvironment = LEFT(Value2,1)
	FROM         tblControl
	WHERE     [Key] = N'CompanyName' AND SubKey = 'General';

	SET  @vchText = '*** Reading pallet for Probat.'; 
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

	SET  @vchText ='Probat Pallets To Be Posted: ' + CASE WHEN @vchPalletIDs = '' THEN 'Nothing.' ELSE LTRIM(@vchPalletIDs) END;
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
			BEGIN TRY

			Set @vchISeriesFile = 'PXPLIO$' + @chrEnvironment + RTRIM(@vchFacility); 

			SET @vchSQLStmt = 'INSERT INTO ' + @vchISeriesLibrary + '.' + @vchISeriesFile + 
							  --' (PIFAC, PIPAL, PIQTY, PIPROD, PIMACH, PIEMP, PIPDTE, PIEDTE, PICDTE, PICTIM, PICMP ,PILOT, PISORD)
							  '(PXTRNF, PXTRNFDT, PXACTC, PXPALID, PXCRTDT, PXMACH, PXPROD, PXQTY, PXORDNAM, PXORDTYP, ' +
							  	'PXSTRDT, PXCMPF, PXSORD, PXPAL, PXPQTY, PXTSEQ, PXCDTE, PXCTIM, PXCUSR, PXMDTE, PXMTIM, PXMUSR)
								VALUES(' + CAST(@intTRANSFERED as char(1)) +
								',''' + CONVERT(varchar(25), @dteTRANSFERED_TIMESTAMP, (121)) + 
								''',''' + @chrACTIVITY + 
								''',''' + @vchPALLET_ID + 
								''',''' + CONVERT(varchar(25),@dteCREATE_TIMESTAMP, (121)) + 
								''',''' +@vchRECEIVING_STATION + 
								''',''' +@vchCUSTOMER_CODE + 
								''',' + CAST(@intAMOUNT as varchar(10)) +
								',''' + @vchORDER_NAME + 
								''',' + CAST(@intORDER_TYP as varchar(3)) + 
								',''' + CONVERT(varchar(25),@dteSTART_TIMESTAMP, (120)) +
								''',''' + @chrORDER_COMPLETE +
								''',' + CAST(@intShopOrder as varchar(10)) + 
								',''' + @vchPalletID_PXPAL +
								''',' + CAST(@decQtyOnPallet as varchar(15)) + 
								',' + CAST(@intTransactionSeq as varchar(10)) + 
								',' + CAST(@intCreationDate as varchar(8)) + 
								',' + CAST(@intCreationTime as varchar(6)) +
								',''' + @vchCreatedBy + 
								''',' + CAST(@intMaintenanceDate as varchar(10)) + 
								',' + CAST(@intMaintenanceTime as varchar(10)) +
								',''' + @vchMaintainedBy + 
								''')' ;

Print @vchSQLStmt
				IF @vchSQLStmt IS NOT NULL		
				BEGIN							

					IF @vchISeriesName = 'S105HF5M'
						EXEC (@vchSQLStmt) at S105HF5M
					ELSE
						IF @vchISeriesName = 'S10A8379'
							EXEC (@vchSQLStmt) at S10A8379
						
					SET @vchText = 'Pallet for Probat has been Posted to BPCS: ' + 
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


						SET @vchText = 'Pallet for Probat has been saved to PP: ' + 
								CASE WHEN @vchPALLET_ID = '' THEN 'Nothing.' ELSE @vchPALLET_ID END;	
						EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText;	 

						DELETE FROM tblProbatPallet WHERE CURRENT OF csrPallet;
					
					COMMIT TRANSACTION

					IF @intCountPosted - @intCount = 0
						SET @vchText = 'All pallets for Probat have bee posted successfully.'	
					ELSE
						SET @vchText = Cast((@intCountPosted - @intCount) as varchar(10)) + ' pallets for Probat did not post. $$$$'

					EXEC PPsp_AppendToTextFile @vchLogFileName, @vchText 
				END		
			END TRY
			BEGIN CATCH
			IF (XACT_STATE()) = -1
				ROLLBACK TRANSACTION;
				PRINT ERROR_MESSAGE();
				SET @vchText = 'Error on posting ' + @vchPallet_ID + ': ' + ERROR_MESSAGE();		
				EXEC PPsp_AppendToTextFile @vchLogFileName,  @vchText;							
			END CATCH
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
END

GO

