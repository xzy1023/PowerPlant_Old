
-- =============================================
-- Author:		Bong Lee
-- Create date: Mar. 9 2010
-- Description:	Machine Paid Hours Maintenance
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_MachinePaidHours_Maint]
(
	@vchFacility varchar(3),
	@vchMachineID varchar(10),
	@intShopOrder int,
	@vchOperator varchar(10), 
	@dteShiftProductionDate as DateTime,
	@intShiftNo as tinyint,
	@PaidHours as decimal(5,2),
	@vchCreatedBy as varchar(50)
)
AS
BEGIN

	SET NOCOUNT ON;
	
	BEGIN TRY

	IF EXISTS (SELECT * FROM tblMachinePaidHours 
				WHERE Facility = @vchFacility 
					AND MachineID = @vchMachineID
					AND ShopOrder = @intShopOrder
					AND Operator = @vchOperator
					AND ShiftProductionDate = @dteShiftProductionDate
					AND ShiftNo = @intShiftNo) 
	BEGIN
		UPDATE tblMachinePaidHours SET PaidHours = @PaidHours,
				DateChanged = getdate(), ChangedBy = @vchCreatedBy
				WHERE Facility = @vchFacility 
					AND MachineID = @vchMachineID
					AND ShopOrder = @intShopOrder
					AND Operator = @vchOperator
					AND ShiftProductionDate = @dteShiftProductionDate
					AND ShiftNo = @intShiftNo
	END
	ELSE	
	BEGIN					
		INSERT INTO tblMachinePaidHours
            (Facility,MachineID,ShopOrder,Operator,ShiftProductionDate,ShiftNo,PaidHours,CreatedBy,ChangedBy)
			 VALUES (@vchFacility,@vchMachineID,@intShopOrder,@vchOperator, @dteShiftProductionDate,@intShiftNo, @PaidHours, @vchCreatedBy, @vchCreatedBy);
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

