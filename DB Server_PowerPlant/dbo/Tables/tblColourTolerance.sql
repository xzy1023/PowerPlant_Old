CREATE TABLE [dbo].[tblColourTolerance] (
    [Facility]  CHAR (3)       NOT NULL,
    [RoasterNo] VARCHAR (10)   CONSTRAINT [DF__tblColour__Roast__4B622666] DEFAULT ((0)) NOT NULL,
    [Blend]     VARCHAR (6)    CONSTRAINT [DF__tblColour__Blend__4C564A9F] DEFAULT ((0)) NOT NULL,
    [Tolerance] DECIMAL (4, 2) CONSTRAINT [DF__tblColour__Toler__4D4A6ED8] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [tblColourTolerance_PK] PRIMARY KEY NONCLUSTERED ([Facility] ASC, [RoasterNo] ASC, [Blend] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblColourTolerance]
    ON [dbo].[tblColourTolerance]([Blend] ASC, [Facility] ASC, [RoasterNo] ASC);


GO

-- =====================================================================
-- Author:		Bong Lee
-- Create date: Oct 22, 2013
-- Description:	Flag the Roasting Specifications Update List table to indicate 
--				tblColourTolerance has been changed and will be required 
--				synchronization with the same table in other server. 
-- =====================================================================
CREATE TRIGGER [dbo].[tgrColourTolerance]
   ON  [dbo].[tblColourTolerance]
   AFTER INSERT, UPDATE, DELETE
AS 
BEGIN
	IF (SELECT Facility FROM INSERTED) = '09' OR  (SELECT Facility FROM DELETED) = '09'
	BEGIN
		UPDATE [dbo].[tblRoastingSpecsUpdList] 
			SET Updated = 1, Lastupdate = getdate()  
			WHERE Facility = '09' AND TableName = 'tblColourTolerance'
	END 
END

GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Tolerance', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'DateCreated', @value = N'4/18/2005 11:12:34 AM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblColourTolerance', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'RoasterNo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblColourTolerance', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'tblColourTolerance', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'LastUpdated', @value = N'4/18/2005 2:09:15 PM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Blend', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'RecordCount', @value = N'820', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblColourTolerance', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Orientation', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'OrderByOn', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'RoasterNo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Tolerance', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Updatable', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Blend', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Acceptable Tolerance', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'Tolerance';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblColourTolerance', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

