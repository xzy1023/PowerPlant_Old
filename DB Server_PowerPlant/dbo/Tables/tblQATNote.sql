CREATE TABLE [dbo].[tblQATNote] (
    [NoteID]          INT           IDENTITY (1, 1) NOT NULL,
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


-- =====================================================================
-- Author:		Bong Lee
-- Create date: Oct 15, 2018
-- Description:	Flag the Down Load Table List to require to download the
--              tblQATNote when the data in the table is changed.
-- =====================================================================
CREATE TRIGGER [dbo].[tgrQATNote]
ON [dbo].[tblQATNote]
AFTER INSERT, UPDATE, DELETE 
AS
   UPDATE dbo.tblDownLoadTableList set active = 1 where TableName = 'tblQATNote'

GO

