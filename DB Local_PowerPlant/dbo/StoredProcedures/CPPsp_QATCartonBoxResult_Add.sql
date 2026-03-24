

-- =============================================
-- Author:		Bong Lee
-- Create date: July 10, 2018
-- Description:	WO#17432 Insert record to the QAT Carton box visual check result table
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_QATCartonBoxResult_Add] 	

	@dteBatchID					datetime
	,@vchFacility				varchar(3)
	,@vchInterfaceID			varchar(24)
	,@intShopOrder				int
	,@dteSOStartTime			datetime
	,@vchPackagingLine			varchar(10)
	,@dteTestEndTime			datetime
	,@intTestResult				tinyint
	,@dteTestStartTime			datetime
	,@vchTesterID				varchar(10)
	,@chrQATEntryPoint			char(1)
AS
BEGIN
	BEGIN TRY

		INSERT INTO tblQATCartonboxResult
			(
				BatchID
				,Facility
				,InterfaceID
				,ShopOrder
				,SOStartTime
				,PackagingLine
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
				,@intShopOrder
				,@dteSOStartTime
				,@vchPackagingLine
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

