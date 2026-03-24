CREATE TABLE [dbo].[tblWorkSubGroup] (
    [Active]       BIT          CONSTRAINT [DF_tblWorkSubGroup_Active] DEFAULT ('1') NOT NULL,
    [Facility]     CHAR (3)     NOT NULL,
    [WorkGroup]    VARCHAR (10) NOT NULL,
    [WorkSubGroup] VARCHAR (10) NOT NULL,
    [Description]  VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_tblWorkSubGroup] PRIMARY KEY CLUSTERED ([Facility] ASC, [WorkGroup] ASC, [WorkSubGroup] ASC)
);


GO

