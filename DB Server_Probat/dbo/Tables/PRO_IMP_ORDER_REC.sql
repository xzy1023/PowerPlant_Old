CREATE TABLE [dbo].[PRO_IMP_ORDER_REC] (
    [CUSTOMER_ID]          INT           NOT NULL,
    [TRANSFERED]           INT           NOT NULL,
    [TRANSFERED_TIMESTAMP] DATETIME      NULL,
    [ACTIVITY]             VARCHAR (1)   NOT NULL,
    [ORDER_NAME]           VARCHAR (20)  NOT NULL,
    [DELIVERY_NAME]        VARCHAR (20)  NOT NULL,
    [SCHEDULED_TIME]       DATETIME      NOT NULL,
    [AMOUNT_ORDER]         INT           NOT NULL,
    [AMOUNT_DELIVERY]      INT           NOT NULL,
    [ZONE]                 INT           NOT NULL,
    [CUSTOMER_CODE]        VARCHAR (20)  NOT NULL,
    [ORDER_DESCRIPTION]    VARCHAR (200) NULL,
    [DELIVERY_DESCRIPTION] VARCHAR (200) NULL,
    [LOT_NAME]             VARCHAR (20)  NOT NULL,
    [DATA_1]               VARCHAR (20)  NULL,
    [DATA_2]               VARCHAR (20)  NULL,
    [DATA_3]               VARCHAR (20)  NULL,
    [DATA_11]              VARCHAR (20)  NULL,
    [DATA_12]              VARCHAR (20)  NULL,
    [DATA_13]              VARCHAR (20)  NULL,
    CONSTRAINT [PK_PRO_IMP_ORDER_REC] PRIMARY KEY CLUSTERED ([CUSTOMER_ID] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'name of delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'DELIVERY_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'belongs to the delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'DATA_13';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'contract must be unique', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'ORDER_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'belongs to the delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'DATA_11';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'belongs to the delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'DATA_12';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 decimal place; amount of Purchase Order', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'AMOUNT_ORDER';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'scheduled date/time of delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'SCHEDULED_TIME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'customer code of the coffee type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred (set by Customer, if processed )', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 Green coffee; 2 Roasted coffee; 3 Ground coffee', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'ZONE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique identifier for import', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'CUSTOMER_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 decimal place; amount of delivery (amount of truck)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'AMOUNT_DELIVERY';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'belongs to the contract', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'DATA_3';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''I'' Insert; ''U'' Update; ''D'' Delete; ''F'' Finished', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'ACTIVITY';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'belongs to the contract', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'DATA_2';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp, set by probat and customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'belongs to the contract', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'DATA_1';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TrackingID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER_REC', @level2type = N'COLUMN', @level2name = N'LOT_NAME';


GO

