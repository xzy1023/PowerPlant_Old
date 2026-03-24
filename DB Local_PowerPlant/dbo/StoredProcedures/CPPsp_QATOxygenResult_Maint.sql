
-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 06, 2018
-- Description:	WO#17432 Insert record to the QAT Oxygen Test Result Header and detail tables
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_QATOxygenResult_Maint] 	

	@dteBatchID					datetime
	,@vchFacility				varchar(3)
	,@vchInterfaceID			varchar(24)
	,@decMaxOxygen				decimal(6,5)	= NULL
	,@dteOverrideID				datetime		= NULL
	,@intShopOrder				int
	,@dteSOStartTime			datetime		= NULL
	,@vchPackagingLine			varchar(10)		= NULL
	,@intReTestNo				int	
	,@dteTestEndTime			datetime
	,@intTestResult				tinyint			= NULL
	,@dteTestStartTime			datetime		= NULL
	,@vchTesterID				varchar(10)		= NULL
	,@chrQATEntryPoint			char(1)			= NULL

	,@decOxygen					decimal(6,5)	= NULL
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

		-- Add the test detail result to the table, if it is not for update for override id
		 IF @dteOverrideID IS NULL AND @dteTestTime IS NOT NULL
		 BEGIN
			INSERT INTO tblQATOxygenResultDetail
				(
				BatchID
				,LaneNo
				,Oxygen
				,ReTestNo
				,SampleNo
				,TestResult
				,TestTime
				)
				VALUES  
					(
					@dteBatchID
					,@intLaneNo
					,@decOxygen
					,@intReTestNo
					,@intSampleNo
					,@bitDetailTestResult
					,@dteTestTime
					)
		END

		-- if it is last sampling of the test, update the header
		IF @bitLastSample = 1 
		BEGIN
			SELECT @bitIsHearderfound = 1 FROM tblQATOxygenResultHeader
			WHERE (Facility = @vchFacility) 
				AND (ShopOrder = @intShopOrder) 
				AND (PackagingLine = @vchPackagingLine)
				AND (BatchID = @dteBatchID) 
				AND (ReTestNo = @intReTestNo)

			IF	@bitIsHearderfound = 1
			BEGIN
				UPDATE       tblQATOxygenResultHeader
				SET		OverrideID = @dteOverrideID
						,TestEndTime = @dteTestEndTime
						,TestResult = @intTestResult
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
			SELECT @bitIsHearderfound = 1 FROM tblQATOxygenResultHeader
			WHERE (Facility = @vchFacility) 
				AND (ShopOrder = @intShopOrder) 
				AND (PackagingLine = @vchPackagingLine)
				AND (BatchID = @dteBatchID) 
				AND (ReTestNo = @intReTestNo)

			IF	@bitIsHearderfound = 1
			BEGIN
				UPDATE       tblQATOxygenResultHeader
				SET		OverrideID = @dteOverrideID
						,TestEndTime = @dteTestEndTime
				WHERE   (Facility = @vchFacility) 
					AND (ShopOrder = @intShopOrder) 
					AND (PackagingLine = @vchPackagingLine)
					AND (BatchID = @dteBatchID) 
					AND (ReTestNo = @intReTestNo)
			END
		END 

		-- If it is the first sampling of the test, write a header record
		IF @intSampleNo = 1 
			OR (@bitIsHearderfound = 0 AND @bitLastSample = 1 )
			OR (@bitIsHearderfound = 0 AND @dteOverrideID IS NOT NULL)
		BEGIN
			INSERT INTO tblQATOxygenResultHeader
				(
				BatchID
				,Facility
				,InterfaceID
				,MaxOxygen
				,OverrideID
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
					,@vchFacility
					,@vchInterfaceID
					,@decMaxOxygen
					,@dteOverrideID
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

