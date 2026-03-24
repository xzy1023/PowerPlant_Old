CREATE TABLE [dbo].[tblSessionControl] (
    [Facility]            CHAR (3)       NOT NULL,
    [ComputerName]        VARCHAR (50)   NOT NULL,
    [StartTime]           DATETIME       NULL,
    [StopTime]            DATETIME       NULL,
    [DefaultPkgLine]      CHAR (10)      NOT NULL,
    [OverridePkgLine]     CHAR (10)      NOT NULL,
    [ShopOrder]           INT            NOT NULL,
    [ItemNumber]          VARCHAR (35)   NOT NULL,
    [Operator]            VARCHAR (10)   NOT NULL,
    [LogOnTime]           DATETIME       NOT NULL,
    [DefaultShiftNo]      CHAR (1)       NOT NULL,
    [OverrideShiftNo]     CHAR (1)       NOT NULL,
    [CasesScheduled]      INT            NOT NULL,
    [CasesProduced]       INT            NOT NULL,
    [PalletsCreated]      INT            NOT NULL,
    [BagLengthUsed]       DECIMAL (4, 2) NOT NULL,
    [ReworkWgt]           DECIMAL (8, 2) NOT NULL,
    [LooseCases]          INT            NOT NULL,
    [ProductionDate]      DATETIME       NOT NULL,
    [ServerCnnIsOk]       BIT            NULL,
    [CarriedForwardCases] INT            NULL,
    [ShiftProductionDate] DATETIME       NULL,
    [StartDownTime]       DATETIME       NULL,
    [CaseCounter]         INT            NULL
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packaging Line Assigned for the Computer ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControl', @level2type = N'COLUMN', @level2name = N'DefaultPkgLine';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shop Order Start Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControl', @level2type = N'COLUMN', @level2name = N'StartTime';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Entered Packaging Line', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControl', @level2type = N'COLUMN', @level2name = N'OverridePkgLine';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shift No Based On Log-on Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControl', @level2type = N'COLUMN', @level2name = N'DefaultShiftNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shop Order No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControl', @level2type = N'COLUMN', @level2name = N'ShopOrder';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cases Scheduled for the shift', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControl', @level2type = N'COLUMN', @level2name = N'CasesScheduled';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Entered Shift No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControl', @level2type = N'COLUMN', @level2name = N'OverrideShiftNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of the Computer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControl', @level2type = N'COLUMN', @level2name = N'ComputerName';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cases Produced for the shift', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControl', @level2type = N'COLUMN', @level2name = N'CasesProduced';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'No of Pallets Created for the Shift', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControl', @level2type = N'COLUMN', @level2name = N'PalletsCreated';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bag Length Used in Inches', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControl', @level2type = N'COLUMN', @level2name = N'BagLengthUsed';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'No. of Cases On Incomplete Pallet', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControl', @level2type = N'COLUMN', @level2name = N'LooseCases';


GO

