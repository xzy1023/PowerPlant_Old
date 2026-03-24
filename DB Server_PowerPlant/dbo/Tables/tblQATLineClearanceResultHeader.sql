CREATE TABLE [dbo].[tblQATLineClearanceResultHeader] (
    [RRN]           INT          IDENTITY (1, 1) NOT NULL,
    [BatchID]       DATETIME     NOT NULL,
    [ByPassAllTest] BIT          NOT NULL,
    [ConfirmedBy]   VARCHAR (10) NULL,
    [Facility]      VARCHAR (3)  NOT NULL,
    [InterfaceID]   VARCHAR (50) NOT NULL,
    [PackagingLine] CHAR (10)    NOT NULL,
    [QAConfirmed]   BIT          CONSTRAINT [DF_tblQATLineClearanceResultHeader_QAConfirmed] DEFAULT ((0)) NULL,
    [ShopOrder]     INT          NOT NULL,
    [SOStartTime]   DATETIME     NOT NULL,
    [TestEndTime]   DATETIME     NOT NULL,
    [TestStartTime] DATETIME     NOT NULL,
    [TesterID]      VARCHAR (10) NULL,
    [QATEntryPoint] CHAR (1)     NULL,
    CONSTRAINT [PK_tblQATLineClearanceResultHeader] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATLineClearanceResultHeader_1]
    ON [dbo].[tblQATLineClearanceResultHeader]([InterfaceID] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATLineClearanceResultHeader]
    ON [dbo].[tblQATLineClearanceResultHeader]([Facility] ASC, [ShopOrder] ASC, [PackagingLine] ASC, [BatchID] ASC);


GO

