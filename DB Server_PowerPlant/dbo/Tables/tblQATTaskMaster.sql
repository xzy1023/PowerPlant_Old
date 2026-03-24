CREATE TABLE [dbo].[tblQATTaskMaster] (
    [Active]          BIT          CONSTRAINT [DF_tblQATTaskMaster_Active] DEFAULT ((1)) NOT NULL,
    [Facility]        VARCHAR (3)  NOT NULL,
    [TaskID]          INT          IDENTITY (1, 1) NOT NULL,
    [TaskDescription] VARCHAR (50) NOT NULL,
    [UpdatedAt]       DATETIME     NULL,
    [UpdatedBy]       VARCHAR (50) NULL,
    CONSTRAINT [PK_tblQATTaskMaster] PRIMARY KEY CLUSTERED ([TaskID] ASC)
);


GO


-- =====================================================================
-- Author:		Bong Lee
-- Create date: Oct 15, 2018
-- Description:	Flag the Down Load Table List to require to download the
--              tblQATTaskMaster when the data in the table is changed.
-- =====================================================================
CREATE TRIGGER [dbo].[tgrQATTaskMaster]
ON [dbo].[tblQATTaskMaster]
AFTER INSERT, UPDATE, DELETE 
AS
   UPDATE dbo.tblDownLoadTableList set active = 1 where TableName = 'tblQATTaskMaster'

GO

