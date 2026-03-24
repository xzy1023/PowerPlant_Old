CREATE TABLE [dbo].[@del_tblItemMasterFromERP] (
    [Facility]                CHAR (3)        NOT NULL,
    [ItemDesc1]               VARCHAR (50)    NOT NULL,
    [ItemMajorClass]          CHAR (1)        NULL,
    [ItemNumber]              VARCHAR (35)    NOT NULL,
    [ItemType]                CHAR (1)        NULL,
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
    [Tie]                     DECIMAL (6, 3)  NOT NULL,
    [Tier]                    DECIMAL (6, 3)  NOT NULL,
    [UPCCode]                 VARCHAR (14)    NOT NULL
);


GO

