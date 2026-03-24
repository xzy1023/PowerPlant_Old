CREATE TABLE [dbo].[tblEquipment] (
    [Active]                 BIT          CONSTRAINT [DF_tblEquipment_Active] DEFAULT ((1)) NOT NULL,
    [facility]               CHAR (3)     NOT NULL,
    [EquipmentID]            CHAR (10)    NOT NULL,
    [Type]                   CHAR (1)     NOT NULL,
    [SubType]                CHAR (1)     NULL,
    [Description]            VARCHAR (30) NULL,
    [GroupID]                VARCHAR (10) NULL,
    [ProbatID]               VARCHAR (10) NULL,
    [IPCSharedGroup]         TINYINT      NULL,
    [WorkCenter]             VARCHAR (10) NULL,
    [EnableDownTimeDuration] BIT          NULL,
    CONSTRAINT [PK_tblMachine] PRIMARY KEY CLUSTERED ([Type] ASC, [EquipmentID] ASC, [facility] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'P = Pkg line, R = Roaster, G =Grinder, B = Bin, T = Tote, X = Pallet Station
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblEquipment', @level2type = N'COLUMN', @level2name = N'Type';


GO

