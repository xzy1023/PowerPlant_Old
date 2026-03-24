CREATE TABLE [dbo].[tblRoastingLog] (
    [Facility]       CHAR (3)       NOT NULL,
    [DateTest]       DATETIME       NOT NULL,
    [Shift]          TINYINT        NULL,
    [BatchNo]        SMALLINT       NOT NULL,
    [RoasterNo]      VARCHAR (10)   CONSTRAINT [DF__tblRoasti__Roast__0CBAE877] DEFAULT ((0)) NOT NULL,
    [Blend]          VARCHAR (6)    CONSTRAINT [DF__tblRoasti__Blend__0DAF0CB0] DEFAULT ((0)) NOT NULL,
    [Colour]         DECIMAL (5, 2) CONSTRAINT [DF__tblRoasti__Colou__0EA330E9] DEFAULT ((0)) NULL,
    [Moisture]       DECIMAL (4, 2) CONSTRAINT [DF__tblRoasti__Moist__0F975522] DEFAULT ((0)) NULL,
    [FinalTemp]      DECIMAL (6, 2) CONSTRAINT [DF__tblRoasti__Final__108B795B] DEFAULT ((0)) NULL,
    [Quench]         DECIMAL (5, 2) CONSTRAINT [DF__tblRoasti__Quenc__117F9D94] DEFAULT ((0)) NULL,
    [RoasterInit]    VARCHAR (50)   NULL,
    [Rejected]       CHAR (1)       NULL,
    [Comments]       VARCHAR (512)  NULL,
    [SpecMin]        DECIMAL (5, 2) CONSTRAINT [DF__tblRoasti__SpecM__1273C1CD] DEFAULT ((0)) NULL,
    [SpecMax]        DECIMAL (5, 2) CONSTRAINT [DF__tblRoasti__SpecM__1367E606] DEFAULT ((0)) NULL,
    [SpecTarget]     DECIMAL (5, 2) CONSTRAINT [DF__tblRoasti__SpecT__145C0A3F] DEFAULT ((0)) NULL,
    [MaxMoisture]    DECIMAL (4, 2) CONSTRAINT [DF__tblRoasti__MaxMo__15502E78] DEFAULT ((0)) NULL,
    [TargetMoisture] DECIMAL (4, 2) CONSTRAINT [DF__tblRoasti__Targe__164452B1] DEFAULT ((0)) NULL,
    [MinMoisture]    DECIMAL (4, 2) CONSTRAINT [DF__tblRoasti__MinMo__173876EA] DEFAULT ((0)) NULL,
    [SpecFinalTemp]  DECIMAL (6, 2) CONSTRAINT [DF__tblRoasti__SpecF__182C9B23] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_tblRoastingLog] PRIMARY KEY CLUSTERED ([Facility] ASC, [DateTest] ASC, [RoasterNo] ASC, [Blend] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblRoastingLog]
    ON [dbo].[tblRoastingLog]([DateTest] ASC, [RoasterNo] ASC, [Blend] ASC, [Rejected] ASC);


GO

CREATE NONCLUSTERED INDEX [Blend]
    ON [dbo].[tblRoastingLog]([Blend] ASC);


GO

CREATE NONCLUSTERED INDEX [RoasterNo]
    ON [dbo].[tblRoastingLog]([RoasterNo] ASC);


GO

CREATE NONCLUSTERED INDEX [DateTest]
    ON [dbo].[tblRoastingLog]([DateTest] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'SpecMax', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DefaultView', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'9', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'TargetMoisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'DateCreated', @value = N'4/26/2004 9:55:19 AM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Colour', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Format', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'DateTest', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'DateTest', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'TargetMoisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Blend', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'14', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'15', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Batch #', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'10', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Format', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Sampling Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Format', @value = N'General Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'2100', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Specification Target', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'MaxMoisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'UnicodeCompression', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Roaster Initial', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Blend', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'SpecFinalTemp', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMEMode', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Comments', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Minimum Moisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'SpecTarget', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Format', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'17', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'RecordCount', @value = N'89140', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'12', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'MaxMoisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Final Temperature', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'10', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'FinalTemp', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'UnicodeCompression', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'OrderByOn', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'SpecFinalTemp', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'SpecMin', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'50', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Format', @value = N'General Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Moisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Format', @value = N'General Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'RoasterNo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'SpecMax', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Quench', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'LastUpdated', @value = N'1/3/2005 11:10:52 AM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Specification Minimum', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Specificatiom Maximum', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Comments', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Moisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Colour', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'FinalTemp', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'13', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Format', @value = N'General Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'MS_IMESentMode', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'MinMoisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'RoasterInit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Quench', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'UnicodeCompression', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Spec. Final Temp.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'RoasterNo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'11', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Format', @value = N'General Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'16', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecFinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'MinMoisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MinMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'RoasterInit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'BatchNo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Target Moisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'SpecMin', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Roaster #', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'DateTest';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'255', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'Required', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'Rejected', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'FinalTemp';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Format', @value = N'General Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'Updatable', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'SpecTarget', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'Rejected', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'SourceField', @value = N'BatchNo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'10', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'Orientation', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Caption', @value = N'Specification Moisture', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DisplayControl', @value = N'109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'SourceTable', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO

EXECUTE sp_addextendedproperty @name = N'Type', @value = N'12', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'7', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'DataUpdatable', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecTarget';


GO

EXECUTE sp_addextendedproperty @name = N'Size', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Colour';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Rejected';


GO

EXECUTE sp_addextendedproperty @name = N'OrdinalPosition', @value = N'8', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterInit';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DecimalPlaces', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'MaxMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'DefaultValue', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Blend';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Quench';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnOrder', @value = N'0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Moisture';


GO

EXECUTE sp_addextendedproperty @name = N'AllowZeroLength', @value = N'True', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'Comments';


GO

EXECUTE sp_addextendedproperty @name = N'Attributes', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'RoasterNo';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnHidden', @value = N'False', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMin';


GO

EXECUTE sp_addextendedproperty @name = N'Name', @value = N'tblRoastingLog', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog';


GO

EXECUTE sp_addextendedproperty @name = N'ColumnWidth', @value = N'-1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'TargetMoisture';


GO

EXECUTE sp_addextendedproperty @name = N'CollatingOrder', @value = N'1033', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblRoastingLog', @level2type = N'COLUMN', @level2name = N'SpecMax';


GO

