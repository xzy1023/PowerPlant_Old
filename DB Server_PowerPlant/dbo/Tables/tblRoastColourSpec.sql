CREATE TABLE [dbo].[tblRoastColourSpec] (
    [EffectiveDate] DATETIME       CONSTRAINT [DF__tblRoastC__Effec__25869641] DEFAULT (getdate()) NOT NULL,
    [Blend]         VARCHAR (6)    CONSTRAINT [DF__tblRoastC__Blend__267ABA7A] DEFAULT ((0)) NOT NULL,
    [SpecMin]       DECIMAL (5, 2) CONSTRAINT [DF__tblRoastC__SpecM__276EDEB3] DEFAULT ((0)) NOT NULL,
    [SpecMax]       DECIMAL (5, 2) CONSTRAINT [DF__tblRoastC__SpecM__286302EC] DEFAULT ((0)) NOT NULL,
    [SpecTarg]      DECIMAL (5, 2) CONSTRAINT [DF__tblRoastC__SpecT__29572725] DEFAULT ((0)) NOT NULL,
    [CreationDate]  DATETIME       CONSTRAINT [DF__tblRoastC__Creat__2A4B4B5E] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     VARCHAR (50)   NULL,
    CONSTRAINT [aaaaatblRoastColSpec_PK] PRIMARY KEY NONCLUSTERED ([EffectiveDate] ASC, [Blend] ASC)
);


GO

CREATE NONCLUSTERED INDEX [Blend #]
    ON [dbo].[tblRoastColourSpec]([Blend] ASC);


GO

CREATE NONCLUSTERED INDEX [EffectiveDate]
    ON [dbo].[tblRoastColourSpec]([EffectiveDate] ASC);


GO

-- =====================================================================
-- Author:		Bong Lee
-- Create date: Oct 22, 2013
-- Description:	Flag the Roasting Specifications Update List table to indicate 
--				tblRoastColourSpec has been changed and will be required 
--				synchronization with the same table in other server. 
-- =====================================================================
CREATE TRIGGER [dbo].[tgrRoastColourSpec]
   ON  [dbo].[tblRoastColourSpec]
   AFTER INSERT, UPDATE, DELETE
AS 
BEGIN
	UPDATE [dbo].[tblRoastingSpecsUpdList] 
		SET Updated = 1, Lastupdate = getdate()  
		WHERE Facility = '09' AND TableName = 'tblRoastColourSpec'
END

GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Blend', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'tblRoastColSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'SpecMax', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Format', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'2205', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'CreationDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'OrderByOn', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastColSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'Now()', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastColSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Specificatiom Maximum', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'EffectiveDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Format', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'SpecMin', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Orientation', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'1890', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastColSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Format', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'LastUpdated', @value = N'6/15/2004 9:47:42 AM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'SpecMax', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'CreationDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'2145', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'SpecMin', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'2265', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Blend', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'EffectiveDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastColSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'DateCreated', @value = N'4/19/2004 5:45:21 PM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'Now()', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastColSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'RecordCount', @value = N'228', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastColSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'Updatable', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'SpecTarg', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Specification Target', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'SpecTarg', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecTarg';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Specification Minimum', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastColourSpec', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

