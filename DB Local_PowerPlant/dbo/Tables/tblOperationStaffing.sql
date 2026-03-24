CREATE TABLE [dbo].[tblOperationStaffing] (
    [RRN]           INT          IDENTITY (1, 1) NOT NULL,
    [facility]      CHAR (3)     NOT NULL,
    [PackagingLine] CHAR (10)    NOT NULL,
    [StartTime]     DATETIME     NOT NULL,
    [StaffID]       VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_tblOperationStaffing] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

