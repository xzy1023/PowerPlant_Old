CREATE TABLE [dbo].[tblRoaster] (
    [Facility]      CHAR (3)       NOT NULL,
    [RoasterNo]     VARCHAR (10)   CONSTRAINT [DF__tblRoaste__Roast__1367E606] DEFAULT ((0)) NOT NULL,
    [HighTemp]      DECIMAL (4, 1) CONSTRAINT [DF__tblRoaste__HighT__145C0A3F] DEFAULT ((0)) NULL,
    [HighVariation] DECIMAL (4, 1) CONSTRAINT [DF__tblRoaste__HighV__15502E78] DEFAULT ((0)) NULL,
    [LowTemp]       DECIMAL (4, 1) CONSTRAINT [DF__tblRoaste__LowTe__164452B1] DEFAULT ((0)) NULL,
    [LowVariation]  DECIMAL (4, 1) CONSTRAINT [DF__tblRoaste__LowVa__173876EA] DEFAULT ((0)) NULL,
    CONSTRAINT [tblRoaster_PK] PRIMARY KEY NONCLUSTERED ([Facility] ASC, [RoasterNo] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'RecordCount', @value = N'7', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'HighVariation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'RoasterNo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'tblRoaster', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'highest temperature  variation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'DateCreated', @value = N'4/30/2004 2:05:44 PM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'RoasterNo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoaster', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'HighTemp', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'lowest temperature', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'HighVariation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'LastUpdated', @value = N'5/10/2004 9:20:11 AM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'lowest temperature variation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoaster', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoaster', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'LowTemp', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Orientation', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'highest temperature', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'LowVariation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'HighTemp', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Updatable', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'LowTemp', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'OrderByOn', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoaster', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoaster', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'LowVariation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowVariation';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighTemp';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'LowTemp';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoaster', @level2type = N'COLUMN', @level2name = N'HighVariation';


GO

