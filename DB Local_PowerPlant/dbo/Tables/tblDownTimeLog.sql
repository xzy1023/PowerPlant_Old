CREATE TABLE [dbo].[tblDownTimeLog] (
    [RRN]                 INT           IDENTITY (1, 1) NOT NULL,
    [InActive]            BIT           NOT NULL,
    [Facility]            CHAR (3)      NOT NULL,
    [ShopOrder]           INT           NOT NULL,
    [MachineType]         VARCHAR (3)   NOT NULL,
    [MachineSubType]      VARCHAR (10)  NOT NULL,
    [MachineID]           CHAR (10)     NOT NULL,
    [Operator]            VARCHAR (10)  NOT NULL,
    [Technician]          VARCHAR (10)  NOT NULL,
    [DownTimeBegin]       DATETIME      NOT NULL,
    [DownTimeEnd]         DATETIME      NOT NULL,
    [Shift]               TINYINT       NOT NULL,
    [ReasonType]          SMALLINT      NOT NULL,
    [ReasonCode]          SMALLINT      NOT NULL,
    [Comment]             VARCHAR (255) NULL,
    [CreatedBy]           VARCHAR (10)  NOT NULL,
    [CreationDate]        DATETIME      CONSTRAINT [DF_tblDownTimeLog_CreationDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]           VARCHAR (10)  NOT NULL,
    [LastUpdated]         DATETIME      NOT NULL,
    [ShiftProductionDate] DATETIME      NULL,
    [EventID]             VARCHAR (50)  NULL,
    CONSTRAINT [PK_tblDownTimeLog] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblDownTimeLog]
    ON [dbo].[tblDownTimeLog]([MachineType] ASC, [MachineSubType] ASC, [MachineID] ASC, [DownTimeBegin] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_tblDownTimeLog_1]
    ON [dbo].[tblDownTimeLog]([ShiftProductionDate] ASC, [Shift] ASC, [MachineID] ASC, [Operator] ASC, [Facility] ASC);


GO

