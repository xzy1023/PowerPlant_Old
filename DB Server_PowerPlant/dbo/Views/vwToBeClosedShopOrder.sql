-- =============================================
-- Author:		Bong Lee
-- Create date: Dec 14, 2015	WO#1096
-- Description:	View for the shop order numbers that are to be closed and all its pallets have been posted.
-- =============================================
CREATE VIEW [dbo].[vwToBeClosedShopOrder]
AS
SELECT        tCSOH.RRN, tCSOH.Facility, tCSOH.ShopOrder, tCSOH.DefaultPkgLine, tCSOH.Operator, tCSOH.SessionStartTime, tCSOH.ClosingTime, tCSOH.UpdatedToBPCS, 
                         tCSOH.BPCSClosingTime, tCSOH.LastUpdated, tCSOH.CreationTime
FROM            dbo.tblToBeClosedShopOrder AS tCSOH LEFT OUTER JOIN
                             (SELECT        Facility, ShopOrder
                               FROM            dbo.tblPallet
                               GROUP BY Facility, ShopOrder) AS tPLT ON tCSOH.Facility = tPLT.Facility AND tCSOH.ShopOrder = tPLT.ShopOrder
WHERE        (tCSOH.UpdatedToBPCS = '0') AND (tPLT.ShopOrder IS NULL)

GO

