CREATE TABLE [dbo].[tblQATStartUpResultDetail] (
    [RRN]           INT         IDENTITY (1, 1) NOT NULL,
    [BatchID]       DATETIME    NOT NULL,
    [Facility]      VARCHAR (3) NOT NULL,
    [PackagingLine] CHAR (10)   NOT NULL,
    [ShopOrder]     INT         NOT NULL,
    [TaskEndTime]   DATETIME    NOT NULL,
    [TaskID]        INT         NOT NULL,
    [TaskStartTime] DATETIME    NOT NULL,
    [TaskStatus]    TINYINT     NOT NULL,
    CONSTRAINT [PK_tblQATStartUpResult_1] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tblQATStartUpResultDetail]
    ON [dbo].[tblQATStartUpResultDetail]([Facility] ASC, [ShopOrder] ASC, [PackagingLine] ASC, [BatchID] ASC, [TaskID] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'where 0 = N/A; 1 = Done ; 2 = not Done', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATStartUpResultDetail', @level2type = N'COLUMN', @level2name = N'TaskStatus';


GO

