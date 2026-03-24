CREATE TABLE [dbo].[tblQATSerialConn] (
    [SerialConnID]   INT          NOT NULL,
    [Active]         BIT          CONSTRAINT [DF_tblQATSerialConn_Active] DEFAULT ((1)) NOT NULL,
    [Facility]       VARCHAR (3)  NOT NULL,
    [BaudRate]       INT          NOT NULL,
    [ComPort]        TINYINT      NOT NULL,
    [DataBits]       TINYINT      NOT NULL,
    [Parity]         TINYINT      NOT NULL,
    [SerialConnDesc] VARCHAR (50) NULL,
    [StopBits]       TINYINT      NOT NULL,
    [UpdatedAt]      DATETIME     CONSTRAINT [DF_tblQATSerialConn_UpdatedAt] DEFAULT (getdate()) NULL,
    [UpdatedBy]      VARCHAR (50) NULL,
    CONSTRAINT [PK_tblQATSerialConn_1] PRIMARY KEY CLUSTERED ([SerialConnID] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblQATSerialConn]
    ON [dbo].[tblQATSerialConn]([Facility] ASC, [Active] ASC, [SerialConnID] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 = None; 1 = Odd; 2 = Even, 3 = Mark;; 4 = Space', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATSerialConn', @level2type = N'COLUMN', @level2name = N'Parity';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0= None; 1 = 1; 2 = 2; 3 =1.5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblQATSerialConn', @level2type = N'COLUMN', @level2name = N'StopBits';


GO

