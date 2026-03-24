











CREATE VIEW [dbo].[vwLabelData]
AS
SELECT     TOP (100) PERCENT dbo.tblDynamicLabelData.Facility, dbo.tblDynamicLabelData.RecordType, dbo.tblDynamicLabelData.LabelKey, 
                      dbo.tblDynamicLabelData.PackagingLine, dbo.tblDynamicLabelData.ShopOrder, CASE WHEN ISNULL(dbo.tblItemMaster.OverrideItem, '') 
                      = '' THEN dbo.tblDynamicLabelData.ItemNumber ELSE dbo.tblItemMaster.OverrideItem END AS ItemNumber, dbo.tblDynamicLabelData.PalletID, 
                      dbo.tblDynamicLabelData.Quantity, dbo.tblDynamicLabelData.MPProductionDate, dbo.tblDynamicLabelData.ProductionDate, 
                      dbo.tblDynamicLabelData.CreationTime, dbo.tblDynamicLabelData.Operator, dbo.tblItemMaster.ItemDesc1, dbo.tblItemMaster.ItemDesc2, 
                      dbo.tblItemMaster.ItemDesc3, dbo.tblItemMaster.SCCCode, dbo.tblItemMaster.UPCCode, dbo.tblItemMaster.DomicileText1, 
                      dbo.tblItemMaster.DomicileText2, dbo.tblItemMaster.DomicileText3, dbo.tblItemMaster.DomicileText4, dbo.tblItemMaster.DomicileText5, 
                      dbo.tblItemMaster.DomicileText6, dbo.tblItemMaster.NetWeight, dbo.tblItemMaster.PackSize, dbo.tblItemMaster.ProductionShelfLifeDays, 
                      dbo.tblDynamicLabelData.PreFmtProductionDate, dbo.tblDynamicLabelData.PreFmtExpiryDate,
					  -- WO#6437 ADD Start
	                  CASE WHEN RecordType = 'P' THEN (CASE WHEN tILO.PalletLabelFmt = '' THEN 'Pallet_prd.itf' ELSE tILO.PalletLabelFmt END) ELSE dbo.tblItemMaster.CaseLabelFmt1 END AS CasePalletFmt1, 
                      CASE WHEN RecordType = 'P' THEN (CASE WHEN tILO.PalletLabelFmt = '' THEN 'Pallet_prd.itf' ELSE tILO.PalletLabelFmt END) ELSE dbo.tblItemMaster.CaseLabelFmt2 END AS CasePalletFmt2, 
                      CASE WHEN RecordType = 'P' THEN (CASE WHEN tILO.PalletLabelFmt = '' THEN 'Pallet_prd.itf' ELSE tILO.PalletLabelFmt END) ELSE dbo.tblItemMaster.CaseLabelFmt3 END AS CasePalletFmt3, 
					  -- WO#6437 ADD Stop
  					  -- WO#6437 DEL Start
                      --CASE WHEN RecordType = 'P' THEN 'Pallet_prd.itf' ELSE dbo.tblItemMaster.CaseLabelFmt1 END AS CasePalletFmt1, 
                      --CASE WHEN RecordType = 'P' THEN 'Pallet_prd.itf' ELSE dbo.tblItemMaster.CaseLabelFmt2 END AS CasePalletFmt2, 
                      --CASE WHEN RecordType = 'P' THEN 'Pallet_prd.itf' ELSE dbo.tblItemMaster.CaseLabelFmt3 END AS CasePalletFmt3, 
					  -- WO#6437 DEL Stop
                      dbo.tblItemMaster.PackageCoderFmt1, dbo.tblItemMaster.PackageCoderFmt2, dbo.tblItemMaster.PackageCoderFmt3, 
                      dbo.tblItemMaster.FilterCoderFmt, dbo.tblDynamicLabelData.LotID, dbo.tblItemMaster.CaseLabelFmt1, dbo.tblItemMaster.CaseLabelFmt2, 
                      dbo.tblItemMaster.CaseLabelFmt3, dbo.tblDynamicLabelData.ItemNumber AS Expr1, dbo.tblItemMaster.OverrideItem
					-- WO#6437 ADD Start
					  ,tILO.ProductionDateDescOnBox
					  ,tILO.ExpiryDateDescOnBox
					  ,tILO.AdditionalText1
					  ,tILO.AdditionalText2
					  --,dbo.tblitemmaster.PlantCode --Adding Plant Code for JMS - Nov 21 2024
					  ,CONVERT(varchar(20), dbo.tblDynamicLabelData.ShopOrder) +'-'+dbo.tblDynamicLabelData.MPProductionDate AS BatchNumber --ADO Task 14901 - Adding BatchNumber for Case labels - Nov 21 2024
					-- WO#6437 ADD Stop
					  ,RIGHT(YEAR(GETDATE()), 1) + RIGHT('000' + CAST(DATEPART(DAYOFYEAR, GETDATE()) AS VARCHAR(3)), 3) + FORMAT(GETDATE(), 'HHmmss') AS SysDateTime,
					  --,RIGHT(YEAR(GETDATE()), 1) + RIGHT('000' + CAST(DATEPART(DAYOFYEAR, GETDATE()) AS VARCHAR(3)), 3) + FORMAT(GETDATE(), 'HHmmss.fff') AS SysDateTime
					  tblItemMaster.ProductionDateDesc, tblItemMaster.LabelDateFmtCode
FROM         dbo.tblDynamicLabelData LEFT OUTER JOIN
                      dbo.tblItemMaster ON dbo.tblDynamicLabelData.Facility = dbo.tblItemMaster.Facility AND 
                      dbo.tblDynamicLabelData.ItemNumber = dbo.tblItemMaster.ItemNumber
-- WO#6437 ADD Start
LEFT OUTER JOIN		  dbo.tblItemLabelOvrr tILO
ON					  dbo.tblDynamicLabelData.Facility = tILO.Facility AND 
                      dbo.tblDynamicLabelData.ItemNumber = tILO.ItemNumber
-- WO#6437 ADD Stop
ORDER BY dbo.tblDynamicLabelData.LabelKey

GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[46] 4[13] 2[40] 3) )"
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
         Begin Table = "tblDynamicLabelData"
            Begin Extent = 
               Top = 6
               Left = 17
               Bottom = 228
               Right = 244
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "tblItemMaster"
            Begin Extent = 
               Top = 7
               Left = 301
               Bottom = 228
               Right = 496
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
      Begin ColumnWidths = 38
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
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1935
         Alias = 2010
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         Sort', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwLabelData';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'Type = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwLabelData';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwLabelData';


GO

