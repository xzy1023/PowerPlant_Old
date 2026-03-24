CREATE TABLE [dbo].[tblQATPalletTypeResult] (
    [RRN]           INT          IDENTITY (1, 1) NOT NULL,
    [BatchID]       DATETIME     NOT NULL,
    [ConfirmedBy]   VARCHAR (10) NULL,
    [Facility]      VARCHAR (3)  NOT NULL,
    [InterfaceID]   VARCHAR (24) NOT NULL,
    [PackagingLine] CHAR (10)    NOT NULL,
    [PalletCode]    CHAR (1)     NOT NULL,
    [QAConfirmed]   BIT          CONSTRAINT [DF_tblQATPalletTypeResult_QAConfirmed] DEFAULT ((0)) NULL,
    [ShopOrder]     INT          NOT NULL,
    [SOStartTime]   DATETIME     NOT NULL,
    [TestEndTime]   DATETIME     NOT NULL,
    [TestStartTime] DATETIME     NOT NULL,
    [TesterID]      VARCHAR (10) NULL,
    [QATEntryPoint] CHAR (1)     NULL,
    CONSTRAINT [PK_tblQATPalletTypeResult] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tblQATPalletTypeResult]
    ON [dbo].[tblQATPalletTypeResult]([Facility] ASC, [ShopOrder] ASC, [PackagingLine] ASC, [BatchID] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATPalletTypeResult_1]
    ON [dbo].[tblQATPalletTypeResult]([InterfaceID] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'A = CHEP, B = One-Way', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATPalletTypeResult', @level2type = N'COLUMN', @level2name = N'PalletCode';


GO

