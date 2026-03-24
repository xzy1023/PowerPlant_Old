CREATE TABLE [dbo].[PRO_IMP_COMMAND] (
    [TRANSFERED]           INT          NULL,
    [TRANSFERED_TIMESTAMP] DATETIME     NULL,
    [CUSTOMER_ID]          INT          NOT NULL,
    [COMMAND_NR]           INT          NULL,
    [PARA_SWITCH]          INT          NULL,
    [PARA_1_INT]           INT          NULL,
    [PARA_2_INT]           INT          NULL,
    [PARA_3_INT]           INT          NULL,
    [PARA_4_INT]           INT          NULL,
    [PARA_5_INT]           INT          NULL,
    [PARA_1_NAME]          VARCHAR (20) NULL,
    [PARA_2_NAME]          VARCHAR (20) NULL,
    [PARA_3_NAME]          VARCHAR (20) NULL,
    [PARA_4_NAME]          VARCHAR (20) NULL,
    [INFO1]                VARCHAR (80) NULL,
    CONSTRAINT [PK_PRO_IMP_COMMAND] PRIMARY KEY CLUSTERED ([CUSTOMER_ID] ASC)
);


GO

