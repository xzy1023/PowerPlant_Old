CREATE TABLE [dbo].[tblGrinderRate] (
    [Facility]  CHAR (3)     NOT NULL,
    [Grinder]   VARCHAR (10) NOT NULL,
    [Grind]     VARCHAR (6)  NOT NULL,
    [GrindRate] SMALLINT     NOT NULL,
    CONSTRAINT [PK_tblGrinderRate_1] PRIMARY KEY CLUSTERED ([Facility] ASC, [Grinder] ASC, [Grind] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Grinder Rate (lbs/hour)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrinderRate', @level2type = N'COLUMN', @level2name = N'GrindRate';


GO

