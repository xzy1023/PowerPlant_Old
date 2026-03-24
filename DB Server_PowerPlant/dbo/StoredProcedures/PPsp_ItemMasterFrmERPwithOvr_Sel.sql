-- =============================================
-- Author:		Bong Lee
-- WO#:			1096
-- Create date: May. 20, 2014
-- Description:	Select item master data from ERP system with Item Label Override
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ItemMasterFrmERPwithOvr_Sel]
	-- Add the parameters for the stored procedure here
	@vchAction varchar(30) = NULL,
	@vchFacility varchar(3),
	@vchItemNumber varchar(35) 
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT * FROM [tblItemMasterFromERP] tIMERP
	LEFT OUTER JOIN	[tblItemLabelOvrr] tIMOVR
	ON tIMERP.Facility = tIMOVR.Facility AND tIMERP.ItemNumber = tIMOVR.ItemNumber
	WHERE tIMERP.Facility = @vchFacility and tIMERP.ItemNumber = @vchItemNumber
END

GO

