CREATE TABLE [dbo].[tblItemLabelOvrr] (
    [BagLength]               DECIMAL (4, 2) NULL,
    [BagLengthRequired]       CHAR (1)       NULL,
    [CaseLabelDateFmtCode]    CHAR (2)       NULL,
    [CaseLabelFmt1]           VARCHAR (25)   NULL,
    [CaseLabelFmt2]           VARCHAR (25)   NULL,
    [CaseLabelFmt3]           VARCHAR (25)   NULL,
    [DateToPrintFlag]         CHAR (1)       NULL,
    [DomicileText1]           VARCHAR (24)   NULL,
    [DomicileText2]           VARCHAR (24)   NULL,
    [DomicileText3]           VARCHAR (24)   NULL,
    [DomicileText4]           VARCHAR (24)   NULL,
    [DomicileText5]           VARCHAR (24)   NULL,
    [DomicileText6]           VARCHAR (24)   NULL,
    [ExpiryDateDesc]          VARCHAR (30)   NULL,
    [Facility]                CHAR (3)       NOT NULL,
    [FilterCoderFmt]          VARCHAR (25)   NULL,
    [ItemDesc1]               VARCHAR (50)   NULL,
    [ItemDesc2]               VARCHAR (50)   NULL,
    [ItemDesc3]               VARCHAR (50)   NULL,
    [ItemNumber]              VARCHAR (35)   NOT NULL,
    [NetWeight]               VARCHAR (10)   NULL,
    [NetWeightUOM]            CHAR (2)       NULL,
    [OverrideItem]            VARCHAR (35)   NULL,
    [OvrDesc1Flag]            BIT            NOT NULL,
    [OvrNetWeightFlag]        BIT            NOT NULL,
    [OvrNetWeightUOMFlag]     BIT            NOT NULL,
    [OvrPackSizeFlag]         BIT            NOT NULL,
    [PackageCoderFmt1]        VARCHAR (25)   NULL,
    [PackageCoderFmt2]        VARCHAR (25)   NULL,
    [PackageCoderFmt3]        VARCHAR (25)   NULL,
    [PackSize]                VARCHAR (12)   NULL,
    [PalletCode]              CHAR (1)       NULL,
    [PkgLabelDateFmtCode]     CHAR (2)       NULL,
    [PrintCaseLabel]          CHAR (1)       NULL,
    [PrintSOLot]              CHAR (1)       NULL,
    [ProductionDateDesc]      VARCHAR (30)   NULL,
    [SlipSheet]               BIT            NULL,
    [UseSCCAsUPC]             CHAR (1)       NULL,
    [LastUpdatedBy]           VARCHAR (100)  NULL,
    [LastUpdatedAt]           DATETIME       NULL,
    [ProductionDateDescOnBox] VARCHAR (30)   NULL,
    [ExpiryDateDescOnBox]     VARCHAR (30)   NULL,
    [AdditionalText1]         VARCHAR (30)   NULL,
    [AdditionalText2]         VARCHAR (30)   NULL,
    [PalletLabelFmt]          VARCHAR (25)   NULL,
    [CaseLabelApplicator]     TINYINT        NULL,
    [InsertBrewerFilter]      BIT            NULL,
    CONSTRAINT [PK_tblItemLabelOvrr] PRIMARY KEY CLUSTERED ([Facility] ASC, [ItemNumber] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 = Yes; 0 = No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemLabelOvrr', @level2type = N'COLUMN', @level2name = N'InsertBrewerFilter';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Code Dating - I$CDC in IIME$ (Date format code for Labels)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemLabelOvrr', @level2type = N'COLUMN', @level2name = N'PkgLabelDateFmtCode';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N' 1 = Left Side Only; 2 = Right Side Only; 3 = Both Sides', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemLabelOvrr', @level2type = N'COLUMN', @level2name = N'CaseLabelApplicator';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''0'' = None; ''1''= Print Expiry Date; ''2''= Print Production Date; ''3''= Print Expiry & Prodcution Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemLabelOvrr', @level2type = N'COLUMN', @level2name = N'DateToPrintFlag';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''Y'' - Print Shop Order Lot; ''N'' - Do Not Print', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemLabelOvrr', @level2type = N'COLUMN', @level2name = N'PrintSOLot';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Code Dating - I$CDC in IIME$ (Date format code for Labels)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemLabelOvrr', @level2type = N'COLUMN', @level2name = N'CaseLabelDateFmtCode';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''Y'' - Print Case Label; ''N'' - Do Not Print Case Label', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemLabelOvrr', @level2type = N'COLUMN', @level2name = N'PrintCaseLabel';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'"Y" - Required; "N"- Not required so the Bag Length can be zero', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemLabelOvrr', @level2type = N'COLUMN', @level2name = N'BagLengthRequired';


GO

