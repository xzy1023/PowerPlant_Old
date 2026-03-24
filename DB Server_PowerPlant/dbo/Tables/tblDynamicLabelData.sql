CREATE TABLE [dbo].[tblDynamicLabelData] (
    [Facility]             CHAR (3)      NOT NULL,
    [RecordType]           CHAR (1)      NOT NULL,
    [LabelKey]             NVARCHAR (50) NOT NULL,
    [PackagingLine]        CHAR (10)     NOT NULL,
    [ShopOrder]            INT           NULL,
    [ItemNumber]           VARCHAR (35)  NOT NULL,
    [PalletID]             VARCHAR (9)   NULL,
    [Quantity]             INT           NULL,
    [MPProductionDate]     VARCHAR (20)  NULL,
    [MPExpiryDate]         VARCHAR (20)  NULL,
    [ProductionDate]       SMALLDATETIME NOT NULL,
    [PreFmtProductionDate] VARCHAR (50)  NULL,
    [PreFmtExpiryDate]     VARCHAR (50)  NULL,
    [CreationTime]         DATETIME      CONSTRAINT [DF_tblDynamicLabelData_CreationTime] DEFAULT (getdate()) NOT NULL,
    [Operator]             VARCHAR (50)  NOT NULL,
    [LotID]                VARCHAR (25)  NOT NULL,
    [PrintedFlag]          BIT           CONSTRAINT [DF_tblDynamicLabelData_PrintedFlag] DEFAULT ('0') NOT NULL,
    [DefaultPkgLine]       CHAR (10)     NULL,
    [LastUpdated]          DATETIME      CONSTRAINT [DF_tblDynamicLabelData_LastUpdated] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_tblDynamicLabelData_1] PRIMARY KEY CLUSTERED ([LabelKey] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mother Parkers Production Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDynamicLabelData', @level2type = N'COLUMN', @level2name = N'MPProductionDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mother Parkers Expiry Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDynamicLabelData', @level2type = N'COLUMN', @level2name = N'MPExpiryDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'P=Pallet Label, C=Case Label for Case Labeler/Package Coder/Filter Coder, I=Item Label', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDynamicLabelData', @level2type = N'COLUMN', @level2name = N'RecordType';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Alternate Formated Production Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDynamicLabelData', @level2type = N'COLUMN', @level2name = N'PreFmtProductionDate';


GO

