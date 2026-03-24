
-- =============================================================================
-- Author:		
-- WO#17423:	Aug. 15, 2018	Bong Lee
-- Description:	Select QAT supervisor override information
-- ==============================================================================
CREATE PROCEDURE [dbo].[CPPsp_QATOverride_Sel] 
	@vchFacility		varchar(3)		= NULL
	,@dteOverrideID		datetime		= NULL
	,@intShopOrder		int				= NULL
	,@vchPackagingLine  varchar(10)		= NULL

AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
	SELECT * from tblQATOverride
			WHERE Facility = ISNULL(@vchFacility, Facility)
				AND OverrideID = ISNULL(@dteOverrideID, OverrideID)
				AND ShopOrder = ISNULL(@intShopOrder, ShopOrder)
				AND PackagingLine= ISNULL(@vchPackagingLine, PackagingLine)
			ORDER BY RRN DESC
	END TRY
	BEGIN CATCH
			THROW
	END CATCH
END

GO

