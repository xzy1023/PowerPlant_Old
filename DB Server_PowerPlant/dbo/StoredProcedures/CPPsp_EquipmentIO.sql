
-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 1, 2006
-- Description:	Equipment I/O Module
-- WO#648		Mar. 16, 2012	Bong Lee
-- Description:	Select the equipments in the same IPC IPCSharedGroup
-- (Asumption): The equipment id is 6 charater long and the 7th chareter is its associate equipment ID
--				e.g. 3713-03, 3713-03A, 3713-03B
-- WO#871		Mar. 11, 2014	Bong Lee
-- Description:	Select the equipments which links to Probat (i.e. Probat client installed on the IPC). 
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_EquipmentIO]
	-- Add the parameters for the stored procedure here
	@chrFacility char(3), 
	@chrEquipmentID char(10),
	@chrType char(1) = NULL,
	@chrSubType char(1) = NULL
	,@vchAction varchar(50) = NULL	--WO#648
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    --WO#648 ADD Start
    IF @vchAction = 'ActiveSharedGroup'
		SELECT * FROM tblEquipment 
			WHERE Facility = @chrFacility 
				AND EquipmentID like RTRIM(LEFT(@chrEquipmentID,7)) + '%'				
				AND [Type] = @chrType
				AND Active = 1
				AND (IPCSharedGroup is NULL 
					OR IPCSharedGroup  = (SELECT IPCSharedGroup FROM tblEquipment 
											WHERE Facility = @chrFacility 
												AND EquipmentID = @chrEquipmentID
												AND [Type] = @chrType
												AND Active = 1))
    ELSE
    --WO#648 ADD Stop
	--WO#871 ADD Start
		IF @vchAction = 'ProbatEnabled'
		SELECT * FROM tblEquipment tEQ
			LEFT OUTER JOIN tblComputerConfig tCF
			ON tEQ.Facility = tCF.Facility AND tEQ.EquipmentID = tCF.Packagingline
			WHERE tEQ.Facility = @chrFacility 
				AND (EquipmentID = @chrEquipmentID OR @chrEquipmentID IS NULL)
				AND (tEQ.[Type] = @chrType OR @chrType IS NULL)
				AND tEQ.Active = 1
				AND tCF.ProbatEnabled = 1
		 ELSE
		 	IF @vchAction = 'ProbatDisabled'
			SELECT * FROM tblEquipment tEQ
				LEFT OUTER JOIN tblComputerConfig tCF
				ON tEQ.Facility = tCF.Facility AND tEQ.EquipmentID = tCF.Packagingline
				WHERE tEQ.Facility = @chrFacility 
					AND (EquipmentID = @chrEquipmentID OR @chrEquipmentID IS NULL)
					AND ([Type] = @chrType OR @chrType IS NULL)
					AND Active = 1
					AND tCF.ProbatEnabled = 0
			ELSE

--WO#871 ADD Stop
			IF @chrType is Not NULL
			SELECT  * FROM tblEquipment 
				WHERE facility = @chrFacility and
						EquipmentID = @chrEquipmentID and
						[Type] = @chrType and 
						active = 1
END

GO

