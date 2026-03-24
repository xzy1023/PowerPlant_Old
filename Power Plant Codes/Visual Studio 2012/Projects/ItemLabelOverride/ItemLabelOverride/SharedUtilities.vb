Imports System.IO

Public Class SharedUtilities
    Public Shared Function GetControlRcd(ByVal strKey As String, ByVal strSubKey As String, ByVal strAction As String) As dsControl.PPsp_Control_SelRow
        Dim taCtl As New dsControlTableAdapters.PPsp_Control_SelTableAdapter
        Dim dtCtl As New dsControl.PPsp_Control_SelDataTable
        Try
            taCtl.Fill(dtCtl, strKey, strSubKey, strAction)

            If dtCtl.Rows.Count > 0 Then
                Return (dtCtl.Rows(0))
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function IsFileExisted(strFileName) As Boolean
        Dim strFilePath As String
        'ID#6696 ADD start
        Dim strFilesInDir() As String
        Dim strFile As String
        Dim blnFind As Boolean = 0
        'ID#6696 ADD stop

        strFilePath = My.Settings.strFilePath & strFileName
        'ID#6696 ADD start
        strFilesInDir = Directory.GetFiles(My.Settings.strFilePath)
        For Each strFile In strFilesInDir
            If strFile = strFilePath Then
                blnFind = 1
                Exit For
            End If
        Next
        'ID#6696 ADD stop

        'ID#6696 Return File.Exists(strFilePath)
        Return blnFind          'ID#6696
    End Function
End Class
