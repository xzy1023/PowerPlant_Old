CREATE TABLE [dbo].[PRO_EXP_TRANSF] (
    [TRANSFERED]            INT          NULL,
    [TRANSFERED_TIMESTAMP]  DATETIME     NULL,
    [RECORDING_DATE]        DATETIME     NULL,
    [DESTINATION]           VARCHAR (20) NULL,
    [ORDER_NAME]            VARCHAR (20) NULL,
    [BATCH_ID]              INT          NULL,
    [MASTER_ID]             INT          NULL,
    [CUSTOMER_CODE]         VARCHAR (20) NULL,
    [SOURCE]                VARCHAR (20) NULL,
    [S_PRODUCT_ID]          INT          NULL,
    [S_CUSTOMER_CODE]       VARCHAR (20) NULL,
    [S_TYPE_CELL]           VARCHAR (20) NULL,
    [S_EMPTY]               INT          NULL,
    [WEIGHT]                INT          NULL,
    [START_FLAG]            INT          NULL,
    [END_FLAG]              INT          NULL,
    [PRO_EXPORT_GENERAL_ID] INT          NOT NULL,
    [S_LOT_NAME]            VARCHAR (20) NULL,
    CONSTRAINT [PK_PRO_EXP_TRANSF] PRIMARY KEY CLUSTERED ([PRO_EXPORT_GENERAL_ID] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_PRO_EXP_TRANSF]
    ON [dbo].[PRO_EXP_TRANSF]([TRANSFERED] ASC);


GO



Create TRIGGER [dbo].[tgrJrn_PRO_EXP_TRANSF]
ON [dbo].[PRO_EXP_TRANSF]
After INSERT
AS  
BEGIN
	BEGIN TRY
		IF Exists (select distinct [ORDER_TYP] ,[ORDER_NAME] FROM [dbo].[PRO_IMP_ORDER] 
			WHERE ORDER_TYP = 60 AND [ORDER_NAME] = (SELECT ORDER_NAME FROM inserted)
		)
		BEGIN
			INSERT INTO [dbo].[JRN_EXP_TRANSF]
			SELECT inserted.* from inserted  

			UPDATE PRO_EXP_TRANSF SET MASTER_ID = S_PRODUCT_ID 
				WHERE [PRO_EXPORT_GENERAL_ID] = (SELECT PRO_EXPORT_GENERAL_ID FROM inserted)
		END 
	END TRY
	BEGIN CATCH
	END CATCH;
End

GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat internal product number for the batch identifies the product composition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'MASTER_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'marks the last record, which belongs to the batch', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'END_FLAG';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of source silo cell', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'SOURCE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Customer code of the component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'S_CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Customer code of the component (cell information)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'S_TYPE_CELL';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'date of acquisition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'RECORDING_DATE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'3 decimal places; acquisition weight of the component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'WEIGHT';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'order of destination cell', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'ORDER_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer status - 0: default; 1: Transferred (set by Customer, if processed)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'TRANSFERED';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'marks the first record, which belongs to the batch', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'START_FLAG';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'unique batch id (Probat internal number)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'BATCH_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Customer code of the coffee type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'CUSTOMER_CODE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat internal product_id of the component', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'S_PRODUCT_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique Export ID, explained in table PRO_EXP_PROCESS_SEQ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'PRO_EXPORT_GENERAL_ID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mother Parkers Lot Data', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'S_LOT_NAME';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FLAG, if source cell goes empty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'S_EMPTY';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Insert/Update timestamp, set by probat and customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'TRANSFERED_TIMESTAMP';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of destination cell', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PRO_EXP_TRANSF', @level2type = N'COLUMN', @level2name = N'DESTINATION';


GO

