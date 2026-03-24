


-- =============================================
-- Author:		Bong Lee
-- Create date: Dec. 05, 2006
-- Description:	Get Production Date By Shift
-- =============================================
CREATE  PROCEDURE [dbo].[CPPsp_GetProdDateByShift]
	-- Add the parameters for the stored procedure here
	@chrFacility char(3),
	@intGivenShiftNo tinyint,
	@dteGivenDateTime datetime, 
	@vchMachineID varchar(10) = NULL,
	@vchWorkGroup varchar(3) = NULL,
	@dteProductionDate datetime OUTPUT

AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	Select @dteProductionDate = dbo.fnGetProdDateByShift (@chrFacility,@intGivenShiftNo,@dteGivenDateTime ,@vchMachineID,@vchWorkGroup)
END

GO

