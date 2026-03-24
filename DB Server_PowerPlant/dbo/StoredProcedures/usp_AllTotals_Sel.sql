
-- =============================================
-- Author:		Bong Lee
-- Create date: 2010-4-21
-- Description:	Read KRONOS(HR. Application)
-- =============================================
CREATE PROCEDURE [dbo].[usp_AllTotals_Sel]
	@dteFromTime as datetime,
	@dteToTime as datetime,
	@vchFacility as varchar(3),
	@vchCountry as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @tblPC Table (
		PAYCODE varchar(50)
	)

	SET @vchCountry = UPPER(@vchCountry)

	INSERT INTO @tblPC VALUES(@vchCountry + '-REG');
	INSERT INTO @tblPC VALUES(@vchCountry + '-REG2');
	INSERT INTO @tblPC VALUES(@vchCountry + '-REG3');
	INSERT INTO @tblPC VALUES(@vchCountry + '-OT');
	INSERT INTO @tblPC VALUES(@vchCountry + '-DT');

	SELECT @vchCountry = @vchCountry + '%'

	;WITH cteWC AS (
	SELECT DISTINCT LEFT(EquipmentID,4) as WorkCenter from dbo.tblEquipment where facility = @vchFacility
	UNION
	SELECT DISTINCT LEFT(EquipmentID,4) as WorkCenter from MPFWPP01.PowerPlant_Prd.dbo.tblEquipment where facility = @vchFacility
	UNION
	SELECT DISTINCT LEFT(EquipmentID,4) as WorkCenter from MPAJPP01.PowerPlant_Prd.dbo.tblEquipment where facility = @vchFacility
	)

	SELECT vAT.APPLYDATE, vAT.WFCLABORLEVELNAME3 AS [Work Center], vAT.WFCLABORLEVELNAME5 AS Shift, vAT.PAYCODENAME, [WFCTIMEINSECONDS]/36/100 AS [Labor Hours Worked], vAT.PERSONFULLNAME, vAT.PERSONNUM, vAT.WFCLABORLEVELDSC3, vAT.WFCLABORLEVELDSC5,
		   '->' as EndOfRecord, vAT.*
	FROM [MPHOSQLPROD01\APPS].MPKRONOS.dbo.VP_ALLTOTALS vAT
	INNER JOIN cteWC
	On vAT.WFCLABORLEVELNAME3 = cteWC.WorkCenter
	INNER JOIN @tblPC tPC
	ON vAT.PAYCODENAME = tPC.PAYCODE
	WHERE vAT.APPLYDATE BETWEEN @dteFromTime AND @dteToTime 
		--AND ((vAT.WFCLABORLEVELNAME3)="3711" Or (vAT.WFCLABORLEVELNAME3)="3712" Or (vAT.WFCLABORLEVELNAME3)="3713" Or (vAT.WFCLABORLEVELNAME3)="3714" Or (vAT.WFCLABORLEVELNAME3)="3715" Or (vAT.WFCLABORLEVELNAME3)="3716" Or (vAT.WFCLABORLEVELNAME3)="3717") 
		--AND ((vAT.WFCLABORLEVELNAME5)="3") 
		--AND ((vAT.PAYCODENAME)="US-REG" Or (vAT.PAYCODENAME)="US-REG2" Or (vAT.PAYCODENAME)="US-REG3" Or (vAT.PAYCODENAME)="US-OT" Or (vAT.PAYCODENAME)="US-DT") 
		--AND CASE WHEN @vchCountry = 'US' THEN vAT.PAYCODENAME LIKE 'US%' ELSE vAT.PAYCODENAME NOT LIKE 'US%' END
		AND vAT.PAYCODENAME LIKE @vchCountry 
--		AND vAT.WFCLABORLEVELNAME1='F50' 
		AND vAT.PAYCODETYPE='P'
	ORDER BY vAT.WFCLABORLEVELNAME3, vAT.WFCLABORLEVELNAME5, vAT.PAYCODENAME;
END

GO

