Imports System.Data.SqlClient
Public Class SharedFunctions
    Private Sub New()
    End Sub

    Public Shared Function PopNumKeyPad(ByVal frm As Form, ByVal ctl As Control) As DialogResult
        Dim ptCtlStartPoint As New Point
        Dim ptFormLocation As New Point
        Dim intX As Integer
        Dim intY As Integer
        Dim KeyPad As New frmNumKeyPad

        ptFormLocation = frm.Location
        ptCtlStartPoint = ctl.Location
        intX = ptFormLocation.X + ptCtlStartPoint.X
        intY = ptFormLocation.Y + ptCtlStartPoint.Y + ctl.Height + 30

        ' if the key pad is outside the right boundary of the form, set X = form width - keypad width
        If intX + KeyPad.Size.Width > frm.Size.Width Then
            intX = frm.Size.Width - KeyPad.Size.Width
        End If

        ' if the key pad is outside the bottom boundary of the form, set X = form height - keypad height
        If intY + KeyPad.Size.Height > frm.Size.Height Then
            intY = frm.Size.Height - KeyPad.Size.Height
        End If
        KeyPad.txtDisplay.Text = RTrim(ctl.Text)
        KeyPad.Location = New Point(intX, intY)
        PopNumKeyPad = KeyPad.ShowDialog()

    End Function
    Public Shared Function PopAlphaNumKB(ByVal frm As Form, ByVal ctl As Control) As DialogResult
        Dim ptCtlStartPoint As New Point
        Dim ptFormLocation As New Point
        Dim intX As Integer
        Dim intY As Integer
        Dim KeyPad As New frmAlphaNumKB

        ptFormLocation = frm.Location
        ptCtlStartPoint = ctl.Location
        intX = ptFormLocation.X + ptCtlStartPoint.X
        intY = ptFormLocation.Y + ptCtlStartPoint.Y + ctl.Height + 30

        ' if the key pad is outside the right boundary of the form, set X = form width - keypad width
        If intX + KeyPad.Size.Width > frm.Size.Width Then
            intX = frm.Size.Width - KeyPad.Size.Width
        End If

        ' if the key pad is outside the bottom boundary of the form, set X = form height - keypad height
        If intY + KeyPad.Size.Height > frm.Size.Height Then
            intY = frm.Size.Height - KeyPad.Size.Height
        End If
        KeyPad.txtDisplay.Text = RTrim(ctl.Text)
        KeyPad.Location = New Point(intX, intY)
        PopAlphaNumKB = KeyPad.ShowDialog()

    End Function
    Public Shared Sub ClearInputFields(ByVal frm As Form)
        Dim ctl As Control
        For Each ctl In frm.Controls
            If TypeOf ctl Is TextBox Or TypeOf ctl Is ComboBox Then
                ctl.Text = ""
            End If
        Next
    End Sub
    Public Shared Function IsSvrConnOK() As Boolean
        'Function: Is server connection OK?
        Dim cnnServer As New SqlConnection(gstrServerCnnStr)
        IsSvrConnOK = True
        'if the connection to server failure, end the routine
        Try
            cnnServer.Open()
        Catch ex As SqlException
            Return False
        Catch ex As Exception
            Throw New Exception("Error in IsSvrConnOK" & vbCrLf & ex.Message)
        Finally
            If Not cnnServer.State <> ConnectionState.Closed Then
                cnnServer.Close()
            End If
        End Try
    End Function
    Public Shared Sub uploadDTLogToServer()
        Try
            'WO#648 DEL Start
            'Dim taDTLog As New dsDownTimeLogTableAdapters.tblDownTimeLogTableAdapter
            'Dim tblDownTimeLog As New dsDownTimeLog.tblDownTimeLogDataTable
            'Dim dr As dsDownTimeLog.tblDownTimeLogRow
            'WO#648 DEL Stop
            'WO#648 ADD Start
            Dim taDTLog As New dsDownTimeLogTableAdapters.CPPsp_DownTimeLog_SelTableAdapter
            Dim tblDownTimeLog As New dsDownTimeLog.CPPsp_DownTimeLog_SelDataTable
            Dim dr As dsDownTimeLog.CPPsp_DownTimeLog_SelRow
            'WO#648 ADD Stop
            Try
                taDTLog.Connection.ConnectionString = gstrLocalCnnStr
                'WO#648 taDTLog.FillByNothing(tblDownTimeLog
                taDTLog.FillByNothing(tblDownTimeLog, "ALL", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)  'WO#648
                If tblDownTimeLog.Rows.Count > 0 Then
                    For Each dr In tblDownTimeLog.Rows
                        With dr
                            gtaDownTimeLog.Insert(dr("InActive"), dr("Facility"), dr("ShopOrder"), dr("MachineType"), dr("MachineSubType"), dr("MachineID"), _
                                dr("Operator"), dr("Technician"), dr("DownTimeBegin"), dr("DownTimeEnd"), dr("Shift"), _
                                dr("ReasonType"), dr("ReasonCOde"), dr.Comment, dr("CreatedBy"), dr("CreationDate"), dr("UpdatedBy"), dr("LastUPdated"), dr.ShiftProductionDate, dr.EventID)
                            taDTLog.Delete(dr("RRN"))
                        End With
                    Next
                End If
            Catch ex As SqlException
                If ex.ErrorCode = -2146232060 Then
                    gtaDownTimeLog.Connection.ConnectionString = gstrLocalCnnStr
                    gblnConnectUp = False
                Else
                    Throw New Exception("Error in uploadDTLogToServer" & vbCrLf & ex.Message)
                End If
            Catch ex As Exception
                Throw New Exception("Error in uploadDTLogToServer" & vbCrLf & ex.Message)
            End Try
        Catch ex As Exception
            Throw New Exception("Error in uploadDTLogToServer" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Shared Function GetRunningInstance(ByVal _
               processName As String) As Process
        Dim proclist() As Process = _
           Process.GetProcessesByName(processName)
        For Each p As Process In proclist
            If p.Id <> Process.GetCurrentProcess().Id Then
                Return p
            End If
        Next
        Return Nothing
    End Function
    Public Shared Sub AddMessageLineToForm(ByVal frm As Form, ByVal strMsg As String)
        Dim lblNote As New Label

        Try
            'lblNote.Name = "lblMessage"
            'lblNote.Text = strMsg
            'lblNote.Location = New Point(22, 565)
            'lblNote.ForeColor = Color.LightSalmon
            'lblNote.AutoSize = True
            'lblNote.Font = New Font(FontFamily.GenericSansSerif, 18.0F, FontStyle.Bold)
            'frm.Controls.Add(lblNote)
            With frm.Controls.Item("lblMessage")
                .Text = strMsg
                .Visible = True
            End With
        Catch ex As Exception
            Throw New Exception("Error in AddMessageLineToForm" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Sub RmvMessageLineFromForm(ByVal frm As Form)
        'Dim ctl As Control
        'For Each ctl In frm.Controls
        '    If ctl.Name = "lblMessage" AndAlso TypeOf ctl Is Label Then
        '        frm.Controls.Remove(ctl)
        '    End If
        'Next
        frm.Controls.Item("lblMessage").Visible = False
    End Sub

    Public Shared Sub UpdateSessionControlDownTime(ByVal blnStart As Boolean)

        Dim cmSC As SqlCommand
        Dim strValue As String
        Try
            If blnStart Then
                strValue = "SetStartDownTime_On"
            Else
                strValue = "SetStartDownTime_Off"
            End If
            Using cnnSC = New SqlConnection(gstrLocalCnnStr)
                With cnnSC
                    cmSC = cnnSC.CreateCommand()
                    .Open()
                End With
                With cmSC
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "CPPsp_SessionControlIO"
                    .Parameters.AddWithValue("@chrAction", strValue)
                    .ExecuteNonQuery()
                End With
            End Using
        Catch ex As Exception
            Throw New Exception("Error in UpdateSessionControlDownTime" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Function GetProductionDateByShift(ByVal strFacility As String, ByVal intOverrideShift As Integer, _
            ByVal dteGivenDateTime As DateTime, Optional ByVal strPkgLine As String = "", Optional ByVal strWorkGroup As String = "") As DateTime
        'Get production date by shift
        Dim cmSC As SqlCommand
        Dim arParms() As SqlParameter
        Dim strConn As String

        Try
            If String.IsNullOrEmpty(gstrLocalCnnStr) Then
                strConn = gstrServerCnnStr
            Else
                strConn = gstrLocalCnnStr
            End If

            Using cnnSC = New SqlConnection(strConn)
                With cnnSC
                    cmSC = cnnSC.CreateCommand()
                    .Open()
                End With
                With cmSC
                    .Parameters.Clear()

                    ReDim arParms(5)
                    arParms = New SqlParameter(UBound(arParms)) {}

                    ' Faccility Input Parameter
                    arParms(0) = New SqlParameter("@chrFacility", SqlDbType.Char, 3)
                    arParms(0).Value = strFacility

                    ' Overrided Shift Input Parameter
                    arParms(1) = New SqlParameter("@intGivenShiftNo", SqlDbType.TinyInt)
                    arParms(1).Value = intOverrideShift

                    ' Current Data time Input Parameter
                    arParms(2) = New SqlParameter("@dteGivenDateTime", SqlDbType.DateTime)
                    arParms(2).Value = dteGivenDateTime

                    ' Work Group Input Parameter
                    arParms(3) = New SqlParameter("@vchMachineID", SqlDbType.VarChar, 10)
                    arParms(3).Value = strPkgLine

                    ' Work Group Input Parameter
                    arParms(4) = New SqlParameter("@vchWorkGroup", SqlDbType.VarChar, 3)
                    arParms(4).Value = strWorkGroup

                    ' Work Group Input Parameter
                    arParms(5) = New SqlParameter("@dteProductionDate", SqlDbType.DateTime)
                    arParms(5).Direction = ParameterDirection.Output

                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "CPPsp_GetProdDateByShift"
                    .Parameters.AddRange(arParms)
                    .ExecuteNonQuery()

                    GetProductionDateByShift = arParms(5).Value

                End With
            End Using

        Catch ex As Exception
            Throw New Exception("Error in GetProductionDateByShift" & vbCrLf & ex.Message)
        End Try


    End Function

    Public Shared Function GetSessionControl() As dsSessionControl.CPPsp_SessionControlIORow
        Try
            Using daSC As New dsSessionControlTableAdapters.CPPsp_SessionControlIOTableAdapter
                Using dtSC As New dsSessionControl.CPPsp_SessionControlIODataTable
                    daSC.Fill(dtSC, "SelectAllFields")
                    Return dtSC.Rows(0)
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error in GetSessionControl" & vbCrLf & ex.Message)
        End Try
    End Function
    Public Shared Function GetSessionControlHst(ByVal strFacility As String, ByVal strPackagingLine As String, ByVal intShopOrder As Integer, ByVal dteGivenDateTime As DateTime) As dsSessionControlHst.CPPsp_SessionControlHstIORow
        Dim drSCH As dsSessionControlHst.CPPsp_SessionControlHstIORow
        Try
            Using daSCH As New dsSessionControlHstTableAdapters.CPPsp_SessionControlHstIOTableAdapter
                Using dtSCH As New dsSessionControlHst.CPPsp_SessionControlHstIODataTable
                    daSCH.Fill(dtSCH, "BySOLine", strPackagingLine, intShopOrder, Nothing, Nothing, Nothing, strFacility, Nothing, Nothing)
                    For Each drSCH In dtSCH.Rows
                        If dteGivenDateTime >= drSCH.StartTime AndAlso dteGivenDateTime <= drSCH.StopTime Then
                            Return drSCH
                            Exit Function
                        End If
                    Next
                    Return Nothing
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error in GetSessionControlHst" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Function GetSharedEquiptmentList(ByVal strFacility As String, ByVal strPackagingLine As String, strType As String, strSubtype As String, strAction As String) As dsEquipment.dtEquipmentDataTable
        Dim taEq As New dsEquipmentTableAdapters.taEquipment
        Dim dtEq As New dsEquipment.dtEquipmentDataTable
        taEq.Fill(dtEq, strFacility, strPackagingLine, strType, strSubtype, strAction)
        Return dtEq
    End Function
End Class
