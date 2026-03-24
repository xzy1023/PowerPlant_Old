CREATE TABLE [dbo].[tblSessionControl] (
    [Facility]            CHAR (3)       NOT NULL,
    [ComputerName]        VARCHAR (50)   NOT NULL,
    [StartTime]           DATETIME       NULL,
    [StopTime]            DATETIME       NULL,
    [DefaultPkgLine]      CHAR (10)      NOT NULL,
    [OverridePkgLine]     CHAR (10)      NULL,
    [ShopOrder]           INT            NULL,
    [ItemNumber]          VARCHAR (35)   NULL,
    [Operator]            VARCHAR (10)   NOT NULL,
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

