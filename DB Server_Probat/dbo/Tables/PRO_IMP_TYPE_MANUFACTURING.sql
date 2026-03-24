CREATE TABLE [dbo].[PRO_IMP_TYPE_MANUFACTURING] (
    [CUSTOMER_ID]          INT          NOT NULL,
    [TRANSFERED]           INT          NOT NULL,
    [TRANSFERED_TIMESTAMP] DATETIME     NULL,
    [ACTIVITY]             VARCHAR (1)  NOT NULL,
    [ZONE]                 INT          NOT NULL,
    [CUSTOMER_CODE]        VARCHAR (20) NOT NULL,
    [EXT_POS_NAME]         VARCHAR (20) NOT NULL,
    [LOAD_METHOD]          INT          NOT NULL,
    [TYPE_100_NAME]        VARCHAR (20) NULL,
    [BLEND_NAME]           VARCHAR (20) NULL,
    [REQUEST_WEIGHT]       INT          NULL,
    [LOSS]                 INT          NULL,
    [ARCHIV_NR]            INT          NULL,
    [USE_OPTIONAL_COMP]    INT          NOT NULL,
    [OPTIONAL_COMP_NAME]   VARCHAR (20) NULL,
    [OPTIONAL_COMP_PART]   INT          NULL,
    CONSTRAINT [PK_PRO_IMP_TYPE_MANUFACTURING] PRIMARY KEY CLUSTERED ([CUSTOMER_ID] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''I'' Insert; ''U'' Update; ''D'' Delete', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'ACTIVITY';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 Green coffee; 2 Roasted coffee; 3 Ground coffee', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'ZONE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp, set by probat and customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CUSTOMER_CODE of the type. The CUSTOMER_CODE will be shown in all dialogs and silos. ZONE+CUSTOMER_CODE must be unique', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Defines the typename by LOAD_METHOD = 1; Depending on the receiving station(EXT_POS_NAME) it must be a green-, roast- or green coffee type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'TYPE_100_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Customer name (silo, roaster, grinder or packing machine) Reference in Probat table positions (identifies the row to be edited )', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'EXT_POS_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred (set by Customer, if processed )', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique identifier for import', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'CUSTOMER_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0: blocked; 1: use only one type (100%) (no blend needed); 2: use a blending program', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'LOAD_METHOD';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Defines the typename by LOAD_METHOD = 2; Depending on the receiving station(EXT_POS_NAME) it must be a green-, roast- or green coffee type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'BLEND_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Part [%] of the optional component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'OPTIONAL_COMP_PART';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Int. 2 dp.; for example by roasters it is the shrinkage.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'LOSS';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Integer 1 dp; How much coffee will be send by one request.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'REQUEST_WEIGHT';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'for example by roasters it is the recipe no. of the roaster itself.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'ARCHIV_NR';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0/1 use optional component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'USE_OPTIONAL_COMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type name of the optional component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPE_MANUFACTURING', @level2type = N'COLUMN', @level2name = N'OPTIONAL_COMP_NAME';


GO

