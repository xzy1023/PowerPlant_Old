CREATE TABLE [dbo].[tblPalletCount] (
    [Facility]              NCHAR (3)     NULL,
    [BatchStartTime]        DATETIME      NOT NULL,
    [ProductionLine]        NVARCHAR (50) NOT NULL,
    [ShopOrder]             INT           NOT NULL,
    [Shift]                 NVARCHAR (1)  NULL,
    [BatchEndTime]          DATETIME      NULL,
    [PalletsPacked]         INT           NULL,
    [CasesPacked]           INT           NULL,
    [CreatedBy]             NVARCHAR (50) NULL,
    [UpdatedBy]             NVARCHAR (50) NULL,
    [LastUpdated]           DATETIME      NULL,
    [UnitsPerPallet]        INT           NULL,
    [EnteredUnitsPerPallet] INT           NULL,
    [SaleableUnitsPerCase]  SMALLINT      NULL,
    [UploadStatus]          BIT           NOT NULL,
    CONSTRAINT [PK_tblPalletCount] PRIMARY KEY CLUSTERED ([BatchStartTime] ASC, [ProductionLine] ASC, [ShopOrder] ASC)
);


GO

