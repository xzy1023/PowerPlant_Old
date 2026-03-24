
-- =============================================
-- Author:		Bong Lee
-- Create date: Oct 20, 2006
-- Description:	Get Pallet No.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_GetPalletNo]
	@chrFacility char(3)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	Update tblCurrentPalletNo set CurrentPalletNo = (Case when CurrentPalletNo < HighestPalletno Then 
			CurrentPalletNo + 1 Else LowestPalletNo End) Where Facility = @chrFacility
	Select CurrentPalletNo from tblCurrentPalletNo Where Facility = @chrFacility
END

GO

