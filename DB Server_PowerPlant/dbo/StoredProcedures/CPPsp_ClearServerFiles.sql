


-- =============================================
-- Author:		Bong Lee
-- Create date: Jan 31, 2007
-- Description:	Initialize files for Implementation
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_ClearServerFiles] 
	-- Add the parameters for the stored procedure here
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	truncate table dbo.tblCimControlJob
	truncate table dbo.tblComponentScrap
	truncate table dbo.tblDownTimeLog
	truncate table dbo.tblDynamicLabelData
	truncate table dbo.tblOperationStaffing
	truncate table dbo.tblPallet
	truncate table dbo.tblPalletHst
	truncate table dbo.tblPLCLog
	truncate table dbo.tblSessionControlHst
	truncate table dbo.tblWeightLog

	select count(*) from dbo.tblCimControlJob
	select count(*) from dbo.tblComponentScrap
	select count(*) from dbo.tblDownTimeLog
	select count(*) from dbo.tblDynamicLabelData
	select count(*) from dbo.tblOperationStaffing
	select count(*) from dbo.tblPallet
	select count(*) from dbo.tblPalletHst
	select count(*) from dbo.tblPLCLog
	select count(*) from dbo.tblSessionControlHst
	select count(*) from dbo.tblWeightLog

	update dbo.tblCurrentPalletNo set CurrentPalletNo = 609999999
END

GO

