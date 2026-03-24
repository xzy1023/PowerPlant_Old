-- =============================================
-- Author:		Bong Lee
-- WO#:			1096
-- Create date: Jun. 17, 2014
-- Description:	Maintain Item Label Override table
-- WO#6437		Dec. 18, 2017	Bong Lee
-- Description:	Add columns ProductionDateDescOnBox,ExpiryDateDescOnBox, AdditionalText1, 
--				AdditionalText2 and PalletLabelFmt
-- WO#21178		Nov. 26, 2018	Bong Lee
-- Description:	Add columns CaseLabelApplicator and InsertBrewerFilter
-- =============================================
CREATE PROCEDURE [dbo].[PPsp_ItemLabelOverride_Maint]
	-- Add the parameters for the stored procedure here
	@vchAction varchar(50)
	,@decBagLength dec(4,2)
	,@chrBagLengthRequired char(1) = NULL
	,@chrCaseLabelDateFmtCode char(2) = NULL
	,@vchCaseLabelFmt1 varchar(25) = NULL
	,@vchCaseLabelFmt2 varchar(25) = NULL
	,@vchCaseLabelFmt3 varchar(25) = NULL
	,@chrDateToPrintFlag char(1) = NULL
	,@vchDomicileText1 varchar(24) = NULL
	,@vchDomicileText2 varchar(24) = NULL
	,@vchDomicileText3 varchar(24) = NULL
	,@vchDomicileText4 varchar(24) = NULL
	,@vchDomicileText5 varchar(24) = NULL
	,@vchDomicileText6 varchar(24) = NULL
	,@vchExpiryDateDesc varchar(30) = NULL
	,@vchFacility varchar(3) = NULL
	,@vchFilterCoderFmt varchar(25) = NULL
	,@vchItemDesc1 varchar(50) = NULL
	,@vchItemDesc2 varchar(50) = NULL
	,@vchItemDesc3 varchar(50) = NULL
	,@vchItemNumber varchar(35) = NULL
	,@vchNetWeight varchar(10) = NULL
	,@chrNetWeightUOM char(2) = NULL
	,@vchOverrideItem varchar(35) = NULL
	,@bitOvrDesc1Flag bit = NULL
	,@bitOvrNetWeightFlag bit = NULL
	,@bitOvrNetWeightUOMFlag bit = NULL
	,@bitOvrPackSizeFlag bit = NULL
	,@vchPackageCoderFmt1 varchar(25) = NULL
	,@vchPackageCoderFmt2 varchar(25) = NULL
	,@vchPackageCoderFmt3 varchar(25) = NULL
	,@vchPackSize varchar(12) = NULL
	,@chrPalletCode char(1) = NULL
	,@chrPkgLabelDateFmtCode char(2) = NULL
	,@chrPrintCaseLabel char(1) = NULL
	,@chrPrintSOLot char(1) = NULL
	,@vchProductionDateDesc varchar(30) = NULL
	,@bitSlipSheet bit = NULL
	,@chrUseSCCAsUPC char(1) = NULL
	,@vchLastUpdatedBy varchar(100) = NULL
-- WO#6437 ADD Start
	,@vchProductionDateDescOnBox varchar(30) = NULL
	,@vchExpiryDateDescOnBox varchar(30) = NULL
	,@vchAdditionalText1 varchar(30) = NULL 
	,@vchAdditionalText2 varchar(30) = NULL
	,@vchPalletLabelFmt varchar(25) = NULL
-- WO#6437 ADD End
-- WO#21178 ADD Start
	,@intCaseLabelApplicator	tinyint = NULL
	,@bitInsertBrewerFilter		bit = NULL
-- WO#21178 ADD Stop
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY

    IF @vchAction = 'ADD'
		BEGIN 
			-- insert record and write the after image of the record to the audit log
			INSERT INTO tblItemLabelOvrr
                    (BagLength, BagLengthRequired, CaseLabelDateFmtCode, CaseLabelFmt1, CaseLabelFmt2, CaseLabelFmt3, DateToPrintFlag, DomicileText1
                    ,DomicileText2 ,DomicileText3, DomicileText4, DomicileText5, DomicileText6, ExpiryDateDesc, Facility, FilterCoderFmt
                    ,ItemDesc1, ItemDesc2, ItemDesc3, ItemNumber, NetWeight ,NetWeightUOM, OverrideItem, OvrDesc1Flag, OvrNetWeightFlag
                    ,OvrNetWeightUOMFlag, OvrPackSizeFlag, PackageCoderFmt1, PackageCoderFmt2 ,PackageCoderFmt3, PackSize, PalletCode
					,PkgLabelDateFmtCode, PrintCaseLabel, PrintSOLot, ProductionDateDesc, SlipSheet, UseSCCAsUPC, LastUpdatedBy, LastUpdatedAt
					,ProductionDateDescOnBox, ExpiryDateDescOnBox, AdditionalText1, AdditionalText2, PalletLabelFmt									-- WO#6437
					,CaseLabelApplicator,InsertBrewerFilter																							-- WO#21178
					)
			-- WO#6437	Output 'Insert', Inserted.*
			Output 'Insert', @vchLastUpdatedBy, Getdate(), Inserted.*			-- WO#6437
			Into tblItemLabelOvrrAudit
			VALUES (@decBagLength ,@chrBagLengthRequired ,@chrCaseLabelDateFmtCode ,@vchCaseLabelFmt1 ,@vchCaseLabelFmt2 ,@vchCaseLabelFmt3 ,@chrDateToPrintFlag, @vchDomicileText1	
					,@vchDomicileText2 ,@vchDomicileText3 ,@vchDomicileText4 ,@vchDomicileText5	,@vchDomicileText6 ,@vchExpiryDateDesc ,@vchFacility, @vchFilterCoderFmt 	
					,@vchItemDesc1 ,@vchItemDesc2 ,@vchItemDesc3 ,@vchItemNumber ,@vchNetWeight	,@chrNetWeightUOM ,@vchOverrideItem	,@bitOvrDesc1Flag, @bitOvrNetWeightFlag
					,@bitOvrNetWeightUOMFlag ,@bitOvrPackSizeFlag ,@vchPackageCoderFmt1 ,@vchPackageCoderFmt2 ,@vchPackageCoderFmt3	,@vchPackSize, @chrPalletCode
					,@chrPkgLabelDateFmtCode ,@chrPrintCaseLabel ,@chrPrintSOLot ,@vchProductionDateDesc ,@bitSlipSheet	,@chrUseSCCAsUPC, @vchLastUpdatedBy, getDate()
					,@vchProductionDateDescOnBox, @vchExpiryDateDescOnBox, @vchAdditionalText1, @vchAdditionalText2, @vchPalletLabelFmt				-- WO#6437
					,@intCaseLabelApplicator,@bitInsertBrewerFilter																					-- WO#21178
					)

		END
	ELSE
	-- Update data to the Item Label Override Table and write the before image of the record to the audit table.
		IF @vchAction = 'UPDATE'
			BEGIN
				UPDATE tblItemLabelOvrr
				SET [BagLength] = @decBagLength
				,[BagLengthRequired] = @chrBagLengthRequired
				,[CaseLabelDateFmtCode] = @chrCaseLabelDateFmtCode
				,[CaseLabelFmt1] = @vchCaseLabelFmt1
				,[CaseLabelFmt2] = @vchCaseLabelFmt2
				,[CaseLabelFmt3] = @vchCaseLabelFmt3
				,[DateToPrintFlag] = @chrDateToPrintFlag
				,[DomicileText1] = @vchDomicileText1
				,[DomicileText2] = @vchDomicileText2
				,[DomicileText3] = @vchDomicileText3
				,[DomicileText4] = @vchDomicileText4
				,[DomicileText5] = @vchDomicileText5
				,[DomicileText6] = @vchDomicileText6
				,[ExpiryDateDesc] = @vchExpiryDateDesc
				,[Facility] = @vchFacility
				,[FilterCoderFmt] = @vchFilterCoderFmt
				,[ItemDesc1] = @vchItemDesc1
				,[ItemDesc2] = @vchItemDesc2
				,[ItemDesc3] = @vchItemDesc3
				,[ItemNumber] = @vchItemNumber
				,[NetWeight] = @vchNetWeight
				,[NetWeightUOM] = @chrNetWeightUOM
				,[OverrideItem] = @vchOverrideItem
				,[OvrDesc1Flag] = @bitOvrDesc1Flag
				,[OvrNetWeightFlag] = @bitOvrNetWeightFlag
				,[OvrNetWeightUOMFlag] = @bitOvrNetWeightUOMFlag
				,[OvrPackSizeFlag] = @bitOvrPackSizeFlag
				,[PackageCoderFmt1] = @vchPackageCoderFmt1
				,[PackageCoderFmt2] = @vchPackageCoderFmt2
				,[PackageCoderFmt3] = @vchPackageCoderFmt3
				,[PackSize] = @vchPackSize
				,[PalletCode] = @chrPalletCode
				,[PkgLabelDateFmtCode] = @chrPkgLabelDateFmtCode
				,[PrintCaseLabel] = @chrPrintCaseLabel
				,[PrintSOLot] = @chrPrintSOLot
				,[ProductionDateDesc] = @vchProductionDateDesc
				,[SlipSheet] = @bitSlipSheet
				,[UseSCCAsUPC] = @chrUseSCCAsUPC
				,[LastUpdatedBy] = @vchLastUpdatedBy
				,[LastUpdatedAt] = getdate()
				-- WO#6437 ADD Start
				,ProductionDateDescOnBox = @vchProductionDateDescOnBox
				,ExpiryDateDescOnBox = @vchExpiryDateDescOnBox
				,AdditionalText1 = @vchAdditionalText1
				,AdditionalText2 = @vchAdditionalText2
				,PalletLabelFmt = @vchPalletLabelFmt
				-- WO#21178 ADD Start
				,CaseLabelApplicator = @intCaseLabelApplicator
				,InsertBrewerFilter	= @bitInsertBrewerFilter	
				-- WO#21178 ADD Stop
				Output 'Update', @vchLastUpdatedBy, Getdate(), Deleted.*
				-- WO#6437 ADD End
				-- WO#6437	Output 'Update', Deleted.*
				Into tblItemLabelOvrrAudit
				WHERE Facility = @vchFacility and ItemNumber = @vchItemNumber
			END
		ELSE
			IF @vchAction = 'DELETE'
				BEGIN
					-- Delete record and write the before image of the record to the audit log	
					DELETE tblItemLabelOvrr
					-- WO#6437	Output 'Delete', Deleted.*
					Output 'Delete', @vchLastUpdatedBy, Getdate(), Deleted.*		-- WO#6437
					Into tblItemLabelOvrrAudit
					WHERE Facility = @vchFacility and ItemNumber = @vchItemNumber
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

