
-- =============================================================================
-- Author:		
-- WO#17423:	Aug. 28, 2018	Bong Lee
-- Description:	Select QAT Date Code test result
-- ==============================================================================
CREATE PROCEDURE [dbo].[CPPsp_QATDateCodeResult_Sel] 
	@vchFacility		varchar(3)		= NULL
	,@dteBatchID		datetime		= NULL
	,@intDateCodeType	int				= NULL
	,@vchPackagingLine  varchar(10)		= NULL
	,@intShopOrder		int				= NULL
	,@dteFromTime		datetime		= NULL
	,@dteToTime			datetime		= NULL

AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
	SELECT * from [tblQATDateCodeResult] 
			WHERE Facility = ISNULL(@vchFacility, Facility)
				AND BatchID = ISNULL(@dteBatchID, BatchID)
				AND DateCodeType = ISNULL(@intDateCodeType, DateCodeType)
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

