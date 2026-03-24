CREATE TABLE [dbo].[tblQATCheckWeigherResultDetail] (
    [RRN]          INT            IDENTITY (1, 1) NOT NULL,
    [ActualWeight] DECIMAL (6, 2) NULL,
    [BatchID]      DATETIME       NOT NULL,
    [Recipe]       VARCHAR (30)   NULL,
    [RetestNo]     TINYINT        NOT NULL,
    [TareWeight]   DECIMAL (6, 2) NULL,
    [TestResult]   TINYINT        NULL,
    [TestTime]     DATETIME       NOT NULL,
    CONSTRAINT [PK_tblQATCheckWeigherResultDetail_1] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'where 1 = Pass, 0 = Fail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATCheckWeigherResultDetail', @level2type = N'COLUMN', @level2name = N'TestResult';


GO

