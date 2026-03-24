CREATE TABLE [dbo].[PRO_IMP_ORDER] (
    [CUSTOMER_ID]          INT           NOT NULL,
    [TRANSFERED]           INT           NOT NULL,
    [TRANSFERED_TIMESTAMP] DATETIME      NULL,
    [ACTIVITY]             CHAR (1)      NOT NULL,
    [ORDER_TYP]            INT           NULL,
    [ORDER_NAME]           VARCHAR (20)  NOT NULL,
    [SCHEDULED_TIME]       DATETIME      NOT NULL,
    [RECEIVING_STATION]    VARCHAR (20)  NOT NULL,
    [WEIGHT_IN]            INT           NOT NULL,
    [WEIGHT_OUT]           INT           NOT NULL,
    [CUSTOMER_CODE]        VARCHAR (20)  NOT NULL,
    [LINE_CONTROL]         VARCHAR (20)  NULL,
    [SOURCE_GROUP]         INT           NULL,
    [TARGET_GROUP]         INT           NULL,
    [ORDER_DESCRIPTION]    VARCHAR (200) NULL,
    [TYPE_COMP_01]         VARCHAR (20)  NULL,
    [TYPE_COMP_02]         VARCHAR (20)  NULL,
    [TYPE_COMP_03]         VARCHAR (20)  NULL,
    [TYPE_COMP_04]         VARCHAR (20)  NULL,
    [TYPE_COMP_05]         VARCHAR (20)  NULL,
    [TYPE_COMP_06]         VARCHAR (20)  NULL,
    [TYPE_COMP_07]         VARCHAR (20)  NULL,
    [TYPE_COMP_08]         VARCHAR (20)  NULL,
    [TYPE_COMP_09]         VARCHAR (20)  NULL,
    [TYPE_COMP_10]         VARCHAR (20)  NULL,
    [TYPE_COMP_11]         VARCHAR (20)  NULL,
    [TYPE_COMP_12]         VARCHAR (20)  NULL,
    [TYPE_COMP_13]         VARCHAR (20)  NULL,
    [TYPE_COMP_14]         VARCHAR (20)  NULL,
    [PART_COMP_01]         INT           NULL,
    [PART_COMP_02]         INT           NULL,
    [PART_COMP_03]         INT           NULL,
    [PART_COMP_04]         INT           NULL,
    [PART_COMP_05]         INT           NULL,
    [PART_COMP_06]         INT           NULL,
    [PART_COMP_07]         INT           NULL,
    [PART_COMP_08]         INT           NULL,
    [PART_COMP_09]         INT           NULL,
    [PART_COMP_10]         INT           NULL,
    [PART_COMP_11]         INT           NULL,
    [PART_COMP_12]         INT           NULL,
    [PART_COMP_13]         INT           NULL,
    [PART_COMP_14]         INT           NULL,
    [TYPE_OPTIONAL_COMP]   VARCHAR (20)  NULL,
    [PART_OPTIONAL_COMP]   INT           NULL,
    CONSTRAINT [PK_PRO_IMP_ORDER] PRIMARY KEY CLUSTERED ([CUSTOMER_ID] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing Line', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'LINE_CONTROL';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'type of the 14. component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'TYPE_COMP_14';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp, set by probat and customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'scheduled date/time of delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'SCHEDULED_TIME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'customer code of the coffee type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 decimal place; percentage of the 14. component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'PART_COMP_14';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Group withdrawal coffee', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'SOURCE_GROUP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''I'' Insert; ''U'' Update; ''D'' Delete; ''F'' Finished', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'ACTIVITY';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'name of roaster, grinder or packmaschine', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'RECEIVING_STATION';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 decimal place; percentage of the 1. component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'PART_COMP_01';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'2: Roasters; 3: Packmachines whole bean; 4: Grinders; 5 : Packmachines Ground; 6: Relocation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'ORDER_TYP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique identifier for import', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'CUSTOMER_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Group charging coffee', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'ORDER_DESCRIPTION';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 deciaml place; amount of coffee input', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'WEIGHT_IN';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'type of the 2. component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'TYPE_COMP_02';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'name of order', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'ORDER_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred (set by Customer, if processed )', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 deciaml place; amount of coffee output', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'WEIGHT_OUT';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'type of the 1. component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_ORDER', @level2type = N'COLUMN', @level2name = N'TYPE_COMP_01';


GO

