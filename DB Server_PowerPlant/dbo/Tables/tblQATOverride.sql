CREATE TABLE [dbo].[tblQATOverride] (
    [RRN]           INT          IDENTITY (1, 1) NOT NULL,
    [BatchID]       DATETIME     NOT NULL,
    [ByPassLanes]   VARCHAR (30) NULL,
    [ByPassTest]    BIT          NOT NULL,
    [Facility]      VARCHAR (3)  NOT NULL,
    [OverridedBy]   VARCHAR (50) NOT NULL,
    [OverrideID]    DATETIME     CONSTRAINT [DF_tblQATOverride_OverrideID] DEFAULT (getdate()) NOT NULL,
    [PackagingLine] CHAR (10)    NOT NULL,
    [QATDefnID]     INT          NOT NULL,
    [ShopOrder]     INT          NOT NULL,
    [SOStartTime]   DATETIME     NOT NULL,
    [Alert]         BIT          CONSTRAINT [DF_tblQATOverride_Alert] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_tblQATOverride] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tblQATOverride_1]
    ON [dbo].[tblQATOverride]([OverrideID] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATOverride]
    ON [dbo].[tblQATOverride]([Facility] ASC, [ShopOrder] ASC, [PackagingLine] ASC);


GO

