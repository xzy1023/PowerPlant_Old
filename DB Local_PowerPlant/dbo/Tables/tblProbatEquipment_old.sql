CREATE TABLE [dbo].[tblProbatEquipment_old] (
    [RecID]             CHAR (2)        NOT NULL,
    [Facility]          CHAR (3)        NOT NULL,
    [ProbatEqID]        VARCHAR (10)    NOT NULL,
    [Description]       VARCHAR (50)    NOT NULL,
    [Type]              CHAR (1)        NOT NULL,
    [SubType]           CHAR (1)        NOT NULL,
    [GroupID]           INT             NOT NULL,
    [Capacity]          NUMERIC (11, 4) NOT NULL,
    [BPCSMachineID]     VARCHAR (10)    NOT NULL,
    [WorkCenter]        INT             NOT NULL,
    [EquipmentGroupPos] INT             CONSTRAINT [DF_tblProbatEquipment_EquipmentGroup] DEFAULT ((0)) NOT NULL,
    [MachineWholeBean]  VARCHAR (20)    CONSTRAINT [DF_tblProbatEquipment_ProbatMachineWB] DEFAULT ('') NOT NULL,
    [MachineGround]     VARCHAR (20)    CONSTRAINT [DF_tblProbatEquipment_ProbatMachineGD] DEFAULT ('') NOT NULL,
    [MachineFlavor]     VARCHAR (20)    CONSTRAINT [DF_tblProbatEquipment_ProbatMachineFL] DEFAULT ('') NOT NULL
);


GO

CREATE NONCLUSTERED INDEX [IX_tblProbatEquipment]
    ON [dbo].[tblProbatEquipment_old]([Type] ASC, [BPCSMachineID] ASC, [ProbatEqID] ASC, [Facility] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat Machine name - Groud Coffee', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblProbatEquipment_old', @level2type = N'COLUMN', @level2name = N'MachineGround';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat Machine name - Flavor Coffee', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblProbatEquipment_old', @level2type = N'COLUMN', @level2name = N'MachineFlavor';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat Machine name - Whole Bean', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblProbatEquipment_old', @level2type = N'COLUMN', @level2name = N'MachineWholeBean';


GO

