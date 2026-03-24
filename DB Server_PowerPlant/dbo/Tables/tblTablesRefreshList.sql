CREATE TABLE [dbo].[tblTablesRefreshList] (
    [Sequence]  INT           NOT NULL,
    [TableName] VARCHAR (100) NOT NULL,
    [Active]    BIT           NOT NULL,
    [Refresh1]  BIT           NOT NULL,
    [Refresh2]  BIT           NOT NULL,
    [Refresh3]  BIT           NOT NULL,
    CONSTRAINT [PK_tblTablesRefreshList] PRIMARY KEY CLUSTERED ([Sequence] ASC)
);


GO

