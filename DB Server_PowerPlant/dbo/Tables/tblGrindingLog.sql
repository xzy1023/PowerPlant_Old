CREATE TABLE [dbo].[tblGrindingLog] (
    [Active]        BIT            CONSTRAINT [DF_tblGrindingLog_Active] DEFAULT ((1)) NOT NULL,
    [GrindJobID]    INT            IDENTITY (1, 1) NOT NULL,
    [Facility]      CHAR (3)       NOT NULL,
    [Grinder]       VARCHAR (10)   NOT NULL,
    [Blend]         VARCHAR (6)    NOT NULL,
    [Grind]         VARCHAR (6)    NOT NULL,
    [EquipmentType] CHAR (10)      NOT NULL,
    [Bin]           VARCHAR (6)    NOT NULL,
    [EquipmentID]   CHAR (10)      NULL,
    [CreatedBy]     VARCHAR (50)   NOT NULL,
    [Shift]         TINYINT        NOT NULL,
    [ActualWgt]     INT            NOT NULL,
    [StartTime]     DATETIME       NULL,
    [EndTime]       DATETIME       NULL,
    [MinColour]     DECIMAL (5, 2) NOT NULL,
    [TargetColour]  DECIMAL (5, 2) NOT NULL,
    [MaxColour]     DECIMAL (5, 2) NOT NULL,
    [Comment]       VARCHAR (255)  NULL,
    [Rejected]      BIT            NOT NULL,
    [RejectedWgt]   INT            NOT NULL,
    [Finalized]     BIT            CONSTRAINT [DF_tblGrindingLog_Finalized] DEFAULT ((0)) NOT NULL,
    [ScheduleID]    VARCHAR (50)   NULL,
    [CreationTime]  DATETIME       CONSTRAINT [DF_tblGrindingLog_CreationTime_1] DEFAULT (getdate()) NULL,
    [UpdatedBy]     VARCHAR (50)   NULL,
    [UpdatedTime]   DATETIME       NULL,
    [Status]        VARCHAR (10)   NULL,
    [TimeStamp]     ROWVERSION     NOT NULL,
    CONSTRAINT [PK_tblGrindingLog] PRIMARY KEY CLUSTERED ([GrindJobID] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblGrindingLog_2]
    ON [dbo].[tblGrindingLog]([Facility] ASC, [Grinder] ASC, [Blend] ASC, [Grind] ASC, [StartTime] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_tblGrindingLog]
    ON [dbo].[tblGrindingLog]([Facility] ASC, [ScheduleID] ASC, [Active] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_tblGrindingLog_1]
    ON [dbo].[tblGrindingLog]([Active] ASC, [Facility] ASC, [Status] ASC);


GO

