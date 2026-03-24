CREATE TABLE [dbo].[tblShift] (
    [Facility]            CHAR (3)     NOT NULL,
    [WorkGroup]           VARCHAR (10) NOT NULL,
    [Shift]               TINYINT      NOT NULL,
    [Description]         VARCHAR (50) NOT NULL,
    [FromTime]            DATETIME     NOT NULL,
    [ToTime]              DATETIME     NOT NULL,
    [ShiftSequence]       INT          NOT NULL,
    [UseSEDateForShiftPD] BIT          NOT NULL,
    [Displayable]         BIT          NOT NULL,
    [ShiftsPerDay]        TINYINT      NOT NULL,
    [ShiftPatternCode]    TINYINT      NOT NULL,
    [Method]              TINYINT      NOT NULL,
    CONSTRAINT [PK_tblShift] PRIMARY KEY CLUSTERED ([Facility] ASC, [WorkGroup] ASC, [Shift] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Use the shift ended date for ShiftProductionDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShift', @level2type = N'COLUMN', @level2name = N'UseSEDateForShiftPD';


GO

