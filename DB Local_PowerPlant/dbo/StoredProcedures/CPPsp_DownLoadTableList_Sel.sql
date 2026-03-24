

-- =============================================================================
-- Author:		Bong Lee
-- Create date: Mar. 08, 2012
-- Description:	Select Down Load Table List
-- ==============================================================================
CREATE PROCEDURE [dbo].[CPPsp_DownLoadTableList_Sel] 
	@chrAction varchar(30) = NULL,
	@vchFacility varchar(3) = NULL,
	@bitActive bit = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRY
		IF @chrAction = 'ListAll'
		BEGIN
			SELECT *
			FROM   importdata.dbo.tblDownLoadTableList 
			WHERE  (@vchFacility is null OR Facility = @vchFacility)
				AND (@bitActive is null OR Active = @bitActive)
		END
		ELSE
			IF @chrAction = 'LastDownLoadTime'
			BEGIN
				SELECT  MAX(LastDownLoad) as LastDownLoadTime
				FROM   importdata.dbo.tblDownLoadTableList
				WHERE  (@vchFacility is null OR Facility = @vchFacility)
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

