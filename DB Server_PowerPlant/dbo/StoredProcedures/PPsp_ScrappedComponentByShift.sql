
-- =============================================
-- Author:		Bong Lee
-- Create date: Jul. 14, 2011
-- Description:	Scrapped Component by Shift Production Date & Shift
-- WO#359		Jan. 30, 2012	Bong Lee
-- Description: use the ShiftSequence, ShiftDesc from tfnSessionControlHstDetail
-- WO#325		Oct. 21, 2013	Bong Lee
-- Description:	Show special scraps
-- Task#6631		Aug. 31, 2015	Bong Lee
-- Description:		Use the item type in tblItemMaster to determine the component is raw material or not.
/* -- To Test --
EXEC	[PPsp_ScrappedComponentByShift]
		@vchAction = N'ByOperator',
		@vchComponentType = N'RawMaterial',
		@vchFacility = N'01',
		@dteFromDate = N'7/14/2011',
		@intFromShift = 1,
		@dteToDate = N'7/14/2011',
		@intToShift = 3,
		@intShift = NULL,
		@vchWorker = NULL
*/
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ScrappedComponentByShift]
	@vchAction varchar(50) = NULL,
	@vchComponentType varchar(15),
	@vchFacility varchar(3),
	@dteFromDate datetime , 
	@intFromShift int, 
	@dteToDate datetime,
	@intToShift int, 
	@intShift int = NULL,
	@vchWorker varchar(10) = NULL
AS
BEGIN
	DECLARE @chrEnvironment as char(1)
	DECLARE @vchSQLStmt as nvarchar(3000)
	DECLARE @vchISeriesSQLStmt as nvarchar(300)
	DECLARE @vchOrderBy as nvarchar(200)
	DECLARE @iSeriesName as nvarchar(10)
	DECLARE @vchUserLib varchar(10)
	DECLARE @vchOriginalLib varchar(10)
	DECLARE @vchFromShopOrder varchar(25)
	DECLARE @vchToShopOrder varchar(25)

	-- Declare Constants						--Task#6631
	DECLARE @cstComponentType varchar(25)			--Task#6631
	DECLARE @cstItemType varchar(25)			--Task#6631

/* Task#6631 DEL Start
	DECLARE @tIIM Table (
		IPROD varchar(35)
	)
Task#6631  DEL Stop */
	SET NOCOUNT ON;
	BEGIN TRY

		SELECT @cstComponentType = 'RawMaterial'		--Task#6631
			   ,@cstItemType = 'SubWIP'					--Task#6631
			   


	/* Task#6631 DEL Start
		SELECT @chrEnvironment = UPPER(SUBSTRING(Value2,1,1)) from tblControl Where [Key] = 'Facility' and SubKey = 'General'
		SELECT @iSeriesName = Case When @chrEnvironment = 'P' Then Value1 Else Value2 END from tblControl Where [Key] = 'iSeriesNames' and SubKey = 'ServerNames'

	 -- Check the processing environment. Is production or UA?
		If @chrEnvironment = 'P'
		BEGIN
			Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibPrd'
		END
		ELSE
		BEGIN
			If @chrEnvironment = 'U'
			BEGIN
				Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibUA'
			END
			ELSE
			BEGIN
				Select @vchUserLib = value1, @vchOriginalLib = value2 From tblControl Where [key] = 'BPCSDataLibDev'
			END
		END 
		
		IF @vchComponentType = 'RawMaterial'
		BEGIN
			SELECT @vchISeriesSQLStmt = N'''SELECT IPROD FROM ' + @vchOriginalLib + '.iim WHERE (ipfdv = ''''BEAN'''' or ipfdv = ''''GRND'''') Order by iprod'''

		END		
		ELSE
		BEGIN
			SELECT @vchISeriesSQLStmt = N'''SELECT IPROD FROM ' + @vchOriginalLib + '.iim WHERE ipfdv <>''''BEAN'''' AND ipfdv <> ''''GRND'''' Order by iprod'''
		END
		
		SELECT @vchSQLStmt = 'Select * from openquery(' + @iSeriesName + ',' + @vchISeriesSQLStmt + ')' 
	
		INSERT INTO @tIIM
		EXEC sp_ExecuteSQL @vchSQLStmt
		Task#6631  DEL Stop */

		IF @vchAction = 'ByOperator'
		BEGIN
-- WO#325	SELECT tCS.Component, tIM.ItemDesc1 as ItemDesc, tSCH.ShopOrder, tSCH.DefaultPkgLine, tEqt.Description, tSCH.ShiftProductionDate
-- WO#359		,tS.ShiftSequence, tS.Description as ShiftDesc, tCS.Quantity, tSCH.Operator as Worker, tPS.FirstName + ' ' + tPS.LastName as WorkerName
			SELECT tCS.Component, ISNULL(tIM.ItemDesc1,tSS.Description) as ItemDesc						--WO#325 
				,tSCH.ShopOrder, tSCH.DefaultPkgLine, tEqt.Description, tSCH.ShiftProductionDate		--WO#325 
				,tSCH.ShiftSequence, tSCH.ShiftDesc															-- WO#359
				,tCS.Quantity, tSCH.Operator as Worker, tPS.FirstName + ' ' + tPS.LastName as WorkerName	-- WO#359
			FROM tblComponentScrap tCS
			INNER JOIN tfnSessionControlHstDetail(NULL, @vchFacility, NULL, @vchWorker, NULL, NULL, @dteFromDate, @intFromShift,@dteToDate, @intToShift) tSCH
			ON tCS.ShopOrder = tSCH.ShopOrder AND CONVERT(varchar(19),tCS.StartTime,120) = CONVERT(varchar(19),tSCH.StartTime,120)
			--Task#6631	INNER JOIN @tIIM tIIM
			--Task#6631	ON tCS.Component = tIIM.iprod
			LEFT OUTER JOIN tblItemMaster tIM
			ON tCS.Facility = tIM.Facility AND tCS.Component = tIM.ItemNumber
			LEFT OUTER JOIN tblSpecialScrap tSS											--WO#325
			ON tCS.Component = tSS.Component											--WO#325
			LEFT OUTER JOIN dbo.tblPlantStaff tPS 
			On tCS.Facility = tPS.Facility AND tSCH.Operator = tPS.StaffID 
			LEFT OUTER JOIN dbo.tblEquipment tEqt
			ON tCS.Facility = tEqt.Facility AND tSCH.DefaultPkgLine = tEqt.EquipmentID 
-- WO#359 Del Start			
			--LEFT OUTER JOIN vwLineWorkShiftType vLWT
			--ON tCS.Facility = vLWT.Facility AND tSCH.DefaultPkgLine = vLWT.PackagingLine
			--LEFT OUTER JOIN tblShift tS
			--ON tCS.Facility = tS.Facility AND  vLWT.WorkShiftType = tS.WorkGroup AND  tSCH.OverrideShiftNo = tS.Shift
-- WO#359 Del Stop			
			WHERE tCS.Facility = @vchFacility AND tSCH.ShiftProductionDate BETWEEN @dteFromDate AND @dteToDate
				AND (@vchWorker IS NULL OR tSCH.Operator = @vchWorker)
				AND (@intShift IS NULL OR tSCH.OverrideShiftNo = @intShift)
				AND ((@vchComponentType = @cstComponentType AND itemtype = @cstItemType)		--Task#6631
				OR (@vchComponentType <> @cstComponentType AND itemtype <> @cstItemType))		--Task#6631
		END
		ELSE
		IF @vchAction = 'ByUtilityTech'
		BEGIN
--WO#325	SELECT tCS.Component, tIM.ItemDesc1 as ItemDesc, tSCH.ShopOrder, tSCH.DefaultPkgLine, tEqt.Description, tSCH.ShiftProductionDate
			SELECT tCS.Component, ISNULL(tIM.ItemDesc1,tSS.Description) as ItemDesc							-- WO#325 
				,tSCH.ShopOrder, tSCH.DefaultPkgLine, tEqt.Description, tSCH.ShiftProductionDate			-- WO#325
				,tSCH.ShiftSequence, tSCH.ShiftDesc															-- WO#359
				,tSCH.OverrideShiftNo as Shift,	tCS.Quantity, tOS.StaffID as Worker, tPS.FirstName + ' ' + tPS.LastName as WorkerName
			FROM tblComponentScrap tCS
			INNER JOIN tfnSessionControlHstDetail(NULL, @vchFacility, NULL, NULL, NULL, NULL, @dteFromDate, @intFromShift,@dteToDate, @intToShift) tSCH
			ON tCS.ShopOrder = tSCH.ShopOrder AND CONVERT(varchar(19),tCS.StartTime,120) = CONVERT(varchar(19),tSCH.StartTime,120)
			--Task#6631	INNER JOIN @tIIM tIIM
			--Task#6631	ON tCS.Component = tIIM.iprod
			INNER JOIN [tblOperationStaffing] tOS
			On tSCH.Facility = tOS.Facility and tSCH.DefaultPkgLine = tOS.PackagingLine And tSCH.[StartTime] = tOS.[StartTime]
			LEFT OUTER JOIN tblItemMaster tIM
			ON tCS.Facility = tIM.Facility AND tCS.Component = tIM.ItemNumber
			LEFT OUTER JOIN tblSpecialScrap tSS											--WO#325
			ON tCS.Component = tSS.Component											--WO#325
			LEFT OUTER JOIN dbo.tblPlantStaff tPS 
			On tCS.facility = tPS.facility AND tOS.StaffID = tPS.StaffID 
			LEFT OUTER JOIN dbo.tblEquipment tEqt
			ON tCS.facility = tEqt.facility AND tSCH.DefaultPkgLine = tEqt.EquipmentID 
-- WO#359 Del Start			
			--LEFT OUTER JOIN vwLineWorkShiftType vLWT
			--ON tCS.Facility = vLWT.Facility AND tSCH.DefaultPkgLine = vLWT.PackagingLine
			--LEFT OUTER JOIN tblShift tS
			--ON tCS.Facility = tS.Facility AND  vLWT.WorkShiftType = tS.WorkGroup AND  tSCH.OverrideShiftNo = tS.Shift
-- WO#359 Del Stop	
			WHERE tCS.Facility = @vchFacility AND tSCH.ShiftProductionDate BETWEEN @dteFromDate AND @dteToDate
				AND (@vchWorker IS NULL OR tOS.StaffID = @vchWorker)
				AND (@intShift IS NULL OR tSCH.OverrideShiftNo = @intShift)
				AND ((@vchComponentType = @cstComponentType AND itemtype = @cstItemType)		--Task#6631
				OR (@vchComponentType <> @cstComponentType AND itemtype <> @cstItemType))		--Task#6631
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
	END CATCH
END

GO

