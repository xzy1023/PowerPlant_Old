CREATE TABLE [dbo].[tblDynamicLabelData_20241125] (
    [Facility]             CHAR (3)      NOT NULL,
    [RecordType]           CHAR (1)      NOT NULL,
    [LabelKey]             NVARCHAR (50) NOT NULL,
    [PackagingLine]        CHAR (10)     NOT NULL,
    [ShopOrder]            INT           NULL,
    [ItemNumber]           VARCHAR (35)  NOT NULL,
    [PalletID]             VARCHAR (9)   NULL,
    [Quantity]             INT           NULL,
    [MPProductionDate]     VARCHAR (20)  NULL,
    [ProductionDate]       SMALLDATETIME NOT NULL,
    [PreFmtProductionDate] VARCHAR (50)  NULL,
    [PreFmtExpiryDate]     VARCHAR (50)  NULL,
    [CreationTime]         DATETIME      NOT NULL,
    [Operator]             VARCHAR (50)  NOT NULL,
    [LotID]                VARCHAR (25)  NOT NULL,
    [PrintedFlag]          BIT           NOT NULL,
    [DefaultPkgLine]       CHAR (10)     NULL,
    [LastUpdated]          DATETIME      NULL
);


GO

