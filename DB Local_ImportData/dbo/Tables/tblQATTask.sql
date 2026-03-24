CREATE TABLE [dbo].[tblQATTask] (
    [Active]    BIT          CONSTRAINT [DF_tblQATTask_Active] DEFAULT ((1)) NOT NULL,
    [Facility]  VARCHAR (3)  NOT NULL,
    [NoteID]    INT          NULL,
    [QATDefnID] INT          NOT NULL,
    [TaskID]    INT          NOT NULL,
    [TaskSeq]   INT          NOT NULL,
    [UpdatedAt] DATETIME     NULL,
    [UpdatedBy] VARCHAR (50) NULL,
    CONSTRAINT [PK_tblQATTask] PRIMARY KEY CLUSTERED ([Facility] ASC, [QATDefnID] ASC, [TaskID] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATTask]
    ON [dbo].[tblQATTask]([Facility] ASC, [QATDefnID] ASC, [TaskID] ASC, [TaskSeq] ASC, [Active] ASC);


GO

