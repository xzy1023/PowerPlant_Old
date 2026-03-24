CREATE TABLE [dbo].[tblQATMaterialsResultDetail] (
    [RRN]                INT          IDENTITY (1, 1) NOT NULL,
    [BatchID]            DATETIME     NOT NULL,
    [ComponentNo]        VARCHAR (35) NOT NULL,
    [ShopOrder]          INT          NOT NULL,
    [OverrideID]         DATETIME     NULL,
    [ScannedComponentNo] VARCHAR (50) NULL,
    [ScannedLotNo]       VARCHAR (35) NULL,
    [TestResult]         BIT          NOT NULL,
    [TestTime]           DATETIME     NOT NULL,
    CONSTRAINT [PK_tblQATMaterialVerificationResult] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATMaterialsResultDetail]
    ON [dbo].[tblQATMaterialsResultDetail]([BatchID] ASC, [ShopOrder] ASC, [ComponentNo] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'where 1 = Pass, 0 = Fail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATMaterialsResultDetail', @level2type = N'COLUMN', @level2name = N'TestResult';


GO

