CREATE TABLE [dbo].[tblQATEntryPoint] (
    [QATEntryPoint]         CHAR (1)     NOT NULL,
    [EntryPointDescription] VARCHAR (50) NOT NULL,
    [UpdatedAt]             DATETIME     NOT NULL,
    [UpdatedBy]             VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_tblQATEntryPoint] PRIMARY KEY CLUSTERED ([QATEntryPoint] ASC)
);


GO

