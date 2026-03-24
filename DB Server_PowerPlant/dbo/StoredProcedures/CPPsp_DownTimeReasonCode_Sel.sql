

-- =============================================
-- Author:		Bong Lee
-- Create date: Jan. 12, 2010
-- Description:	Select Down Time Reason Code
-- =============================================
Create PROCEDURE [dbo].[CPPsp_DownTimeReasonCode_Sel]
	@vchAction varchar(50),
	@chrFacility char(3), 
	@vchMachineSubType varchar(10) = NULL,
	@vchMachineType varchar(3) = NULL,
	@vchReasonType int = Null,
	@intReasonCode int = Null
AS
BEGIN
	
	SET NOCOUNT ON;

	BEGIN TRY
		IF @vchAction = 'CodeAndDescription_ByCode'
		BEGIN
			SELECT     MachineType, ReasonType, ReasonCode, CAST(ReasonCode AS varchar(10)) + '-' + Description AS Description
			FROM         tblDTReasonCode
			WHERE     (Facility = @chrFacility) AND
					  (MachineType = @vchMachineType OR  MachineType = 'A') AND 
					  (MachineSubType = @vchMachineSubType) AND (Active = 1)
			ORDER BY ReasonCode
		END
		ELSE
		  IF @vchAction = 'ByDescription '
		  BEGIN
			SELECT     MachineType,  ReasonType, ReasonCode, Description
			FROM         tblDTReasonCode
			WHERE     (Facility = @chrFacility) AND 
					  (MachineType = @vchMachineType OR MachineType = 'A') AND 
					  (MachineSubType = @vchMachineSubType OR MachineSubType = 'A') AND
					  (ReasonType = @vchReasonType) AND 
					  (Active = 1)
			ORDER BY Description
		  END
		  ELSE
			IF @vchAction = 'ByReasonCode '
			BEGIN
				SELECT     MachineType, ReasonCode, ReasonType, Description
				FROM         tblDTReasonCode
				WHERE     (Facility = @chrFacility) AND
						  (MachineType = @vchMachineType OR MachineType = 'A') AND
						  (MachineSubType = @vchMachineSubType OR MachineSubType = 'A') AND
						  (ReasonCode = @intReasonCode) AND  
						  (Active = 1)
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

