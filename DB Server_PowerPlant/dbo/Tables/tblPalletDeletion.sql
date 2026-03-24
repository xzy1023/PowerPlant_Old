CREATE TABLE [dbo].[tblPalletDeletion] (
    [DeletionTime]         DATETIME     NULL,
    [DeletedBy]            VARCHAR (10) NULL,
    [RRN]                  INT          NOT NULL,
    [Facility]             CHAR (3)     NOT NULL,
    [PalletID]             INT          NOT NULL,
    [QtyPerPallet]         INT          NOT NULL,
    [Quantity]             INT          NOT NULL,
    [ItemNumber]           VARCHAR (35) NOT NULL,
    [DefaultPkgLine]       CHAR (10)    NOT NULL,
    [Operator]             VARCHAR (10) NULL,
    [CreationDate]         CHAR (8)     NULL,
    [CreationTime]         CHAR (6)     NULL,
    [OrderComplete]        CHAR (1)     NOT NULL,
    [LotID]                VARCHAR (25) NULL,
    [ShopOrder]            INT          NOT NULL,
    [StartTime]            DATETIME     NOT NULL,
    [ProductionDate]       CHAR (8)     NULL,
    [ExpiryDate]           CHAR (8)     NULL,
    [PrintStatus]          SMALLINT     NOT NULL,
    [CreationDateTime]     DATETIME     NOT NULL,
    [ShiftProductionDate]  DATETIME     NULL,
    [ShiftNo]              TINYINT      NULL,
    [LastUpdate]           DATETIME     NULL,
    [OutputLocation]       VARCHAR (10) NULL,
    [DestinationShopOrder] INT          NULL,
    CONSTRAINT [PK_tblPalletDeletion] PRIMARY KEY CLUSTERED ([PalletID] ASC, [Facility] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shop Order Start Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPalletDeletion', @level2type = N'COLUMN', @level2name = N'StartTime';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 = nothing, 1 = submitted to print, 2 = printed', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPalletDeletion', @level2type = N'COLUMN', @level2name = N'PrintStatus';


GO

