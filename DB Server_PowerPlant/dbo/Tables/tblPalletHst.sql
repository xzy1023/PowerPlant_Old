CREATE TABLE [dbo].[tblPalletHst] (
    [RRN]                  INT          IDENTITY (1, 1) NOT NULL,
    [Facility]             CHAR (3)     NOT NULL,
    [PalletID]             INT          NOT NULL,
    [QtyPerPallet]         INT          CONSTRAINT [DF_tblPalletHst_UploadStatus] DEFAULT ('0') NOT NULL,
    [Quantity]             INT          NOT NULL,
    [ItemNumber]           VARCHAR (35) NOT NULL,
    [DefaultPkgLine]       CHAR (10)    NOT NULL,
    [Operator]             VARCHAR (10) NOT NULL,
    [CreationDate]         CHAR (8)     CONSTRAINT [DF_tblPalletHst_CreationDate] DEFAULT (getdate()) NOT NULL,
    [CreationTime]         CHAR (6)     NOT NULL,
    [OrderComplete]        CHAR (1)     NOT NULL,
    [LotID]                VARCHAR (25) NOT NULL,
    [ShopOrder]            INT          NOT NULL,
    [StartTime]            DATETIME     NOT NULL,
    [ProductionDate]       CHAR (8)     NOT NULL,
    [ExpiryDate]           CHAR (8)     NOT NULL,
    [PrintStatus]          SMALLINT     CONSTRAINT [DF_tblPalletHst_PrintStatus] DEFAULT ((0)) NOT NULL,
    [CreationDateTime]     DATETIME     DEFAULT (getdate()) NOT NULL,
    [ShiftProductionDate]  DATETIME     NULL,
    [ShiftNo]              TINYINT      NULL,
    [LastUpdate]           DATETIME     NULL,
    [OutputLocation]       VARCHAR (10) NULL,
    [DestinationShopOrder] INT          NULL,
    [BatchNumber]          VARCHAR (20) CONSTRAINT [DF_tblPalletHst_BatchNumber] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_tblPalletHst] PRIMARY KEY CLUSTERED ([PalletID] ASC, [Facility] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblPalletHst]
    ON [dbo].[tblPalletHst]([ShopOrder] ASC, [DefaultPkgLine] ASC, [Facility] ASC, [CreationDateTime] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_tblPalletHst_1]
    ON [dbo].[tblPalletHst]([DefaultPkgLine] ASC, [ShopOrder] ASC, [Facility] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shop Order Start Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPalletHst', @level2type = N'COLUMN', @level2name = N'StartTime';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 = nothing, 1 = submitted to print, 2 = printed', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPalletHst', @level2type = N'COLUMN', @level2name = N'PrintStatus';


GO

