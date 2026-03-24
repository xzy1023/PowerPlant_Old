CREATE TABLE [dbo].[tblItemMasterTxFromERPHst] (
    [LastUpdate]              DATETIME        CONSTRAINT [DF_tblItemMasterTxFromERPHst_LastUpdate] DEFAULT (getdate()) NOT NULL,
    [TxID]                    BIGINT          NOT NULL,
    [TxTime]                  DATETIME        NOT NULL,
    [Action]                  CHAR (1)        NOT NULL,
    [Processed]               BIT             NOT NULL,
    [Facility]                CHAR (3)        NOT NULL,
    [ItemDesc1]               VARCHAR (50)    NOT NULL,
    [ItemMajorClass]          VARCHAR (10)    NOT NULL,
    [ItemNumber]              VARCHAR (35)    NOT NULL,
    [ItemType]                VARCHAR (10)    NOT NULL,
    [LabelWeight]             DECIMAL (10, 3) NOT NULL,
    [LabelWeightUOM]          CHAR (2)        NOT NULL,
    [NetWeight]               VARCHAR (10)    NOT NULL,
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
    [GrsDepth]                DECIMAL (16, 8) NULL,
    [GrsHeight]               DECIMAL (16, 8) NULL,
    [GrsWidth]                DECIMAL (16, 8) NULL,
    CONSTRAINT [PK_tblItemMasterTxFromERPHst] PRIMARY KEY CLUSTERED ([TxID] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''I'' Insert; ''U'' Update; ''D'' Delete', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemMasterTxFromERPHst', @level2type = N'COLUMN', @level2name = N'Action';


GO

