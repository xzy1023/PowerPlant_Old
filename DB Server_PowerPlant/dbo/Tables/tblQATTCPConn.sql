CREATE TABLE [dbo].[tblQATTCPConn] (
    [Active]      BIT           CONSTRAINT [DF_tblQATTCPConn_Active] DEFAULT ((1)) NOT NULL,
    [Command1]    VARCHAR (50)  NULL,
    [Command2]    VARCHAR (50)  NULL,
    [Command3]    VARCHAR (50)  NULL,
    [Description] VARCHAR (100) NULL,
    [Facility]    VARCHAR (3)   NOT NULL,
    [IPAddress]   VARCHAR (50)  NOT NULL,
    [Model]       VARCHAR (50)  NULL,
    [Port]        INT           NOT NULL,
    [TCPConnID]   INT           IDENTITY (1, 1) NOT NULL,
    [UpdatedAt]   DATETIME      CONSTRAINT [DF_tblQATTCPConn_UpdatedAt] DEFAULT (getdate()) NULL,
    [UpdatedBy]   VARCHAR (50)  NULL,
    CONSTRAINT [PK_tblQATTCPConn_1] PRIMARY KEY CLUSTERED ([TCPConnID] ASC)
);


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tblQATTCPConn]
    ON [dbo].[tblQATTCPConn]([IPAddress] ASC, [Active] ASC, [Facility] ASC, [Port] ASC);


GO


-- =====================================================================
-- Author:		Bong Lee
-- Create date: Oct 15, 2018
-- Description:	Flag the Down Load Table List to require to download the
--              tblQATTCPConn when the data in the table is changed.
-- =====================================================================
CREATE TRIGGER [dbo].[tgrQATTCPConn]
ON [dbo].[tblQATTCPConn]
AFTER INSERT, UPDATE, DELETE 
AS
   UPDATE dbo.tblDownLoadTableList set active = 1 where TableName = 'tblQATTCPConn'

GO

