
-- =============================================
-- Author:		Bong Lee
-- Create date: Jul. 14, 2011
-- Description:	Scrapped Component by Shift Production Date
-- WO#325		Oct. 21, 2013	Bong Lee
-- Description:	Show special scraps

/* -- To Test --
	EXEC	[dbo].[PPsp_ScrappedComponentByDate]
		@vchAction = N'ByShiftProductionDate',
		@vchFacility = N'01',
		@dteFromDate = N'03/25/2010',
		@dteToDate = N'03/27/2010'
*/
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ScrappedComponentByDate]
	@vchAction varchar(50) = NULL,
	@vchFacility varchar(3),
	@dteFromDate datetime , 
	@dteToDate datetime

AS
BEGIN
	BEGIN TRY
		IF @vchAction = 'ByShiftProductionDate'
		BEGIN
			--WO#325 SELECT tCS.Component, tIM.ItemDesc1 as ItemDesc, tSCH.DefaultPkgLine, tSCH.ShiftProductionDate, tCS.Quantity 
			SELECT tCS.Component, ISNULL(tIM.ItemDesc1,tSS.Description) as ItemDesc		--WO#325 
				,tSCH.DefaultPkgLine, tSCH.ShiftProductionDate, tCS.Quantity			--WO#325
			FROM tblComponentScrap tCS
			LEFT OUTER JOIN tfnSessionControlHstDetail(NULL, @vchFacility, NULL, NULL, NULL, NULL, @dteFromDate, 1,@dteToDate,3) tSCH
			ON tCS.ShopOrder = tSCH.ShopOrder AND tCS.StartTime = tSCH.StartTime
			LEFT OUTER JOIN tblItemMaster tIM
			ON tCS.Facility = tIM.Facility AND tCS.Component = tIM.ItemNumber
			LEFT OUTER JOIN tblSpecialScrap tSS											--WO#325
			ON tCS.Component = tSS.Component											--WO#325
			WHERE tCS.Facility = @vchFacility AND tSCH.ShiftProductionDate between @dteFromDate and @dteToDate
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
	END CATCH
END

GO

