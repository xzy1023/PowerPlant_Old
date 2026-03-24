CREATE TABLE [dbo].[tblDownLoadTableList] (
    [TableName]      VARCHAR (50) NOT NULL,
    [DownLoadStatus] BIT          NOT NULL,
    [Active]         BIT          NOT NULL,
    [Facility]       CHAR (3)     NOT NULL,
    [LastDownLoad]   DATETIME     NULL,
    CONSTRAINT [PK_tblDownLoadTableList] PRIMARY KEY CLUSTERED ([Facility] ASC, [TableName] ASC)
);


GO

