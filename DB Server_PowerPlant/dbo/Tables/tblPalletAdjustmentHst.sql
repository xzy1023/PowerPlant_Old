CREATE TABLE [dbo].[tblPalletAdjustmentHst] (
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
    [TxTime]                DATETIME        NOT NULL,
    [Processed]             BIT             NOT NULL,
    CONSTRAINT [PK_tblPalletAdjustmentHst] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

