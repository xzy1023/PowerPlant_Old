
-- =============================================
-- Author:		Bong Lee
-- WO#512:		May 02, 2012	Bong Lee
-- Description:	Show who printed the pallet labels
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_LabelPrintLog_Sel]
	@vchAction varchar(50) = NULL, 
	@vchFacility varchar(3) = NULL,
	@intLabelID int = NULL,
	@vchPkgLine varchar(10)= NULL,
	@intShopOrder int = NULL,
	@dteFromTimeSubmited datetime = NULL,
	@dteToTimeSubmited datetime = NULL,
	@vchPrintedByID varchar(10) = NULL
AS
BEGIN

	SET NOCOUNT ON;
	BEGIN TRY
	
	SELECT tLPL.*, tPS.FirstName + ' ' + tPS.LastName as PrintedByName, tPH.ShopOrder
		,tPH.ItemNumber, tPH.CreationDateTime, tPH.Quantity, tPH.QtyPerPallet
	FROM  tblLabelPrintLog tLPL	
	LEFT OUTER JOIN tblPlantStaff tPS
	ON tLPL.Facility = tPS.Facility	AND tLPL.Requestor = tPS.StaffID
	LEFT OUTER JOIN tblPalletHst tPH
	ON tLPL.LabelID = tPH.PalletID
	WHERE (@vchFacility is NULL OR tLPL.Facility = @vchFacility)
		   AND (@intLabelID is NULL OR	@intLabelID = tLPL.LabelId)
		   AND (@vchPkgLine is NULL OR @vchPkgLine = tLPL.DefaultPkgLine)
		   AND (@intShopOrder is NULL OR @intShopOrder = tPH.ShopOrder)
		   --AND ((@dteFromTimeSubmited IS NULL AND @dteToTimeSubmited IS NULL) OR (tLPL.TimeSubmit between @dteFromTimeSubmited AND @dteToTimeSubmited))
		   AND (tLPL.TimeSubmit >= ISNULL(@dteFromTimeSubmited, '1800/01/01 00:00:00') AND tLPL.TimeSubmit <= ISNULL(@dteToTimeSubmited, '9999/12/31 23:59:59'))
		   AND (@vchPrintedByID IS NULL OR @vchPrintedByID = tLPL.Requestor)
	ORDER BY PalletID,TimeSubmit
	
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

grant execute on object::dbo.PPsp_LabelPrintLog_Sel to ppuser

GO

