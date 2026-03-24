
-- =============================================
-- Author:		Bong Lee
-- Create date: Nov 3, 2006
-- Description:	Copy Bill Of Materials data from staging area to production area
-- WO#1297      Aug. 19, 2014   Bong Lee
-- Description: Enhance error handling.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ImportBOM]
	@chrFacility as char(3),
	@vchImportData as varchar(50)
AS
BEGIN
--	DECLARE @chrFacility varchar(3)
	DECLARE @vchSQLStmt varchar(1000)
-- WO#1297	DECLARE @ErrorMessage NVARCHAR(4000);
-- WO#1297	DECLARE @ErrorSeverity INT;
-- WO#1297	DECLARE @ErrorState INT;
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

--	TRUNCATE table tblBillOfMaterials
		
--	SET @chrFacility = (SELECT value1 FROM tblControl WHERE ([Key] = 'Facility'))
	BEGIN TRANSACTION [Tran1]		-- WO#1297
	BEGIN TRY
		DELETE tblBillOfMaterials Where facility = @chrFacility
		
		SET @vchSQLStmt = 	
		'INSERT INTO tblBillOfMaterials ' +
		'SELECT * ' +
		'FROM ' + @vchImportData + '.dbo.tblBillOfMaterials WHERE facility = ''' + @chrFacility + ''''
			
		execute (@vchSQLStmt)
		COMMIT TRANSACTION [Tran1]	-- WO#1297	
	END TRY
	BEGIN CATCH
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
	END CATCH
END

GO

