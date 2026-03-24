CREATE TABLE [dbo].[tblToBeClosedShopOrder] (
    [RRN]              INT          IDENTITY (1, 1) NOT NULL,
    [Facility]         CHAR (3)     NOT NULL,
    [ShopOrder]        INT          NOT NULL,
    [DefaultPkgLine]   CHAR (10)    NOT NULL,
    [Operator]         VARCHAR (10) NOT NULL,
    [SessionStartTime] DATETIME     NOT NULL,
    [ClosingTime]      DATETIME     NOT NULL,
    [UpdatedToBPCS]    BIT          CONSTRAINT [DF_tblToBeClosedShopOrder_UpdatedToBPCS] DEFAULT ((0)) NOT NULL,
    [BPCSClosingTime]  DATETIME     NULL,
    [LastUpdated]      DATETIME     CONSTRAINT [DF_tblToBeClosedShopOrder_LastUpdate] DEFAULT (getdate()) NOT NULL,
    [CreationTime]     DATETIME     CONSTRAINT [DF_tblToBeClosedShopOrder_CreationTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_tblToBeClosedShopOrder] PRIMARY KEY CLUSTERED ([Facility] ASC, [ShopOrder] ASC, [ClosingTime] ASC)
);


GO

