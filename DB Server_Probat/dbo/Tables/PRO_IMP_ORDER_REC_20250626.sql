CREATE TABLE [dbo].[PRO_IMP_ORDER_REC_20250626] (
    [CUSTOMER_ID]          INT           NOT NULL,
    [TRANSFERED]           INT           NOT NULL,
    [TRANSFERED_TIMESTAMP] DATETIME      NULL,
    [ACTIVITY]             VARCHAR (1)   NOT NULL,
    [ORDER_NAME]           VARCHAR (20)  NOT NULL,
    [DELIVERY_NAME]        VARCHAR (20)  NOT NULL,
    [SCHEDULED_TIME]       DATETIME      NOT NULL,
    [AMOUNT_ORDER]         INT           NOT NULL,
    [AMOUNT_DELIVERY]      INT           NOT NULL,
    [ZONE]                 INT           NOT NULL,
    [CUSTOMER_CODE]        VARCHAR (20)  NOT NULL,
    [ORDER_DESCRIPTION]    VARCHAR (200) NULL,
    [DELIVERY_DESCRIPTION] VARCHAR (200) NULL,
    [LOT_NAME]             VARCHAR (20)  NOT NULL,
    [DATA_1]               VARCHAR (20)  NULL,
    [DATA_2]               VARCHAR (20)  NULL,
    [DATA_3]               VARCHAR (20)  NULL,
    [DATA_11]              VARCHAR (20)  NULL,
    [DATA_12]              VARCHAR (20)  NULL,
    [DATA_13]              VARCHAR (20)  NULL
);


GO

