Imports System
Imports System.linq

Public Class frmViewDTLog
    'WO#14866 ADD Start
    Const cstByShift As String = "By Shift"
    Const cstLastX As String = "Last 12"
    Const cstPackagingLine As String = "P"
    Dim dteShiftProdDate As DateTime
    'WO#14866 ADD Stop

    Private Sub frmViewDTLog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim drSC As dsSessionControl.CPPsp_SessionControlIORow                'WO#14866
        Try
            'On the first load of the application, shows items By shift, otherwise keep the same view from last run
            If gstrLogViewOption = String.Empty Then
                Me.UcHeading1.ScreenTitle = "Down Time " & cstLastX
                btnOption.Text = cstByShift
            Else
                'Assign the last log view option as current option text
                btnOption.Text = gstrLogViewOption
            End If

            If gblnConnectUp = False Then
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
            Else
                SharedFunctions.RmvMessageLineFromForm(Me)
            End If

            'WO#14866 ADD Start
            If gstrMachineType = "P" Then
                drSC = SharedFunctions.GetSessionControl()
                dteShiftProdDate = drSC.ShiftProductionDate
            Else
                dteShiftProdDate = SharedFunctions.GetProductionDateByShift(gstrFacility, gintShift, Now, gstrMachineID, gstrMachineType)
            End If


            btnOption_Click(sender, e)
            'gtaDownTimeLog.Fill(DsDownTimeLog.CPPsp_DownTimeLog_Sel, "CurrentSessionTopX", gstrFacility, gstrMachineType, Nothing, gstrMachineID, Nothing, False, Nothing, Nothing)
            ' display available option - By Shift
            'btnOption.Text = cstByShift
            'WO#14866 ADD Stop

            'WO#14866   gtaDownTimeLog.Fill(DsDownTimeLog.CPPsp_DownTimeLog_Sel, "CurrentSessionTopX", gstrFacility, gstrMachineType, Nothing, gstrMachineID, Nothing, False)   'WO#648
            'WO#648 gtaDownTimeLog.Fill(Me.DsDownTimeLog.tblDownTimeLog, gstrFacility, gstrMachineType, gstrMachineID)
        Catch ex As SqlClient.SqlException
            If ex.ErrorCode = -2146232060 And (ex.Number = 64 Or ex.Number = 1231 Or ex.Number = 11001) And Not String.IsNullOrEmpty(gstrLocalCnnStr) Then
                gtaDownTimeLog.Connection.ConnectionString = gstrLocalCnnStr
                SharedFunctions.AddMessageLineToForm(Me, gcstSvrCnnFailure)
                gblnConnectUp = False
            Else
                MessageBox.Show("Error in frmViewDTLog_Load" & Environment.NewLine & ex.Message)
            End If
        Catch ex As System.Exception
            System.Windows.Forms.MessageBox.Show("Error in frmViewDTLog_Load" & Environment.NewLine & ex.Message)
        End Try
    End Sub
    'WO#14866 ADD Start
    Private Sub btnOption_Click(sender As Object, e As EventArgs) Handles btnOption.Click
        'Save the veiw option was selected currently
        gstrLogViewOption = btnOption.Text

        'By last X
        If btnOption.Text = cstLastX Then
            'set next available view option
            btnOption.Text = cstByShift
            txtShiftDuration.Visible = False
            lblShiftDuration.Visible = False
            gtaDownTimeLog.Fill(DsDownTimeLog.CPPsp_DownTimeLog_Sel, "CurrentSessionTopX", gstrFacility, gstrMachineType, Nothing, gstrMachineID, Nothing, False, Nothing, Nothing)
            UcHeading1.ScreenTitle = "Down Time " & cstLastX
        Else        'By shift
            'set next available view option
            btnOption.Text = cstLastX
            gtaDownTimeLog.Fill(DsDownTimeLog.CPPsp_DownTimeLog_Sel, "ByShift", gstrFacility, gstrMachineType, Nothing, gstrMachineID, Nothing, False, gintShift, dteShiftProdDate)
            txtShiftDuration.Text = GetShiftDTDuration.ToString
            txtShiftDuration.Visible = True
            lblShiftDuration.Visible = True
            UcHeading1.ScreenTitle = "Down Time " & cstByShift
        End If

        Me.Refresh()
    End Sub
    ' WO#14867 ADD Start – AT 12/19/2018
    Private Function GetShiftDTDuration() As Integer

        ' Dim dvLog As DataView
        ' Dim dtLog As DataTable
        Dim intDurationInMinutes As Integer = 0
        Try
            For Each v As String In
                From row
                In dgvDownTimeLog.Rows
                Group By val = DirectCast(row, DataGridViewRow).Cells("dvgTxtDTBegin").Value
                Into MaxValue = Max(CInt(DirectCast(row, DataGridViewRow).Cells("dgvTxtDuration").Value))
                Select Str = MaxValue.ToString()
                intDurationInMinutes = intDurationInMinutes + CInt(v)
            Next

            '            dvLog = DsDownTimeLog.Tables("CPPsp_DownTimeLog_Sel").DefaultView
            '            If dvLog.Count > 0 Then
            ' dvLog.Sort = "Duration"
            ' dtLog = dvLog.ToTable()
            ' intDurationInMinutes = dtLog.Rows(dtLog.Rows.Count - 1).Item("Duration")
            ' End If
        Catch ex As Exception
            Throw New Exception("Error in GetShiftDTDuration" & vbCrLf & ex.Message)
        End Try
        Return intDurationInMinutes
    End Function
    ' WO#14867 ADD Stop – AT 12/19/2018
    'WO#14866 ADD Stop

    Private Sub btnPrvScn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub
End Class