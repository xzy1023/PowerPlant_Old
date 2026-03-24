
-- =============================================
-- Author:		Bong Lee
-- Create date: May 6, 2012
-- Description:	Actual Packaging Line Efficency
-- WO#5103:		Mar. 03, 2017	Bong Lee	
-- Description:	Allow to filter by shift
-- WO#27470:	Aug. 27, 2019	Bong Lee
-- Description:	Routing rates are required to base on effective date
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ActualMachineEfficiency]
	@vchFacility varchar(3),
	@vchMachineID varchar(10),
	@dteFromProdDate as DateTime,
	@intFromShift as tinyint,
	@dteToProdDate as DateTime,
	@intToShift as tinyint
	,@intInclOnlyShift as tinyint = NULL		-- WO#5103
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRY;

		DECLARE @tblData TABLE(
		ShiftProductionDate datetime
		,t numeric(17,7)
		,y numeric(17,7)
		,StdWCEfficiency dec(5,4)
		)
		
		DECLARE @numIntercept numeric(17,7)
		DECLARE @numSlope numeric(17,7)
		DECLARE @IntCount int

		;With SCH_CTE AS
		(
			SELECT tSCH_CTE.Facility, tSCH_CTE.PackagingLine, tSCH_CTE.ShiftProductionDate, 
				Case When Sum(tSCH_CTE.RunTime) = 0 Then 0 Else Sum(tSCH_CTE.StdMachineHrEarnedInUnit) / Sum(tSCH_CTE.RunTime) End as LineEff,
				Max(tSCH_CTE.StdWCEfficiency) as StdWCEfficiency
			FROM 
				(
				  SELECT tSCH.Facility, tSCH.DefaultPkgLine as PackagingLine, tSCH.ShiftProductionDate, tSCH.ShopOrder, (tSCH.CasesProduced + tSCH.AdjustedQty) as CasesProduced,
					tSCH.PaidRunTime as RunTime, 
					CASE When ISNULL(tfSMER.MachineHours,0)= 0 Then 0 Else (tSCH.CasesProduced + ISNULL(tSCH.AdjustedQty,0))/ (POWER(10,ISNULL(tfSMER.BasisCode,0)) / tfSMER.MachineHours) End As StdMachineHrEarnedInUnit,	-- WO#27470
					IIF(tfSMER.StdWorkCenterEfficiency = 0, NULL, tfSMER.StdWorkCenterEfficiency) as StdWCEfficiency						-- WO#27470
					-- WO#27470 CASE When tSMER_ID.MachineHours is NULL and tSMER_WC.MachineHours is Null Then 0 Else (tSCH.CasesProduced + tSCH.AdjustedQty) / (POWER(10,ISNULL(tSMER_ID.BasisCode,ISNULL(tSMER_WC.BasisCode,0))) / ISNULL(tSMER_ID.MachineHours,ISNULL(tSMER_WC.MachineHours,0))) End As StdMachineHrEarnedInUnit,
					-- WO#27470 ISNULL(tSMER_ID.StdWorkCenterEfficiency,ISNULL(tSMER_WC.StdWorkCenterEfficiency,0)) as StdWCEfficiency
				  FROM dbo.tfnSessionControlHstSummary('WithAdjByLineSO',@vchFacility,@vchMachineID,NULL,NULL,NULL,@dteFromProdDate,@intFromShift,@dteToProdDate,@intToShift) tSCH
				  OUTER APPLY [dbo].[tfnStdMachineEfficiencyRate] (tSCH.Facility, tSCH.ItemNumber, tSCH.DefaultPkgLine, LEFT(tSCH.DefaultPkgLine,4), tSCH.ShiftProductionDate) as tfSMER			-- WO#27470
				  -- WO#27470 DEL Start
				  --LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_ID
				  --ON tSCH.Facility = tSMER_ID.Facility AND tSCH.ItemNumber = tSMER_ID.ItemNumber AND tSCH.DefaultPkgLine = tSMER_ID.MachineID
				  --LEFT OUTER JOIN dbo.tblStdMachineEfficiencyRate tSMER_WC
				  --ON tSCH.Facility = tSMER_WC.Facility AND tSCH.ItemNumber = tSMER_WC.ItemNumber AND CAST(SUBSTRING(tSCH.DefaultPkgLine,1,4) as int) = tSMER_WC.WorkCenter and tSMER_WC.MachineID = ''	
				  -- WO#27470 DEL Stop
				  WHERE tSCH.ShiftNo = ISNULL(@intInclOnlyShift, tSCH.ShiftNo)					-- WO#5103
				) as tSCH_CTE 
			WHERE tSCH_CTE.RunTime > 0 and tSCH_CTE.CasesProduced > 0
			GROUP BY tSCH_CTE.Facility, tSCH_CTE.PackagingLine, tSCH_CTE.ShiftProductionDate
			)

			INSERT INTO @tblDATA
			SELECT ShiftProductionDate
				,DATEDIFF(day, @dteFromProdDate ,ShiftProductionDate) AS t
				,LineEff AS y
				,StdWCEfficiency
			FROM SCH_CTE
			
		Set @intCount = (SELECT COUNT(*) FROM @tblDATA)
		
		IF @intCount > 0					-- WO#5103
		BEGIN								-- WO#5103
			--Calculation of regression line slope
			SET @numSlope =
				-- WO#5103 ADD Start
				CASE WHEN (SELECT @intCount * SUM(t*t) FROM @tblDATA) -
						POWER((SELECT SUM(t) FROM @tblDATA),2) <> 0
				THEN
				-- WO#5103 ADD Stop
					(
						(SELECT @intCount * SUM(t*y) FROM @tblDATA) -
							(SELECT SUM(t) * SUM(y) FROM @tblDATA)
					) /
					(
						(SELECT @intCount * SUM(t*t) FROM @tblDATA) -
							POWER((SELECT SUM(t) FROM @tblDATA),2)
					)
				-- WO#5103 ADD Start
				ELSE
					0
				END
				-- WO#5103 ADD Stop
			-- Calculateion of regression line inercept
			SET @numIntercept = 
				(
				(SELECT SUM(y) FROM @tblDATA) - 
					(SELECT @numSlope * SUM(t) FROM @tblDATA)
				) /
				@intCount
		
		END					-- WO#5103	
		SELECT 
			ShiftProductionDate
			,t
			,y
			,(t * @numSlope + @numIntercept) as trend
			,StdWCEfficiency
		From @tblDATA
		

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
	END CATCH;
END

GO

