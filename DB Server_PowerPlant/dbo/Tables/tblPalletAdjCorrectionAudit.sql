CREATE TABLE [dbo].[tblPalletAdjCorrectionAudit] (
    [AuditSeq]              INT          IDENTITY (1, 1) NOT NULL,
    [AuditAction]           VARCHAR (10) NOT NULL,
    [AuditCreatedBy]        VARCHAR (50) NOT NULL,
    [AuditCreationDate]     DATETIME     NOT NULL,
    [RRN]                   INT          NOT NULL,
    [Facility]              VARCHAR (3)  NOT NULL,
    [PalletID]              INT          NOT NULL,
    [TransactionDate]       DATETIME     NOT NULL,
    [TransactionReasonCode] VARCHAR (2)  NOT NULL,
    [CreationDate]          DATETIME     NOT NULL,
    [CreatedBy]             VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_tblPalletAdjCorrectionAudit] PRIMARY KEY CLUSTERED ([AuditSeq] ASC)
);


GO

