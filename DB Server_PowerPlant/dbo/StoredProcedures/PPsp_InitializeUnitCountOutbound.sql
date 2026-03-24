

-- =============================================
-- Author:		Bong Lee
-- Create date: July 30, 2012
-- Description:	WO#5370 Update the Outbound interface table
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_InitializeUnitCountOutbound] 	
	
	@vchComputerName			varchar(50),
	@dteCreationTime			datetime,
	@vchFacility				varchar(3),
	@vchItemNumber				varchar(35),
	@vchOperator				varchar(10),
	@vchOrderChange				varchar(20),
	@vchPackagingLine			varchar(10),
	@chrShiftNo					char(1),
	@intShopOrder				int,
	@dteSOStartTime				Datetime,
	@intUnitCount				int,
	@intUnitsPerPallet			int
AS
BEGIN
	DECLARE @intDestinationShopOrder	int;
	DECLARE @vchOutputLocation			varchar(10);

	-- variables for error handler.
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
	DECLARE @ErrorProcedure nvarchar(200);
	DECLARE @ErrorLine int;
	DECLARE @ErrorNumber int;
	DECLARE @ErrorMessage NVARCHAR(4000);

	BEGIN TRY
		IF EXISTS (SELECT * FROM tblUnitCountOutbound WHERE PackagingLine = @vchPackagingLine)
		BEGIN
			UPDATE tblUnitCountOutbound
			SET ComputerName = @vchComputerName
				,CreationTime = @dteCreationTime
				,Facility = @vchFacility
				,ItemNumber = @vchItemNumber
				,Operator = @vchOperator
				,OrderChange = @vchOrderChange
				,ShiftNo = @chrShiftNo
				,ShopOrder = @intShopOrder
				,SOStartTime = @dteSOStartTime
				,UnitCount = @intUnitCount
				,UnitsPerPallet =  @intUnitsPerPallet
			WHERE PackagingLine = @vchPackagingLine
		END
		ELSE
		BEGIN
			SELECT  @intDestinationShopOrder = 0
				  , @vchOutputLocation = ''
			INSERT INTO tblUnitCountOutbound
			  (
			   [ComputerName]
			  ,[CreationTime]
			  ,[DestinationShopOrder]
			  ,Facility
			  ,[ItemNumber]
			  ,[Operator]
			  ,[OrderChange]
			  ,[OutputLocation]
			  ,[PackagingLine]
			  ,[ShiftNo]
			  ,[ShopOrder]
			  ,[SOStartTime]
			  ,[UnitCount]
			  ,[UnitsPerPallet]
			  )
			VALUES(
				@vchComputerName
				,@dteCreationTime
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
				,@intUnitCount 
				,@intUnitsPerPallet
				) 
		END

	END TRY
	BEGIN CATCH
			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine);
	END CATCH
END

GO

