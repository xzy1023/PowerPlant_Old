CREATE TABLE [dbo].[tblFGColourLog] (
    [RRN]      INT            IDENTITY (1, 1) NOT NULL,
    [TestNo]   NCHAR (20)     NOT NULL,
    [DateTest] DATETIME       NOT NULL,
    [ProdTime] NCHAR (20)     NULL,
    [Blend]    NCHAR (6)      CONSTRAINT [DF__tblFGColo__Blend__39E294A9] DEFAULT ((0)) NOT NULL,
    [Grind]    NCHAR (6)      CONSTRAINT [DF__tblFGColo__Grind__3AD6B8E2] DEFAULT ((0)) NULL,
    [Colour]   DECIMAL (5, 2) CONSTRAINT [DF__tblFGColo__Colou__3BCADD1B] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_tblFGColourLog_1] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblFGColourLog]
    ON [dbo].[tblFGColourLog]([DateTest] ASC);


GO

