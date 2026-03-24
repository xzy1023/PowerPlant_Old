



-- =============================================
-- Author:		Bong Lee
-- Create date: April 28, 2008
-- Description:	Copy Grinding Schedule from staging area to the distination area
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ImportGrindingSchedule]
	@chrFacility as char(3),
	@vchImportData as varchar(50),
	@intTodayPlusOne as int
AS
BEGIN
--	DECLARE @chrFacility varchar(3)
	DECLARE @vchSQLStmt varchar(1000)
	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	--	Clear the schedule which production date equals to or greater than the specified date

	BEGIN TRY
		DELETE tblGrindingSchedule Where GSCFAC = @chrFacility AND GSCPDTE >= @intTodayPlusOne
		
		SET @vchSQLStmt = 	
		'INSERT INTO tblGrindingSchedule ' +
		'SELECT * ' +
		'FROM ' + @vchImportData + '.dbo.tblGrindingSchedule WHERE GSCFAC = ''' + @chrFacility + '''' +
		' AND GSCPDTE >= ' + CAST(@intTodayPlusOne as varchar(8))

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

