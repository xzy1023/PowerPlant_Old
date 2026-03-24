
-- =============================================
-- Author:		Bong Lee
-- Create date: Jan 28, 2008
-- Description:	Select Bins from the related Grinder
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_GrinderBin_Sel] 
	-- Add the parameters for the stored procedure here
	@chrFacility char(3) , 
	@vchGrinder varchar(6) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT Bin from tblGrinderBin Where (Facility = @chrfacility) AND (Grinder = @vchGrinder) Order by Bin
END

GO

