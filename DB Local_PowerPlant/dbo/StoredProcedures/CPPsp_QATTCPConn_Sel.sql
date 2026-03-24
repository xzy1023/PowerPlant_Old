
-- =============================================================================
-- Author:		
-- WO#17423:	Sep. 26, 2018	Bong Lee
-- Description:	Select QAT TCP/IP connection information
-- ==============================================================================
CREATE PROCEDURE [dbo].[CPPsp_QATTCPConn_Sel] 
	@vchFacility varchar(3) = NULL
	,@intTCPConnID int = NULL
	,@bitActive bit = 1

AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
	SELECT * from [dbo].[tblQATTCPConn]
			WHERE Facility = ISNULL(@vchFacility, Facility)
				AND TCPConnID = ISNULL(@intTCPConnID, TCPConnID )
				AND (@bitActive IS NULL OR Active = @bitActive)
	END TRY
	BEGIN CATCH
			THROW
	END CATCH
END

GO

