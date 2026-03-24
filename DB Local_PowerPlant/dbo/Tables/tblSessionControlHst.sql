CREATE TABLE [dbo].[tblSessionControlHst] (
    [RRN]                 INT            IDENTITY (1, 1) NOT NULL,
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
    [CaseCounter]         INT            NULL
);


GO

CREATE NONCLUSTERED INDEX [IX_tblSessionControlHst_2]
    ON [dbo].[tblSessionControlHst]([Facility] ASC, [DefaultPkgLine] ASC, [ShopOrder] ASC, [OverrideShiftNo] ASC, [ProductionDate] ASC, [Operator] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_tblSessionControlHst_3]
    ON [dbo].[tblSessionControlHst]([Facility] ASC, [OverrideShiftNo] ASC, [ShiftProductionDate] ASC);


GO

