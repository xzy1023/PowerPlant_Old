
-- =============================================
-- Author:		Bong Lee
-- Create date: Jan 10, 2012
-- Description:	Maintain Scrap Component
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ScrapComponent_Maint] 
	 @vchAction	varchar(50) 
	,@Facility	varchar(3) = Null
	,@ShopOrder	int = 0
	,@StartTime	datetime = Null
	,@Component	varchar(35) = Null
	,@Quantity	decimal(8,2) = 0
	,@vchCreatedBy	varchar(10) = Null
	,@vchRtnMsg varchar(150) output
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @bitExist as bit

	BEGIN TRY

	SET @bitExist = 0
	-- check record existance

	IF Exists (SELECT 1 FROM dbo.tblComponentScrap
		WHERE Facility = @Facility 
		AND ShopOrder = @ShopOrder
		AND StartTime = @StartTime
		AND Component = @Component)
	SET @bitExist = 1


	IF @vchAction = 'MODIFY'
	BEGIN
		IF @bitExist = 0 
			SET @vchAction = 'ADD'
		ELSE
		BEGIN
			IF @Quantity = 0
				SET @vchAction = 'DELETE'
			ELSE
				SET @vchAction = 'UPDATE'
		END
	END

    IF @vchAction = 'ADD' 
	BEGIN 
		IF @bitExist = 0
		BEGIN
			INSERT INTO dbo.tblComponentScrap
				  (Facility, ShopOrder, StartTime, Component, Quantity)
			Output 'Add', @vchCreatedBy, Getdate(), Inserted.*
						Into tblComponentScrapAudit
			VALUES  (
					@Facility
					,isnull(@ShopOrder,0)
					,@StartTime
					,@Component
					,@Quantity
					)
				SET @vchRtnMsg = '200 -- Scrap record has been added successfully.'
		END
		ELSE
			SET @vchRtnMsg = '901 -- Scrap record with same shop order and time frame is already existed. Add failure.'		-- WO#498

	END
	ELSE
		IF @vchAction = 'UPDATE'
			Begin 
				IF @bitExist = 1
				BEGIN													 
					UPDATE    dbo.tblComponentScrap
					   SET    Quantity = @Quantity
					Output 'Update', @vchCreatedBy, Getdate(), Deleted.*
						Into tblComponentScrapAudit
					WHERE Facility = @Facility 
						AND ShopOrder = @ShopOrder
						AND StartTime = @StartTime
						AND Component = @Component
					SET @vchRtnMsg = '200 -- Scrap has been updated successfully.'		
				END															
				ELSE
				BEGIN
					SET @vchRtnMsg = '901 -- Update scrap record can not be found. Update failure'	
				END
			End
		ELSE
			IF @vchAction = 'DELETE'
			BEGIN
				IF @bitExist = 1
				Begin
					DELETE FROM dbo.tblComponentScrap
						Output 'Delete', @vchCreatedBy, Getdate(), Deleted.*
							Into tblComponentScrapAudit
						WHERE Facility = @Facility 
							AND ShopOrder = @ShopOrder
							AND StartTime = @StartTime
							AND Component = @Component
					SET @vchRtnMsg = '200	-- Scrap has been deleted successfully.'	
				END
				ELSE
					SET @vchRtnMsg = '901 -- Deletion scrap record can not be found. Deletion failure'	
			End
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();

		-- Use RAISERROR inside the CATCH block to return error
		-- information about the original error that caused
		-- execution to jump to the CATCH block.
		RAISERROR (@ErrorMessage, -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH;
END

GO

