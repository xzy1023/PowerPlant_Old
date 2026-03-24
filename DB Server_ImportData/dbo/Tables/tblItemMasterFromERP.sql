CREATE TABLE [dbo].[tblItemMasterFromERP] (
    [Facility]                CHAR (3)        NOT NULL,
    [ItemDesc1]               VARCHAR (50)    NOT NULL,
    [ItemMajorClass]          VARCHAR (10)    NOT NULL,
    [ItemNumber]              VARCHAR (35)    NOT NULL,
    [ItemType]                VARCHAR (10)    NOT NULL,
    [LabelWeight]             DECIMAL (10, 3) NOT NULL,
    [LabelWeightUOM]          CHAR (2)        NOT NULL,
    [NetWeight]               VARCHAR (10)    NULL,
    [PackagesPerSaleableUnit] DECIMAL (5)     NOT NULL,
    [PackSize]                VARCHAR (12)    NOT NULL,
    [ProductionShelfLifeDays] INT             NOT NULL,
    [QtyPerPallet]            INT             NOT NULL,
    [SaleableUnitPerCase]     DECIMAL (5)     NOT NULL,
    [SCCCode]                 VARCHAR (14)    NOT NULL,
    [ShipShelfLifeDays]       INT             NOT NULL,
    [Tie]                     INT             NOT NULL,
    [Tier]                    INT             NOT NULL,
    [UPCCode]                 VARCHAR (14)    NOT NULL,
    [StdCostPerLB]            DECIMAL (15, 5) NOT NULL,
    CONSTRAINT [PK_tblItemMasterFromERP] PRIMARY KEY CLUSTERED ([Facility] ASC, [ItemNumber] ASC)
);


GO

