
-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 17, 2007
-- Description:	Ground Cofee Packaging Activity Selection
-- IC#5854:		May. 15, 2014	Bong Lee
--				BarBlend Issue - duplicated result records.
-- =============================================
CREATE PROCEDURE [dbo].[LTsp_SltedPkgActivitySel]
	-- Add the parameters for the stored procedure here
	@vchProcEnv	varchar(10),
	@vchFacility as varchar(3) = NULL,
	@vchGCLotID as varchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @vchServerName as varchar(50);
--IC#5854	DECLARE @vchSQLStmt  varchar(650);
--IC#5854	DECLARE @vchServerSQLStmt  varchar(600);
	DECLARE @vchSQLStmt  varchar(1250);				--IC#5854
	DECLARE @vchServerSQLStmt  varchar(1200);		--IC#5854
	DECLARE @vchUserLib varchar(10);
	DECLARE @vchOriginalLib varchar(10);
	DECLARE @vchFromRow varchar(10);
	DECLARE @vchToRow varchar(10);
	
    -- Insert statements for procedure here
	If @vchProcEnv = 'PRD'
	BEGIN
		Select @vchServerName = value1 From tblControl Where [key] = 'iSeriesNames'
		Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibPRD'
	END
	ELSE
	BEGIN
		If @vchProcEnv = 'UA'
		BEGIN
			Select @vchServerName = value2 From tblControl Where [key] = 'iSeriesNames'
			Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibUA'
		END
		ELSE
		BEGIN
			Select @vchServerName = value2 From tblControl Where [key] = 'iSeriesNames'
			Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibDev'
		END
	END 

--IC#5854	Set @vchServerSQLStmt = 'SELECT SPCBLND as CmptBlend, SPFBLND as FGBlend, SPGRND as Grind, SPLINE as PkgLine, SPSORD as ShopOrder, ' + 
	Set @vchServerSQLStmt = 'SELECT CASE WHEN SPFBLND ='''''''' THEN '''''''' ELSE SPCBLND END as CmptBlend, SPFBLND as FGBlend, SPGRND as Grind, SPLINE as PkgLine, SPSORD as ShopOrder, ' +	--IC#5854
			'SPBLPRD as ItemNumber, SPPALID as PalletID, SPPCDT as PalletDateCreated, INT(SPPCTM/100) as PalletTimeCreated, ' + 
--IC#5854	'INT(Round(SPPQTY,0)) as PalletQty, INT(Round(SPAQTY,0)) as AdjQty, INT(Round(SPPQTY + SPAQTY,0)) as NetQty, INT(Round(SPWGHT,0)) as Weight, ' + 
			'INT(Round(SPPQTY,0)) as PalletQty, INT(Round(SPAQTY,0)) as AdjQty, INT(Round(SPPQTY + SPAQTY,0)) as NetQty, INT(Round(SUM(SPWGHT),0)) as Weight, ' + 
			'INT(Round(IMNNWU * INT(Round(SPPQTY + SPAQTY,0)),0)) as LblWeight ' +
			'FROM ' + @vchUserLib + '.FSPA$ T1 LEFT OUTER JOIN ' + @vchOriginalLib + '.IIM T2 ON T1.SPBLPRD = IPROD '
			
	/*IC#5854 DEL Start
	IF @vchFacility IS NULL
		SET @vchServerSQLStmt = @vchServerSQLStmt  + 'WHERE SPGCLOT = ''''' + @vchGCLotID + ''''' ORDER BY SPPCDT, SPPCTM'	ELSE		SET @vchServerSQLStmt = @vchServerSQLStmt  + 'WHERE ((SPFAC =''''' + @vchFacility + ''''') AND (SPGCLOT = ''''' + @vchGCLotID + ''''')) ORDER BY SPPCDT, SPPCTM'	IC#5854 DEL Stop */

	--IC#5854 ADD Start
	IF @vchFacility IS NULL
		SET @vchServerSQLStmt = @vchServerSQLStmt  + 'WHERE SPGCLOT = ''''' + @vchGCLotID + '''''' 	ELSE		SET @vchServerSQLStmt = @vchServerSQLStmt  + 'WHERE ((SPFAC =''''' + @vchFacility + ''''') AND (SPGCLOT = ''''' + @vchGCLotID + ''''')) '	SET @vchServerSQLStmt = @vchServerSQLStmt + 'GROUP BY CASE WHEN SPFBLND ='''''''' THEN '''''''' ELSE SPCBLND END, SPFBLND, SPGRND, SPLINE, SPSORD, SPBLPRD, SPPALID, SPPCDT, ' +
			'INT(SPPCTM/100), INT(Round(SPPQTY,0)), INT(Round(SPAQTY,0)), INT(Round(SPPQTY + SPAQTY,0)),  INT(Round(SPPQTY + SPAQTY,0)), INT(Round(IMNNWU * INT(Round(SPPQTY + SPAQTY,0)),0)) '
	SET @vchServerSQLStmt = @vchServerSQLStmt + ' ORDER BY SPPCDT, INT(SPPCTM/100)'	--IC#5854 ADD Stop		Set @vchSQLStmt = 'Select * from ' + 			'OPENQUERY(' + @vchServerName + ',''' + RTRIM(@vchServerSQLStmt) + ''' ) '
PRINT @vchSQLStmt
	EXEC (@vchSQLStmt)


END

GO

