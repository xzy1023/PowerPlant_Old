CREATE TABLE [dbo].[tblQATNote] (
    [NoteID]          INT           NOT NULL,
    [Note]            VARCHAR (512) NOT NULL,
    [Active]          BIT           CONSTRAINT [DF_tblQATNote_Actove] DEFAULT ((1)) NOT NULL,
    [Facility]        VARCHAR (3)   NOT NULL,
    [NoteDescription] VARCHAR (50)  NULL,
    [UpdatedAt]       DATETIME      CONSTRAINT [DF_tblQATNote_UpdatedAt] DEFAULT (getdate()) NULL,
    [UpdatedBy]       VARCHAR (50)  NULL,
    CONSTRAINT [PK_tblQATNote] PRIMARY KEY CLUSTERED ([NoteID] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATNote]
    ON [dbo].[tblQATNote]([NoteID] ASC, [Active] ASC, [Facility] ASC);


GO

