
-- =========================================================
-- Author:		Bong Lee
-- Create date: Sep. 18, 2017
-- Description:	Select records from Unit Count Inbound table
-- =========================================================

CREATE PROCEDURE [dbo].[PPsp_UnitCountInbound_Sel]
	@vchFacility as varchar(3) = NULL
	,@vchPackagingLine as varchar(10) = NULL
	,@dteSOStartTime as datetime = NULL
	,@intShopOrder as int = NULL
	,@intProcessingStatus as tinyint = NULL
	,@vchOrderChange as varchar(20) = NULL
	,@vchOutputLocation as varchar(10) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;

	BEGIN TRY
		IF @vchOutputLocation = 'DF'
		BEGIN
			SELECT *
			FROM tblUnitCountInbound
			WHERE Facility = ISNULL(@vchFacility, Facility)
				AND PackagingLine = ISNULL(@vchPackagingLine, Packagingline)
				AND ShopOrder = ISNULL(@intShopOrder, ShopOrder)
				AND SOStartTime = ISNULL(@dteSOStartTime, SOStartTime)
				AND ProcessingStatus = ISNULL(@intProcessingStatus, ProcessingStatus)
				AND ISNULL(OrderChange,'') = ISNULL(@vchOrderChange, ISNULL(OrderChange,''))
				AND OutPutLocation <> 'RAF'
			ORDER BY CreationTime
		 END 
		 ELSE
		 BEGIN
			SELECT *
			FROM tblUnitCountInbound
			WHERE Facility = ISNULL(@vchFacility, Facility)
				AND PackagingLine = ISNULL(@vchPackagingLine, Packagingline)
				AND ShopOrder = ISNULL(@intShopOrder, ShopOrder)
				AND SOStartTime = ISNULL(@dteSOStartTime, SOStartTime)
				AND ProcessingStatus = ISNULL(@intProcessingStatus, ProcessingStatus)
				AND ISNULL(OrderChange,'') = ISNULL(@vchOrderChange, ISNULL(OrderChange,''))
				AND OutPutLocation = ISNULL(@vchOutPutLocation, OutPutLocation)
			ORDER BY CreationTime
		 END 

	END TRY
	BEGIN CATCH
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
	END CATCH
END

GO

