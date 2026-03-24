CREATE TABLE [dbo].[PRO_EXP_TRACE_FORWARD] (
    [TRANSFERED]            INT          NOT NULL,
    [TRANSFERED_TIMESTAMP]  DATETIME     NULL,
    [RECORDING_DATE]        DATETIME     NULL,
    [POS_NAME]              VARCHAR (20) NULL,
    [MASTER_ID]             INT          NULL,
    [ORDER_NAME]            VARCHAR (20) NULL,
    [LOT_NAME]              VARCHAR (20) NULL,
    [CUSTOMER_CODE]         VARCHAR (20) NULL,
    [D_MASTER_ID]           INT          NULL,
    [D_PALLET_ID]           VARCHAR (20) NULL,
    [D_CUSTOMER_CODE]       VARCHAR (20) NULL,
    [D_ORDER_NAME]          VARCHAR (20) NULL,
    [D_POS_NAME]            VARCHAR (20) NULL,
    [START_FLAG]            INT          NULL,
    [END_FLAG]              INT          NULL,
    [PRO_EXPORT_GENERAL_ID] INT          NULL
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'packaging machine', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'D_POS_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of position of the product', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'POS_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Customer code of the component(green coffee)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'D_CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Customer code of the master ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'order name of the contract', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'ORDER_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred (set by Customer, if processed)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'marks the last record, which belongs to this export', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'END_FLAG';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'marks the first record, which belongs to this export', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'START_FLAG';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat internal product number for the batch. Identifies the product composition.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'MASTER_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'order name of the packaging order', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'D_ORDER_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pallet-ID from Mother Parkers', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'D_PALLET_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'date of acquisition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'RECORDING_DATE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat internal product number for the batch. Identifies the product composition.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'D_MASTER_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp, set by probat and customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mother Parker’s TrackingID of the component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'LOT_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique Export ID, explained in table PRO_EXP_PROCESS_SEQ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRACE_FORWARD', @level2type = N'COLUMN', @level2name = N'PRO_EXPORT_GENERAL_ID';


GO

