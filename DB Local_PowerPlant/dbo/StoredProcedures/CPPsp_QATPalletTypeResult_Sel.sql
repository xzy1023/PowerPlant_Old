
-- =============================================================================
-- Author:		
-- WO#17423:	Aug. 24, 2018	Bong Lee
-- Description:	Select QAT pallet type test result
-- ==============================================================================
CREATE PROCEDURE [dbo].[CPPsp_QATPalletTypeResult_Sel] 
	@vchFacility		varchar(3)		= NULL
	,@dteBatchID		datetime		= NULL
	,@intShopOrder		int				= NULL
	,@vchPackagingLine  varchar(10)		= NULL
	,@dteFromTime		datetime		= NULL
	,@dteToTime			datetime		= NULL

AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
	SELECT * from [tblQATPalletTypeResult] 
			WHERE Facility = ISNULL(@vchFacility, Facility)
				AND BatchID = ISNULL(@dteBatchID, BatchID)
				AND ShopOrder = ISNULL(@intShopOrder, ShopOrder)
				AND PackagingLine= ISNULL(@vchPackagingLine, PackagingLine)
				AND TestStartTime >= ISNULL(@dteFromTime, TestStartTime)
				AND TestStartTime <= ISNULL(@dteToTime, TestStartTime)
			ORDER BY RRN DESC
	END TRY
	BEGIN CATCH
			THROW
	END CATCH
END

GO

