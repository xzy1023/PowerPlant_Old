Imports System.Linq
Imports PowerPlant.ServerModels

Public Class frmLogScrapsRejectPoint

    Dim _dbServer As New ServerModels.PowerPlantEntities()

    Private Sub frmLogScraps_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable
        'Dim taSO As New dsShopOrderTableAdapters.CPPsp_ShopOrderIOTableAdapter
        Dim intRtnCde As Integer
        Dim strAction As String

        Try
            UcHeading1.ScreenTitle = "Log Scrap Reject Points"
            Me.lblItemDesc.Text = ""

            gdrEquipment = SharedFunctions.GetEquipmentInfo(gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine)
            strAction = "ALLBySO"
            If gdrEquipment.SubType = "T" Then
                strAction = "ALLBySO+OtherScrap"
            End If

            'Retrieve Shop Order record using the Shop Order from the Session Control
            Dim shopOrder As Int32
            Dim equipmentID As String
            intRtnCde = gtaSO.Fill(tblSO, "GetSO&Item", gdrSessCtl("Facility"), gdrSessCtl("ShopOrder"), "")
            With tblSO.Rows(0)
                Me.lblShopOrder.Text = .Item("ShopOrder")
                shopOrder = .Item("ShopOrder")
                Me.lblItemNo.Text = .Item("ItemNumber")
                equipmentID = .Item("PackagingLine")
                Me.lblItemDesc.Text = .Item("ItemDesc1")
                Try
                    gtaCompScrap.Fill(Me.DsComponentScrap.CPPsp_EditComponentScrap, .Item("Facility"), CType(.Item("ShopOrder"), Integer),
                            gdrSessCtl("StartTime"), "", 0, strAction)
                Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True And ex.ErrorCode = -2146232060
                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                    gtaCompScrap.Fill(Me.DsComponentScrap.CPPsp_EditComponentScrap, .Item("Facility"), CType(.Item("ShopOrder"), Integer),
                            gdrSessCtl("StartTime"), "", 0, strAction)
                End Try
            End With


            ' declare the data source for the dgv
            Dim rejectPointDgvLogs As New List(Of RejectPointDgvLog)
            ' Add the reject points that have been logged to the dgv data source for this equipment and shop order
            Dim rejectPointLogs As List(Of ServerModels.tblScrapRejectPointLog) = _dbServer.tblScrapRejectPointLog.AsQueryable.Where(
                Function(x) x.Equipment = equipmentID And x.ShopOrder = shopOrder _
                And x.ProductionDate = DateAndTime.Today And x.Shift = gdrSessCtl.OverrideShiftNo).ToList()

            'Dim rejectPointLogs As List(Of ServerModels.tblScrapRejectPointLog) = _dbServer.tblScrapRejectPointLog.AsQueryable.Where(
            '    Function(x) x.Equipment = equipmentID And x.ShopOrder = shopOrder).ToList()

            Dim today As Date = DateAndTime.Today

            For Each rejectPointLog As ServerModels.tblScrapRejectPointLog In rejectPointLogs
                Dim rejectPointDgvLog As New RejectPointDgvLog With {
                    .ID = rejectPointLog.RejectPointID,
                    .Qty = rejectPointLog.Qty
                }
                rejectPointDgvLogs.Add(rejectPointDgvLog)
            Next
            ' Add the reject points that have not been logged for this equipment to the dgv data source
            Dim equipmentScraps = _dbServer.tblEquipment_ScrapRejectPoint.AsQueryable.Where(Function(x) x.EquipmentID = equipmentID And x.VisibleInIPC = True).ToList()
            For Each equipmentScrap As ServerModels.tblEquipment_ScrapRejectPoint In equipmentScraps
                If Not rejectPointLogs.Any(Function(x) x.RejectPointID = equipmentScrap.RejectPointID And x.Shift = gdrSessCtl.OverrideShiftNo) Then
                    Dim rejectPointDgvLog As New RejectPointDgvLog With {
                        .ID = equipmentScrap.RejectPointID,
                        .Qty = 0,
                        .Shift = gdrSessCtl.OverrideShiftNo
                    }
                    rejectPointDgvLogs.Add(rejectPointDgvLog)
                End If
            Next
            'add description and shift to rejectPointDgvLogs
            For Each rejectPointDgvLog As RejectPointDgvLog In rejectPointDgvLogs
                Dim rejectPoint As ServerModels.tblScrapRejectPoint = _dbServer.tblScrapRejectPoint.AsQueryable.Where(Function(x) x.RRN = rejectPointDgvLog.ID).FirstOrDefault()
                If rejectPoint IsNot Nothing Then
                    rejectPointDgvLog.RejectPoint = rejectPoint.RejectPoint
                    rejectPointDgvLog.Shift = gdrSessCtl.OverrideShiftNo
                End If
            Next
            ' Bind the dgv to the data source and sort the dgv by the ID column
            dgvRejectPoints.DataSource = rejectPointDgvLogs.OrderBy(Function(x) x.ID).ToList()
            dgvRejectPoints.AutoResizeColumns()

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPrvScn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        Dim qty As Int32

        For Each dgvdr As DataGridViewRow In dgvRejectPoints.Rows
            With dgvdr

                'get the quantity from the dgv
                If IsDBNull(.Cells("Qty").Value) Then
                    qty = 0
                Else
                    qty = .Cells("Qty").Value
                End If

                'if the quantity is greater than or equal to zero, log the reject point, otherwise show a message box and ignore the entry
                If qty >= 0 And qty < 999999 Then
                    Dim rejectPointLog As New ServerModels.tblScrapRejectPointLog With {
                                            .Equipment = gdrEquipment.EquipmentID,
                                            .ShopOrder = gdrSessCtl.ShopOrder,
                                            .RejectPointID = dgvdr.Cells("ID").Value,
                                            .Qty = qty,
                                            .ModifiedDateTime = DateTime.Now,
                                            .ItemNumber = gdrSessCtl.ItemNumber,
                                            .Operator = gdrSessCtl.Operator,
                                            .Shift = gdrSessCtl.OverrideShiftNo,
                                            .ProductionDate = DateAndTime.Today}
                    'check if the reject point log already exists
                    Dim existingRejectPointLog = _dbServer.tblScrapRejectPointLog.AsQueryable.Where(
                        Function(x) x.Equipment = gdrSessCtl.OverridePkgLine _
                        And x.ShopOrder = rejectPointLog.ShopOrder _
                        And x.RejectPointID = rejectPointLog.RejectPointID _
                        And x.Shift = gdrSessCtl.OverrideShiftNo _
                        And x.ProductionDate = DateAndTime.Today).FirstOrDefault()

                    If existingRejectPointLog IsNot Nothing Then
                        'if the reject point log already exists, update it
                        existingRejectPointLog.Qty = rejectPointLog.Qty
                        existingRejectPointLog.ModifiedDateTime = DateTime.Now
                        existingRejectPointLog.Shift = gdrSessCtl.OverrideShiftNo
                        existingRejectPointLog.Operator = gdrSessCtl.Operator
                    Else
                        'if the reject point log does not exist, add it
                        _dbServer.AddTotblScrapRejectPointLog(rejectPointLog)
                    End If
                Else
                    MessageBox.Show("Quantity can not be less than zero or more than 999999, entry is ignored.", "Invalid Entry")
                End If

            End With
        Next

        Try
            _dbServer.SaveChanges()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Me.Close()
    End Sub

    Private Sub dgvRejectPoints_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvRejectPoints.CellMouseDown
        Dim dgrKeyPad As DialogResult
        Dim ctl As New Control

        Try
            If e.RowIndex >= 0 AndAlso dgvRejectPoints.Columns(e.ColumnIndex).Name = "Qty" Then
                dgvRejectPoints.ReadOnly = False
                With dgvRejectPoints.Rows(e.RowIndex).Cells("Qty")
                    If Not IsDBNull(.Value) Then
                        ctl.Text = .Value
                    End If
                    ctl.Location = dgvRejectPoints.Location
                    dgrKeyPad = SharedFunctions.PopNumKeyPad(Me, ctl)
                    If dgrKeyPad = Windows.Forms.DialogResult.OK Then
                        If gstrNumPadValue = "" Then
                            .Value = 0
                        Else
                            .Value = gstrNumPadValue
                        End If
                        If .Value < 0 Then
                            MessageBox.Show("Scrap quantity can not be less than zero. Entry is ignored", "Invalid Entry")
                            .Value = 0
                        End If
                        dgvRejectPoints.RefreshEdit()

                    End If
                End With
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ctl.Dispose()
        End Try
    End Sub

End Class

Public Class RejectPointDgvLog
    Public Property ID As String
    Public Property Shift As Int32
    Public Property RejectPoint As String
    Public Property Qty As String
End Class