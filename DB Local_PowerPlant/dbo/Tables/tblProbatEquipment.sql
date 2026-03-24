CREATE TABLE [dbo].[tblProbatEquipment] (
    [Status]            BIT          CONSTRAINT [DF_tblProbatEquipment_Status] DEFAULT ((1)) NOT NULL,
    [Facility]          CHAR (3)     NOT NULL,
    [ProbatEqID]        VARCHAR (10) NOT NULL,
    [Type]              CHAR (1)     NOT NULL,
    [GroupID]           INT          NOT NULL,
    [MachineID]         VARCHAR (10) NOT NULL,
    [EquipmentGroupPos] INT          CONSTRAINT [DF_tblProbatEquipment_EquipmentGroupPos] DEFAULT ((0)) NOT NULL,
    [MachineWholeBean]  VARCHAR (20) NOT NULL,
    [MachineGround]     VARCHAR (20) NOT NULL,
    [MachineFlavor]     VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_tblProbatEquipment] PRIMARY KEY CLUSTERED ([Facility] ASC, [ProbatEqID] ASC, [Type] ASC, [MachineID] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat Machine name - Flavor Coffee', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblProbatEquipment', @level2type = N'COLUMN', @level2name = N'MachineFlavor';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat Machine name - Whole Bean', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblProbatEquipment', @level2type = N'COLUMN', @level2name = N'MachineWholeBean';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Probat Machine name - Groud Coffee', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblProbatEquipment', @level2type = N'COLUMN', @level2name = N'MachineGround';


GO

