






-- =============================================
-- Author:		Bong Lee
-- Create date: 10/17/2007
-- Description:	Inventory for Items Made from Green Coffee Lot
-- WO#xxx:		May. 15, 2014	Bong Lee
--				BarBlend Issue - duplicated result records.
-- =============================================
CREATE PROCEDURE [dbo].[LTsp_GCLotInventory_Test] 
	@vchProcEnv	varchar(10),
	@vchGCLot varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @vchServerName as varchar(50);
	DECLARE @vchServerSQLStmt  varchar(1900);
	DECLARE @vchSQLStmt  varchar(2000);
	DECLARE @vchUserLib varchar(10);
	DECLARE @vchOriginalLib varchar(10);

    -- Check the processing environment. Is production or UA?
	If @vchProcEnv = 'PRD'
	BEGIN
		Select @vchServerName = value1 From tblControl Where [key] = 'iSeriesNames'
		Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibPrd'
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
			Set @vchServerSQLStmt = 'SELECT Facility, whse, location, ipprod, iplot, PalletID, OnhandQty, IntransitQty, Weight FROM (' +		--WO#xxx
			'SELECT T4.wmfac as Facility,T2.lwhs as whse, T1.ipprod, T3.idesc, T2.lloc as Location, T1.iplot, ' +							--WO#xxx
--WO#xxx	Set @vchServerSQLStmt = 'SELECT T4.wmfac as Facility,T2.lwhs as whse, T1.ipprod, T3.idesc, T2.lloc as Location, T1.iplot, ' +  
			'T0.SPPALID as PalletID, CAST(T2.lopb-T2.lissu +T2.ladju+T2.lrct as int) as OnhandQty,0 as IntransitQty, ' +
			'CAST(Round((T2.lopb-T2.lissu +T2.ladju+T2.lrct)* T6.CaseQty,0) as int) as Weight ' +
			'From ' + @vchUserLib + '.FSPA$ T0 ' +				
			'Left outer join ' + @vchUserLib + '.iipi$ T1 on T0.SPPALID = T1.ippal ' +
			'LEFT OUTER JOIN ' + @vchOriginalLib + '.ilil08 T2 on T1.iplot = T2.llot ' +
			'LEFT OUTER JOIN ' + @vchOriginalLib + '.iim T3 on T1.ipprod = T3.iprod ' +
			'LEFT OUTER JOIN ' + @vchOriginalLib + '.iwm T4 on T2.lwhs = T4.lwhs ' +
'Left outer join (Select SBSORD, Sum(SBQTY) as CaseQty From ' + @vchUserLib + '.FSOBC$L01 Group by SBSORD) as T6 on SPSORD = T6.SBSORD ' +
			'WHERE T0.SPGCLOT =''''' + @vchGCLot + ''''' AND T2.llot is not null and ' +
			'T2.lopb-T2.lissu +T2.ladju+T2.lrct <> 0 ' +
			'Union All ' +
			'SELECT T5.wmfac as Facility,T2.iwhs as whse ,T1.ipprod, T4.idesc,''''''''as Location, T1.iplot, ' +          			'T0.SPPALID as PalletID, 0 as OnHandQty, CAST((T2.iqall-T2.iqrec) as int) as IntransitQty, ' +
			'CAST(Round((T2.iqall-T2.iqrec)*T6.CaseQty,0) as int) as weight ' + 			'From ' + @vchUserLib + '.FSPA$ T0 ' +	                       			'LEFT OUTER JOIN ' + @vchUserLib + '.iipi$ T1 on T0.SPPALID = T1.ippal ' +                                                    			'LEFT OUTER JOIN ' + @vchOriginalLib + '.eil T2 on T1.iplot = T2.ilot ' +                    			'LEFT OUTER JOIN ' + @vchOriginalLib + '.ech T3 on T2.iord = T3.hord ' +                     			'LEFT OUTER JOIN ' + @vchOriginalLib + '.iim T4 on T1.ipprod = T4.iprod ' +
			'LEFT OUTER JOIN ' + @vchOriginalLib + '.iwm T5 on T2.iwhs = T5.lwhs ' +
'Left outer join (Select SBSORD, Sum(SBQTY) as CaseQty From ' + @vchUserLib + '.FSOBC$L01 Group by SBSORD) as T6 on SPSORD = T6.SBSORD ' +
			'WHERE T0.SPGCLOT =''''' + @vchGCLot + ''''' AND T3.HDTYP = ''''9'''' AND T2.iqall-T2.iqrec <> 0 ' +
			'and T1.iplot <> '''''''' ' +
			-- WO#xxx Add Start
			')  tResult ' +
			'GROUP BY Facility, whse, location, ipprod, iplot, PalletID, OnhandQty, IntransitQty, Weight ' +		
			-- WO#xxx Add Stop
			'Order by Facility, whse,location, ipprod, iplot'

		Set @vchSQLStmt = 'SELECT * From ' +			'OPENQUERY(' + @vchServerName + ',''' + RTRIM(@vchServerSQLStmt) + ''' ) ' 

print @vchSQLStmt
	EXEC (@vchSQLStmt)
END

GO

