
-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 06, 2018
-- Description:	WO#17432 Insert record to the QAT check weigher test result header and detail tables
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_QATDateCodeResult_Maint] 	

	@dteBatchID					datetime
	,@vchDateCodeValue			varchar(50)		= NULL
	,@intDateCodeType			int
	,@vchFacility				varchar(3)
	,@vchInterfaceID			varchar(24)		= NULL
	,@vchPackagingLine			varchar(10)
	,@intRetestNo				tinyint
	,@intShopOrder				int
	,@dteSOStartTime			datetime
	,@dteTestEndTime			datetime		= NULL
	,@intTestResult				tinyint			= NULL
	,@dteTestStartTime			datetime
	,@dteTestTime				datetime		= NULL
	,@vchTesterID				varchar(10)		= NULL
	,@chrQATEntryPoint			char(1)			= NULL
			
AS
BEGIN
	DECLARE		@bitIsHearderfound		bit		= 0

	BEGIN TRY

	BEGIN TRANSACTION

		-- Add the test detail result to the table, if it is not for update for override id
		IF @dteTestTime IS NOT NULL
		BEGIN
			INSERT INTO tblQATDateCodeResultDetail
                    (BatchID
					, DateCodeType
					, DateCodeValue
					, Facility
					, RetestNo
					, TestResult
					, TestTime
					)
				VALUES  
					(
					@dteBatchID
					,@intDateCodeType
					,@vchDateCodeValue
					,@vchFacility
					,@intReTestNo
					,@intTestResult
					,@dteTestTime
					)
		END

		-- if the hearder already exist, update the header else insert one.
		SELECT @bitIsHearderfound = 1 FROM tblQATDateCodeResultHeader
			WHERE   (BatchID = @dteBatchID) 
				AND (DateCodeType = @intDateCodeType)
				AND (RetestNo = @intRetestNo)

		IF	@bitIsHearderfound = 1
		BEGIN
			UPDATE       tblQATDateCodeResultHeader
			SET		TestEndTime = @dteTestEndTime
					,TestResult = @intTestResult
			WHERE   (BatchID = @dteBatchID) 
				AND (DateCodeType = @intDateCodeType)
				AND (RetestNo = @intRetestNo)
		END
		ELSE
		BEGIN
			INSERT INTO tblQATDateCodeResultHeader
				(
				BatchID
				,DateCodeType
				,Facility
				,InterfaceID
				,PackagingLine
				,ReTestNo
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
				,@intDateCodeType
				,@vchFacility
				,@vchInterfaceID
				,@vchPackagingLine
				,@intReTestNo
				,@intShopOrder
				,@dteSOStartTime
				,@dteTestEndTime
				,@intTestResult
				,@dteTestStartTime
				,@vchTesterID
				,@chrQATEntryPoint
				);
		END

		COMMIT TRAN 

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN;
		THROW
	END CATCH
END

GO

