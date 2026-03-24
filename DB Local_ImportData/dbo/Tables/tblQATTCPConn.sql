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
    [TCPConnID]   INT           NOT NULL,
    [UpdatedAt]   DATETIME      CONSTRAINT [DF_tblQATTCPConn_UpdatedAt] DEFAULT (getdate()) NULL,
    [UpdatedBy]   VARCHAR (50)  NULL,
    CONSTRAINT [PK_tblQATTCPConn_1] PRIMARY KEY CLUSTERED ([TCPConnID] ASC)
);


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tblQATTCPConn]
    ON [dbo].[tblQATTCPConn]([IPAddress] ASC, [Active] ASC, [Facility] ASC, [Port] ASC);


GO

