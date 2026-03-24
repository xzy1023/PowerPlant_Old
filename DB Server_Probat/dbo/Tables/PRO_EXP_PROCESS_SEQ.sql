CREATE TABLE [dbo].[PRO_EXP_PROCESS_SEQ] (
    [TRANSFERED]            INT          NULL,
    [TRANSFERED_TIMESTAMP]  DATETIME     NULL,
    [PROCESS_TIME]          DATETIME     NULL,
    [PRO_EXPORT_GENERAL_ID] INT          NULL,
    [TABLE_NO]              INT          NULL,
    [TABLE_NAME]            VARCHAR (40) NULL
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique Export ID, recordno. of table which has to be imported', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PROCESS_SEQ', @level2type = N'COLUMN', @level2name = N'PRO_EXPORT_GENERAL_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'table no. of table, which has to be processed. For example: 102 PRO_EXT_SUM_DEST, 103 PRO_EXT_SUM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PROCESS_SEQ', @level2type = N'COLUMN', @level2name = N'TABLE_NO';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp, set by probat and customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PROCESS_SEQ', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date of acquisition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PROCESS_SEQ', @level2type = N'COLUMN', @level2name = N'PROCESS_TIME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred (set by Customer, if processed)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PROCESS_SEQ', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

