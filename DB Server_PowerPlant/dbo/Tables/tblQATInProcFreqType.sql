CREATE TABLE [dbo].[tblQATInProcFreqType] (
    [FreqTypeID]          INT          NOT NULL,
    [FreqTypeDescription] VARCHAR (50) NOT NULL,
    [UpdatedAt]           DATETIME     NOT NULL,
    [UpdatedBy]           VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_tblQATInProcFreqType] PRIMARY KEY CLUSTERED ([FreqTypeID] ASC)
);


GO

