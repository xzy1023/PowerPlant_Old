
-- =============================================
-- Author:		Bong Lee
-- Create date: Nov.29, 2011
-- Description: Scrap Oomponent Selection
-- Task#6631	Aug. 31, 2015	Bong Lee
-- Description:	Use information from Dynamics AX
/* -- To Test --
EXEC	[PPsp_ScrapComponentWithBOM_Sel]
		@vchAction = NULL,
		@vchFacility = N'07',
		@vchMachineID = '3713-07   ',
		@intShopOrder = 10010053,
		@dteSessionStartTime = N'2015-08-04',

*/
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ScrapComponentWithBOM_Sel]
	@vchAction varchar(50) = NULL,
	@vchFacility varchar(3),
	@vchMachineID varchar(10),
	@intShopOrder int,
	@dteSessionStartTime datetime
AS
BEGIN

	SET NOCOUNT ON;

	-- Task#6631 DECLARE @chrEnvironment as char(1);
	DECLARE @vchSQLStmt as nvarchar(4000);
	/* Task#6631 DEL Start
	DECLARE @vchISeriesSQLStmt as nvarchar(3000);
	DECLARE @vchiSeriesName as nvarchar(10);
	DECLARE @vchUserLib varchar(10);
	DECLARE @vchOriginalLib varchar(10);
	Task#6631 DEL Stop */
	-- Task#6631 ADD Start
	DECLARE @vchAXSQLStmt as nvarchar(3000);
	DECLARE @vchCompanyNo nvarchar(10);
	DECLARE @vchServerName as nvarchar(50);
	DECLARE @vchDBName nvarchar(50);
	-- Task#6631 ADD Stop

	DECLARE @tblBOM TABLE 
	(
		Component varchar(35),
		UOM varchar(2)
	)

	BEGIN TRY
		/* Task#6631 DEL Start
		SELECT @chrEnvironment = UPPER(SUBSTRING(Value2,1,1)) from tblControl Where [Key] = 'Facility' and SubKey = 'General'
		SELECT @vchiSeriesName = Case When @chrEnvironment = 'P' Then Value1 Else Value2 END from tblControl Where [Key] = 'iSeriesNames' and SubKey = 'ServerNames'

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
		Task#6631 DEL Stop */	
		-- Task#6631 ADD Start
		SELECT @vchCompanyNo = Value2 FROM tblControl WHERE [Key] = 'Facility' AND SubKey = 'General'
		SELECT @vchServerName = Value1, @vchDBName = Value2 FROM tblControl WHERE [Key] = 'ERPEnv' AND SubKey = 'General'
		-- Task#6631 ADD Stop

		-- If the line is a tea line, add MixScrap & LabScrap components to the temporary BOM table
		If EXISTS (SELECT 1 FROM tblEquipment WHERE EquipmentID = @vchMachineID and SubType = 'T')
		BEGIN
			INSERT INTO @tblBOM VALUES('MixScrap', 'LB')
		END

		INSERT INTO @tblBOM VALUES('LabScrap', 'LB')
		/* Task#6631 DEL Start
		SET	@vchISeriesSQLStmt = N'SELECT MPROD as Component, IUMS as UOM FROM ' + @vchOriginalLib + '.FMA ' +
						'LEFT OUTER JOIN ' + @vchOriginalLib + '.IIM ON MPROD = IPROD WHERE MORD = ' +
						Cast(@intShopOrder as varchar(10)) + ' ORDER BY MSEQ Desc'

		SET @vchSQLStmt =  'EXEC(''' + @vchISeriesSQLStmt + ''') at ' + @vchiSeriesName
		Task#6631 DEL Stop */	

		SET	@vchAXSQLStmt = N'SELECT ItemID as Component, UnitID as UOM FROM ' + @vchDBName + '.dbo.ProdBOM WHERE PRODID =  ''''' +
							CAST(@intShopOrder as varchar(10)) + '''''  AND DATAAREAID = ''''' + @vchCompanyNo + '''''' 
		SET @vchSQLStmt = 'SELECT * FROM OPENQUERY([' + @vchServerName + '],''' + @vchAXSQLStmt + ''')';

	PRINT '@vchSQLStmt = ' + @vchSQLStmt 
		INSERT INTO @tblBOM
		EXEC sp_ExecuteSQL @vchSQLStmt

	--select * from @tblBOM
	--	SET @vchSQLStmt = N'SELECT tBOM.Component, tIM.ItemDesc1, tBOM.UOM, Quantity ' + 
	--					   'FROM OPENQUERY(' + @vchiSeriesName + ',''' + @vchISeriesSQLStmt + ''') tBOM ' +
	--					   'LEFT OUTER JOIN (SELECT * FROM tblComponentScrap WHERE ShopOrder = ' + Cast(@intShopOrder as varchar(10)) + ' AND StartTime = ''' + CONVERT(varchar(24),@dteSessionStartTime,121) + ''') tCS ' +
	--					   'ON tCS.ShopOrder = tBOM.ShopOrder AND tCS.Component = tBOM.Component ' +
	--					   'LEFT OUTER JOIN tblItemMaster tIM ' +
	--					   'ON tBOM.Component = tIM.ItemNumber '

		SELECT tBOM.Component
			,CASE tBOM.Component 
				WHEN 'MixScrap' THEN 'Mixed Scrap'
				WHEN 'LabScrap' THEN 'Lab. Scrap'
				ELSE tIM.ItemDesc1  
				END AS Description
			,tBOM.UOM
			,tCS.Quantity 
			FROM @tblBOM tBOM
			LEFT OUTER JOIN (SELECT * FROM tblComponentScrap 
								WHERE ShopOrder = @intShopOrder 
								AND StartTime = @dteSessionStartTime) tCS
--								AND convert(varchar(19),StartTime,120) = convert(varchar(19),@dteSessionStartTime,120)) tCS
			ON tCS.Component = tBOM.Component
			LEFT OUTER JOIN (SELECT ItemNumber,itemDesc1 + ' ' + itemDesc2 as itemDesc1 FROM tblItemMaster WHERE Facility = @vchFacility) tIM 
			ON tBOM.Component = tIM.ItemNumber
			

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

