CREATE TABLE [dbo].[@del_tblControl] (
    [Facility]    NCHAR (3)      NOT NULL,
    [Key]         NVARCHAR (50)  NOT NULL,
    [SubKey]      NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (255) NULL,
    [Value1]      NVARCHAR (255) NULL,
    [Value2]      NVARCHAR (255) NULL,
    CONSTRAINT [PK_tblControl] PRIMARY KEY CLUSTERED ([Facility] ASC, [Key] ASC)
);


GO

