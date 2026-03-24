CREATE TABLE [dbo].[PRO_IMP_TYPES] (
    [CUSTOMER_ID]          INT           NOT NULL,
    [TRANSFERED]           INT           NOT NULL,
    [TRANSFERED_TIMESTAMP] DATETIME      NULL,
    [ACTIVITY]             VARCHAR (1)   NOT NULL,
    [ZONE]                 INT           NOT NULL,
    [CUSTOMER_CODE]        VARCHAR (20)  NOT NULL,
    [NAME]                 VARCHAR (20)  NULL,
    [MACO]                 VARCHAR (20)  NULL,
    [HOLDING_TIME]         INT           NULL,
    [COLOR]                INT           NULL,
    [COLOR_MIN]            INT           NULL,
    [COLOR_MAX]            INT           NULL,
    [DENSITY]              INT           NULL,
    [DENSITY_MIN]          INT           NULL,
    [DENSITY_MAX]          INT           NULL,
    [PACKAGE_SIZE]         INT           NULL,
    [HUMIDITY]             INT           NULL,
    [HUMIDITY_MIN]         INT           NULL,
    [HUMIDITY_MAX]         INT           NULL,
    [CALC_LOSS]            INT           NULL,
    [DESCRIPTION]          VARCHAR (200) NULL,
    [SIEVE_TARGET_1]       INT           NULL,
    [SIEVE_MIN_1]          INT           NULL,
    [SIEVE_MAX_1]          INT           NULL,
    [SIEVE_TARGET_2]       INT           NULL,
    [SIEVE_MIN_2]          INT           NULL,
    [SIEVE_MAX_2]          INT           NULL,
    [SIEVE_TARGET_3]       INT           NULL,
    [SIEVE_MIN_3]          INT           NULL,
    [SIEVE_MAX_3]          INT           NULL,
    [KIND_OF_COFFEE]       INT           NULL,
    CONSTRAINT [PK_PRO_IMP_TYPES_1] PRIMARY KEY CLUSTERED ([CUSTOMER_ID] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_PRO_IMP_TYPES]
    ON [dbo].[PRO_IMP_TYPES]([CUSTOMER_CODE] ASC, [CUSTOMER_ID] DESC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The below numeric values are the Power Index of 2.(e.g.:ORGANIC=0000000000000100 in binary);0=N/A;1=decaf;2=organic;3=UTZ;4=fairtrade;5=low quality (e.g. broken beans);6=rework allowed;7=arabica 100%;8=purchased (e.g. coffee from another customer);9=rainforest (Rainforest Alliance Certified);10=rework2 allowed;11=no coffee(product is no coffee, e.g. flavor components);12=flavor (flavored coffee);13=N/A;14=N/A;15=N/A.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'KIND_OF_COFFEE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 Green coffee; 2 Roasted coffee; 3 Ground coffee', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'ZONE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Int 2dp; Max. Color ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'COLOR_MAX';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CUSTOMER_CODE of the type. The CUSTOMER_CODE will be shown in all dialogs and silos. ZONE+CUSTOMER_CODE must be unique', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'integer in 2 decimal places; Min. Moisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'HUMIDITY_MIN';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'int  2dp; Target value for sieve 3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'SIEVE_TARGET_3';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'int  2dp; Max. value for sieve 1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'SIEVE_MAX_1';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Int 2dp; Density of the type, in Europe[kg/m³] (the unit has to be declare)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'DENSITY';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'int  2dp; Target value for sieve 1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'SIEVE_TARGET_1';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred (set by Customer, if processed )', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'int  2dp; Max. value for sieve 3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'SIEVE_MAX_3';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Int 2dp; Min. Color ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'COLOR_MIN';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'integer in 2 decimal places', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'HUMIDITY';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'int  2dp; Max. value for sieve 2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'SIEVE_MAX_2';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'int  2dp; Min. value for sieve 1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'SIEVE_MIN_1';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''I'' Insert; ''U'' Update; ''D'' Delete', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'ACTIVITY';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'integer in 2 decimal places. Calculated loss for each type (for example shrinkage). e.g. green coffee amount = roasted coffee amount / ( 1-CALC_LOSS)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'CALC_LOSS';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Int 2dp; Maximum Density of the type, in Europe[kg/m³] (the unit has to be declare)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'DENSITY_MAX';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Holding time [min] (for example degasing time)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'HOLDING_TIME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique identifier for import', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'CUSTOMER_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Package Size, in Europe [g] (the unit has to be declare)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'PACKAGE_SIZE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Int 2dp', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'COLOR';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'int  2dp; Min. value for sieve 2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'SIEVE_MIN_2';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reference to CUSTOMER_CODE. For example brasil, columbia …', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Int 2dp; Minimum Density of the type, in Europe[kg/m³] (the unit has to be declare)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'DENSITY_MIN';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'integer in 2 decimal places; Max. Moisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'HUMIDITY_MAX';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reference to CUSTOMER_CODE. Material code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'MACO';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'int  2dp; Min. value for sieve 3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'SIEVE_MIN_3';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'int  2dp; Target value for sieve 2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_IMP_TYPES', @level2type = N'COLUMN', @level2name = N'SIEVE_TARGET_2';


GO

