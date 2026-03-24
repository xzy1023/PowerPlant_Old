

-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 17, 2007
-- Description:	Ground Cofee Packaging Activity Selection
-- =============================================
CREATE PROCEDURE [dbo].[LTsp_SelPkgActivity]
	-- Add the parameters for the stored procedure here
	@vchProcEnv	varchar(10),
	@vchFacility as varchar(3),
	@chrFromTime as char(14),
	@chrToTime as char(14),
	@chrPkgLine char(10) = '', 
	@vchShopOrder varchar(10) = '',
	@vchBlend varchar(6) = '',
	@vchGrind varchar(6) = '',
	@intStartRowIndex	int,
	@intMaximumRows		int,
	@vchSortExpression	varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @vchServerName as varchar(50);
	DECLARE @vchServerSQLStmt  varchar(1500);
	DECLARE @vchSQLStmt  varchar(1700);
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

	IF (LEN(@vchSortExpression) = 0) or (@vchSortExpression = 'PalCrtDate')
		SET @vchSortExpression = 'IPPCDT,IPPCTM'
	ELSE 
		IF @vchSortExpression = 'CmpntBlend'
			SET @vchSortExpression = 'SBBLEND'
		ELSE IF @vchSortExpression = 'FGBlend'
				SET @vchSortExpression = 'IPBLND'
		ELSE IF @vchSortExpression = 'Grind'
				SET @vchSortExpression = 'IPGRND'
		ELSE IF @vchSortExpression = 'Line'
				SET @vchSortExpression = 'IPMACH'
		ELSE IF @vchSortExpression = 'ShopOrder'
				SET @vchSortExpression = 'IPREF'
		ELSE IF @vchSortExpression = 'Item'
				SET @vchSortExpression = 'IPPROD'

	Set @vchServerSQLStmt = 'Select IFNULL(T3.SBBLEND,'''' '''') as SBBLEND, IPBLND, IPGRND, IPMACH, IPREF, IPPROD, IPPAL, IPPCDT, IPPCTM, IPPQTY, IFNULL(T1.TQTY,0) + IFNULL(T2.TQTY,0) as AdjQty, ' + 
		'IPPQTY + IFNULL(T1.TQTY,0) + IFNULL(T2.TQTY,0) as NetQty, ' + 
		'Round((IPPQTY + IFNULL(T1.TQTY,0) + IFNULL(T2.TQTY,0)) * IFNULL(T3.SBQTY,0) ,0) as Weight, ' +
		'Round((IPPQTY + IFNULL(T1.TQTY,0) + IFNULL(T2.TQTY,0)) * IFNULL(T4.IMNNWU,0) ,0) as LblWeight ' +
		'FROM ' + @vchUserLib + '.iipi$l05 ' +
		'Left outer join ' + @vchUserLib + '.FSPA$ as T5 on IPPAL = T5.SPPALID ' +		
		'Left outer join ' + @vchUserLib + '.FSOBC$L01 as T3 on IPREF = T3.SBSORD ' +
		'Left outer join ' + @vchOriginalLib + '.iim as T4 on IPPROD = T4.IPROD ' +
		'Left outer join (Select TQTY, TLOT From ' + @vchOriginalLib + '.ithl59 Where TTYPE = ''''PR''''  and tlot <> '''''''' and THPRSH <> 0) as T1 on iplot = T1.tlot ' +
		'Left outer join (Select TQTY, TLOT From ' + @vchUserLib + '.ITH$AL03 Where TTYPE = ''''PR''''  and tlot <> '''''''' and THPRSH <> 0) as T2 on iplot = T2.tlot ' +
		'Where T5.SPPALID IS NULL AND IPFAC = ''''' + @vchFacility + ''''' AND (IPPCDT >= ' + LEFT(@chrFromTime,8) + 
		') AND ((IPPCDT * 1000000 + IPPCTM) Between ' +  @chrFromTime + ' AND ' + @chrToTime + ') And (T4.IITYP = ''''D'''' OR T4.IITYP = ''''C'''')'

	If RTRIM(@vchBlend) <> '' 
	Begin
		Set @vchServerSQLStmt = RTRIM(@vchServerSQLStmt) + ' AND T3.SBBLEND = ''''' + @vchBlend + ''''''
		If RTRIM(@vchGrind) <> '*' 
			Set @vchServerSQLStmt = RTRIM(@vchServerSQLStmt) + ' AND IPGRND = ''''' + @vchGrind + ''''''
	End
	
	Set @vchServerSQLStmt = RTRIM(@vchServerSQLStmt) + ' AND IPID = ''''IP'''''

	If RTRIM(@chrPkgLine) <> '' 
		Set @vchServerSQLStmt = RTRIM(@vchServerSQLStmt) + ' AND IPMACH = ''''' + @chrPkgLine + ''''''

	If RTRIM(@vchShopOrder) <> '' 
		Set @vchServerSQLStmt = RTRIM(@vchServerSQLStmt) + ' AND IPREF = ''''' + @vchShopOrder + ''''''

	Set @vchServerSQLStmt = RTRIM(@vchServerSQLStmt) + ' AND IPSTS = ''''90'''' AND IPLSTS = ''''90'''' Order by IPPCDT,IPPCTM'

PRINT 'vchServerSQLStmt = ' + @vchServerSQLStmt
--	Set @vchSQLStmt = 'SELECT Case when PAlletID IS NULL Then '''' Else ''1'' End as Status, Datepart(Second,getdate()) as timestamp , T2.* From ' +
--			'OPENQUERY(' + @vchServerName + ',''' + RTRIM(@vchServerSQLStmt) + ''' ) As T2 ' +
--			'Left Outer Join dbo.tblGCPkgActivity on T2.IPPAL = PalletID'
	
	SET @vchFromRow = CAST(@intStartRowIndex + 1 as varchar(10))
	SET @vchToRow = CAST(@intStartRowIndex + @intMaximumRows as varchar(10))
	
--	Set @vchSQLStmt = 'Select T4.* from (' +
--			'SELECT  T2.*, ROW_NUMBER() OVER(ORDER BY ' + @vchSortExpression + ') as RowNum From ' +
--			'OPENQUERY(' + @vchServerName + ',''' + RTRIM(@vchServerSQLStmt) + ''' ) As T2 ' +
--			'Left Outer Join dbo.tblGCPkgActivity as T3 on T2.IPPAL = T3.PalletID AND T2.SBBLEND = T3.CmptBlend Where T3.PalletID is NULL ' +
--			') as T4 WHERE RowNum BETWEEN ' + @vchFromRow + ' AND ' + @vchToRow
	Set @vchSQLStmt = 'Select T1.* from (' +
			'SELECT  *, ROW_NUMBER() OVER(ORDER BY ' + @vchSortExpression + ') as RowNum From ' +
			'OPENQUERY(' + @vchServerName + ',''' + RTRIM(@vchServerSQLStmt) + ''' ) ' +
			') as T1 WHERE RowNum BETWEEN ' + @vchFromRow + ' AND ' + @vchToRow
PRINT 'vchSQLStmt = ' + @vchSQLStmt
	EXEC (@vchSQLStmt)
END

GO

