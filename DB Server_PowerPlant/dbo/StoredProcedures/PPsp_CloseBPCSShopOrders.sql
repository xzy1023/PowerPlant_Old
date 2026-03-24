


-- =============================================
-- Author:		Bong Lee
-- Create date: Feb. 25, 2011
-- Description:	Close BPCS Shop Orders
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_CloseBPCSShopOrders]
	@vchToBeClosedShopOrders as varchar(1000) OUTPUT,
	@vchClosedShopOrders as varchar(1000) OUTPUT
AS
BEGIN

	DECLARE @vchProcEnv as varchar(50)
	DECLARE @vchiSeriesServerName as varchar(50)
	DECLARE @vchUserLib varchar(10)
	DECLARE @vchOriginalLib varchar(10)
	DECLARE @nvchSQLStmt nvarchar(1000)

	DECLARE @intRRN int
    DECLARE @chrFacility char(3)
	DECLARE @intShopOrder int
	DECLARE @dteBPCSClosingTime as datetime

	SET NOCOUNT ON;
	BEGIN TRY

		SELECT @vchProcEnv = Rtrim(Substring(DB_NAME(),Charindex('_',DB_NAME())+1,10))

		IF @vchProcEnv = 'PRD'
		BEGIN
			Select @vchiSeriesServerName = value1 FROM tblControl WHERE [key] = 'iSeriesNames'
			Select @vchUserLib = value1, @vchOriginalLib = value2 FROM tblControl WHERE [key] = 'BPCSDataLibPRD'
		END
		ELSE
		BEGIN
			IF @vchProcEnv = 'UA'
			BEGIN
				Select @vchiSeriesServerName = value2 From tblControl Where [key] = 'iSeriesNames'
				Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibUA'
			END
			ELSE
			BEGIN
				Select @vchiSeriesServerName = value2 From tblControl Where [key] = 'iSeriesNames'
				Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibDev'
			END
		END 
		
		Select @vchUserLib = Substring(@vchUserLib,1,Len(@vchUserLib)-1) + 'R'

		IF object_id('tempdb..#tblClosingStatus') is not null
			DROP TABLE #tblClosingStatus

		CREATE TABLE #tblClosingStatus(
			--ShopOrder int,
			Status char(2),
			DateLastChanged int,
			TimeLastChanged int
			) ON [PRIMARY]


		SELECT @vchToBeClosedShopOrders = '', @vchClosedShopOrders = ''

		-- Select unposted closed shop order for which all its related pallet has been posted to BPCS
		DECLARE csrClosedSOHst CURSOR FAST_FORWARD FOR
			SELECT tCSOH.RRN, tCSOH.Facility, tCSOH.ShopOrder FROM tblToBeClosedShopOrder tCSOH
				LEFT OUTER JOIN (SELECT Facility, ShopOrder FROM tblPallet GROUP BY Facility, ShopOrder) tPLT
				ON tCSOH.Facility = tPLT.Facility and  tCSOH.ShopOrder =  tPLT.ShopOrder
			WHERE tCSOH.UpdatedToBPCS = '0' and tPLT.ShopOrder is NULL
			ORDER BY tCSOH.Facility, tCSOH.ShopOrder, tCSOH.CreationTime

		OPEN csrClosedSOHst

		-- read a record from the tblClosedShopOrder and load the data to the variables
		FETCH NEXT FROM csrClosedSOHst INTO @intRRN, @chrFacility, @intShopOrder

		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @vchToBeClosedShopOrders = @vchToBeClosedShopOrders + RTRIM(cast(@intShopOrder as varchar(10))) + ','

			/* Call BPCS stored procedure to close shop order, the return record set will show the update status */
			SET @nvchSQLStmt =  'EXEC(''CALL ' + @vchUserLib + '.SFC$665S(' + Cast(@intShopOrder as varchar(10)) + ')'') at ' + @vchiSeriesServerName

			INSERT INTO #tblClosingStatus
				EXEC sp_ExecuteSQL @nvchSQLStmt

			/* If the shop order is closed in BPCS, update the information in closed shop order table history */
			IF EXISTS (SELECT * FROM #tblClosingStatus WHERE Status = 'SZ')
			BEGIN
				SELECT @dteBPCSClosingTime = Stuff(Stuff(DateLastChanged,5,0,'-'),8,0,'-') + ' ' + dbo.fnCvtNumTimeToDateTime(TimeLastChanged) FROM #tblClosingStatus
				BEGIN TRANSACTION Trn1	
		
					UPDATE tblToBeClosedShopOrder SET UpdatedToBPCS = '1', BPCSClosingTime = @dteBPCSClosingTime, LastUpdated = getdate() WHERE RRN = @intRRN
					INSERT INTO tblClosedShopOrderHst
						(Facility, ShopOrder, DefaultPkgLine, Operator, SessionStartTime, ClosingTime, UpdatedToBPCS, BPCSClosingTime, LastUpdated, CreationTime)
						SELECT Facility, ShopOrder, DefaultPkgLine, Operator, SessionStartTime, ClosingTime, UpdatedToBPCS, BPCSClosingTime, LastUpdated, CreationTime
						FROM         tblToBeClosedShopOrder
						WHERE     (RRN = @intRRN)
					DELETE FROM tblToBeClosedShopOrder WHERE RRN = @intRRN
					SET @vchClosedShopOrders = @vchClosedShopOrders + RTRIM(cast(@intShopOrder as varchar(10))) + ','
				COMMIT TRANSACTION Trn1
			END

			FETCH NEXT FROM csrClosedSOHst INTO @intRRN, @chrFacility, @intShopOrder
		END


		CLOSE csrClosedSOHst
		DEALLOCATE csrClosedSOHst

		If object_id('tempdb..#tblClosingStatus') is not null
		DROP TABLE #tblClosingStatus

		-- if the to be closed shop order list is not blank, take out the last comma
		IF @vchToBeClosedShopOrders <> ''
		BEGIN
			SET @vchToBeClosedShopOrders = LEFT(@vchToBeClosedShopOrders, Len(@vchToBeClosedShopOrders)-1)
		END

		-- if the closed shop order list is not blank, take out the last comma
		IF @vchClosedShopOrders <> ''
		BEGIN
			SET @vchClosedShopOrders = LEFT(@vchClosedShopOrders,Len(@vchClosedShopOrders)-1)
		END

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		BEGIN TRY
			If object_id('tempdb..#tblClosingStatus') is not null
			DROP TABLE #tblClosingStatus
		END TRY
		BEGIN CATCH
		END CATCH

		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();

			-- Use RAISERROR inside the CATCH block to return error
			-- information about the original error that caused
			-- execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, -- Message text.
					   @ErrorSeverity, -- Severity.
					   @ErrorState -- State.
					   );
	END CATCH;

END

GO

