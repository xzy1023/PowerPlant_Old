CREATE TABLE [dbo].[tblCheckWeigherLog] (
    [RRN]           INT            IDENTITY (1, 1) NOT NULL,
    [ActualWeight]  DECIMAL (7, 3) NOT NULL,
    [Packagingline] CHAR (10)      NOT NULL,
    [ShopOrder]     INT            NULL,
    [SOStartTime]   DATETIME       NULL,
    [TestTime]      DATETIME       CONSTRAINT [DF_tblCheckWeigherLog_TestTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_tblCheckWeigherLog] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblCheckWeigherLog]
    ON [dbo].[tblCheckWeigherLog]([TestTime] ASC);


GO

