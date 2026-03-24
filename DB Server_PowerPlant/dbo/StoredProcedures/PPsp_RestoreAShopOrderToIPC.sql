
-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 19 2014
-- Description:	Restore a shop order To an IPC
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RestoreAShopOrderToIPC]
	-- Add the parameters for the stored procedure here
	@vchIPCName nvarchar(10),
	@intShopOrder int
AS
BEGIN
	DECLARE @vchSQLStmt as nvarchar(1000);
	DECLARE @vchServer as nvarchar(100);
	DECLARE @intRetVal int;
	DECLARE @vchShopOrder as nvarchar(10);
	SET NOCOUNT ON;
	BEGIN TRY
		-- Check local Power Plant
		--Set @vchIPCName = N'MPHOIPCV01';
		--Set @intShopOrder = 1020056
		SET @vchShopOrder = Cast(@intShopOrder as nvarchar(10));
		Set @vchServer = N'[' + @vchIPCName + N'\SQLEXPRESS]';
		Select @intRetVal = 0;
		SET @vchSQLStmt = N'select @intRetVal = 1 from ' + @vchServer + N'.localpowerplant.dbo.tblshoporder where shoporder = ' + @vchShopOrder;

		print @vchSQLStmt
		EXEC sp_executesql @vchSQLStmt, N'@intRetVal int output', @intRetVal output

		IF @intRetVal = 0	-- Not found in Local Power Plant
		-- Check Import Data
		BEGIN
			SET @vchSQLStmt = N'select @intRetVal = 1 from ' + @vchServer + N'.Importdata.dbo.tblshoporder where shoporder = ' + @vchShopOrder;

			print @vchSQLStmt
			EXEC sp_executesql @vchSQLStmt, N'@intRetVal int output', @intRetVal output

			IF @intRetVal = 1 -- found in Import Data
			BEGIN
				SET @vchSQLStmt = N'Insert into ' + @vchServer + N'.localpowerplant.dbo.tblShoporder SELECT * from ' + @vchServer + N'.ImportData.dbo.tblshoporder where shoporder = ' + @vchShopOrder;
				EXEC sp_executesql @vchSQLStmt
				Print 'Shop order has been added from IPC Staging DB.'
			END
			ELSE
				BEGIN 	-- Check server shop order file
					SET @vchSQLStmt = N'select @intRetVal = 1 from dbo.tblshoporder where shoporder = ' + @vchShopOrder;
					print @vchSQLStmt
					EXEC sp_executesql @vchSQLStmt, N'@intRetVal int output', @intRetVal output

					IF @intRetVal = 1 -- found on server
					BEGIN
						SET @vchSQLStmt = N'Insert into ' + @vchServer + N'.localpowerplant.dbo.tblShoporder SELECT * from dbo.tblshoporder where shoporder = ' + @vchShopOrder;
						EXEC sp_executesql @vchSQLStmt
						Print 'Shop order has been added from server DB.'
					END
					ELSE
						BEGIN	-- Check server shop order history file
							SET @vchSQLStmt = N'select @intRetVal = 1 from dbo.tblshoporderhst where shoporder = ' + @vchShopOrder;
							print @vchSQLStmt
							EXEC sp_executesql @vchSQLStmt, N'@intRetVal int output', @intRetVal output
							IF @intRetVal = 1 -- found in the history file on server
							BEGIN
								SET @vchSQLStmt = N'Insert into ' + @vchServer + N'.localpowerplant.dbo.tblShoporder SELECT ' +  
										N'[Facility],[ShopOrder],[ItemNumber],[PackagingLine],[StartDate],[StartTime],[EndDate],[EndTime], ' +
										N'[OrderQty],[FinishedQty],[Closed],[LotID],[PkgRate],[ProbatOrderName],[FlavoredCoffee],[OrderType],[ComponentItem] ' +
										N'From dbo.tblshoporderhst where shoporder = ' + @vchShopOrder;
								Print @vchSQLStmt
								EXEC sp_executesql @vchSQLStmt
								Print 'Shop order has been added from shop order history.'
							END
							ELSE
								Print 'The shop order is not found in Power Plant at all'
						END

				END
		END
		ELSE
		BEGIN
			Print 'The shop order is found in the IPC local data base. Was the shop order no. entered correctly?'
		END
	END TRY
	BEGIN CATCH
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure nvarchar(200);
		DECLARE @ErrorLine int;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorMessage NVARCHAR(4000);

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

