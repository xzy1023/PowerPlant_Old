CREATE TABLE [dbo].[tblItemNotes] (
    [ItemNumber] VARCHAR (35)  NOT NULL,
    [SequenceNo] BIGINT        NOT NULL,
    [Text]       VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_tblItemNotes] PRIMARY KEY CLUSTERED ([ItemNumber] ASC, [SequenceNo] ASC)
);


GO

