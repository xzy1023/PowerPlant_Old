Imports System.ServiceProcess
Imports System.Threading
Imports System.Reflection

Public Class LabelPrinting
    Private thdMain As Thread
    'all the threads which calls the WaitOne method will block until some thread calls the Set() method.
    'Dim mreShutdownEvent As ManualResetEvent = New ManualResetEvent(False)
    'all the threads which calls the WaitOne method will not block and free to proceed further
    Dim mrePauseEvent As ManualResetEvent = New ManualResetEvent(True)
    Dim blnOnStop As Boolean = False
    Dim strApplicationName As String = Me.ToString
    Dim objCoLOS As clsCoLOSCLI
    Const cstResponseOK As String = "OK|0000|"

    '*********
    '* Note: *
    '*********
    ' Program is started from the "Shared Sub Main()" defined in srvLabelPrinting.Designer.vb file. 
    ' The code is similar as below.
    ' It is specified in the "Startup Object" in the application of "My Project"
    ' To Debug: Change the application type from Windows Service to "Console Application" in Visual Studtio - My Project

    'Private Shared Sub Main()
    '    Dim servicesToRun As ServiceBase()
    '    servicesToRun = New ServiceBase() {New LabelPrinting()}

    '    If Environment.UserInteractive Then
    '        RunInteractive(servicesToRun)
    '    Else
    '        ServiceBase.Run(servicesToRun)
    '    End If
    'End Sub

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
        Dim strAppCtrResult As String()
        Dim strCoLOSIP As String = Nothing
        Dim intCOLOSPort As Integer
        Dim strResponse As String = String.Empty
        Dim strMsg As String = String.Empty
        Dim blnPgmAbnormalEnd As Boolean = False       'Fix 20211018
        Dim decA As Decimal
        Dim decB As Decimal = 1
        Dim decC As Decimal = 0

        Try
            ' decA = decB / decC
            'Get IP address of CoLOS server
            strAppCtrResult = SharedFunctions.GetConrolTableValues("CoLOS_Server", "General")
            strCoLOSIP = strAppCtrResult(0)

            'get default TCP/IP port for CoLOS CLI server
            intCOLOSPort = My.Settings.intCoLOSPort

            'Create a CoLOS instance
            objCoLOS = New clsCoLOSCLI()
            With objCoLOS
                .IPAddress = strCoLOSIP
                .Port = intCOLOSPort

                'Establish a TCP/IP connection with the CoLOS CLI server
                strResponse = .TCPConnect
                If IsNothing(strResponse) Then
                    'Try to log on to CoLOS CLI
                    strResponse = String.Empty
                    strResponse = .LogInToCoLOS()
                    'Log the CoLOS response to application Event log
                    If strResponse.Contains(cstResponseOK) Then
                        EventLog.WriteEntry(String.Format(strResponse.Replace(vbCr, "").Replace(vbLf, "").TrimEnd(" ", "") & " - Login to CoLOS CLI server successfully."), EventLogEntryType.SuccessAudit)
                    Else
                        strMsg = "Error: " & strResponse.Replace(vbCr, "").Replace(vbLf, "").TrimEnd(" ", "") & ". Unable to Login to print server, CoLOS CLI. Please contact administrator."
                        'EventLog.WriteEntry(String.Format(strMsg), EventLogEntryType.Error)
                        Throw New Exception(strMsg)
                    End If
                Else
                    'If network connection is not ready, throw an exception
                    strMsg = "Error: " & strResponse.Replace(vbCr, "").Replace(vbLf, "").TrimEnd(" ", "") & ". Network connection is not ready."
                    'EventLog.WriteEntry(String.Format(strMsg), EventLogEntryType.Error)
                    Throw New Exception(strMsg)
                End If
            End With

            'Create a new thread to run ProcessPrintRequest routine
            thdMain = New Thread(AddressOf ProcessPrintRequest)
            thdMain.IsBackground = True
            thdMain.Start()
            EventLog.WriteEntry(strApplicationName & " started.", EventLogEntryType.Information)

        Catch ex As Exception
            Try
                strMsg = ex.Message & vbCrLf & ex.StackTrace
                EventLog.WriteEntry(strApplicationName & " " & strMsg, EventLogEntryType.Error)
                ' EventLog.WriteEntry("Send Message: " & ex.Message, EventLogEntryType.Information)
                'SharedFunctions.SendEmail(strMsg, strApplicationName & " is failure.")
            Catch SndEmailEx As Exception
                EventLog.WriteEntry("Monitoring Send Email error - " & SndEmailEx.Message & vbCrLf & SndEmailEx.StackTrace, EventLogEntryType.Error)
            End Try
            blnPgmAbnormalEnd = True           'Fix 20211018
            'Fix 20211018   Throw ex
            'Fix 20211018 ADD Start
        Finally

            If blnPgmAbnormalEnd = True Then
                Throw New System.Exception("Unexpected Error in OnStart - Check log in Event Viewer.")
            End If
            'Fix 20211018 ADD Stop
        End Try

    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        Dim strResponse As String = String.Empty
        Dim strMsg As String = String.Empty

        Try
            'All the waiting threads are blocked and stop to proceed further.
            mrePauseEvent.Reset()
            blnOnStop = True

            Threading.Thread.Sleep(2000)

            'Log off from CoLOS CLI
            strResponse = objCoLOS.LogOutFromCoLOS()
            If strResponse.Contains(cstResponseOK) Then
                'Log out successfully
                EventLog.WriteEntry(String.Format(strResponse.Replace(vbCr, "").Replace(vbLf, "").TrimEnd(" ", "") & " - CoLOS CLI successfully."), EventLogEntryType.SuccessAudit)
            Else
                'Unable to Log out from CoLOS server
                strMsg = "Error: " & strResponse.Replace(vbCr, "").Replace(vbLf, "").TrimEnd(" ", "") & ". Unable to Log out from CoLOS CLI server. Please contact administrator."
                EventLog.WriteEntry(String.Format(strMsg), EventLogEntryType.FailureAudit)
            End If

            'Close the main thread
            If thdMain.IsAlive Then
                thdMain.Abort()
            End If

            'Disconnect the TCP/IP connection with COLOS server
            objCoLOS.TCPDisconnect()

            EventLog.WriteEntry(strApplicationName & " Stopped.", EventLogEntryType.Information)

        Catch ex As Exception
            EventLog.WriteEntry(strApplicationName & "Error in OnStop - " & ex.Message, EventLogEntryType.Error)
        End Try
    End Sub

    Protected Overrides Sub OnPause()
        Try
            'set the mrePauseEvent to false. All the waiting threads are blocked.
            mrePauseEvent.Reset()

            EventLog.WriteEntry(strApplicationName & " Paused.", EventLogEntryType.Information)
            'SharedFunctions.LogPrintEvent(strApplicationName & " Paused.")
        Catch ex As Exception
            EventLog.WriteEntry(strApplicationName & "Error in OnPause - " & ex.Message, EventLogEntryType.Error)
        End Try
    End Sub

    Protected Overrides Sub OnContinue()
        Try
            'set the mrePauseEvent to true. All the waiting threads are unblocked and proceed further
            mrePauseEvent.Set()
            EventLog.WriteEntry(strApplicationName & " Resumed.", EventLogEntryType.Information)

        Catch ex As Exception
            EventLog.WriteEntry(strApplicationName & "Error in OnContinue - " & ex.Message, EventLogEntryType.Error)
        End Try
    End Sub

    Private Shared Sub RunInteractive(ByVal servicesToRun As ServiceBase())
        Dim strInput As String = String.Empty
        Dim onMethod As MethodInfo
        Try
            Dim onStartMethod As MethodInfo = GetType(ServiceBase).GetMethod("OnStart", BindingFlags.Instance Or BindingFlags.NonPublic)

            For Each service As ServiceBase In servicesToRun
                onStartMethod.Invoke(service, New Object() {New String() {}})
            Next

            Console.WriteLine("Press 's' to stop the services, 'p' to pause and 'c' to continue the process...")

            Do While (True)
                strInput = Console.ReadLine()
                Console.WriteLine("Entered " & strInput)

                Select Case strInput.ToLower
                    Case "p"
                        onMethod = GetType(ServiceBase).GetMethod("OnPause", BindingFlags.Instance Or BindingFlags.NonPublic)

                        For Each service As ServiceBase In servicesToRun
                            Console.Write("Paused {0}...", service.ServiceName)
                            onMethod.Invoke(service, Nothing)
                            Console.WriteLine("Paused")
                        Next
                    Case "c"
                        onMethod = GetType(ServiceBase).GetMethod("OnContinue", BindingFlags.Instance Or BindingFlags.NonPublic)

                        For Each service As ServiceBase In servicesToRun
                            Console.Write("Resume {0}...", service.ServiceName)
                            onMethod.Invoke(service, Nothing)
                            Console.WriteLine("Resumed")
                        Next
                    Case "s"
                        onMethod = GetType(ServiceBase).GetMethod("OnStop", BindingFlags.Instance Or BindingFlags.NonPublic)

                        For Each service As ServiceBase In servicesToRun
                            Console.Write("Stopping {0}...", service.ServiceName)
                            onMethod.Invoke(service, Nothing)
                            Console.WriteLine("Stopped")
                        Next
                        Exit Do
                    Case Else
                        'It is invalid input or the program is running the service interactively so the strInput is nothing
                        'Therefore, it will be nothing to be display on the console.
                        Throw New Exception("Running the service interactively")
                        '
                        ' Console.WriteLine("All services stopped.")
                        Exit Do
                End Select
            Loop
        Catch ex As Exception
            Try
                Throw New Exception("Error in RunInteractive" & vbCrLf & ex.Message & vbCrLf & ex.StackTrace)
            Catch
            End Try
        End Try
    End Sub

    Public Sub ProcessPrintRequest()
        Dim intNoOfCaseLabelCopies As Int32
        Dim strAppCtrResult As String()
        Try
            If Me.ServiceName <> String.Empty Then
                strApplicationName = Me.ServiceName
            End If
            'Get default number of copies of case label 
            strAppCtrResult = SharedFunctions.GetConrolTableValues("CaseLabelCopies", "General")
            intNoOfCaseLabelCopies = strAppCtrResult(0)

            Do While True
                'blocks the current thread, mrePauseEvent and wait for the signal by other thread.
                mrePauseEvent.WaitOne(Timeout.Infinite)
                'if current thread, mreShutdownEvent is not blocked, run PrintLabels routine.
                'If Not mreShutdownEvent.WaitOne(0) Then
                PrintLabels(intNoOfCaseLabelCopies, objCoLOS)
                'End If
                Threading.Thread.Sleep(2000)
            Loop

        Catch ex As Exception
            If blnOnStop = False Then
                EventLog.WriteEntry(strApplicationName & ": error in ProcessPrintRequest - " & ex.Message & vbCrLf & ex.StackTrace, EventLogEntryType.Error)
                Try
                    EventLog.WriteEntry("Send Message: " & ex.Message, EventLogEntryType.Information)
                    SharedFunctions.SendEmail(ex.Message, strApplicationName & " is failure.")
                Catch SndEmailEx As Exception
                    EventLog.WriteEntry("Monitoring Send Email error - " & SndEmailEx.Message & vbCrLf & SndEmailEx.StackTrace, EventLogEntryType.Error)
                End Try
            Else
                Try
                    EventLog.WriteEntry(strApplicationName & " - stopping.", EventLogEntryType.Information)
                Catch
                End Try
            End If
        End Try
    End Sub

    Private Sub PrintLabels(intDftNoOfCaseLabelCopies As Integer, objCoLOS As clsCoLOSCLI)
        Dim dtPrintRequests As New dsPrintRequests.PrintRequestDataTable
        Dim taPrintRequests As New dsPrintRequestsTableAdapters.PrintRequestTableAdapter
        Dim qtaPrintRequests As New dsPrintRequestsTableAdapters.QueriesTableAdapter
        Dim drPrintRequests As dsPrintRequests.PrintRequestRow

        Dim taUpdatePallet As New dsPalletTableAdapters.qtaPallet

        Dim strResponse As String = String.Empty
        Dim strMsg As String = String.Empty
        Dim strPrintCommand As String = String.Empty
        Dim intNoOfCopies As Integer
        Dim strPrintResult As String = String.Empty
        Dim strCopies As String = String.Empty
        Dim strLabelType As String = String.Empty
        Dim strJobName As String = String.Empty

        Const cstCase As String = "C"
        Const cstSelectMode As String = "DownloadAndSelect"

        Try
            taPrintRequests.Fill(dtPrintRequests)
            If dtPrintRequests.Rows.Count > 0 Then

                For Each drPrintRequests In dtPrintRequests.Rows
                    If mrePauseEvent.WaitOne(0) Then
                        With drPrintRequests
                            If .DeviceType = cstCase Then
                                If .NoOfCopies > 0 Then
                                    intNoOfCopies = .NoOfCopies
                                Else
                                    intNoOfCopies = intDftNoOfCaseLabelCopies
                                End If
                                strCopies = String.Format("|@Copies={0}", intNoOfCopies)
                            Else
                                strCopies = String.Empty
                                intNoOfCopies = 0
                            End If
                            strPrintCommand = String.Format("devices|lookup|{0}|select|{1}|@SelectMode={2}{3}", .DeviceName, .JobName.TrimEnd, cstSelectMode, strCopies)
                            'strPrintCommand = String.Format("devices|lookup|{0}|select|{1}|@SelectMode={2}{3}", "Z_Test", .JobName.TrimEnd, cstSelectMode, strCopies)
                            strMsg = String.Format("Job Name: {0}{1} Device: {2}{3} Type: {4}, Copies: {5}", .JobName.TrimEnd, vbTab, .DeviceName, vbTab, .DeviceType, intNoOfCopies)
                            SharedFunctions.LogPrintEvent(String.Format("{0}:{1}{2}.", strMsg, vbTab, strPrintCommand))
                            'Send CLI command to CoLOS to print the label
                            strResponse = String.Empty
                            strResponse = objCoLOS.SendMessage(strPrintCommand)
                            'SharedFunctions.LogPrintEvent(strMsg & vbTab & String.Format("{0}: {1}.", strPrintResult, strResponse.Replace(vbCr, "").Replace(vbLf, "").TrimEnd(" ", "")))
                            SharedFunctions.LogPrintEvent(String.Format("{0}:{1}{2}.", strMsg, vbTab, strResponse.Replace(vbCr, "").Replace(vbLf, "").TrimEnd(" ", "")))

                            'Delete the processed record from print request table no matter it was printed or not.
                            strLabelType = .LabelType
                            strJobName = .JobName

                            'Delete the just processed print request record from table.
                            qtaPrintRequests.PPsp_DeletePrintRequest(.RRN)

                            'Update the status of the pallet record to printed (i.e. 2) and ready for ERP interface program to pick up to post
                            If .LabelType = "P" Then
                                taUpdatePallet.PPsp_EditPallet("Printed", 0, .JobName)
                            End If

                        End With
                    End If
                Next
     
            End If
        Catch ex As Exception
            Throw New Exception("Error in PrintLabels" & vbCrLf & ex.Message)
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Try
            If thdMain.IsAlive Then
                thdMain.Abort()
            End If
            MyBase.Finalize()
        Catch ex As Exception
        End Try
    End Sub
End Class
