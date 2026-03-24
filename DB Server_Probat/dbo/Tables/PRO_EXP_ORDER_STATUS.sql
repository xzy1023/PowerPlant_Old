CREATE TABLE [dbo].[PRO_EXP_ORDER_STATUS] (
    [TRANSFERED]            INT          NULL,
    [TRANSFERED_TIMESTAMP]  DATETIME     NULL,
    [RECORDING_DATE]        DATETIME     NULL,
    [ORDER_NAME]            VARCHAR (20) NULL,
    [ORDER_SUB_NAME]        VARCHAR (20) NULL,
    [ORDER_TYP]             INT          NULL,
    [STATUS]                INT          NULL,
    [SCHEDULED_WEIGHT]      INT          NULL,
    [RESULT_WEIGHT]         INT          NULL,
    [YIELD]                 INT          NULL,
    [PRO_EXPORT_GENERAL_ID] INT          NULL
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1: Receivings Green, Roast and Ground; 2: Roasters; 3: Packmachines whole bean; 4: Grinders; 5 : Packmachines Ground; 6: Relocation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_STATUS', @level2type = N'COLUMN', @level2name = N'ORDER_TYP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique Export ID, explained in table PRO_EXP_PROCESS_SEQ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_STATUS', @level2type = N'COLUMN', @level2name = N'PRO_EXPORT_GENERAL_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'date of acquisition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_STATUS', @level2type = N'COLUMN', @level2name = N'RECORDING_DATE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_STATUS', @level2type = N'COLUMN', @level2name = N'YIELD';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp, set by probat and customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_STATUS', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred (set by Customer, if processed)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_STATUS', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 decimal place; Weight of order', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_STATUS', @level2type = N'COLUMN', @level2name = N'RESULT_WEIGHT';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of suborder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_STATUS', @level2type = N'COLUMN', @level2name = N'ORDER_SUB_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 decimal place; Scheduled weight of order.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_STATUS', @level2type = N'COLUMN', @level2name = N'SCHEDULED_WEIGHT';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of order', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_STATUS', @level2type = N'COLUMN', @level2name = N'ORDER_NAME';


GO

