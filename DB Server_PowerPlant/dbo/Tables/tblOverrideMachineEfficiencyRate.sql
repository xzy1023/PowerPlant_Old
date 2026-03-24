CREATE TABLE [dbo].[tblOverrideMachineEfficiencyRate] (
    [Facility]               CHAR (3)       NOT NULL,
    [MachineID]              CHAR (10)      NOT NULL,
    [Active]                 BIT            CONSTRAINT [DF_tblOverrideMachineEfficiencyRate_Active] DEFAULT ((1)) NOT NULL,
    [RateMultiplier]         DECIMAL (7, 4) NOT NULL,
    [LogicForRateMultiplier] VARCHAR (100)  NULL,
    [CreatedBy]              VARCHAR (50)   NULL,
    [CreatedAt]              DATETIME       CONSTRAINT [DF_tblOverrideMachineEfficiencyRate_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [LastUpdated]            DATETIME       CONSTRAINT [DF_tblOverrideMachineEfficiencyRate_LastUpdated] DEFAULT (getdate()) NOT NULL,
    [ItemNumber]             VARCHAR (35)   NOT NULL,
    [RunOperatorsMultiplier] SMALLINT       NOT NULL,
    CONSTRAINT [PK_tblOverrideMachineEfficiencyRate] PRIMARY KEY CLUSTERED ([Facility] ASC, [MachineID] ASC, [Active] ASC, [ItemNumber] ASC)
);


GO

