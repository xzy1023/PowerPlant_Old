
CREATE PROCEDURE [dbo].[PPsp_ShopOrderClosingDate] 
	@vchFacility varchar(3)
	,@intFmShopOrder int
	,@intToShopOrder int
AS
BEGIN
	DECLARE @vchSQLStmt as nvarchar(3000);
	DECLARE @vchERPSQLStmt as nvarchar(1000);
	DECLARE @vchShopOrderRange as nvarchar(200);
	DECLARE @vchFmShopOrder as nvarchar(10);
	DECLARE @vchToShopOrder as nvarchar(10);
	DECLARE @vchCompanyNo nvarchar(10)
	DECLARE @vchServerName as nvarchar(50);
	DECLARE @vchDBName nvarchar(50);
	DECLARE @cstNULLDate as nvarchar(25);

	SET NOCOUNT ON;
	BEGIN TRY

		SELECT @vchCompanyNo = Value2 FROM tblControl WHERE [Key] = 'Facility' AND SubKey = 'General'
		SELECT @vchServerName = Value1, @vchDBName = Value2 FROM tblControl WHERE [Key] = 'ERPEnv' AND SubKey = 'General'
		SET @cstNULLDate = '1900-01-01 00:00:00.000'

		SELECT @vchERPSQLStmt = 'SELECT ProdID, CASE WHEN FinishedDate = ''''' + @cstNULLDate + ''''' THEN NULL ELSE FinishedDate END AS FinishedDate ' + 
					' FROM ' + @vchDBName + '.dbo.PRODTABLE tP ' + 
					'LEFT OUTER JOIN ' + @vchDBName + '.dbo.InventDim tID ' +
					'ON tP.InventDimId = tID.InventDimId and tP.DataareaID = tID.DataareaID ' +
					'WHERE tID.InventSiteID = ''''' + @vchFacility + '''''';

		SELECT  @vchFmShopOrder = Cast(@intFmShopOrder as nvarchar(10)),  @vchToShopOrder = Cast(@intToShopOrder as nvarchar(10))

		IF @intFmShopOrder <> 0 
		    SELECT @vchERPSQLStmt = @vchERPSQLStmt  + ' AND ProdID BETWEEN ' + @vchFmShopOrder + ' AND ' + @vchToShopOrder
					,@vchShopOrderRange = ' AND ShopOrder BETWEEN ' + @vchFmShopOrder + ' AND ' + @vchToShopOrder
		ELSE
			SELECT @vchShopOrderRange = ' '

--print @vchERPSQLStmt

		SELECT @vchSQLStmt = 'SELECT ''' +  @vchFacility + ''' as Facility,  COALESCE (ProdID, tPPSOCD.ShopOrder) as ShopOrder , ' + 
				'COALESCE (tERPSOCD.ERPClosingTime, tPPSOCD.ERPClosingTime) as ERPClosingTime ' +
				'FROM (' +
				'SELECT ProdID, FinishedDate as ERPClosingTime FROM OPENQUERY([' + @vchServerName + '],''' + @vchERPSQLStmt + ''')) tERPSOCD ' +
							'FULL JOIN ' +
							'(SELECT ShopOrder, Max(BPCSClosingTime) AS ERPClosingTime ' +
							'FROM (SELECT ShopOrder, BPCSClosingTime ' +
										'FROM dbo.tblClosedShopOrderHst ' +
										'WHERE Facility = ''' + @vchFacility + '''' + @vchShopOrderRange +
									' UNION '+
									'SELECT ShopOrder, ClosingTime as ERPClosingTime ' +
										'FROM dbo.tblToBeClosedShopOrder ' +
										'WHERE Facility = ''' + @vchFacility + '''' + @vchShopOrderRange + ') AS T1 ' +
							 'Group by ShopOrder) tPPSOCD ' +
							 'ON tERPSOCD.ProdID =  tPPSOCD.ShopOrder'


--print @vchSQLStmt
		EXEC sp_ExecuteSQL @vchSQLStmt

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
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ' Message: ' + CHAR(13) + ERROR_MESSAGE();

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)
	END CATCH
END

GO

