




-- =============================================
-- Author:		Bong Lee
-- Create date: Jan. 29, 2009
-- Description:	Scrapped Coffee list
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ScrappedCoffee]

	@vchAction VARCHAR(30),
	@chrFacility char(3),
	@dteFromTime datetime , 
	@dteToTime datetime
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		SELECT tCS.Facility, tEqt.Description, tSCH.DefaultPkgLine, tCS.ShopOrder, tCS.Component, tIM.ItemDesc1 as ItemDesc, tCS.Quantity, tCS.StartTime
		FROM dbo.tblComponentScrap tCS
		left outer join tblItemMaster tIM 
		on tCS.facility = tIM.facility AND tCS.Component = tIM.ItemNumber
		left outer join (Select Distinct StartTime,ShopOrder, DefaultPkgLine From dbo.tblSessionControlHst) tSCH 
		On tCS.StartTime = tSCH.StartTime and tCS.ShopOrder = tSCH.ShopOrder
		left outer join dbo.tblEquipment tEqt
		On tCS.facility = tEqt.facility AND tSCH.DefaultPkgLine = tEqt.EquipmentID
		left outer join (Select * from openquery(S10A8379,'SELECT iprod FROM PBPCSF.iim WHERE ipfdv = ''BEAN'' or ipfdv = ''GRND''
			Order by iprod')) tIIM
		On tCS.Component = tIIM.iprod
		Where tCS.facility = @chrFacility and (tCS.StartTime between @dteFromTime and @dteToTime) and tIIM.iprod is not null
		Order by tEqt.Description,tCS.shopOrder 
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

