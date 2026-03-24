CREATE TABLE [dbo].[PRO_EXP_PLANT2PLANT] (
    [TRANSFERED]            INT          NOT NULL,
    [TRANSFERED_TIMESTAMP]  DATETIME     NULL,
    [RECORDING_DATE]        DATETIME     NULL,
    [ZONE]                  INT          NOT NULL,
    [DEST_LOCATION]         VARCHAR (20) NULL,
    [MASTER_ID]             INT          NULL,
    [CUSTOMER_CODE]         VARCHAR (20) NULL,
    [SOURCE]                VARCHAR (20) NULL,
    [S_PRODUCT_ID]          INT          NULL,
    [WEIGHT]                INT          NULL,
    [PRO_EXPORT_GENERAL_ID] INT          NULL
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Customer code of coffee', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PLANT2PLANT', @level2type = N'COLUMN', @level2name = N'CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'From Facility', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PLANT2PLANT', @level2type = N'COLUMN', @level2name = N'SOURCE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat internal product number for the batch identifies the product composition at the From Facility', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PLANT2PLANT', @level2type = N'COLUMN', @level2name = N'S_PRODUCT_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'To Facility', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PLANT2PLANT', @level2type = N'COLUMN', @level2name = N'DEST_LOCATION';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat internal product number for the batch identifies the product composition at the To Facility', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PLANT2PLANT', @level2type = N'COLUMN', @level2name = N'MASTER_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred (set by Customer, if processed )', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PLANT2PLANT', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp, set by probat and customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PLANT2PLANT', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'date of acquisition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PLANT2PLANT', @level2type = N'COLUMN', @level2name = N'RECORDING_DATE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Zone (1-Green Bean, 2=Roast Bean, 3=Ground Coffee', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PLANT2PLANT', @level2type = N'COLUMN', @level2name = N'ZONE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'3 decimal places;acquisition weight of the component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PLANT2PLANT', @level2type = N'COLUMN', @level2name = N'WEIGHT';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique Export ID, explained in table PRO_EXP_PROCESS_SEQ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_PLANT2PLANT', @level2type = N'COLUMN', @level2name = N'PRO_EXPORT_GENERAL_ID';


GO

