Module modDownTime
    Public gstrNumPadValue As String
    Public gstrErrMsg As String
    Public gtaDTReasonType As New dsDTReasonTypeTableAdapters.tblDTReasonTypeTableAdapter
    Public gtaPlantStaff As New dsPlantStaffTableAdapters.CPPsp_PlantStaffingIOTableAdapter
    Public gtaDTReasonCode As New dsDTReasonCodeTableAdapters.tblDTReasonCodeTableAdapter
    'Public gtaControl As New dsControlTableAdapters.CPPsp_ControlIOTableAdapter
    'Public gtaComputerConfig As New dsComputerConfigTableAdapters.tblComputerConfigTableAdapter
    Public gtaDownTimeLog As New dsDownTimeLogTableAdapters.CPPsp_DownTimeLog_SelTableAdapter

    Public gstrMachineType As String
    Public gstrMachineID As String
    Public gintShopOrder As Integer
    Public gstrOperator As String
    Public gintShift As Short
    Public gstrFacility As String
    'Public gstrPkgLineType As String
    Public gstrServerCnnStr As String
    Public gstrLocalCnnStr As String
    Public gblnConnectUp As Boolean
    Public gstrMachineSubType As String
    Public gstrEventID As String
    Public Const gcstSvrCnnFailure As String = "No Server connection, records on server may not be viewed."
    Public gblnCommentRequired As Boolean               'WO#3282
    Public gstrComment As String = ""                   'WO#3282
    Public gblnCancelComment As Boolean                 'WO#3282
    Public gstrLogViewOption As String = String.Empty   'WO#14867

    Public Function Main(ByVal CmdArgs() As String) As Integer
        'Dim i As Integer
        'Dim intCounter As Integer
        'Dim processList() As Process
        Dim strConnStr As String
        Dim i As Short
        Dim pInstance As Process = Nothing
        'Dim strLocalConnStr As String
        Try
            Cursor.Current = Cursors.WaitCursor

            'make sure only one instant of the program can be run in a computer       
            For i = 1 To 4
                pInstance = SharedFunctions.GetRunningInstance(Process.GetCurrentProcess().ProcessName)
                If Not pInstance Is Nothing Then
                    System.Threading.Thread.Sleep(1000)
                Else
                    Exit For
                End If
            Next

            If Not pInstance Is Nothing Then
                Dim handle As IntPtr = pInstance.MainWindowHandle

                If Not IntPtr.Zero.Equals(handle) Then
                    'WO#650 Win32Helper.ShowWindow(handle, 1)
                    Win32Helper.SetForegroundWindow(handle)
                    Win32Helper.ShowWindow(handle, ProcessWindowStyle.Maximized)     'WO#650 
                End If
                Application.ExitThread()
            Else
                'processList = Process.GetProcesses
                'For i = 0 To processList.Length - 1
                '    If processList(i).ProcessName = "DownTime" Then
                '        intCounter += 1
                '    End If
                'Next
                'If intCounter > 1 Then
                '    MessageBox.Show("The DownTime application has been opened.", "Down Time Log Application")
                '    Return 1
                'End If

                If My.Application.CommandLineArgs.Count < 7 Then
                    MessageBox.Show("Require 7 parameters and the 8th one is optional.", "Down Time Log Application")
                    Return 1
                End If
                gstrMachineType = UCase(My.Application.CommandLineArgs(0))
                gstrMachineID = My.Application.CommandLineArgs(1)
                gintShopOrder = My.Application.CommandLineArgs(2)
                gstrOperator = My.Application.CommandLineArgs(3)
                gintShift = My.Application.CommandLineArgs(4)
                gstrFacility = My.Application.CommandLineArgs(5)
                gstrMachineSubType = My.Application.CommandLineArgs(6)
                If My.Application.CommandLineArgs.Count > 7 Then
                    gstrEventID = My.Application.CommandLineArgs(7)
                    If gstrMachineType = "P" Then
                        gstrEventID = gstrEventID.Insert(10, " ")
                    End If
                End If
                strConnStr = My.Settings.gstrLocalCnnStr
                strConnStr = Replace(strConnStr, "ComputerName", My.Computer.Name)
                gstrLocalCnnStr = strConnStr
                gstrServerCnnStr = My.Settings.gstrServerCnnStr

                'If the system does not has local MS SQL instance, every connection uses server connection
                If String.IsNullOrEmpty(strConnStr) Then
                    strConnStr = gstrServerCnnStr
                End If

                gtaDTReasonType.Connection.ConnectionString = strConnStr
                gtaDTReasonCode.Connection.ConnectionString = strConnStr
                gtaPlantStaff.Connection.ConnectionString = strConnStr
                'gtaControl.Connection.ConnectionString = strConnStr
                'gtaComputerConfig.Connection.ConnectionString = strConnStr

                If SharedFunctions.IsSvrConnOK Then
                    strConnStr = gstrServerCnnStr
                    gblnConnectUp = True
                Else
                    gblnConnectUp = False
                End If

                gtaDownTimeLog.Connection.ConnectionString = strConnStr

                'gtaControl.Fill(tblControl, "Facility", "General")
                'gtaComputerConfig.Fill(tblComputerConfig, My.Computer.Name)
                'If tblComputerConfig.Rows.Count > 0 Then
                '    gstrFacility = tblComputerConfig.Rows(0)("Facility")
                '    If gstrMachineType = "P" Then
                '        gstrMachineSubType = tblComputerConfig.Rows(0)("PkgLineType")
                '    End If
                'End If

                If gblnConnectUp = True And Not String.IsNullOrEmpty(gstrLocalCnnStr) Then
                    SharedFunctions.uploadDTLogToServer()
                End If

                Cursor.Current = Cursors.Default
                frmDownTime.ShowDialog()
                Return (0)
            End If

        Catch ex As SqlClient.SqlException
            If ex.ErrorCode = -2146232060 And (ex.Number = 64 Or ex.Number = 1231 Or ex.Number = 11001) And String.IsNullOrEmpty(gstrLocalCnnStr) Then
                MessageBox.Show("Data base server connection failed. Please contact supervisor and try again later.")
            Else
                MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor")
            End If
            Return (1)

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor")
            Return (1)
        Finally
            Cursor.Current = Cursors.Default
        End Try
    End Function
End Module
