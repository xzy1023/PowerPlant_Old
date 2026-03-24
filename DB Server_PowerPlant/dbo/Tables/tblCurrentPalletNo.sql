CREATE TABLE [dbo].[tblCurrentPalletNo] (
    [Facility]        CHAR (3) NOT NULL,
    [CurrentPalletNo] INT      NOT NULL,
    [LowestPalletNo]  INT      NOT NULL,
    [HighestPalletNo] INT      NOT NULL,
    CONSTRAINT [PK_tblNextPalletNo] PRIMARY KEY CLUSTERED ([Facility] ASC)
);


GO

