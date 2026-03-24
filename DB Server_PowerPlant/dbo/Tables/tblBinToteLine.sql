CREATE TABLE [dbo].[tblBinToteLine] (
    [RRN]           INT          IDENTITY (1, 1) NOT NULL,
    [Facility]      CHAR (3)     NOT NULL,
    [EquipmentType] CHAR (1)     NOT NULL,
    [BinTote]       VARCHAR (50) NOT NULL,
    [PackagingLine] CHAR (10)    NOT NULL,
    [MaxCapacity]   SMALLINT     NOT NULL,
    CONSTRAINT [PK_tblBinToteLine] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblBinToteLine]
    ON [dbo].[tblBinToteLine]([Facility] ASC, [EquipmentType] ASC, [BinTote] ASC);


GO

