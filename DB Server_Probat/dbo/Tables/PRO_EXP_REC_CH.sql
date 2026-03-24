CREATE TABLE [dbo].[PRO_EXP_REC_CH] (
    [TRANSFERED]            INT          NULL,
    [TRANSFERED_TIMESTAMP]  DATETIME     NULL,
    [RECORDING_DATE]        DATETIME     NULL,
    [ZONE]                  INT          NULL,
    [ORDER_NAME]            VARCHAR (20) NULL,
    [DELIVERY_NAME]         VARCHAR (20) NULL,
    [LOT_NAME]              VARCHAR (20) NULL,
    [WEIGHT]                INT          NULL,
    [CUSTOMER_CODE]         VARCHAR (20) NULL,
    [DESTINATION]           VARCHAR (20) NULL,
    [PRO_EXPORT_GENERAL_ID] INT          NOT NULL,
    [DATA_1]                VARCHAR (20) NULL,
    [DATA_2]                VARCHAR (20) NULL,
    [DATA_3]                VARCHAR (20) NULL,
    [DATA_11]               VARCHAR (20) NULL,
    [DATA_12]               VARCHAR (20) NULL,
    [DATA_13]               VARCHAR (20) NULL,
    CONSTRAINT [PK_PRO_EXP_REC_CH] PRIMARY KEY CLUSTERED ([PRO_EXPORT_GENERAL_ID] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_PRO_EXP_REC_CH]
    ON [dbo].[PRO_EXP_REC_CH]([TRANSFERED] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'contract', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'ORDER_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'belongs to the contract', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'DATA_3';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'customer code of the silo cell', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'DESTINATION';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 decimal place; acquisition weight', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'WEIGHT';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'belongs to the contract', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'DATA_2';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 Green coffee; 2 Roasted coffee; 3 Ground coffee', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'ZONE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'customer code of coffee type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'belongs to the delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'DATA_13';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TrackingID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'LOT_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp, set by probat and customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'belongs to the contract', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'DATA_1';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'belongs to the delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'DATA_12';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'date of acquisition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'RECORDING_DATE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'DELIVERY_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred (set by Customer, if processed )', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique Export ID, explained in table PRO_EXP_PROCESS_SEQ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'PRO_EXPORT_GENERAL_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'belongs to the delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_REC_CH', @level2type = N'COLUMN', @level2name = N'DATA_11';


GO

