'WO#3695 Add Start
Public Enum ShiftMethod
    sequential = 0
    PatternCode = 1
End Enum
'WO#3695 Add Stop

Public Class WorkShift
    Dim intShift As Short
    Dim strDescription As String
    Dim dteFromTime As DateTime
    Dim dteToTime As DateTime
    Dim intMethod As Integer                                                                            'WO#3695
    Dim taShift As New dsShiftTableAdapters.CPPsp_ShiftIOTableAdapter
    Dim tblExpectedShift As New dsShift.CPPsp_ShiftIODataTable
    Public Sub GetExpectedShiftInfoByTime(ByVal dteDateTime As DateTime, ByVal strWorkShiftType As String, ByVal blnNotShowDummyShift As Boolean)   'WO#2645
        'WO#2645 Public Sub New(ByVal dteDateTime As DateTime, ByVal strWorkShiftType As String)
        Try
            'get the expected shift no by current time
            'WO#2645 taShift.Fill(tblExpectedShift, "ExpectedShift", gdrSessCtl("Facility"), strWorkShiftType, dteDateTime)
            taShift.Fill(tblExpectedShift, "ExpectedShift", gdrSessCtl("Facility"), strWorkShiftType, dteDateTime, Nothing, blnNotShowDummyShift)    'WO#2645
            If tblExpectedShift.Rows.Count > 0 Then
                FillData(tblExpectedShift.Rows(0))
            End If
        Catch ex As Exception
            'WO#2645 Throw New Exception("Error in Shift contructor" & vbCrLf & ex.Message)
            Throw New Exception("Error in GetExpectedShiftInfoByTime." & vbCrLf & ex.Message)              'WO#2645
        End Try
    End Sub

    Public Sub New()    'WO#2645
    End Sub             'WO#2645

    'WO#3695 ADD Start
    Public Sub New(ByVal dteDateTime As DateTime, ByVal strWorkShiftType As String, ByVal blnNotShowDummyShift As Boolean)
        GetExpectedShiftInfoByTime(dteDateTime, strWorkShiftType, blnNotShowDummyShift)
    End Sub
    'WO#3695 ADD Stop

    Public Sub GetShiftInfoByShiftNo(ByVal intShift As Integer, ByVal dteDateTime As DateTime, ByVal strWorkShiftType As String, ByVal blnNotShowDummyShift As Boolean)    'WO#2645
        'WO#2645   Public Sub New(ByVal intShift As Integer, ByVal dteDateTime As DateTime, ByVal strWorkShiftType As String)
        Try
            'get the expected shift no by current time
            'WO#2645    taShift.FillByShift(tblExpectedShift, "ShiftTime", gdrSessCtl("Facility"), strWorkShiftType, dteDateTime, intShift)
            taShift.FillByShift(tblExpectedShift, "ShiftTime", gdrSessCtl("Facility"), strWorkShiftType, dteDateTime, intShift, blnNotShowDummyShift)   'WO#2645
            If tblExpectedShift.Rows.Count > 0 Then
                FillData(tblExpectedShift.Rows(0))
            End If
        Catch ex As Exception
            'WO#2645 Throw New Exception("Error in Shift contructor" & vbCrLf & ex.Message)
            Throw New Exception("Error in GetShiftInfoByShiftNo" & vbCrLf & ex.Message)     'WO#2645
        End Try
    End Sub
    Public ReadOnly Property Shift() As Short
        Get
            Return intShift
        End Get
    End Property
    Public ReadOnly Property Description() As String
        Get
            Return strDescription
        End Get
    End Property
    Public ReadOnly Property FromTime() As DateTime
        Get
            Return dteFromTime
        End Get
    End Property
    Public ReadOnly Property ToTime() As DateTime
        Get
            Return dteToTime
        End Get
    End Property
    'WO#3695 Add Start
    Public ReadOnly Property Method() As Integer
        Get
            Return intMethod
        End Get
    End Property
    'WO#3695 Add Stop

    Private Sub FillData(ByVal dr As DataRow)
        intShift = dr("Shift")
        strDescription = dr("Description")
        dteFromTime = dr("FromTime")
        dteToTime = dr("ToTime")
        intMethod = dr("Method")                               'WO#3695
    End Sub

    'WO#3695 Add Start
    Public Function IsEnteredShiftValid(ByVal intEnteredShift As Integer, ByVal dteDateTime As DateTime, ByVal strWorkShiftType As String, ByVal intCurrentShift As Integer) As String
        Dim intNextShift As Integer
        Dim intPreviousShift As Integer
        Dim strErrMsg As String = Nothing

        Try

            'Get current shift from current system time to get the shift method.
            GetExpectedShiftInfoByTime(dteDateTime, strWorkShiftType, True)
            If intCurrentShift = 0 Then
                intCurrentShift = intShift
            End If

            If intMethod = ShiftMethod.PatternCode Then
                GetExpectedShiftInfoByTime(DateAdd(DateInterval.Hour, 12, dteDateTime), strWorkShiftType, True)
                intNextShift = intShift
                GetExpectedShiftInfoByTime(DateAdd(DateInterval.Hour, -12, dteDateTime), strWorkShiftType, True)
                intPreviousShift = intShift
                If intEnteredShift <> intNextShift And intEnteredShift <> intPreviousShift And intEnteredShift <> intCurrentShift Then
                    strErrMsg = "Entered shift no. is not the current, previous or next of shift no., please enter a valid shift no."
                End If
            End If
            Return strErrMsg

        Catch ex As Exception
            Throw New Exception("Error in IsShiftValid" & vbCrLf & ex.Message)
        End Try
    End Function

    'WO#3695 Add Stop
End Class
