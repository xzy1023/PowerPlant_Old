
-- =============================================
-- Author:		Bong Lee
-- Create date: Aug 08, 2018
-- Description:	WO#17432 Insert record to the QAT Pressure Result table
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_QATPressureResult_Maint] 	

	@dteBatchID					datetime
	,@vchFacility				varchar(3)
	,@vchInterfaceID			varchar(24)
	,@dteOverrideID				datetime		= NULL
	,@intShopOrder				int
	,@dteSOStartTime			datetime		= NULL
	,@vchPackagingLine			varchar(10)		= NULL
	,@intReTestNo				int	
	,@dteTestEndTime			datetime
	,@bitTestResult				bit				= NULL
	,@dteTestStartTime			datetime		= NULL
	,@vchTesterID				varchar(10)		= NULL
	,@chrQATEntryPoint			char(1)			= NULL

	,@intLaneNo					int				= NULL
	,@intSampleNo				int				= NULL
	,@bitDetailTestResult		bit				= NULL
	,@dteTestTime				datetime		= NULL

AS
BEGIN

	BEGIN TRY
		BEGIN TRANSACTION

			-- Maintains the header
			IF @intSampleNo IS NULL OR @intSampleNo = 0
			BEGIN
				-- If the header record does not exist, insert header else update header
				IF NOT EXISTS(SELECT 1 FROM tblQATPressureResultHeader 
					WHERE (Facility = @vchFacility) 
						AND (ShopOrder = @intShopOrder) 
						AND (PackagingLine = @vchPackagingLine)
						AND (BatchID = @dteBatchID) 
						AND (ReTestNo = @intReTestNo))

				BEGIN
					INSERT INTO tblQATPressureResultHeader
					(
					BatchID
					,Facility
					,InterfaceID
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
						,@dteOverrideID
						,@vchPackagingLine
						,@intReTestNo
						,@intShopOrder
						,@dteSOStartTime
						,@dteTestEndTime
						,@bitTestResult
						,@dteTestStartTime
						,@vchTesterID
						,@chrQATEntryPoint
					);
				END
				ELSE
				BEGIN
					-- Update the header
					UPDATE  tblQATPressureResultHeader
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
			ELSE
			-- insert test result to detail record if sample no is not 0 or NULL
			BEGIN
				INSERT INTO tblQATPressureResultDetail
					(
					BatchID
					,LaneNo
					,ReTestNo
					,SampleNo
					,TestResult
					,TestTime
					)
					VALUES  
						(
						@dteBatchID
						,@intLaneNo
						,@intReTestNo
						,@intSampleNo
						,@bitDetailTestResult
						,@dteTestTime
						)
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

