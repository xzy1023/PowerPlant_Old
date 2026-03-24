
-- =============================================
-- Author:		Bong Lee
-- Create date: Aug 08, 2018
-- Description:	WO#17432 Insert record to the QAT Weight Result Header table
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_QATWeightResult_Maint] 	

	@dteBatchID					datetime
	,@vchFacility				varchar(3)
	,@vchInterfaceID			varchar(24)
	,@decMaxWeight				decimal(6,1)	= NULL
	,@decMinWeight				decimal(6,1)	= NULL
	,@dteOverrideID				datetime		= NULL
	,@vchPackagingLine			varchar(10)		= NULL
	,@intReTestNo				int	
	,@intShopOrder				int
	,@dteSOStartTime			datetime		= NULL
	,@decTareWeight				decimal(8,4)	= NULL
	,@decTargetWeight			decimal(6,1)	= NULL
	,@dteTestEndTime			datetime
	,@bitTestResult				bit				= NULL
	,@dteTestStartTime			datetime		= NULL
	,@vchTesterID				varchar(10)		= NULL
	,@chrQATEntryPoint			char(1)			= NULL

	,@decActualWeight			decimal(6,1)	= NULL
	,@bitLastSample				bit				= NULL
	,@intLaneNo					int				= NULL
	,@intSampleNo				int				= NULL
	,@bitDetailTestResult		bit				= NULL
	,@dteTestTime				datetime		= NULL
			
AS
BEGIN
	DECLARE		@bitIsHearderfound		bit		= 0

	BEGIN TRY

	BEGIN TRANSACTION
		-- Add weight test detail result to the table, if it is not for update for override id
		 IF @dteOverrideID IS NULL AND @dteTestTime IS NOT NULL
		 BEGIN
			INSERT INTO tblQATWeightResultDetail
				(ActualWeight
				,BatchID
				,LaneNo
				,ReTestNo
				,SampleNo
				,TestResult
				,TestTime)
				VALUES  
					(@decActualWeight
					,@dteBatchID
					,@intLaneNo
					,@intReTestNo
					,@intSampleNo
					,@bitDetailTestResult
					,@dteTestTime)
		END

		-- if it is last sampling of the test, update the header
		IF @bitLastSample = 1 
		BEGIN
			SELECT @bitIsHearderfound = 1 FROM tblQATWeightResultHeader
			WHERE (Facility = @vchFacility) 
				AND (ShopOrder = @intShopOrder) 
				AND (PackagingLine = @vchPackagingLine)
				AND (BatchID = @dteBatchID) 
				AND (ReTestNo = @intReTestNo)

			IF	@bitIsHearderfound = 1
			BEGIN
				UPDATE       tblQATWeightResultHeader
				SET		OverrideID = @dteOverrideID
						,TestEndTime = @dteTestEndTime
						,TestResult = @bitTestResult
				WHERE   (Facility = @vchFacility) 
					AND (ShopOrder = @intShopOrder) 
					AND (PackagingLine = @vchPackagingLine)
					AND (BatchID = @dteBatchID) 
					AND (ReTestNo = @intReTestNo)
			END
		END 

		-- if test data is overriden, update the header
		IF @dteOverrideID IS NOT NULL
		BEGIN
			SELECT @bitIsHearderfound = 1 FROM tblQATWeightResultHeader
			WHERE (Facility = @vchFacility) 
				AND (ShopOrder = @intShopOrder) 
				AND (PackagingLine = @vchPackagingLine)
				AND (BatchID = @dteBatchID) 
				AND (ReTestNo = @intReTestNo)

			IF	@bitIsHearderfound = 1
			BEGIN
				UPDATE       tblQATWeightResultHeader
				SET		OverrideID = @dteOverrideID
						,TestEndTime = @dteTestEndTime
				WHERE   (Facility = @vchFacility) 
					AND (ShopOrder = @intShopOrder) 
					AND (PackagingLine = @vchPackagingLine)
					AND (BatchID = @dteBatchID) 
					AND (ReTestNo = @intReTestNo)
			END
		END 
	
		-- If it is the first sampling of the test, write a header record or header record is not found.
		IF @intSampleNo = 1 
			OR (@bitIsHearderfound = 0 AND @bitLastSample = 1 )
			OR (@bitIsHearderfound = 0 AND @dteOverrideID IS NOT NULL)
		BEGIN
			INSERT INTO tblQATWeightResultHeader
				(BatchID
				,Facility
				,InterfaceID
				,MaxWeight
				,MinWeight
				,OverrideID
				,PackagingLine
				,ReTestNo
				,ShopOrder
				,SOStartTime
				,TareWeight	
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
					,@intReTestNo
					,@intShopOrder
					,@dteSOStartTime
					,@decTareWeight	
					,@decTargetWeight
					,@dteTestEndTime
					,@bitTestResult
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

