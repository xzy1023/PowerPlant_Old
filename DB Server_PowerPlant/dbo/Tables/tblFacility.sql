CREATE TABLE [dbo].[tblFacility] (
    [Facility]         CHAR (3)     NOT NULL,
    [Region]           CHAR (3)     NOT NULL,
    [Description]      VARCHAR (30) NOT NULL,
    [ShortDescription] VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_tblFacility] PRIMARY KEY CLUSTERED ([Facility] ASC)
);


GO

