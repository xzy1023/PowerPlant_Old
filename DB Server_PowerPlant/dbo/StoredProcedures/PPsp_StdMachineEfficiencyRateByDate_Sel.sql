

-- =============================================
-- Author:		Bong Lee
-- Create date: Sep. 9, 2019
-- WO27470	  :	Select Standard Machine Efficiency Rate by Date
-- =============================================
CREATE PROCEDURE PPsp_StdMachineEfficiencyRateByDate_Sel

	 @vchFacility as varchar(3) 
	,@vchItemNumber as varchar(35)	= NULL
	,@vchMachineID as varchar(10)	= NULL
	,@intWorkCenter as int			= NULL
	,@bitCurrentRate as bit			= 1
	--,@dteFromDate as datetime		= NULL 	
	--,@dteToDate as datetime			= NULL

AS
BEGIN

	--Declare @vchFacility as varchar(3) = '01'
	--,@vchItemNumber as varchar(35)	= '1102251'
	--,@vchMachineID as varchar(10)	= NULL
	--,@intWorkCenter as int			= NULL
	--,@dteEffectiveDate as datetime	= NULL --'2018-01-29'
	DECLARE @decLBtoGM as decimal(10,7)

	BEGIN TRY

		-- Get the Pound to Gram conversion rate
		SELECT @decLBtoGM = Value1 FROM tblControl 
			WHERE [KEY] = 'WeightConversion' AND SubKey = 'General'

		--IF @dteFromDate IS NOT NULL 
		If @bitCurrentRate = 0
		BEGIN
			-- Required historical rates
			SELECT  tSMER.[ItemNumber]
					,tSMER.[WorkCenter]
					,tSMER.[MachineID]
					,tSMER.[MachineHours]
					,tSMER.[BasisCode]
					,tSMER.[StdWorkCenterEfficiency]
					,tSMER.[RunOperators]
					,NULL as [MachineHoursOriginal]
					,NULL as [RunOperatorsOriginal]
					,tSMER.EffectiveTime
					,tIM.ItemDesc1 + ' ' + LTRIM(tIM.itemDesc2) as ItemDesc
					,tE.[Description] as MachineDesc
					,CASE When ISNULL(tSMER.MachineHours,0) = 0 Then 0 
						Else POWER(10,ISNULL(tSMER.BasisCode,0)) / tSMER.MachineHours END 
						* (Round(tIM.LabelWeight * tIM.SaleableUnitPerCase * tIM.PackagesPerSaleableUnit / (Case When tIM.LabelWeightUOM = 'GM' Then @decLBtoGM Else 1 End),2))
						as LBSPerHour
					,CASE When ISNULL(tSMER.MachineHours,0) = 0 Then 0 
						Else Round(tIM.SaleableUnitPerCase * tIM.PackagesPerSaleableUnit * POWER(10,ISNULL(tSMER.BasisCode,0)) / (tSMER.MachineHours * 60),2) END
						as SaleableUnitPerMinute
					,CASE When ISNULL(tSMER.MachineHours,0)= 0 Then 0 Else Round(POWER(10,ISNULL(tSMER.BasisCode,0)) / tSMER.MachineHours,2) END 
						as StdUnitPerHr
				FROM [dbo].[tblStdMachineEfficiencyRateHst]  as tSMER
				LEFT OUTER JOIN tblItemMaster tIM
				ON tSMER.Facility = tIM.Facility and tSMER.ItemNumber = tIM.ItemNumber
				LEFT OUTER JOIN tblEquipment tE
				ON tSMER.Facility = tE.facility AND tSMER.MachineID = tE.EquipmentID
				WHERE tSMER.facility = @vchFacility and 
					(@vchItemNumber is NULL OR tSMER.itemnumber = @vchItemNumber ) 
					AND (@vchMachineID is NULL OR tSMER.MachineID = @vchMachineID ) 
					AND (@intWorkCenter is NULL OR tSMER.WorkCenter =@intWorkCenter)
					--AND tSMER.EffectiveTime >= ISNULL(@dteFromDate,tSMER.EffectiveTime)
					--AND tSMER.EffectiveTime <= ISNULL(@dteToDate,getdate())
				ORDER BY 
					Case WHEN @vchItemNumber is NULL THEN  tSMER.[ItemNumber]
					ELSE tSMER.[MachineID] END
					,Case WHEN @vchItemNumber is NULL THEN  tSMER.[MachineID]
					ELSE tSMER.[ItemNumber] END
					,tSMER.EffectiveTime Desc
		END
		ELSE
		BEGIN
			-- Required current rates
			SELECT  tSMER.[ItemNumber]
					,tSMER.[WorkCenter]
					,tSMER.[MachineID]
					,tSMER.[MachineHours]
					,tSMER.[BasisCode]
					,tSMER.[StdWorkCenterEfficiency]
					,tSMER.[RunOperators]
					,tSMER.[MachineHoursOriginal]
					,tSMER.[RunOperatorsOriginal]
					,NULL as EffectiveTime
					,tIM.ItemDesc1 + ' ' + LTRIM(tIM.itemDesc2) as ItemDesc
					,tE.[Description] as MachineDesc
					,CASE When ISNULL(tSMER.MachineHours,0) = 0 Then 0 
						Else POWER(10,ISNULL(tSMER.BasisCode,0)) / tSMER.MachineHours END 
						* (Round(tIM.LabelWeight * tIM.SaleableUnitPerCase * tIM.PackagesPerSaleableUnit / (Case When tIM.LabelWeightUOM = 'GM' Then @decLBtoGM Else 1 End),2))
						as LBSPerHour
					,CASE When ISNULL(tSMER.MachineHours,0) = 0 Then 0 
						Else Round(tIM.SaleableUnitPerCase * tIM.PackagesPerSaleableUnit * POWER(10,ISNULL(tSMER.BasisCode,0)) / (tSMER.MachineHours * 60),2) END
						as SaleableUnitPerMinute
					,CASE When ISNULL(tSMER.MachineHours,0)= 0 Then 0 Else Round(POWER(10,ISNULL(tSMER.BasisCode,0)) / tSMER.MachineHours,2) END 
						as StdUnitPerHr
			FROM [tblStdMachineEfficiencyRate] tSMER
			LEFT OUTER JOIN tblItemMaster tIM
			ON tSMER.Facility = tIM.Facility and tSMER.ItemNumber = tIM.ItemNumber
			LEFT OUTER JOIN tblEquipment tE
			ON tSMER.Facility = tE.facility AND tSMER.MachineID = tE.EquipmentID
			WHERE tSMER.facility = @vchFacility and 
				(@vchItemNumber is NULL OR tSMER.itemnumber = @vchItemNumber ) and
				(@vchMachineID is NULL OR tSMER.MachineID = @vchMachineID ) and 
				(@intWorkCenter is NULL OR tSMER.WorkCenter =@intWorkCenter)
			ORDER BY 
				Case WHEN @vchItemNumber is NULL THEN  tSMER.[ItemNumber] 
					ELSE tSMER.[MachineID] END
				,Case WHEN @vchItemNumber is NULL THEN  tSMER.[MachineID]
					ELSE tSMER.[ItemNumber] END
		END

	END TRY
	BEGIN CATCH
		-- variables for error handler.
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure nvarchar(200);
		DECLARE @ErrorLine int;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorMessage NVARCHAR(4000);

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine);
	END CATCH
END

GO

