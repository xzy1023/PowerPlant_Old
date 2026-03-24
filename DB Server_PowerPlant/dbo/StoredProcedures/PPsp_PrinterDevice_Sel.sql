

-- =============================================================================
-- Author:		Bong Lee
-- Create date: Mar. 06, 2012
-- Description:	Select Packaging Line Printer Device
--
-- Mod.			Date			Author
-- WO#755:		Mar. 18, 2013	Bong Lee
-- Description:	When Action is 'VerifyConnection', shows unique printers only
-- ==============================================================================
CREATE PROCEDURE [dbo].[PPsp_PrinterDevice_Sel] 
	@chrAction varchar(30) = NULL,
	@chrFacility char(3) = NULL,
	@chrPackagingLine char(10) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRY
		IF @chrAction = 'ListAll'
		BEGIN
			SELECT *
			FROM   dbo.tblPkgLinePrinterDevice 
			WHERE  (@chrFacility is null OR Facility = @chrFacility)
				AND (@chrPackagingLine is null OR PackagingLine = @chrPackagingLine)
		END
		ELSE
		IF @chrAction = 'VerifyConnection'
		BEGIN
			DECLARE @vchLine as varchar(15)
			SET @vchLine = '%' + @chrPackagingLine + '%'
			SELECT DeviceName,IPaddress
			FROM   dbo.tblPkgLinePrinterDevice 
			WHERE  (@chrFacility is null OR Facility = @chrFacility)
				AND (@chrPackagingLine is null OR PackagingLine like @chrPackagingLine)
			Group By DeviceName,IPaddress	-- WO#755
			Order By DeviceName
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
	END CATCH
END

GO

