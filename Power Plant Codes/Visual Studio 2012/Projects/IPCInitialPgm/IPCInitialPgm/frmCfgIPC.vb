Imports System.IO
Imports System.IO.Compression
Imports System.Security.AccessControl
Imports System.Data.SqlClient

'Assumption: The content of tblComputerConfig in Server and the staging area is synchronous.

Public Class frmCfgIPC

    Private Sub frmCfgIPC_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.UcHeading1.lblScreenTitle.Text = "Configure IPC"
        lblMsg.Text = ""
        Try
            Me.PPsp_Facility_SelTableAdapter.Fill(Me.DsFacility.PPsp_Facility_Sel, "SelByRegion", "Facility")
            Me.TblComputerConfigTableAdapter.Fill(Me.DsComputerCfg.tblComputerConfig, Me.cboFacility.SelectedValue)
        Catch ex As SqlException
            MessageBox.Show("Please connect the IPC to the network and try again.")
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error in frmCfgIPC_Load" & vbCrLf & ex.Message)
            Me.Close()
        End Try
    End Sub

    Private Sub btnPrvScn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click

        Dim strIndusoftPgmName As String = Nothing
        'Dim blnAttachedPLC As Boolean
        Dim siteResponds As Boolean = False
        Dim drResponse As DialogResult
        Dim tblCmpCfg As New dsComputerCfg.tblComputerConfigDataTable
        Dim strMyComputerName As String = My.Computer.Name
        Dim strIPCToBeReplaced As String = cboPkgLine.SelectedValue
        ' Dim myProcess As Process
        Dim cmd As New SqlCommand()
        Dim conn As New SqlConnection()
        Try
            Dim strSelectedPkgLine As String = Me.DsComputerCfg.tblComputerConfig.Rows(cboPkgLine.SelectedIndex).Item("PackagingLine")
            Me.Cursor = Cursors.WaitCursor

            If strIPCToBeReplaced = strMyComputerName Then
                drResponse = MessageBox.Show("This IPC, " & strIPCToBeReplaced & " is already configured for the selected packaging line. Do you still want to " & _
                                "continue?", "Warning", MessageBoxButtons.YesNo)
                If drResponse = Windows.Forms.DialogResult.No Then
                    Return
                Else
                    'if reconfigure the IPC with the current packaging line, do not check the connectivity
                    siteResponds = False
                End If
            Else
                'Is the IPC connected to the network?
                Try
                    siteResponds = My.Computer.Network.Ping(strIPCToBeReplaced)
                Catch ex As Exception
                End Try
            End If

            If siteResponds Then
                MessageBox.Show("The IPC for the selected packaging line is still connecting to the network. Please select the correct Line " & _
                                "or unplug the network cable for the IPC, " & strIPCToBeReplaced & " which is to be replaced and try again.", "Invalid Selection")
            Else
                TblComputerConfigTableAdapter.Connection.ConnectionString = My.Settings.gstrServerCnnString
                Me.TblComputerConfigTableAdapter.FillByComputer(tblCmpCfg, "SelectAllFields", strMyComputerName, Nothing)
                If tblCmpCfg.Rows.Count > 0 AndAlso tblCmpCfg.Rows(0)("PackagingLine") <> strSelectedPkgLine Then
                    drResponse = MessageBox.Show("This IPC is currently configured for line " & RTrim(tblCmpCfg.Rows(0)("PackagingLine")) & _
                                ". Do you sure to continue?", "Warning on Selection", MessageBoxButtons.YesNo)
                    If drResponse = Windows.Forms.DialogResult.No Then
                        Return
                    End If
                End If

                lblMsg.Text = "Updating IPC configurations ..."
                'lblMsg.Visible = True
                Me.Refresh()
                'This DSComputerCfg is defined from the screen.
                With Me.DsComputerCfg.tblComputerConfig.Rows(cboPkgLine.SelectedIndex)
                    strSelectedPkgLine = .Item("packagingline")
                    'blnAttachedPLC = .Item("attachedPLC")
                    'If blnAttachedPLC = True Then
                    If Not IsDBNull(.Item("IndusoftPgmName")) Then
                        strIndusoftPgmName = .Item("IndusoftPgmName")
                    End If
                    'End If
                End With

                'If the facility of to-be-replaced IPC is different from the spare IPC, change the facility of the tables
                'of the local database of the spare IPC to the facility of the to-be-replaced IPC.

                conn.ConnectionString = My.Settings.gstrLocalCnnString
                With cmd
                    .Connection = conn
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "LPPsp_SwapIPCFacility"

                    ' Create a SqlParameter for each parameter in the stored procedure.
                    Dim prmIPCToBeReplaced As New SqlParameter("vchIPCToBeReplaced ", SqlDbType.VarChar, 50)
                    .Parameters.Add(prmIPCToBeReplaced)
                    prmIPCToBeReplaced.Value = strIPCToBeReplaced
                    .Connection.Open()
                    .ExecuteNonQuery()

                    .CommandText = "CPPsp_ClearLocalFiles"
                    .Parameters.Clear()
                    .ExecuteNonQuery()

                End With
                'Update server database
                'Update computer name in the Computer Configuration Table on server and local database
                TblComputerConfigTableAdapter.Connection.ConnectionString = My.Settings.gstrServerCnnString
                For i As Short = 1 To 2
                    Try
                        If strIPCToBeReplaced = strMyComputerName Then
                            'soft delete the record with the spare IPC name in the Computer Configuration Table
                            TblComputerConfigTableAdapter.SoftDelete(strMyComputerName)
                        Else
                            TblComputerConfigTableAdapter.Delete(strMyComputerName)
                        End If
                    Catch ex As Exception
                    End Try
                    'Update the selected packaging line with the spare IPC computer name in the Computer Configuration Table
                    Me.TblComputerConfigTableAdapter.Update(strMyComputerName, strSelectedPkgLine)

                    'Update local database
                    If i = 1 Then
                        TblComputerConfigTableAdapter.Connection.ConnectionString = My.Settings.gstrLocalCnnString
                    End If
                Next

                'Update Session control table in the local DB
                Me.TblComputerConfigTableAdapter.FillByComputer(tblCmpCfg, "SelectAllFields", strMyComputerName, Nothing)

                Dim taSessCtl As New dsSessCtlTableAdapters.tblSessionControlTableAdapter
                taSessCtl.Update(tblCmpCfg.Rows(0)("Facility"), strMyComputerName, strSelectedPkgLine)

                'Download the indusoft program only if the line is attached to the PLC.
                'If tblCmpCfg.Count > 0 AndAlso DsComputerCfg.tblComputerConfig.Rows(0)("attachedPLC") = True Then
                'If blnAttachedPLC = True Then
                If Not IsNothing(strIndusoftPgmName) Then
                    lblMsg.Text = "Downloading the specific HMI program for the line ..."
                    Dim fileExists As Boolean
                    fileExists = My.Computer.FileSystem.FileExists(My.Settings.gstrDLDestination)
                    If fileExists Then
                        My.Computer.FileSystem.DeleteFile(My.Settings.gstrDLDestination, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
                        'My.Computer.FileSystem.DeleteDirectory(My.Settings.gstrDLDestination, FileIO.DeleteDirectoryOption.DeleteAllContents, FileIO.RecycleOption.DeletePermanently)
                    End If

                    My.Computer.Network.DownloadFile(My.Settings.gstrDLSource & strIndusoftPgmName, My.Settings.gstrDLDestination)

                    'Delete existing Indusoft folder and programs
                    Dim blnFolderExists As Boolean
                    Dim strFolderName As String
                    strFolderName = My.Settings.strInduSoftAppPgm
                    strFolderName = strFolderName.Substring(0, strFolderName.LastIndexOf("\"))
                    'strFolderName = strFolderName.Replace("PowerPlant.App", "")
                    blnFolderExists = My.Computer.FileSystem.DirectoryExists(strFolderName)
                    If blnFolderExists Then
                        My.Computer.FileSystem.DeleteDirectory(strFolderName, FileIO.DeleteDirectoryOption.DeleteAllContents)
                    End If
                    'UnZip the file 
                    lblMsg.Text = "Unzip the softwares ..."
                    Me.Refresh()
                    'strFolderName = strFolderName.Substring(0, strFolderName.Length - 1)
                    strFolderName = strFolderName.Substring(0, strFolderName.LastIndexOf("\"))

                    'Use WIndows unzip feature instead of winzip
                    'Dim startInfo As New ProcessStartInfo("winzip32.exe")

                    'startInfo.Arguments = "-e -o " & My.Settings.gstrDLDestination & " " & strFolderName
                    'myProcess = Process.Start(startInfo)
                    'myProcess.WaitForExit()

                    'My.Settings.gstrDLDestination is the full file path of the zip file, strFolderName is the "extracted to" folder path 
                    ZipFile.ExtractToDirectory(My.Settings.gstrDLDestination, strFolderName)

                    ' Add the access control entry to the directory.
                    'AddDirectorySecurity(strFolderName, "MPHO\ipcspare", FileSystemRights.FullControl, AccessControlType.Allow)
                End If

                lblMsg.Text = "Configuration is all done now."
                'MessageBox.Show("The IPC configuration has been completed.", "For Your Information", MessageBoxButtons.OK)
                MessageBox.Show("IPC configuration is completed now. Please notify IT Help Desk about the both affected IPCs for IPC Invetory update.", "Configuration Completed")

                If My.Settings.gblnRefreshDB = True Then
                    lblMsg.Text = "Refreshing Data base, please wait.."
                    Me.Refresh()
                    DownloadDataFromServer(cboFacility.SelectedValue, strMyComputerName)
                    lblMsg.Text = "Refreshing Data base is complete."
                    MessageBox.Show("Data base has been refreshed. You can select the appropriate type to run as.", "For Your Information", MessageBoxButtons.OK)
                End If

                'MessageBox.Show("IPC configuration is all done now. Please notify IT Help Desk about the affected IPCs for IPC Invetory update.", "Configuration Completed")
                Me.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.StackTrace & vbCrLf & ex.Message, "Unexpected Application Error")
        Finally
            If Not IsNothing(cmd) Then
                cmd.Dispose()
            End If
            Me.Cursor = Cursors.Default
            lblMsg.Text = ""
            'lblMsg.Visible = False
        End Try

    End Sub

    '' Adds an ACL entry on the specified directory for the specified account.
    'Sub AddDirectorySecurity(ByVal FileName As String, ByVal Account As String, ByVal Rights As FileSystemRights, ByVal ControlType As AccessControlType)
    '    ' Create a new DirectoryInfoobject.
    '    Dim dInfo As New DirectoryInfo(FileName)

    '    ' Get a DirectorySecurity object that represents the 
    '    ' current security settings.
    '    Dim dSecurity As DirectorySecurity = dInfo.GetAccessControl()

    '    ' Add the FileSystemAccessRule to the security settings. 
    '    dSecurity.AddAccessRule(New FileSystemAccessRule(Account, Rights, ControlType))

    '    ' Set the new access settings.
    '    dInfo.SetAccessControl(dSecurity)

    'End Sub

    Private Sub cboFacility_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFacility.SelectedValueChanged
        If Not IsNothing(Me.cboFacility.SelectedValue) Then
            Me.TblComputerConfigTableAdapter.Fill(Me.DsComputerCfg.tblComputerConfig, Me.cboFacility.SelectedValue)
        End If
    End Sub

    Sub DownloadDataFromServer(ByVal strFacility As String, ByVal strComputerName As String)
        Dim connServer As New SqlConnection()
        Dim connStaging As New SqlConnection()
        Dim strSqlStmt As String
        Dim sdrDLTL As SqlDataReader
        Dim arlTableName As ArrayList
        Dim strTableName As String
        Dim blnHasFacilityColumn As Boolean
        Dim blnAtLeast1Success As Boolean
        Dim obj As Object
        Dim sdTableName As New SortedDictionary(Of String, Boolean)
        Dim dt As DataTable
        Dim ds As New DataSet

        Try
            connServer.ConnectionString = My.Settings.gstrServerCnnString
            connStaging.ConnectionString = My.Settings.gstrStagingCnnString

            Using cmdServer As New SqlCommand
                strSqlStmt = "SELECT TableName FROM tblDownLoadTableList WHERE Facility = '" & strFacility & "'"
                With cmdServer
                    .Connection = connServer
                    .CommandType = CommandType.Text
                    .CommandText = strSqlStmt
                    .Connection.Open()
                    sdrDLTL = .ExecuteReader()

                    arlTableName = New ArrayList
                    While sdrDLTL.Read
                        arlTableName.Add(sdrDLTL("TableName"))
                    End While
                    .Connection.Close()
                    'loop through the array list to get the table names and load those tables to a dataset for reuse
                    'check if the table has Facility Column, includes it in the filter

                    For Each strTableName In arlTableName

                        strSqlStmt = "SELECT 1 from information_schema.columns where table_name = '" & strTableName & "' and column_name = 'facility'"
                        .CommandText = strSqlStmt

                        .Connection.Open()     'if the connection failure, send email 

                        obj = .ExecuteScalar()
                        .Connection.Close()
                        blnHasFacilityColumn = CType(obj, Boolean)
                        sdTableName.Add(strTableName, blnHasFacilityColumn)

                        'WO#14867   If strTableName = "tblEquipment" Then
                        'WO#14867   strSqlStmt = "SELECT Active, facility, EquipmentID, [Type] ,SubType, Description, IPCSharedGroup,WorkCenter FROM tblEquipment Where facility = '" & strFacility & "'"
                        'WO#14867   Else
                        If blnHasFacilityColumn Then
                            strSqlStmt = "SELECT * from " & strTableName & " where facility = '" & strFacility & " '"
                        Else
                            strSqlStmt = "SELECT * from " & strTableName
                        End If
                        'WO#14867 End If

                        .CommandText = strSqlStmt
                        .Connection.Open()
                        dt = New DataTable(strTableName)
                        dt.Load(.ExecuteReader)
                        .Connection.Close()
                        ds.Tables.Add(dt)
                    Next

                End With

            End Using

            blnAtLeast1Success = False
            Using cmdStaging As New SqlCommand

                strSqlStmt = "Update tblComputerConfig SET ReadyForDownLoad = 0 WHERE Facility = '" & strFacility & " ' and ComputerName = '" & strComputerName & "'"
                With cmdStaging
                    .Connection = connStaging
                    .CommandType = CommandType.Text
                    .CommandText = strSqlStmt
                    If .Connection.State = ConnectionState.Closed Then
                        .Connection.Open()     'if the connection failure, send email 
                    End If
                    .ExecuteNonQuery()
                    '.Connection.Close()

                    'loop through the table download list to export the table on the list to IPC
                    For Each strTableName In arlTableName
                        'For Spare IPC and the table has facility column than use Delete statement otherwise use Truncate statement
                        If sdTableName.Item(strTableName) = True Then
                            strSqlStmt = "DELETE " + strTableName + " Where facility = '" & strFacility & "'"
                        Else
                            strSqlStmt = "Truncate table " & strTableName
                        End If

                        .CommandText = strSqlStmt
                        '.Connection.Open()
                        .ExecuteNonQuery()
                        '.Connection.Close()

                        Using bcp As SqlBulkCopy = New SqlBulkCopy(.Connection.ConnectionString, SqlBulkCopyOptions.TableLock)
                            bcp.BulkCopyTimeout = 1800  ' set processing timeout in Seconds
                            bcp.DestinationTableName = strTableName
                            bcp.WriteToServer(ds.Tables(strTableName))  'Save the data to a table of the dataset 
                        End Using

                        'In the DownloadTablelist Table of IPC staging area, update the download status flag = 1 (i.e. success) 
                        'and set the record Active to indicate the table is ready for download to local database.
                        'WO#359 strSQL = "Update tblDownLoadTableList set DownLoadStatus = 1, Active = 1 WHERE Facility = '" & strFacility & " ' AND TableName ='" & strTableName & "'"
                        strSqlStmt = "Update tblDownLoadTableList set DownLoadStatus = 0, Active = 1, LastDownload = '" & Now() & "' WHERE Facility = '" & strFacility & " ' AND TableName ='" & strTableName & "'"
                        .CommandText = strSqlStmt
                        .ExecuteNonQuery()

                        blnAtLeast1Success = True

                    Next    'Loop ended for Table Names
                End With

                Using cmdServer As New SqlCommand
                    With cmdServer
                        .Connection = connServer
                        .CommandType = CommandType.Text
                        .CommandText = strSqlStmt
                        .Connection.Open()

                        If blnAtLeast1Success Then

                            strSqlStmt = "Update tblComputerConfig set ReadyForDownLoad = 1 WHERE Facility = '" & strFacility & " ' and ComputerName = '" & strComputerName & "'"
                            'Set Ready_For_DownLoad status to Yes in the remote IPC
                            'strSQL = "Update tblComputerConfig SET ReadyForDownLoad = 1 WHERE Facility = '" & strFacility & " ' and ComputerName = '" & row("ComputerName") & "'"
                            cmdStaging.CommandText = strSqlStmt
                            cmdStaging.ExecuteNonQuery()

                            ' Set Ready_For_DownLoad status to Yes in the Power Plant SQL server for audit purpose
                            .CommandText = strSqlStmt
                            .ExecuteNonQuery()
                        End If

                        'Reset the Active flag in the Down Load table list to indicate all the files have been successfully downloaded
                        strSqlStmt = "Update tblDownLoadTableList set Active = 0 WHERE Facility = '" & strFacility & " '"
                        .ExecuteNonQuery()
                        .Connection.Close()
                    End With
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
End Class