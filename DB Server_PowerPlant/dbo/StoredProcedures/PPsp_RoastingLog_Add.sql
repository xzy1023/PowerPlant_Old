

-- =============================================
-- Author:		Bong Lee
-- Create date: Sep. 16, 2008
-- Description:	Add data to Roasting Log
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_RoastingLog_Add] 
	@chrFacility	char(3),
	@dteDateTest	datetime,
	@intShift	tinyint,
	@intBatchNo	smallint,
	@vchRoasterNo	varchar(10),
	@vchBlend	varchar(6),
	@decColour	decimal(5, 2),
	@decMoisture	decimal(4, 2),
	@decFinalTemp	decimal(6, 2),
	@decQuench	decimal(5, 2),
	@vchRoasterInit	varchar(50),
	@chrRejected	char(1),
	@vchComments	varchar(512),
	@decSpecMin	decimal(5, 2),
	@decSpecMax	decimal(5, 2),
	@decSpecTarget	decimal(5, 2),
	@decMaxMoisture	decimal(4, 2),
	@decTargetMoisture	decimal(4, 2),
	@decMinMoisture	decimal(4, 2),
	@decSpecFinalTemp	decimal(6, 2)
 AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;
		INSERT INTO tblRoastingLog
		VALUES    (@chrFacility, @dteDateTest, @intShift, @intBatchNo, @vchRoasterNo, @vchBlend, @decColour, @decMoisture, @decFinalTemp, @decQuench, @vchRoasterInit, @chrRejected,  
                      @vchComments, @decSpecMin, @decSpecMax, @decSpecTarget, @decMaxMoisture, @decTargetMoisture, @decMinMoisture, @decSpecFinalTemp)

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

