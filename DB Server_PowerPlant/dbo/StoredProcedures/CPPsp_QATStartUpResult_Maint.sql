
-- =============================================
-- Author:		Bong Lee
-- Create date: Mar. 27, 2019
-- Description:	WO#17432 Insert record to the test Result Header/Detail tables 
--				for QAT Start-up or shift change checks
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_QATStartUpResult_Maint] 	

	@dteBatchID					datetime
	,@bitByPassAllTest			bit
	,@vchFacility				varchar(3)
	,@vchInterfaceID			varchar(24)
	,@vchPackagingLine			varchar(10)	
	,@chrQATEntryPoint			char(1)	
	,@intShopOrder				int				
	,@dteSOStartTime			datetime		= NULL
	,@dteTestEndTime			datetime		= NULL
	,@dteTestStartTime			datetime		= NULL
	,@vchTesterID				varchar(10)

	,@dteTaskEndTime			datetime		= NULL
	,@intTaskID					int				= NULL
	,@dteTaskStartTime			datetime		= NULL
	,@intTaskStatus				tinyint			= NULL
			
AS
BEGIN

	BEGIN TRY

		BEGIN TRANSACTION
		-- If the header record is not exist, insert else update
			IF NOT EXISTS(SELECT 1 FROM tblQATStartUpResultHeader
				WHERE Facility = @vchFacility 
					AND ShopOrder = @intShopOrder
					AND PackagingLine = @vchPackagingLine
					AND BatchID = @dteBatchID)	
			BEGIN
				INSERT INTO tblQATStartUpResultHeader
					(
					BatchID
					,ByPassAllTest
					,Facility
					,InterfaceID
					,PackagingLine
					,QATEntryPoint
					,ShopOrder
					,SOStartTime
					,TestEndTime
					,TestStartTime
					,TesterID
					)
					VALUES
					(
						@dteBatchID
						,@bitByPassAllTest
						,@vchFacility
						,@vchInterfaceID
						,@vchPackagingLine
						,@chrQATEntryPoint
						,@intShopOrder
						,@dteSOStartTime
						,@dteTestEndTime
						,@dteTestStartTime
						,@vchTesterID
					);
			END
			ELSE
			BEGIN
				UPDATE       tblQATStartUpResultHeader
				SET		TestEndTime = @dteTestEndTime
				WHERE   (Facility = @vchFacility) 
					AND (ShopOrder = @intShopOrder) 
					AND PackagingLine = @vchPackagingLine
					AND (BatchID = @dteBatchID) 
			END

			-- Add test detail result to the table if task id is not null
			If @intTaskID IS NOT NULL 
			BEGIN
				INSERT INTO tblQATStartUpResultDetail
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

