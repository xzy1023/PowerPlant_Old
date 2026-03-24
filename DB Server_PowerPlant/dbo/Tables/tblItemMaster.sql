CREATE TABLE [dbo].[tblItemMaster] (
    [Facility]                CHAR (3)        NOT NULL,
    [ItemNumber]              VARCHAR (35)    NOT NULL,
    [ProductionShelfLifeDays] INT             NOT NULL,
    [LabelWeight]             DECIMAL (10, 3) NOT NULL,
    [LabelWeightUOM]          CHAR (2)        NOT NULL,
    [BagLengthRequired]       CHAR (1)        NOT NULL,
    [BagLength]               DECIMAL (4, 2)  NOT NULL,
    [LabelDateFmtCode]        CHAR (2)        NOT NULL,
    [PackagesPerSaleableUnit] DECIMAL (5)     NOT NULL,
    [SaleableUnitPerCase]     DECIMAL (5)     NOT NULL,
    [QtyPerPallet]            INT             NOT NULL,
    [SCCCode]                 VARCHAR (14)    NOT NULL,
    [UPCCode]                 VARCHAR (14)    NOT NULL,
    [OverrideItem]            VARCHAR (35)    NOT NULL,
    [ItemDesc1]               VARCHAR (50)    NOT NULL,
    [ItemDesc2]               VARCHAR (50)    NOT NULL,
    [ItemDesc3]               VARCHAR (50)    NULL,
    [PackSize]                VARCHAR (12)    NOT NULL,
    [NetWeight]               VARCHAR (10)    NOT NULL,
    [DomicileText1]           VARCHAR (24)    NOT NULL,
    [DomicileText2]           VARCHAR (24)    NOT NULL,
    [DomicileText3]           VARCHAR (24)    NOT NULL,
    [DomicileText4]           VARCHAR (24)    NOT NULL,
    [DomicileText5]           VARCHAR (24)    NOT NULL,
    [DomicileText6]           VARCHAR (24)    NOT NULL,
    [CaseLabelFmt1]           VARCHAR (25)    NOT NULL,
    [CaseLabelFmt2]           VARCHAR (25)    NOT NULL,
    [CaseLabelFmt3]           VARCHAR (25)    NOT NULL,
    [PackageCoderFmt1]        VARCHAR (25)    NOT NULL,
    [PackageCoderFmt2]        VARCHAR (25)    NOT NULL,
    [PackageCoderFmt3]        VARCHAR (25)    NOT NULL,
    [FilterCoderFmt]          VARCHAR (25)    NOT NULL,
    [ProductionDateDesc]      VARCHAR (30)    NULL,
    [ExpiryDateDesc]          VARCHAR (30)    NULL,
    [PrintSOLot]              CHAR (1)        NOT NULL,
    [DateToPrintFlag]         CHAR (1)        NOT NULL,
    [PrintCaseLabel]          CHAR (1)        NOT NULL,
    [Tie]                     INT             NOT NULL,
    [Tier]                    INT             NOT NULL,
    [ShipShelfLifeDays]       INT             NULL,
    [ItemType]                VARCHAR (10)    NOT NULL,
    [ItemMajorClass]          VARCHAR (10)    NOT NULL,
    [PalletCode]              CHAR (1)        NULL,
    [SlipSheet]               BIT             NULL,
    [PkgLabelDateFmtCode]     CHAR (2)        NULL,
    [StdCostPerLB]            DECIMAL (15, 5) NOT NULL,
    [GrsDepth]                DECIMAL (16, 8) NULL,
    [GrsHeight]               DECIMAL (16, 8) NULL,
    [GrsWidth]                DECIMAL (16, 8) NULL,
    [CaseLabelApplicator]     TINYINT         NULL,
    [InsertBrewerFilter]      BIT             NULL,
    [PlantCode]               VARCHAR (10)    CONSTRAINT [DF_tblItemMaster_PlantCode] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_tblItemMaster] PRIMARY KEY CLUSTERED ([Facility] ASC, [ItemNumber] ASC)
);


GO




-- =====================================================================
-- Author:		Bong Lee
-- Create date: June 18, 2009
-- Description:	When the label weight of an item is changed,
--				update the weights in the Shop Order Weight Spec. and 
--				the Shop Order Weight Spec. History tables. Also, flag
--				the Down Load Table List to require to download the 
--              tblShopOrderWeightSpec to IPCS.
-- =====================================================================
CREATE TRIGGER [tgrItemMaster]
ON [dbo].[tblItemMaster]
AFTER UPDATE
AS
Declare @bitLabelWeightChanged as bit
Declare @chrFacility as char(3)
Declare @vchItemNumber as varchar(35)
DECLARE @bitTableChanged bit

Set	@bitTableChanged = 0

-- Was Label weight changed?
Select @chrFacility = tNew.Facility, @vchItemNumber = tNew.ItemNumber,
	@bitLabelWeightChanged = Case When tNew.LabelWeight <> tOld.LabelWeight Then 1 Else 0 End 
From inserted tNew inner join deleted tOld
On tNew.Facility = tOld.Facility and tNew.ItemNumber = tOld.ItemNumber

If @bitLabelWeightChanged = 1
Begin
	-- Update Shop Order Weight Spec.
	Update dbo.tblShopOrderWeightSpec 
	Set TargetWeight = T2.TargetWeight ,MinWeight = T2.MinWeight, 
		MaxWeight = T2.MaxWeight, LowerControlLimit = T2.LowerControlLimit
	From dbo.tblShopOrderWeightSpec T1 
	Cross Apply dbo.fnShopOrderWeightSpec(@chrFacility,T1.ShopOrder) T2
	Where T1.Facility = @chrFacility And T1.ItemNumber = @vchItemNumber

	If @@RowCount > 0 
		Set	@bitTableChanged = 1;		

	-- Update Shop Order Weight Spec. History
	Update dbo.tblShopOrderWeightSpecHst 
		SET TargetWeight = T2.TargetWeight,
			MinWeight = T2.MinWeight,
			MaxWeight = T2.MaxWeight, 
			LowerControlLimit = T2.LowerControlLimit,
			DateUpdated = getdate()
 		FROM tblShopOrderWeightSpecHst AS T1 
		INNER JOIN tblShopOrderWeightSpec AS T2 
		ON T1.Facility = T2.Facility AND T1.ShopOrder = T2.ShopOrder 
		WHERE T1.TargetWeight <> T2.TargetWeight 
			OR T1.MinWeight <> T2.MinWeight
			OR T1.MaxWeight <> T2.MaxWeight 
			OR T1.LowerControlLimit <> T2.LowerControlLimit

	-- flag the Down Load Table List to require to download the table tblShopOrderWeightSpec to IPCs
	If @bitTableChanged = 1
		update dbo.tblDownLoadTableList set active = 1 Where Facility = @chrFacility And TableName = 'tblShopOrderWeightSpec';

End

GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'"Y" - Required; "N"- Not required so the Bag Length can be zero', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemMaster', @level2type = N'COLUMN', @level2name = N'BagLengthRequired';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''Y'' - Print Case Label; ''N'' - Do Not Print Case Label', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemMaster', @level2type = N'COLUMN', @level2name = N'PrintCaseLabel';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''Y'' - Print Shop Order Lot; ''N'' - Do Not Print', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemMaster', @level2type = N'COLUMN', @level2name = N'PrintSOLot';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'''0'' = None; ''1''= Print Expiry Date; ''2''= Print Production Date; ''3''= Print Expiry & Prodcution Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemMaster', @level2type = N'COLUMN', @level2name = N'DateToPrintFlag';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Code Dating - I$CDC in IIME$ (Date format code for Labels)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblItemMaster', @level2type = N'COLUMN', @level2name = N'LabelDateFmtCode';


GO

