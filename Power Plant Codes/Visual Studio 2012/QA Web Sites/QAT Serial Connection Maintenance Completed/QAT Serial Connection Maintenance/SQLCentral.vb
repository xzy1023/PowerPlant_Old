Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System

Public Class SQLCentral
    Private DBCon As New SqlConnection(ConfigurationManager.ConnectionStrings("PowerPlantCnnStr").ConnectionString)
    Private DBCmd As SqlCommand
    Public DBDA As SqlDataAdapter
    Public DBDT As DataTable
    Public Params As New List(Of SqlParameter)
    Public DBDS As DataSet
    Public SQLDS As DataSet
    Public RecordCount As Integer
    Public Exception As String
    Public Sub New()
    End Sub
    Public Sub New(ConnectionString As String)
        DBCon = New SqlConnection(ConnectionString)
    End Sub
    Public Function GetCurrentUserName() As String
        Dim currUserName As String = String.Empty
        Dim currUser As String
        Dim endIndex As Integer
        'currUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name
        currUser = HttpContext.Current.User.Identity.Name.ToString
        endIndex = currUser.LastIndexOf("\")
        currUserName = currUser.Substring(endIndex + 1, currUser.Length - endIndex - 1)
        Return currUserName
    End Function

    Public Function HasConnection() As Boolean
        Try
            DBCon.Open()
            DBCon.Close()
            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Return False
    End Function
    Public Sub RunQuery(Query As String)
        Try
            DBCon.Open()
            DBCmd = New SqlCommand(Query, DBCon)
            DBDA = New SqlDataAdapter(DBCmd)
            DBDS = New DataSet
            DBDA.Fill(DBDS)
            DBCon.Close()
        Catch ex As Exception
            If DBCon.State = ConnectionState.Open Then
                DBCon.Close()
            End If
        End Try
    End Sub
    Public Sub ExecQuery(Query As String, Optional ReturnIdentity As Boolean = False)
        RecordCount = 0
        Exception = ""
        Try
            DBCon.Open()
            DBCmd = New SqlCommand(Query, DBCon)
            Params.ForEach(Sub(p) DBCmd.Parameters.Add(p))
            Params.Clear()
            DBDT = New DataTable
            DBDA = New SqlDataAdapter(DBCmd)
            RecordCount = DBDA.Fill(DBDT)
            If ReturnIdentity = True Then
                '@@Identity = Session
                'Scope_Identity -Session & Scope
                'Identity_current(tablename) = Last identity In table, an ysession & ny scope
                Dim ReturnQuery As String = "Select @@Identity as LastID;"
                DBCmd = New SqlCommand(ReturnQuery, DBCon)
                DBDT = New DataTable
                DBDA = New SqlDataAdapter(DBCmd)
                RecordCount = DBDA.Fill(DBDT)
            End If
        Catch ex As Exception
            Exception = "ExecQuery Error: " & vbNewLine & ex.Message
            MsgBox(ex.Message)
        Finally
            If DBCon.State = ConnectionState.Open Then
                DBCon.Close()
            End If
        End Try
    End Sub
    Public Sub AddParam(Name As String, Value As Object)
        Dim NewParam As New SqlParameter(Name, Value)
        Params.Add(NewParam)
    End Sub
    Public Function HasException(Optional Report As Boolean = False) As Boolean
        If String.IsNullOrEmpty(Exception) Then Return False
        If Report = True Then MsgBox(Exception, MsgBoxStyle.Critical, "Exception:")
        Return True
    End Function
    Public Function HasExceptionMsg(Optional Report As Boolean = False) As String
        If String.IsNullOrEmpty(Exception) Then Return ""
        If Report = True Then
            ' MsgBox(Exception, MsgBoxStyle.Critical, "Exception:")
            Return Exception
        End If
        Return Exception
    End Function

End Class
