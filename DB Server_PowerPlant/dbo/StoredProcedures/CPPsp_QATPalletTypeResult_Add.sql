

-- =============================================
-- Author:		Bong Lee
-- Create date: Aug 28, 2018
-- Description:	WO#17432 Insert record to the QAT Pallet Type result table
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_QATPalletTypeResult_Add] 	

	@dteBatchID					datetime
	,@vchFacility				varchar(3)
	,@vchInterfaceID			varchar(24)
	,@vchPackagingLine			varchar(10)
	,@chrPalletCode				char(1)
	,@intShopOrder				int
	,@dteSOStartTime			datetime
	,@dteTestEndTime			datetime
	,@dteTestStartTime			datetime
	,@vchTesterID				varchar(10)		= NULL
	,@chrQATEntryPoint			char(1)			= NULL
AS
BEGIN
	BEGIN TRY

		INSERT INTO tblQATPalletTypeResult
			(
				BatchID
				,Facility
				,InterfaceID
				,PackagingLine
				,PalletCode
				,ShopOrder
				,SOStartTime
				,TestEndTime
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
				,@chrPalletCode
				,@intShopOrder
				,@dteSOStartTime
				,@dteTestEndTime
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

