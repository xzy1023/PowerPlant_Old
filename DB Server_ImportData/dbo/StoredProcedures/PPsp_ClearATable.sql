
-- =============================================
-- WO#1297      Oct. 1, 2014   Bong Lee
-- Description:	Clear a table
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ClearATable]
	@vchTableName as varchar(100),
	@vchFacility as varchar(3) = NULL
AS
BEGIN
	DECLARE @vchSQLStmt as varchar(1000);

	SET NOCOUNT ON;
	BEGIN TRY
		-- Clear table from the staging DB
		IF @vchFacility is NULL
		BEGIN
			SET @vchSQLStmt = 'Truncate table ' + @vchTableName + ';' 
		END
		ELSE
		BEGIN
			SET @vchSQLStmt = 'Delete FROM ' + @vchTableName + ' WHERE Facility = ''' + @vchFacility + ''';' 
		END 

		EXECUTE  (@vchSQLStmt);

	END TRY
	BEGIN CATCH
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
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ' Message: ' + CHAR(13) + ERROR_MESSAGE();

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)
	END CATCH
END

GO

