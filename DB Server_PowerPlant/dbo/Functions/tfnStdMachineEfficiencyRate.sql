

-- =============================================
-- Author:		Bong Lee
-- Create date: Aug. 23, 2019
-- Description:	Get Standard Machine Efficiency Rate
-- =============================================
CREATE FUNCTION [dbo].[tfnStdMachineEfficiencyRate]
(
	-- Add the parameters for the function here
		@vchFacility varchar(3)
		,@vchItemNumber varchar(35)
		,@vchMachineID varchar(10) 
		,@intWorkCenter int
		,@dteProductionTime datetime
)
RETURNS TABLE 
AS
RETURN
	
	SELECT TOP 1 * FROM (
		SELECT * FROM tblStdMachineEfficiencyRateHst
		  WHERE Facility = @vchFacility 
			AND ItemNumber = @vchItemNumber 
			AND (@intWorkCenter is NULL OR WorkCenter = @intWorkCenter) 
			AND (@vchMachineID is NULL OR MachineID = @vchMachineID OR MachineID = '')
			AND EffectiveTime =   
			(SELECT MAX(EffectiveTime) 
				FROM tblStdMachineEfficiencyRateHst
				WHERE Facility = @vchFacility 
					AND ItemNumber = @vchItemNumber 
					AND (@intWorkCenter is NULL OR WorkCenter = @intWorkCenter)
					AND (@vchMachineID is NULL OR MachineID = @vchMachineID OR MachineID = '')
					AND  EffectiveTime <= @dteProductionTime
			)
		Union
		SELECT * FROM tblStdMachineEfficiencyRateHst
		  WHERE Facility = @vchFacility AND ItemNumber =@vchItemNumber 
			AND (@intWorkCenter is NULL OR WorkCenter = @intWorkCenter)
			AND (@vchMachineID is NULL OR MachineID = @vchMachineID OR MachineID = '') 
			AND EffectiveTime =   
			(SELECT MIN(EffectiveTime) 
				FROM tblStdMachineEfficiencyRateHst
				WHERE Facility = @vchFacility 
					AND ItemNumber = @vchItemNumber 
					AND (@intWorkCenter is NULL OR WorkCenter = @intWorkCenter)
					AND (@vchMachineID is NULL OR MachineID = @vchMachineID OR MachineID = '')
			)			
	) tSMER
	Order by EffectiveTime desc

GO

