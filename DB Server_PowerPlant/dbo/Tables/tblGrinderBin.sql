CREATE TABLE [dbo].[tblGrinderBin] (
    [Facility] CHAR (10)    NOT NULL,
    [Grinder]  VARCHAR (10) NOT NULL,
    [Bin]      VARCHAR (6)  NOT NULL,
    CONSTRAINT [PK_tblGrinderBin] PRIMARY KEY CLUSTERED ([Facility] ASC, [Grinder] ASC, [Bin] ASC)
);


GO

