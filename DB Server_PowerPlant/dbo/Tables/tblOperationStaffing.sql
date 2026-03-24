CREATE TABLE [dbo].[tblOperationStaffing] (
    [RRN]           INT          IDENTITY (1, 1) NOT NULL,
    [facility]      CHAR (3)     NOT NULL,
    [PackagingLine] CHAR (10)    NOT NULL,
    [StartTime]     DATETIME     NOT NULL,
    [StaffID]       VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_tblOperationStaffing] PRIMARY KEY NONCLUSTERED ([RRN] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblOperationStaffing_1]
    ON [dbo].[tblOperationStaffing]([facility] ASC, [PackagingLine] ASC, [StartTime] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packaging Line Assigned for the Computer ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblOperationStaffing', @level2type = N'COLUMN', @level2name = N'PackagingLine';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shop Order Start Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblOperationStaffing', @level2type = N'COLUMN', @level2name = N'StartTime';


GO

