-- =============================================
-- Author:		Bong Lee
-- Create date: Mar 13, 2014
-- Description:	Copy Probat Equipment data from staging area to production area
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ImportProbatEquipment]
	@chrFacility as char(3),
	@vchImportData as varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	/* WO#1297 DEL Start
	DECLARE @vchSQLStmt varchar(1000);
	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
	WO#1297 DEL Stop */

	-- WO#1297 ADD Start
	DECLARE	@nvchSQLStmt nvarchar(4000);						
	DECLARE	@nvchParmDefinition nvarchar(500);					
	DECLARE	@vchSQLStmt nvarchar(500);
	DECLARE	@vchParmDefinition nvarchar(500);

	BEGIN TRANSACTION [Tran1]
	-- WO#1297 ADD Stop

	BEGIN TRY
		Truncate table tblProbatEquipment

	--Add New Records
		SET @vchSQLStmt = 
		'INSERT INTO tblProbatEquipment ' +	
		'SELECT * ' +
		'FROM ' + @vchImportData + '.dbo.tblProbatEquipment WHERE Facility = @chrFacility'				-- WO#1297

		SET @vchParmDefinition = N'@chrFacility char(3)';												-- WO#1297
		EXECUTE sp_executesql @vchSQLStmt, @vchParmDefinition, @chrFacility;							-- WO#1297
	/* WO#1297 DEL Start
	'FROM ' + @vchImportData + '.dbo.tblProbatEquipment WHERE Facility =''' + @chrFacility + ''''

	execute (@vchSQLStmt)
	WO#1297 DEL Stop */
		COMMIT TRANSACTION [Tran1]	-- WO#1297	
	END try
	BEGIN Catch
	/* WO#1297 DEL Start
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
	WO#1297 DEL Stop */
	-- WO#1297 ADD Start
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION [Tran1]

		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure nvarchar(200);
		DECLARE @ErrorLine int;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorMessage NVARCHAR(4000);

		SELECT 
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ', Message: ' + CHAR(13) + ERROR_MESSAGE();

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)
	-- WO#1297 ADD Stop
	END Catch
END

GO

