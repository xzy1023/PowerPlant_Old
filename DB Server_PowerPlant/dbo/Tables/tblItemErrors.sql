CREATE TABLE [dbo].[tblItemErrors] (
    [Facility]                VARCHAR (3)  NOT NULL,
    [ItemNumber]              VARCHAR (35) NOT NULL,
    [ItemNotFound]            BIT          NOT NULL,
    [QtyPerPallet]            BIT          NOT NULL,
    [LabelDateFmtCode]        BIT          NOT NULL,
    [LabelImageName]          BIT          NOT NULL,
    [BagLength]               BIT          NOT NULL,
    [ProductionShelfLifeDays] BIT          NOT NULL,
    [DateCreated]             DATETIME     CONSTRAINT [DF_tblItemErrors_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]             DATETIME     CONSTRAINT [DF_tblItemErrors_DateUpdated] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_tblItemErrors] PRIMARY KEY CLUSTERED ([Facility] ASC, [ItemNumber] ASC)
);


GO

