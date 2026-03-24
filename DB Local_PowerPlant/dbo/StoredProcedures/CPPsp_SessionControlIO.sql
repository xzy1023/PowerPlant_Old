
-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 15, 2006
-- Description:	Session Control IO
-- WO#755: Put a blank between the last and first name
--		   Jan 04,2013 Bong Lee
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_SessionControlIO] 
	-- Add the parameters for the stored procedure here
	@chrAction VARCHAR(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		IF @chrAction = 'SelectAllFields'
		BEGIN
			SELECT TOP 1 T1.*, T2.ItemDesc1 AS ItemDesc,
				-- WO#755 T3.FirstName + '' + T3.LastName AS OperatorName
				T3.FirstName + ' ' + T3.LastName AS OperatorName	-- WO#755
			FROM tblSessionControl T1
			LEFT OUTER JOIN dbo.tblItemMaster T2
			ON T1.ItemNumber = T2.ItemNumber
			LEFT OUTER JOIN dbo.tblPlantStaff T3
			ON T1.Operator = T3.StaffID
		END
		ELSE
		BEGIN
			IF @chrAction = 'SetStartDownTime_On'
			BEGIN
				IF object_id('tblSessionControl') is not null
					update dbo.tblSessionControl set StartDownTime = getdate()	
			END
			ELSE
			BEGIN
				IF @chrAction = 'SetStartDownTime_Off'
		  		BEGIN
					IF object_id('tblSessionControl') is not null
					  update dbo.tblSessionControl set StartDownTime = NULL
				END
			END
		END
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
	
		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();

		-- Use RAISERROR inside the CATCH block to return error
		-- information about the original error that caused
		-- execution to jump to the CATCH block.
		RAISERROR (@ErrorMessage, -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH;
END

GO

