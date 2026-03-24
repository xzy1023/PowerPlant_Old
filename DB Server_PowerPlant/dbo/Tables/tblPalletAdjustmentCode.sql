CREATE TABLE [dbo].[tblPalletAdjustmentCode] (
    [TransactionReasonCode] VARCHAR (2)  NOT NULL,
    [Description]           VARCHAR (50) NOT NULL,
    [AffectBPCS]            BIT          NOT NULL,
    [AffectPowerPlant]      BIT          NOT NULL,
    CONSTRAINT [PK_tblPalletAdjustmentCode] PRIMARY KEY CLUSTERED ([TransactionReasonCode] ASC)
);


GO

