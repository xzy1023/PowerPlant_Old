CREATE TABLE [dbo].[tblControl] (
    [Facility]    CHAR (10)     NOT NULL,
    [Key]         VARCHAR (50)  NOT NULL,
    [SubKey]      VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (255) NOT NULL,
    [Value1]      VARCHAR (255) NOT NULL,
    [Value2]      VARCHAR (255) NULL,
    CONSTRAINT [PK_tblControl] PRIMARY KEY CLUSTERED ([Facility] ASC, [Key] ASC, [SubKey] ASC)
);


GO

