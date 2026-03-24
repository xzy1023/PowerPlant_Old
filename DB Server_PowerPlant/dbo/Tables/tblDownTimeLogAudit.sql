CREATE TABLE [dbo].[tblDownTimeLogAudit] (
    [AuditSeq]            INT           IDENTITY (1, 1) NOT NULL,
    [Action]              VARCHAR (10)  NOT NULL,
    [AuditCreatedBy]      VARCHAR (50)  NOT NULL,
    [AuditCreationDate]   DATETIME      CONSTRAINT [DF_tblDownTimeLogAudit_CreationDate] DEFAULT (getdate()) NOT NULL,
    [RRN]                 INT           NOT NULL,
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
    [CreationDate]        DATETIME      NOT NULL,
    [UpdatedBy]           VARCHAR (10)  NOT NULL,
    [LastUpdated]         DATETIME      NOT NULL,
    [ShiftProductionDate] DATETIME      NULL,
    [EventID]             VARCHAR (50)  NULL,
    CONSTRAINT [PK_tblDownTimeLogAudit_1] PRIMARY KEY CLUSTERED ([AuditSeq] ASC)
);


GO

