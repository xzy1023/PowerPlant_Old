
-- =============================================
-- WO#17432		Bong Lee	Jul 09, 2018
-- Description:	Update Control Table
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_IPCControl_Upd]
	-- Add the parameters for the stored procedure here
	@vchControlKey varchar(50) 
	,@vchValue1 varchar(50) = NULL
	,@vchValue2 varchar(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY

		IF @vchValue1 IS NOT NULL AND @vchValue2 IS NOT NULL
			UPDATE       tblIPCControl
			SET          Value1 = @vchValue1, Value2 = @vchValue2
			WHERE        ControlKey = @vchControlKey
		ELSE
			IF @vchValue1 IS NOT NULL 
				UPDATE      tblIPCControl 
				SET			Value1 = @vchValue1
				WHERE		ControlKey = @vchControlKey
			ELSE
				IF @vchValue2 IS NOT NULL
				UPDATE      tblIPCControl 
				SET			Value2 = @vchValue2
				WHERE		ControlKey = @vchControlKey
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorProcedure nvarchar(200);
		DECLARE @ErrorState INT;
		
		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorState = ERROR_STATE();

		-- Use RAISERROR inside the CATCH block to return error
		-- information about the original error that caused
		-- execution to jump to the CATCH block.
		RAISERROR (@ErrorMessage -- Message text.
				   ,@ErrorSeverity -- Severity.
				   ,@ErrorProcedure -- Stored Procedure.
				   ,@ErrorState -- State.
				   );
	END CATCH;
END

GO

