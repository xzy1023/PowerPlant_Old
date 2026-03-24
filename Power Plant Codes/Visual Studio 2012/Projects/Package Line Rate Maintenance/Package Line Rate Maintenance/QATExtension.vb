Imports System.Net

Public Module QATExtension
    <System.Runtime.CompilerServices.Extension()> _
    Public Function IsInteger(ByVal value As String) As Boolean
        If String.IsNullOrEmpty(value) Then
            Return False
        Else
            Return Int16.TryParse(value, Nothing)
        End If
    End Function
    <System.Runtime.CompilerServices.Extension()> _
    Public Function ToInteger(ByVal value As String) As Boolean
        If value.IsInteger() Then
            If value < 0 Then
                Return False
            Else
                Return True
             End If
        Else
            Return False
        End If
    End Function
    <System.Runtime.CompilerServices.Extension()> _
    Public Function ToIPAddress(ByVal value As String) As Boolean
        If value.IsInteger() Then
            If value < 0 Or value > 255 Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If
    End Function

    <System.Runtime.CompilerServices.Extension()> _
    Public Function IsInteger32(ByVal value As String) As Boolean
        If String.IsNullOrEmpty(value) Then
            Return False
        Else
            Return Int32.TryParse(value, Nothing)
        End If
    End Function

    <System.Runtime.CompilerServices.Extension()> _
    Public Function ToInteger32(ByVal value As String) As Boolean
        If value.IsInteger32() Then
            If value < 0 Then
                Return False
            Else
                Return True
                '                Return Int32.Parse(value)
            End If
        Else
            Return False
        End If
    End Function
  
    Public Function IsIPValid(ByVal addrString As String) As Boolean
        Dim strTemp1 As String = String.Empty
        Dim intCount As Integer = 0
        strTemp1 = addrString.Replace(" ", "")
        'WO#35671 ADD Start
        If addrString.Length <> strTemp1.Length Then
            Return False
        End If
        'WO#35671 ADD Stop
        intCount = strTemp1.Split(".").Length - 1
            If intCount <> 3 Then
            Return False
        End If
        Dim arrTemp() As String = Split(strTemp1, ".")
        Dim i As Integer = 0
        Dim LastNonEmpty As Integer = -1
        For i = 0 To arrTemp.Length - 1
            If arrTemp(i) <> "" Then
                LastNonEmpty += 1
                arrTemp(LastNonEmpty) = arrTemp(i)
            End If
        Next
        ReDim Preserve arrTemp(LastNonEmpty)
        If (arrTemp.Length <> 4) Then
            Return False
        End If
        For i = 0 To arrTemp.Length - 1
            If Not ToIPAddress(arrTemp(i)) Then
                Return False
            End If
            intCount = intCount + 1
        Next
        If Not IsIPValid2(strTemp1) Then
            Return False
        End If
        Return True
    End Function
    Public Function IsIPValid2(ByVal addrString As String) As Boolean
        Dim address As IPAddress = Nothing
        Return IPAddress.TryParse(addrString, address)
    End Function
End Module
