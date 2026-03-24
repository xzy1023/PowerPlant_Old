CREATE TABLE [dbo].[tblDarkeningFactor] (
    [EffectiveDate]   DATETIME       CONSTRAINT [DF__tblDarken__Effec__753864A1] DEFAULT (getdate()) NOT NULL,
    [Blend]           CHAR (10)      CONSTRAINT [DF__tblDarken__Blend__762C88DA] DEFAULT ((0)) NOT NULL,
    [Grind]           CHAR (10)      CONSTRAINT [DF__tblDarken__Grind__7720AD13] DEFAULT ((0)) NOT NULL,
    [DarkeningFactor] DECIMAL (4, 2) CONSTRAINT [DF__tblDarken__Darke__7814D14C] DEFAULT ((0)) NULL,
    [CreationDate]    DATETIME       CONSTRAINT [DF__tblDarken__Creat__7908F585] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       VARCHAR (50)   NULL,
    CONSTRAINT [aaaaatblDarkeningFactor_PK] PRIMARY KEY NONCLUSTERED ([EffectiveDate] ASC, [Blend] ASC, [Grind] ASC)
);


GO

CREATE NONCLUSTERED INDEX [EffectiveDate]
    ON [dbo].[tblDarkeningFactor]([EffectiveDate] ASC);


GO

CREATE NONCLUSTERED INDEX [Blend]
    ON [dbo].[tblDarkeningFactor]([Blend] ASC);


GO

-- =====================================================================
-- Author:		Bong Lee
-- Create date: Oct 22, 2013
-- Description:	Flag the Roasting Specifications Update List table to indicate 
--				tblDarkeningFact has been changed and will be required 
--				synchronization with the same table in other server. 
-- =====================================================================
CREATE TRIGGER [dbo].[tgrDarkeningFactor]
   ON  [dbo].[tblDarkeningFactor]
   AFTER INSERT, UPDATE, DELETE
AS 
BEGIN
		UPDATE [dbo].[tblRoastingSpecsUpdList] 
			SET Updated = 1, Lastupdate = getdate()  
			WHERE Facility = '09' AND TableName = 'tblDarkeningFactor'
END

GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Blend', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'CreationDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblDarkeningFactor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'Now()', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Grind', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblDarkeningFactor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'Orientation', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'DateCreated', @value = N'12/29/2005 5:26:23 PM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'EffectiveDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblDarkeningFactor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'DarkeningFactor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblDarkeningFactor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'Now()', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Blend', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'RecordCount', @value = N'558', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'LastUpdated', @value = N'12/29/2005 6:28:25 PM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'CreationDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'tblDarkeningFactor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblDarkeningFactor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Specification Minimum', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'Updatable', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'EffectiveDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Grind', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'OrderByOn', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Grind';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'DarkeningFactor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'DarkeningFactor';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDarkeningFactor', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

