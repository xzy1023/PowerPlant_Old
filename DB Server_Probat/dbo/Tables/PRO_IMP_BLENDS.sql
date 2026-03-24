CREATE TABLE [dbo].[PRO_IMP_BLENDS] (
    [CUSTOMER_ID]          INT           NOT NULL,
    [TRANSFERED]           INT           NOT NULL,
    [TRANSFERED_TIMESTAMP] DATETIME      NULL,
    [ACTIVITY]             VARCHAR (1)   NOT NULL,
    [ZONE]                 INT           NULL,
    [CUSTOMER_CODE]        VARCHAR (20)  NOT NULL,
    [NAME]                 VARCHAR (20)  NULL,
    [INFO_TXT]             VARCHAR (200) NULL,
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
    CONSTRAINT [PK_PRO_IMP_BLENDS] PRIMARY KEY CLUSTERED ([CUSTOMER_ID] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'type of the 1. component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'TYPE_COMP_01';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'type of the 14. component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'TYPE_COMP_14';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CUSTOMER_CODE of the blend.  ZONE+CUSTOMER_CODE must be unique', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp, set by probat and customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'percentage of the 13. component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'PART_COMP_13';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'additional name referenced to the costumer code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'percentage of the 1. component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'PART_COMP_01';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''I'' Insert; ''U'' Update; ''D'' Delete', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'ACTIVITY';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'percentage of the 14. component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'PART_COMP_14';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique identifier for import', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'CUSTOMER_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'percentage of the 2. component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'PART_COMP_02';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred (set by Customer, if processed )', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'type of the 2. component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'TYPE_COMP_02';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'type of the 13. component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'TYPE_COMP_13';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'INFO_TXT';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 Green coffee; 2 Roasted coffee; 3 Ground coffee', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_BLENDS', @level2type = N'COLUMN', @level2name = N'ZONE';


GO

