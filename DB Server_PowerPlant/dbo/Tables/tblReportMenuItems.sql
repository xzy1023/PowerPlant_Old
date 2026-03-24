CREATE TABLE [dbo].[tblReportMenuItems] (
    [MenuName]     VARCHAR (100) NOT NULL,
    [MenuItemName] VARCHAR (100) NOT NULL,
    [URL]          VARCHAR (500) NOT NULL,
    [Row]          SMALLINT      NULL,
    [Column]       SMALLINT      NULL,
    [ReportID]     INT           IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_tblReportMenuItems] PRIMARY KEY CLUSTERED ([MenuName] ASC, [MenuItemName] ASC)
);


GO

