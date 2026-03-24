CREATE TABLE [dbo].[tblShopOrderWeightSpec_20250702] (
    [Facility]          CHAR (3)        NOT NULL,
    [ShopOrder]         INT             NOT NULL,
    [ItemNumber]        VARCHAR (35)    NOT NULL,
    [Blend]             VARCHAR (11)    NULL,
    [Grind]             VARCHAR (6)     NULL,
    [LabelWeight]       DECIMAL (10, 3) NOT NULL,
    [TargetWeight]      DECIMAL (6, 1)  NOT NULL,
    [MinWeight]         DECIMAL (6, 1)  NOT NULL,
    [MaxWeight]         DECIMAL (6, 1)  NOT NULL,
    [LowerControlLimit] DECIMAL (6, 1)  NOT NULL
);


GO

