
-- =============================================
-- Author:		Bong Lee
-- Create date: Nov. 10, 2006
-- Description:	Item Notes I/O Module
-- Task#2262:	Sep. 18, 2014	Bong Lee	
--				MP-I0057 Drop Sequence No and increase text to varchar(500 )
-- ALM#11578:	Add Sequence No back to the table and sort it by accesending
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_ItemNotesIO]
	-- Add the parameters for the stored procedure here
	@vchItemNumber varchar(35), 
	@vchAction varchar(30) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	If @vchAction = 'AllByItemNo'
		SELECT * FROM tblItemNotes Where ItemNumber = @vchItemNumber
-- Task#2262	ORDER BY SequenceNo
		ORDER BY SequenceNo			-- ALM#11578
END

GO

