CREATE TABLE [dbo].[tblShopOrderWeightSpecHst] (
    [Facility]          CHAR (3)        NOT NULL,
    [ShopOrder]         INT             NOT NULL,
    [ItemNumber]        VARCHAR (35)    NOT NULL,
    [Blend]             VARCHAR (11)    NULL,
    [Grind]             VARCHAR (6)     NULL,
    [LabelWeight]       DECIMAL (10, 3) NOT NULL,
    [TargetWeight]      DECIMAL (6, 1)  NOT NULL,
    [MinWeight]         DECIMAL (6, 1)  NOT NULL,
    [MaxWeight]         DECIMAL (6, 1)  NOT NULL,
    [LowerControlLimit] DECIMAL (6, 1)  NOT NULL,
    [DateCreated]       DATETIME        CONSTRAINT [DF_tblShopOrderWeightSpecHst_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]       DATETIME        CONSTRAINT [DF_tblShopOrderWeightSpecHst_DateUpdated] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_tblShopOrderWeightSpecHst] PRIMARY KEY CLUSTERED ([Facility] ASC, [ShopOrder] ASC)
);


GO

