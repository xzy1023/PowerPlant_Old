CREATE TABLE [dbo].[tblTopCustomers] (
    [Customer] VARCHAR (50) NOT NULL,
    [Blend]    VARCHAR (6)  CONSTRAINT [DF__tblTopCus__Blend__00551192] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [tblTopCustomers_PK] PRIMARY KEY NONCLUSTERED ([Customer] ASC, [Blend] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Blend', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'RecordCount', @value = N'26', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'Updatable', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblTopCustomers', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'50', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblTopCustomers', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'OrderByOn', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'Orientation', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'LastUpdated', @value = N'11/19/2004 9:48:28 AM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'tblTopCustomers', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Blend', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'10', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'UnicodeCompression', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

EXECUTE sp_addextendedproperty @name = N'DateCreated', @value = N'11/19/2004 9:48:28 AM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Customer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblTopCustomers', @level2type = N'COLUMN', @level2name = N'Customer';


GO

