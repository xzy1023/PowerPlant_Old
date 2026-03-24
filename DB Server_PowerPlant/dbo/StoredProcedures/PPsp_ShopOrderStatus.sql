
-- =============================================
-- Author:		Bong Lee
-- Create date: Mar.23 2011
-- Description:	Get Shop Order Status
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ShopOrderStatus]
	-- Add the parameters for the stored procedure here
	@intShopOrder as int,
	@vchShopOrderStatus as varchar(50) OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @bitClosed as bit
	DECLARE @dteShopOrderClosingTime as datetime
	
	BEGIN TRY

		-- First, if it is found in the To Be Closed Shop Order table, use the Power Plant Closing time
		-- since the record has not been posted to close the shop order in BPCS.
		SELECT @dteShopOrderClosingTime = ClosingTime FROM tblToBeClosedShopOrder WHERE RRN = 
		(SELECT MAX(RRN) as RRN FROM tblToBeClosedShopOrder WHERE ShopOrder = @intShopOrder)
		
		-- Second, -- if Shop order is not found in To Be Closed Shop Order table then check the Closed Shop Order History table
		-- If found, use the BPCS Closing Time
		IF @dteShopOrderClosingTime IS NULL		
		BEGIN
			SELECT @dteShopOrderClosingTime = BPCSClosingTime   FROM tblClosedShopOrderHst WHERE RRN = 
				(SELECT MAX(RRN) as RRN FROM tblClosedShopOrderHst WHERE ShopOrder = @intShopOrder)
		END

		-- Is the shop order closed in the Active shop order table
		SELECT @bitClosed = ISNULL(Closed,0) FROM tblShopOrder  WHERE ShopOrder = @intShopOrder

		-- Analysis above resaults to determine the shop order status
		IF @dteShopOrderClosingTime IS NOT NULL		-- Shop order is found in shop order closing table
		BEGIN
			IF @bitClosed = 0					-- Shop order is open
				SET @vchShopOrderStatus = 'Reopen'
			ELSE								-- Shop order is closed or not in the open shop order list
				SET @vchShopOrderStatus = 'Closed at ' + CONVERT(varchar(19), @dteShopOrderClosingTime, 120)
		END
		ELSE
		BEGIN									-- Shop order is not found in shop order closing table
			IF @bitClosed = 0					-- Shop order is open
				SET @vchShopOrderStatus = 'Open'
			ELSE
			BEGIN
				SELECT @dteShopOrderClosingTime = MAX(CreationDateTime) FROM
				(SELECT CreationDateTime FROM tblPallethst WHERE ShopOrder = @intShopOrder AND OrderComplete = 'Y'
				UNION
				SELECT CreationDateTime FROM tblPallet WHERE ShopOrder = @intShopOrder AND OrderComplete = 'Y') tP
	--print @dteShopOrderClosingTime 
				IF @dteShopOrderClosingTime IS NOT NULL
					SET @vchShopOrderStatus = 'Closed at ' + CONVERT(varchar(19), @dteShopOrderClosingTime, 120)
				ELSE
					IF EXISTS (SELECT * FROM tblShopOrderHst WHERE ShopOrder = @intShopOrder)
						SET @vchShopOrderStatus = 'Closed at unknown time'
					ELSE
						SET @vchShopOrderStatus = 'Invalid Shop Order'
			END

		END

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

