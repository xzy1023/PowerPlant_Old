CREATE TABLE [dbo].[tblQATMaterialsResultHeader] (
    [RRN]           INT          IDENTITY (1, 1) NOT NULL,
    [BatchID]       DATETIME     NOT NULL,
    [ConfirmedBy]   VARCHAR (10) NULL,
    [Facility]      VARCHAR (3)  NOT NULL,
    [InterfaceID]   VARCHAR (50) NOT NULL,
    [PackagingLine] CHAR (10)    NOT NULL,
    [QAConfirmed]   BIT          CONSTRAINT [DF_tblQATMaterialsResultHeader_QAConfirmed] DEFAULT ((0)) NULL,
    [ShopOrder]     INT          NOT NULL,
    [SOStartTime]   DATETIME     NOT NULL,
    [TestEndTime]   DATETIME     NOT NULL,
    [TestStartTime] DATETIME     NOT NULL,
    [TesterID]      VARCHAR (10) NULL,
    [QATEntryPoint] CHAR (1)     NULL,
    CONSTRAINT [PK_tblQATMaterialsValidationResultHeader] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tblQATMaterialsResultHeader]
    ON [dbo].[tblQATMaterialsResultHeader]([Facility] ASC, [ShopOrder] ASC, [PackagingLine] ASC, [BatchID] ASC);


GO

