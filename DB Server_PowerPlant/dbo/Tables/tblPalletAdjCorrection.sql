CREATE TABLE [dbo].[tblPalletAdjCorrection] (
    [RRN]                   INT          IDENTITY (1, 1) NOT NULL,
    [Facility]              VARCHAR (3)  NOT NULL,
    [PalletID]              INT          NOT NULL,
    [TransactionDate]       DATETIME     NOT NULL,
    [TransactionReasonCode] VARCHAR (2)  NOT NULL,
    [CreationDate]          DATETIME     CONSTRAINT [DF_tblPalletAdjCorrection_CreationDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]             VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_tblPalletAdjCorrection] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

