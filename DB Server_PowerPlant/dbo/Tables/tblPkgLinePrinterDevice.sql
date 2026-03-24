CREATE TABLE [dbo].[tblPkgLinePrinterDevice] (
    [RRN]             INT          IDENTITY (1, 1) NOT NULL,
    [Facility]        CHAR (3)     NOT NULL,
    [PackagingLine]   CHAR (10)    NOT NULL,
    [DeviceType]      CHAR (1)     NOT NULL,
    [DeviceName]      VARCHAR (50) NOT NULL,
    [IPAddress]       VARCHAR (20) NULL,
    [UseNativeDriver] BIT          NOT NULL,
    [DeviceSubType]   CHAR (1)     CONSTRAINT [DF_tblPkgLinePrinterDevice_DeviceSubType] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_tblPkgLinePrinterDevice_1] PRIMARY KEY CLUSTERED ([RRN] ASC)
);


GO

CREATE UNIQUE NONCLUSTERED INDEX [idx_tblPkgLinePrinterDevice]
    ON [dbo].[tblPkgLinePrinterDevice]([Facility] ASC, [PackagingLine] ASC, [DeviceType] ASC, [DeviceName] ASC);


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'C = Case label print, P = Pallet label print, F = Filter coder, X = Package coder, R = Pallet Label Reprint
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPkgLinePrinterDevice', @level2type = N'COLUMN', @level2name = N'DeviceType';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CimControl device name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPkgLinePrinterDevice', @level2type = N'COLUMN', @level2name = N'DeviceName';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'B = Carton Box', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPkgLinePrinterDevice', @level2type = N'COLUMN', @level2name = N'DeviceSubType';


GO

