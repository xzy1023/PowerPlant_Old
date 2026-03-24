CREATE TABLE [dbo].[tblQATDateCodeResultHeader] (
    [RRN]           INT          IDENTITY (1, 1) NOT NULL,
    [BatchID]       DATETIME     NOT NULL,
    [ConfirmedBy]   VARCHAR (10) NULL,
    [DateCodeType]  INT          NOT NULL,
    [Facility]      VARCHAR (3)  NOT NULL,
    [InterfaceID]   VARCHAR (24) NOT NULL,
    [PackagingLine] CHAR (10)    NOT NULL,
    [QAConfirmed]   BIT          CONSTRAINT [DF_tblQATDateCodeResultHeader_QAConfirmed] DEFAULT ((0)) NULL,
    [RetestNo]      TINYINT      NOT NULL,
    [ShopOrder]     INT          NOT NULL,
    [SOStartTime]   DATETIME     NOT NULL,
    [TestEndTime]   DATETIME     NOT NULL,
    [TestResult]    TINYINT      NULL,
    [TestStartTime] DATETIME     NOT NULL,
    [TesterID]      VARCHAR (10) NULL,
    [QATEntryPoint] CHAR (1)     NULL,
    CONSTRAINT [PK_tblQATDateCodeResultHeader] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATDateCodeResultHeader_1]
    ON [dbo].[tblQATDateCodeResultHeader]([InterfaceID] ASC);


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tblQATDateCodeResultHeader]
    ON [dbo].[tblQATDateCodeResultHeader]([BatchID] ASC, [DateCodeType] ASC, [RetestNo] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'where 1 = Pass, 0 = Fail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATDateCodeResultHeader', @level2type = N'COLUMN', @level2name = N'TestResult';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'where 1 = PackageCoder, 2 = Carton, 3 = Case', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATDateCodeResultHeader', @level2type = N'COLUMN', @level2name = N'DateCodeType';


GO

