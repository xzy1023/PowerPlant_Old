CREATE TABLE [dbo].[tblShopOrderErrors] (
    [Facility]                VARCHAR (3)  NOT NULL,
    [ShopOrder]               INT          NOT NULL,
    [ItemNumber]              VARCHAR (35) NOT NULL,
    [WorkCenter]              INT          NOT NULL,
    [PackagingLine]           VARCHAR (10) NOT NULL,
    [Routing]                 BIT          NOT NULL,
    [LotID]                   BIT          NOT NULL,
    [ShipShelfLifeDays]       BIT          NOT NULL,
    [ProductionShelfLifeDays] BIT          NOT NULL,
    [InvalidOperation]        BIT          NOT NULL,
    [DateCreated]             DATETIME     CONSTRAINT [DF_tblShopOrderErrors_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]             DATETIME     CONSTRAINT [DF_tblShopOrderErrors_DateUpdated] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_tblShopOrderErrors] PRIMARY KEY CLUSTERED ([Facility] ASC, [ShopOrder] ASC, [ItemNumber] ASC, [WorkCenter] ASC, [PackagingLine] ASC)
);


GO

