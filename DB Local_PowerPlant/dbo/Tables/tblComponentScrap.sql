CREATE TABLE [dbo].[tblComponentScrap] (
    [Facility]  CHAR (3)       NOT NULL,
    [ShopOrder] INT            NOT NULL,
    [StartTime] DATETIME       NOT NULL,
    [Component] VARCHAR (35)   NOT NULL,
    [Quantity]  DECIMAL (8, 2) NOT NULL
);


GO

