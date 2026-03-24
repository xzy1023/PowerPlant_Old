
-- =============================================
-- Author:		Zhiyuan Xiao
-- Create date: June. 27, 2025
-- Description:	Refresh DynamicLabel Data
-- Mod.			Date			Author
-- V7.01		June 27,2025	Sagar K/Zhiyuan X 
-- Description: Refresh DynamicLabel Data
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RefreshDynamicLabelData]
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY

		-- Step 1: list of computers that have midnight refresh enabled and active
		DECLARE @ComputerList TABLE (ComputerName VARCHAR(50));
		INSERT INTO @ComputerList (ComputerName)
			SELECT ComputerName FROM dbo.tblComputerConfig WHERE EnableCaseLabelMidnightRefresh = 1 and RecordStatus = 1;

		-- Step 2: Create a temporary table to store packaging lines with open shop orders from each computer
		IF OBJECT_ID('tempdb..#TempPackagingLines') IS NOT NULL DROP TABLE #TempPackagingLines;
			CREATE TABLE #TempPackagingLines (PackagingLine VARCHAR(50), Facility char(3), DefaultShiftNo char(1));
		DECLARE @CurrentComputer VARCHAR(50);
		DECLARE ComputerCursor CURSOR FOR SELECT ComputerName FROM @ComputerList;
		DECLARE @SQL NVARCHAR(MAX);

		-- Step 3: loop through each computer linked sql express to look up opening shop order package lines
		OPEN ComputerCursor; FETCH NEXT FROM ComputerCursor INTO @CurrentComputer;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @SQL = '
   			INSERT INTO #TempPackagingLines (Facility, PackagingLine, DefaultShiftNo)
   				SELECT TOP(1) Facility, DefaultPkgLine, DefaultShiftNo FROM [' + @CurrentComputer + '\SQLEXPRESS].[LocalPowerPlant].[dbo].[tblSessionControl] WHERE ShopOrder > 0 AND DefaultPkgLine IS NOT NULL; ';
   			EXEC sp_executesql @SQL;
   			FETCH NEXT FROM ComputerCursor INTO @CurrentComputer;
		END
		CLOSE ComputerCursor;
		DEALLOCATE ComputerCursor;

		-- Step 4: Update tblDynamicLabelData for records with matching packaging lines which label type IN ('C', 'X')
		DECLARE @dteProductionDate as datetime = CAST(DATEADD(DAY, 0, GETDATE()) AS DATE)
		UPDATE d
		SET 
			d.ProductionDate = @dteProductionDate,
			d.LastUpdated = GETDATE(),
			d.MPProductionDate = dbo.fnConvertDate(@dteProductionDate, '01', t.Facility, t.DefaultShiftNo),
			d.PreFmtProductionDate = 
       			CASE 
           			WHEN v.PreFmtProductionDate IS NOT NULL THEN
               			v.ProductionDateDesc + ' ' + dbo.fnConvertDate(@dteProductionDate, v.LabelDateFmtCode, t.Facility, t.DefaultShiftNo)
           			ELSE d.PreFmtProductionDate
       			END
		FROM [PowerPlant_Prd].[dbo].[tblDynamicLabelData] d
		INNER JOIN vwLabelData v ON d.LabelKey = v.LabelKey
		INNER JOIN #TempPackagingLines t ON d.DefaultPkgLine = t.PackagingLine
		WHERE d.RecordType IN ('C', 'X') and d.ProductionDate = CAST(DATEADD(DAY, -1, GETDATE()) AS DATE);

		select RecordType, LabelKey, ShopOrder, ItemNumber,MPProductionDate, PreFmtProductionDate, ProductionDate, DefaultPkgLine, LastUpdated
		FROM [PowerPlant_Prd].[dbo].[tblDynamicLabelData] d
		WHERE d.DefaultPkgLine IN (SELECT PackagingLine FROM #TempPackagingLines)
		AND d.RecordType IN ('C', 'X');

		-- Step 5: Send the new request in cimcontrol table
		DECLARE @Facility CHAR(3), @DftPkgLine CHAR(10), @DeviceType CHAR(1), @StartTime DATETIME, @JobName VARCHAR(50), @Copies SMALLINT;
		DECLARE PrintCursor CURSOR FOR
		SELECT Top 2 Facility, DefaultPkgLine, RecordType, CreationTime, LabelKey
			FROM [PowerPlant_Prd].[dbo].[tblDynamicLabelData]
			WHERE DefaultPkgLine IN (SELECT PackagingLine FROM #TempPackagingLines) AND RecordType IN ('C', 'X') AND ProductionDate = CAST(GETDATE() AS DATE)
			order by CreationTime desc;

			--WHERE DefaultPkgLine  AND RecordType IN ('C', 'X') AND ProductionDate = CAST(GETDATE() AS DATE);

		OPEN PrintCursor;
		FETCH NEXT FROM PrintCursor INTO @Facility, @DftPkgLine, @DeviceType, @StartTime, @JobName;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @Copies = CASE 
							WHEN @DeviceType = 'C' THEN 5000 
							WHEN @DeviceType = 'X' THEN 1 
							ELSE 1 
						  END;

			EXEC [PPsp_CreatePrintRequest]
				@chrLabelType = 'C',
				@chrFacility = @Facility,
				@chrDftPkgLine = @DftPkgLine,
				@chrDeviceType = @DeviceType,
				@dteStartTime = @StartTime,
				@vchJobName = @JobName,
				@bitSbmFromPalletStation = 0,
				@intCopies = @Copies,
				@vchRequestor = 'AutoRefresh',
				@bitIsManualRequest = 0;

			FETCH NEXT FROM PrintCursor INTO @Facility, @DftPkgLine, @DeviceType, @StartTime, @JobName;
		END
		CLOSE PrintCursor;
		DEALLOCATE PrintCursor;

		DROP TABLE #TempPackagingLines;

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

