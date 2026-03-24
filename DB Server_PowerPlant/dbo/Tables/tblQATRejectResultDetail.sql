CREATE TABLE [dbo].[tblQATRejectResultDetail] (
    [RRN]        INT      IDENTITY (1, 1) NOT NULL,
    [BatchID]    DATETIME NOT NULL,
    [RetestNo]   TINYINT  NOT NULL,
    [TestResult] TINYINT  NULL,
    [TestTime]   DATETIME NOT NULL,
    CONSTRAINT [PK_tblQATRejectResultDetail_1] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'where 1 = Pass, 0 = Fail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATRejectResultDetail', @level2type = N'COLUMN', @level2name = N'TestResult';


GO

