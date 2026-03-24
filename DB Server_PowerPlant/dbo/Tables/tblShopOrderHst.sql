CREATE TABLE [dbo].[tblShopOrderHst] (
    [DateCreated]     SMALLDATETIME   NOT NULL,
    [LastUpdated]     SMALLDATETIME   NOT NULL,
    [Facility]        CHAR (3)        NOT NULL,
    [ShopOrder]       INT             NOT NULL,
    [ItemNumber]      VARCHAR (35)    NOT NULL,
    [PackagingLine]   CHAR (10)       NOT NULL,
    [StartDate]       INT             NOT NULL,
    [StartTime]       INT             NOT NULL,
    [EndDate]         INT             NOT NULL,
    [EndTime]         INT             NOT NULL,
    [OrderQty]        DECIMAL (11, 3) NOT NULL,
    [FinishedQty]     DECIMAL (11, 3) NOT NULL,
    [Closed]          BIT             NULL,
    [LotID]           VARCHAR (25)    NULL,
    [PkgRate]         DECIMAL (7, 5)  NOT NULL,
    [ProbatOrderName] VARCHAR (20)    CONSTRAINT [DF_tblShopOrderHst_ProbatOrderName] DEFAULT ('') NOT NULL,
    [FlavoredCoffee]  CHAR (1)        CONSTRAINT [DF_tblShopOrderHst_FlavoredCoffee] DEFAULT ('') NOT NULL,
    [OrderType]       TINYINT         CONSTRAINT [DF_tblShopOrderHst_OrderType] DEFAULT ((0)) NOT NULL,
    [ComponentItem]   VARCHAR (35)    CONSTRAINT [DF_tblShopOrderHst_ComponentItem] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_tblShopOrderHst] PRIMARY KEY CLUSTERED ([Facility] ASC, [ShopOrder] ASC)
);


GO

