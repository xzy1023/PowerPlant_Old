CREATE TABLE [dbo].[tblOverFill] (
    [ItemNumber] VARCHAR (35)   NOT NULL,
    [OverFill%]  DECIMAL (4, 2) NOT NULL,
    [Inactive]   BIT            NOT NULL,
    CONSTRAINT [PK_tblOverFill] PRIMARY KEY CLUSTERED ([ItemNumber] ASC)
);


GO

