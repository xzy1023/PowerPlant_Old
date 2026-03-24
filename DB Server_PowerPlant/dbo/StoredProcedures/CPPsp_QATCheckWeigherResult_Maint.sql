
-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 06, 2018
-- Description:	WO#17432 Insert record to the QAT check weigher test result header and detail tables
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_QATCheckWeigherResult_Maint] 	

	@dteBatchID					datetime
	,@vchFacility				varchar(3)
	,@vchInterfaceID			varchar(24)
	,@dteOverrideID				datetime		= NULL
	,@vchPackagingLine			varchar(10)		= NULL
	,@intRetestNo				int	
	,@intShopOrder				int
	,@dteSOStartTime			datetime		= NULL
	,@dteTestEndTime			datetime
	,@intTestResult				tinyint			= NULL
	,@dteTestStartTime			datetime		= NULL
	,@vchTesterID				varchar(10)		= NULL
	,@chrQATEntryPoint			char(1)			= NULL

	,@decActualWeight			decimal(6,2)	= NULL
	,@decMaxWeight				decimal(6,2)	= NULL
	,@decMinWeight				decimal(6,2)	= NULL
	,@vchRecipe					varchar(30)		= NULL
	,@decTareWeight				decimal(6,2)	= NULL
	,@decTargetWeight			decimal(6,2)	= NULL
	,@intDetailTestResult		tinyint			= NULL
	,@dteTestTime				datetime		= NULL
			
AS
BEGIN
	DECLARE		@bitIsHearderfound		bit		= 0

	BEGIN TRY

	BEGIN TRANSACTION

		-- Add the test detail result to the table, if it is not for update for override id
		IF @dteOverrideID IS NULL AND @dteTestTime IS NOT NULL
		BEGIN
			INSERT INTO tblQATCheckWeigherResultDetail
				(
				ActualWeight
				,BatchID
				,Recipe
				,RetestNo
				,TareWeight
				,TestResult
				,TestTime
				)
				VALUES  
					(
					@decActualWeight
					,@dteBatchID
					,@vchRecipe
					,@intRetestNo
					,@decTareWeight
					,@intDetailTestResult
					,@dteTestTime
					)
		END

		-- if the hearder already exist, update the header else insert one.
		SELECT @bitIsHearderfound = 1 FROM tblQATCheckWeigherResultHeader
		WHERE (Facility = @vchFacility) 
			AND (ShopOrder = @intShopOrder) 
			AND (PackagingLine = @vchPackagingLine)
			AND (BatchID = @dteBatchID) 
			AND (RetestNo = @intRetestNo)

		IF	@bitIsHearderfound = 1
		BEGIN
			UPDATE       tblQATCheckWeigherResultHeader
			SET		OverrideID = @dteOverrideID
					,TestEndTime = @dteTestEndTime
					,TestResult = @intTestResult
			WHERE   (Facility = @vchFacility) 
				AND (ShopOrder = @intShopOrder) 
				AND (PackagingLine = @vchPackagingLine)
				AND (BatchID = @dteBatchID) 
				AND (RetestNo = @intRetestNo)
		END
		ELSE
		BEGIN
			INSERT INTO tblQATCheckWeigherResultHeader
				(
				BatchID
				,Facility
				,InterfaceID
				,MaxWeight
				,MinWeight
				,OverrideID
				,PackagingLine
				,RetestNo
				,ShopOrder
				,SOStartTime
				,TargetWeight
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
					,@decMaxWeight
					,@decMinWeight
					,@dteOverrideID
					,@vchPackagingLine
					,@intRetestNo
					,@intShopOrder
					,@dteSOStartTime
					,@decTargetWeight
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

