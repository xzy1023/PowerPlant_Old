


-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 1, 2006
-- Description:	Select Equipment 
-- WO#359:		Feb. 10, 2012	Bong Lee
--				Add action 'ListAllByTypeWithBlank'
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_Equipment_Sel]
	-- Add the parameters for the stored procedure here
	@vchAction varchar(30),
	@chrFacility char(3), 
	@chrType char(1),
	@chrEquipmentID char(10) =NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @vchAction = 'ByIDType'
	Begin
		IF @chrType is Not NULL
			SELECT  Active, facility, EquipmentID, Type, SubType, Description, GroupID, ProbatID, IPCSharedGroup, WorkCenter
				FROM  tblEquipment
				WHERE facility = @chrFacility and
					  EquipmentID = @chrEquipmentID and
					  [Type] = @chrType and 
					  active = 1 and EquipmentID not in ('SPARE', 'Tote')
	END
	ELSE
	BEGIN
		IF @vchAction = 'ListByType'
			SELECT  Active, facility, EquipmentID, Type, SubType, Description, GroupID, ProbatID, IPCSharedGroup, WorkCenter
				FROM  tblEquipment
				WHERE facility = @chrFacility and
					  [Type] = @chrType and 
					  active = 1 and EquipmentID not in ('SPARE', 'Tote')
			Order BY Description
		ELSE
		BEGIN
			IF @vchAction = 'ListByTypeWithBlank'
				SELECT 1 as active, NULL as facility, Null as EquipmentID,
					   NULL as Type, Null as SubType, '' as Description,
					   NULL as GroupID, NULL as ProbatID, NULL as IPCSharedGroup,
					   NULL as WorkCenter
				UNION
				SELECT  Active, facility, EquipmentID, Type, SubType, Description, GroupID, ProbatID, IPCSharedGroup, WorkCenter
					FROM  tblEquipment
					WHERE facility = @chrFacility and
						  [Type] = @chrType and 
						  active = 1 and EquipmentID not in ('SPARE', 'Tote')
				Order BY Description
-- WO#359 Add Start
			ELSE
			BEGIN
				IF @vchAction = 'ListAllByTypeWithBlank'
					SELECT NULL as active, NULL as facility, Null as EquipmentID,
						   NULL as Type, Null as SubType, '*ALL' as Description,
						   NULL as GroupID, NULL as ProbatID, NULL as IPCSharedGroup,
						   NULL as WorkCenter
					UNION
					SELECT  Active, facility, EquipmentID, Type, SubType, Description, GroupID, ProbatID, IPCSharedGroup, WorkCenter
						FROM  tblEquipment
						WHERE facility = @chrFacility 
							  AND [Type] = @chrType 
							  AND (EquipmentID= @chrEquipmentID OR (@chrEquipmentID is NULL AND EquipmentID NOT IN ('SPARE', 'Tote')))
					Order BY Description
			END
-- WO#359 Add Stop
		END
	END
	
END

GO

