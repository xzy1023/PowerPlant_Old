CREATE TABLE [dbo].[tblScrapRejectPoint] (
    [RRN]         INT            IDENTITY (1, 1) NOT NULL,
    [RejectPoint] NVARCHAR (50)  NOT NULL,
    [Comments]    NVARCHAR (200) CONSTRAINT [DF_tblScrapRejectPoint_Comments] DEFAULT ('') NULL,
    CONSTRAINT [PK_tblScrapRejectPoint] PRIMARY KEY CLUSTERED ([RRN] ASC),
    CONSTRAINT [UK_RejectPoint_tblScrapRejectPoint] UNIQUE NONCLUSTERED ([RejectPoint] ASC)
);


GO

