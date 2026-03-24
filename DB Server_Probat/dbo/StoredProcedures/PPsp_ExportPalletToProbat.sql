
-- =============================================
-- WO#871	    Jun. 5, 2015   Bong Lee
-- Description:	Export pallet records to Power Plant/Probat interface server
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ExportPalletToProbat]

AS
BEGIN

	DECLARE @vchServerName as varchar(50);
	DECLARE @vchSQLInstance as varchar(50);
	DECLARE @vchDBName as varchar(50);
	DECLARE @bitConnected as bit;
	DECLARE @vchSQLStmt as nvarchar(500);
	DECLARE @ParmDefinition as nvarchar(500);


	DECLARE @intCUSTOMER_ID as int
			,@intTRANSFERED as int
			,@dteTRANSFERED_TIMESTAMP as datetime
			,@chrACTIVITY as char(1)
			,@vchPALLET_ID as varchar(20)
			,@dteCREATE_TIMESTAMP as datetime
			,@vchRECEIVING_STATION as varchar(20)
			,@vchCUSTOMER_CODE as varchar(20)
			,@intAMOUNT as int
			,@vchORDER_NAME as varchar(20)
			,@intORDER_TYP as int
			,@dteSTART_TIMESTAMP as datetime
			,@chrORDER_COMPLETE char(1)

	SET @ParmDefinition = N'@CUSTOMER_ID int ' +
			N',@TRANSFERED int ' +
			N',@TRANSFERED_TIMESTAMP datetime ' +
			N',@ACTIVITY char(1) ' +
			N',@PALLET_ID varchar(20) ' +
			N',@CREATE_TIMESTAMP datetime ' +
			N',@RECEIVING_STATION varchar(20) ' +
			N',@CUSTOMER_CODE varchar(20) ' +
			N',@AMOUNT int ' +
			N',@ORDER_NAME varchar(20) ' +
			N',@ORDER_TYP int ' +
			N',@START_TIMESTAMP datetime ' +
			N',@ORDER_COMPLETE char(1)';

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
			DECLARE csrPallet CURSOR SCROLL FOR
			SELECT  CUSTOMER_ID, TRANSFERED, TRANSFERED_TIMESTAMP, ACTIVITY, PALLET_ID, CREATE_TIMESTAMP
					,RECEIVING_STATION, CUSTOMER_CODE, AMOUNT, ORDER_NAME, ORDER_TYP, START_TIMESTAMP, ORDER_COMPLETE
			FROM    PRO_IMP_PALLET
			WHERE   TRANSFERED = 0 

			OPEN csrPallet

			FETCH NEXT FROM csrPallet INTO 
				@intCUSTOMER_ID, @intTRANSFERED, @dteTRANSFERED_TIMESTAMP, @chrACTIVITY, @vchPALLET_ID
				,@dteCREATE_TIMESTAMP, @vchRECEIVING_STATION, @vchCUSTOMER_CODE, @intAMOUNT, @vchORDER_NAME
				,@intORDER_TYP, @dteSTART_TIMESTAMP, @chrORDER_COMPLETE

			WHILE @@FETCH_STATUS = 0
			BEGIN

				SELECT  @vchSQLStmt = 'INSERT INTO [' + @vchSQLInstance + '].' + @vchDBName + '.[dbo].[PRO_IMP_PALLET] ' +
				'VALUES(@CUSTOMER_ID, @TRANSFERED, @TRANSFERED_TIMESTAMP, @ACTIVITY, @PALLET_ID ' +
					  ',@CREATE_TIMESTAMP, @RECEIVING_STATION, @CUSTOMER_CODE, @AMOUNT, @ORDER_NAME ' +
					  ',@ORDER_TYP, @START_TIMESTAMP, @ORDER_COMPLETE)'
Print @vchSQLStmt
				EXECUTE sp_executesql @vchSQLStmt, @ParmDefinition, 
						@CUSTOMER_ID = @intCUSTOMER_ID
						,@TRANSFERED = @intTRANSFERED
						,@TRANSFERED_TIMESTAMP = @dteTRANSFERED_TIMESTAMP
						,@ACTIVITY = @chrACTIVITY
						,@PALLET_ID = @vchPALLET_ID
						,@CREATE_TIMESTAMP = @dteCREATE_TIMESTAMP
						,@RECEIVING_STATION = @vchRECEIVING_STATION
						,@CUSTOMER_CODE = @vchCUSTOMER_CODE
						,@AMOUNT = @intAMOUNT
						,@ORDER_NAME = @vchORDER_NAME
						,@ORDER_TYP = @intORDER_TYP
						,@START_TIMESTAMP = @dteSTART_TIMESTAMP
						,@ORDER_COMPLETE = @chrORDER_COMPLETE

				UPDATE [dbo].[PRO_IMP_PALLET] SET TRANSFERED = 1, [TRANSFERED_TIMESTAMP] = getdate() 
				WHERE [CUSTOMER_ID] = @intCUSTOMER_ID

				FETCH NEXT FROM csrPallet INTO 
					@intCUSTOMER_ID, @intTRANSFERED, @dteTRANSFERED_TIMESTAMP, @chrACTIVITY, @vchPALLET_ID
					,@dteCREATE_TIMESTAMP, @vchRECEIVING_STATION, @vchCUSTOMER_CODE, @intAMOUNT, @vchORDER_NAME
					,@intORDER_TYP, @dteSTART_TIMESTAMP, @chrORDER_COMPLETE

			END
		  END
	CLOSE csrPallet
	DEALLOCATE csrPallet

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

