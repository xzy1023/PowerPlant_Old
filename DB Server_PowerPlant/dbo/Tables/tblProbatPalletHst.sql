CREATE TABLE [dbo].[tblProbatPalletHst] (
    [CUSTOMER_ID]          INT             NOT NULL,
    [TRANSFERED]           INT             CONSTRAINT [DF_tblProbatPalletHst_TRANSFERED] DEFAULT ((0)) NOT NULL,
    [TRANSFERED_TIMESTAMP] DATETIME        NULL,
    [ACTIVITY]             CHAR (1)        CONSTRAINT [DF_tblProbatPalletHst_ACTIVITY] DEFAULT ('I') NOT NULL,
    [PALLET_ID]            VARCHAR (20)    NOT NULL,
    [CREATE_TIMESTAMP]     DATETIME        NOT NULL,
    [RECEIVING_STATION]    VARCHAR (20)    NOT NULL,
    [CUSTOMER_CODE]        VARCHAR (20)    NOT NULL,
    [AMOUNT]               INT             NOT NULL,
    [ORDER_NAME]           VARCHAR (20)    NOT NULL,
    [ORDER_TYP]            INT             NOT NULL,
    [START_TIMESTAMP]      DATETIME        NOT NULL,
    [ORDER_COMPLETE]       CHAR (1)        NOT NULL,
    [ShopOrder]            INT             NOT NULL,
    [PalletID_PXPAL]       VARCHAR (9)     NOT NULL,
    [QtyOnPallet]          DECIMAL (11, 3) NOT NULL,
    [TransactionSeq]       INT             NOT NULL,
    [CreationDate]         INT             NOT NULL,
    [CreationTime]         INT             NOT NULL,
    [CreatedBy]            VARCHAR (10)    NOT NULL,
    [MaintenanceDate]      INT             NOT NULL,
    [MaintenanceTime]      INT             NOT NULL,
    [MaintainedBy]         VARCHAR (10)    NOT NULL,
    [Facility]             VARCHAR (3)     NOT NULL,
    CONSTRAINT [PK_tblProbatPalletHst] PRIMARY KEY CLUSTERED ([CUSTOMER_ID] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''I'' for insert', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblProbatPalletHst', @level2type = N'COLUMN', @level2name = N'ACTIVITY';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'One decimal place internally', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblProbatPalletHst', @level2type = N'COLUMN', @level2name = N'AMOUNT';


GO

