CREATE TABLE [dbo].[tblQATTester] (
    [TesterID]    VARCHAR (10)  NOT NULL,
    [TesterName]  VARCHAR (100) NOT NULL,
    [LastUpdated] DATETIME      CONSTRAINT [DF_tblQATTester_LastUpdated] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_tblQATTester] PRIMARY KEY CLUSTERED ([TesterID] ASC)
);


GO

