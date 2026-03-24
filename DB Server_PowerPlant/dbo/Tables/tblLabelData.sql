CREATE TABLE [dbo].[tblLabelData] (
    [Facility]        CHAR (3)      NOT NULL,
    [RecordType]      CHAR (1)      NOT NULL,
    [LabelKey]        NVARCHAR (50) NOT NULL,
    [ShopOrder]       INT           NOT NULL,
    [OverrideItem]    VARCHAR (35)  NOT NULL,
    [PalletID]        VARCHAR (9)   NULL,
    [FilterCoderFmt]  VARCHAR (25)  NULL,
    [CasePalletFmt]   VARCHAR (25)  NULL,
    [PackageCoderFmt] VARCHAR (25)  NULL,
    [Quantity]        INT           NOT NULL,
    [ItemDesc1]       VARCHAR (20)  NULL,
    [ItemDesc2]       VARCHAR (20)  NULL,
    [ItemDesc3]       VARCHAR (20)  NULL,
    [PackSize]        VARCHAR (12)  NOT NULL,
    [NetWeight]       VARCHAR (8)   NOT NULL,
    [SCCCode]         VARCHAR (14)  NOT NULL,
    [UPCCode]         VARCHAR (14)  NOT NULL,
    [DomicileText1]   VARCHAR (24)  NULL,
    [DomicileText2]   VARCHAR (24)  NULL,
    [DomicileText3]   VARCHAR (24)  NULL,
    [DomicileText4]   VARCHAR (24)  NULL,
    [DomicileText5]   VARCHAR (24)  NULL,
    [DomicileText6]   VARCHAR (24)  NULL,
    [PackagingLine]   VARCHAR (15)  NOT NULL,
    [ProductionDate]  DATETIME      NULL,
    [ExpiryDate]      DATETIME      NULL,
    [AlternateDate]   VARCHAR (20)  NULL,
    [CreationTime]    DATETIME      CONSTRAINT [DF_tblLabelData_CreationTime] DEFAULT (getdate()) NOT NULL,
    [PrintedFlag]     BIT           NULL,
    CONSTRAINT [PK_tblLabelData] PRIMARY KEY CLUSTERED ([LabelKey] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PackagingLine + Label Type + (SO #/Item #/Pallet #)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLabelData', @level2type = N'COLUMN', @level2name = N'LabelKey';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description_old', @value = N'PackagingLine + Label Type + (SO #/Item #/Pallet #)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLabelData', @level2type = N'COLUMN', @level2name = N'LabelKey';


GO

