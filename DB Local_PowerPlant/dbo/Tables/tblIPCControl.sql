CREATE TABLE [dbo].[tblIPCControl] (
    [ControlKey]  VARCHAR (50)  NOT NULL,
    [Value1]      VARCHAR (50)  NULL,
    [Value2]      VARCHAR (50)  NULL,
    [Description] VARCHAR (255) NULL,
    CONSTRAINT [PK_tblIPCControl] PRIMARY KEY CLUSTERED ([ControlKey] ASC)
);


GO

