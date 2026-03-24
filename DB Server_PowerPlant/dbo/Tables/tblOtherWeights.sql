CREATE TABLE [dbo].[tblOtherWeights] (
    [Active]      BIT           CONSTRAINT [DF_tblOtherWeigts_Active] DEFAULT ((1)) NOT NULL,
    [Facility]    CHAR (3)      NOT NULL,
    [ScheduleID]  VARCHAR (30)  NOT NULL,
    [ReworkWgt]   INT           NOT NULL,
    [SpillageWgt] INT           NOT NULL,
    [CreatedBy]   VARCHAR (50)  NOT NULL,
    [CreatedAt]   SMALLDATETIME CONSTRAINT [DF_tblOtherWeights_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]   VARCHAR (50)  NOT NULL,
    [UpdatedAt]   SMALLDATETIME CONSTRAINT [DF_tblOtherWeights_UpdatedAt] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_tblOtherWeights] PRIMARY KEY CLUSTERED ([Facility] ASC, [ScheduleID] ASC)
);


GO

