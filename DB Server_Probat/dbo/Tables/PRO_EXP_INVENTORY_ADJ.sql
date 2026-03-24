CREATE TABLE [dbo].[PRO_EXP_INVENTORY_ADJ] (
    [TRANSFERED]            INT          NOT NULL,
    [TRANSFERED_TIMESTAMP]  DATETIME     NULL,
    [MASTER_ID]             INT          NOT NULL,
    [LOT_NAME]              VARCHAR (20) NOT NULL,
    [ZONE]                  INT          NOT NULL,
    [CUSTOMER_CODE]         VARCHAR (20) NOT NULL,
    [AMOUNT]                INT          NOT NULL,
    [CUSTOMER_CODE_RD]      VARCHAR (20) NOT NULL,
    [REASON_CODE]           VARCHAR (20) NOT NULL,
    [PRO_EXPORT_GENERAL_ID] INT          NULL,
    [LOCATION]              VARCHAR (20) NULL
);


GO

