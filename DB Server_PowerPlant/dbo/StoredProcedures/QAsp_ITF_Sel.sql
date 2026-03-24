-- =============================================
-- Author:	Bong Lee
-- Create date: Apr. 5, 2011
-- Description:	Select Lotus Notes Instance Tracking data
-- =============================================
CREATE PROCEDURE QAsp_ITF_Sel 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT * FROM OPENQUERY(NOTES_ITF, 'SELECT CompNumber,"Date",PCAT ,Facility,Concern,ComOther,CName,Product,ProductNum,Brand,DateCodingEx,CodeDate,DateCreated FROM fmITF ') 
END

GO

