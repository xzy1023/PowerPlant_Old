CREATE TABLE [dbo].[tblUnitCountInbound] (
    [ComputerName]         VARCHAR (50) NOT NULL,
    [CreationTime]         DATETIME     CONSTRAINT [DF_tblUnitCountInbound_CreationTime] DEFAULT (getdate()) NOT NULL,
    [DestinationShopOrder] INT          NOT NULL,
    [Facility]             VARCHAR (3)  NOT NULL,
    [ItemNumber]           VARCHAR (35) NOT NULL,
    [LastUpdateTime]       DATETIME     CONSTRAINT [DF_tblUnitCountInbound_LastUpdateTime] DEFAULT (getdate()) NOT NULL,
    [Operator]             VARCHAR (10) NOT NULL,
    [OrderChange]          VARCHAR (20) NULL,
    [OutputLocation]       VARCHAR (10) NOT NULL,
    [PackagingLine]        VARCHAR (10) NOT NULL,
    [PalletId]             INT          CONSTRAINT [DF_tblUnitCountInbound_PalletId] DEFAULT ((0)) NOT NULL,
    [ProcessingStatus]     TINYINT      CONSTRAINT [DF_tblUnitCountInbound_ProcessingStatus] DEFAULT ((0)) NOT NULL,
    [ShiftNo]              CHAR (1)     NOT NULL,
    [ShopOrder]            INT          NOT NULL,
    [SOStartTime]          DATETIME     NOT NULL,
    [TxID]                 INT          NOT NULL,
    [UnitCount]            INT          NOT NULL,
    [UnitsPerPallet]       INT          NOT NULL,
    CONSTRAINT [PK_tblUnitCountInbound] PRIMARY KEY CLUSTERED ([TxID] ASC, [Facility] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblUnitCountInbound]
    ON [dbo].[tblUnitCountInbound]([ProcessingStatus] ASC, [CreationTime] ASC, [Facility] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 = Record Created; 1 = Count added to the IPC Session Control Table; 2 = Pallet created from IPC.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblUnitCountInbound', @level2type = N'COLUMN', @level2name = N'ProcessingStatus';


GO

