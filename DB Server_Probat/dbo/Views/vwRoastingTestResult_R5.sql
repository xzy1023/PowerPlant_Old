CREATE VIEW dbo.vwRoastingTestResult_R5
AS
SELECT        CONVERT(varchar(19), tPEBC.RECORDING_DATE, 120) + '_' + CAST(tPEBC.PRO_EXPORT_GENERAL_ID AS varchar(30)) + '_' + tPEBC.SOURCE_MACHINE + '_' + ISNULL(tPIT.NAME, 'Unknown') AS LabelKey, 
                         tPEBC.RECORDING_DATE, tPEBC.BATCH_ID, tPEBC.CUSTOMER_CODE, tPEBC.ORDER_NAME, CAST(tPEBC.END_TEMP / 10.0 AS decimal(6, 2)) AS END_TEMP, CAST(tPEBC.WATER / 10.0 AS decimal(5, 2)) AS WATER, 
                         CAST(tPEBC.COLOR / 100.0 AS decimal(5, 2)) AS COLOR, CAST(tPEBC.COLOR_MIN / 100.0 AS decimal(5, 2)) AS COLOR_MIN, CAST(tPEBC.COLOR_MAX / 100.0 AS decimal(5, 2)) AS COLOR_MAX, 
                         CAST(CASE WHEN tPEBC.MOISTURE_MANUAL IS NULL THEN tPEBC.MOISTURE ELSE tPEBC.MOISTURE_MANUAL END / 100.0 AS decimal(4, 2)) AS MOISTURE, CAST(tPEBC.MOISTURE_MIN / 100.0 AS decimal(4, 2)) 
                         AS MOISTURE_MIN, CAST(tPEBC.MOISTURE_MAX / 100.0 AS decimal(4, 2)) AS MOISTURE_MAX, tPEBC.DENSITY / 100.00 AS DENSITY, tPEBC.DENSITY_MIN / 100.00 AS DENSITY_MIN, 
                         tPEBC.DENSITY_MAX / 100.00 AS DENSITY_MAX, tPEBC.CREATED, tPEBC.PLC_BATCH_NR, tPEBC.INPUT_WEIGHT / 10.0 AS INPUT_WEIGHT, tPEBC.SOURCE_MACHINE, tPEBC.DESTINATION, tPIT.NAME, 
                         SUBSTRING(tPIT.NAME, CHARINDEX('BLD ', tPIT.NAME) + 4, LEN(tPIT.NAME) - CHARINDEX('BLD ', tPIT.NAME) + 4) AS [Blend Number], tPEBC.PRO_EXPORT_GENERAL_ID, SUBSTRING(DB_NAME(), CHARINDEX('_', DB_NAME()) 
                         - 2, 2) AS Facility, CAST(tPEBC.PRO_EXPORT_GENERAL_ID AS varchar(30)) + '-' + SUBSTRING(DB_NAME(), CHARINDEX('_', DB_NAME()) - 2, 2) AS UniqueKey
FROM            dbo.PRO_EXP_BATCH_CHECK AS tPEBC LEFT OUTER JOIN
                             (SELECT DISTINCT CUSTOMER_CODE, NAME
                               FROM            dbo.PRO_IMP_TYPES) AS tPIT ON tPEBC.CUSTOMER_CODE = tPIT.CUSTOMER_CODE
WHERE        (tPEBC.ORDER_TYP = 2) AND (tPEBC.CREATED > DATEADD(DAY, - 3, GETDATE())) AND (tPEBC.SOURCE_MACHINE = 'R5')

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
         Begin Table = "tPEBC"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 269
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tPIT"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 234
               Right = 225
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwRoastingTestResult_R5';


GO

EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwRoastingTestResult_R5';


GO

