CREATE TABLE [dbo].[tblUnitCountOutbound] (
    [ComputerName]         VARCHAR (50) NOT NULL,
    [CreationTime]         DATETIME     NOT NULL,
    [DestinationShopOrder] INT          NOT NULL,
    [Facility]             VARCHAR (3)  NOT NULL,
    [ItemNumber]           VARCHAR (35) NOT NULL,
    [Operator]             VARCHAR (10) NOT NULL,
    [OrderChange]          VARCHAR (20) NULL,
    [OutputLocation]       VARCHAR (10) NOT NULL,
    [PackagingLine]        VARCHAR (10) NOT NULL,
    [ShiftNo]              CHAR (1)     NOT NULL,
    [ShopOrder]            INT          NOT NULL,
    [SOStartTime]          DATETIME     NOT NULL,
    [UnitCount]            INT          NOT NULL,
    [UnitsPerPallet]       INT          NOT NULL,
    CONSTRAINT [PK_tblUnitCountOutbound] PRIMARY KEY CLUSTERED ([PackagingLine] ASC, [Facility] ASC)
);


GO

