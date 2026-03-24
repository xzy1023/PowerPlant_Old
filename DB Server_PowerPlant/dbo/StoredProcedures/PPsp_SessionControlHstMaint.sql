

-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 15, 2006
-- Description:	Session Control History Maintenance
-- WO#3593:		Bong Lee	Oct. 27, 2011
-- Description:	Allow to update Rework Weight
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_SessionControlHstMaint] 
	@vchAction VARCHAR(30),
	@vchCreatedBy varchar(50) = NULL,
	@chrFacility char(3)= NULL,
	@vchComputerName varchar(50)=NULL,
	@dteStartTime datetime = NULL,
	@dteStopTime datetime  = NULL,
	@chrPkgLine char(10) = NULL,
	@chrOverridePkgLine char(10) = NULL,
	@intShopOrder int = 0,
	@vchItemNumber varchar(35) =NULL,
	@vchOperator varchar(10) = NULL,
	@dteLogOnTime datetime  = NULL,
	@intDefaultShiftNo int = 0,
	@intOverrideShiftNo int = 0,
	@intCasesScheduled int = 0,
	@intCasesProduced int = 0,
	@intPalletsCreated int = 0,
	@decBagLengthUsed decimal(3,1) = 0.0,
	@decReworkWgt decimal(8,2) = 0.00,
	@intLooseCases int = 0,
	@dteProductionDate datetime = NULL,
	@intCarriedForwardCases int = 0,
	@dteShiftProductionDate datetime = NULL,
	@intRRN int = NULL,
	@RRN int = NULL
	
AS
BEGIN
	SET XACT_ABORT ON
	SET NOCOUNT ON;

	BEGIN TRY

	BEGIN TRANSACTION 

	IF @vchAction = 'Delete' 
		Set @intRRN = @RRN

	-- Update data to the Session Control History Table and write the before image of the record to the audit table.
	IF @vchAction = 'Update'
	BEGIN
		--Update tblSessionControlHst Set LogOnTime = @dteLogOnTime, StartTime = ISNULL(@dteStartTime,StartTime), StopTime=@dteStopTime, DefaultPkgLine = @chrPkgLine,
		Update tblSessionControlHst Set LogOnTime = @dteLogOnTime, StartTime = (CASE WHEN @dteStartTime is NULL OR CONVERT(VARCHAR(19),@dteStartTime, 120) = CONVERT(VARCHAR(19),StartTime, 120) THEN StartTime ELSE @dteStartTime END), 
			StopTime=@dteStopTime, DefaultPkgLine = @chrPkgLine,
			Operator = @vchOperator, OverrideShiftNo = @intOverrideShiftNo, ShiftProductionDate = @dteShiftProductionDate,
			CasesProduced = @intCasesProduced, PalletsCreated = @intPalletsCreated, LooseCases = @intLooseCases, CarriedForwardCases = @intCarriedForwardCases
			,ReworkWgt = @decReworkWgt	-- WO#359
		Output 'Update', @vchCreatedBy, Getdate(), Deleted.*
		Into tblSessionControlHstAudit
		Where RRN = @intRRN
	END
	ELSE	
		IF @vchAction = 'Insert'
		BEGIN
			IF NOT EXISTS (Select 1 from tblSessionControlhst 
							WHERE Facility = @chrFacility and DefaultPkgLine = @chrPkgLine 
							AND StartTime = @dteStartTime)
			BEGIN
				-- Get computer name from Computer Configuration table based on Machine Line ID
				Select @vchComputerName = ComputerName from tblcomputerconfig where facility = @chrFacility and recordstatus = '1' and PackagingLine = @chrPkgLine
				-- insert record and write the after image of the record to the audit log	
				INSERT INTO tblSessionControlHst
						 (Facility, ComputerName, StartTime, StopTime, DefaultPkgLine, 
						  OverridePkgLine, ShopOrder, ItemNumber, Operator, LogOnTime, DefaultShiftNo, 
						  OverrideShiftNo, CasesScheduled, CasesProduced, PalletsCreated, 
						  BagLengthUsed, ReworkWgt, LooseCases, ProductionDate, CarriedForwardCases, ShiftProductionDate)
				Output 'Insert', @vchCreatedBy, Getdate(), Inserted.*
				Into tblSessionControlHstAudit
				VALUES  (@chrFacility,@vchComputerName,@dteStartTime,@dteStopTime,@chrPkgLine,@chrOverridePkgLine,
						@intShopOrder,@vchItemNumber,@vchOperator,@dteLogOnTime,@intDefaultShiftNo,@intOverrideShiftNo,
						@intCasesScheduled,@intCasesProduced,@intPalletsCreated,@decBagLengthUsed,@decReworkWgt,
						@intLooseCases,@dteProductionDate,@intCarriedForwardCases,@dteShiftProductionDate)
			END
			ELSE
				Return 999
		END	
		ELSE
			-- Delete record and write the before image of the record to the audit log	
			IF @vchAction = 'Delete'
			BEGIN
				Delete From tblSessionControlHst 
				Output 'Delete', @vchCreatedBy, Getdate(), Deleted.*
				Into tblSessionControlHstAudit
				Where rrn = @RRN
			END
			ELSE
			IF @vchAction = 'Select'
			BEGIN
				Select * From tblSessionControlHst where rrn = @intRRN
			END

	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION 
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

