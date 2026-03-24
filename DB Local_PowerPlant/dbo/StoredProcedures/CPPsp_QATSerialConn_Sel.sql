
-- =============================================================================
-- Author:		
-- WO#17423:	Aug. 13, 2018	Bong Lee
-- Description:	Select QAT serial connection information
-- ==============================================================================
CREATE PROCEDURE [dbo].[CPPsp_QATSerialConn_Sel] 
	@vchFacility varchar(3) = NULL
	,@intSerialConnID int = NULL
	,@bitActive bit = 1

AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
	SELECT * from [dbo].[tblQATSerialConn]
			WHERE Facility = ISNULL(@vchFacility, Facility)
				AND SerialConnID = ISNULL(@intSerialConnID, SerialConnID )
				AND (@bitActive IS NULL OR Active = @bitActive)
	END TRY
	BEGIN CATCH
			THROW
	END CATCH
END

GO

