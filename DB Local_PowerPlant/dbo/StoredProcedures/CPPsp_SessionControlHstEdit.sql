

-- =============================================
-- Author:		Bong Lee
-- Create date: Sep 15, 2006
-- Description:	Session Control History IO
-- WO#755: Add Column CaseCounter
--		   Feb 01, 2012 Bong Lee
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_SessionControlHstEdit] 
	-- Add the parameters for the stored procedure here
	@vchAction VARCHAR(30),
	@chrFacility char(3)= '',
	@vchComputerName varchar(50)='',
	@dteStartTime datetime = NULL,
	@dteStopTime datetime  = NULL,
	@chrPkgLine char(10) = NULL,
	@chrOverridePkgLine char(10) = '',
	@intShopOrder int = 0,
	@vchItemNumber varchar(35) =' ',
	@vchOperator varchar(10) = '',
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
	@dteShiftProductionDate datetime = NULL
	,@intCaseCounter int = 0				--WO#755
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		IF @vchAction = 'Insert'
		BEGIN
			IF NOT EXISTS (Select 1 from tblSessionControlhst 
							WHERE DefaultPkgLine = @chrPkgLine 
							AND StartTime = @dteStartTime)
			BEGIN

			INSERT INTO tblSessionControlHst
                     (Facility, ComputerName, StartTime, StopTime, DefaultPkgLine, 
					  OverridePkgLine, ShopOrder, ItemNumber, Operator, LogOnTime, DefaultShiftNo, 
                      OverrideShiftNo, CasesScheduled, CasesProduced, PalletsCreated, 
					  BagLengthUsed, ReworkWgt, LooseCases, ProductionDate, CarriedForwardCases, ShiftProductionDate
					  ,CaseCounter --WO#755
					  )			   --WO#755
			VALUES  (@chrFacility,@vchComputerName,@dteStartTime,@dteStopTime,@chrPkgLine,@chrOverridePkgLine,
					@intShopOrder,@vchItemNumber,@vchOperator,@dteLogOnTime,@intDefaultShiftNo,@intOverrideShiftNo,
					@intCasesScheduled,@intCasesProduced,@intPalletsCreated,@decBagLengthUsed,@decReworkWgt,
					@intLooseCases,@dteProductionDate,@intCarriedForwardCases,@dteShiftProductionDate
					,@intCaseCounter --WO#755
					)  			     --WO#755
			END
		END	
		
END

GO

