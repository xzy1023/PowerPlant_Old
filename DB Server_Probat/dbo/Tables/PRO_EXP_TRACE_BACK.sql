CREATE TABLE [dbo].[PRO_EXP_TRACE_BACK] (
    [TRANSFERED]            INT          NOT NULL,
    [TRANSFERED_TIMESTAMP]  DATETIME     NULL,
    [RECORDING_DATE]        DATETIME     NULL,
    [POS_NAME]              VARCHAR (20) NULL,
    [ORDER_NAME]            VARCHAR (20) NULL,
    [MASTER_ID]             INT          NULL,
    [CUSTOMER_CODE]         VARCHAR (20) NULL,
    [BASIC_MASTER_ID]       INT          NULL,
    [BASIC_CUSTOMER_CODE]   VARCHAR (20) NULL,
    [BASIC_LOT_NAME]        VARCHAR (20) NULL,
    [PERCENTAGE]            INT          NULL,
    [START_FLAG]            INT          NULL,
    [END_FLAG]              INT          NULL,
    [PRO_EXPORT_GENERAL_ID] INT          NULL
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mother Parker’s TrackingID of the component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_BACK', @level2type = N'COLUMN', @level2name = N'BASIC_LOT_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Customer code of the master ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_BACK', @level2type = N'COLUMN', @level2name = N'CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of position of the product', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_BACK', @level2type = N'COLUMN', @level2name = N'POS_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'marks the last record, which belongs to this export', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_BACK', @level2type = N'COLUMN', @level2name = N'END_FLAG';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Customer code of the component(green coffee)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_BACK', @level2type = N'COLUMN', @level2name = N'BASIC_CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat internal product number for the batch. Identifies the product composition.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_BACK', @level2type = N'COLUMN', @level2name = N'MASTER_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'marks the first record, which belongs to this export', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_BACK', @level2type = N'COLUMN', @level2name = N'START_FLAG';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'date of acquisition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_BACK', @level2type = N'COLUMN', @level2name = N'RECORDING_DATE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp, set by probat and customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_BACK', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique Export ID, explained in table PRO_EXP_PROCESS_SEQ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_BACK', @level2type = N'COLUMN', @level2name = N'PRO_EXPORT_GENERAL_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred (set by Customer, if processed)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_BACK', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat internal product_id of the component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_BACK', @level2type = N'COLUMN', @level2name = N'BASIC_MASTER_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'order name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_BACK', @level2type = N'COLUMN', @level2name = N'ORDER_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'3 decimal places; Percentage of the component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_BACK', @level2type = N'COLUMN', @level2name = N'PERCENTAGE';


GO

