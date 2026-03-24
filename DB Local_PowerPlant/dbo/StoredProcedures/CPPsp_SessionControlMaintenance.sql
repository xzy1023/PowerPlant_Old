

-- =============================================
-- Author:		Bong Lee
-- Create date: July 30, 2012
-- Description:	Session Control Maintenance
-- =============================================
CREATE PROCEDURE [dbo].[CPPsp_SessionControlMaintenance] 

	@vchAction VARCHAR(50)
	,@chrFacility char(3)= ''
	,@vchComputerName varchar(50) = NULL
	,@dteSOStartTime datetime = NULL
	,@dteSOStopTime datetime  = NULL
	,@chrDefaultPkgLine char(10) = NULL
	,@chrOverridePkgLine char(10) = ''
	,@intShopOrder int = 0
	,@vchItemNumber varchar(35) =' '
	,@vchOperator varchar(10) = ''
	,@dteLogOnTime datetime  = NULL
	,@tnyDefaultShiftNo tinyint = 0
	,@tnyOverrideShiftNo tinyint = 0
	,@intCasesScheduled int = 0
	,@intCasesProduced int = 0
	,@intPalletsCreated int = 0
	,@decBagLengthUsed decimal(3,1) = 0.0
	,@decReworkWgt decimal(8,2) = 0.00
	,@intLooseCases int = 0
	,@dteProductionDate datetime = NULL
	,@intCarriedForwardCases int = 0
	,@dteShiftProductionDate datetime = NULL
	,@ServerCnnIsOk bit = NULL
	,@intQuantity int = 0
	--,@intRowCount int = 0 OUTPUT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

		IF NOT EXISTS (Select 1 from tblSessionControl
							WHERE ComputerName = @vchComputerName)
		BEGIN
			IF @@servicename <> 'SQLEXPRESS'
			BEGIN
			
				INSERT INTO tblSessionControl
						 (Facility, ComputerName, StartTime, StopTime, DefaultPkgLine 
						  ,OverridePkgLine, ShopOrder, ItemNumber, Operator, LogOnTime
						  ,DefaultShiftNo 
						  ,OverrideShiftNo
						  ,CasesScheduled
						  ,CasesProduced
						  ,PalletsCreated 
						  ,BagLengthUsed
						  ,ReworkWgt
						  ,LooseCases
						  ,ProductionDate
						  ,CarriedForwardCases
						  ,ShiftProductionDate
						  )
				VALUES  (@chrFacility,@vchComputerName,@dteSOStartTime,@dteSOStopTime,@chrDefaultPkgLine,@chrOverridePkgLine
						,@intShopOrder,@vchItemNumber,@vchOperator,@dteLogOnTime
						,@tnyDefaultShiftNo
						,@tnyOverrideShiftNo
						,@intCasesScheduled
						,@intCasesProduced
						,@intPalletsCreated
						,@decBagLengthUsed
						,@decReworkWgt
						,@intLooseCases
						,ISNULL(@dteProductionDate, GETDATE())
						,@intCarriedForwardCases
						,ISNULL(@dteShiftProductionDate, GETDATE())
						)
			END
		END
		
		-- Session Control record exists
		IF @vchAction = 'UpdateCaseCount'
		BEGIN
		   UPDATE tblSessionControl SET
				CaseCounter = @intQuantity
				WHERE DefaultPkgLine = @chrDefaultPkgLine AND
					  --StartTime = @dteSOStartTime AND
					  ABS(datediff(second, StartTime,  @dteSOStartTime)) <= 1 AND
  					  ShopOrder = @intShopOrder AND
					  ShopOrder <> 0	
		END
			ELSE	
			IF @vchAction = 'StartShopOrder'
			BEGIN
				UPDATE tblSessionControl SET
						 DefaultPkgLine = @chrDefaultPkgLine
						,OverridePkgLine = @chrOverridePkgLine
						,Operator = @vchOperator
						,OverrideShiftNo = @tnyOverrideShiftNo
						,DefaultShiftNo = @tnyDefaultShiftNo
						,ShopOrder = @intShopOrder
						,StartTime = @dteSOStartTime
						,CasesScheduled = @intCasesScheduled
						,ItemNumber = @vchItemNumber
						,BagLengthUsed = @decBagLengthUsed
						,StopTime = NULL
						,CasesProduced = 0
						,ReworkWgt = 0
						,LooseCases = 0
						,PalletsCreated = 0
						,ProductionDate = @dteProductionDate
						,CarriedForwardCases = @intCarriedForwardCases
						,ShiftProductionDate = dbo.fnGetProdDateByShift(@chrFacility,@tnyOverrideShiftNo,@dteProductionDate,@chrOverridePkgLine,NULL)
						WHERE ComputerName = @vchComputerName
				END
				ELSE
					IF @vchAction = 'StopShopOrder'
					BEGIN
						UPDATE tblSessionControl SET
							  StopTime = @dteSOStopTime
							 ,CasesProduced = @intCasesProduced
							 ,BagLengthUsed = @decBagLengthUsed 
							 ,ReworkWgt = @decReworkWgt
							 ,LooseCases = @intLooseCases
							 ,CarriedForwardCases = @intCarriedForwardCases
							 WHERE ComputerName = @vchComputerName
					END
					ELSE
						IF @vchAction = 'Reset'
						BEGIN
						UPDATE tblSessionControl SET
								 ShopOrder = 0
								,StopTime = NULL
								,casesProduced = 0
								,PalletsCreated = 0
								,BagLengthUsed = 0
								,ReworkWgt = 0
								,LooseCases = 0
								,CarriedForwardCases = 0
								,ItemNumber = ''
								WHERE ComputerName = @vchComputerName
						END
						ELSE
							IF @vchAction = 'CreatePallet'
							BEGIN
								UPDATE tblSessionControl SET
									 LooseCases = 0
									,CasesProduced = CasesProduced +  @intQuantity
									,PalletsCreated = PalletsCreated + 1
									,ProductionDate = CONVERT(datetime,@dteProductionDate,120)
									WHERE ComputerName = @vchComputerName
							END
							ELSE
								IF	@vchAction = 'LogOn'
								BEGIN
									UPDATE tblSessionControl  SET
										 DefaultPkgLine = @chrDefaultPkgLine
										,OverridePkgLine = @chrOverridePkgLine
										,Operator = @vchOperator
										,OverrideShiftNo = @tnyOverrideShiftNo
										,LogOnTime = @dteLogOnTime
										WHERE ComputerName = @vchComputerName
								END
								--ELSE
								--IF	@vchAction = 'SetServerConnectionStatus'
								--	UPDATE tblSessionControl SET ServerCnnIsOK = @ServerCnnIsOk
								--	WHERE ComputerName = @vchComputerName
								
END

GO

