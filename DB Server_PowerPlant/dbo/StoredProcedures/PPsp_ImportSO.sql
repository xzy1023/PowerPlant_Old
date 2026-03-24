-- =============================================
-- Author:		Bong Lee
-- Create date: Nov 1, 2006
-- Description:	Copy Shop Order data from staging area to production area
-- WO#871:		Mar 7, 2014		Bong Lee
-- Description:	Add 5 columns in the Shop Order table for Probat DMS upgrade
-- WO#1297      Aug. 19, 2014   Bong Lee
-- Description: Enhance error handling.
-- WO#3982:		Mar. 9, 2017	Bong Lee
-- Description:	Validate downloaded shop orders			
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ImportSO]
	@chrFacility as char(3),
	@vchImportData as varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
--WO#871	DECLARE @vchSQLStmt varchar(1000)
	DECLARE @vchSQLStmt varchar(1800);			--WO#871
-- WO#1297	DECLARE @ErrorMessage NVARCHAR(4000);
-- WO#1297	DECLARE @ErrorSeverity INT;
-- WO#1297	DECLARE @ErrorState INT;
	SET NOCOUNT ON;
	BEGIN TRANSACTION [Tran1]		-- WO#1297
	BEGIN TRY
	--SET @chrFacility = (SELECT value1 FROM tblControl WHERE ([Key] = 'Facility'))

		-- Update record if value(s) are changed
		SET @vchSQLStmt = 	
		'Update dbo.tblShopOrder ' + 
		'SET ItemNumber = T2.ItemNumber,' +
		'PackagingLine = T2.PackagingLine, ' +
		'StartDate = T2.StartDate, ' +
		'StartTime = T2.StartTime, ' +
		'EndDate = T2.EndDate, ' +
		'EndTime = T2.EndTime, ' +
		'OrderQty = T2.OrderQty, ' +
		'FinishedQty = T2.FinishedQty, ' +
--WO#871	'Closed = T2.Closed, ' +
		'LotID = T2.LotID, ' +
		'PkgRate = T2.PkgRate ' +
		--WO#871 ADD Start
		',ProbatOrderName = T2.ProbatOrderName ' +
		',FlavoredCoffee = T2.FlavoredCoffee ' +
		',OrderType = T2.OrderType ' +
		',ComponentItem = T2.ComponentItem ' +
		--WO#871 ADD Stop
 		'FROM tblShopOrder AS T1 ' +
		'INNER JOIN ' + @vchImportData + '.dbo.tblShopOrder AS T2 ' +
		'ON T1.ShopOrder = T2.ShopOrder ' +
		'WHERE T1.Facility = ''' + @chrFacility + ''' AND ' +
		  '(T1.ItemNumber <> T2.ItemNumber ' +
		'OR T1.PackagingLine <> T2.PackagingLine ' +
		'OR T1.StartDate <> T2.StartDate ' +
		'OR T1.StartTime <> T2.StartTime ' +
		'OR T1.EndDate <> T2.EndDate ' +
		'OR T1.EndTime <> T2.EndTime ' +
		'OR T1.OrderQty <> T2.OrderQty ' +
		'OR T1.FinishedQty <> T2.FinishedQty ' +
--WO#871	'OR ISNULL(T1.Closed,'''') <> ISNULL(T2.Closed,'''') ' +
		'OR T1.LotID <> T2.LotID ' +
--WO#871	'OR T1.PkgRate <> T2.PkgRate)' 
		--WO#871 ADD Start 
		'OR T1.PkgRate <> T2.PkgRate ' +
		'OR T1.ProbatOrderName <> T2.ProbatOrderName ' +
		'OR T1.FlavoredCoffee <> T2.FlavoredCoffee ' +
		'OR T1.OrderType <> T2.OrderType ' +
		'OR T1.ComponentItem <> T2.ComponentItem' +
		')' 
		--WO#871 ADD Stop

	Print @vchSQLStmt
		execute (@vchSQLStmt)
Print 'Updated - ' + cast(@@Rowcount as varchar(10))

		--Add New Records
		SET @vchSQLStmt = 
		'INSERT INTO tblShopOrder ' +
		'SELECT T1.* FROM ' + @vchImportData + '.dbo.tblShopOrder As T1  ' +
		'LEFT OUTER JOIN  tblShopOrder AS T2 ' +
		'ON T1.ShopOrder = T2.ShopOrder ' +
		'WHERE T1.Facility = ''' + @chrFacility + ''' AND T2.ShopOrder IS NULL' 
Print @vchSQLStmt
		execute (@vchSQLStmt)
Print 'Inserted - ' + cast(@@Rowcount as varchar(10))

		--Delete Records if record is not found in ImportData
		SET @vchSQLStmt = 
		'DELETE tblShopOrder ' +
		'FROM tblShopOrder AS T1 ' +
		'LEFT OUTER JOIN ' + @vchImportData + '.dbo.tblShopOrder AS T2 ' +
		'ON T1.ShopOrder = T2.ShopOrder ' +
		'WHERE T1.Facility = ''' + @chrFacility + ''' AND T2.ShopOrder IS NULL' 
Print @vchSQLStmt
		execute (@vchSQLStmt)
Print 'Deleted - ' + cast(@@Rowcount as varchar(10))

		COMMIT TRANSACTION [Tran1]	-- WO#1297	
	-- Save the Weight Specifications by Shop order
	EXEC PPsp_ShopOrderWeightSpec_Upd @chrFacility, @vchImportData, 99999999
	

	-- Save the Shop order information
	EXEC PPsp_ArchiveShopOrder @chrFacility, @vchImportData	

	-- Validate downloaded shop orders
	EXEC PPsp_ValidateDownloadedSO			-- WO#3982

	END TRY
	BEGIN CATCH
	/* WO#1297 DEL Start
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
	WO#1297 DEL Stop */
	-- WO#1297 ADD Start
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION [Tran1]

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
	-- WO#1297 ADD Stop
	END CATCH
END

GO

