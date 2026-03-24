CREATE TABLE [dbo].[PRO_EXP_ERROR_CODE] (
    [TRANSFERED]            INT          NULL,
    [TRANSFERED_TIMESTAMP]  DATETIME     NULL,
    [TABLE_NO]              INT          NULL,
    [TABLE_NAME]            VARCHAR (40) NULL,
    [ERROR_CODE]            INT          NULL,
    [ERROR_TXT]             VARCHAR (80) NULL,
    [CUSTOMER_CODE]         VARCHAR (20) NULL,
    [ORDER_TYP]             INT          NULL,
    [ORDER_NAME]            VARCHAR (20) NULL,
    [DESTINATION]           VARCHAR (20) NULL,
    [PRO_EXPORT_GENERAL_ID] INT          NULL,
    [CUSTOMER_ID]           INT          NULL
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique Export ID, recordno. of table, which has to be imported', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ERROR_CODE', @level2type = N'COLUMN', @level2name = N'PRO_EXPORT_GENERAL_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp, set by probat and customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ERROR_CODE', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'If available', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ERROR_CODE', @level2type = N'COLUMN', @level2name = N'DESTINATION';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred (set by Customer, if processed)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ERROR_CODE', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Table name with error', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ERROR_CODE', @level2type = N'COLUMN', @level2name = N'TABLE_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique identifier for import', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ERROR_CODE', @level2type = N'COLUMN', @level2name = N'CUSTOMER_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Table no. with error. e.g. 102 - PRO_EXT_SUM_DEST;  103 - PRO_EXT_SUM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ERROR_CODE', @level2type = N'COLUMN', @level2name = N'TABLE_NO';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Error text', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ERROR_CODE', @level2type = N'COLUMN', @level2name = N'ERROR_TXT';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Error code no', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ERROR_CODE', @level2type = N'COLUMN', @level2name = N'ERROR_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'If available 2: Roaster; 3: Wholebean pack; 4: Grinder; 5: Ground coffee pack', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ERROR_CODE', @level2type = N'COLUMN', @level2name = N'ORDER_TYP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'If available', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ERROR_CODE', @level2type = N'COLUMN', @level2name = N'CUSTOMER_CODE';


GO

