CREATE TABLE [dbo].[tblComponentScrapAudit] (
    [AuditSeq]     INT            IDENTITY (1, 1) NOT NULL,
    [Action]       CHAR (10)      NOT NULL,
    [CreatedBy]    VARCHAR (50)   NOT NULL,
    [CreationDate] DATETIME       NOT NULL,
    [Facility]     CHAR (3)       NOT NULL,
    [ShopOrder]    INT            NOT NULL,
    [StartTime]    DATETIME       NOT NULL,
    [Component]    VARCHAR (35)   NOT NULL,
    [Quantity]     DECIMAL (8, 2) NOT NULL,
    CONSTRAINT [PK_tblComponentScrapAudit] PRIMARY KEY CLUSTERED ([AuditSeq] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shop Order Start Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblComponentScrapAudit', @level2type = N'COLUMN', @level2name = N'StartTime';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblComponentScrapAudit', @level2type = N'COLUMN', @level2name = N'ShopOrder';


GO

