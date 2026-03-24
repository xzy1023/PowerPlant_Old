CREATE TABLE [dbo].[tblScrapRejectPointLog] (
    [RRN]              INT           IDENTITY (1, 1) NOT NULL,
    [ProductionDate]   DATE          NOT NULL,
    [Shift]            INT           NOT NULL,
    [ShopOrder]        INT           NOT NULL,
    [ItemNumber]       NVARCHAR (35) NOT NULL,
    [Equipment]        NVARCHAR (10) NOT NULL,
    [Operator]         NVARCHAR (10) NOT NULL,
    [RejectPointID]    INT           NOT NULL,
    [Qty]              INT           NOT NULL,
    [ModifiedDateTime] DATETIME      NOT NULL,
    CONSTRAINT [PK_tblScrapRejectPointLog] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

