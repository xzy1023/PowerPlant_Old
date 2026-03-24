Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports Microsoft.SqlServer.Server
Imports System.IO

Module Module1
    Dim strLocalServerCnnStr As String = String.Empty

    Sub Main()
        'Dim strServerName As String = System.Environment.MachineName
        Dim strSQLStmt As String
        Dim arlTableName As ArrayList
        Dim dRdr As SqlDataReader
        Dim strMsg As String
        Dim strCmdArgFacility As String
        Dim strCmdArgSQLServerName As String
        Dim strCmdArgPPDB As String
        Dim strLogFileName As String
        Dim cmd As New SqlCommand

        strLogFileName = String.Format(My.Application.Info.DirectoryPath & My.Settings.gstrLogFilePath, Now.Date.ToString("yyyyMMdd"))

        Try
            Dim strCmdArg As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Application.CommandLineArgs
            If strCmdArg.Count < 3 Then
                Console.WriteLine("Require 3 parameters. 1.SQL Server Instance Name; 2. Catalog Name; 3. Facility; 4. Log file path")
            Else
                strCmdArgSQLServerName = strCmdArg(0)
                strCmdArgPPDB = strCmdArg(1)
                strCmdArgFacility = strCmdArg(2)
                If strCmdArg.Count > 3 Then
                    strLogFileName = strCmdArg(3)
                End If

                'Modify connection string to the connect SQL server and catalog
                strLocalServerCnnStr = String.Format(My.Settings.gstrPPServerCnnStr, strCmdArgSQLServerName, strCmdArgPPDB)

                Using cnnLocalSrc As SqlConnection = New SqlConnection(strLocalServerCnnStr)
                    cnnLocalSrc.Open()
                    'strSQLStmt = "PPsp_Control_Sel"
                    'cmd = New SqlCommand(strSQLStmt, cnnLocalSrc)
                    'cmd.CommandType = CommandType.StoredProcedure
                    'cmd.Parameters.Add("vchKey", SqlDbType.VarChar, 50).Value = "ERPDataXChgLogFolder"
                    'cmd.Parameters.Add("vchAction", SqlDbType.VarChar, 50).Value = "ByKey"
                    'dRdr = cmd.ExecuteReader()
                    'While dRdr.Read
                    '    arlTableName.Add(dRdr("Value1"))
                    'End While


                    ' Set Ready_For_DownLoad status to No in the Power Plant SQL server for audit purpose
                    strSQLStmt = "Update tblComputerConfig set ReadyForDownLoad = 0 WHERE Facility = '" & strCmdArgFacility & " ' AND RecordStatus = 1"
                    cmd = New SqlCommand(strSQLStmt, cnnLocalSrc)
                    cmd.CommandType = CommandType.Text
                    cmd.ExecuteNonQuery()

                    strSQLStmt = "select TableName from tblDownLoadTableList Where Facility = '" & strCmdArgFacility & "' and Active = '1' "
                    cmd.CommandText = strSQLStmt
                    dRdr = cmd.ExecuteReader()

                    arlTableName = New ArrayList
                    While dRdr.Read
                        arlTableName.Add(dRdr("TableName"))
                    End While
                    cnnLocalSrc.Close()
                End Using

                If arlTableName.Count > 0 Then
                    'Download imported tables from Server database to Local IPCs staging database
                    'strSQLSvrExportCnnStr = String.Format(My.Settings.gstrSQLExportCnnStr, strServerName, strPowerPlantDB)
                    strMsg = "There will be total " & arlTableName.Count & " tables will be exported"
                    WriteLog(strLogFileName, strMsg)

                    For Each strTableName As String In arlTableName
                        WriteLog(strLogFileName, "Table: " & strTableName)
                    Next

                    strMsg = "Exporting downloaded tables from " & strCmdArgSQLServerName & ", " & strCmdArgPPDB & " to IPCs started."
                    WriteLog(strLogFileName, strMsg)

                    ExportDataToIPCStagingArea(strCmdArgFacility, strCmdArgSQLServerName, strCmdArgPPDB, arlTableName, strLogFileName)

                    strMsg = "Exporting downloaded tables from " & strCmdArgSQLServerName & ", " & strCmdArgPPDB & " to IPCs completed."
                    WriteLog(strLogFileName, strMsg)
                    'At the end of the PPsp_ExportDataToLocalDB stored procedure will flag the Computer Configuration table to indicate
                    'the data in IPC is ready to import from staging database to Local Power Plant database
                Else
                    strMsg = "No Files are required to be download."
                    WriteLog(strLogFileName, strMsg)
                End If
                End If
        Catch ex As Exception
            WriteLog(strLogFileName, ex.Message)
            Throw ex
        End Try
    End Sub

    Sub ExportDataToIPCStagingArea(ByVal strFacility As String, ByVal strServerName As String, ByVal strPowerPlantDB As String, _
            ByVal arlTableName As ArrayList, ByVal strLogFileName As String)
        Dim strSQL As String
        Dim dr As SqlDataReader
        Dim strDestCnnStr As String
        Dim strTableName As String
        Dim dt As DataTable
        Dim ds As New DataSet
        Dim blnHasFacilityColumn As Boolean
        Dim strMsg As String
        Dim strIPCImportDataDB As String = "ImportData"
        Dim blnAtLeast1Success As Boolean
        Dim sdTableName As New SortedDictionary(Of String, Boolean)
        Dim sqlCmd As New SqlCommand()
        Dim gstrSQLExportCnnStr As String
        Const cintNoOfSeconds As Integer = 120

        Try
            'loop through the array list to get the table names and load those tables to a dataset for reuse
            gstrSQLExportCnnStr = "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID=dataimporter;Password=dataimporter#1; Connect Timeout = 10"

            Using cnnLocalDB As SqlConnection = New SqlConnection(strLocalServerCnnStr)
                cnnLocalDB.Open()
                sqlCmd.Connection = cnnLocalDB
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandTimeout = cintNoOfSeconds
                'Check the tabl on the server. if the table has Facility Column, includes it in the filter
                For Each strTableName In arlTableName

                    strSQL = "select 1 from information_schema.columns where table_name = '" & strTableName & "' and column_name = 'facility'"
                    sqlCmd.CommandText = strSQL

                    blnHasFacilityColumn = sqlCmd.ExecuteScalar()

                    sdTableName.Add(strTableName, blnHasFacilityColumn)

                    'WO#14867   If strTableName = "tblEquipment" Then
                    'WO#14867    strSQL = "SELECT Active, facility, EquipmentID, [Type] ,SubType, Description, IPCSharedGroup,WorkCenter FROM tblEquipment Where facility = '" & strFacility & "'"
                    'WO#14867    Else
                    If blnHasFacilityColumn Then
                        strSQL = "Select * from " & strTableName & " where facility = '" & strFacility & " '"
                    Else
                        strSQL = "Select * from " & strTableName
                    End If
                    'WO#14867   End If

                    'Load all the down load tables from the server local data base to the tables in a data set
                    Using cmd As New SqlCommand(strSQL, cnnLocalDB)
                        dt = New DataTable(strTableName)
                        dt.Load(cmd.ExecuteReader)
                        ds.Tables.Add(dt)
                    End Using

                Next

                'get the list of active IPCs from local data base in the server for exporting the data
                strSQL = "SELECT ComputerName,PackagingLine FROM dbo.tblComputerConfig " & _
                        "WHERE RecordStatus = 1 and (facility = '" & strFacility & " ' OR PackagingLine = 'Spare') " & _
                        "ORDER BY ComputerName"
                sqlCmd.CommandText = strSQL
                sqlCmd.Connection = cnnLocalDB
                dr = sqlCmd.ExecuteReader
                dt = New DataTable
                dt.Load(dr)

                'loop through the active IPC list to get the IPC name the the destination connection string 
                For Each row As DataRow In dt.Rows
                    Try
                        blnAtLeast1Success = False
                        strMsg = "Start exporting data to IPC - " & row("ComputerName")
                        WriteLog(strLogFileName, strMsg)


                        strDestCnnStr = String.Format(gstrSQLExportCnnStr, row("ComputerName") & "\SQLEXPRESS", strIPCImportDataDB)

                        Using cnnDescDB As SqlConnection = New SqlConnection(strDestCnnStr)
                            'Update the ReadyForDownLoad flag to False of the destination IPC record in the Computer Config table on the IPC staging data base to avoid any access 
                            'during the down load process.
                            strSQL = "Update tblComputerConfig SET ReadyForDownLoad = 0 WHERE Facility = '" & strFacility & " ' and ComputerName = '" & row("ComputerName") & "'"
                            Try
                                If cnnDescDB.State = ConnectionState.Closed Then
                                    cnnDescDB.Open()     'if the connection failure except spare IPCs, send email 
                                End If
                            Catch ex As Exception
                                Dim strSubject As String
                                If UCase(LTrim(row("PackagingLine"))) <> "SPARE" Then
                                    strSubject = "Error on " & row("ComputerName") & " - Env:" & strPowerPlantDB
                                    strMsg = "Error in exporting data to IPC, " & row("ComputerName") & ", staging area - " & ex.Message
                                    WriteLog(strLogFileName, strMsg)
                                    sndEmail(strMsg, cnnLocalDB, strSubject)
                                End If
                                Continue For
                            End Try
                            sqlCmd.CommandText = strSQL
                            sqlCmd.Connection = cnnDescDB
                            sqlCmd.CommandType = CommandType.Text
                            sqlCmd.ExecuteNonQuery()

                            'loop through the table download list to export the tables on the list to IPC
                            For Each strTableName In arlTableName
                                'For Spare IPC and the table has facility column than use Delete statement otherwise use Truncate statement
                                If UCase(row("PackagingLine").ToString.Trim) = "SPARE" AndAlso sdTableName.Item(strTableName) = True Then
                                    strSQL = "DELETE " + strTableName + " Where facility = '" & strFacility & "'"
                                Else
                                    strSQL = "Truncate table " & strTableName
                                End If

                                Using cmd2 As New SqlCommand(strSQL, cnnDescDB)
                                    cmd2.ExecuteNonQuery()
                                End Using

                                strMsg = "Exporting table " & strTableName & " to IPC - " & row("ComputerName")
                                WriteLog(strLogFileName, strMsg)

                                'Copy tables to the staging DB in IPC.
                                Using bcp As SqlBulkCopy = New SqlBulkCopy(strDestCnnStr, SqlBulkCopyOptions.TableLock)
                                    bcp.BulkCopyTimeout = 1800  ' set processing timeout in Seconds
                                    bcp.DestinationTableName = strTableName
                                    bcp.WriteToServer(ds.Tables(strTableName))  'Save the data to a table of the dataset 
                                End Using

                                'In the DownloadTablelist Table of IPC staging area, update the download status flag = 1 (i.e. success) 
                                'and set the record Active to indicate the table is ready for download to local database.
                                strSQL = "Update tblDownLoadTableList set DownLoadStatus = 1, Active = 1, LastDownload = getdate() WHERE Facility = '" & strFacility & " ' AND TableName ='" & strTableName & "'"
                                sqlCmd.CommandText = strSQL
                                sqlCmd.Connection = cnnDescDB
                                sqlCmd.CommandType = CommandType.Text
                                sqlCmd.ExecuteNonQuery()

                                blnAtLeast1Success = True

                            Next    'Loop ended for Table Names

                            If blnAtLeast1Success Then
                                ' Set Ready_For_DownLoad status to Yes in the Power Plant SQL server for audit purpose
                                strSQL = "Update tblComputerConfig set ReadyForDownLoad = 1 WHERE Facility = '" & strFacility & " ' and ComputerName = '" & row("ComputerName") & "'"
                                sqlCmd.CommandText = strSQL
                                sqlCmd.Connection = cnnLocalDB
                                sqlCmd.CommandType = CommandType.Text
                                sqlCmd.ExecuteNonQuery()

                                'Set Ready_For_DownLoad status to Yes in the remote IPC
                                strSQL = "Update tblComputerConfig SET ReadyForDownLoad = 1 WHERE Facility = '" & strFacility & " ' and ComputerName = '" & row("ComputerName") & "'"
                                sqlCmd.CommandText = strSQL
                                sqlCmd.Connection = cnnDescDB
                                sqlCmd.CommandType = CommandType.Text
                                sqlCmd.ExecuteNonQuery()
                            End If

                            strMsg = "End exporting data to IPC - " & row("ComputerName")
                            WriteLog(strLogFileName, strMsg)
                            cnnDescDB.Close()
                        End Using
                    Catch ex As Exception
                        Dim strSubject As String
                        If UCase(LTrim(row("PackagingLine"))) <> "SPARE" Then
                            strSubject = "Error on " & row("ComputerName") & " - Env:" & strPowerPlantDB
                            strMsg = "Error in exporting data to IPC, " & row("ComputerName") & ", staging area - " & ex.Message
                            WriteLog(strLogFileName, strMsg)
                            sndEmail(strMsg, cnnLocalDB, strSubject)
                        End If
                        Continue For
                    End Try
                Next    'Loop ended for IPC

                'Reset the Active flag in the Down Load table list to indicate all the files have been successfully downloaded
                strSQL = "Update tblDownLoadTableList set Active = 0 WHERE Facility = '" & strFacility & " '"
                sqlCmd.CommandText = strSQL
                sqlCmd.Connection = cnnLocalDB
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.ExecuteNonQuery()

                cnnLocalDB.Close()
            End Using
        Catch ex As Exception
            WriteLog(strLogFileName, ex.Message)
            Throw ex
        End Try
    End Sub

    Sub WriteLog(ByVal strFileName As String, ByVal strMsg As String)
        Try
            Console.Write(DateTime.Now & " - ")
            Console.WriteLine(strMsg)
            If My.Computer.FileSystem.FileExists(strFileName) = False Then
                My.Computer.FileSystem.WriteAllText(strFileName, String.Empty, False)
            End If
            My.Computer.FileSystem.WriteAllText(strFileName, DateTime.Now & " - " & strMsg & vbCrLf, True)
        Catch ex As Exception
            Throw New Exception("- <Error> in WriteLog " & ex.Message)
        End Try
    End Sub

    Sub sndEmail(ByVal strMsgBody As String, ByVal cnnLocalDB As SqlConnection, Optional ByVal strSubject As String = Nothing) 'WO#359
        Dim arParms(1) As SqlParameter
        Dim strSQL As String

        Try
            strSQL = "PPsp_SndMsgForSupport"
            arParms = New SqlParameter(UBound(arParms)) {}

            ' Message Body Input Parameter
            arParms(0) = New SqlParameter("@vchMsgBody", SqlDbType.VarChar, Integer.MaxValue)
            arParms(0).Value = strMsgBody

            ' Message Subject Input Parameter                                       
            arParms(1) = New SqlParameter("@vchSubject", SqlDbType.VarChar, 512)
            arParms(1).Value = strSubject

            Using cmd As New SqlCommand
                cmd.Parameters.Add(arParms(0))
                cmd.Parameters.Add(arParms(1))
                cmd.CommandText = strSQL
                cmd.Connection = cnnLocalDB
                cmd.CommandType = CommandType.StoredProcedure
                cmd.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            Throw New Exception("Error in sndEmail - " & ex.Message)
        End Try
    End Sub
End Module
