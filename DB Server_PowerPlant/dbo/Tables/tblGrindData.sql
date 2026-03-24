CREATE TABLE [dbo].[tblGrindData] (
    [RelativeRecNo] INT            IDENTITY (1, 1) NOT NULL,
    [GrindJobID]    INT            NOT NULL,
    [DateTest]      DATETIME       CONSTRAINT [DF_tblGrindData_DateTest] DEFAULT (getdate()) NOT NULL,
    [Colour]        DECIMAL (5, 2) NOT NULL,
    [Operator]      VARCHAR (50)   NOT NULL,
    [Shift]         TINYINT        NULL,
    [Inactive]      BIT            NOT NULL,
    CONSTRAINT [PK_tblGrindData] PRIMARY KEY CLUSTERED ([RelativeRecNo] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblGrindData]
    ON [dbo].[tblGrindData]([GrindJobID] ASC, [DateTest] ASC, [Inactive] ASC);


GO

