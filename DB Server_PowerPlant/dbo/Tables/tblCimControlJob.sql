CREATE TABLE [dbo].[tblCimControlJob] (
    [RRN]                  INT          IDENTITY (1, 1) NOT NULL,
    [TimeSubmit]           DATETIME     CONSTRAINT [DF_tblCimControlJob_TimeSubmit] DEFAULT (getdate()) NOT NULL,
    [LabelType]            CHAR (1)     NOT NULL,
    [DefaultPkgLine]       CHAR (10)    NOT NULL,
    [StartTime]            DATETIME     NOT NULL,
    [JobName]              VARCHAR (50) NOT NULL,
    [DeviceName]           VARCHAR (50) NOT NULL,
    [DeviceType]           CHAR (1)     NOT NULL,
    [NoOfCopies]           SMALLINT     NULL,
    [UseNativeDriver]      BIT          NOT NULL,
    [SbmFromPalletStation] BIT          NOT NULL,
    [Facility]             CHAR (3)     NOT NULL,
    CONSTRAINT [PK_tblCimControlJob] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shop Order Start Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCimControlJob', @level2type = N'COLUMN', @level2name = N'StartTime';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packaging Line Assigned for the Computer ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCimControlJob', @level2type = N'COLUMN', @level2name = N'DefaultPkgLine';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'C=Case Label; F=Filter Coder; P=Pallet Label; X=Package Coder; ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCimControlJob', @level2type = N'COLUMN', @level2name = N'DeviceType';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CimControl device name (points to the job source file and search the label format on a column of the file by using the "JobName")', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCimControlJob', @level2type = N'COLUMN', @level2name = N'DeviceName';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Can be Pallet ID, Item #, ShopOrder # ...etc', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCimControlJob', @level2type = N'COLUMN', @level2name = N'JobName';


GO

