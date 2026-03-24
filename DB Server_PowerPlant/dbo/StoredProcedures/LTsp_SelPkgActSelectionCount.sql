-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 17, 2007
-- Description:	Ground Cofee Packaging Activity Selection
-- =============================================
CREATE PROCEDURE [dbo].[LTsp_SelPkgActSelectionCount]
	-- Add the parameters for the stored procedure here
	@vchProcEnv	varchar(10),
	@vchFacility as varchar(3),
	@chrFromTime as char(14),
	@chrToTime as char(14),
	@chrPkgLine char(10) = '', 
	@vchShopOrder varchar(10) = '',
	@vchBlend varchar(6) = '',
	@vchGrind varchar(6) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @vchServerName as varchar(50);
	DECLARE @vchServerSQLStmt  varchar(1200);
	DECLARE @vchSQLStmt  varchar(1400);
	DECLARE @vchUserLib varchar(10);
	DECLARE @vchOriginalLib varchar(10);
	
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

	Set @vchServerSQLStmt = 'Select count(*) from ' + @vchUserLib + '.iipi$l05 ' +		'Left outer join ' + @vchUserLib + '.FSPA$ as T5 on IPPAL = T5.SPPALID ' +					'Left outer join ' + @vchUserLib + '.FSOBC$L01 as T1 on IPREF = T1.SBSORD ' +		'Where T5.SPPALID IS NULL AND IPFAC = ''''' + @vchFacility + ''''' AND (IPPCDT >= ' + LEFT(@chrFromTime,8) + ') AND ((IPPCDT * 1000000 + IPPCTM) Between ' +  @chrFromTime + ' AND ' + @chrToTime + ') ' 
	print ' Blend= ' + RTRIM(@vchBlend)
	If RTRIM(@vchBlend) <> '' 
	Begin
		Set @vchServerSQLStmt = RTRIM(@vchServerSQLStmt) + ' AND T1.SBBLEND = ''''' + @vchBlend + ''''''
		If RTRIM(@vchGrind) <> '*' 
			Set @vchServerSQLStmt = RTRIM(@vchServerSQLStmt) + ' AND IPGRND = ''''' + @vchGrind + ''''''
	End

	Set @vchServerSQLStmt = RTRIM(@vchServerSQLStmt) + ' AND IPID = ''''IP'''''

	If RTRIM(@chrPkgLine) <> '' 
		Set @vchServerSQLStmt = RTRIM(@vchServerSQLStmt) + ' AND IPMACH = ''''' + @chrPkgLine + ''''''

	If RTRIM(@vchShopOrder) <> '' 
		Set @vchServerSQLStmt = RTRIM(@vchServerSQLStmt) + ' AND IPREF = ''''' + @vchShopOrder + ''''''

	Set @vchServerSQLStmt = RTRIM(@vchServerSQLStmt) + ' AND IPSTS = ''''90'''' AND IPLSTS = ''''90'''' '

PRINT @vchServerSQLStmt
	Set @vchSQLStmt = 'SELECT * From ' +			'OPENQUERY(' + @vchServerName + ',''' + RTRIM(@vchServerSQLStmt) + ''' )  '
	PRINT @vchSQLStmt
	EXEC (@vchSQLStmt)
END

GO

