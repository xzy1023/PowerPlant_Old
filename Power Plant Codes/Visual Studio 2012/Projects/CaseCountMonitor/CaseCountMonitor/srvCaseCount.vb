Imports System.Threading

Public Class srvCaseCount

    Private Delegate Sub MyDelegate(ByVal objArgs As Object)

    Friend Class StateObj
        Friend stu As SharedFunctions.stuLineInfoXML
        Friend strComputerName As String
        Friend RetVal As String
    End Class

    Private t1 As Thread

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
        t1 = New Thread(AddressOf Monitoring)
        t1.IsBackground = True
        t1.Start()
        EventLog.WriteEntry(Me.ServiceName & " started.", EventLogEntryType.Information)        'WO#6314
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        t1.Abort()
        Me.Stop()       'WO#6314
        Me.Dispose()    'WO#6314
    End Sub

    Public Sub Monitoring()
        Dim dtLineInfo As New DataTable
        Dim stu As SharedFunctions.stuLineInfoXML
        Dim strFileName As String
        Dim strComputerName As String
        'WO#6314    Dim thd As Thread = Nothing
        Dim objState As New StateObj
        Dim intMaxRetry As Integer = 10                      'WO#6314
        Dim intRetryCount As Integer                        'WO#6314        
        Dim strFileHasError As String = String.Empty        'WO#6314

        Try

            dtLineInfo = SharedFunctions.buildIPCConnectionList()

            While (True)
                For Each drLineInfo As DataRow In dtLineInfo.Rows
                    strFileName = My.Settings.gstrXMLOutputFolder & RTrim(drLineInfo.Item("Line")) & ".xml"
                    Try     'WO#6314
                        stu = SharedFunctions.GetLineInfoXML(strFileName)
                        'WO#6314 Add Start
                    Catch IOEx As Exception
                        EventLog.WriteEntry("Case Count Monitor service warning - " & IOEx.Message & vbCrLf & IOEx.StackTrace, EventLogEntryType.Warning)
                        If strFileHasError <> strFileName Then
                            strFileHasError = strFileName
                            intRetryCount = 1
                        Else
                            intRetryCount = intRetryCount + 1
                            If intRetryCount = intMaxRetry Then
                                Try
                                    SharedFunctions.SendEmail("Case Count Monitor service warning - " & IOEx.Message)
                                    Thread.Sleep(Integer.Parse(My.Settings.intTimeInterval) / dtLineInfo.Rows.Count)
                                Catch
                                End Try
                                intRetryCount = 0
                            End If
                        End If
                        Continue For
                    End Try
                    intRetryCount = 0
                    'WO#6314 Add Stop
                    If My.Settings.blnWriteEvent Then
                        EventLog.WriteEntry("Shop Order: " & stu.ShopOrder & ", " & stu.Line & ", XML - " & stu.CaseCount & " From Last cycle-: " & drLineInfo.Item("CasesProduced").ToString, EventLogEntryType.Information)
                    End If
                    If stu.ShopOrder > 0 AndAlso stu.CaseCount <> drLineInfo.Item("CasesProduced") Then
                        strComputerName = drLineInfo.Item("ComputerName")
                        ' Queue a task
                        objState.stu = stu
                        objState.strComputerName = strComputerName

                        'run the UpdateDB subroutine in different thread each time.
                        Dim d As MyDelegate = AddressOf UpdateDB
                        d.BeginInvoke(objState, Nothing, Nothing)

                        'ThreadPool.QueueUserWorkItem(New System.Threading.WaitCallback _
                        '                       (AddressOf UpdateDB), objState)

                        'thd = New Thread(Sub() UpdateDB(stu, strComputerName))
                        'thd.Start()
                        drLineInfo.Item("CasesProduced") = stu.CaseCount
                    End If

                Next

                Thread.Sleep(Integer.Parse(My.Settings.intTimeInterval))
            End While

        Catch ex As Exception

            EventLog.WriteEntry("Case Count Monitor service error - " & ex.Message & vbCrLf & ex.StackTrace, EventLogEntryType.Error)
            Try
                EventLog.WriteEntry("Send Message: " & ex.Message, EventLogEntryType.Information)   'WO#6314
                SharedFunctions.SendEmail(ex.Message)
            Catch SndEmailEx As Exception
                EventLog.WriteEntry("Monitoring Send Email error - " & SndEmailEx.Message & vbCrLf & SndEmailEx.StackTrace, EventLogEntryType.Error)
            End Try

        Finally
            'WO#6314 DEL Start
            'WO#6314 IsNothing(thd) Then
            'WO#6314  thd.Abort()    
            If Not IsNothing(t1) Then          'WO#6314
                OnStop()                     'WO#6314
            End If
        End Try
    End Sub

    Public Sub UpdateDB(ByVal objArgs As Object)
        Dim exError As Exception
        Dim strComputerName As String = String.Empty
        Dim stu As SharedFunctions.stuLineInfoXML
        Dim objState As StateObj

        Try

            objState = CType(objArgs, StateObj)

            strComputerName = objState.strComputerName
            stu = objState.stu

            If My.Settings.blnWriteEvent Then
                EventLog.WriteEntry(strComputerName & " , " & stu.Line & " , " & stu.ShopOrder.ToString & " , " & stu.SOStartTime & " , " & stu.CaseCount.ToString)
            End If

            'update the Case Count column of the Session Control record on the IPC.
            exError = SharedFunctions.UpdateCaseCountToIPC(strComputerName, stu.Line, stu.ShopOrder, stu.SOStartTime, stu.CaseCount)

            If Not IsNothing(exError) Then
                EventLog.WriteEntry("UpdateCaseCountToIPC in error - computer name: " & strComputerName & vbCrLf & exError.Message, EventLogEntryType.Warning)
            End If
        Catch ex As Exception
            EventLog.WriteEntry("Error in UpdateDB - computer name: " & strComputerName & vbCrLf & ex.Message & vbCrLf & ex.StackTrace, EventLogEntryType.Error)
            Try
                SharedFunctions.SendEmail(ex.Message)
            Catch SndEmailEx As Exception
                EventLog.WriteEntry("UpdateDB Send Email error - " & SndEmailEx.Message & vbCrLf & SndEmailEx.StackTrace, EventLogEntryType.Error)
            End Try
        End Try
    End Sub
End Class
