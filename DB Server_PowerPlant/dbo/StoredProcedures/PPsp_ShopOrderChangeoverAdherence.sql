

-- =============================================
-- Author:		Bong Lee
-- Create date: May 3, 2010
-- Description:	Shop Order Changeover Adherence
--				Sched Changeover time from MiMi vs actual change over time in Power Plant
-- Task#6631	Sep. 08, 2015	Bong Lee
-- Description:	Use information from Dynamics AX
/*
EXEC	[dbo].[PPsp_ShopOrderChangeoverAdherence]
		@vchAction = NULL,
		@chrFacility = N'07',
		@dteFromDate = N'7/1/2015',
		@dteToDate = N'9/7/2015',
		@vchPackagingLine = NULL,
		@vchOperator = NULL
*/
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ShopOrderChangeoverAdherence]

	@vchAction varchar(50)= NULL,
	@chrFacility char(3),
	@dteFromDate as DateTime,
	@dteToDate as DateTime,
	@vchPackagingLine varchar(10) = NULL,
	@vchOperator varchar(10) = NULL
AS
BEGIN
	-- Task#6631 DECLARE @chrEnvironment as char(1)
	DECLARE @vchSQLStmt as nvarchar(3000)
	-- Task#6631 DECLARE @vchISeriesSQLStmt as nvarchar(600)
	DECLARE @vchOrderBy as nvarchar(200)
	/* Task#6631 DEL Start
	DECLARE @iSeriesName as nvarchar(10)
	DECLARE @vchUserLib varchar(10)
	DECLARE @vchOriginalLib varchar(10)
	Task#6631 DEL Stop */
	DECLARE @vchFromShopOrder varchar(25)
	DECLARE @vchToShopOrder varchar(25)
	DECLARE @vchParmDefination nvarchar(100)
	-- Task#6631 ADD Start
	DECLARE @vchAXSQLStmt as nvarchar(3000);
	DECLARE @vchCompanyNo nvarchar(10);
	DECLARE @vchServerName as nvarchar(50);
	DECLARE @vchDBName nvarchar(50);
	-- Task#6631 ADD Stop

	SET NOCOUNT ON;
	BEGIN TRY
		/* Task#6631 DEL Start
		SELECT @chrEnvironment = UPPER(SUBSTRING(Value2,1,1)) from tblControl Where [Key] = 'Facility' and SubKey = 'General'
		SELECT @iSeriesName = Case When @chrEnvironment = 'P' Then Value1 Else Value2 END from tblControl Where [Key] = 'iSeriesNames' and SubKey = 'ServerNames'

	 -- Check the processing environment. Is production or UA?
		If @chrEnvironment = 'P'
		BEGIN
			Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibPrd'
		END
		ELSE
		BEGIN
			If @chrEnvironment = 'U'
			BEGIN
				Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibUA'
			END
			ELSE
			BEGIN
				Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibDev'
			END
		END 
		Task#6631 DEL Stop */

		-- Task#6631 ADD Start
		SELECT @vchCompanyNo = Value2 FROM tblControl WHERE [Key] = 'Facility' AND SubKey = 'General';
		SELECT @vchServerName = Value1, @vchDBName = Value2 FROM tblControl WHERE [Key] = 'ERPEnv' AND SubKey = 'General';
		-- Task#6631 ADD Stop

		/* Task#6631 DEL Start
		SELECT @vchISeriesSQLStmt = N'''SELECT SCFAC as Facility, SCSDTE as StartDate, SCSTIM as StartTime, SCEDTE as EndDate, SCETIM as EndTime, SCSORD as ShopOrder, SCPTIM as ProcessingHours, SCPROD as ItemNumber, SCEQPID as EquipmentID, SCQTY as Quantity FROM '
				 + @vchUserLib + '.FMMSCH$L01 WHERE SCFAC = ''''' + @chrFacility + ' ''''  AND (SCBACTT = ''''P'''' AND SCRACTT = ''''P'''') AND SCSDTE BETWEEN ' + Convert(varchar(10),@dteFromDate,112) + ' AND ' + Convert(varchar(10),@dteToDate,112) + ' AND SCPTIM > 0 AND SCSORD <> 0 '' '
		Task#6631 DEL Stop */
		-- Task#6631 ADD Start
		SELECT @vchFromShopOrder = Cast(Min(ShopOrder) as varchar(25)), @vchToShopOrder= CAST(Max(ShopOrder) as varchar(25))
			FROM tblShopOrderHst 
			WHERE Facility = @chrFacility AND StartDate BETWEEN Convert(varchar(10),@dteFromDate,112) and Convert(varchar(10),@dteToDate,112);

		SELECT @vchAXSQLStmt = N'''SELECT MPMIMIPLANSHOPORDER as ShopOrder, tRP.MPMIMIPLANPROCESSTIME as ProcessingHours FROM ' + @vchDBName + '.[dbo].[ReqPO] as tRP ' +
					'LEFT OUTER JOIN ' + @vchDBName + '.dbo.InventDim as tID ON tRP.CovInventDimId = tID.InventDimId AND tRP.DataAreaID = tID.DataAreaID ' + 
					'WHERE tRP.MPMIMIPLANSHOPORDER BETWEEN ''''' + @vchFromShopOrder + ''''' AND ''''' + @vchToShopOrder + ''''' ' +
					'AND tRP.DataAreaID = ''''' + @vchCompanyNo + ''''' ' +
					'AND tRP.[MPMIMIPLANRELATEDACTIVITYTYPE] = ''''P'''' ' +
					'AND tRP.[MPMIMIPLANBASEACTIVITYTYPE] = ''''P'''' ' +
					'AND tID.INVENTSITEID = ''''' + @chrFacility + '''''''';

		-- Task#6631 ADD Stop
		
		-- Task#6631	SET @vchSQLStmt = N'SELECT tSCHD.Facility, ISNULL(tEqt.Description,tEqt2.Description) as Description, tSCHD.EquipmentID as SchdLineID, tDTL.MachineID as ActualLineID, ActualChangeOverTime, tSCHD.ShopOrder, tDTL.Operator as OperatorID, tSCHD.ItemNumber,' +
		-- Task#6631	'tPS.FirstName + '' '' + tPS.LastName as Operator, tIM.ItemDesc1 as ItemDesc, tSCHD.Quantity, ' + 
		-- Task#6631	'FLOOR(tSCHD.ProcessingHours * 60.00) as SchdNetProcessingMin, ' + 
		SET @vchSQLStmt = N'SELECT tSCHD.Facility, ISNULL(tEqt.Description,tEqt2.Description) as Description, tSCHD.PackagingLine as SchdLineID, tDTL.MachineID as ActualLineID, ActualChangeOverTime, tSCHD.ShopOrder, tDTL.Operator as OperatorID, tSCHD.ItemNumber,' +		-- Task#6631
				'tPS.FirstName + '' '' + tPS.LastName as Operator, tIM.ItemDesc1 as ItemDesc, tSCHD.OrderQty as Quantity, ' +		-- Task#6631
				'FLOOR(ISNULL(tRPO.ProcessingHours,0) * 60.00) as SchdNetProcessingMin, ' +						-- Task#6631
				'STUFF(STUFF(tSCHD.StartDate,7,0,''-''),5,0,''-'') + '' '' +  dbo.fnCvtNumTimeToDateTime(tSCHD.StartTime) as SchdStartTime, ' + 
				'STUFF(STUFF(tSCHD.EndDate,7,0,''-''),5,0,''-'') + '' '' +  dbo.fnCvtNumTimeToDateTime(tSCHD.EndTime) as SchdEndTime, ' +
				'DATEDIFF(minute, STUFF(STUFF(tSCHD.StartDate,7,0,''-''),5,0,''-'') + '' '' +  dbo.fnCvtNumTimeToDateTime(tSCHD.StartTime), STUFF(STUFF(tSCHD.EndDate,7,0,''-''),5,0,''-'') + '' '' +  dbo.fnCvtNumTimeToDateTime(tSCHD.EndTime)) as SchdGrossProcessMin, ' +
				'DATEDIFF(minute, STUFF(STUFF(tSCHD.StartDate,7,0,''-''),5,0,''-'') + '' '' +  dbo.fnCvtNumTimeToDateTime(tSCHD.StartTime), STUFF(STUFF(tSCHD.EndDate,7,0,''-''),5,0,''-'') + '' '' +  dbo.fnCvtNumTimeToDateTime(tSCHD.EndTime)) - FLOOR(ISNULL(tRPO.ProcessingHours,0) * 60.00) as SchdChgOverTime ' +		-- Task#6631
			-- Task#6631 'DATEDIFF(minute, STUFF(STUFF(tSCHD.StartDate,7,0,''-''),5,0,''-'') + '' '' +  dbo.fnCvtNumTimeToDateTime(tSCHD.StartTime), STUFF(STUFF(tSCHD.EndDate,7,0,''-''),5,0,''-'') + '' '' +  dbo.fnCvtNumTimeToDateTime(tSCHD.EndTime)) - FLOOR(tSCHD.ProcessingHours * 60.00) as SchdChgOverTime ' +
			-- Task#6631 'FROM OPENQUERY(' + @iSeriesName + ',' + @vchISeriesSQLStmt + ') tSCHD ' + 
			'FROM tblShopOrderHst tSCHD ' +
			'LEFT OUTER JOIN OPENQUERY([' + @vchServerName + '],' + @vchAXSQLStmt + ') tRPO ' +			-- Task#6631 
			'ON tSCHD.ShopOrder = tRPO.ShopOrder ' +													-- Task#6631
			'LEFT OUTER JOIN (SELECT Facility, ShopOrder, MachineID, Operator, SUM(DATEDIFF(minute,DownTimeBegin, DownTimeEnd)) as ActualChangeOverTime From tblDownTimeLog WHERE Facility =''' + @chrFacility + ''' AND MachineType = ''P'' ' + 
				'AND ReasonCode in (SELECT Distinct ReasonCode from dbo.tblDTReasonCode where description like ''%changeover%'') Group by Facility, ShopOrder, MachineID, Operator) tDTL ' +
			'On tSCHD.Facility = tDTL.Facility AND tSCHD.ShopOrder = tDTL.ShopOrder ' +
			'LEFT OUTER JOIN tblItemMaster tIM ' +
			'On tSCHD.Facility = tIM.facility AND tSCHD.ItemNumber = tIM.ItemNumber ' +
			'LEFT OUTER JOIN dbo.tblPlantStaff tPS ' +
			'On tSCHD.Facility = tPS.Facility AND tDTL.Operator = tPS.StaffID ' +
			'LEFT OUTER JOIN dbo.tblEquipment tEqt ' +
			'On tDTL.Facility = tEqt.Facility AND tDTL.MachineID = tEqt.EquipmentID ' +
			'LEFT OUTER JOIN dbo.tblEquipment tEqt2 ' +
			-- Task#6631'	On tSCHD.Facility = tEqt2.Facility AND tSCHD.EquipmentID = tEqt2.EquipmentID ' +
			-- Task#6631	'WHERE (@vchPackagingLine is NULL OR tDTL.MachineID = @vchPackagingLine OR tSCHD.EquipmentID = @vchPackagingLine) ' +
			'On tSCHD.Facility = tEqt2.Facility AND tSCHD.PackagingLine = tEqt2.EquipmentID ' +			-- Task#6631
			'WHERE (@vchPackagingLine is NULL OR tDTL.MachineID = @vchPackagingLine OR tSCHD.PackagingLine = @vchPackagingLine) ' +			-- Task#6631
			' AND (@vchOperator is NULL OR tDTL.Operator = @vchOperator) ' +
			'AND tSCHD.Facility = ''' + @chrFacility + ''' ' +											-- Task#6631
			'AND tSCHD.StartDate BETWEEN ' + Convert(varchar(10),@dteFromDate,112) + ' AND ' + Convert(varchar(10),@dteToDate,112) +  ' ' + -- Task#6631 
			'AND tEqt2.[Type] = ''P'' ' +																-- Task#6631
			'Order by ISNULL(tEqt.Description,tEqt2.Description) '	
			
	print 'AXSQLStmt=' + @vchAXSQLStmt 
	print 'SQLStmt=' + @vchSQLStmt

	SET @vchParmDefination = N'@vchPackagingLine varchar(10), @vchOperator varchar(10)'
	EXEC sp_ExecuteSQL @vchSQLStmt, @vchParmDefination, @vchPackagingLine=@vchPackagingLine, @vchOperator=@vchOperator;

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

