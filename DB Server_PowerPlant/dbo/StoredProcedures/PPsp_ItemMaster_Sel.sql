
-- =============================================
-- Author:		Bong Lee
-- Create date: Jun 24, 2014
-- Description:	Select Item Master
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ItemMaster_Sel]
	@vchFacility varchar(3),
	@vchFromItemNo varchar(35) = NULL,
	@vchToItemNo varchar(35) = NULL,
	@vchImageFileName varchar(25) = NULL,
	@vchAction varchar(30) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	DECLARE @vchFileName varchar(27);

	IF @vchImageFileName IS not NULL
	   SET @vchFileName = '%' + @vchImageFileName + '%';
	    
	IF @vchToItemNo IS NULL 
		SET @vchToItemNo = @vchFromItemNo;

	IF @vchFromItemNo IS NULL 
		SET @vchFromItemNo = @vchToItemNo;

	SELECT * FROM tblItemMaster 
	WHERE Facility = @vchFacility 
		AND ((@vchFromItemNo is NULL AND @vchToItemNo is NULL) 
			OR (itemnumber BETWEEN @vchFromItemNo and @vchToItemNo))
		AND ((@vchImageFileName is null) 
				OR CaseLabelFmt1 like @vchFileName
				OR CaseLabelFmt2 like @vchFileName
				OR CaseLabelFmt3 like @vchFileName
				OR PackageCoderFmt1 like @vchFileName
				OR PackageCoderFmt2 like @vchFileName
				OR PackageCoderFmt3 like @vchFileName
				OR FilterCoderFmt like @vchFileName
			)
END

GO

