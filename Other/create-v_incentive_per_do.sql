USE [FFB_Dbase]
GO

/****** Object:  View [dbo].[v_incentive_per_do]    Script Date: 4/22/2022 11:35:43 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO










CREATE VIEW [dbo].[v_incentive_per_do]
AS
SELECT convert(date,DATEADD(d, -1, DATEADD(m, DATEDIFF(m, 0, CONVERT(date, CONCAT(periode, '01'))) + 1, 0))) as tgl
      ,i.[PMKSID]
	  ,code as SAP_VendorCode
	  ,Netto * Incentive AS bayar_with_pph
	  ,ROUND(i.PPH22 * Netto * Incentive / 100, 0) AS pph
	  ,ROUND( (Netto * Incentive) - ROUND(i.PPH22 * Netto * Incentive / 100, 0), 0) as bayar_no_pph
	  ,concat(i.PMKSID, ' (', right(replace(convert(varchar(8), DATEADD(d, -1, DATEADD(m, DATEDIFF(m, 0, CONVERT(date, CONCAT(periode, '01'))) + 1, 0)), 5),'-',''),4), ')') as Assignment
	  ,CompanyCode
	  ,BusinessArea
	  ,BusinessAreaCoP
	  ,HouseBank
	  ,periode as yyyymm
  FROM [dbo].[t_Incentive] i
	LEFT JOIN tblSupplier S ON i.PMKSID = S.PMKSID AND i.SupplierID = S.SupplierID and S.StatusActive = 'Y'
	LEFT JOIN tblPMKS PMKS ON i.PMKSID = PMKS.PMKSID
  WHERE i.Approval = 1 and ISNULL(i.FCDefunctInd,0)=0
    AND ISNULL(i.uploadtosap,0)=0

--SELECT convert(date,DATEADD(d, -1, DATEADD(m, DATEDIFF(m, 0, CONVERT(date, CONCAT(period, '01'))) + 1, 0))) as tgl
--      ,i.[PMKSID]
--	  ,code as SAP_VendorCode
--	  ,Netto * Incentive AS bayar_with_pph
--	  ,ROUND(i.pph22 * Netto * Incentive / 100, 0) AS pph
--	  ,ROUND( (Netto * Incentive) - ROUND(i.pph22 * Netto * Incentive / 100, 0), 0) as bayar_no_pph
--	  ,concat(i.PMKSID, ' (', right(replace(convert(varchar(8), DATEADD(d, -1, DATEADD(m, DATEDIFF(m, 0, CONVERT(date, CONCAT(period, '01'))) + 1, 0)), 5),'-',''),4), ')') as Assignment
--	  ,CompanyCode
--	  ,BusinessArea
--	  ,BusinessAreaCoP
--	  ,HouseBank
--	  ,period as yyyymm
--  FROM [FFB_Dbase].[dbo].[tblIncentive] i
--	LEFT JOIN tblSupplier S ON i.PMKSID = S.PMKSID AND i.SupplierID = S.SupplierID and S.StatusActive = 'Y'
--	LEFT JOIN tblPMKS PMKS ON i.PMKSID = PMKS.PMKSID
--  WHERE i.Approval = 1
--    AND (i.upload_to_sap IS NULL OR i.upload_to_sap = 0)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
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
         Begin Table = "tblIncentive"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_incentive_per_do'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_incentive_per_do'
GO


