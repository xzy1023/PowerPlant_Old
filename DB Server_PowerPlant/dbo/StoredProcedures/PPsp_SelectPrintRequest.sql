
-- =============================================
-- Author:		Bong Lee
-- Create date: Oct 25, 2006
-- Description:	Select CimControlJob records
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_SelectPrintRequest]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	SELECT tblCimControlJob.* FROM tblCimControlJob ORDER BY RRN
END

GO

