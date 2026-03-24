


-- =============================================
-- Author:		Bong Lee
-- Create date: Nov 06, 2006
-- Description:	Item Master IO module
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_ItemMasterIO]
	@chrFacility char(3),
	@vchItemNo varchar(35),

	@vchAction varchar(30) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	If @vchAction = 'AllByItemNo'
		SELECT * FROM tblItemMaster Where Facility= @chrFacility AND itemnumber =  @vchItemNo
END

GO

