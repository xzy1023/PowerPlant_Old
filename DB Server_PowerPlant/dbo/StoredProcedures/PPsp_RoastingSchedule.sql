


-- =============================================
-- Author:		Bong Lee
-- Create date: Augest. 18, 2011
-- Description:	Roasting Schedule Report
-- WO#871:		Bong Lee	Apr.7,2014	  
-- Description:	Add a column for shop order number.
-- =============================================
/* Testing
	Exec ('CALL ubpcsusr.SFC$735S (''07'',20110902,20110902)') at S105HF5M

	[PPsp_RoastingSchedule]	@chrFacility = N'07',@dteFromDate = N'9/2/2011',@dteToDate = N'9/2/2011'
*/
CREATE PROCEDURE [dbo].[PPsp_RoastingSchedule] 

	@chrFacility char(3),
	@dteFromDate datetime,
	@dteToDate datetime

AS
BEGIN
	--DECLARE @intProductionDate as int
	DECLARE @vchProcEnv as varchar(50)
	DECLARE @vchiSeriesServerName as varchar(50)
	DECLARE @vchUserLib varchar(10)
	DECLARE @vchOriginalLib varchar(10)
	DECLARE @vchSQLStmt varchar(1000)

	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);

	BEGIN TRY

	SET NOCOUNT ON;

	Select @vchProcEnv = Rtrim(Substring(DB_NAME(),Charindex('_',DB_NAME())+1,10))

	If @vchProcEnv = 'PRD'
	BEGIN
		Select @vchiSeriesServerName = value1 From tblControl Where [key] = 'iSeriesNames'
		Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibPRD'
	END
	ELSE
	BEGIN
		If @vchProcEnv = 'UA'
		BEGIN
			Select @vchiSeriesServerName = value2 From tblControl Where [key] = 'iSeriesNames'
			Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibUA'
		END
		ELSE
		BEGIN
			Select @vchiSeriesServerName = value2 From tblControl Where [key] = 'iSeriesNames'
			Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibDev'
		END
	END 
	
	Select @vchUserLib = Substring(@vchUserLib,1,Len(@vchUserLib)-1) + 'R'

	/* get data from BPCS */

	DECLARE @tblRS Table (
		Facility char(3)			-- SCFAC	
		,ShopOrder int				-- SCSORD	WO#871
		,RoasterID char(10)			-- SCRSTRID
		,RoasterNo char(10)			-- SCRSTR#	 		
		,RoasterStartDate int		-- SCRSDTE
		,RoasterStartTime int		-- SCRSTIM
		,ActivityID varchar(24)		-- SCBACTI
		,ActivityType char(1)		-- SCRACTT 
		,RelatedActivityID varchar(24)		-- SCRACTI 
		,ActivitySequenceNo int		-- SCSEQNO 
		,EquipmentID varchar(10)	-- SCEQPID
		,EquipmentSubType char(1)	-- SCEQTP1
		,ItemNumber varchar(35)		-- SCPROD
		,Blend varchar(11)			-- SCBLEND
		,Grind varchar(6)			-- SCGRIND
		,NoOfBatches int			-- SCBATCH
		,Quantity decimal(11,3)		-- SCQTY
		,Operation varchar(15)		-- SCOPER
		,StartDate int				-- SCSDTE
		,StartTime int				-- SCSTIM
		,EndDate int				-- SCEDTE
		,EndTime int				-- SCETIM
		,ScheduleExportDate int		-- SCXDTE
		,ScheduleExportTime int		-- SCXTIM
		,CreateDate int				-- SCCDTE
		,CreateTime int				-- SCCTIM
	)

	SET @vchSQLStmt =  'EXEC(''CALL ' + @vchUserLib + '.SFC$735S (''''' + @chrFacility + ''''',' + Convert(varchar(8),@dteFromDate,112) + ',' + Convert(varchar(8),@dteToDate,112) + ')'') at ' + @vchiSeriesServerName

	INSERT INTO @tblRS
	Exec (@vchSQLStmt)

	;WITH cte_Rster AS(
		SELECT 
		RTRIM(RoasterID) as RoasterID
		,stuff(stuff(cast(StartDate as char(8)),5,0,'/'),8,0,'/') + dbo.fnCvtNumTimeToDateTime(StartTime) as RoasterStartTime
		,stuff(stuff(cast(EndDate as char(8)),5,0,'/'),8,0,'/') + dbo.fnCvtNumTimeToDateTime(EndTime) as RoasterEndTime
		,ActivityID
		,Blend 
		,Round(NoOfBatches,0) as NoOfBatches
		,Round(Quantity,0) as Quantity
		,stuff(stuff(cast(ScheduleExportDate as char(8)),5,0,'/'),8,0,'/') as ScheduleExportDate
		,ShopOrder		--WO#871
		FROM @tblRs
		WHERE ActivityType = 'R'
	)

	,cte_Bin AS (
		SELECT 
		ActivityID
		,RTRIM(EquipmentID) as EquipmentID
		,stuff(stuff(cast(StartDate as char(8)),5,0,'/'),8,0,'/') + dbo.fnCvtNumTimeToDateTime(StartTime) as BinStartDateTime
		,stuff(stuff(cast(EndDate as char(8)),5,0,'/'),8,0,'/') + dbo.fnCvtNumTimeToDateTime(EndTime) as BinEndDateTime
		,EquipmentSubType
		,Round(SUM(Quantity),0) as BinQuantity	
		FROM @tblRs
		WHERE ActivityType = 'B'
		GROUP BY ActivityID
			,RTRIM(EquipmentID)
			,stuff(stuff(cast(StartDate as char(8)),5,0,'/'),8,0,'/') + dbo.fnCvtNumTimeToDateTime(StartTime) 
			,stuff(stuff(cast(EndDate as char(8)),5,0,'/'),8,0,'/') + dbo.fnCvtNumTimeToDateTime(EndTime)
			,EquipmentSubType
	)

	,cte_Grinder AS (
		SELECT 
		ActivityID
		,RTRIM(EquipmentID) as EquipmentID
		,stuff(stuff(cast(StartDate as char(8)),5,0,'/'),8,0,'/') + dbo.fnCvtNumTimeToDateTime(StartTime) as StartDateTime
		,ShopOrder		--WO#871
		FROM @tblRs
		WHERE ActivityType = 'G'
	)

	,cte_PkgLine AS (
		SELECT 
		ActivityID
		,RTRIM(EquipmentID) as EquipmentID
		,stuff(stuff(cast(StartDate as char(8)),5,0,'/'),8,0,'/') + dbo.fnCvtNumTimeToDateTime(StartTime) as StartDateTime
		,ShopOrder		--WO#871
		FROM @tblRs
		WHERE ActivityType = 'P'
	)
	
	SELECT ROW_NUMBER() OVER(PARTITION BY tR.ActivityID ORDER BY tb.BinStartDateTime) as RowNumber
			,tR.RoasterID ,tR.RoasterStartTime, tR.RoasterEndTime
			,tR.ActivityID ,tR.Blend , tR.NoOfBatches
			,CASE WHEN ROW_NUMBER() OVER(PARTITION BY tR.ActivityID ORDER BY tb.BinStartDateTime) = 1 THEN tR.Quantity ELSE 0 END as Quantity
			,tR.ScheduleExportDate
			,tB.EquipmentID as Bin 
			,tB.BinStartDateTime
			,tB.BinEndDateTime
			,tB.BinQuantity
			,tB.EquipmentSubType as EqtSubType
			,tG.EquipmentID as Grinder, tG.StartDateTime as GdrStartTime
			,tP.EquipmentID as PkgLine, tP.StartDateTime as LineStartTime
			,tR.ShopOrder		--WO#871
		FROM cte_Rster tR
		LEFT OUTER JOIN cte_Bin tB 
		ON tR.ActivityID = tB.ActivityID
		LEFT OUTER JOIN cte_Grinder tG 
		ON tR.ActivityID = tG.ActivityID
		LEFT OUTER JOIN cte_PkgLine tP 
		ON tR.ActivityID = tP.ActivityID
		Order by tR.ActivityID,RowNumber


	END TRY
	BEGIN CATCH
		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorNumber = ERROR_NUMBER(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');

		RAISERROR 
			(
			@ErrorMessage, 
			@ErrorSeverity, 
			@ErrorState,     -- parameter: original error state.               
			@ErrorNumber,    -- parameter: original error number.
			@ErrorSeverity,  -- parameter: original error severity.
			@ErrorProcedure, -- parameter: original error procedure name.
			@ErrorLine       -- parameter: original error line number.
			);

	END CATCH
END

GO

