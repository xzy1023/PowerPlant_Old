CREATE TABLE [dbo].[tblRoastingSpecsUpdList] (
    [Facility]        CHAR (3)     NOT NULL,
    [TableName]       VARCHAR (50) NOT NULL,
    [Updated]         BIT          NOT NULL,
    [Lastupdate]      DATETIME     NULL,
    [LastSynchronize] DATETIME     NULL,
    CONSTRAINT [PK_tblRoastingSpecsUpdList] PRIMARY KEY CLUSTERED ([Facility] ASC, [TableName] ASC)
);


GO

