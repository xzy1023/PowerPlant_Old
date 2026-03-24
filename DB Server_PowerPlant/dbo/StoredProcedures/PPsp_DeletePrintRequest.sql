
-- =============================================
-- Author:		Bong Lee
-- Create date: Oct 25, 2006
-- Description:	Delete CimControlJob records
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_DeletePrintRequest]
	-- Add the parameters for the stored procedure here
	@intRRN int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

    DELETE FROM tblCimControlJob WHERE RRN = @intRRN
END

GO

