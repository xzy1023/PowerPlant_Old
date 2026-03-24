CREATE TABLE [dbo].[PRO_EXP_BATCH_CHECK] (
    [TRANSFERED]            INT          NOT NULL,
    [TRANSFERED_TIMESTAMP]  DATETIME     NULL,
    [RECORDING_DATE]        DATETIME     NULL,
    [BATCH_ID]              INT          NOT NULL,
    [ORDER_TYP]             INT          NOT NULL,
    [CUSTOMER_CODE]         VARCHAR (20) NULL,
    [ORDER_NAME]            VARCHAR (20) NULL,
    [END_TEMP]              INT          NULL,
    [WATER]                 INT          NULL,
    [COLOR]                 INT          NULL,
    [COLOR_MIN]             INT          NULL,
    [COLOR_MAX]             INT          NULL,
    [MOISTURE]              INT          NULL,
    [MOISTURE_MIN]          INT          NULL,
    [MOISTURE_MAX]          INT          NULL,
    [DENSITY]               INT          NULL,
    [DENSITY_MIN]           INT          NULL,
    [DENSITY_MAX]           INT          NULL,
    [CREATED]               DATETIME     NULL,
    [PLC_BATCH_NR]          INT          NULL,
    [INPUT_WEIGHT]          INT          NULL,
    [SOURCE_MACHINE]        VARCHAR (20) NULL,
    [DESTINATION]           VARCHAR (20) NULL,
    [PRO_EXPORT_GENERAL_ID] INT          NULL,
    [MOISTURE_MANUAL]       INT          NULL
);


GO

CREATE NONCLUSTERED INDEX [IX_PRO_EXP_BATCH_CHECK]
    ON [dbo].[PRO_EXP_BATCH_CHECK]([ORDER_TYP] ASC, [CREATED] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'2 decimal places; Min. Density defined in type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'DENSITY_MIN';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'2 decimal places; Max. Density defined in type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'DENSITY_MAX';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Customer code of the coffee type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 decimal place; Final roasting temperature (value from batch protocol)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'END_TEMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'2 decimal places; Max. moisture defined in type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'MOISTURE_MAX';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'2 decimal places; Min. moisture defined in type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'MOISTURE_MIN';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 decimal place; Water precooling (value from batch protocol)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'WATER';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Roasting/grinding order name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'ORDER_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'2 decimal places; Max. Density of the batch', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'DENSITY';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'2 decimal places; Max. color of the batch', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'COLOR';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'2 decimal places; Moisture of the batch', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'MOISTURE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'2 decimal places; Min. color defined in type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'COLOR_MIN';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'2 decimal places; Max. color defined in type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'COLOR_MAX';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred (set by Customer, if processed)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of source (roaster or grinder)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'SOURCE_MACHINE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 decimal place; Input weight of the charge', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'INPUT_WEIGHT';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp, set by probat and customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PLC roasting batch number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'PLC_BATCH_NR';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'date of acquisition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'RECORDING_DATE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'2: Roasters; 4: Grinders', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'ORDER_TYP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'unique batch id (Probat internal number)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'BATCH_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Time batch was created', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'CREATED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'2 decimal places; Manually entered moisture value', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'MOISTURE_MANUAL';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique Export ID, explained in table PRO_EXP_PROCESS_SEQ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'PRO_EXPORT_GENERAL_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of the silo cell', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_BATCH_CHECK', @level2type = N'COLUMN', @level2name = N'DESTINATION';


GO

