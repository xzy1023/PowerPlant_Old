-- =============================================
-- ALM#11828  : Apr 05, 2016	Bong Lee
-- Description:	Get the current active shop order of the given packaging line 
-- =============================================
CREATE PROCEDURE [PPsp_GetCurrentActiveShopOrder]
	@vchPackagingLine as varchar(50)
	,@bitChkDestinationIPCConnFirst bit
	,@intActiveShopOrder int OUTPUT
	,@bitConnected bit OUTPUT
AS
BEGIN
	DECLARE @vchDeviceName as varchar(50)
	DECLARE @nvchSQLStmt as nvarchar(1000)
	DECLARE @nvchParmDefination nvarchar(100)
	DECLARE @intShopOrder as int

	SET @intActiveShopOrder = 0
	SET @bitConnected = 0

	BEGIN TRY
		SELECT TOP 1 @vchDeviceName = [ComputerName] 
		FROM [tblComputerConfig]
		WHERE [RecordStatus] = 1 AND [PackagingLine] = @vchPackagingLine

		IF @vchDeviceName IS NOT NULL
		BEGIN
			IF @bitChkDestinationIPCConnFirst = 1 OR @bitChkDestinationIPCConnFirst = 0
				EXEC PPsp_IsDeviceConnected @vchDeviceName, @bitConnected OUTPUT

			IF @bitConnected  = 1 OR @bitChkDestinationIPCConnFirst = 0
			BEGIN
				BEGIN TRY
					SET  @nvchSQLStmt = N'SELECT @intShopOrder = ShopOrder FROM [' + @vchDeviceName + '\SQLEXPRESS].LocalPowerPlant.dbo.tblSessionControl'
					SET @nvchParmDefination = '@intShopOrder int OUTPUT'
					EXEC sp_ExecuteSQL @nvchSQLStmt, @nvchParmDefination, @intShopOrder=@intActiveShopOrder OUTPUT;
					SET @bitConnected = 1;
				END TRY
				BEGIN CATCH
					SET @bitConnected = 0
				END CATCH
			END
		END

		RETURN 
	END TRY
	BEGIN CATCH

		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure nvarchar(200);
		DECLARE @ErrorLine int;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorMessage NVARCHAR(4000);

		IF (XACT_STATE()) = -1
			ROLLBACK TRANSACTION 

		SELECT 
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ', Message: ' + CHAR(13) + ERROR_MESSAGE();

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)
	END CATCH
END

GO

