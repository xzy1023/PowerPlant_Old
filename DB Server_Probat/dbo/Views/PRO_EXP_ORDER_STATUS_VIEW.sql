
-- =============================================
-- Author:		Bong Lee
-- Mod. date: Mar 18, 2016	FX20160318
-- Description:	Check more Probat tables for different order type.
-- FIX20160422: Mar 18, 2016	Bong Lee
-- Description:	Filter invalid Order Name.
-- =============================================

CREATE VIEW [dbo].[PRO_EXP_ORDER_STATUS_VIEW]
AS
SELECT      tStatus.TRANSFERED, tStatus.TRANSFERED_TIMESTAMP, tStatus.RECORDING_DATE, tStatus.ORDER_NAME,
			tStatus.ORDER_SUB_NAME,	tStatus.ORDER_TYP, tStatus.STATUS, tStatus.SCHEDULED_WEIGHT, 
            tStatus.RESULT_WEIGHT, tStatus.YIELD, tStatus.PRO_EXPORT_GENERAL_ID AS ID
FROM            dbo.PRO_EXP_ORDER_STATUS tStatus
LEFT OUTER JOIN [dbo].[PRO_EXP_ORDER_LOAD_R] tLR
ON	tStatus.[ORDER_NAME] = tLR.[ORDER_NAME] and tStatus.[TRANSFERED] = tLR.[TRANSFERED]
LEFT OUTER JOIN [dbo].[PRO_EXP_ORDER_UNLOAD_R] tULR
ON	tStatus.[ORDER_NAME] = tULR.[ORDER_NAME] and tStatus.[TRANSFERED] = tULR.[TRANSFERED]
LEFT OUTER JOIN [dbo].[PRO_EXP_ORDER_LOAD_G] tLG
ON	tStatus.[ORDER_NAME] = tLG.[ORDER_NAME] and tStatus.[TRANSFERED] = tLG.[TRANSFERED]
LEFT OUTER JOIN [dbo].[PRO_EXP_ORDER_LOAD_G] tULG
ON	tStatus.[ORDER_NAME] = tULG.[ORDER_NAME] and tStatus.[TRANSFERED] = tULG.[TRANSFERED]
LEFT OUTER JOIN [dbo].[PRO_EXP_TRANSF] tTFR
ON	tStatus.[ORDER_NAME] = tTFR.[ORDER_NAME] and tStatus.[TRANSFERED] = tTFR.[TRANSFERED]
WHERE   (DATEDIFF(Second, tStatus.RECORDING_DATE, GETDATE()) / 3600.0 > 1.0) AND (tStatus.TRANSFERED = 0)
		AND (
		(tStatus.ORDER_TYP = 2 AND tLR.[ORDER_NAME] IS NULL and tULR.[ORDER_NAME] IS NULL) OR 
		(tStatus.ORDER_TYP = 4 AND tLG.[ORDER_NAME] IS NULL and tULG.[ORDER_NAME] IS NULL)  OR
		(tStatus.ORDER_TYP = 6 AND tTFR.[ORDER_NAME] IS NULL)
		)
		AND Len(tStatus.[ORDER_NAME]) > 7 AND IsNumeric(tStatus.[ORDER_NAME]) = 1 			-- FIX20160422

GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'PRO_EXP_ORDER_STATUS_VIEW';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
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
         Begin Table = "PRO_EXP_ORDER_STATUS"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 269
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
      Begin ColumnWidths = 10
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
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'PRO_EXP_ORDER_STATUS_VIEW';


GO

