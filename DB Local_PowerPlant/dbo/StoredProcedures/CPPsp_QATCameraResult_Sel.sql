
-- =============================================================================
-- Author:		
-- WO#17423:	Aug. 24, 2018	Bong Lee
-- Description:	Select QAT cameras verification result
-- ==============================================================================
CREATE PROCEDURE [dbo].[CPPsp_QATCameraResult_Sel] 
	@vchFacility		varchar(3)		= NULL
	,@dteBatchID		datetime		= NULL
	,@vchPackagingLine  varchar(10)		= NULL
	,@intShopOrder		int				= NULL
	,@dteFromTime		datetime		= NULL
	,@dteToTime			datetime		= NULL

AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
	SELECT * from tblQATCameraResult
			WHERE Facility = ISNULL(@vchFacility, Facility)
				AND BatchID = ISNULL(@dteBatchID, BatchID)
				AND PackagingLine= ISNULL(@vchPackagingLine, PackagingLine)
				AND ShopOrder = ISNULL(@intShopOrder, ShopOrder)
				AND TestStartTime >= ISNULL(@dteFromTime, TestStartTime)
				AND TestStartTime <= ISNULL(@dteToTime, TestStartTime)
	END TRY
	BEGIN CATCH
			THROW
	END CATCH
END

GO

