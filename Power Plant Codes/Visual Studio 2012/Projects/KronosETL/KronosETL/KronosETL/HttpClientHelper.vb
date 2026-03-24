Imports System
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.ComponentModel
Imports System.Globalization
Imports System.Diagnostics
Imports System.Diagnostics.Eventing.Reader
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Threading.Tasks

'Need to add references - System.Net.Http, System.web.extensions
Public Class HttpClientHelper
    Private ReadOnly client As HttpClient

    Dim strUserName As String = String.Empty
    Dim strPassword As String = String.Empty
    Dim strClientId As String = String.Empty
    Dim strClientSecret As String = String.Empty
    Dim strGrantType As String = String.Empty
    Dim strAppKey As String = String.Empty
    Dim strToken As String = String.Empty
    Dim strContentType As String = String.Empty
    Dim strBaseUrl As String = String.Empty
    Dim strAuthenticationAPI As String = String.Empty
    Dim strAPIName As String = String.Empty
    Dim strCommonRecDtaAPIName As String = String.Empty
    Dim strBaseUrlWithoutVersion As String = String.Empty
    Dim strAPIParameter As String = String.Empty

    Public Sub New()
        client = New HttpClient
    End Sub
    Public Sub New(ByVal httpClient As HttpClient)
        client = httpClient
    End Sub
    Public Property UserName As String
        Get
            Return strUserName
        End Get
        Set(value As String)
            strUserName = value
        End Set
    End Property
    Public Property Password As String
        Get
            Return strPassword
        End Get
        Set(value As String)
            strPassword = value
        End Set
    End Property
    Public Property ClientId As String
        Get
            Return strClientId
        End Get
        Set(value As String)
            strClientId = value
        End Set
    End Property
    Public Property ClientSecret As String
        Get
            Return strClientSecret
        End Get
        Set(value As String)
            strClientSecret = value
        End Set
    End Property
    Public Property GrantType As String
        Get
            Return strGrantType
        End Get
        Set(value As String)
            strGrantType = value
        End Set
    End Property

    Public Property AppKey As String
        Get
            Return strAppKey
        End Get
        Set(value As String)
            strAppKey = value
        End Set
    End Property

    Public Property ContentType As String
        Get
            Return strContentType
        End Get
        Set(value As String)
            strContentType = value
        End Set
    End Property
    Public Property BaseUrl As String
        Get
            Return strBaseUrl
        End Get
        Set(value As String)
            strBaseUrl = value
            strBaseUrlWithoutVersion = strBaseUrl.Replace("/v1", "")
        End Set
    End Property
    Public ReadOnly Property BaseUrlWithoutVersion As String
        Get
            Return strBaseUrlWithoutVersion
        End Get
    End Property

    Public Property AuthenticationAPI As String
        Get
            Return strAuthenticationAPI
        End Get
        Set(value As String)
            strAuthenticationAPI = value
        End Set
    End Property
    Public Property APIName As String
        Get
            Return strAPIName
        End Get
        Set(value As String)
            strAPIName = value
        End Set
    End Property

    Public Property CommonRecDtaAPIName As String
        Get
            Return strCommonRecDtaAPIName
        End Get
        Set(value As String)
            strCommonRecDtaAPIName = value
        End Set
    End Property

    Public Property APIParameter As String
        Get
            Return strAPIParameter
        End Get
        Set(value As String)
            strAPIParameter = value
        End Set
    End Property

    Public Property APIAccessToken As String
        Get
            Return strToken
        End Get
        Set(value As String)
            strToken = value
        End Set
    End Property

    Public Overloads Async Function GetAsync(ByVal strScriptContent As Http.StringContent) As Task(Of HttpResponseMessage)
        Dim hrmResult As HttpResponseMessage
        Try
            Using client As HttpClient = New HttpClient()
                client.BaseAddress = New Uri(strBaseUrl)
                client.DefaultRequestHeaders.Add("AppKey", strAppKey)
                client.DefaultRequestHeaders.Add("ContentType", strContentType)
                client.DefaultRequestHeaders.Add("Authorization", strToken)
                hrmResult = Await client.PostAsync(strBaseUrl & strAPIName, strScriptContent)
            End Using
            Return hrmResult
        Catch ex As Exception
            Throw New Exception("Error in GetAsync" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Overloads Async Function GetAsync() As Task(Of HttpResponseMessage)
        Dim hrmResult As HttpResponseMessage
        Try
            Using client As HttpClient = New HttpClient()
                client.BaseAddress = New Uri(strBaseUrl)
                client.DefaultRequestHeaders.Add("AppKey", strAppKey)
                client.DefaultRequestHeaders.Add("Authorization", strToken)
                hrmResult = Await client.GetAsync(strBaseUrl & strAPIName & strAPIParameter)
            End Using
            Return hrmResult
        Catch ex As Exception
            Throw New Exception("Error in GetAsync" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Overloads Async Function PostAsyncForAccessToken() As Task(Of HttpResponseMessage)
        Dim hrmResult As HttpResponseMessage

        Try
            Using client = New HttpClient()
                client.BaseAddress = New Uri(strBaseUrlWithoutVersion)
                client.DefaultRequestHeaders.Add("AppKey", strAppKey)
                client.DefaultRequestHeaders.Add("ContentType", strContentType)

                Dim content = New FormUrlEncodedContent({
                               New KeyValuePair(Of String, String)("username", strUserName),
                               New KeyValuePair(Of String, String)("password", strPassword),
                               New KeyValuePair(Of String, String)("client_id", strClientId),
                               New KeyValuePair(Of String, String)("client_secret", strClientSecret),
                               New KeyValuePair(Of String, String)("grant_type", strGrantType)
                                                    })
                hrmResult = Await client.PostAsync(strBaseUrlWithoutVersion & strAuthenticationAPI, content)
            End Using
            Return hrmResult
        Catch ex As Exception
            Throw New Exception("Error in PostAsync" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Overloads Async Function PostAsync(ByVal strAPIParam As String) As Task(Of HttpResponseMessage)
        Dim hrmResult As HttpResponseMessage
        Dim strScriptContent As Http.StringContent = Nothing
        Try
            Using client = New HttpClient()
                client.BaseAddress = New Uri(strBaseUrl)
                client.DefaultRequestHeaders.Add("AppKey", strAppKey)
                client.DefaultRequestHeaders.Add("ContentType", strContentType)
                client.DefaultRequestHeaders.Add("Authorization", strToken)
                hrmResult = Await client.PostAsync(strBaseUrl & strAPIName & strAPIParam, strScriptContent)
            End Using
            Return hrmResult
        Catch ex As Exception
            Throw New Exception("Error in PostAsyncByContent" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Overloads Async Function PostAsync(ByVal strScriptContent As Http.StringContent, ByVal strToken As String) As Task(Of HttpResponseMessage)
        Dim hrmResult As HttpResponseMessage
        Try
            Using client = New HttpClient()
                client.BaseAddress = New Uri(strBaseUrl)
                client.DefaultRequestHeaders.Add("AppKey", strAppKey)
                client.DefaultRequestHeaders.Add("ContentType", strContentType)
                client.DefaultRequestHeaders.Add("Authorization", strToken)
                hrmResult = Await client.PostAsync(strBaseUrl & strAPIName, strScriptContent)
            End Using
            Return hrmResult
        Catch ex As Exception
            Throw New Exception("Error in PostAsyncByContent" & vbCrLf & ex.Message)
        End Try
    End Function

End Class
