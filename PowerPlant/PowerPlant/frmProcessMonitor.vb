Public Class frmProcessMonitor
    Dim gstrErrMsg As String

    Private Sub frmProcessMonitor_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Timer1.Stop()
    End Sub

    Private Sub frmMachineEfficiency_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim daCT As New dsControlTableTableAdapters.PPsp_Control_SelTableAdapter
        Dim dtCT As New dsControlTable.PPsp_Control_SelDataTable
        Dim intSOActProduced, intSOActCsPerHour, intSOSchCsPerHour, intSOProgress As Integer
        Dim sngSOEfficiency, sngSftEfficiency As Single
        Dim blnSOAlert, blnSftAlert As Boolean
        Dim intSftActProduced, intsftActCsPerHour, intSftSchProduced, intSftSchCsPerHour, intSftProgress As Integer
        'Dim dteStopTime As New System.Nullable(Of Date)
        Try
            Me.UcHeading1.lblScreenTitle.Text = "Process Monitor"

            btnPrvScn.Focus()
            'Initialize the label values 
            lblSO_Efficiency.Text = 0.0
            lblSO_Progress.Text = 0
            lblSOAct_CaseProd.Text = 0
            lblSOAct_CsPerHr.Text = 0
            lblSOSch_CaseProd.Text = 0
            lblSOSch_CsPerHr.Text = 0

            lblShift_Efficiency.Text = 0.0
            lblShift_Progress.Text = 0
            lblShiftAct_CaseProd.Text = 0
            lblShiftAct_CsPerHr.Text = 0
            lblShiftSch_CaseProd.Text = 0
            lblShiftSch_CsPerHr.Text = 0

            With gdrSessCtl
                lblOperator.Text = SharedFunctions.GetStaffName(.Operator, "P")
                lblPkgLine.Text = SharedFunctions.GetEquipmentDescription(.Facility, .DefaultPkgLine)
                lblShift.Text = String.Format("-- {0} --Shift #{1} --", Format(.ShiftProductionDate, "MMM.dd"), .OverrideShiftNo.ToString)
                SharedFunctions.GetLineEfficiency(.Facility, .DefaultPkgLine, .Operator, .OverrideShiftNo, .ShopOrder, Now(), .StartTime, .DefaultPkgLine, .OverridePkgLine, .ShopOrder, _
                                .ItemNumber, .Operator, .DefaultShiftNo, .OverrideShiftNo, .CasesScheduled, .CasesProduced, .PalletsCreated, .LooseCases, .ProductionDate, .CarriedForwardCases, .ShiftProductionDate, intSOActProduced, intSOActCsPerHour, _
                                intSOSchCsPerHour, sngSOEfficiency, blnSOAlert, intSOProgress, intSftActProduced, intsftActCsPerHour, intSftSchProduced, intSftSchCsPerHour, sngSftEfficiency, blnSftAlert, intSftProgress)

                lblShift_Efficiency.Text = Math.Round(sngSftEfficiency, 2)
                lblShift_Progress.Text = intSftProgress
                lblShiftAct_CaseProd.Text = intSftActProduced
                lblShiftAct_CsPerHr.Text = intsftActCsPerHour
                lblShiftSch_CaseProd.Text = intSftSchProduced
                lblShiftSch_CsPerHr.Text = intSftSchCsPerHour

                If .ShopOrder <> 0 Then
                    lblSO_Efficiency.Text = Math.Round(sngSOEfficiency, 2)
                    lblSO_Progress.Text = intSOProgress
                    lblSOAct_CaseProd.Text = intSOActProduced
                    lblSOAct_CsPerHr.Text = intSOActCsPerHour
                    lblSOSch_CaseProd.Text = .CasesScheduled
                    lblSOSch_CsPerHr.Text = intSOSchCsPerHour
                End If

                If intSOProgress < 0 Then
                    lblSO_Progress.ForeColor = Color.Crimson
                Else
                    lblSO_Progress.ForeColor = Color.GreenYellow
                End If

                If intSftProgress < 0 Then
                    lblShift_Progress.ForeColor = Color.Crimson
                Else
                    lblShift_Progress.ForeColor = Color.GreenYellow
                End If

                If blnSOAlert Then
                    Me.lblSO_Efficiency.ForeColor = Color.Crimson
                Else
                    Me.lblSO_Efficiency.ForeColor = Color.GreenYellow
                End If

                If blnSftAlert Then
                    Me.lblShift_Efficiency.ForeColor = Color.Crimson
                Else
                    Me.lblShift_Efficiency.ForeColor = Color.GreenYellow
                End If

            End With


            'if the calling form is Inquiry, do not start timer for closing the screen automatically
            If gstrPrvForm <> "frmInquiry" Then
                daCT.Fill(dtCT, "AutoOffTime", "fmProcessMonitor", "ByKey_SubKey")
                If dtCT.Rows.Count > 0 Then
                    Timer1.Interval = CType(dtCT.Rows(0).Item("Value1"), Integer)
                Else
                    Timer1.Interval = 5000
                End If

                Timer1.Start()
            Else
                Timer1.Stop()
            End If
            gstrPrvForm = Me.Name
        Catch ex As SqlClient.SqlException When (ex.Number = 64 Or ex.Number = 1231) And gblnSvrConnIsUp = True
            SharedFunctions.SetServerCnnStatusInSessCtl(False)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub btnPrvScn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.Close()
    End Sub
End Class