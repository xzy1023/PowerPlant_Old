CREATE TABLE [dbo].[tblGrindWeightTolerance] (
    [Active]        BIT            CONSTRAINT [DF_tblGrindWeightTolerance_Active] DEFAULT ((1)) NOT NULL,
    [Facility]      CHAR (3)       NOT NULL,
    [EffectiveDate] DATETIME       CONSTRAINT [DF_tblGrindWeightTolerance_EffectiveDate] DEFAULT (getdate()) NOT NULL,
    [PosTolerance]  DECIMAL (4, 3) NOT NULL,
    [NegTolerance]  DECIMAL (4, 3) NOT NULL,
    [CreationDate]  DATETIME       CONSTRAINT [DF_tblGrindWeightTolerance_CreationDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_tblGrindWeightTolerance] PRIMARY KEY CLUSTERED ([Facility] ASC, [EffectiveDate] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Negative Tolerance in Percent', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindWeightTolerance', @level2type = N'COLUMN', @level2name = N'NegTolerance';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Positive Tolerance in Percent', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindWeightTolerance', @level2type = N'COLUMN', @level2name = N'PosTolerance';


GO

