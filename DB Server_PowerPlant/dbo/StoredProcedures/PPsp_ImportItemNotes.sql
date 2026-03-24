

-- =============================================
-- Author:		Bong Lee
-- Create date: Nov 3, 2006
-- Description:	Copy Item Notes data from staging area to production area
-- WO#1297      Aug. 12, 2014   Bong Lee
-- Description: Remove column SequenceNo for the new interface with MS Dynamics AX 
-- ALM#11578	March. 15, 2016   Bong Lee	
-- Description: Add column Sequence No back to the table to handle text more than 500 characters long
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ImportItemNotes]
	@vchImportData as varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	DECLARE @vchSQLStmt varchar(1000);

	BEGIN TRANSACTION [Tran1]		-- WO#1297
	BEGIN TRY
	Truncate table tblItemNotes

	--Add New Records
	SET @vchSQLStmt = 
-- WO#1297 	'INSERT INTO tblItemNotes (ItemNumber, SequenceNo, [Text]) ' +	
-- WO#1297 	'SELECT ItemNumber, SequenceNo, [Text] ' +
--ALM#11578	'INSERT INTO tblItemNotes (ItemNumber, [Text]) ' +		-- WO#1097 
--ALM#11578	'SELECT ItemNumber, [Text] ' +							-- WO#1097 
 	'INSERT INTO tblItemNotes (ItemNumber, SequenceNo, [Text]) ' +		--ALM#11578
	'SELECT ItemNumber, SequenceNo, [Text] ' +							--ALM#11578
	'FROM ' + @vchImportData + '.dbo.tblItemNotes as T1'

	execute (@vchSQLStmt)
	COMMIT TRANSACTION [Tran1]	-- WO#1297	

	END try
	BEGIN Catch
		IF @@TRANCOUNT > 0					-- WO#1297
			ROLLBACK TRANSACTION [Tran1]	-- WO#1297

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
	END Catch
END

GO

