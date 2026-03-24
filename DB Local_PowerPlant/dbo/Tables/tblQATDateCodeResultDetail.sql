CREATE TABLE [dbo].[tblQATDateCodeResultDetail] (
    [RRN]           INT          IDENTITY (1, 1) NOT NULL,
    [BatchID]       DATETIME     NOT NULL,
    [DateCodeType]  INT          NOT NULL,
    [DateCodeValue] VARCHAR (50) NULL,
    [Facility]      VARCHAR (3)  NOT NULL,
    [RetestNo]      TINYINT      NOT NULL,
    [TestResult]    TINYINT      NULL,
    [TestTime]      DATETIME     NOT NULL,
    CONSTRAINT [PK_tblQATDateCodeResultDetail] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tblQATDateCodeResultDetail]
    ON [dbo].[tblQATDateCodeResultDetail]([BatchID] ASC, [DateCodeType] ASC, [RetestNo] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'where 1 = Pass, 0 = Fail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATDateCodeResultDetail', @level2type = N'COLUMN', @level2name = N'TestResult';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'where 1 = PackageCoder, 2 = Carton, 3 = Case', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATDateCodeResultDetail', @level2type = N'COLUMN', @level2name = N'DateCodeType';


GO

