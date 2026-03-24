CREATE TABLE [dbo].[tblMachinePaidHours] (
    [Facility]            CHAR (3)       NOT NULL,
    [MachineID]           CHAR (10)      NOT NULL,
    [ShopOrder]           INT            NOT NULL,
    [Operator]            VARCHAR (10)   NOT NULL,
    [ShiftProductionDate] SMALLDATETIME  NOT NULL,
    [ShiftNo]             TINYINT        NOT NULL,
    [PaidHours]           DECIMAL (5, 2) NOT NULL,
    [DateCreated]         SMALLDATETIME  CONSTRAINT [DF_tblMachineRunHours_DateCreated] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]           VARCHAR (50)   NOT NULL,
    [DateChanged]         SMALLDATETIME  CONSTRAINT [DF_tblMachineRunHours_DateChanged] DEFAULT (getdate()) NOT NULL,
    [ChangedBy]           VARCHAR (50)   NOT NULL,
    CONSTRAINT [PK_tblMachineRunHours] PRIMARY KEY CLUSTERED ([Facility] ASC, [MachineID] ASC, [ShopOrder] ASC, [Operator] ASC, [ShiftProductionDate] ASC, [ShiftNo] ASC)
);


GO

