CREATE TABLE [dbo].[PRO_EXP_TRACE_BKW_FB] (
    [TRANSFERED]            INT          NOT NULL,
    [TRANSFERED_TIMESTAMP]  DATETIME     NULL,
    [ACTIVITY]              CHAR (1)     NOT NULL,
    [MASTER_ID]             INT          NOT NULL,
    [LOT_NAME]              VARCHAR (20) NOT NULL,
    [ZONE]                  INT          NOT NULL,
    [CUSTOMER_CODE]         VARCHAR (20) NOT NULL,
    [USER_ID]               VARCHAR (20) NOT NULL,
    [JOB_ID]                VARCHAR (20) NOT NULL,
    [REQUEST_TIMESTAMP]     DATETIME     NOT NULL,
    [FB_MASTER_ID]          INT          NOT NULL,
    [FB_PALLET_ID]          VARCHAR (20) NOT NULL,
    [FB_ZONE]               INT          NOT NULL,
    [FB_CUSTOMER_CODE]      VARCHAR (20) NOT NULL,
    [FB_RECEIVING_STATION]  VARCHAR (20) NOT NULL,
    [PRO_EXPORT_GENERAL_ID] INT          NULL
);


GO

