CREATE TABLE [dbo].[tblStdMachineEfficiencyRateHst] (
    [Facility]                CHAR (3)       NOT NULL,
    [ItemNumber]              VARCHAR (50)   NOT NULL,
    [WorkCenter]              INT            NOT NULL,
    [MachineID]               CHAR (10)      NOT NULL,
    [MachineHours]            DECIMAL (8, 3) NOT NULL,
    [BasisCode]               CHAR (1)       NOT NULL,
    [StdWorkCenterEfficiency] DECIMAL (5, 4) NOT NULL,
    [RunOperators]            SMALLINT       NULL,
    [EffectiveTime]           DATETIME       NOT NULL,
    CONSTRAINT [PK_tblStdMachineEfficiencyRateHst] PRIMARY KEY CLUSTERED ([Facility] ASC, [ItemNumber] ASC, [WorkCenter] ASC, [MachineID] ASC, [EffectiveTime] ASC)
);


GO

