Public Class frmShopOrderSchedule
    Dim gstrErrMsg As String
    Private Sub frmShopOrderSchedule_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim strMyComputerName As String = My.Computer.Name
        'WO#871 Dim strMyComputerName As String = gdrSessCtl.ComputerName

        Try
            UcHeading1.ScreenTitle = "Shop Order Schedule"
            txtPkgLine.Text = ""
            txtPkgLine.Text = gdrSessCtl.OverridePkgLine

            'If the latest downloaded data is ready in staging area and shop order is not started, import data
            'from the staging area into the local data base
            If gdrSessCtl.ShopOrder = 0 Then                                        'WO#5370
                'WO#871 SharedFunctions.ImportMasterTables(strMyComputerName)
                If SharedFunctions.IsDataReadyForRefresh Then                       'WO#5370
                    SharedFunctions.ImportMasterTables(gstrMyComputerName)           'WO#871
                    txtPkgLine_TextChanged(sender, e)                               'WO#5370
                End If                                                              'WO#5370

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPrvScn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub

    Private Sub dgvSOSchedule_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgvSOSchedule.CellFormatting
        Try
            Dim strItemDesc1 As String  'WO#755
            Dim strItemDesc2 As String  'WO#755

            Select Case dgvSOSchedule.Columns(e.ColumnIndex).Name
                Case "OrderQty"
                    e.Value = Math.Round(e.Value, 0)
                Case "SODescription"
                    Dim sb As New System.Text.StringBuilder
                    With dgvSOSchedule.Rows(e.RowIndex)

                        'Remove the Shop Order number from the SODescription
                        If IsDBNull(e.Value) = False Then
                            If IsDBNull(.Cells("strBlend").Value) = False Then      'WO#274
                                e.Value = RTrim(e.Value.ToString.Substring(InStr(e.Value, " ", CompareMethod.Text)))
                            Else                                                    'WO#274
                                e.Value = String.Empty                              'WO#274
                            End If                                                  'WO#274

                            'if the blend of the item is not blank, append the item descripton 1 & 2 to it
                            'WO#274 If .Cells("strBlend").Value <> "" Then

                            'WO#755 ADD Start
                            If IsDBNull(.Cells("strItemDesc1").Value) = True Then
                                strItemDesc1 = String.Empty
                            Else
                                strItemDesc1 = RTrim(.Cells("strItemDesc1").Value)
                            End If

                            If IsDBNull(.Cells("strItemDesc2").Value) = True Then
                                strItemDesc2 = String.Empty
                            Else
                                'FX150814 strItemDesc2 = RTrim(.Cells("strItemDesc1").Value)
                                strItemDesc2 = RTrim(.Cells("strItemDesc2").Value)      'FX150814
                            End If
                            'WO#755 ADD Stop
                            'WO#755 e.Value = String.Format("{0} {1} {2}", e.Value, RTrim(.Cells("strItemDesc1").Value), RTrim(.Cells("strItemDesc2").Value))
                            'FX150814 e.Value = String.Format("{0} {1} {2}", e.Value, strItemDesc1, strItemDesc2) 'WO#755
                            e.Value = String.Format("{0} {1} ", strItemDesc1, strItemDesc2) 'FX150814 
                            'WO#274 End If
                        End If
                    End With
                Case "StartDateTime"
                    e.Value = Format(e.Value, "MM/dd HH:mm")
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub txtPkgLine_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPkgLine.TextChanged
        Try
            'Dim tblEQ As New dsEquipment.CPPsp_EquipmentIODataTable

            gstrErrMsg = Nothing
            'Retrieve equipment record using the packaging line 
            If txtPkgLine.Text <> "" Then
                lblPkgLine.Visible = False
                lblPkgLine.Text = String.Empty
                'WO#359 lblPkgLine.Text = SharedFunctions.GetEquipmentDescription(gdrCmpCfg("Facility"), sender.text)
                'WO#359 If lblPkgLine.Text = String.Empty Then
                If Not SharedFunctions.IsLineActive(Nothing, txtPkgLine.Text) Then 'WO#359
                    dgvSOSchedule.DataSource = Nothing
                    gstrErrMsg = "Please enter a valid Packaging Line."
                    MessageBox.Show(gstrErrMsg, "Invalid information")
                    txtPkgLine.Focus()
                Else
                    lblPkgLine.Visible = True
                    lblPkgLine.Text = SharedFunctions.GetEquipmentDescription(gdrCmpCfg("Facility"), sender.text) 'WO#359
                    'Fill the data grid view with Shop Order and Item information.
                    dgvSOSchedule.DataSource = CPPspShopOrderIOBindingSource
                    CPPsp_ShopOrderIOTableAdapter.Fill(DsShopOrder.CPPsp_ShopOrderIO, "GetSOList", gdrSessCtl.Facility, 0, txtPkgLine.Text)
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub txtPkgLine_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtPkgLine.Validating
        Dim tblSO As New dsShopOrder.CPPsp_ShopOrderIODataTable

        If Me.ActiveControl.Name = "btnPrvScn" Then
            Me.Close()
        Else
            If Not IsNothing(gstrErrMsg) Then
                MessageBox.Show(gstrErrMsg, "Invalid information")
            End If
        End If
    End Sub

    Private Sub popupAlphaNumKB(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtPkgLine.MouseDown
        Dim dgrKeyPad As DialogResult
        Try
            If TypeOf sender Is TextBox Or (sender.name = "cboShopOrder" And gblnDropDownIsClicked = False) Then
                dgrKeyPad = SharedFunctions.PopAlphaNumKB(Me, sender)
                If dgrKeyPad = Windows.Forms.DialogResult.OK Then
                    If Not IsNothing(gstrNumPadValue) AndAlso gstrNumPadValue <> "" Then
                        sender.text = Microsoft.VisualBasic.Left(gstrNumPadValue, sender.maxLength)
                    Else
                        sender.text = ""
                    End If
                End If
            End If
            If TypeOf sender Is ComboBox Then
                gblnDropDownIsClicked = False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class