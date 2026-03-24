
-- =============================================
-- Author:		Bong Lee
-- Create date: Jun 13, 2014	WO#1297
-- Description:	Copy Item master data from staging area to production area (Rewrite)
-- Create date: Aug 27, 2015	Fix20150827
-- Description:	Field length of ItemDesc3 changed in item master table
-- Create date: Oct 09, 2015	Fix20151009
-- Description:	If the insert item is already existed in Power Plant, change action code to 'U' for update
-- Create date: Jan 29, 2016	Fix20160129
-- Description:	Change Tie and Tier to integer
-- WO#20797	  : Dec 21, 2018	Bong Lee
-- Description:	Import 3 more fields from AX - GrsDepth, GrsHeight and GrsWidth and 
--				import 2 more fields fromtblItemLabelOvrr
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ImportItemMaster]
	@vchInputFacility as varchar(3),
	@vchImportData as varchar(50)
AS
BEGIN
--	DECLARE @vchInputFacility varchar(3)
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	--DECLARE @vchSQLStmt varchar(4000)
	-- WO#20797	DECLARE	@nvchSQLStmt nvarchar(4000);
	DECLARE	@nvchSQLStmt nvarchar(MAX);							-- WO#20797
	DECLARE	@nvchParmDefinition nvarchar(500);
	DECLARE @bitNewItem as bit;									-- Fix20151009

	DECLARE @chrAction as char(1)
			,@vchFacility as varchar(3)
			,@vchItemDesc1 as varchar(50)
			,@chrItemMajorClass as varchar(10)
			,@vchItemNumber as varchar(35)
			,@vchItemType as varchar(10)
			,@decLabelWeight as dec(10,3)
			,@chrLabelWeightUOM as char(2)
			,@vchNetWeight as varchar(10)
			,@decPackagesPerSaleableUnit as dec(5,0)
			,@vchPackSize as varchar(12)
			,@intProductionShelfLifeDays as int
			,@intQtyPerPallet as int
			,@decSaleableUnitPerCase as dec(5,0)
			,@vchSCCCode as varchar(14)
			,@intShipShelfLifeDays as int
			--,@decTie as dec(6,3)
			--,@decTier as dec(6,3)
			,@decTie as int
			,@decTier as int
			,@vchUPCCode as varchar(14)
			,@decStdCostPerLB as dec(15,5)
			-- WO#20797 ADD Start
			,@decGrsDepth as dec(16,8)				
			,@decGrsHeight as dec(16,8)			
			,@decGrsWidth as dec(16,8)	
			-- WO#20797 ADD Stop
	SET XACT_ABORT ON;

	BEGIN TRY

		/* 
			Update tblItemMasterFromERP from the ERP delta transactions
		*/	
		SET @nvchSQLStmt = 
			'DECLARE csrTx CURSOR FOR ' +
				'SELECT Action, Facility, ItemDesc1, ItemMajorClass, ItemNumber, ItemType, LabelWeight, LabelWeightUOM ' +
					',NetWeight, PackagesPerSaleableUnit, PackSize, ProductionShelfLifeDays, QtyPerPallet ' +
					',SaleableUnitPerCase, SCCCode, ShipShelfLifeDays, Tie, Tier, UPCCode, StdCostPerLB ' +
					',GrsDepth ,GrsHeight, GrsWidth ' +																	-- WO#20797
					'FROM ' + @vchImportData + '.dbo.tblItemMasterTxFromERP ' +
					'WHERE Processed = 0 AND Facility = @vchInputFacility ' +
					'ORDER BY TxID; ' 
			--'FOR UPDATE OF ProcessedFlag;'

		SET @nvchParmDefinition = N'@vchInputFacility varchar(3)';
		EXEC sp_executesql @nvchSQLStmt, @nvchParmDefinition, @vchInputFacility;
		OPEN csrTx;

		FETCH NEXT FROM csrTx INTO @chrAction ,@vchFacility	,@vchItemDesc1 ,@chrItemMajorClass ,@vchItemNumber
			,@vchItemType ,@decLabelWeight ,@chrLabelWeightUOM ,@vchNetWeight ,@decPackagesPerSaleableUnit	
			,@vchPackSize ,@intProductionShelfLifeDays ,@intQtyPerPallet ,@decSaleableUnitPerCase ,@vchSCCCode	
			,@intShipShelfLifeDays ,@decTie	,@decTier ,@vchUPCCode, @decStdCostPerLB
			,@decGrsDepth, @decGrsHeight ,@decGrsWidth															-- WO#20797
		WHILE @@FETCH_STATUS = 0
		BEGIN
			-- Fix20151009 ADD Start
			SET @bitNewItem = 0;
			IF NOT EXISTS (SELECT 1 FROM tblItemMasterFromERP WHERE Facility = @vchFacility and  ItemNumber = @vchItemNumber)
				SET @bitNewItem = 1;

			IF @chrAction = 'I' AND @bitNewItem = 0
				SET @chrAction = 'U';
			ELSE 
				IF @chrAction = 'U' AND @bitNewItem = 1
					SET @chrAction = 'I';	
			-- Fix20151009 ADD Stop

			BEGIN TRANSACTION;

			IF @chrAction = 'I'
				INSERT INTO tblItemMasterFromERP
					(Facility, ItemDesc1, ItemMajorClass, ItemNumber, ItemType, LabelWeight, LabelWeightUOM
					,NetWeight, PackagesPerSaleableUnit, PackSize, ProductionShelfLifeDays, QtyPerPallet
					,SaleableUnitPerCase, SCCCode, ShipShelfLifeDays, Tie, Tier, UPCCode, StdCostPerLB
					,GrsDepth, GrsHeight, GrsWidth																		-- WO#20797	
					) 
				VALUES(@vchFacility	,@vchItemDesc1 ,@chrItemMajorClass ,@vchItemNumber
					,@vchItemType ,@decLabelWeight ,@chrLabelWeightUOM ,@vchNetWeight ,@decPackagesPerSaleableUnit	
					,@vchPackSize ,@intProductionShelfLifeDays ,@intQtyPerPallet ,@decSaleableUnitPerCase ,@vchSCCCode	
					,@intShipShelfLifeDays ,@decTie	,@decTier ,@vchUPCCode, @decStdCostPerLB
					,@decGrsDepth, @decGrsHeight, @decGrsWidth															-- WO#20797	
					);
			ELSE
				IF @chrAction = 'U'
					UPDATE tblItemMasterFromERP
					SET Facility = @vchFacility, ItemDesc1 = @vchItemDesc1, ItemMajorClass = @chrItemMajorClass
						,ItemNumber = @vchItemNumber, ItemType = @vchItemType , LabelWeight = @decLabelWeight
						,LabelWeightUOM = @chrLabelWeightUOM, NetWeight = @vchNetWeight, PackagesPerSaleableUnit = @decPackagesPerSaleableUnit
						,PackSize = @vchPackSize, ProductionShelfLifeDays = @intProductionShelfLifeDays, QtyPerPallet = @intQtyPerPallet
						,SaleableUnitPerCase = @decSaleableUnitPerCase, SCCCode = @vchSCCCode, ShipShelfLifeDays = @intShipShelfLifeDays
						,Tie = @decTie, Tier = @decTier, UPCCode = @vchUPCCode, StdCostPerLB = @decStdCostPerLB
						,GrsDepth = @decGrsDepth, GrsHeight = @decGrsHeight, GrsWidth = @decGrsWidth					-- WO#20797	
					WHERE Facility = @vchFacility and  ItemNumber = @vchItemNumber
				ELSE
					IF @chrAction = 'D'
					DELETE tblItemMasterFromERP
					FROM tblItemMasterFromERP 
					WHERE Facility = @vchFacility and  ItemNumber = @vchItemNumber

			SET @nvchSQLStmt = 'Update ' + @vchImportData + '.dbo.tblItemMasterTxFromERP SET Processed = 1 WHERE CURRENT OF csrTx;'
			EXEC sp_executesql @nvchSQLStmt;
			--Exec(@vchSQLStmt);

			--UPDATE tblItemMasterTxFromERP
			--	SET ProcessedFlag = True
			--	WHERE CURRENT OF csrTx;
			COMMIT TRANSACTION;

			FETCH NEXT FROM csrTx INTO @chrAction ,@vchFacility	,@vchItemDesc1 ,@chrItemMajorClass ,@vchItemNumber
				,@vchItemType ,@decLabelWeight ,@chrLabelWeightUOM ,@vchNetWeight ,@decPackagesPerSaleableUnit	
				,@vchPackSize ,@intProductionShelfLifeDays ,@intQtyPerPallet ,@decSaleableUnitPerCase ,@vchSCCCode	
				,@intShipShelfLifeDays ,@decTie	,@decTier ,@vchUPCCode, @decStdCostPerLB
				,@decGrsDepth, @decGrsHeight ,@decGrsWidth																-- WO#20797		
		END

		
		CLOSE csrTx;
		DEALLOCATE csrTx;


		--/* Copy data from subset of fields of tblItemMaster in the staging area to tblItemMasterFromERP in the staging areaa*/
		--SET @vchSQLStmt = 'TRUNCATE table ' + @vchImportData + '.dbo.tblItemMasterFromERP'
		--Execute (@vchSQLStmt)

		--SET @vchSQLStmt = 
		--'INSERT INTO ' + @vchImportData + '.dbo.tblItemMasterFromERP (' +
		--	'Facility, ItemDesc1, ItemMajorClass, ItemNumber, ItemType, LabelWeight, LabelWeightUOM, NetWeight, PackagesPerSaleableUnit, PackSize, ' +
		--	'ProductionShelfLifeDays, QtyPerPallet, SaleableUnitPerCase, SCCCode, ShipShelfLifeDays, Tie, Tier, UPCCode ' +
		--	')' +
		--'SELECT Facility, ItemDesc1, ItemMajorClass, ItemNumber, ItemType, LabelWeight, LabelWeightUOM, NetWeight, PackagesPerSaleableUnit, PackSize, ' +
		--	'ProductionShelfLifeDays, QtyPerPallet, SaleableUnitPerCase, SCCCode, ShipShelfLifeDays, Tie, Tier, UPCCode ' +
		--'FROM  ' + @vchImportData + '.dbo.tblItemMaster ' +
		--'WHERE Facility = ' + @vchInputFacility 
		--Execute (@vchSQLStmt)

		--/* Copy data from tblItemMasterFromERP in staging area to tblItemMasterFromERP in destination data base */
		---- Update record if value(s) are changed
		--SET @vchSQLStmt = 		
		--'Update dbo.tblItemMasterFromERP SET ' +
		--'ItemDesc1 = T2.ItemDesc1, ' +
		--'ItemMajorClass = T2.ItemMajorClass, ' +	
		--'ItemType = T2.ItemType, ' +									
		--'LabelWeight = T2.LabelWeight, ' + 
		--'LabelWeightUOM = T2.LabelWeightUOM, ' +
		--'NetWeight = T2.NetWeight, ' +
		--'PackagesPerSaleableUnit = T2.PackagesPerSaleableUnit, ' +
		--'PackSize = T2.PackSize, ' + 
		--'ProductionShelfLifeDays = T2.ProductionShelfLifeDays, ' +
		--'QtyPerPallet = T2.QtyPerPallet, ' +
		--'SaleableUnitPerCase = T2.SaleableUnitPerCase, ' +
		--'SCCCode = T2.SCCCode, ' +
		--'ShipShelfLifeDays = T2.ShipShelfLifeDays, ' +	
		--'Tie = T2.Tie, ' +
		--'Tier = T2.Tier, ' +
		--'UPCCode = T2.UPCCode ' +
		--'FROM tblItemMasterFromERP AS T1 ' + 
		--'INNER JOIN ' + @vchImportData + '.dbo.tblItemMasterFromERP AS T2 ' +
		--'ON T1.Facility = T2.Facility AND T1.ItemNumber = T2.ItemNumber ' +
		--'WHERE T1.Facility = ''' + @vchInputFacility + ''' AND ' +
		--' (Convert(varbinary(50),T1.ItemDesc1) <> Convert(varbinary(50),T2.ItemDesc1) ' +
		--'OR T1.ItemMajorClass <> T2.ItemMajorClass ' +	
		--'OR T1.ItemType <> T2.ItemType ' +									
		--'OR T1.LabelWeight <> T2.LabelWeight ' +
		--'OR T1.LabelWeightUOM <> T2.LabelWeightUOM ' +
		--'OR T1.NetWeight <> T2.NetWeight ' +
		--'OR T1.PackagesPerSaleableUnit <> T2.PackagesPerSaleableUnit ' +
		--'OR T1.PackSize <> T2.PackSize ' +
		--'OR T1.ProductionShelfLifeDays <> T2.ProductionShelfLifeDays ' +
		--'OR T1.SaleableUnitPerCase <> T2.SaleableUnitPerCase ' +
		--'OR T1.QtyPerPallet <> T2.QtyPerPallet ' +
		--'OR T1.SCCCode <> T2.SCCCode ' +
		--'OR T1.ShipShelfLifeDays <> T2.ShipShelfLifeDays ' +	
		--'OR T1.Tie <> T2.Tie ' +
		--'OR T1.Tier <> T2.Tier ' +
		--'OR T1.UPCCode <> T2.UPCCode ' +
		--')'
		--execute (@vchSQLStmt)

		----Add New Records
		--SET @vchSQLStmt =
		--'INSERT INTO tblItemMasterFromERP ' +
		--'SELECT T1.* FROM ' + @vchImportData + '.dbo.tblItemMasterFromERP As T1 ' +
		--'LEFT OUTER JOIN  tblItemMasterFromERP AS T2 ' +
		--'ON T1.ItemNumber = T2.ItemNumber AND T1.Facility = T2.Facility ' +
		--'WHERE T1.Facility = ''' + @vchInputFacility + ''' AND T2.ItemNumber IS NULL '

		--Execute (@vchSQLStmt)

		----Delete Records if record is not found in ImportData
		--SET @vchSQLStmt =
		--'DELETE tblItemMasterFromERP ' +
		--'FROM tblItemMasterFromERP AS T1 ' +
		--'LEFT OUTER JOIN ' + @vchImportData + '.dbo.tblItemMasterFromERP AS T2 ' +
		--'ON T1.ItemNumber = T2.ItemNumber AND T1.Facility = T2.Facility ' +
		--'WHERE T1.Facility = ''' + @vchInputFacility + ''' AND T2.ItemNumber IS NULL ' 

		--Execute (@vchSQLStmt)

	/* 
		Copy data from tblItemMasterFromERP and tblItemMasterOvrr to tblItemMaster
	*/

		-- Update record if value(s) are changed
		SET @nvchSQLStmt = 		
		'Update dbo.tblItemMaster ' +
		'SET ProductionShelfLifeDays=T2.ProductionShelfLifeDays, ' +
		'LabelWeight=T2.LabelWeight, ' + 
		'LabelWeightUOM=T2.LabelWeightUOM, ' +
		'BagLengthRequired=T2.BagLengthRequired, ' + 
		'BagLength=T2.BagLength, ' + 
		'LabelDateFmtCode=T2.LabelDateFmtCode, ' +
		'PackagesPerSaleableUnit=T2.PackagesPerSaleableUnit, ' +
		'SaleableUnitPerCase=T2.SaleableUnitPerCase, ' +
		'QtyPerPallet=T2.QtyPerPallet, ' +
		'SCCCode=T2.SCCCode, ' +
		'UPCCode=T2.UPCCode, ' +
		'OverrideItem=T2.OverrideItem, ' +
		'ItemDesc1=T2.ItemDesc1, ' +
		'ItemDesc2=T2.ItemDesc2, ' + 
		'ItemDesc3=T2.ItemDesc3, ' +
		'PackSize=T2.PackSize, ' + 
		'NetWeight=T2.NetWeight, ' +
		'DomicileText1=T2.DomicileText1, ' + 
		'DomicileText2=T2.DomicileText2, ' +
		'DomicileText3=T2.DomicileText3, ' + 
		'DomicileText4=T2.DomicileText4, ' +
		'DomicileText5=T2.DomicileText5, ' + 
		'DomicileText6=T2.DomicileText6, ' +
		'CaseLabelFmt1=T2.CaseLabelFmt1, ' +
		'CaseLabelFmt2=T2.CaseLabelFmt2, ' +
		'CaseLabelFmt3=T2.CaseLabelFmt3, ' +
		'PackageCoderFmt1=T2.PackageCoderFmt1, ' +
		'PackageCoderFmt2=T2.PackageCoderFmt2, ' +
		'PackageCoderFmt3=T2.PackageCoderFmt3, ' +
		'FilterCoderFmt=T2.FilterCoderFmt, ' +
		'ProductionDateDesc=T2.ProductionDateDesc, ' +
		'ExpiryDateDesc=T2.ExpiryDateDesc, ' +
		'PrintSOLot=T2.PrintSOLot, ' +
		'DateToPrintFlag=T2.DateToPrintFlag, ' +
		'PrintCaseLabel=T2.PrintCaseLabel, ' +
		'Tie=T2.Tie, ' +
		'Tier=T2.Tier, ' +
		'ShipShelfLifeDays=T2.ShipShelfLifeDays, ' +	
		'ItemType=T2.ItemType, ' +									
		'ItemMajorClass=T2.ItemMajorClass, ' +						
		'PalletCode=T2.PalletCode, ' +								
		'SlipSheet=T2.SlipSheet, ' +	
		'PkgLabelDateFmtCode=T2.PkgLabelDateFmtCode, ' +
		'StdCostPerLB=T2.StdCostPerLB ' +
		',GrsDepth=T2.GrsDepth ' +																			-- WO#20797
		',GrsHeight=T2.GrsHeight ' +																		-- WO#20797
		',GrsWidth=T2.GrsWidth ' +																			-- WO#20797	
		',CaseLabelApplicator=T2.CaseLabelApplicator ' +													-- WO#20797
		',InsertBrewerFilter=T2.InsertBrewerFilter ' +														-- WO#20797
		'FROM tblItemMaster AS T1 ' + 
		'INNER JOIN dbo.vwItemMasterWithOvrr AS T2 ' +
		'ON T1.ItemNumber = T2.ItemNumber AND T1.Facility = T2.Facility ' +
--		'WHERE T1.Facility = ''' + @vchInputFacility + ''' AND ' +
		'WHERE T1.Facility = @vchInputFacility AND ' +
		'(T1.ProductionShelfLifeDays<>T2.ProductionShelfLifeDays ' +
		'OR T1.LabelWeight<>T2.LabelWeight ' +
		'OR T1.LabelWeightUOM<>T2.LabelWeightUOM ' +
		'OR T1.BagLengthRequired<>T2.BagLengthRequired ' +
		'OR T1.BagLength<>T2.BagLength ' +
		'OR T1.LabelDateFmtCode<>T2.LabelDateFmtCode ' + 
		'OR T1.PackagesPerSaleableUnit<>T2.PackagesPerSaleableUnit ' +
		'OR T1.SaleableUnitPerCase<>T2.SaleableUnitPerCase ' +
		'OR T1.QtyPerPallet<>T2.QtyPerPallet ' +
		'OR T1.SCCCode<>T2.SCCCode ' +
		'OR T1.UPCCode<>T2.UPCCode ' +
		'OR T1.OverrideItem<>T2.OverrideItem ' +
		'OR Convert(varbinary(50),T1.ItemDesc1)<>Convert(varbinary(50),T2.ItemDesc1) ' +
		'OR Convert(varbinary(50),T1.ItemDesc2)<>Convert(varbinary(50),T2.ItemDesc2) ' +
		'OR Convert(varbinary(50),T1.ItemDesc3)<>Convert(varbinary(50),T2.ItemDesc3) ' +		-- Fix20150827
		-- Fix20150827  'OR Convert(varbinary(20),T1.ItemDesc3) <> Convert(varbinary(20),T2.ItemDesc3) ' +
		'OR T1.PackSize<>T2.PackSize ' +
		'OR T1.NetWeight<>T2.NetWeight ' +
		'OR Convert(varbinary(24),T1.DomicileText1)<>Convert(varbinary(24),T2.DomicileText1) ' +
		'OR Convert(varbinary(24),T1.DomicileText2)<>Convert(varbinary(24),T2.DomicileText2) ' +
		'OR Convert(varbinary(24),T1.DomicileText3)<>Convert(varbinary(24),T2.DomicileText3) ' +
		'OR Convert(varbinary(24),T1.DomicileText4)<>Convert(varbinary(24),T2.DomicileText4) ' +
		'OR Convert(varbinary(24),T1.DomicileText5)<>Convert(varbinary(24),T2.DomicileText5) ' +
		'OR Convert(varbinary(24),T1.DomicileText6)<>Convert(varbinary(24),T2.DomicileText6) ' +
		'OR T1.CaseLabelFmt1<>T2.CaseLabelFmt1 ' +
		'OR T1.CaseLabelFmt2<>T2.CaseLabelFmt2 ' +
		'OR T1.CaseLabelFmt3<>T2.CaseLabelFmt3 ' +
		'OR T1.PackageCoderFmt1<>T2.PackageCoderFmt1 ' +
		'OR T1.PackageCoderFmt2<>T2.PackageCoderFmt2 ' +
		'OR T1.PackageCoderFmt3<>T2.PackageCoderFmt3 ' +
		'OR T1.FilterCoderFmt<>T2.FilterCoderFmt ' +
		'OR T1.ProductionDateDesc<>T2.ProductionDateDesc ' +
		'OR T1.ExpiryDateDesc<>T2.ExpiryDateDesc ' +
		'OR T1.PrintSOLot<>T2.PrintSOLot ' +
		'OR T1.DateToPrintFlag<>T2.DateToPrintFlag ' +
		'OR T1.PrintCaseLabel<>T2.PrintCaseLabel ' +
		'OR T1.Tie<>T2.Tie ' +
		'OR T1.Tier<>T2.Tier ' +
		'OR T1.ShipShelfLifeDays<>T2.ShipShelfLifeDays ' +
		'OR T1.ItemType<>T2.ItemType ' +									
		'OR T1.ItemMajorClass<>T2.ItemMajorClass ' +						
		'OR T1.PalletCode<>T2.PalletCode ' +								
		'OR T1.SlipSheet<>T2.SlipSheet ' +
		'OR T1.PkgLabelDateFmtCode<>T2.PkgLabelDateFmtCode ' +
		'OR T1.StdCostPerLB<>T2.StdCostPerLB ' +
		'OR T1.GrsDepth<>T2.GrsDepth ' +																		-- WO#20797
		'OR T1.GrsHeight<>T2.GrsHeight ' +																		-- WO#20797
		'OR T1.GrsWidth<>T2.GrsWidth ' +																		-- WO#20797	
		'OR T1.CaseLabelApplicator<>T2.CaseLabelApplicator ' +													-- WO#20797
		'OR T1.InsertBrewerFilter<>T2.InsertBrewerFilter ' +													-- WO#20797
		')'
		SET @nvchParmDefinition = N'@vchInputFacility varchar(3)';
		EXEC sp_executesql @nvchSQLStmt, @nvchParmDefinition, @vchInputFacility;
		--execute (@vchSQLStmt)

		--Add New Records to item Master if record is not found in tblItemMaster
		SET @nvchSQLStmt =
		'INSERT INTO tblItemMaster ' +
		'SELECT T1.* FROM dbo.vwItemMasterWithOvrr As T1 ' +
		'LEFT OUTER JOIN  tblItemMaster AS T2 ' +
		'ON T1.ItemNumber = T2.ItemNumber AND T1.Facility = T2.Facility ' +
		'WHERE T1.Facility = @vchInputFacility AND T2.ItemNumber IS NULL ';
		--'WHERE T1.Facility = ''' + @vchInputFacility + ''' AND T2.ItemNumber IS NULL '

		SET @nvchParmDefinition = N'@vchInputFacility varchar(3)';
		EXEC sp_executesql @nvchSQLStmt, @nvchParmDefinition, @vchInputFacility;
		--Execute (@vchSQLStmt)

		--Delete Records from item Master if record is not found in tblItemMasterFromERP
		SET @nvchSQLStmt =
		'DELETE tblItemMaster ' +
		'FROM tblItemMaster AS T1 ' +
		'LEFT OUTER JOIN dbo.tblItemMasterFromERP AS T2 ' +
		'ON T1.ItemNumber = T2.ItemNumber AND T1.Facility = T2.Facility ' +
		'WHERE T1.Facility = @vchInputFacility AND T2.ItemNumber IS NULL ';
		--'WHERE T1.Facility = ''' + @vchInputFacility + ''' AND T2.ItemNumber IS NULL ' 		

		SET @nvchParmDefinition = N'@vchInputFacility varchar(3)';
		EXEC sp_executesql @nvchSQLStmt, @nvchParmDefinition, @vchInputFacility;
		--execute (@vchSQLStmt)

	END TRY
	BEGIN CATCH

		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorProcedure nvarchar(200);
		DECLARE @ErrorLine int;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorMessage NVARCHAR(4000);

		IF (XACT_STATE()) = -1
			ROLLBACK TRANSACTION 

		SELECT 
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorProcedure = ERROR_PROCEDURE(),
			@ErrorLine  = ERROR_LINE(),
			@ErrorNumber  = ERROR_NUMBER(),
			@ErrorMessage = N'Error %d, Level %d, State %d, Procedure %s, Line %d' + ', Message: ' + CHAR(13) + ERROR_MESSAGE();

			-- Use RAISERROR inside the CATCH block to return error information about the original error 
			-- that caused execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage, @ErrorSeverity, 1, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine)
	END CATCH
END

GO

