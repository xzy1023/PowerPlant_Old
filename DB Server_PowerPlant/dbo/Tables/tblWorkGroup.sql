CREATE TABLE [dbo].[tblWorkGroup] (
    [facility]    CHAR (3)     NOT NULL,
    [WorkGroup]   VARCHAR (10) NOT NULL,
    [Description] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_tblDepartment] PRIMARY KEY CLUSTERED ([facility] ASC, [WorkGroup] ASC)
);


GO

