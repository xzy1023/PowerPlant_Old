CREATE TABLE [dbo].[tblPalletExt] (
    [CreationDateTime] DATETIME      NOT NULL,
    [Creator]          NVARCHAR (50) NULL,
    CONSTRAINT [PK_tblPalletExt] PRIMARY KEY CLUSTERED ([CreationDateTime] ASC)
);


GO

