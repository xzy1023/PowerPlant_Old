





-- =============================================
-- Author:		Bong Lee
-- Create date: Sep. 26, 2008
-- Description:	Roast Coffee Inventory Shortage
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoastCoffeeInventoryShortage] 
	-- Add the parameters for the stored procedure here
	@vchAction varchar(50) = NULL, 
	@chrFacility char(3),
	@dteProductionDate datetime,
	@intShift tinyint
AS
BEGIN
	--DECLARE @intProductionDate as int
	DECLARE @vchProductionDate as varchar(8)
	DECLARE @vchFromSilo as varchar(5)
	DECLARE @vchToSilo as varchar(5)
	DECLARE @vchSQLStmt as varchar(2000)


	SET NOCOUNT ON;

	--SELECT @intProductionDate = Cast(Convert(varchar(10),@dteProductionDate,112) as int)
	SELECT @vchProductionDate = Convert(varchar(10),@dteProductionDate,112) 
	SELECT @vchFromSilo = Value1, @vchToSilo = Value2 from tblControl 
		WHERE [key] = 'RoastCoffeeInventoryShortage' and Subkey = 'SiloRange' and facility = @chrFacility
	--SET @vchSQLStmt = 'Select ELEMENT ,SUM(CONTENTS/10) as WgtRoastedCoffee from SILOS ' + 
	--		' WHERE id > ' + @vchFromSilo + ' and CELKNR > ' + @vchToSilo + ' GROUP BY ELEMENT';

	SET @vchSQLStmt = 
	'WITH cteWgtToGrind (Blend, WgtToGrind) ' +
	'AS ' +
	'( ' +
	'SELECT Blend, Sum(ScheduledWgt) - Sum(ActualWgt) as WgtToGrind FROM ( ' +
		'SELECT tGS.GSCBlend as Blend,tGS.GSCACTI as ScheduleID, SUM(tGS.GSCQTYI)/Count(*) as scheduledWgt, ' +
			'SUM(ISNULL(tGL.ActualWgt,0)) as ActualWgt ' +
			'FROM dbo.tblGrindingSchedule tGS ' +
			'left outer join dbo.tblGrindingLog tGL On GSCACTI = ScheduleID ' +
			'WHERE tGS.GSCFAC =''' + @chrFacility + ''' AND tGS.GSCPDTE = ' + @vchProductionDate + ' AND tGS.GSCSFT# <= ' + cast(@intShift as char(1)) + ' AND tGS.GSCBLEND <> '''' ' +
				'AND NOT (isnull(tGL.Status,'''') = ''Done'' and isnull(tGL.Status, '''') = ''Dropped'') ' +
			'Group BY tGS.GSCBlend,tGS.GSCACTI) as tGrindingWgt ' +
	'Group BY Blend ' +
	') ' +
     'SELECT T1.Blend, WgtToGrind, ISNULL(WgtRoastedCoffee,0) as WgtRoastedCoffee, ISNULL(WgtRoastedCoffee,0) - WgtToGrind as WgtShort FROM cteWgtToGrind T1 ' +
		'LEFT OUTER JOIN (	' +
	'SELECT ELEMENT as Blend, WgtRoastedCoffee ' +
			'FROM Openquery(PROBAT,''Select ELEMENT ,SUM(CONTENTS/10) as WgtRoastedCoffee from SILOS ' + 
			' WHERE id > ' + @vchFromSilo + ' and CELKNR > ' + @vchToSilo + ' GROUP BY ELEMENT'')) T2 ' +
	 'ON T1.Blend = T2.Blend ' +
	 'Order By T1.Blend'

	print @vchSQLStmt
	exec (@vchSQLStmt)
END

GO

