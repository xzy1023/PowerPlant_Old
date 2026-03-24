CREATE TABLE [dbo].[tblAdHocGrindingSchedule] (
    [Facility]       CHAR (3)      NOT NULL,
    [ScheduleID]     VARCHAR (30)  NOT NULL,
    [Grinder]        VARCHAR (10)  NOT NULL,
    [Blend]          CHAR (6)      NOT NULL,
    [Grind]          CHAR (6)      NOT NULL,
    [ProductionDate] INT           NOT NULL,
    [FromTank]       VARCHAR (10)  NULL,
    [ToBin]          VARCHAR (10)  NOT NULL,
    [EquipmentID]    CHAR (10)     NOT NULL,
    [ScheduledWgt]   INT           NOT NULL,
    [StartDate]      INT           NOT NULL,
    [StartTime]      INT           NOT NULL,
    [EndDate]        INT           NOT NULL,
    [EndTime]        INT           NOT NULL,
    [CreatedBy]      VARCHAR (50)  NOT NULL,
    [CreatedAt]      SMALLDATETIME CONSTRAINT [DF_tblAdHocGrindingSchedule_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]      VARCHAR (50)  NOT NULL,
    [UpdatedAt]      SMALLDATETIME CONSTRAINT [DF_tblAdHocGrindingSchedule_UpdatedAt] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_tblAdHocGrindingSchedule] PRIMARY KEY CLUSTERED ([Facility] ASC, [ScheduleID] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblAdHocGrindingSchedule]
    ON [dbo].[tblAdHocGrindingSchedule]([Facility] ASC, [ProductionDate] ASC, [StartDate] ASC, [StartTime] ASC);


GO

