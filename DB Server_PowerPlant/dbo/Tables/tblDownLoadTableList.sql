CREATE TABLE [dbo].[tblDownLoadTableList] (
    [TableName]      VARCHAR (50) NOT NULL,
    [DownLoadStatus] BIT          NOT NULL,
    [Active]         BIT          NOT NULL,
    [Facility]       CHAR (3)     NOT NULL,
    CONSTRAINT [PK_tblLocalTableList] PRIMARY KEY CLUSTERED ([Facility] ASC, [TableName] ASC)
);


GO

