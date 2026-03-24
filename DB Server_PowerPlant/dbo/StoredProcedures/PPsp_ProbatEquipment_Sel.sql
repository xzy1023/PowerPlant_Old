

-- =============================================
-- Author:		Bong Lee
-- Create date: Jan 28, 2008
-- Description:	Select Probat Equipment
-- WO#871		Mar. 9 2015	Bong Lee
-- Description:	Exclude new columns added for Probat			
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ProbatEquipment_Sel] 
	@chrAction varchar(30),
	@chrFacility char(3), 
	@chrType char(1),
	@chrSubType char(1) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	IF @chrAction = 'All'
-- WO#871		SELECT * from dbo.tblProbatEquipment where Facility = @chrfacility AND [Type] = @chrType AND Subtype = @chrSubtype AND RecID='MX' Order by ProbatEqID
-- WO#871 ADD Start
		SELECT RecID, Facility, ProbatEqID, Description, Type, SubType, GroupID, Capacity, BPCSMachineID, WorkCenter
			FROM tblProbatEquipment
			WHERE (Facility = @chrfacility) AND (Type = @chrType) AND (SubType = @chrSubtype) AND (RecID = 'MX')
			ORDER BY ProbatEqID
-- WO#871 ADD Stop
	ELSE
		IF @chrAction = 'AllWithBlank'
			SELECT  'MX' as RecID, '' as Facility ,'' as ProbatEqID,'' as Description,'' as [Type],'' as SubType, 0 as GroupID, 0 as Capacity,'' as BPCSMachineID,0 as WorkCenter
			Union
-- WO#871	SELECT * from dbo.tblProbatEquipment where Facility = @chrfacility AND [Type] = @chrType AND Subtype = @chrSubtype AND RecID='MX' Order by ProbatEqID
-- WO#871 ADD Start
			SELECT RecID, Facility, ProbatEqID, Description, Type, SubType, GroupID, Capacity, BPCSMachineID, WorkCenter
				FROM dbo.tblProbatEquipment 
				WHERE Facility = @chrfacility AND [Type] = @chrType AND Subtype = @chrSubtype AND RecID='MX' 
				ORDER BY ProbatEqID
-- WO#871 ADD Stop
			
END

GO

