
-- =============================================
-- Author:		Bong Lee
-- Create date: Aug 08, 2018
-- Description:	WO#17432 Insert record to the QAT Line Clearance Result Header/Detail tables
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_QATLineClearanceResult_Maint] 	

	@dteBatchID					datetime
	,@bitByPassAllTest			bit
	,@vchFacility				varchar(3)
	,@vchInterfaceID			varchar(24)
	,@vchPackagingLine			varchar(10)		
	,@intShopOrder				int				
	,@dteSOStartTime			datetime		= NULL
	,@dteTestEndTime			datetime		= NULL
	,@dteTestStartTime			datetime		= NULL
	,@vchTesterID				varchar(10)		= NULL
	,@chrQATEntryPoint			char(1)			= NULL

	,@dteTaskEndTime			datetime		= NULL
	,@intTaskID					int				= NULL
	,@dteTaskStartTime			datetime		= NULL
	,@intTaskStatus				tinyint			= NULL
			
AS
BEGIN

	BEGIN TRY

		BEGIN TRANSACTION
		-- If the header record is not exist, insert else update
			IF NOT EXISTS(SELECT 1 FROM tblQATLineClearanceResultHeader
				WHERE Facility = @vchFacility 
					AND ShopOrder = @intShopOrder
					AND PackagingLine = @vchPackagingLine
					AND BatchID = @dteBatchID)	
			BEGIN
				INSERT INTO tblQATLineClearanceResultHeader
					(
					BatchID
					,ByPassAllTest
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
						,@bitByPassAllTest
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
				UPDATE       tblQATLineClearanceResultHeader
				SET		TestEndTime = @dteTestEndTime
				WHERE   (Facility = @vchFacility) 
					AND (ShopOrder = @intShopOrder) 
					AND PackagingLine = @vchPackagingLine
					AND (BatchID = @dteBatchID) 
			END

			-- Add test detail result to the table if task id is not null
			If @intTaskID IS NOT NULL 
			BEGIN
				INSERT INTO tblQATLineClearanceResultDetail
					(
					BatchID
					,Facility
					,PackagingLine
					,ShopOrder
					,TaskEndTime
					,TaskID
					,TaskStartTime
					,TaskStatus
					)
					VALUES  
						(@dteBatchID
						,@vchFacility
						,@vchPackagingLine
						,@intShopOrder
						,@dteTaskEndTime
						,@intTaskID
						,@dteTaskStartTime
						,@intTaskStatus
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

