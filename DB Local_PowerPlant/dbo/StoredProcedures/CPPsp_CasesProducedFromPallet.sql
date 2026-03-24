



-- =============================================
-- Author:		Bong Lee
-- Create date: Mar. 6, 2009
-- Description:	Calculate Number of Cases Produced for a Shop Order Ffrom Created Pallets
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_CasesProducedFromPallet] 

	@vchAction varchar(50)=NULL,
	@chrFacility char(3),
	@chrPkgLine char(10),
	@intShopOrder int,
	@intShiftNo int,
	@dteGivenTime DateTime = NULL,
	@dteShiftProductionDate DateTime = NULL,
	@intCasesProduced int OUTPUT
	
AS
BEGIN

	SET NOCOUNT ON;
	
	BEGIN TRY

		IF @dteShiftProductionDate is NULL
			SELECT @dteShiftProductionDate =[dbo].[fnGetProdDateByShift] (Facility, @intShiftNo, @dteGivenTime , PackagingLine, WorkShiftType) from tblComputerConfig 
				Where Facility = @chrFacility AND PackagingLine = @chrPkgLine 
		

		-- If tblPalletHst is found in database, this procedure is run from common server else is from local server
		IF object_id('dbo.tblPalletHst') is not null
		BEGIN
			-- Cases produced for shop order
			IF @intShiftNo = 0 
			BEGIN
				Select @intCasesProduced = (Select Isnull(Sum(quantity),0) from tblPallethst Where shoporder = @intShopOrder)
							 + (Select Isnull(Sum(quantity),0) from tblPallet Where shoporder = @intShopOrder)
			END
			-- Cases produced for shop order in the given shift
			ELSE
			BEGIN

				Select @intCasesProduced = Isnull(Sum(quantity),0) from (
					Select * from tblPallethst 
						Where shoporder = @intShopOrder 
							AND ShiftProductionDate = @dteShiftProductionDate
							AND ShiftNo = @intShiftNo
					Union
					Select * from tblPallet 
						Where shoporder = @intShopOrder 
							AND ShiftProductionDate = @dteShiftProductionDate
							AND ShiftNo = @intShiftNo
					) as T1
			END
		END
		ELSE
		-- calculate from the local database	
		BEGIN
			IF @intShiftNo = 0 
			BEGIN
				Select @intCasesProduced = Isnull(Sum(quantity),0) from tblPallet 
					Where shoporder = @intShopOrder
			END
			ELSE
			-- Cases produced for shop order in the given shift
			BEGIN
				Select @intCasesProduced = Isnull(Sum(quantity),0) From tblPallet tPH
					Where shoporder = @intShopOrder 
					AND ShiftProductionDate = @dteShiftProductionDate
					AND ShiftNo = @intShiftNo
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

