CREATE TABLE [dbo].[tblSessionControlHst] (
    [RRN]                 INT            IDENTITY (1, 1) NOT NULL,
    [Facility]            NCHAR (3)      NOT NULL,
    [ComputerName]        VARCHAR (50)   NOT NULL,
    [StartTime]           DATETIME       NOT NULL,
    [StopTime]            DATETIME       NOT NULL,
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
    [CaseCounter]         INT            CONSTRAINT [DF_tblSessionControlHst_CaseCounter] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_tblSessionControlHst] PRIMARY KEY CLUSTERED ([DefaultPkgLine] ASC, [StartTime] ASC, [Facility] ASC)
);


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_tblSessionControlHst_1]
    ON [dbo].[tblSessionControlHst]([RRN] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_tblSessionControlHst]
    ON [dbo].[tblSessionControlHst]([StartTime] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_tblSessionControlHst_4]
    ON [dbo].[tblSessionControlHst]([Facility] ASC, [ShiftProductionDate] ASC, [OverrideShiftNo] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_tblSessionControlHst_2]
    ON [dbo].[tblSessionControlHst]([Facility] ASC, [DefaultPkgLine] ASC, [ShopOrder] ASC, [OverrideShiftNo] ASC, [ProductionDate] ASC, [Operator] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'No of Pallets Created for the Shift', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHst', @level2type = N'COLUMN', @level2name = N'PalletsCreated';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bag Length Used in Inches', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHst', @level2type = N'COLUMN', @level2name = N'BagLengthUsed';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'No. of Cases On Incomplete Pallet', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHst', @level2type = N'COLUMN', @level2name = N'LooseCases';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shift No Based On Log-on Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHst', @level2type = N'COLUMN', @level2name = N'DefaultShiftNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Entered Shift No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHst', @level2type = N'COLUMN', @level2name = N'OverrideShiftNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cases Scheduled for the shift', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHst', @level2type = N'COLUMN', @level2name = N'CasesScheduled';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cases Produced for the shift', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHst', @level2type = N'COLUMN', @level2name = N'CasesProduced';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shop Order Start Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHst', @level2type = N'COLUMN', @level2name = N'StartTime';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Machined ID Assigned for the Computer ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHst', @level2type = N'COLUMN', @level2name = N'DefaultPkgLine';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Entered Machined ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHst', @level2type = N'COLUMN', @level2name = N'OverridePkgLine';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shop Order No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHst', @level2type = N'COLUMN', @level2name = N'ShopOrder';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of the Computer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHst', @level2type = N'COLUMN', @level2name = N'ComputerName';


GO

