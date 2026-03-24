CREATE TABLE [dbo].[tblQATRejectResultHeader] (
    [RRN]           INT          IDENTITY (1, 1) NOT NULL,
    [BatchID]       DATETIME     NOT NULL,
    [ConfirmedBy]   VARCHAR (10) NULL,
    [Facility]      VARCHAR (3)  NOT NULL,
    [InterfaceID]   VARCHAR (24) NOT NULL,
    [PackagingLine] CHAR (10)    NOT NULL,
    [QAConfirmed]   BIT          CONSTRAINT [DF_tblQATRejectResult_QAConfirmed] DEFAULT ((0)) NULL,
    [RetestNo]      INT          NOT NULL,
    [ShopOrder]     INT          NOT NULL,
    [SOStartTime]   DATETIME     NOT NULL,
    [TestEndTime]   DATETIME     NOT NULL,
    [TestResult]    TINYINT      NULL,
    [TestStartTime] DATETIME     NOT NULL,
    [TesterID]      VARCHAR (10) NULL,
    [QATEntryPoint] CHAR (1)     NULL,
    CONSTRAINT [PK_tblQATRejectResult] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'where 1 = Pass, 0 = Fail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATRejectResultHeader', @level2type = N'COLUMN', @level2name = N'TestResult';


GO

