CREATE TABLE [dbo].[tblPalletAdjustment] (
    [RRN]                   BIGINT          NOT NULL,
    [Facility]              VARCHAR (3)     NOT NULL,
    [ShopOrder]             INT             NOT NULL,
    [MachineID]             VARCHAR (10)    NOT NULL,
    [Operator]              VARCHAR (10)    NOT NULL,
    [PalletID]              INT             NOT NULL,
    [LotNumber]             VARCHAR (25)    NULL,
    [AdjustedQty]           DECIMAL (11, 3) NOT NULL,
    [TransactionReasonCode] VARCHAR (2)     NOT NULL,
    [TransactionDate]       DATETIME        NOT NULL,
    CONSTRAINT [PK_tblPalletAdjustment] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblPalletAdjustment]
    ON [dbo].[tblPalletAdjustment]([Facility] ASC, [ShopOrder] ASC, [MachineID] ASC, [Operator] ASC);


GO

