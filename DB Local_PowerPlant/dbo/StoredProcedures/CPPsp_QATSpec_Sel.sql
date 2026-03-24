
-- =============================================================================
-- Author:		
-- WO#17423:	Aug. 13, 2018	Bong Lee
-- Description:	Select QAT specification of the quanlity
-- ==============================================================================
CREATE PROCEDURE [dbo].[CPPsp_QATSpec_Sel] 
	@vchFacility varchar(3) = NULL
	,@intSpecID int = NULL
	,@bitActive bit = 1

AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
	SELECT * from [dbo].[tblQATSpec]
			WHERE Facility = ISNULL(@vchFacility, Facility)
				AND TestSpecID = ISNULL(@intSpecID, TestSpecID)
				AND (@bitActive IS NULL OR Active = @bitActive)
	END TRY
	BEGIN CATCH
			THROW
	END CATCH
END

GO

