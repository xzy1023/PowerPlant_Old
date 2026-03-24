

-- =============================================
-- Author:		Bong Lee
-- Create date: May. 29, 2009
-- Description:	Green Bean Runout report
-- WO#689		May, 30, 2012	Bong Lee	
--				Add a parameter for requesting tea or coffee
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RawMaterialRunout] 
	-- Add the parameters for the stored procedure here
	@chrReportType char(1), 
	@chrFacility char(3)
	,@chrCoffeeOrTea char(1) -- WO#689

AS
BEGIN
	--DECLARE @intProductionDate as int
	DECLARE @vchProcEnv as varchar(50)
	DECLARE @vchiSeriesServerName as varchar(50)
	DECLARE @vchUserLib varchar(10)
	DECLARE @vchOriginalLib varchar(10)
	DECLARE @vchSQLStmt varchar(1000)

	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);

	BEGIN TRY

	SET NOCOUNT ON;

	Select @vchProcEnv = Rtrim(Substring(DB_NAME(),Charindex('_',DB_NAME())+1,10))

	If @vchProcEnv = 'PRD'
	BEGIN
		Select @vchiSeriesServerName = value1 From tblControl Where [key] = 'iSeriesNames'
		Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibPRD'
	END
	ELSE
	BEGIN
		If @vchProcEnv = 'UA'
		BEGIN
			Select @vchiSeriesServerName = value2 From tblControl Where [key] = 'iSeriesNames'
			Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibUA'
		END
		ELSE
		BEGIN
			Select @vchiSeriesServerName = value2 From tblControl Where [key] = 'iSeriesNames'
			Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibDev'
		END
	END 
	
	Select @vchUserLib = Substring(@vchUserLib,1,Len(@vchUserLib)-1) + 'R'

	/* get data from BPCS */

	-- WO#689 SET @vchSQLStmt =  'EXEC(''CALL ' + @vchUserLib + '.SFC$730S (''''' + @chrFacility + ''''','''''+ @chrReportType + ''''')'') at ' + @vchiSeriesServerName
	SET @vchSQLStmt =  'EXEC(''CALL ' + @vchUserLib + '.SFC$730S (''''' + @chrFacility + ''''','''''+ @chrReportType + ''''','''''+ @chrCoffeeOrTea + ''''')'') at ' + @vchiSeriesServerName -- WO#689
	Exec (@vchSQLStmt)
	
	END TRY
	BEGIN CATCH
		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorNumber = ERROR_NUMBER(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');

		RAISERROR 
			(
			@ErrorMessage, 
			@ErrorSeverity, 
			@ErrorState,     -- parameter: original error state.               
			@ErrorNumber,    -- parameter: original error number.
			@ErrorSeverity,  -- parameter: original error severity.
			@ErrorProcedure, -- parameter: original error procedure name.
			@ErrorLine       -- parameter: original error line number.
			);

	END CATCH
END

GO

