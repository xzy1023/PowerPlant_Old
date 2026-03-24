CREATE TABLE [dbo].[tblQATSpecFormula] (
    [Active]      BIT           NOT NULL,
    [FormulaID]   INT           NOT NULL,
    [Description] VARCHAR (255) NOT NULL,
    [UpdatedAt]   DATETIME      NOT NULL,
    [UpdatedBy]   VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_tblQATSpecFormula] PRIMARY KEY CLUSTERED ([FormulaID] ASC)
);


GO

