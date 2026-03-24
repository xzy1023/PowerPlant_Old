CREATE TABLE [dbo].[tblGreenCoffeeLot] (
    [Facility]        CHAR (3)     NOT NULL,
    [GCLotID]         VARCHAR (50) NOT NULL,
    [CreatedBy]       VARCHAR (50) NOT NULL,
    [DateCreated]     DATETIME     CONSTRAINT [DF_tblGreenCoffeeLot_DateCreated] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]       VARCHAR (50) NOT NULL,
    [DateLastUpdated] DATETIME     CONSTRAINT [DF_tblGreenCoffeeLot_DateLastUpdated] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_tblGreenCoffeeLot] PRIMARY KEY CLUSTERED ([Facility] ASC, [GCLotID] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_tblGreenCoffeeLot]
    ON [dbo].[tblGreenCoffeeLot]([GCLotID] ASC);


GO

