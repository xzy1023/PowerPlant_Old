Imports System.Diagnostics
Imports System.Text
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient


Public Class SharedFunctions
    Public Structure objectsToInstall
        'Declare data members
        Public PgmModule As String
        Public FileFolder As String
        Public FileNames() As String
        Public CurVersion As String
    End Structure

    Public Structure DeploymentInfo
        Dim strPlant As String
        Dim intDeploymentID As Integer
        Dim strEnv As String
        Dim strServerOrIPC As String
        Dim strDeploymentType As String
    End Structure

    Private Sub New()
    End Sub

    Public Shared Function Deploy(ByVal strComputerName As String, ByVal strSourceFolder As String, ByVal arlPgmModels As ArrayList, ByVal blnRestartIPC As Boolean, ByVal txtLog As TextBox, ByVal stuDplymtInfo As DeploymentInfo) As String
        Dim strTargetFolder As String
        Dim strFileNames() As String
        Dim strCurVersion As String
        Dim strProcessName As String
        Dim sb As New StringBuilder
        Dim strCommand As String
        Dim intRtnCode As Integer
        Dim stuPM As objectsToInstall

        Try

            With sb

                For Each stuPM In arlPgmModels
                    With stuPM
                        strProcessName = .PgmModule
                        strTargetFolder = .FileFolder
                        strFileNames = .FileNames
                        strCurVersion = .CurVersion

                        'Kill the proess before deploy if it is still running on the IPC
                        sb.Append(KillProcessOnRemoteIPC(strProcessName, strComputerName))
                    End With

                    'Copy the file(s) over the IPC
                    For Each strFileName As String In strFileNames
                        If UCase(strFileName) = "*ALL" Then
                            sb.Append(XCopyFolders(strSourceFolder, strTargetFolder, strComputerName, True))
                        Else
                            sb.Append(CopyFileToRemoteIPC(strTargetFolder, strComputerName, strSourceFolder, strFileName, strCurVersion))
                        End If
                    Next

                Next

                'Restart the IPC, so it will start to run the new programs based on the default account.
                If blnRestartIPC = True Then
                    strCommand = "ShutDown  /m \\" & strComputerName & " /r /t 5"
                    intRtnCode = Microsoft.VisualBasic.Shell(strCommand, AppWinStyle.Hide, True)
                    .AppendFormat("Restarted {0}.", strComputerName).AppendLine()
                    .AppendLine("------------------")
                End If
                Return sb.ToString

            End With
        Catch ex As Exception
            UpdateLog(txtLog, sb.ToString, stuDplymtInfo)
            Throw New Exception("Error in Deploy" & vbCrLf & ex.Message)
        End Try
    End Function

    'Public Shared Function DeployProbatClient(strComputerName As String, ByVal strProbatSourceFolder As String, ByVal arlPgmModels As ArrayList, strCopyType As String, txtLog As TextBox, stuDplymtInfo As DeploymentInfo) As String
    '    Dim sb As New StringBuilder
    '    Dim stuPM As objectsToInstall
    '    Dim strTargetFolder As String = ""
    '    Dim strFileNames() As String
    '    Dim strCurVersion As String
    '    Dim strProcessName As String

    '    Try

    '        sb.AppendFormat("Copy {0} to - {1} started", strComputerName, strCopyType)
    '        sb.AppendLine("")
    '        SharedFunctions.UpdateLog(txtLog, sb.ToString, stuDplymtInfo)
    '        sb.Length = 0

    '        If strCopyType = "Folders" Then

    '            sb.AppendLine(SharedFunctions.XCopyFolders(strProbatSourceFolder, strTargetFolder, strComputerName, chkDelProbatClient.IsChecked))
    '        Else

    '            For Each stuPM In arlPgmModels
    '                With stuPM
    '                    strProcessName = .PgmModule
    '                    strTargetFolder = .FileFolder
    '                    strFileNames = .FileNames
    '                    strCurVersion = .CurVersion
    '                End With
    '                sb.AppendLine(SharedFunctions.Deploy(strComputerName, strProbatSourceFolder, arlPgmModels, 0, txtLog, stuDplymtInfo))
    '            Next
    '        End If
    '        Return sb.ToString
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message & " - error in DeployProbatClient", ex)
    '    End Try
    'End Function

    Private Shared Function KillProcessOnRemoteIPC(ByVal strProcessName As String, ByVal strComputerName As String) As String
        Dim sb As New StringBuilder
        Dim proclist() As Process
        Dim intRtnCode As Integer

        Try
            With sb
                'Shell("taskkill /s " & strComputerName & " /IM " & strProcessName & ".exe", AppWinStyle.NormalFocus, True)
                'If the named process is found in the process list on the remote PC, terminate the process within 5 seconds
                For i As Int16 = 1 To 2

                    proclist = Process.GetProcessesByName(strProcessName, strComputerName)
                    If proclist.Length > 0 Then
                        For Each p As Process In proclist
                            intRtnCode = Microsoft.VisualBasic.Shell("taskkill /s " & strComputerName & " /PID " & p.Id, AppWinStyle.NormalFocus, True)
                            'Dim strCommand As String
                            'strCommand = "taskkill /s " & strComputerName & " /PID " & p.Id
                            '.AppendLine(ExecDos("sqlcmd.exe", strCommand))
                            '.AppendFormat("{0} has been terminated in {1}.", strProcessName, strComputerName).AppendLine()
                        Next
                        Exit For
                    Else
                        If i = 2 Then
                            .AppendFormat("{0} is not running in {1}.", strProcessName, strComputerName).AppendLine()
                        Else
                            System.Threading.Thread.Sleep(1000)
                        End If
                    End If
                Next
            End With
            Return sb.ToString

        Catch ex As Exception
            Throw New Exception("Error in KillProcessOnRemoteIPC" & vbCrLf & ex.Message)
        End Try
    End Function

    Private Shared Function CopyFileToRemoteIPC(ByVal strTargetFolder As String, ByVal strComputerName As String, ByVal strSourceFolder As String, _
                                     ByVal strFileName As String, ByVal strCurVersion As String) As String
        Dim sb As New StringBuilder
        Dim strRenameTo As String

        Try

            With sb
                strTargetFolder = "\\" & strComputerName & "\" & strTargetFolder

                'Copy the target file to another file with given version number on the remote PC for versioning backup
                If strCurVersion <> String.Empty Then
                    If InStrRev(strFileName, ".config") > 0 Then
                        strRenameTo = strFileName.Replace(".config", "_" & strCurVersion & ".config")
                    Else
                        strRenameTo = strFileName.Replace(".", "_" & strCurVersion & ".")
                    End If
                    Try
                        File.Copy(Path.Combine(strTargetFolder, strFileName), Path.Combine(strTargetFolder, strRenameTo), True)
                        .AppendFormat("{0} was saved to {1}.", strTargetFolder & "\" & strFileName, strRenameTo).AppendLine()
                    Catch ex As Exception
                        .AppendLine(ex.Message)
                    End Try
                End If
                'Copy file from local to remote PC with overwritting
                File.Copy(Path.Combine(strSourceFolder, strFileName), Path.Combine(strTargetFolder, strFileName), True)

                .AppendFormat("Copy {0} to {1}.", strSourceFolder & "\" & strFileName, strTargetFolder).AppendLine()
            End With
            Return sb.ToString

        Catch ex As Exception
            Throw New Exception("Error in CopyFileToRemoteIPC" & vbCrLf & ex.Message)
        End Try
    End Function

    'xcopy folders
    Public Shared Function XCopyFolders(ByVal strSourcePath As String, ByVal strDestPath As String, ByVal strComputerName As String, ByVal blnDltFolderFirst As Boolean) As String
        Dim strCommand As String
        Dim sb As New StringBuilder
        Dim strTargetFolder As String

        Try
            XCopyFolders = String.Empty
            If My.Computer.Network.Ping(strComputerName, 1500) Then
                strTargetFolder = "\\" & strComputerName & "\" & strDestPath

                sb.AppendFormat("Deploy folder - {0} to {1}", strSourcePath, strTargetFolder)
                sb.AppendLine()

                If blnDltFolderFirst = True Then
                    If Directory.Exists(strTargetFolder) Then
                        strCommand = String.Format("cmd /c Rmdir /s/q ""{0}""", strTargetFolder)
                        Microsoft.VisualBasic.Shell(strCommand, AppWinStyle.Hide, True)
                        sb.AppendFormat("Delete folder {0}", strTargetFolder)
                        sb.AppendLine("")
                    End If
                End If

                strCommand = String.Format("xcopy ""{0}"" ""{1}"" /e/y/i/z/q/k", strSourcePath, strTargetFolder)

                Microsoft.VisualBasic.Shell(strCommand, AppWinStyle.Hide, True)
                If Directory.Exists(strTargetFolder) Then
                    sb.AppendFormat("Size of folder {0} is {1}", strTargetFolder, GetDirectorySize(strTargetFolder))
                    sb.AppendLine("")
                    sb.AppendLine("Copy folder Success")
                Else
                    sb.AppendLine("Destination folder not found. Copy was Failure")
                End If
                sb.AppendLine()
                Return sb.ToString
            End If
        Catch ex As Exception
            Throw New Exception("Error in XCopyFolders" & vbCrLf & ex.Message)
        End Try
    End Function
    ' CopyDirectory Function is for future use
    Public Shared Sub CopyDirectory(ByVal sourcePath As String, ByVal destPath As String)
        Dim sb As New StringBuilder
        Try

            'Create the destination folder if not exist
            If Not Directory.Exists(destPath) Then
                Directory.CreateDirectory(destPath)
            End If

            'Copy every file from the source folder to the destination folder
            For Each file__1 As String In Directory.GetFiles(sourcePath)
                Dim dest As String = Path.Combine(destPath, Path.GetFileName(file__1))
                File.Copy(file__1, dest)
            Next

            'Copy every folder from the source folder to the destination folder
            For Each folder As String In Directory.GetDirectories(sourcePath)
                Dim dest As String = Path.Combine(destPath, Path.GetFileName(folder))
                CopyDirectory(folder, dest)
            Next

        Catch ex As Exception
            Throw New Exception("Error in CopyDiretory" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Shared Function GetDirectorySize(ByVal sPath As String) As Long
        Dim lngSize As Long = 0
        Dim diDir As New System.io.DirectoryInfo(sPath)
        Dim fil As System.IO.FileInfo
        Try
            If diDir.Exists Then
                For Each fil In diDir.GetFiles()
                    lngSize += fil.Length
                Next fil

                ' Recursively call the function
                Dim subDirInfo As System.IO.DirectoryInfo
                For Each subDirInfo In diDir.GetDirectories()
                    lngSize += GetDirectorySize(subDirInfo.FullName)
                Next subDirInfo
            End If
            Return lngSize

        Catch ex As Exception
            Throw New Exception("Error in GetDiretorySize" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Function DeploySQL(ByVal txtLog As TextBox, ByVal strSQLInstance As String, ByVal strDataBase As String, ByVal strSourceFolderSQL As String, ByVal dtDeployItems As dsDeployItems.tblDeploymentListDataTable, ByVal strSingleItem As String, ByVal stuDplymtInfo As DeploymentInfo) As String

        'Dim strCommand As String
        Dim drDeployItem As dsDeployItems.tblDeploymentListRow = Nothing
        Dim sb As New StringBuilder
        Dim da As New dsDeployItemsTableAdapters.tblDeploymentListTableAdapter
        Dim strErrMsg As String = ""

        Try
            If strSingleItem <> "*ALL" Then
                For Each drDeployItem In dtDeployItems
                    If drDeployItem.FileName = strSingleItem Then
                        Exit For
                    End If
                Next
                sb = DeploySQLItem(strSQLInstance, strDataBase, strSourceFolderSQL, strSingleItem, drDeployItem)
                ''deployASQLItem(strSQLInstance, strDataBase, strSourceFolderSQL, strSingleItem)
                'strCommand = String.Format("-S {0} -d {1} -i ""{2}""", strSQLInstance, strDataBase, strSourceFolderSQL & "\" & strSingleItem)
                '' UpdateLog(txtLog, ExecDos("sqlcmd.exe", strCommand))
                'sb.AppendFormat("Deploy - {0}", strSingleItem)
                'sb.AppendLine()
                'sb.AppendFormat("sqlcmd.exe {0}", strSingleItem)
                'strErrMsg = ExecDos("sqlcmd.exe", strCommand)
                'If strErrMsg <> "" Then
                '    sb.AppendLine(strErrMsg)
                'Else
                '    ' update deployment date on the Deployment List table if no error on deployment
                '    For Each dr In dtDeployItems
                '        If dr.FileName = strSingleItem Then
                '            dr.DeploymentDate = Now()
                '            Try
                '                da.Update(dr)
                '            Catch ex1 As Exception
                '            End Try
                '            Exit For
                '        End If

                '    Next
                'End If
                'sb.AppendLine()
                UpdateLog(txtLog, sb.ToString, stuDplymtInfo)
            Else
                'UpdateLog(txtLog, "** - " & strSQLInstance)
                For Each drDeployItem In dtDeployItems
                    sb = DeploySQLItem(strSQLInstance, strDataBase, strSourceFolderSQL, drDeployItem.FileName, drDeployItem)
                    ''strCommand = String.Format("sqlcmd -S {0} -d {1} -i ""{2}""", strSQLInstance, strDataBase, strSourceFolderSQL & "\" & dr.FileName)
                    'strCommand = String.Format("-S {0} -d {1} -i ""{2}""", strSQLInstance, strDataBase, strSourceFolderSQL & "\" & dr.FileName)
                    ''Shell(strCommand, AppWinStyle.NormalFocus, True)
                    ''UpdateLog(txtLog, "Deploy - " & dr.FileName)
                    ''UpdateLog(txtLog, ExecDos("sqlcmd.exe", strCommand))
                    'sb.AppendFormat("Deploy - {0}", dr.FileName)
                    'sb.AppendLine()
                    'strErrMsg = ExecDos("sqlcmd.exe", strCommand)
                    'If strErrMsg <> "" Then
                    '    sb.AppendLine(strErrMsg)
                    'Else
                    '    ' update deployment date on the Deployment List table if no error on deployment
                    '    dr.DeploymentDate = Now()
                    '    Try
                    '        da.Update(dr)
                    '    Catch ex1 As Exception
                    '    End Try
                    'End If
                    'sb.AppendLine()
                    UpdateLog(txtLog, sb.ToString, stuDplymtInfo)

                Next
            End If

            'If strErrMsg = "" Then
            If Not sb.ToString.Contains("** ERROR **") Then
                Return String.Format("Deployment on {0},{1} completed.", strSQLInstance, strDataBase)
            Else
                Return String.Format("Deployment on {0},{1} contains error(s).", strSQLInstance, strDataBase)
            End If

        Catch ex As Exception
            Try
                UpdateLog(txtLog, sb.ToString, stuDplymtInfo)
            Catch ex1 As Exception
                MessageBox.Show(ex1.Message)
            End Try
            Throw New Exception("Error in DeploySQ" & vbCrLf & ex.Message)
        End Try

    End Function

    Public Shared Function DeploySQLItem(strSQLInstance As String, strDataBase As String, strSourceFolderSQL As String, strFileName As String, dr As dsDeployItems.tblDeploymentListRow) As StringBuilder
        Dim strCommand As String
        Dim strErrMsg As String = ""
        Dim sb As New StringBuilder
        Dim da As New dsDeployItemsTableAdapters.tblDeploymentListTableAdapter
        Try
            strCommand = String.Format("-S {0} -d {1} -i ""{2}""", strSQLInstance, strDataBase, strSourceFolderSQL & "\" & strFileName)
            sb.AppendFormat("Deploy - {0}", strFileName)
            sb.AppendLine()
            strErrMsg = ExecDos("sqlcmd.exe", strCommand)

            ' If strErrMsg <> "" AndAlso ((strErrMsg.IndexOf("rows affected") = -1 AndAlso strErrMsg.IndexOf("Changed database context") <> 0) Or strErrMsg.IndexOf("level 16") = -1) Then
            If strErrMsg <> "" Then
                sb.AppendLine(strErrMsg)
            End If

            If Not strErrMsg.Contains("** ERROR **") Then
                dr.DeploymentDate = Now()
                Try
                    da.Update(dr)
                Catch ex1 As Exception
                End Try
            End If
            sb.AppendLine()
            Return (sb)
        Catch ex As Exception
            Throw (ex)
        End Try
    End Function

    Public Shared Function IPC_DBBackup(ByVal txtLog As TextBox, ByVal strSQLInstance As String, ByVal strSQLFileName As String, ByVal stuDplymtInfo As DeploymentInfo) As String
        Dim strCommand As String

        Try
            strCommand = String.Format("-S {0} -d {1} -i ""{2}""", strSQLInstance, "Master", strSQLFileName)
            'Shell(strCommand, AppWinStyle.NormalFocus, True)
            UpdateLog(txtLog, ExecDos("sqlcmd.exe", strCommand), stuDplymtInfo)

            Return String.Format("Data base backup on {0} completed.", strSQLInstance)
        Catch ex As Exception
            Throw New Exception("Error in IPC_DBBackup" & vbCrLf & ex.Message)
        End Try
    End Function


    Public Shared Function IPC_DBRestore(ByVal txtLog As TextBox, ByVal strSQLInstance As String, ByVal strSQLFileName As String, ByVal stuDplymtInfo As DeploymentInfo) As String
        Dim strCommand As String

        Try
            strCommand = String.Format("-S {0} -d {1} -i ""{2}""", strSQLInstance, "Master", strSQLFileName)
            UpdateLog(txtLog, ExecDos("sqlcmd.exe", strCommand), stuDplymtInfo)

            Return String.Format("Data base Restore on {0} completed.", strSQLInstance)
        Catch ex As Exception
            Throw New Exception("Error in IPC_DBRestore" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Shared Function GetComputerList(ByVal strServerName As String, ByVal strEnv As String, ByVal strDBName As String) As dsIPCNames.tblcomputerConfigDataTable
        Dim daCF As New dsIPCNamesTableAdapters.tblcomputerConfigTableAdapter
        Dim dtCF As New dsIPCNames.tblcomputerConfigDataTable
        Dim strConn As String
        Try

            strConn = ChangeConnectionString(My.Settings.strConn, strServerName, strEnv, strDBName)

            daCF.Connection.ConnectionString = strConn
            daCF.Fill(dtCF)

            Return dtCF

        Catch ex As Exception
            Throw New Exception(ex.Message & " - error in GetComputerList", ex)
        End Try

    End Function

    Public Shared Function ChangeConnectionString(ByVal strOrginCnn As String, ByVal strNewServerName As String, ByVal strEnv As String, ByVal strDBName As String) As String
        Dim strCnn As String
        Try
            strCnn = strOrginCnn.Replace("MPHOPP01", strNewServerName)

            strCnn = strCnn.Replace("PowerPlant_Deployment", strDBName)
            'If strNewServerName = "MPHOTEST05" Then
            '    If strEnv = "Dev" Then
            '        strCnn = strCnn.Replace("PowerPlant_Deployment", "PowerPlantAX_" & strEnv)
            '    Else
            '        strCnn = strCnn.Replace("PowerPlant_Deployment", "PowerPlantAX_" & strEnv)
            '    End If
            'Else
            '    strCnn = strCnn.Replace("PowerPlant_Deployment", "PowerPlant_" & strEnv)
            'End If
                Return strCnn
        Catch ex As Exception
            Throw New Exception(ex.Message & " - error in ChangeConnectionString", ex)
        End Try

    End Function

    Public Shared Function ExecDos(ByVal strCommand As String, ByVal strArguments As String) As String
        Dim p As New Process
        Dim psi As New ProcessStartInfo
        Try

            With psi
                .UseShellExecute = False
                .RedirectStandardOutput = True
                .RedirectStandardError = True
                '.Arguments = "www.google.com"
                '.WorkingDirectory = "C:\windows\system32" 'this for nt* computers 
                '.FileName = "ping"
                .Arguments = strArguments
                '.WorkingDirectory = strWorkingDirectory
                .FileName = strCommand
                .WindowStyle = ProcessWindowStyle.Hidden
                '.WindowStyle = ProcessWindowStyle.Normal

            End With

            p.StartInfo = psi
            p.Start()
            p.WaitForExit()

            Dim sr As IO.StreamReader = p.StandardOutput
            Dim sb As New System.Text.StringBuilder("")
            Dim input As Integer = sr.Read
            Do Until (input = -1)
                sb.Append(ChrW(input))
                input = sr.Read
            Loop
            If sb.Length > 0 AndAlso (sb.ToString.Contains("Level 16") Or sb.ToString.Contains("Level 15")) Then
                sb.Append(" ** ERROR ** ")
            End If

            Dim srErr As IO.StreamReader = p.StandardError
            Dim sbErr As New System.Text.StringBuilder("")
            Dim inputErr As Integer = srErr.Read
            Do Until (inputErr = -1)
                sbErr.Append(ChrW(inputErr))
                inputErr = srErr.Read
            Loop
            If sbErr.Length > 0 Then
                sbErr.Append(" ** ERROR ** ")
            End If
            sb.Append(sbErr.ToString)

            Return sb.ToString

        Catch ex As Exception
            Throw (New Exception(ex.Message & " - error in ExecDos", ex))
        End Try
    End Function

    Public Shared Sub UpdateLog(ByVal txtLog As TextBox, ByVal strMsg As String, ByVal stuDI As DeploymentInfo)
        Dim cmd As New SqlCommand
        Dim cnn As New SqlConnection
        Dim strLogFile As String = My.Settings.strLogFile & "Log.txt"

        Try

            If strMsg <> String.Empty Then
                strMsg = Now() & " " & strMsg
            End If
            txtLog.Text = txtLog.Text & strMsg
            File.AppendAllText(strLogFile, strMsg)

            cnn.ConnectionString = My.Settings.strConn
            cnn.Open()

            Dim strSQLStmt As String

            strSQLStmt = "INSERT INTO tblDeploymentLog " +
                         "(Plant, DeploymentID, Environment, ServerOrIPC, DeploymentType, Message) " +
                         "VALUES (@Plant,@DeploymentID,@Environment,@ServerOrIPC,@DeploymentType,@Message)"

            With cmd
                .Connection = cnn
                .CommandType = CommandType.Text
                .CommandText = strSQLStmt
                .Parameters.Add(New SqlParameter("@Plant", SqlDbType.VarChar, 10)).Value = stuDI.strPlant
                .Parameters.Add(New SqlParameter("@DeploymentID", SqlDbType.Int)).Value = stuDI.intDeploymentID
                .Parameters.Add(New SqlParameter("@Environment", SqlDbType.VarChar, 3)).Value = stuDI.strEnv
                .Parameters.Add(New SqlParameter("@ServerOrIPC", SqlDbType.Char, 1)).Value = stuDI.strServerOrIPC
                .Parameters.Add(New SqlParameter("@DeploymentType", SqlDbType.VarChar, 10)).Value = stuDI.strDeploymentType
                .Parameters.Add(New SqlParameter("@Message", SqlDbType.VarChar, 1000)).Value = strMsg
                .ExecuteNonQuery()
            End With

        Catch ex As Exception
            Throw ex
        Finally
            cnn.Close()
        End Try
    End Sub

    Public Shared Sub chgRegistry(ByVal strComputerName As String, ByVal strRegistryHive As String, ByVal strSubKeyPath As String, ByVal strKeyName As String, ByVal strNewValue As String)
        Dim rkODBC As Microsoft.Win32.RegistryKey
        Try
            If strRegistryHive = "LocalMachine" Then
                rkODBC = Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, strComputerName).OpenSubKey(strSubKeyPath, True)
                rkODBC.SetValue(strKeyName, strNewValue)
                rkODBC.Close()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub ConfigureProbatClientRegistry(ByVal strComputerName As String, strCfgProbatClientRegistryBatchFile As String, txtLog As TextBox, gstuDplymtInfo As DeploymentInfo)
        Dim sb As New System.Text.StringBuilder
        Dim strCommand As String = String.Empty
        Dim strArgument As String = String.Empty
        Try

            sb.AppendFormat("Configure Probat Client registry key for {0} started", strComputerName)
            SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
            sb.Length = 0
            If My.Computer.Network.Ping(strComputerName, 1500) Then
                strCommand = "C:\PSTools\PSexec"
                strArgument = String.Format("\\{0} -s -i ""{1}"" ", strComputerName, strCfgProbatClientRegistryBatchFile)
                sb.AppendLine(SharedFunctions.ExecDos(strCommand, strArgument))

                'strCommand = strCommand & " " & strArgument
                'Shell(strCommand, AppWinStyle.Hide, True)
                sb.AppendFormat("Configure Probat Client registry key for {0} completed", strComputerName)
            Else
                sb.AppendFormat("** Can not connect to {0}. Configure Probat Client registry key failure.", strComputerName)
            End If
            sb.AppendLine()
            SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
        Catch ex As Exception
            Throw (New Exception(ex.Message & " - error in ConfigureProbatClientRegistry", ex))
        End Try
    End Sub

    Private Shared Sub DeleteDirectory(path As String)

        If Directory.Exists(path) Then

            'Delete all files from the Directory

            For Each filepath As String In Directory.GetFiles(path)

                File.Delete(filepath)

            Next

            'Delete all child Directories

            For Each dir As String In Directory.GetDirectories(path)

                DeleteDirectory(dir)

            Next

            'Delete a Directory

            Directory.Delete(path)

        End If

    End Sub

End Class
