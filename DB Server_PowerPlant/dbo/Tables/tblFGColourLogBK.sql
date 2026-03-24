CREATE TABLE [dbo].[tblFGColourLogBK] (
    [RRN]      INT            NOT NULL,
    [TestNo]   CHAR (20)      NOT NULL,
    [DateTest] DATETIME       NOT NULL,
    [ProdTime] CHAR (20)      NULL,
    [Blend]    CHAR (6)       CONSTRAINT [DF__tblFGColoBK__Blend__39E294A9] DEFAULT ((0)) NOT NULL,
    [Grind]    CHAR (6)       CONSTRAINT [DF__tblFGColoBK__Grind__3AD6B8E2] DEFAULT ((0)) NULL,
    [Colour]   DECIMAL (5, 2) CONSTRAINT [DF__tblFGColo__ColouBK__3BCADD1B] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_tblFGColourLogBK] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

