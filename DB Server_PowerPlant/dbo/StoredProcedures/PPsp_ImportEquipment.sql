
-- =============================================
-- Author:		Bong Lee
-- Create date: Nov 3, 2006
-- Description:	Copy Equipment data from staging area to production area
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ImportEquipment]
	@chrFacility as char(3),
	@vchImportData as varchar(50)
AS
BEGIN
	--	DECLARE @chrFacility varchar(3)
	DECLARE @vchSQLStmt varchar(1000)
	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	BEGIN TRY
	DELETE tblEquipment Where facility = @chrFacility
		
	SET @vchSQLStmt = 	
		'INSERT INTO tblEquipment ' +
		'SELECT * ' +
		'FROM ' + @vchImportData + '.dbo.tblEquipment WHERE facility = ''' + @chrFacility + ''''
			
	execute (@vchSQLStmt)
	
	END TRY
	BEGIN CATCH
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
	END CATCH
END

GO

