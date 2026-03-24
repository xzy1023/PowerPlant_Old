
-- =============================================
-- Author:		Bong Lee
-- Create date: Mar. 13, 2014
-- Description:	Select Probat Equipment records
-- WO#1297:		Mar. 13, 2014	Bong Lee
-- Description:	Add a parameter for selecting record status.
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_ProbatEquipment_Sel] 
	-- Add the parameters for the stored procedure here
	@chrAction varchar(30) = NULL
	,@bitActive bit = NULL
	,@vchMachineType char(1) = NULL	
	,@vchMachineID varchar(10) = NULL
	,@vchProbatEqID varchar(10) = NULL
	,@vchFacility varchar(3) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN
			SELECT  *
			FROM [dbo].[tblProbatEquipment]
				WHERE ([Type] = @vchMachineType OR  @vchMachineType IS NULL)
					AND (MachineID = @vchMachineID OR @vchMachineID IS NULL)
					AND (ProbatEqID = @vchProbatEqID OR  @vchProbatEqID IS NULL)
					AND (Facility = @vchFacility OR @vchFacility IS NULL)
					AND ([Status] = @bitActive OR @bitActive IS NULL)
		END

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

