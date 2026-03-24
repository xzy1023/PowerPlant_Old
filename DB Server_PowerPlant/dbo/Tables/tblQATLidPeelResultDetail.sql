CREATE TABLE [dbo].[tblQATLidPeelResultDetail] (
    [RRN]        INT      IDENTITY (1, 1) NOT NULL,
    [BatchID]    DATETIME NOT NULL,
    [LaneNo]     INT      NULL,
    [RetestNo]   TINYINT  NOT NULL,
    [SampleNo]   INT      NULL,
    [TestResult] BIT      NOT NULL,
    [TestTime]   DATETIME NOT NULL,
    CONSTRAINT [PK_tblQATLidPeelResultDetail] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'where 1 = Pass, 0 = Fail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATLidPeelResultDetail', @level2type = N'COLUMN', @level2name = N'TestResult';


GO

