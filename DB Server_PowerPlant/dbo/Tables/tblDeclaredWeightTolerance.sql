CREATE TABLE [dbo].[tblDeclaredWeightTolerance] (
    [DeclaredNetQuantity] BIGINT         NOT NULL,
    [Percentage]          BIT            NOT NULL,
    [Tolerance]           DECIMAL (6, 2) NOT NULL,
    [MPPercentage]        BIT            NOT NULL,
    [MPTolerance]         DECIMAL (6, 2) NOT NULL,
    [DateLastChange]      DATETIME       CONSTRAINT [DF_tblDeclaredWeightolerance_DateLastChange] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_tblDeclaredWeightolerance] PRIMARY KEY CLUSTERED ([DeclaredNetQuantity] ASC)
);


GO

