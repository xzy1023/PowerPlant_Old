

-- =============================================
-- Author:		Bong Lee
-- Create date: Jun 13, 2014	WO#1096
-- Description:	view for Item Master From ERP joins Item Label Override 
-- WO#1297:		Aug 11, 2014	Bong Lee	
-- Description: Add stardard Cost per LB column
-- WO#20797	  : Dec 21, 2018	Bong Lee
-- Description:	Import 3 more fields from AX - GrsDepth, GrsHeight and GrsWidth and 
--				import 2 more fields fromtblItemLabelOvrr
-- Description: Adding PlantCode to the view because 'PPsp_PostProcess_DownLoadItemMaster_0X' is failing, where X is 1/2/7/9
-- =============================================
CREATE VIEW [dbo].[vwItemMasterWithOvrr]
AS
SELECT        tERP.Facility, tERP.ItemNumber, tERP.ProductionShelfLifeDays, tERP.LabelWeight, tERP.LabelWeightUOM, ISNULL(tOVR.BagLengthRequired, 'N') 
                         AS BagLengthRequired, ISNULL(tOVR.BagLength, 0.00) AS BagLength, ISNULL(tOVR.CaseLabelDateFmtCode, '') AS LabelDateFmtCode, 
                         tERP.PackagesPerSaleableUnit, tERP.SaleableUnitPerCase, tERP.QtyPerPallet, tERP.SCCCode, 
                         CASE WHEN tOVR.UseSCCAsUPC = 'Y' THEN tERP.SCCCode ELSE tERP.UPCCode END AS UPCCode, ISNULL(tOVR.OverrideItem, '') AS OverrideItem, 
                         CASE WHEN tOVR.OvrDesc1Flag = 1 THEN tOVR.ItemDesc1 ELSE tERP.ItemDesc1 END AS ItemDesc1, ISNULL(tOVR.ItemDesc2, '') AS ItemDesc2, 
                         ISNULL(tOVR.ItemDesc3, '') AS ItemDesc3, CASE WHEN OvrPackSizeFlag = 1 THEN tOVR.PackSize ELSE tERP.PackSize END AS PackSize, 
                         CASE WHEN tOVR.OvrNetWeightFlag = 1 THEN CASE WHEN tOVR.OvrNetWeightUOMFlag = 1 THEN tOVR.NetWeight + tOVR.NetWeightUOM ELSE tOVR.NetWeight + RIGHT(tERP.NetWeight,
                          2) END ELSE CASE WHEN tOVR.OvrNetWeightUOMFlag = 1 THEN LEFT(tERP.NetWeight, LEN(tERP.NetWeight) - 2) 
                         + tOVR.NetWeightUOM ELSE tERP.NetWeight END END AS NetWeight, ISNULL(tOVR.DomicileText1, '') AS DomicileText1, ISNULL(tOVR.DomicileText2, '') 
                         AS DomicileText2, ISNULL(tOVR.DomicileText3, '') AS DomicileText3, ISNULL(tOVR.DomicileText4, '') AS DomicileText4, ISNULL(tOVR.DomicileText5, '') 
                         AS DomicileText5, ISNULL(tOVR.DomicileText6, '') AS DomicileText6, ISNULL(tOVR.CaseLabelFmt1, '') AS CaseLabelFmt1, ISNULL(tOVR.CaseLabelFmt2, '') 
                         AS CaseLabelFmt2, ISNULL(tOVR.CaseLabelFmt3, '') AS CaseLabelFmt3, ISNULL(tOVR.PackageCoderFmt1, '') AS PackageCoderFmt1, 
                         ISNULL(tOVR.PackageCoderFmt2, '') AS PackageCoderFmt2, ISNULL(tOVR.PackageCoderFmt3, '') AS PackageCoderFmt3, ISNULL(tOVR.FilterCoderFmt, '') 
                         AS FilterCoderFmt, ISNULL(tOVR.ProductionDateDesc, '') AS ProductionDateDesc, ISNULL(tOVR.ExpiryDateDesc, '') AS ExpiryDateDesc, ISNULL(tOVR.PrintSOLot, 'N') 
                         AS PrintSOLot, ISNULL(tOVR.DateToPrintFlag, '0') AS DateToPrintFlag, ISNULL(tOVR.PrintCaseLabel, 'Y') AS PrintCaseLabel, tERP.Tie, tERP.Tier, 
                         tERP.ShipShelfLifeDays, tERP.ItemType, tERP.ItemMajorClass, ISNULL(tOVR.PalletCode, '') AS PalletCode, ISNULL(tOVR.SlipSheet, 0) AS SlipSheet, 
                         ISNULL(tOVR.PkgLabelDateFmtCode, '') AS PkgLabelDateFmtCode, tERP.StdCostPerLB
						 ,ISNULL(tERP.GrsDepth,0) as GrsDepth, ISNULL(tERP.GrsHeight,0) as GrsHeight, ISNULL(tERP.GrsWidth,0) as GrsWidth				-- WO#20797
						 ,ISNULL(tOVR.CaseLabelApplicator,0) as CaseLabelApplicator, ISNULL(tOVR.InsertBrewerFilter,0) as InsertBrewerFilter			-- WO#20797
						 ,'' PlantCode --Adding PlantCode to view because 'PPsp_PostProcess_DownLoadItemMaster_0X' is failing.
FROM            dbo.tblItemMasterFromERP AS tERP LEFT OUTER JOIN
                dbo.tblItemLabelOvrr AS tOVR ON tERP.Facility = tOVR.Facility AND tERP.ItemNumber = tOVR.ItemNumber

GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwItemMasterWithOvrr';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[29] 4[48] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tERP"
            Begin Extent = 
               Top = 17
               Left = 34
               Bottom = 192
               Right = 254
            End
            DisplayFlags = 280
            TopColumn = 11
         End
         Begin Table = "tOVR"
            Begin Extent = 
               Top = 22
               Left = 330
               Bottom = 204
               Right = 545
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 46
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwItemMasterWithOvrr';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N' = 11
         Column = 4815
         Alias = 2145
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwItemMasterWithOvrr';


GO

