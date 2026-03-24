
-- =============================================
-- Author:		Bong Lee
-- Create date: Jan 31, 2007
-- Description:	Initialize files for Implementation
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_ClearLocalFiles] 
	-- Add the parameters for the stored procedure here
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	truncate table dbo.tblComponentScrap
	truncate table dbo.tblDownTimeLog
	truncate table dbo.tblOperationStaffing
	truncate table dbo.tblPallet
	truncate table dbo.tblPLCLog
	truncate table dbo.tblSessionControlHst
	truncate table dbo.tblWeightLog
END

GO

