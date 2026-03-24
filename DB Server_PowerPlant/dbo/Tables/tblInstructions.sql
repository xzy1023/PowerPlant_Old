CREATE TABLE [dbo].[tblInstructions] (
    [Header]       VARCHAR (255) NULL,
    [Instructions] TEXT          NULL,
    [Footer]       VARCHAR (255) NULL,
    [LastUpdated]  DATETIME      NULL
);


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'tblInstructions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'12', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'RecordCount', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblInstructions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Instructions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblInstructions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Footer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblInstructions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

EXECUTE sp_addextendedproperty @name = N'UnicodeCompression', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'Orientation', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblInstructions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'DateCreated', @value = N'5/11/2004 11:46:33 AM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Header', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Footer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'UnicodeCompression', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'Updatable', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Instructions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'LastUpdated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'10', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'LastUpdated', @value = N'4/13/2005 10:54:17 AM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'OrderByOn', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'UnicodeCompression', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Instructions';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'10', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Footer';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Header', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'Header';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'LastUpdated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblInstructions', @level2type = N'COLUMN', @level2name = N'LastUpdated';


GO

