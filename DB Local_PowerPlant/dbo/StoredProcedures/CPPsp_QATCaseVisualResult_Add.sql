

-- =============================================
-- Author:		Bong Lee
-- Create date: Aug 28, 2018
-- Description:	WO#17432 Insert record to the QAT case visual test result table
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_QATCaseVisualResult_Add] 	

	@dteBatchID					datetime
	,@vchFacility				varchar(3)
	,@vchInterfaceID			varchar(24)
	,@vchPackagingLine			varchar(10)
	,@intShopOrder				int
	,@dteSOStartTime			datetime
	,@dteTestEndTime			datetime
	,@intTestResult				tinyint
	,@dteTestStartTime			datetime
	,@vchTesterID				varchar(10)
	,@chrQATEntryPoint			char(1)
AS
BEGIN
	BEGIN TRY

		INSERT INTO tblQATCaseVisualResult
			(
				BatchID
				,Facility
				,InterfaceID
				,PackagingLine
				,ShopOrder
				,SOStartTime
				,TestEndTime
				,TestResult
				,TestStartTime
				,TesterID
				,QATEntryPoint
			)
			VALUES
			(
				@dteBatchID
				,@vchFacility
				,@vchInterfaceID
				,@vchPackagingLine
				,@intShopOrder
				,@dteSOStartTime
				,@dteTestEndTime
				,@intTestResult
				,@dteTestStartTime
				,@vchTesterID
				,@chrQATEntryPoint
			);

	END TRY
	BEGIN CATCH
			-- Use Throw inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			THROW
	END CATCH
END

GO

