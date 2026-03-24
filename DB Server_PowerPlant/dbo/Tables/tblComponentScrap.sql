CREATE TABLE [dbo].[tblComponentScrap] (
    [Facility]  CHAR (3)       NOT NULL,
    [ShopOrder] INT            NOT NULL,
    [StartTime] DATETIME       NOT NULL,
    [Component] VARCHAR (35)   NOT NULL,
    [Quantity]  DECIMAL (8, 2) NOT NULL,
    CONSTRAINT [PK_tblComponentScrap] PRIMARY KEY CLUSTERED ([Facility] ASC, [ShopOrder] ASC, [StartTime] ASC, [Component] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packaging Line Assigned for the Computer ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblComponentScrap', @level2type = N'COLUMN', @level2name = N'ShopOrder';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shop Order Start Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblComponentScrap', @level2type = N'COLUMN', @level2name = N'StartTime';


GO

