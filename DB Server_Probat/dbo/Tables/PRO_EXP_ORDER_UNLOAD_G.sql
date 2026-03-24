CREATE TABLE [dbo].[PRO_EXP_ORDER_UNLOAD_G] (
    [TRANSFERED]            INT          NOT NULL,
    [TRANSFERED_TIMESTAMP]  DATETIME     NOT NULL,
    [RECORDING_DATE]        DATETIME     NULL,
    [ORDER_NAME]            VARCHAR (20) NULL,
    [BATCH_ID]              INT          NULL,
    [S_MASTER_ID]           INT          NULL,
    [SOURCE_NAME]           VARCHAR (20) NULL,
    [S_CUSTOMER_CODE]       VARCHAR (20) NULL,
    [DEST_NAME]             VARCHAR (20) NULL,
    [D_MASTER_ID]           INT          NULL,
    [D_CUSTOMER_CODE]       VARCHAR (20) NULL,
    [WEIGHT]                INT          NULL,
    [PRO_EXPORT_GENERAL_ID] INT          NULL,
    [LOCATION]              VARCHAR (20) NULL
);


GO



Create TRIGGER [tgrJrn_PRO_EXP_ORDER_UNLOAD_G]
ON [dbo].[PRO_EXP_ORDER_UNLOAD_G]
After INSERT
AS  
BEGIN
	Insert into [Probat01_Prd_Jrn].[dbo].[PRO_EXP_ORDER_UNLOAD_G]
	Select * from inserted  
End

GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp, set by probat and customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_UNLOAD_G', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'name Sof the grinder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_UNLOAD_G', @level2type = N'COLUMN', @level2name = N'SOURCE_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique Export ID, explained in table PRO_EXP_PROCESS_SEQ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_UNLOAD_G', @level2type = N'COLUMN', @level2name = N'PRO_EXPORT_GENERAL_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'date of acquisition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_UNLOAD_G', @level2type = N'COLUMN', @level2name = N'RECORDING_DATE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'“S6700”, special field Mother Parkers', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_UNLOAD_G', @level2type = N'COLUMN', @level2name = N'LOCATION';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'type of destination, ground coffee type name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_UNLOAD_G', @level2type = N'COLUMN', @level2name = N'D_CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred (set by Customer, if processed )', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_UNLOAD_G', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat internal product number for the batch identifies the product input grinder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_UNLOAD_G', @level2type = N'COLUMN', @level2name = N'S_MASTER_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 decimal place; acquisition weight of the batch', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_UNLOAD_G', @level2type = N'COLUMN', @level2name = N'WEIGHT';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat internal product number for the batch identifies the product output grinder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_UNLOAD_G', @level2type = N'COLUMN', @level2name = N'D_MASTER_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'unique batch id (Probat internal number)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_UNLOAD_G', @level2type = N'COLUMN', @level2name = N'BATCH_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'type of the source, ground coffee type, type ->grinder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_UNLOAD_G', @level2type = N'COLUMN', @level2name = N'S_CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'destination, name of the silo cell', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_UNLOAD_G', @level2type = N'COLUMN', @level2name = N'DEST_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'order grinder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_ORDER_UNLOAD_G', @level2type = N'COLUMN', @level2name = N'ORDER_NAME';


GO

