CREATE TABLE [dbo].[tblQATWeightResultDetail] (
    [RRN]          INT            IDENTITY (1, 1) NOT NULL,
    [ActualWeight] DECIMAL (6, 1) NOT NULL,
    [BatchID]      DATETIME       NOT NULL,
    [LaneNo]       INT            NULL,
    [RetestNo]     INT            NOT NULL,
    [SampleNo]     INT            NOT NULL,
    [TestResult]   BIT            NOT NULL,
    [TestTime]     DATETIME       NOT NULL,
    CONSTRAINT [PK_tblQATWeightResultDetail] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'where 1 = Pass, 0 = Fail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATWeightResultDetail', @level2type = N'COLUMN', @level2name = N'TestResult';


GO

