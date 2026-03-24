



-- =============================================
-- Author:		Bong Lee
-- Create date: Oct 20, 2006
-- Description:	Pallet IO
-- WO#181:		Bong Lee	Create date: Mar 24, 2011
-- Description:	Add Facility filter option
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_PalletIO] 
	-- Add the parameters for the stored procedure here
	@chrPkgLine char(10) = '', 
	@vchShopOrder varchar(10) = '',
	@intPrintStatus int = 0,
	@vchFacility varchar(3) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	DECLARE @vchSQLStmt  varchar(350);
	DECLARE @vchWhere  varchar(100);
	DECLARE @vchOrderBy varchar(100)

	SET NOCOUNT ON;
	BEGIN TRY

	SELECT * ,CreationDate + CreationTime as CreationOrder
	FROM tblPallet 
	WHERE PrintStatus = CAST(@intPrintStatus as CHAR(1)) 
		And (@vchFacility is NULL OR Facility = @vchFacility)
		And  (@chrpkgline = '' Or DefaultPkgLine = @chrPkgLine)
		And  (@vchShopOrder = '' Or ShopOrder = cast(@vchShopOrder as int))
	
	ORDER By CASE WHEN @chrPkgLine <> '' and @vchShopOrder <> '' THEN CreationDate + CreationTime
				WHEN @chrPkgLine <> '' and @vchShopOrder = '' THEN Convert(varchar(50),ShopOrder) + CreationDate + CreationTime
				WHEN @chrPkgLine = '' and @vchShopOrder <> '' THEN DefaultPkgLine + CreationDate + CreationTime
				ELSE DefaultPkgLine + Convert(varchar(50),ShopOrder) +  CreationDate + CreationTime
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

