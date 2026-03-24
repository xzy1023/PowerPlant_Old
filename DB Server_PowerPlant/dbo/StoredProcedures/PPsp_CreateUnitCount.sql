

-- =============================================
-- Author:		Bong Lee
-- Create date: July 30, 2012
-- Description:	WO#5370 Insert record to the Inbound interface table
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_CreateUnitCount] 	
	
	@vchComputerName			varchar(50),
	@dteCreationTime			datetime,
	@intDestinationShopOrder	int,
	@vchFacility				varchar(3),
	@vchItemNumber				varchar(35),
	@vchOperator				varchar(10),
	@vchOrderChange				varchar(20),
	@vchOutputLocation			varchar(10),
	@vchPackagingLine			varchar(10),
	@chrShiftNo					char(1),
	@intShopOrder				int,
	@dteSOStartTime				Datetime,
	@intTxID					int,
	@intUnitCount				int,
	@intUnitsPerPallet			int
AS
BEGIN
	-- variables for error handler.
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
	DECLARE @ErrorProcedure nvarchar(200);
	DECLARE @ErrorLine int;
	DECLARE @ErrorNumber int;
	DECLARE @ErrorMessage NVARCHAR(4000);

	BEGIN TRY

		INSERT INTO tblUnitCountInbound
			(
				ComputerName
				,CreationTime
				,DestinationShopOrder
				,Facility
				,ItemNumber
				,Operator
				,OrderChange
				,OutputLocation
				,PackagingLine
				,ShiftNo
				,ShopOrder
				,SOStartTime
				,TxID
				,UnitCount
				,UnitsPerPallet
			)
			VALUES(
				@vchComputerName
				--,@dteCreationTime
				,GETDATE()
				,@intDestinationShopOrder
				,@vchFacility
				,@vchItemNumber
				,@vchOperator
				,@vchOrderChange
				,@vchOutputLocation
				,@vchPackagingLine
				,@chrShiftNo
				,@intShopOrder
				,@dteSOStartTime
				,@intTxID
				,@intUnitCount
				,@intUnitsPerPallet
				);

	END TRY
	BEGIN CATCH
			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine);
	END CATCH
END

GO

