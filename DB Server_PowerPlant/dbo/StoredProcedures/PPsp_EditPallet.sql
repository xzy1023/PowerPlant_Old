-- =============================================
-- Author:		Bong Lee
-- Create date: Oct 20, 2006
-- Description:	Edit Pallet
-- WO#297		Bong Lee		Apr. 15, 2011
-- Description:	Update UpdatedBy and LastUpPdate columns when @chrAction = 'SubmitedToPrint'
--				Add Repost Action	
-- WO#37864		Bong Lee		Feb. 03, 2021
-- Description:	when @chrAction = 'SubmitedToPrint', if the output location of the pallet is 'RAF' (i.e. Bulk-off) or
--				is NULL (i.e. non empty capsule lines), set PrintStatus = 1 for printing pallet label. 
--				Otherwise (i.e. direct feed lines), set PrintStatus = 2 to skip printing pallet label and ready for posting to ERP.
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_EditPallet]
	-- Add the parameters for the stored procedure here
	@chrAction varchar(20),
	@intPalletID int = 0,
	@vchJobName varchar(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	If @chrAction = 'Delete'
		DELETE FROM tblPallet WHERE (PalletID = @intPalletID)
	Else
	  If @chrAction = 'SubmitedToPrint'
		BEGIN																				-- WO#37864
-- WO#297 Del UPDATE tblPallet SET PrintStatus = 1 
		 UPDATE tblPallet SET PrintStatus = 1, LastUpdate = GetDate() 
			WHERE (PalletID = @intPalletID) AND PrintStatus = 0
			 AND (OutputLocation = 'RAF' or ISNULL(OutputLocation,'') ='')					-- WO#37864
		-- WO#37864 ADD Start
		 UPDATE tblPallet SET PrintStatus = 2, LastUpdate = GetDate() 
			WHERE (PalletID = @intPalletID) AND PrintStatus = 0
			 AND NOT (OutputLocation = 'RAF' or ISNULL(OutputLocation,'') ='')	
		END				
		-- WO#37864 ADD Stop
	  Else
		 If @chrAction = 'Printed'
			UPDATE tblPallet SET PrintStatus = 2 
				WHERE  PrintStatus = 1 AND PalletID IN 
				 (SELECT PalletID FROM tblDynamicLabelData WHERE LabelKey = @vchJobName)
		-- WO#297 Add Start
	     Else
			If @chrAction = 'RePost'
			UPDATE tblPallet SET PrintStatus = 2 
			WHERE (PalletID = @intPalletID) AND PrintStatus = 1	
		-- WO#297 Add Stop
END

GO

