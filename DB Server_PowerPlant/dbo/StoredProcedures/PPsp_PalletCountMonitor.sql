
-- =============================================
-- WO#5370:		Jul. 13 2017	Bong Lee
-- Description: Unit Count Monitor.
-- WO#37864:	Nov. 23 2020	Bong Lee
-- Description: Handle Gima 590 line type.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_PalletCountMonitor]
	@vchFacility as varchar(3)
AS
BEGIN
	DECLARE @vchComputerName			varchar(50);
	DECLARE @vchPackagingLine			varchar(10);
	DECLARE	@intShopOrder				int;
	DECLARE @dteSOStartTime				Datetime;
	DECLARE @intUnitCount				int;
	DECLARE @vchSQLStmt					varchar(1000);
	DECLARE @vchTableName				varchar(100)
	DECLARE @intIdx_1					int;
	DECLARE @intIdx_2					int;
	DECLARE @int_MaxValue				int;

	DECLARE @tblProblemLines Table
	(	
		ComputerName varchar(50)
	)

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

	SET NOCOUNT ON;
	BEGIN TRY
		SELECT @intIdx_2 = 2147483647, @int_MaxValue= 2147483647;

		SELECT TOP 1 @vchComputerName= ComputerName
			,@vchPackagingLine = PackagingLine
			,@intShopOrder = ShopOrder
			,@dteSOStartTime = SOStartTime
			,@intIdx_1 = TxID 
			,@intUnitCount = UnitCount
			FROM tblUnitCountInbound 
-- WO#37864		WHERE ProcessingStatus = 0 AND Facility = @vchFacility AND OutputLocation <> 'RAF'
		WHERE ProcessingStatus = 0 AND Facility = @vchFacility AND (OutputLocation <> 'RAF' or PackagingLine like '3990%')		-- WO#37864
		ORDER BY [TxID]

		WHILE @intIdx_1 is not NULL AND @intIdx_1 <= @intIdx_2 AND @intIdx_1 <> @int_MaxValue
		BEGIN
			Begin Try
				IF NOT EXISTS(SELECT 1 FROM @tblProblemLines WHERE ComputerName = @vchComputerName)
				BEGIN
					SET @vchTableName =  '[' + @vchComputerName + N'\SQLEXPRESS].localpowerplant.dbo.tblSessionControl'
					SET @vchSQLStmt =
						'UPDATE ' + @vchTableName + ' SET CaseCounter = CaseCounter + ' + CAST(@intUnitCount as varchar(10)) +
							' WHERE ShopOrder = ' + CAST(@intShopOrder as varchar(12)) +
							' AND DefaultPkgLine = ''' + @vchPackagingLine + ''' ' +
							' AND ABS(datediff(s, starttime, ''' + CONVERT(varchar(23),@dteSOStartTime,121) + ''' )) < 1'
			
					PRINT @vchSQLStmt;
					EXEC (@vchSQLStmt)
					IF  @@ROWCOUNT > 0
					BEGIN
						UPDATE tblUnitCountInbound SET ProcessingStatus = 1 WHERE TxID = @intIdx_1 
					END
				END
			END TRY
			BEGIN CATCH
				INSERT INTO @tblProblemLines VALUES(@vchComputerName)
			END CATCH
		
			-- fetch next record that has not been processed to the variables
			SELECT TOP 1 @vchComputerName= ComputerName
				,@vchPackagingLine = PackagingLine
				,@intShopOrder = ShopOrder
				,@dteSOStartTime = SOStartTime
				,@intIdx_2 = TxID 
				,@intUnitCount = UnitCount
			FROM tblUnitCountInbound 
-- WO#37864				WHERE ProcessingStatus = 0 AND Facility = @vchFacility AND OutputLocation <> 'RAF' AND TxID > @intIdx_1
			WHERE ProcessingStatus = 0 
				AND Facility = @vchFacility 
				AND (OutputLocation <> 'RAF' or PackagingLine like '3990%')
				AND TxID > @intIdx_1											-- WO#37864	
			ORDER BY [TxID]

			IF  @intIdx_1 = @intIdx_2 or @intIdx_2 is null
			BEGIN
				SET @intIdx_1 = NULL
			END
			ELSE
			BEGIN
				SET @intIdx_1 = @intIdx_2
			END

		END

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
							N', Procedure ' + ISNULL(@ErrorProcedure,'') +
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

