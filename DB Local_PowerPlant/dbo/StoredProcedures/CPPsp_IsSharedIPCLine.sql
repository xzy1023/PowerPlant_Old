

-- =============================================
-- Author:		Bong Lee
-- Create date: Mar. 11, 2010
-- Description:	Whether the overrode packaging line is the shared IPC line
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_IsSharedIPCLine] 
	@vchFacility varchar(3),
	@vchDefaultPkgLine varchar(10),
	@vchOverridePkgLine varchar(10),
	@bitResult bit OUTPUT

AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY;
		SET @bitResult = 0

		SELECT @bitResult = 1 FROM tblEquipment 
			WHERE Facility = @vchFacility AND EquipmentID = @vchOverridePkgLine AND Type = 'P'
				AND IPCSharedGroup = (SELECT IPCSharedGroup FROM tblEquipment 
								WHERE Facility = @vchFacility AND EquipmentID = @vchDefaultPkgLine AND Type = 'P')
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

