Public Class frmInitialPgm

    Const cstStdMsg As String = "Please select the 'Configure IPC' or 'Configure IPC Viewer' button to configure this spare IPC to a specify line first."

    Private Sub frmInitialPgm_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        Dim drCC As dsComputerCfg.tblComputerConfigRow
        btnPalletStation.BackColor = Color.OldLace
        btnRunIndusoft.BackColor = Color.OldLace
        btnIPCViewer.BackColor = Color.OldLace
        btnNoIndusoft.BackColor = Color.OldLace
        Using taCC As New dsComputerCfgTableAdapters.tblComputerConfigTableAdapter
            Using dtCC As New dsComputerCfg.tblComputerConfigDataTable
                taCC.FillByComputer(dtCC, "SelectAllFields", My.Computer.Name, Nothing)
                If dtCC.Rows.Count > 0 Then
                    drCC = dtCC.Rows(0)
                    With drCC
                        If .PalletStation Then
                            btnPalletStation.BackColor = Color.GreenYellow
                        ElseIf Not IsNothing(.IndusoftPgmName) Then
                            If LCase(.PkgLineType) = "ipcviewer" Then
                                btnIPCViewer.BackColor = Color.GreenYellow
                            Else
                                btnRunIndusoft.BackColor = Color.GreenYellow
                            End If
                        ElseIf LCase(.PackagingLine).TrimEnd <> "spare" Then
                            btnNoIndusoft.BackColor = Color.GreenYellow
                        End If
                    End With
                End If
            End Using
        End Using
    End Sub

    Private Sub frmInitialPgm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.UcHeading1.lblScreenTitle.Text = "Select IPC Function"
    End Sub

    Private Sub btnPalletStation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPalletStation.Click
        Dim strErrMsg As String = Nothing
        Dim strUserID, strPassword As String
        Try
            If myUtilities.IsConfigured(RunAs.PalletStation) Then
                strErrMsg = myUtilities.IsSelectedRight(My.Computer.Name, RunAs.PalletStation)
                If strErrMsg <> String.Empty Then
                    MessageBox.Show(strErrMsg, "Selected improper Option")
                Else
                    strUserID = "ipcuser" & My.Settings.gstrPalletStationAccount
                    strPassword = strUserID
                    myUtilities.SetDefaultProfile(strUserID, strPassword)

                    System.Diagnostics.Process.Start(My.Settings.strDotNetPgm, "logon")
                    Application.Exit()
                End If
            Else
                MessageBox.Show(cstStdMsg, "Warning")
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnRunIndusoft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunIndusoft.Click
        'Dim strFileName As String
        Dim strErrMsg As String = Nothing
        Dim strUserID, strPassword As String
        Try
            If myUtilities.IsConfigured(RunAs.HasIndusoft) Then
                strErrMsg = myUtilities.IsSelectedRight(My.Computer.Name, RunAs.HasIndusoft)
                If strErrMsg <> String.Empty Then
                    MessageBox.Show(strErrMsg, "Selected improper Option")
                Else
                    strUserID = "ipcuser" & My.Settings.gstrIWSDftAccount
                    strPassword = strUserID
                    myUtilities.SetDefaultProfile(strUserID, strPassword)

                    'strFileName = """" & My.Settings.strInduSoftRunTime & """ """ & My.Settings.strInduSoftPgm & """"
                    System.Diagnostics.Process.Start(My.Settings.strInduSoftStartUpPgm)
                    Application.Exit()
                End If
            Else
                MessageBox.Show(cstStdMsg, "Warning")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnNoIndusoft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoIndusoft.Click
        Dim strErrMsg As String = Nothing
        Dim strUserID, strPassword As String
        Try
            If myUtilities.IsConfigured(RunAs.HasNoIndusoft) Then
                strErrMsg = myUtilities.IsSelectedRight(My.Computer.Name, RunAs.HasNoIndusoft)
                If strErrMsg <> String.Empty Then
                    MessageBox.Show(strErrMsg, "Selected improper Option")
                Else
                    strUserID = "ipcuser" & My.Settings.gstrNoIWSDftAccount
                    strPassword = strUserID
                    myUtilities.SetDefaultProfile(strUserID, strPassword)

                    System.Diagnostics.Process.Start(My.Settings.strDotNetPgm, "logon_NoPLC")
                    Application.Exit()
                End If
            Else
                MessageBox.Show(cstStdMsg, "Warning")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnIPCViewer_Click(sender As Object, e As EventArgs) Handles btnIPCViewer.Click
        Dim strErrMsg As String = Nothing
        Dim strUserID, strPassword As String
        Try
            If myUtilities.IsConfigured(RunAs.IPCViewer) Then
                strErrMsg = myUtilities.IsSelectedRight(My.Computer.Name, RunAs.IPCViewer)
                If strErrMsg <> String.Empty Then
                    MessageBox.Show(strErrMsg, "Selected improper Option")
                Else
                    strUserID = "ipcview" & My.Settings.gstrIPCViewerAccount
                    strPassword = "ipcview$" & My.Settings.gstrIPCViewerAccount
                    myUtilities.SetDefaultProfile(strUserID, strPassword)

                    System.Diagnostics.Process.Start(My.Settings.strIPCViewerPgm)
                    Application.Exit()
                End If
            Else
                strErrMsg = "Please select the 'Configure IPC Viewer' button to configure this spare IPC to a specify IPC Viewer first."
                MessageBox.Show(strErrMsg, "Warning")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnShupDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShupDown.Click
        'Dim ProcID As Integer
        Try
            frmShutDown.Show()
            'Application.Exit()
            'System.Diagnostics.Process.Start("Shutdown", "/s")
            '' Start the Windows ShutDwon, and store the process id.
            'ProcID = Shell("Shutdown", AppWinStyle.NormalFocus)
            '' Activate the shutdown.
            'AppActivate(ProcID)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCfgIPC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCfgIPC.Click
        Try
            If My.Computer.Network.IsAvailable = True Then
                'myUtilities.SyncIPCCfgData(My.Computer.Name)        'WO#16894
                frmCfgIPC.ShowDialog()
            Else
                MessageBox.Show("Please connect the IPC to the network and try again.")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCfgIPCViewer_Click(sender As Object, e As EventArgs) Handles btnCfgIPCViewer.Click
        Try
            If My.Computer.Network.IsAvailable = True Then
                'myUtilities.SyncIPCCfgData(My.Computer.Name)        'WO#16894
                frmCfgIPCViewer.ShowDialog()
            Else
                MessageBox.Show("Please connect the IPC to the network and try again.")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnTSCtlPnl_Click(sender As Object, e As EventArgs) Handles btnTSCtlPnl.Click
        Try
            System.Diagnostics.Process.Start(My.Settings.strDotNetPgm, "tscontrolpanel")
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnInquiry_Click(sender As Object, e As EventArgs) Handles btnInquiry.Click
        Try
            System.Diagnostics.Process.Start(My.Settings.strDotNetPgm, "inquiry")
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
