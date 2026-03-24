
-- =============================================
-- Author:		Bong Lee
-- Create date: Aug. 5, 2007
-- Description:	Save Shop Order data from staging area to production area for give retention days
-- WO#871:		Mar 7, 2014		Bong Lee
-- Description:	Add 5 columns in the Shop Order table for Probat DMS upgrade
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ArchiveShopOrder]
	@chrFacility as char(3),
	@vchImportData as varchar(50),
	@intRetentionDays as tinyInt = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
--WO#871	DECLARE @vchSQLStmt varchar(1000)
	DECLARE @vchSQLStmt varchar(1800);			--WO#871
	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
	SET NOCOUNT ON;
	BEGIN TRY

	-- Update record if value(s) are changed
		SET @vchSQLStmt = 	
		'Update dbo.tblShopOrderHst ' + 
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
		'PkgRate = T2.PkgRate, ' +
		'LastUpdated = Getdate() ' +
		--WO#871 ADD Start
		',ProbatOrderName = T2.ProbatOrderName ' +
		',FlavoredCoffee = T2.FlavoredCoffee ' +
		',OrderType = T2.OrderType ' +
		',ComponentItem = T2.ComponentItem ' +
		--WO#871 ADD Stop
 		'FROM tblShopOrderHst AS T1 ' +
		'INNER JOIN ' + @vchImportData + '.dbo.tblShopOrder AS T2 ' +
		'ON T1.Facility = T2.Facility and T1.ShopOrder = T2.ShopOrder ' +
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
		'INSERT INTO tblShopOrderHst ' +
--WO#871	'SELECT T1.*, getdate(), getdate() FROM ' + @vchImportData + '.dbo.tblShopOrder As T1  ' +
		'SELECT getdate(), getdate(), T1.* FROM ' + @vchImportData + '.dbo.tblShopOrder As T1  ' +		--WO#871
		'LEFT OUTER JOIN  tblShopOrderHst AS T2 ' +
		'ON T1.Facility = T2.Facility and T1.ShopOrder = T2.ShopOrder ' +
		'WHERE T1.Facility = ''' + @chrFacility + ''' AND T2.ShopOrder IS NULL' 
Print @vchSQLStmt
		execute (@vchSQLStmt)
Print 'Inserted - ' + cast(@@Rowcount as varchar(10))

		--Delete Records if Start Date of the record is greater than the retention days
		IF @intRetentionDays is not NULL
			DELETE tblShopOrderHst FROM tblShopOrderHst
			WHERE Facility =  @chrFacility 
				AND DateDiff(d,STUFF(STUFF(CAST(StartDate AS CHAR(8)),5,0,'-'),8,0,'-'),Getdate()) > @intRetentionDays
Print 'Deleted - ' + cast(@@Rowcount as varchar(10))

	END TRY
	BEGIN CATCH
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
	END CATCH
END

GO

