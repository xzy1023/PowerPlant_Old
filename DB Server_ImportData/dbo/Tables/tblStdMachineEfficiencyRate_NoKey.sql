CREATE TABLE [dbo].[tblStdMachineEfficiencyRate_NoKey] (
    [Facility]                CHAR (3)       NOT NULL,
    [ItemNumber]              VARCHAR (35)   NOT NULL,
    [WorkCenter]              INT            NOT NULL,
    [MachineID]               CHAR (10)      NOT NULL,
    [MachineHours]            DECIMAL (8, 3) NOT NULL,
    [BasisCode]               CHAR (1)       NOT NULL,
    [StdWorkCenterEfficiency] DECIMAL (5, 4) NOT NULL,
    [RunOperators]            SMALLINT       NULL
);


GO

