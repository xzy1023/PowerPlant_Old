CREATE TABLE [dbo].[tblFinalTempSpec] (
    [Facility]      CHAR (3)       NOT NULL,
    [EffectiveDate] DATETIME       CONSTRAINT [DF__tblFinalT__Effec__2A4B4B5E] DEFAULT (getdate()) NOT NULL,
    [RoasterNo]     VARCHAR (10)   CONSTRAINT [DF__tblFinalT__Roast__2B3F6F97] DEFAULT ((0)) NOT NULL,
    [Blend]         VARCHAR (6)    CONSTRAINT [DF__tblFinalT__Blend__2C3393D0] DEFAULT ((0)) NOT NULL,
    [SpecFinalTemp] DECIMAL (5, 1) CONSTRAINT [DF__tblFinalT__SpecF__2D27B809] DEFAULT ((0)) NOT NULL,
    [CreationDate]  DATETIME       CONSTRAINT [DF__tblFinalT__Creat__2E1BDC42] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     VARCHAR (50)   NULL,
    CONSTRAINT [tblFinalTempSpec_PK] PRIMARY KEY NONCLUSTERED ([Facility] ASC, [EffectiveDate] ASC, [RoasterNo] ASC, [Blend] ASC)
);


GO

CREATE NONCLUSTERED INDEX [Blend]
    ON [dbo].[tblFinalTempSpec]([Blend] ASC);


GO

CREATE NONCLUSTERED INDEX [RoasterNo]
    ON [dbo].[tblFinalTempSpec]([RoasterNo] ASC);


GO

CREATE NONCLUSTERED INDEX [DateCreate]
    ON [dbo].[tblFinalTempSpec]([EffectiveDate] ASC);


GO

-- =====================================================================
-- Author:		Bong Lee
-- Create date: Oct 22, 2013
-- Description:	Flag the Roasting Specifications Update List table to indicate 
--				tblFinalTempSpec has been changed and will be required 
--				synchronization with the same table in other server. 
-- =====================================================================
CREATE TRIGGER [dbo].[tgrFinalTempSpec]
   ON  [dbo].[tblFinalTempSpec]
   AFTER INSERT, UPDATE, DELETE
AS 
BEGIN
	IF (SELECT Facility FROM INSERTED) = '09' OR  (SELECT Facility FROM DELETED) = '09'
	BEGIN
		UPDATE [dbo].[tblRoastingSpecsUpdList] 
			SET Updated = 1, Lastupdate = getdate()  
			WHERE Facility = '09' AND TableName = 'tblFinalTempSpec'
	END 
END

GO

EXECUTE sp_addextendedproperty @name = N'RecordCount', @value = N'3471', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Creator', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'tblFinalTempSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'RoasterNo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'CreationDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'UnicodeCompression', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblFinalTempSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'CreationDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblFinalTempSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'OrderByOn', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'Now()', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'2100', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Creator', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'SpecFinalTemp', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'SpecFinalTemp', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'LastUpdated', @value = N'10/13/2004 12:42:46 PM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblFinalTempSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Blend', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Updatable', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblFinalTempSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'EffectiveDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Blend', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'10', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'1995', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'EffectiveDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblFinalTempSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'DateCreated', @value = N'5/5/2004 1:02:00 PM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Spec. Final Temp', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblFinalTempSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Orientation', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'RoasterNo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'50', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'Now()', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblFinalTempSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

