CREATE TABLE [dbo].[tblItemMaster] (
    [Facility]                CHAR (3)        NOT NULL,
    [ItemNumber]              VARCHAR (35)    NOT NULL,
    [ProductionShelfLifeDays] INT             NOT NULL,
    [LabelWeight]             DECIMAL (10, 3) NOT NULL,
    [LabelWeightUOM]          CHAR (2)        NULL,
    [BagLengthRequired]       CHAR (1)        NULL,
    [BagLength]               DECIMAL (4, 2)  NULL,
    [LabelDateFmtCode]        CHAR (2)        NULL,
    [PackagesPerSaleableUnit] DECIMAL (5)     NULL,
    [SaleableUnitPerCase]     DECIMAL (5)     NOT NULL,
    [QtyPerPallet]            INT             NULL,
    [SCCCode]                 VARCHAR (14)    NULL,
    [UPCCode]                 VARCHAR (14)    NULL,
    [OverrideItem]            VARCHAR (35)    NULL,
    [ItemDesc1]               VARCHAR (50)    NULL,
    [ItemDesc2]               VARCHAR (50)    NULL,
    [ItemDesc3]               VARCHAR (20)    NULL,
    [PackSize]                VARCHAR (12)    NULL,
    [NetWeight]               VARCHAR (10)    NULL,
    [DomicileText1]           VARCHAR (24)    NULL,
    [DomicileText2]           VARCHAR (24)    NULL,
    [DomicileText3]           VARCHAR (24)    NULL,
    [DomicileText4]           VARCHAR (24)    NULL,
    [DomicileText5]           VARCHAR (24)    NULL,
    [DomicileText6]           VARCHAR (24)    NULL,
    [CaseLabelFmt1]           VARCHAR (25)    NULL,
    [CaseLabelFmt2]           VARCHAR (25)    NULL,
    [CaseLabelFmt3]           VARCHAR (25)    NULL,
    [PackageCoderFmt1]        VARCHAR (25)    NULL,
    [PackageCoderFmt2]        VARCHAR (25)    NULL,
    [PackageCoderFmt3]        VARCHAR (25)    NULL,
    [FilterCoderFmt]          VARCHAR (25)    NULL,
    [ProductionDateDesc]      VARCHAR (10)    NULL,
    [ExpiryDateDesc]          VARCHAR (10)    NULL,
    [PrintSOLot]              CHAR (1)        NULL,
    [DateToPrintFlag]         CHAR (1)        NULL,
    [PrintCaseLabel]          CHAR (1)        NULL,
    [Tie]                     INT             NOT NULL,
    [Tier]                    INT             NOT NULL,
    [ShipShelfLifeDays]       INT             NULL,
    [ItemType]                VARCHAR (10)    NOT NULL,
    [ItemMajorClass]          VARCHAR (10)    NOT NULL,
    [PalletCode]              CHAR (1)        NULL,
    [SlipSheet]               BIT             NULL,
    [StdCostPerLB]            DECIMAL (15, 5) NOT NULL,
    CONSTRAINT [PK_tblItemMaster] PRIMARY KEY CLUSTERED ([Facility] ASC, [ItemNumber] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''Y'' - Print Case Label; ''N'' - Do Not Print Case Label', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemMaster', @level2type = N'COLUMN', @level2name = N'PrintCaseLabel';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''Y'' - Print Shop Order Lot; ''N'' - Do Not Print', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemMaster', @level2type = N'COLUMN', @level2name = N'PrintSOLot';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'"Y" - Required; "N"- Not required so the Bag Length can be zero', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemMaster', @level2type = N'COLUMN', @level2name = N'BagLengthRequired';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''0'' = None; ''1''= Print Expiry Date; ''2''= Print Production Date; ''3''= Print Expiry & Prodcution Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemMaster', @level2type = N'COLUMN', @level2name = N'DateToPrintFlag';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Code Dating - I$CDC in IIME$ (Date format code for Labels)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemMaster', @level2type = N'COLUMN', @level2name = N'LabelDateFmtCode';


GO

