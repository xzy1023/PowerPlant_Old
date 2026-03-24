CREATE TABLE [dbo].[tblPlantStaff] (
    [ActiveRecord]     BIT          NOT NULL,
    [Facility]         CHAR (3)     NOT NULL,
    [WorkGroup]        VARCHAR (10) NOT NULL,
    [StaffID]          VARCHAR (10) NOT NULL,
    [FirstName]        VARCHAR (50) NOT NULL,
    [LastName]         VARCHAR (50) NOT NULL,
    [StaffClass]       VARCHAR (50) NOT NULL,
    [Password]         VARCHAR (50) NULL,
    [ResetPassword]    BIT          NOT NULL,
    [DateLastChange]   DATETIME     CONSTRAINT [DF_tblPlantStaff_DateLastChange] DEFAULT (getdate()) NOT NULL,
    [WorkSubGroup]     VARCHAR (10) NOT NULL,
    [PrintAccessLevel] INT          CONSTRAINT [DF_tblPlantStaff_PrintAccessLevel] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_tblUserProfile] PRIMARY KEY CLUSTERED ([Facility] ASC, [WorkGroup] ASC, [StaffID] ASC)
);


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tblPlantStaff]
    ON [dbo].[tblPlantStaff]([StaffID] ASC, [Facility] ASC);


GO



-- =====================================================================
-- Author:		Bong Lee
-- Create date: Sep 25, 2006
-- Description:	Flag the Down Load Table List to require to download the
--              tblPlantStaff when the data in the table is changed.
-- =====================================================================
CREATE TRIGGER [tgrPlantStaff]
ON [dbo].[tblPlantStaff]
AFTER INSERT, UPDATE, DELETE 
AS
	Declare @intDeleteCount int
	Declare @intInsertCount int
	Select @intDeleteCount = COUNT(*) FROM deleted;
	Select @intInsertCount = COUNT(*) FROM inserted;
	
	if @intDeleteCount = 1
		update dbo.tblDownLoadTableList set active = 1 where TableName = 'tblPlantStaff' AND 
		Facility = (Select facility from deleted)

	if @intInsertCount = 1
		if update(facility)
			update dbo.tblDownLoadTableList set active = 1 where TableName = 'tblPlantStaff' AND 
			Facility = (Select facility from inserted)

GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Access level to print LPs and Case Labels 1=Everything 2=No reprint', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPlantStaff', @level2type = N'COLUMN', @level2name = N'PrintAccessLevel';


GO

