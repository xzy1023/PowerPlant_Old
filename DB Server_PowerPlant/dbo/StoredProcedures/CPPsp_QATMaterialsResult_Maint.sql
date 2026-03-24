
-- =============================================
-- Author:		Bong Lee
-- Create date: Aug 08, 2018
-- Description:	WO#17432 Insert record to the QAT Materials Result Header/detail tables
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_QATMaterialsResult_Maint] 	

	@dteBatchID					datetime
	,@vchFacility				varchar(3)
	,@vchInterfaceID			varchar(24)
	,@vchPackagingLine			varchar(10)		
	,@intShopOrder				int				
	,@dteSOStartTime			datetime		= NULL
	,@dteTestEndTime			datetime		= NULL
	,@dteTestStartTime			datetime		= NULL
	,@vchTesterID				varchar(10)		= NULL
	,@chrQATEntryPoint			char(1)			= NULL	
	,@vchComponentNo			varchar(35)		= NULL
	,@dteOverrideID				datetime		= NULL
	,@vchScannedComponentNo		varchar(50)		= NULL
	,@intScannedLotNo			varchar(35)		= NULL
	,@bitTestResult				bit				= NULL
	,@dteTestTime				datetime		= NULL
		
AS
BEGIN

	BEGIN TRY

		BEGIN TRANSACTION
		-- 
			-- if the header record does not exist then inert the record else update.
			IF NOT EXISTS(
				SELECT 1 FROM tblQATMaterialsResultHeader
				WHERE Facility = @vchFacility 
					AND ShopOrder = @intShopOrder
					AND PackagingLine = @vchPackagingLine
					AND BatchID = @dteBatchID	
				) 
			BEGIN
				INSERT INTO tblQATMaterialsResultHeader
					(
					BatchID
					,Facility
					,InterfaceID
					,PackagingLine
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
						,@intShopOrder
						,@dteSOStartTime
						,@dteTestEndTime
						,@dteTestStartTime
						,@vchTesterID
						,@chrQATEntryPoint
					);
			END
			ELSE
			BEGIN
				UPDATE       tblQATMaterialsResultHeader
				SET		TestEndTime = @dteTestEndTime
				WHERE   (Facility = @vchFacility) 
					AND (ShopOrder = @intShopOrder) 
					AND (PackagingLine = @vchPackagingLine)
					AND (BatchID = @dteBatchID) 
			END

			-- Add Materials test detail result to the table
			If @vchComponentNo IS NOT NULL 
			BEGIN
				INSERT INTO tblQATMaterialsResultDetail
					(
					BatchID
					,ComponentNo
					,ShopOrder
					,OverrideID
					,ScannedComponentNo
					,ScannedLotNo
					,TestResult
					,TestTime
					)
					VALUES  
						(
						@dteBatchID
						,@vchComponentNo
						,@intShopOrder
						,@dteOverrideID
						,@vchScannedComponentNo
						,@intScannedLotNo
						,@bitTestResult
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

