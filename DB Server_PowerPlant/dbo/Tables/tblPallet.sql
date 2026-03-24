CREATE TABLE [dbo].[tblPallet] (
    [RRN]                  INT          IDENTITY (1, 1) NOT NULL,
    [Facility]             CHAR (3)     NOT NULL,
    [PalletID]             INT          NOT NULL,
    [QtyPerPallet]         INT          CONSTRAINT [DF_tblPallet_UploadStatus] DEFAULT ('0') NOT NULL,
    [Quantity]             INT          NOT NULL,
    [ItemNumber]           VARCHAR (35) NOT NULL,
    [DefaultPkgLine]       CHAR (10)    NOT NULL,
    [Operator]             VARCHAR (10) NOT NULL,
    [CreationDate]         CHAR (8)     CONSTRAINT [DF_tblPallet_CreationDate] DEFAULT (CONVERT([char](8),getdate(),(112))) NOT NULL,
    [CreationTime]         CHAR (6)     CONSTRAINT [DF_tblPallet_CreationTime] DEFAULT (replace(CONVERT([char](8),getdate(),(114)),':','')) NOT NULL,
    [OrderComplete]        CHAR (1)     NOT NULL,
    [LotID]                VARCHAR (25) NOT NULL,
    [ShopOrder]            INT          NOT NULL,
    [StartTime]            DATETIME     NOT NULL,
    [ProductionDate]       CHAR (8)     NOT NULL,
    [ExpiryDate]           CHAR (8)     NOT NULL,
    [PrintStatus]          SMALLINT     CONSTRAINT [DF_tblPallet_PrintStatus] DEFAULT ((0)) NOT NULL,
    [CreationDateTime]     DATETIME     DEFAULT (getdate()) NOT NULL,
    [ShiftProductionDate]  DATETIME     NULL,
    [ShiftNo]              TINYINT      NOT NULL,
    [LastUpdate]           DATETIME     DEFAULT (getdate()) NULL,
    [OutputLocation]       VARCHAR (10) NULL,
    [DestinationShopOrder] INT          NULL,
    [BatchNumber]          VARCHAR (20) CONSTRAINT [DF_tblPallet_BatchNumber] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_tblPallet] PRIMARY KEY CLUSTERED ([PalletID] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblPallet_1]
    ON [dbo].[tblPallet]([RRN] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_tblPallet]
    ON [dbo].[tblPallet]([DefaultPkgLine] ASC, [ShopOrder] ASC, [Facility] ASC, [CreationDateTime] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shop Order Start Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPallet', @level2type = N'COLUMN', @level2name = N'StartTime';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 = nothing, 1 = submitted to print, 2 = printed', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPallet', @level2type = N'COLUMN', @level2name = N'PrintStatus';


GO

