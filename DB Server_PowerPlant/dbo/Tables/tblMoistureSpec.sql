CREATE TABLE [dbo].[tblMoistureSpec] (
    [EffectiveDate]  DATETIME       CONSTRAINT [DF__tblMoistu__Effec__21B6055D] DEFAULT (getdate()) NOT NULL,
    [Blend]          VARCHAR (6)    CONSTRAINT [DF__tblMoistu__Blend__22AA2996] DEFAULT ((0)) NOT NULL,
    [MaxMoisture]    DECIMAL (3, 2) CONSTRAINT [DF__tblMoistu__MaxMo__239E4DCF] DEFAULT ((0)) NOT NULL,
    [TargetMoisture] DECIMAL (3, 2) CONSTRAINT [DF__tblMoistu__Targe__24927208] DEFAULT ((0)) NOT NULL,
    [MinMoisture]    DECIMAL (3, 2) CONSTRAINT [DF__tblMoistu__MinMo__25869641] DEFAULT ((0)) NOT NULL,
    [CreationDate]   DATETIME       CONSTRAINT [DF__tblMoistu__Creat__267ABA7A] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      VARCHAR (50)   NULL,
    CONSTRAINT [tblMoistureSpec_PK] PRIMARY KEY NONCLUSTERED ([EffectiveDate] ASC, [Blend] ASC)
);


GO

CREATE NONCLUSTERED INDEX [Blend #]
    ON [dbo].[tblMoistureSpec]([Blend] ASC);


GO

CREATE NONCLUSTERED INDEX [CreationDate]
    ON [dbo].[tblMoistureSpec]([EffectiveDate] ASC);


GO

-- =====================================================================
-- Author:		Bong Lee
-- Create date: Oct 22, 2013
-- Description:	Flag the Roasting Specifications Update List table to indicate 
--				tblMoistureSpec has been changed and will be required 
--				synchronization with the same table in other server. 
-- =====================================================================
CREATE TRIGGER [dbo].[tgrMoistureSpec]
   ON  [dbo].[tblMoistureSpec]
   AFTER INSERT, UPDATE, DELETE
AS 
BEGIN
	UPDATE [dbo].[tblRoastingSpecsUpdList] 
		SET Updated = 1, Lastupdate = getdate()  
		WHERE Facility = '09' AND TableName = 'tblMoistureSpec'
END

GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblMoistureSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Updatable', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'MinMoisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'EffectiveDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblMoistureSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'MinMoisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'RecordCount', @value = N'293', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec';


GO

EXECUTE sp_addextendedproperty @name = N'LastUpdated', @value = N'1/3/2005 11:09:29 AM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblMoistureSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'MaxMoisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'TargetMoisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'Now()', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'CreationDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'TargetMoisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'CreationDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Orientation', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblMoistureSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'DateCreated', @value = N'5/6/2004 9:49:09 AM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'EffectiveDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Blend', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'tblMoistureSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Blend', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'MaxMoisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'EffectiveDate';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblMoistureSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'Now()', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

EXECUTE sp_addextendedproperty @name = N'OrderByOn', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblMoistureSpec', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblMoistureSpec', @level2type = N'COLUMN', @level2name = N'CreationDate';


GO

