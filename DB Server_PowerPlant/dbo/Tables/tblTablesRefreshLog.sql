CREATE TABLE [dbo].[tblTablesRefreshLog] (
    [MigrationID]      VARCHAR (50)  NOT NULL,
    [TableName]        VARCHAR (100) NOT NULL,
    [FromServer]       VARCHAR (50)  NULL,
    [FromDataBase]     VARCHAR (50)  NULL,
    [ToServer]         VARCHAR (50)  NULL,
    [ToDataBase]       VARCHAR (50)  NULL,
    [FacilityFilter]   VARCHAR (3)   NULL,
    [RecordCount_From] INT           NULL,
    [RecordCount_To]   INT           NULL,
    [CreationTime]     DATETIME      NULL,
    [LastUpdate]       DATETIME      NULL
);


GO

