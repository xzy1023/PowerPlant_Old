-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 28, 2015
-- Description:	Select Output Location 
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_OutputLocation_Sel]
	@vchAction varchar(30) = NULL,
	@vchFacility varchar(3),
	@vchPackagingline varchar(35)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
		SELECT * FROM tblOutputLocation 
			WHERE Facility = @vchFacility 
			  AND PackagingLine = @vchPackagingline
			  AND Active = 1
END

GO

