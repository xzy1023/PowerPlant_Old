
-- =============================================
-- Author:		Bong Lee
-- Create date: Oct. 24 2006
-- Description:	Convert date by date code
-- #0001:		Jul. 12, 2007	Bong Lee	
-- Description:	Add new date code 36 
--				(it requires to add a new input parameter, Shift)
-- #0002:		Aug. 14, 2007	Bong Lee	
-- Description:	Add new date code 37 
--				fix code 22
-- #0003:		Mar. 11, 2008	Bong Lee	
-- Description:	Add new date code 38 
-- #0004:		Mar. 27, 2008	Bong Lee	
-- Description:	Add new date code 39 (MM-DD-YYYY) 
-- #0005:		Aug. 14, 2008	Bong Lee	
-- Description:	For date code 18, make the location character is server
--				specific. 
-- #0006:		Nov. 07, 2008	Bong Lee	
-- Description:	Add date code 40 - YDDD (where Y is alpha year of last digit of the year. 0=A, 1=B ...9=K etc.)
--				e.g. I312 for Nov 7, 2008. 
--				It is similar to existing code 26 -  DDDY  
-- #0007:		Jan. 07, 2009	Bong Lee	
-- Description:	Modify date code 26 - DDDY 
--				It should be based on the result of (Year - 2000 ) modulo 26. 
--				So A can be 2000, 2026, 2052 ..(2000 + 26 * n) where n=0,1,2.3...n .etc. 
--				e.g. 007A for Jan7,2000; 007K for Jan 7, 2010
--				It is different from the existing code 40 -  YDDD 
-- #0008:		March. 18, 2009	Bong Lee	
-- Description:	Swap date code 01 and 09, so 01 becomes YYMMDD and 09 becomes YMMDD
-- #0009:		Jan. 18, 2009	Bong Lee	
-- Description:	Error on code 37, the substring starting position is incorrect. 
-- WO#380:		Dec.08 2010 Bong Lee
-- Description:	Add new date code 41 for YYYY-MM-DD eg. 2010-12-08
-- WO#xxx:		Nov.11 2013 Bong Lee
-- Description:	Add new date code 42 for YYYY MM DD eg. 2013 NO 11 
-- WO#1280:		Jul.10 2014 Bong Lee
-- Description:	Add new date code 43 for YYYYMMM    eg. 2013NOV 
-- MOD20160506:	May.06 2016 Bong Lee
-- Description:	Add new date code 44 for DDMMMYYYY  eg. 10MAR2016 
-- MOD20160720:	Jul.20 2016 Bong Lee
-- Description:	Add new date code 45 for MM DD YYYY eg. 07 20 2016 
-- WO#4242:		Nov.29 2016 Bong Lee
-- Description:	Add new date code 46 for DD-MM-YYYY eg. 28-OC-2016 
-- WO#5641:		May.19 2017 Bong Lee
-- Description:	Add new date code 47 for DD MM YYYY eg. 19 MA 2017 
-- CHG#57		Apr.09 2021 Zhiyuan Xiao
-- Description:	Add new date code 48 for MMYY eg. AL21
-- ICDT #46482	Jun.14 2024 Zhiyuan Xiao
-- Description:	Add new date code 49 for Y DDD Where Y = last digit of current year & DDD = 3-digit Julian date
--				e.g. 4 166 for Jun 14, 2024.
-- =============================================
CREATE FUNCTION [dbo].[fnConvertDate]
(
	-- Add the parameters for the function here
	@dteDateMDY DateTime,
	@chrDateFmtCode char(2),
	@chrFacility char(3) = NULL,
	@intShift tinyint = 0		--Mod#01
)
RETURNS varchar(20)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @vchResult varchar(20)
	DECLARE @chrYYMMDD varchar(6)
	DECLARE @chrYYYYMMDD varchar(8)	--Mod#03
	DECLARE @chrDD_MMM_YY char(9)
	DECLARE @chrDD_MMM_YYYY char(11)
	DECLARE @chrDD_MM_YYYY char(10)		-- WO#4242	
	DECLARE @chr2CharMthWord char(2)
	DECLARE @chrShiftNo varchar(2)	--Mod#01
	
-- Del#0008	IF @chrDateFmtCode = '01'	-- YMMDD
	IF @chrDateFmtCode = '01'	-- YYMMDD  -- Add#0008
		BEGIN
			SET @vchResult  = CONVERT(varchar(6),@dteDateMDY,12) --Add#0008
--Del#0008	SET @vchResult = SUBSTRING(CONVERT(varchar(6),@dteDateMDY,12),2,5)
		END
	ELSE
		IF @chrDateFmtCode = '02'	-- MMDDY
		BEGIN
			SET @chrYYMMDD = CONVERT(varchar(6),@dteDateMDY,12)
			SET @vchResult = SUBSTRING(@chrYYMMDD,3,4) + SUBSTRING(@chrYYMMDD,2,1)
		END
	ELSE
		IF @chrDateFmtCode = '03'	-- Julian DDDY	
		BEGIN
			SET @vchResult = dbo.fnFillLeadingZeros (3,CAST(DATEPART(dayofyear,@dteDateMDY) as VARCHAR(3))) + SUBSTRING(CAST(DATEPART(year,@dteDateMDY) as CHAR(4)),4,1)
		END
	ELSE
-- Del#0002		IF @chrDateFmtCode = '04' OR @chrDateFmtCode = '22'	-- Julian YDDD
		IF @chrDateFmtCode = '04'	-- Julian YDDD		-- <Add#0001>
		BEGIN
			SET @vchResult = SUBSTRING(CAST(DATEPART(year,@dteDateMDY) as CHAR(4)),4,1) + dbo.fnFillLeadingZeros (3,CAST(DATEPART(dayofyear,@dteDateMDY) as VARCHAR(3)))  
		END
	--ELSE
	--	IF @chrDateFmtCode = '05'	-- YWMDDM where W is for Fort Worth for Sysco label
	--	BEGIN
	--		SET @chrYYMMDD = CONVERT(varchar(6),@dteDateMDY,12)
	--		SET @vchResult = SUBSTRING(@chrYYMMDD,2,1) + 'W' + SUBSTRING(@chrYYMMDD,3,1) + SUBSTRING(@chrYYMMDD,5,2) + SUBSTRING(@chrYYMMDD,4,1)
	--	END
	ELSE
		IF @chrDateFmtCode = '06'	-- MMMDDYY
		BEGIN
			SET @chrDD_MMM_YY = UPPER(CONVERT(char(9),@dteDateMDY,6))
			SET @vchResult = SUBSTRING(@chrDD_MMM_YY,4,3) + SUBSTRING(@chrDD_MMM_YY,1,2) + SUBSTRING(@chrDD_MMM_YY,8,2)
		END
	ELSE
		IF @chrDateFmtCode = '07'	-- MMM DD YYYY
		BEGIN
			SET @chrDD_MMM_YYYY = UPPER(CONVERT(char(11),@dteDateMDY,106))
			SET @vchResult = SUBSTRING(@chrDD_MMM_YYYY,4,3) + ' ' + SUBSTRING(@chrDD_MMM_YYYY,1,2) + ' ' + SUBSTRING(@chrDD_MMM_YYYY,8,4)
		END
	ELSE
		IF @chrDateFmtCode = '08'	-- MMDDYY
		BEGIN
			SET @chrYYMMDD = CONVERT(varchar(6),@dteDateMDY,12)
			SET @vchResult = SUBSTRING(@chrYYMMDD,3,4) + SUBSTRING(@chrYYMMDD,1,2)
		END
	ELSE
--Del#0008 IF @chrDateFmtCode = '09'	-- YYMMDD
/*Add#0008*/ IF @chrDateFmtCode = '09'	-- YMMDD	
		BEGIN
--Del#0008	SET @vchResult  = CONVERT(varchar(6),@dteDateMDY,12)
			SET @vchResult = SUBSTRING(CONVERT(varchar(6),@dteDateMDY,12),2,5) --Add#0008
		END
	ELSE
		IF @chrDateFmtCode = '10'	-- MMM DD YY
		BEGIN
			SET @chrDD_MMM_YY = UPPER(CONVERT(char(9),@dteDateMDY,6))
			SET @vchResult = SUBSTRING(@chrDD_MMM_YY,4,3) + ' ' + SUBSTRING(@chrDD_MMM_YY,1,2) + ' ' + SUBSTRING(@chrDD_MMM_YY,8,2)
		END
	ELSE
		IF @chrDateFmtCode = '11'	-- DD MMM YY
		BEGIN
			SET @vchResult = UPPER(CONVERT(char(9),@dteDateMDY,6))
		END
	ELSE
		IF @chrDateFmtCode = '12'	-- DDMMYY
		BEGIN
			SET @chrYYMMDD = CONVERT(varchar(6),@dteDateMDY,12)
			SET @vchResult = SUBSTRING(@chrYYMMDD,5,2) + SUBSTRING(@chrYYMMDD,3,2) + SUBSTRING(@chrYYMMDD,1,2)
		END
	ELSE
		IF @chrDateFmtCode = '13'	-- DDMMMYY
		BEGIN
			SET @chrDD_MMM_YY  = UPPER(CONVERT(char(9),@dteDateMDY,6))
			SET @vchResult = REPLACE(@chrDD_MMM_YY,' ','')
		END
	ELSE
		IF @chrDateFmtCode = '14'	-- MMYY
		BEGIN
			SET @chrYYMMDD = CONVERT(varchar(6),@dteDateMDY,12)
			SET @vchResult =  SUBSTRING(@chrYYMMDD,3,2) + SUBSTRING(@chrYYMMDD,1,2)
		END
	ELSE
		IF @chrDateFmtCode = '15'	-- MM DD YY
		BEGIN
			SET @chrYYMMDD = CONVERT(varchar(6),@dteDateMDY,12)
			SET @vchResult = SUBSTRING(@chrYYMMDD,3,2) + ' ' + SUBSTRING(@chrYYMMDD,5,2) +  ' ' + SUBSTRING(@chrYYMMDD,1,2)
		END
	ELSE
		IF @chrDateFmtCode = '16'	-- YY MM DD 
		BEGIN
			SET @chrYYMMDD = CONVERT(varchar(6),@dteDateMDY,12)
			SET @vchResult = SUBSTRING(@chrYYMMDD,1,2) +  ' ' + SUBSTRING(@chrYYMMDD,3,2) + ' ' + SUBSTRING(@chrYYMMDD,5,2)
		END
	ELSE
		IF @chrDateFmtCode = '17'	-- YYMMMDD
		BEGIN
			SET @chrDD_MMM_YY = UPPER(CONVERT(char(9),@dteDateMDY,6))
			SET @vchResult =  SUBSTRING(@chrDD_MMM_YY,8,2) + SUBSTRING(@chrDD_MMM_YY,4,3) + SUBSTRING(@chrDD_MMM_YY,1,2)
		END
	ELSE
		IF @chrDateFmtCode = '18'	-- YTMDDM where T is for Toronto, W is for Ford Worth in Sysco label
		BEGIN
			SET @chrYYMMDD = CONVERT(varchar(6),@dteDateMDY,12)
			if (SELECT Value1 from tblControl where [Key] = 'Facility' and SubKey = 'General') = '01 '
				SET @vchResult = SUBSTRING(@chrYYMMDD,2,1) + 'T' + SUBSTRING(@chrYYMMDD,3,1) + SUBSTRING(@chrYYMMDD,5,2) + SUBSTRING(@chrYYMMDD,4,1)
			ELSE
				IF (SELECT Value1 from tblControl where [Key] = 'Facility' and SubKey = 'General') = '07 '
					SET @vchResult = SUBSTRING(@chrYYMMDD,2,1) + 'W' + SUBSTRING(@chrYYMMDD,3,1) + SUBSTRING(@chrYYMMDD,5,2) + SUBSTRING(@chrYYMMDD,4,1)
		END
	ELSE
		IF @chrDateFmtCode = '20'	-- DD-MM-YYYY
		BEGIN
			SET @vchResult = CONVERT(varchar(20),@dteDateMDY,105)
		END
	ELSE
		IF @chrDateFmtCode = '21'	-- DDMMY
		BEGIN
			SET @chrYYMMDD = CONVERT(varchar(6),@dteDateMDY,12)
			SET @vchResult = SUBSTRING(@chrYYMMDD,5,2) + SUBSTRING(@chrYYMMDD,3,2) + SUBSTRING(@chrYYMMDD,2,1)
		END
	ELSE
		IF @chrDateFmtCode = '22'	-- Julian YDDD-BC
		BEGIN
			SET @vchResult = SUBSTRING(CAST(DATEPART(year,@dteDateMDY) as CHAR(4)),4,1) + dbo.fnFillLeadingZeros (3,CAST(DATEPART(dayofyear,@dteDateMDY) as VARCHAR(3))) + '-BC' 
		END
	ELSE
		IF @chrDateFmtCode = '23'	-- AGDDDYYSS where SS is Shift No.
		BEGIN
			SET @chrYYMMDD = CONVERT(varchar(6),@dteDateMDY,12)
--Del#0001 begin 
			--IF exists(SELECT Shift
			--FROM dbo.tblShift 
			--WHERE Facility = @chrFacility and WorkGroup = 'P' and Shift = 2
			--AND (CONVERT(varchar(8),@dteDateMDY,14) BETWEEN CONVERT(varchar(8),FromTime,14)
			--AND CONVERT(varchar(8),ToTime,14))) 
			--SET @chrShiftNo = '02'
		--ELSE
			--IF exists(SELECT shift
			--	FROM dbo.tblShift 
			--	WHERE Facility = @chrFacility and WorkGroup = 'P' and Shift = 1
			--	AND (CONVERT(varchar(8),@dteDateMDY,14) BETWEEN CONVERT(varchar(8),FromTime,14)
			--	AND CONVERT(varchar(8),ToTime,14))) 
		
			--	SET @chrShiftNo = '01'
			--ELSE
			--	SET @chrShiftNo = '03'
			-- SET @vchResult = 'AG' + CAST(DATEPART(dayofyear,@dteDateMDY) as CHAR(3)) + SUBSTRING(@chrYYMMDD,1,2) + @chrShiftNo
--Del#0001 end
			SET @vchResult = 'AG' + dbo.fnFillLeadingZeros (3,CAST(DATEPART(dayofyear,@dteDateMDY) as VARCHAR(3))) + SUBSTRING(@chrYYMMDD,1,2) + '0' + CAST(@intShift as VARCHAR(2))
		END
	ELSE
		IF @chrDateFmtCode = '24'	-- YYMM
		BEGIN
			SET @chrYYMMDD = CONVERT(varchar(6),@dteDateMDY,12)
			SET @vchResult = SUBSTRING(@chrYYMMDD,1,4)
		END
	ELSE
		IF @chrDateFmtCode = '25'	-- YYMMDD where MM is 2 alpha from month word
		BEGIN
			SET @chrYYMMDD = CONVERT(varchar(6),@dteDateMDY,12)
			SET @chr2CharMthWord = (SELECT CASE month(@dteDateMDY)
										WHEN 3 THEN 'MR'
										WHEN 4 THEN 'AL' 
										WHEN 6 THEN 'JN' 
										WHEN 7 THEN 'JL' 
										ELSE upper(SUBSTRING(CONVERT(varchar(9),@dteDateMDY,7),1,2)) END) 
			SET @vchResult = STUFF(@chrYYMMDD,3,2,@chr2CharMthWord)
		END
	ELSE
		IF @chrDateFmtCode = '26'	-- based on the result of (Year - 2000 ) modulo 26.
									-- e.g. 007A for Jan7,2000; 007K for Jan7,2010; 007Z for Jan7,2025; 007A for Jan7,2026
		BEGIN
			SET @chrYYMMDD = CONVERT(varchar(6),@dteDateMDY,12)
-- Del#0007	SET @vchResult = dbo.fnFillLeadingZeros(3,CAST(DATEPART(dayofyear,@dteDateMDY) as VARCHAR(3))) + CHAR(ASCII(SUBSTRING(@chrYYMMDD,2,1)) + 17)
-- Add#0007
			SET @vchResult = dbo.fnFillLeadingZeros(3,CAST(DATEPART(dayofyear,@dteDateMDY) as VARCHAR(3))) + CHAR((DATEPART(year,@dteDateMDY)-2000) % 26 + 65)
		END	
	ELSE
		IF @chrDateFmtCode = '27'	-- YYDDMM where MM is 2 alpha from month word
		BEGIN
			SET @chrYYMMDD = CONVERT(varchar(6),@dteDateMDY,12)
			SET @chr2CharMthWord = (SELECT CASE month(@dteDateMDY)
										WHEN 3 THEN 'MR'
										WHEN 4 THEN 'AL' 
										WHEN 6 THEN 'JN' 
										WHEN 7 THEN 'JL' 
										ELSE upper(SUBSTRING(CONVERT(varchar(9),@dteDateMDY,7),1,2)) END) 
			SET @vchResult = @chr2CharMthWord + SUBSTRING(@chrYYMMDD,5,2) + SUBSTRING(@chrYYMMDD,1,2)
		END
	ELSE
		IF @chrDateFmtCode = '28'	-- MMDDYYYY
		BEGIN
			SET @vchResult = (SELECT REPLACE(CONVERT(varchar(10),@dteDateMDY,110),'-',''))
		END	
	ELSE
		IF @chrDateFmtCode = '29'	-- DDMMYYYY
		BEGIN
			SET @vchResult = (SELECT REPLACE(CONVERT(varchar(10),@dteDateMDY,105),'-',''))
		END	
	ELSE
		IF @chrDateFmtCode = '30'	-- YYYYDDMM
		BEGIN
			SET @vchResult = (SELECT CONVERT(varchar(10),@dteDateMDY,112))
		END	
	ELSE
		IF @chrDateFmtCode = '31'	-- MMMYYYY
		BEGIN
			SET @chrDD_MMM_YYYY = UPPER(CONVERT(char(11),@dteDateMDY,106))
			SET @vchResult = SUBSTRING(@chrDD_MMM_YYYY,4,3) + SUBSTRING(@chrDD_MMM_YYYY,8,4)
		END
	ELSE
		IF @chrDateFmtCode = '32'	-- MM-DD-YY
		BEGIN
			SET @vchResult = CONVERT(varchar(8),@dteDateMDY,10)
		END
	ELSE
		IF @chrDateFmtCode = '33'	-- DD-MMM-YYYY
		BEGIN
			SET @vchResult = REPLACE(UPPER(CONVERT(varchar(11),@dteDateMDY,106)),' ','-')
		END
	ELSE
		IF @chrDateFmtCode = '34'	-- MM/DD/YYYY
		BEGIN
			SET @vchResult = CONVERT(varchar(10),@dteDateMDY,101)
		END
	ELSE
		IF @chrDateFmtCode = '35'	-- MM/DD/YYYY
		BEGIN
			SET @vchResult = CONVERT(varchar(8),@dteDateMDY,1)
		END
-- Add#0001-Begin
	ELSE
		IF @chrDateFmtCode = '36'	-- JJJYS where JJJ is Julian day, Year code A-Z cycle start from 2000
									-- S is shift no. 
		BEGIN
			SET @vchResult = dbo.fnFillLeadingZeros (3,CAST(DATEPART(dayofyear,@dteDateMDY) as VARCHAR(3))) + CHAR((DATEPART(yy,@dteDateMDY) - 2000 ) % 26 + 65) + CAST(@intShift as varchar(2))
		END
-- Add#0001-End
-- Add#0002-Begin
	ELSE
		IF @chrDateFmtCode = '37'	-- Julian YYDDD	
		BEGIN
--	del#0009		SET @vchResult = dbo.fnFillLeadingZeros (2,SUBSTRING(CAST(DATEPART(year,@dteDateMDY) as CHAR(4)),4,2)) + dbo.fnFillLeadingZeros (3,CAST(DATEPART(dayofyear,@dteDateMDY) as VARCHAR(3)))  
			SET @vchResult = SUBSTRING(CAST(DATEPART(year,@dteDateMDY) as CHAR(4)),3,2) + dbo.fnFillLeadingZeros (3,CAST(DATEPART(dayofyear,@dteDateMDY) as VARCHAR(3)))  -- ADD#0009
		END
-- Add#0002-End
-- Add#0003-Begin
	ELSE
		IF @chrDateFmtCode = '38'	-- YYYYMMDD where MM is 2 alpha from month word
		BEGIN
			SET @chrYYYYMMDD = CONVERT(varchar(8),@dteDateMDY,112)
			SET @chr2CharMthWord = (SELECT CASE month(@dteDateMDY)
										WHEN 3 THEN 'MR'
										WHEN 4 THEN 'AL' 
										WHEN 6 THEN 'JN' 
										WHEN 7 THEN 'JL' 
										ELSE upper(SUBSTRING(CONVERT(varchar(9),@dteDateMDY,7),1,2)) END) 
			SET @vchResult = STUFF(@chrYYYYMMDD,5,2,@chr2CharMthWord)
		END
-- Add#0003-End
-- Add#0004-Begin
	ELSE
		IF @chrDateFmtCode = '39'	-- DD-MMM-YY 
		BEGIN
			SET @vchResult = REPLACE(UPPER(CONVERT(varchar(11),@dteDateMDY,06)),' ','-')
		END
-- Add#0004-End
-- Add#0006-Begin
	ELSE
		IF @chrDateFmtCode = '40'	-- YDDD ALPHA YR A=0 B=1 e.g. I312 for Nov 7, 2008
		BEGIN
			SET @chrYYMMDD = CONVERT(varchar(6),@dteDateMDY,12)
			SET @vchResult = CHAR(ASCII(SUBSTRING(@chrYYMMDD,2,1)) + 17) + dbo.fnFillLeadingZeros(3,CAST(DATEPART(dayofyear,@dteDateMDY) as VARCHAR(3))) 
		END	
-- Add#0006-End
-- WO#380-Begin
	ELSE
		IF @chrDateFmtCode = '41'	-- YYYY-MM-DD e.g. 2010-12-08 for Dec 8, 2010
		BEGIN
			SET @vchResult = CONVERT(varchar(10),@dteDateMDY,121)
		END	
-- WO#380-End
-- AddWO#xxx-Begin
	ELSE
		IF @chrDateFmtCode = '42'	-- YYYY MM DD where MM is 2 alpha from month word
		BEGIN
			SET @chrYYYYMMDD = CONVERT(varchar(8),@dteDateMDY,112)
			SET @chr2CharMthWord = (SELECT CASE month(@dteDateMDY)
										WHEN 3 THEN 'MR'
										WHEN 4 THEN 'AL' 
										WHEN 6 THEN 'JN' 
										WHEN 7 THEN 'JL' 
										ELSE upper(SUBSTRING(CONVERT(varchar(9),@dteDateMDY,7),1,2)) END) 
			SET @vchResult = STUFF(STUFF(STUFF(@chrYYYYMMDD,5,2,@chr2CharMthWord),7,0,' '),5,0,' ')
		END
-- AddWO#xxx-End
-- WO#1280 ADD Start
	ELSE
		IF @chrDateFmtCode = '43'	-- YYYYMMM
		BEGIN
			SET @chrDD_MMM_YYYY = UPPER(CONVERT(char(11),@dteDateMDY,106))
			SET @vchResult = SUBSTRING(@chrDD_MMM_YYYY,8,4) + SUBSTRING(@chrDD_MMM_YYYY,4,3)
		END
-- WO#1280 ADD Stop
-- MOD20160506 ADD Start
	ELSE
		IF @chrDateFmtCode = '44'	-- DDMMMYYYY
		BEGIN
			SET @vchResult = REPLACE(UPPER(CONVERT(varchar(11),@dteDateMDY,106)),' ','')
		END
-- MOD20160506 ADD Stop
-- MOD20160720 ADD Start
	ELSE
		IF @chrDateFmtCode = '45'	-- MM DD YYYY
		BEGIN
			SET @vchResult = (SELECT REPLACE(CONVERT(varchar(10),@dteDateMDY,110),'-',' '))
		END
-- MOD20160720 ADD Stop
-- WO#4242 ADD Start
	ELSE
		IF @chrDateFmtCode = '46'	-- DD-MM-YYYY where MM is 2 alpha from month word
		BEGIN
			SET @chrDD_MM_YYYY = CONVERT(varchar(10),@dteDateMDY,105)
			SET @chr2CharMthWord = (SELECT CASE month(@dteDateMDY)
										WHEN 3 THEN 'MR'
										WHEN 4 THEN 'AL' 
										WHEN 6 THEN 'JN' 
										WHEN 7 THEN 'JL' 
										ELSE upper(SUBSTRING(CONVERT(varchar(9),@dteDateMDY,7),1,2)) END) 
			SET @vchResult = STUFF(@chrDD_MM_YYYY,4,2,@chr2CharMthWord)
		END
-- WO#4242 ADD Stop
-- WO#5641 ADD Start
	ELSE
		IF @chrDateFmtCode = '47'	-- DD MM YYYY where MM is 2 alpha from month word
		BEGIN
			SET @chrDD_MM_YYYY = CONVERT(varchar(10),@dteDateMDY,105)
			SET @chr2CharMthWord = (SELECT CASE month(@dteDateMDY)
										WHEN 3 THEN 'MR'
										WHEN 4 THEN 'AL' 
										WHEN 6 THEN 'JN' 
										WHEN 7 THEN 'JL' 
										ELSE upper(SUBSTRING(CONVERT(varchar(9),@dteDateMDY,7),1,2)) END) 
			SET @vchResult = REPLACE(STUFF(@chrDD_MM_YYYY,4,2,@chr2CharMthWord),'-',' ')
		END
-- WO#5641 ADD Stop
-- CHG#57 ADD Start
	ELSE
		IF @chrDateFmtCode = '48'	-- MMYY where MM is 2 alpha from month word
		BEGIN
			SET @chrYYMMDD = CONVERT(varchar(10),@dteDateMDY,12)  --060516
			SET @chr2CharMthWord = (SELECT CASE month(@dteDateMDY)
										WHEN 3 THEN 'MR'
										WHEN 4 THEN 'AL' 
										WHEN 6 THEN 'JN' 
										WHEN 7 THEN 'JL' 
										ELSE upper(SUBSTRING(CONVERT(varchar(9),@dteDateMDY,7),1,2)) END) 
			SET @vchResult =  @chr2CharMthWord + SUBSTRING(@chrYYMMDD,1,2)
		END
-- CHG#57 ADD Stop
-- INCIDENT #46482 ADD Start
	ELSE
			IF @chrDateFmtCode = '49'	-- Y DDD where Y = last digit of current year & DDD = 3-digit Julian date
			BEGIN
				SET @vchResult = Right(Year(getDate()), 1) + ' ' + Right((datepart(year, getDate()) * 1000 + datepart(dy, getDate())), 3)
			END
-- INCIDENT #46482 ADD Stop
	-- Return the result of the function
	RETURN @vchResult
END

GO

