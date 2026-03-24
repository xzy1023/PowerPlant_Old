CREATE TABLE [dbo].[tblQATStartUpResultHeader] (
    [RRN]           INT          IDENTITY (1, 1) NOT NULL,
    [BatchID]       DATETIME     NOT NULL,
    [ByPassAllTest] BIT          NOT NULL,
    [ConfirmedBy]   VARCHAR (10) NULL,
    [Facility]      VARCHAR (3)  NOT NULL,
    [InterfaceID]   VARCHAR (50) NOT NULL,
    [PackagingLine] CHAR (10)    NOT NULL,
    [QAConfirmed]   BIT          CONSTRAINT [DF_tblQATStartUpResultHeader_QAConfirmed] DEFAULT ((0)) NULL,
    [QATEntryPoint] CHAR (1)     NULL,
    [ShopOrder]     INT          NOT NULL,
    [SOStartTime]   DATETIME     NOT NULL,
    [TestEndTime]   DATETIME     NOT NULL,
    [TestStartTime] DATETIME     NOT NULL,
    [TesterID]      VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_tblQATStartUpResultHeader] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATStartUpResultHeader]
    ON [dbo].[tblQATStartUpResultHeader]([Facility] ASC, [ShopOrder] ASC, [PackagingLine] ASC, [BatchID] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATStartUpResultHeader_1]
    ON [dbo].[tblQATStartUpResultHeader]([InterfaceID] ASC);


GO

