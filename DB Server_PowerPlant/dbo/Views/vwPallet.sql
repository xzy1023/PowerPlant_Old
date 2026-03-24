
CREATE VIEW [dbo].[vwPallet]
AS
SELECT        RRN, Facility, PalletID, QtyPerPallet, Quantity, ItemNumber, DefaultPkgLine, Operator, CreationDate, CreationTime, OrderComplete, LotID, ShopOrder, StartTime, ProductionDate, ExpiryDate, PrintStatus, CreationDateTime, 
                         ShiftProductionDate, ShiftNo, LastUpdate, OutputLocation, NULL AS DestinationShopOrder, BatchNumber
FROM            dbo.tblPallet

GO

