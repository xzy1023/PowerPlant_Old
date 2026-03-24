CREATE TABLE [dbo].[tblGrindingSchedule] (
    [GSCFAC]   VARCHAR (3)  NOT NULL,
    [GSCWHS]   VARCHAR (3)  NOT NULL,
    [GSCGRNDR] VARCHAR (10) NOT NULL,
    [GSCMACH]  VARCHAR (10) NOT NULL,
    [GSCACTI]  VARCHAR (24) NOT NULL,
    [GSCPROD]  VARCHAR (35) NOT NULL,
    [GSCBLEND] VARCHAR (6)  NOT NULL,
    [GSCGRIND] VARCHAR (6)  NOT NULL,
    [GSCQTYI]  INT          NOT NULL,
    [GSCQTYO]  INT          NOT NULL,
    [GSCSDTE]  INT          NOT NULL,
    [GSCSTIM]  INT          NOT NULL,
    [GSCEDTE]  INT          NOT NULL,
    [GSCETIM]  INT          NOT NULL,
    [GSCISDTE] INT          NOT NULL,
    [GSCISTIM] INT          NOT NULL,
    [GSCIEDTE] INT          NOT NULL,
    [GSCIETIM] INT          NOT NULL,
    [GSCFBIN]  VARCHAR (10) NOT NULL,
    [GSCTBIN]  VARCHAR (10) NOT NULL,
    [GSCPKLIN] CHAR (10)    NOT NULL,
    [GSCSORD]  INT          NOT NULL,
    [GSCXDTE]  INT          NOT NULL,
    [GSCXTIM]  INT          NOT NULL,
    [GSCPDTE]  INT          NOT NULL,
    [GSCSFT#]  INT          NOT NULL
);


GO

CREATE NONCLUSTERED INDEX [IX_tblGrindingSchedule]
    ON [dbo].[tblGrindingSchedule]([GSCFAC] ASC, [GSCPDTE] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Start Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCSTIM';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Grinder ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCGRNDR';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Blend', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCBLEND';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shift #', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCSFT#';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Schedule Export Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCXTIM';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Input Start Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCISTIM';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Input End Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCIETIM';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Output Bin', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCTBIN';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shop Order #', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCSORD';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Input Quantity', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCQTYI';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'End Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCETIM';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Item Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCPROD';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity ID (i.e. Schedule ID)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCACTI';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Grind', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCGRIND';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Warehouse', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCWHS';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Start Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCSDTE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Input Start Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCISDTE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Output Quantity', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCQTYO';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BPCS Machine ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCMACH';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Input End Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCIEDTE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Production Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCPDTE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Input Bin', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCFBIN';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'End Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCEDTE';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packaging Line', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCPKLIN';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Facility', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCFAC';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Schedule Export Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblGrindingSchedule', @level2type = N'COLUMN', @level2name = N'GSCXDTE';


GO

