CREATE TABLE [dbo].[PRO_IMP_TRACE_BKW] (
    [CUSTOMER_ID]          INT          NOT NULL,
    [TRANSFERED]           INT          NOT NULL,
    [TRANSFERED_TIMESTAMP] DATETIME     NULL,
    [ACTIVITY]             CHAR (1)     NOT NULL,
    [PALLET_ID]            VARCHAR (20) NOT NULL,
    [MASTER_ID]            INT          NOT NULL,
    [ZONE]                 INT          NOT NULL,
    [CUSTOMER_CODE]        VARCHAR (20) NOT NULL,
    [USER_ID]              VARCHAR (20) NOT NULL,
    [JOB_ID]               VARCHAR (20) NOT NULL,
    [REQUEST_TIMESTAMP]    DATETIME     NOT NULL,
    CONSTRAINT [PK_PRO_IMP_TRACE_BKW] PRIMARY KEY CLUSTERED ([CUSTOMER_ID] ASC)
);


GO

