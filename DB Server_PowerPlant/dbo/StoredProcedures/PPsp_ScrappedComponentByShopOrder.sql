
-- =============================================
-- Author:		Bong Lee
-- Create date: Jan. 29, 2009
-- Description:	Scrapped Coffee list
-- Task#6631	Aug. 31, 2015	Bong Lee
-- Description:	Use information from Dynamics AX (rewrite)
/* -- To Test --
EXEC	[PPsp_ScrappedComponentByShopOrder]
		@vchAction = N'RawMaterial',
		@chrFacility = N'07',
		@dteFromTime = N'8/1/2015 00:00:00',
		@dteToTime = N'8/25/2015 00:00:00'
*/
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ScrappedComponentByShopOrder]

	@vchAction varchar(50),
	@chrFacility char(3),
	@dteFromTime datetime , 
	@dteToTime datetime
AS
BEGIN

	DECLARE @vchFinalSQLStmt as nvarchar(3000)
	DECLARE @vchERPSQLStmt as nvarchar(3000)
	DECLARE @vchOrderBy as nvarchar(200)
	DECLARE @vchCompanyNo nvarchar(10)
	DECLARE @vchServerName as nvarchar(50)
	DECLARE @vchDBName nvarchar(50)
	DECLARE @vchFromShopOrder varchar(25)
	DECLARE @vchToShopOrder varchar(25)
	DECLARE @cstComponentType varchar(15)	
	DECLARE @cstItemType varchar(30)

	SET NOCOUNT ON;
	BEGIN TRY

		SELECT @cstComponentType = 'RawMaterial'
			   ,@cstItemType = '''''SubWIP'''' , ''''RMOth'''''  

		SELECT @vchCompanyNo = Value2 FROM tblControl WHERE [Key] = 'Facility' AND SubKey = 'General'
		SELECT @vchServerName = Value1, @vchDBName = Value2 FROM tblControl WHERE [Key] = 'ERPEnv' AND SubKey = 'General'

		SELECT  @vchToShopOrder = CAST(MAX(ShopOrder) as varchar(25)), @vchFromShopOrder = CAST(MIN(ShopOrder) as varchar(25))
		FROM dbo.tblComponentScrap
		WHERE Facility = @chrFacility AND (StartTime between @dteFromTime AND @dteToTime)
print @vchFromShopOrder
print @vchToShopOrder
		IF @vchFromShopOrder IS NOT NULL
		BEGIN
			SET @vchERPSQLStmt = 
				N'SELECT tPJ.prodid as ShopOrder, tPJB.itemid as Item, SUM(tPJB.BOMCONSUMP) as QtyIssued 
				FROM ' + @vchDBName + N'.dbo.ProdJournalTable tPJ
				LEFT OUTER JOIN ' + @vchDBName + N'.dbo.ProdJournalBOM tPJB
				ON tPJ.Journalid = tPJB.Journalid 
				AND tPJ.Dataareaid = tPJB.Dataareaid
				LEFT OUTER JOIN ' + @vchDBName + N'.dbo.InventItemGroupItem tIIGI
				ON tPJB.itemid = tIIGI.itemid
				AND tPJ.dataareaid = tIIGI.ItemDataareaId
				WHERE tPJ.prodid BETWEEN ''''' + @vchFromShopOrder + ''''' AND ''''' + @vchToShopOrder +
					 ''''' AND tPJ.dataareaid = ''''' + @vchCompanyNo +
					 ''''' AND journaltype = 0 AND JOURNALNAMEID <> ''''PICK''''
					 AND ((''''' + @vchAction + ''''' = ''''' + @cstComponentType + ''''' AND tIIGI.ItemGroupId in (' + @cstItemType + '))
					 OR (''''' + @vchAction + ''''' <> ''''' + @cstComponentType + ''''' AND tIIGI.ItemGroupId not in (' + @cstItemType + ')))
				group by tPJ.prodid, tPJB.itemid'

			IF @vchAction = @cstComponentType  
				SET @vchOrderBy = N'Order by tEqt.Description, Convert(varchar(8),tCS.StartTime,112), tCS.shopOrder '
			ELSE
				SET @vchOrderBy = N'Order by tEqt.Description, Convert(varchar(8),tCS.StartTime,112), tCS.Component '

			Set @vchFinalSQLStmt = N'SELECT tCS.Facility, tEqt.Description, tSCH.DefaultPkgLine, tCS.StartTime, tCS.ShopOrder, tCS.Component, tPS.FirstName + '' '' + tPS.LastName as Operator ' + 
				', ISNULL(tIM.ItemDesc1,tSS.Description) as ItemDesc ' +		
				', tCS.Quantity, tQI.QtyIssued ' +
				'FROM dbo.tblComponentScrap tCS ' +
				'INNER JOIN (SELECT ShopOrder FROM tblComponentScrap ' +
						'WHERE Facility = @chrFacility AND (StartTime between @dteFromTime AND @dteToTime) ' +
						'GROUP BY ShopOrder) tCSSO ' +
				'ON tCS.ShopOrder = tCSSO.ShopOrder ' +
				'left outer join tblItemMaster tIM ' +
				'on tCS.facility = tIM.facility AND tCS.Component = tIM.ItemNumber ' +
				'LEFT OUTER JOIN tblSpecialScrap tSS ' +									
				'ON tCS.Component = tSS.Component	' +										
				'LEFT OUTER JOIN (SELECT Facility, ShopOrder, StartTime, DefaultPkgLine, Operator ' +
						'FROM tfnSessionControlHstDetail (NULL,@chrFacility,NULL,NULL,NULL,NULL,@dteFromTime,1,@dteToTime,3) ' +
						'GROUP BY Facility, ShopOrder, StartTime, DefaultPkgLine, Operator) tSCH ' +
				'ON tCS.Facility = tSCH.Facility AND tCS.ShopOrder = tSCH.ShopOrder AND CONVERT(varchar(19),tCS.StartTime,120) = CONVERT(varchar(19),tSCH.StartTime,120) ' +
				'LEFT OUTER JOIN dbo.tblPlantStaff tPS ' +
				'On tCS.facility = tPS.facility AND tSCH.Operator = tPS.StaffID ' +
				'LEFT OUTER JOIN dbo.tblEquipment tEqt ' +
				'On tCS.facility = tEqt.facility AND tSCH.DefaultPkgLine = tEqt.EquipmentID ' +
				'LEFT OUTER JOIN (Select * from openquery([' + @vchServerName + '],''' + @vchERPSQLStmt + ''')) tQI ' + 
				'On tCS.ShopOrder = tQI.ShopOrder AND tCS.Component = tQI.item ' +
				'WHERE ((''' + @vchAction + ''' = ''' + @cstComponentType + ''' AND tIM.ItemType in (''SubWIP'' , ''RMOth'')) ' +
				'OR (''' + @vchAction + ''' <> ''' + @cstComponentType + ''' AND tIM.ItemType not in (''SubWIP'' , ''RMOth''))) ' +	
				@vchOrderBy

	--print '1=' + @vchERPSQLStmt 
	--print '2=' + @vchFinalSQLStmt

			EXEC sp_ExecuteSQL @vchFinalSQLStmt, N'@dteFromTime datetime, @dteToTime datetime, @chrFacility char(3), @vchFromShopOrder varchar(25), @vchToShopOrder varchar(25) ', @dteFromTime ,@dteToTime, @chrFacility, @vchFromShopOrder, @vchToShopOrder
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

