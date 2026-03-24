
-- =============================================
-- Author:		Bong Lee
-- Create date: 10/4/2007
-- Description:	Customer Order Shipments
-- WO#xxx:		May. 15, 2014	Bong Lee
--				BarBlend Issue - duplicated result records.
-- =============================================
CREATE PROCEDURE [dbo].[LTsp_COShipments] 
	@vchProcEnv	varchar(10),
	@vchGCLot varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	DECLARE @vchServerName as varchar(50);
--WO#xxx	DECLARE @vchServerSQLStmt varchar(1000);
--WO#xxx	DECLARE @vchSQLStmt varchar(1200);
	DECLARE @vchServerSQLStmt varchar(1400);	--WO#xxx
	DECLARE @vchSQLStmt varchar(1600);			--WO#xxx
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

		Set @vchServerSQLStmt = 'SELECT T2.ICUST, T2.IWHS, T2.IORD, ' +
			'CASE T4.sidtsp WHEN 0 THEN 0 ELSE CAST(T2.IQALL as int) END as IQALL, ' + 
			'T3.hname,T3.chcphn,T3.hatn, T1.ipprod, T1.ippal, T1.iplot,T4.sidtsp , ' +
			'CASE T4.sidtsp WHEN 0 THEN CAST(T2.IQALL as int) ELSE 0 END as AdjQty, ' +
--WO#xxx	'CAST(Round(CAST(T2.IQALL as int) * T6.SBQTY,0) as int) as Weight, ' +
			'SUM(CAST(Round(CAST(T2.IQALL as int) * T6.SBQTY,0) as int)) as Weight, ' +		--WO#xxx
			'CAST(Round(CAST(T2.IQALL as int) * T5.IMNNWU,0) as int) as LabelWeight ' +
			'From ' + @vchUserLib + '.FSPA$ T0 ' +	
			'Left outer join ' + @vchUserLib + '.iipi$ T1 on T0.SPPALID = T1.ippal ' +
			'left outer join ' + @vchOriginalLib + '.eil T2 ' +
			'on T1.iplot = T2.ilot ' +
			'left outer join ' + @vchOriginalLib + '.ech T3 ' +
			'on T2.iord = T3.hord ' +
			'left outer join ' + @vchOriginalLib + '.sih T4 ' +
			'on T2.iinvn = T4.siinvn and T2.iord = T4.siord ' +
			'LEFT OUTER JOIN ' + @vchOriginalLib + '.iim T5 on T1.ipprod = T5.iprod ' +
			'left outer join ' + @vchUserLib + '.FSOBC$ T6 ' +
			'on T0.SPSORD = T6.SBSORD and T0.SPCBLND = T6.SBBLEND ' +
			'WHERE T0.SPGCLOT =''''' + @vchGCLot + ''''' AND T3.HDTYP = ''''1'''' ' +
-- WO#xxx Add Start
			'GROUP BY T2.ICUST, T2.IWHS, T2.IORD, CASE T4.sidtsp WHEN 0 THEN 0 ELSE CAST(T2.IQALL as int) END, ' +
			'T3.hname,T3.chcphn,T3.hatn, T1.ipprod, T1.ippal, T1.iplot,T4.sidtsp, ' +
			'CASE T4.sidtsp WHEN 0 THEN CAST(T2.IQALL as int) ELSE 0 END, ' +
			'CAST(Round(CAST(T2.IQALL as int) * T5.IMNNWU,0) as int) ' +
-- WO#xxx Add Stop
			'ORDER BY T2.ICUST,T1.iplot,T2.IORD'
		Set @vchSQLStmt = 'SELECT * From ' +			'OPENQUERY(' + @vchServerName + ',''' + RTRIM(@vchServerSQLStmt) + ''' ) ' 
	
print @vchServerSQLStmt
print @vchSQLStmt
	EXEC (@vchSQLStmt)
END

GO

