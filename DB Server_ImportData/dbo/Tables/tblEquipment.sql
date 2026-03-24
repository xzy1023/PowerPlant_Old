CREATE TABLE [dbo].[tblEquipment] (
    [Active]      BIT          NULL,
    [facility]    CHAR (3)     NOT NULL,
    [EquipmentID] CHAR (10)    NOT NULL,
    [Type]        CHAR (1)     NOT NULL,
    [SubType]     CHAR (1)     NOT NULL,
    [Description] VARCHAR (30) NOT NULL
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'P = Pkg line, R = Roaster, G =Grinder, B = Bin, T = Tote, X = Pallet Station
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblEquipment', @level2type = N'COLUMN', @level2name = N'Type';


GO

