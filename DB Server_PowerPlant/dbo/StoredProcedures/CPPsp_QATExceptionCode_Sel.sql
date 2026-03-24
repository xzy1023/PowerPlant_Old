
-- =============================================================================
-- Author:		Bong Lee
-- WO#17423:	Mar. 11, 2019	Bong Lee
-- Description:	Select QAT Exception Code
-- ==============================================================================
CREATE PROCEDURE [dbo].[CPPsp_QATExceptionCode_Sel] 
	@vchFacility varchar(3) = NULL
	,@intExceptionCode int = NULL
	,@bitActive bit = 1
	,@intExceptionSubCode int = NULL

AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
	SELECT * FROM [dbo].[tblQATExceptionCode]
			WHERE Facility = ISNULL(@vchFacility, Facility)
				AND ExceptionCode = ISNULL(@intExceptionCode, ExceptionCode)
				AND Active = @bitActive
				AND ExceptionSubCode = ISNULL(@intExceptionSubCode, ExceptionSubCode)
	END TRY
	BEGIN CATCH
			THROW
	END CATCH
END

GO

