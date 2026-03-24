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

-- =====================================================================
-- Author:		Bong Lee
-- Create date: Nov. 12, 2014
-- Description:	When the record is picked up by AX (i.e. UpdatedToBPCS 
--				is changed from 0 to 1, update the [BPCSClosingTime]
--				with current time.
-- =====================================================================
CREATE TRIGGER [dbo].[tgrToBeClosedShopOrder]
	ON [dbo].[tblToBeClosedShopOrder]
	AFTER UPDATE
AS 
BEGIN
	DECLARE @bitOldUpdatedToBPCS as bit;
	DECLARE @bitNewUpdatedToBPCS as bit;
	SET NOCOUNT ON;

	SELECT @bitOldUpdatedToBPCS = UpdatedToBPCS from Deleted;
	SELECT @bitNewUpdatedToBPCS = UpdatedToBPCS from Inserted;

	IF @bitOldUpdatedToBPCS = 0 AND @bitNewUpdatedToBPCS = 1
	BEGIN
		UPDATE tblToBeClosedShopOrder 
		SET tblToBeClosedShopOrder.[BPCSClosingTime] = GetDate(),
			tblToBeClosedShopOrder.[LastUpdated] = GetDate()
		FROM tblToBeClosedShopOrder T1
		INNER JOIN Deleted
		ON T1.[Facility] =  Deleted.[Facility]
			AND T1.[ShopOrder] = Deleted.[ShopOrder]
			AND T1.[ClosingTime] = Deleted.[ClosingTime];
	END
END

GO

