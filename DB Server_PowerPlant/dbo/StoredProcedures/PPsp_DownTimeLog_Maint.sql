
-- =============================================
-- Author:		Bong Lee
-- Create date: Dec , 2009
-- Description:	Maintain Down Time Log
-- WO#498:		May 27, 2011	Bong Lee
-- Description:	Check dupliated down time is entered when update as well.
--				Add Machine Type, reason code and inactive for checking duplicate
--				Add OUTPUT parameter to indicate error
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_DownTimeLog_Maint] 
	@vchAction varchar(50), 
	@chrFacility	char(3) = Null,
	@intShopOrder	int = 0,
	@vchMachineType	varchar(3) = Null,
	@vchMachineSubType	varchar(10) = Null,
	@vchMachineID	varchar(10) = Null,
	@vchOperator	varchar(10) = Null,
	@vchTechnician	varchar(10) = Null,
	@dteDownTimeBegin	datetime = Null,
	@dteDownTimeEnd	datetime = Null,
	@intShift	tinyint = 0,
	@intReasonType	smallint = 0,
	@intReasonCode	smallint = 0,
	@vchComment	varchar(255) = Null,
	@vchCreatedBy	varchar(10) = Null,
	@dteCreationDate	datetime = Null,
	@vchUpdatedBy	varchar(10) = Null,
	@dteLastUpdated	datetime = Null,
	@dteShiftProductionDate	datetime = Null,
	@vchEventID varchar(50) = Null,
	@intOriginalRRN int = NULL,
	@vchRtnMsg varchar(150) output
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @intRRN as int

	BEGIN TRY
-- WO#498 ADD Start
	-- check record existance
	IF @vchAction <> 'DELETE'
	BEGIN
		SELECT Top 1 @intRRN = rrn FROM tblDownTimeLog
			WHERE Facility = @chrFacility 
			AND MachineType	= @vchMachineType	
			AND MachineID = @vchMachineID 
			AND INACTIVE = '0'					
			AND ReasonCode = @intReasonCode	
			AND (((@dteDownTimeBegin > DownTimeBegin AND @dteDownTimeBegin < DownTimeEnd)
				OR  (@dteDownTimeEnd > DownTimeBegin AND @dteDownTimeEnd < DownTimeEnd))
				OR  (DownTimeBegin > @dteDownTimeBegin AND DownTimeBegin < @dteDownTimeEnd)
				OR  (@dteDownTimeBegin = DownTimeBegin)
				OR  (@dteDownTimeEnd = DownTimeEnd))

	END
-- WO#498 ADD Stop

    IF @vchAction = 'ADD'
	Begin 
/* WO#498 DEL Start
		IF NOT EXISTS (
				Select 1 from tblDownTimeLog
				WHERE DownTimeBegin = @dteDownTimeBegin 
				AND Facility = @chrFacility 
				AND MachineID = @vchMachineID 
				AND DownTimeEnd = @dteDownTimeEnd
		)
 WO#498 DEL Stop */
		
	

		IF @intRRN is NULL	-- WO#498
		BEGIN
			INSERT INTO tblDownTimeLog
				  (InActive, Facility, ShopOrder, MachineType, MachineSubType, MachineID, Operator, Technician, DownTimeBegin, DownTimeEnd, Shift, ReasonType, 
				  ReasonCode, Comment, CreatedBy, CreationDate, UpdatedBy, LastUpdated, ShiftProductionDate, EventID)
			Output 'Add', @vchCreatedBy, Getdate(), Inserted.*
						Into tblDownTimeLogAudit
			VALUES  ('0',
					@chrFacility,
					isnull(@intShopOrder,0),
					@vchMachineType,
					@vchMachineSubType,
					@vchMachineID,
					@vchOperator,
					isnull(@vchTechnician,''),
					@dteDownTimeBegin,
					@dteDownTimeEnd,
					@intShift,
					@intReasonType,
					@intReasonCode,
					isnull(@vchComment,''),
					@vchCreatedBy,
					@dteCreationDate,
					@vchUpdatedBy,
					@dteLastUpdated,
					@dteShiftProductionDate,
					@vchEventID)
				SET @vchRtnMsg = '200 -- Down Time has been added.'		-- WO#498				
			End
			ELSE
				SET @vchRtnMsg = '901 -- Down Time with same machine, reason and time frame is already existed. Add failure.'		-- WO#498
-- WO#498			Return 999

	END
	ELSE
		IF @vchAction = 'UPDATE'
			Begin 
				IF @intRRN is NULL OR									-- WO#498
					(@intRRN is not null AND @intRRN = @intOriginalRRN)	-- WO#498
				BEGIN													-- WO#498
					UPDATE    tblDownTimeLog
					   SET    Facility = @chrFacility, ShopOrder = isnull(@intShopOrder,0), MachineType = @vchMachineType, MachineID = @vchMachineID,  
							  Operator = @vchOperator, Technician = isnull(@vchTechnician,''), DownTimeBegin = @dteDownTimeBegin, DownTimeEnd = @dteDownTimeEnd, 
							  Shift = @intShift, ReasonType = @intReasonType, ReasonCode = @intReasonCode, Comment = isnull(@vchComment,''), UpdatedBy = @vchUpdatedBy, 
							  LastUpdated = @dteLastUpdated, ShiftProductionDate = @dteShiftProductionDate
					Output 'Update', @vchUpdatedBy, Getdate(), Deleted.*
						Into tblDownTimeLogAudit
					WHERE     (RRN = @intOriginalRRN)
					SET @vchRtnMsg = '200 -- Down Time has been updated.'	-- WO#498	
				END															-- WO#498
				ELSE
				BEGIN
					SET @vchRtnMsg = '901 -- Changed Down Time is alerady existed on the same machine, reason and time frame. Update failure'		-- WO#498
				END
			End
		ELSE
		IF @vchAction = 'DELETE'
		Begin
			DELETE FROM tblDownTimeLog
				Output 'Delete', @vchCreatedBy, Getdate(), Deleted.*
					Into tblDownTimeLogAudit
				WHERE     (RRN = @intOriginalRRN)
			SET @vchRtnMsg = '200	-- Selected Down Time has been deleted.'	-- WO#498	

		End
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

