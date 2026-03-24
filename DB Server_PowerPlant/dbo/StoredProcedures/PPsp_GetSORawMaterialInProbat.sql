

-- =============================================================================
-- Author:		Bong Lee
-- Create date: Jan. 23, 2019
-- Description:	Get raw material from Probat by shop order
-- ==============================================================================
CREATE PROCEDURE [dbo].[PPsp_GetSORawMaterialInProbat] 
	@intShopOrder				int	
	,@varFacility				varchar(3) 
	,@varRawMaterialID			varchar(35)	= NULL	OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @varSQLStmt			varchar(max)
	DECLARE @varShopOrder		varchar(15)
	DECLARE @varProbarDB		varchar(35)
	DECLARE @tblResults			TABLE(CUSTOMER_CODE sysname)

	BEGIN TRY

		SET @varShopOrder = CAST(@intShopOrder as varchar(15))

		SELECT @varProbarDB = [Value2]
		  FROM [tblControl]
		  WHERE [Facility] = @varFacility AND [Key] = 'ProbatInterfaceDB'

		SET @varSQLStmt = 'SELECT TOP 1 CUSTOMER_CODE FROM ( ' +
						'SELECT TOP 1 CUSTOMER_CODE ' +
						'FROM [' + @varProbarDB + '].[dbo].[PRO_EXP_ORDER_SEND_PG] ' +
						'WHERE ORDER_NAME = ''' + @varShopOrder + ''' ' +
						'UNION ' +
						'SELECT TOP 1 CUSTOMER_CODE ' +
						'FROM [' + @varProbarDB + '].[dbo].[PRO_EXP_ORDER_SEND_PB] ' +
						'WHERE ORDER_NAME = ''' + @varShopOrder + ''' ' +
						') T1 '

			INSERT @tblResults(CUSTOMER_CODE)
			EXEC (@varSQLStmt)
			SELECT @varRawMaterialID = CUSTOMER_CODE FROM @tblResults

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
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ', Message: ' + CHAR(13) + ERROR_MESSAGE();

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)
	END CATCH
END

GO

