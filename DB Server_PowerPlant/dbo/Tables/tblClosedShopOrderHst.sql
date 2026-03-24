CREATE TABLE [dbo].[tblClosedShopOrderHst] (
    [RRN]              INT          IDENTITY (1, 1) NOT NULL,
    [Facility]         CHAR (3)     NOT NULL,
    [ShopOrder]        INT          NOT NULL,
    [DefaultPkgLine]   CHAR (10)    NOT NULL,
    [Operator]         VARCHAR (10) NOT NULL,
    [SessionStartTime] DATETIME     NOT NULL,
    [ClosingTime]      DATETIME     NOT NULL,
    [UpdatedToBPCS]    BIT          CONSTRAINT [DF_tblClosedShopOrderHst_UpdatedToBPCS] DEFAULT ((0)) NOT NULL,
    [BPCSClosingTime]  DATETIME     NULL,
    [LastUpdated]      DATETIME     CONSTRAINT [DF_tblClosedShopOrderHst_LastUpdate] DEFAULT (getdate()) NOT NULL,
    [CreationTime]     DATETIME     CONSTRAINT [DF_tblClosedShopOrderHst_CreationTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_tblClosedShopOrderHst] PRIMARY KEY CLUSTERED ([Facility] ASC, [ShopOrder] ASC, [ClosingTime] ASC)
);


GO

