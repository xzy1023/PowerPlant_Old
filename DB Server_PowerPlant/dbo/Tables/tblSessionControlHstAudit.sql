CREATE TABLE [dbo].[tblSessionControlHstAudit] (
    [AuditSeq]            INT            IDENTITY (1, 1) NOT NULL,
    [Action]              VARCHAR (10)   NOT NULL,
    [CreatedBy]           VARCHAR (50)   NOT NULL,
    [CreationDate]        DATETIME       CONSTRAINT [DF_tblSessionControlHstAudit_CreationDate] DEFAULT (getdate()) NOT NULL,
    [RRN]                 INT            NOT NULL,
    [Facility]            NCHAR (3)      NOT NULL,
    [ComputerName]        VARCHAR (50)   NOT NULL,
    [StartTime]           DATETIME       NOT NULL,
    [StopTime]            DATETIME       NULL,
    [DefaultPkgLine]      CHAR (10)      NOT NULL,
    [OverridePkgLine]     CHAR (10)      NULL,
    [ShopOrder]           INT            NULL,
    [ItemNumber]          VARCHAR (35)   NULL,
    [Operator]            VARCHAR (10)   NULL,
    [LogOnTime]           DATETIME       NULL,
    [DefaultShiftNo]      CHAR (1)       NULL,
    [OverrideShiftNo]     CHAR (1)       NULL,
    [CasesScheduled]      INT            NULL,
    [CasesProduced]       INT            NULL,
    [PalletsCreated]      INT            NULL,
    [BagLengthUsed]       DECIMAL (4, 2) NULL,
    [ReworkWgt]           DECIMAL (8, 2) NULL,
    [LooseCases]          INT            NULL,
    [ProductionDate]      DATETIME       NULL,
    [ServerCnnIsOk]       BIT            NULL,
    [CarriedForwardCases] INT            NULL,
    [ShiftProductionDate] DATETIME       NULL,
    [StartDownTime]       DATETIME       NULL,
    [CaseCounter]         INT            CONSTRAINT [DF_tblSessionControlHstAudit_CaseCounter] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_tblSessionControlHstAudit] PRIMARY KEY CLUSTERED ([AuditSeq] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bag Length Used in Inches', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHstAudit', @level2type = N'COLUMN', @level2name = N'BagLengthUsed';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'No. of Cases On Incomplete Pallet', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHstAudit', @level2type = N'COLUMN', @level2name = N'LooseCases';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of the Computer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHstAudit', @level2type = N'COLUMN', @level2name = N'ComputerName';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cases Scheduled for the shift', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHstAudit', @level2type = N'COLUMN', @level2name = N'CasesScheduled';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cases Produced for the shift', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHstAudit', @level2type = N'COLUMN', @level2name = N'CasesProduced';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'No of Pallets Created for the Shift', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHstAudit', @level2type = N'COLUMN', @level2name = N'PalletsCreated';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shop Order No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHstAudit', @level2type = N'COLUMN', @level2name = N'ShopOrder';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shift No Based On Log-on Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHstAudit', @level2type = N'COLUMN', @level2name = N'DefaultShiftNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Entered Shift No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHstAudit', @level2type = N'COLUMN', @level2name = N'OverrideShiftNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shop Order Start Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHstAudit', @level2type = N'COLUMN', @level2name = N'StartTime';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Machined ID Assigned for the Computer ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHstAudit', @level2type = N'COLUMN', @level2name = N'DefaultPkgLine';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Entered Machined ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblSessionControlHstAudit', @level2type = N'COLUMN', @level2name = N'OverridePkgLine';


GO

