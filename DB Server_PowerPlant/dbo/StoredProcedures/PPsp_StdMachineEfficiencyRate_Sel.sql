
-- =============================================
-- Author:		Bong Lee
-- Create date: Feb 11 2009
-- Description:	Retrieve Standard Machine Run Rate and budget Efficiency Rate
-- WO#755:		Feb. 08, 2013	Bong Lee
--				Use Equipment table to get the Work Center of the line
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_StdMachineEfficiencyRate_Sel]
	@vchAction as varchar(50),	-- ChkExistance
	@chrFacility char(3), 
	@vchItemNumber varchar(35),
	@chrPkgLine char(10),
	@decMachineHours decimal(8,3) OUTPUT,
	@chrBasisCode char(1) OUTPUT,
	@decStdWCEfficiency decimal(5,4) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    BEGIN TRY
		SET @decMachineHours = 0

		-- Try to use machine id to search first. If not found use work center of the machine to search
		SELECT @decMachineHours=ISNULL(MachineHours,0), @chrBasisCode=ISNULL(BasisCode,0),@decStdWCEfficiency= ISNULL(StdWorkCenterEfficiency,0) 
		FROM dbo.tblStdMachineEfficiencyRate 
		WHERE Facility = @chrFacility And
			  ItemNumber = @vchItemNumber AND
			  MachineID = @chrPkgLine

		IF @decMachineHours = 0
			SELECT @decMachineHours=ISNULL(MachineHours,0), @chrBasisCode=ISNULL(BasisCode,0),@decStdWCEfficiency= ISNULL(StdWorkCenterEfficiency,0) 
			/* WO#755 DEL Start
			FROM dbo.tblStdMachineEfficiencyRate 
			WHERE Facility = @chrFacility And
				  ItemNumber = @vchItemNumber AND
				  WorkCenter = CAST(Substring(@chrPkgLine,1,4) as int) AND
			WO#755 DEL Stop */
			-- WO#755 ADD Start
			FROM dbo.tblStdMachineEfficiencyRate tSMER
			WHERE tSMER.Facility = @chrFacility 
				  AND tSMER.ItemNumber = @vchItemNumber 
				  AND tSMER.WorkCenter =  CAST((SELECT WorkCenter FROM tblEquipment WHERE EquipmentID = @chrPkgLine) as int) 
				  AND tSMER.MachineID = ''		
			-- WO#755 ADD Stop
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		CLOSE object_cursor
		DEALLOCATE object_cursor

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

