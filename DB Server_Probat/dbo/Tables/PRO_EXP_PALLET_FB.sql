CREATE TABLE [dbo].[PRO_EXP_PALLET_FB] (
    [TRANSFERED]            INT          NOT NULL,
    [TRANSFERED_TIMESTAMP]  DATETIME     NULL,
    [RECORDING_DATE]        DATETIME     NULL,
    [DESTINATION]           VARCHAR (20) NULL,
    [ORDER_NAME]            VARCHAR (20) NULL,
    [ORDER_TYP]             INT          NOT NULL,
    [PALLET_ID]             VARCHAR (20) NOT NULL,
    [MASTER_ID]             INT          NOT NULL,
    [CUSTOMER_CODE]         VARCHAR (20) NOT NULL,
    [S_PRODUCT_ID]          INT          NULL,
    [S_CUSTOMER_CODE]       VARCHAR (20) NULL,
    [WEIGHT]                INT          NOT NULL,
    [START_FLAG]            INT          NULL,
    [END_FLAG]              INT          NULL,
    [PRO_EXPORT_GENERAL_ID] INT          NULL,
    [LOCATION]              VARCHAR (20) NULL
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred  (set by Customer, if processed)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PALLET_FB', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp, set by probat and customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PALLET_FB', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'date of acquisition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PALLET_FB', @level2type = N'COLUMN', @level2name = N'RECORDING_DATE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of receiving station', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PALLET_FB', @level2type = N'COLUMN', @level2name = N'DESTINATION';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'order of receiving station', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PALLET_FB', @level2type = N'COLUMN', @level2name = N'ORDER_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'3: Packmachine whole bean; 5: Packmachine ground', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PALLET_FB', @level2type = N'COLUMN', @level2name = N'ORDER_TYP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'unique batch id ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PALLET_FB', @level2type = N'COLUMN', @level2name = N'PALLET_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat internal product number to Identify the product composition.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PALLET_FB', @level2type = N'COLUMN', @level2name = N'MASTER_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Customer code of the roast coffee type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PALLET_FB', @level2type = N'COLUMN', @level2name = N'CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat internal product_id of the component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PALLET_FB', @level2type = N'COLUMN', @level2name = N'S_PRODUCT_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Customer code of the component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PALLET_FB', @level2type = N'COLUMN', @level2name = N'S_CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'acquisition weight of the component. 3 decimal places.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PALLET_FB', @level2type = N'COLUMN', @level2name = N'WEIGHT';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'marks the first record, which belongs to the batch', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PALLET_FB', @level2type = N'COLUMN', @level2name = N'START_FLAG';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'marks the last record, which belongs to the batch', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PALLET_FB', @level2type = N'COLUMN', @level2name = N'END_FLAG';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique Export ID, explained in table PRO_EXP_PROCESS_SEQ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PALLET_FB', @level2type = N'COLUMN', @level2name = N'PRO_EXPORT_GENERAL_ID';


GO

