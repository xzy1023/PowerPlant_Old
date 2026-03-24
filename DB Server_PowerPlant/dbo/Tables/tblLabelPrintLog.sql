CREATE TABLE [dbo].[tblLabelPrintLog] (
    [RRN]                  INT          IDENTITY (1, 1) NOT NULL,
    [TimeSubmit]           DATETIME     NOT NULL,
    [LabelType]            CHAR (1)     NOT NULL,
    [DefaultPkgLine]       CHAR (10)    NOT NULL,
    [StartTime]            DATETIME     NOT NULL,
    [JobName]              VARCHAR (50) NOT NULL,
    [DeviceName]           VARCHAR (50) NOT NULL,
    [SbmFromPalletStation] BIT          NOT NULL,
    [Facility]             CHAR (3)     NOT NULL,
    [Requestor]            VARCHAR (10) NULL,
    [LabelId]              VARCHAR (50) NOT NULL,
    [IsManualRequest]      BIT          CONSTRAINT [DF_tblLabelPrintLog_IsManualRequest] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_tblLabelPrintLog] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Can be Pallet ID, Item #, ShopOrder # ...etc', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLabelPrintLog', @level2type = N'COLUMN', @level2name = N'JobName';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CimControl device name (points to the job source file and search the label format on a column of the file by using the "JobName")', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLabelPrintLog', @level2type = N'COLUMN', @level2name = N'DeviceName';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shop Order Start Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLabelPrintLog', @level2type = N'COLUMN', @level2name = N'StartTime';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packaging Line Assigned for the Computer ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLabelPrintLog', @level2type = N'COLUMN', @level2name = N'DefaultPkgLine';


GO

