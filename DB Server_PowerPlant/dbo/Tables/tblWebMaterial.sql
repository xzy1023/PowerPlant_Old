CREATE TABLE [dbo].[tblWebMaterial] (
    [RRN]          INT          IDENTITY (1, 1) NOT NULL,
    [ItemNumber]   VARCHAR (20) NOT NULL,
    [CoreDiameter] FLOAT (53)   NOT NULL,
    [RollDiameter] FLOAT (53)   NOT NULL,
    [Length]       FLOAT (53)   NOT NULL,
    [Thickness]    FLOAT (53)   NOT NULL,
    [IMPs]         INT          NULL,
    [IMPLength]    FLOAT (53)   NULL,
    [CreatedOn]    DATETIME     NULL,
    [CreatedBy]    VARCHAR (30) NULL,
    CONSTRAINT [PK_tblWebMaterial] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

