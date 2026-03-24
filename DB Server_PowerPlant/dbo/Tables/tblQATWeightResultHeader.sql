CREATE TABLE [dbo].[tblQATWeightResultHeader] (
    [RRN]           INT            IDENTITY (1, 1) NOT NULL,
    [BatchID]       DATETIME       NOT NULL,
    [ConfirmedBy]   VARCHAR (10)   NULL,
    [Facility]      VARCHAR (3)    NOT NULL,
    [InterfaceID]   VARCHAR (24)   NULL,
    [MaxWeight]     DECIMAL (6, 1) NOT NULL,
    [MinWeight]     DECIMAL (6, 1) NOT NULL,
    [OverrideID]    DATETIME       NULL,
    [PackagingLine] CHAR (10)      NOT NULL,
    [QAConfirmed]   BIT            CONSTRAINT [DF_tblQATWeightResultHeader_QAConfirmed] DEFAULT ((0)) NOT NULL,
    [RetestNo]      TINYINT        NOT NULL,
    [ShopOrder]     INT            NOT NULL,
    [SOStartTime]   DATETIME       NOT NULL,
    [TareWeight]    DECIMAL (8, 4) NULL,
    [TargetWeight]  DECIMAL (6, 1) NOT NULL,
    [TestEndTime]   DATETIME       NULL,
    [TestResult]    BIT            NOT NULL,
    [TestStartTime] DATETIME       NOT NULL,
    [TesterID]      VARCHAR (10)   NULL,
    [QATEntryPoint] CHAR (1)       NULL,
    CONSTRAINT [PK_tblQATWeightResultHeader] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATWeightResultHeader_1]
    ON [dbo].[tblQATWeightResultHeader]([InterfaceID] ASC);


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tblQATWeightResultHeader]
    ON [dbo].[tblQATWeightResultHeader]([Facility] ASC, [ShopOrder] ASC, [PackagingLine] ASC, [BatchID] ASC, [RetestNo] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'where 1 = Pass, 0 = Fail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATWeightResultHeader', @level2type = N'COLUMN', @level2name = N'TestResult';


GO

