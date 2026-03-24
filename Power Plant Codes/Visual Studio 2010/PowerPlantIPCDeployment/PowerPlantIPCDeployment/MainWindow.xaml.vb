Imports System.Windows.Forms
Imports System.IO
Imports System.Data

Class MainWindow
    Dim gstuDplymtInfo As New SharedFunctions.DeploymentInfo

    Private Sub MainWindow_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Dim strLogFile As String = My.Settings.strLogFile & "Log.txt"
        chkPowerPlant_Unchecked(sender, e)
        chkDownTime_Unchecked(sender, e)
        chkInitialPgm_Unchecked(sender, e)
        txtSourceFolder.Text = My.Settings.strSourceFolder
        txtIPCBackupPgm.Text = My.Application.Info.DirectoryPath & "\BackupDB_IPC.sql"
        txtIPCRestorePgm.Text = My.Application.Info.DirectoryPath & "\RestoreDB_IPC.sql"
        txtSourceFolderSQL.Text = My.Settings.strSourceFolderSQL
        chkWCF_Unchecked(sender, e)
        chkTCP_Unchecked(sender, e)
        txtWCFTCPSourceFolder.Text = My.Settings.strSourceFolder

        If Not File.Exists(strLogFile) Then
            MessageBox.Show("Please create the file " & strLogFile & " before running the application.")
            Me.Close()
        End If
    End Sub

    Private Sub btnExit_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub chkPowerPlant_Checked(sender As Object, e As System.Windows.RoutedEventArgs) Handles chkPowerPlant.Checked
        txtPowerPlantFolder.Text = My.Settings.strPowerPlantFolder
        txtPowerPlantFiles.Text = My.Settings.strPowerPlantFiles
        lblPowerPlantFolder.Visibility = Visibility.Visible
        txtPowerPlantFolder.Visibility = Visibility.Visible
        btnPowerPlantFolder.Visibility = Visibility.Visible
        lblPowerPlantFiles.Visibility = Visibility.Visible
        txtPowerPlantFiles.Visibility = Visibility.Visible
        lblPowerPlantCurVer.Visibility = Visibility.Visible
        txtPowerPlantCurVer.Visibility = Visibility.Visible
        btnPowerPlantFolder.Visibility = Visibility.Visible
    End Sub

    Private Sub chkPowerPlant_Unchecked(sender As Object, e As System.Windows.RoutedEventArgs) Handles chkPowerPlant.Unchecked
        txtPowerPlantFolder.Text = String.Empty
        txtPowerPlantFiles.Text = String.Empty
        lblPowerPlantFolder.Visibility = Visibility.Hidden
        txtPowerPlantFolder.Visibility = Visibility.Hidden
        btnPowerPlantFolder.Visibility = Visibility.Hidden
        lblPowerPlantFiles.Visibility = Visibility.Hidden
        txtPowerPlantFiles.Visibility = Visibility.Hidden
        lblPowerPlantCurVer.Visibility = Visibility.Hidden
        txtPowerPlantCurVer.Visibility = Visibility.Hidden
    End Sub

    Private Sub chkDownTime_Checked(sender As Object, e As System.Windows.RoutedEventArgs) Handles chkDownTime.Checked
        txtDownTimeFolder.Text = My.Settings.strDownTimeFolder
        txtDownTimeFiles.Text = My.Settings.strDownTimeFiles
        lblDownTimeFolder.Visibility = Visibility.Visible
        txtDownTimeFolder.Visibility = Visibility.Visible
        btnDownTimeFolder.Visibility = Visibility.Visible
        lblDownTimeFiles.Visibility = Visibility.Visible
        txtDownTimeFiles.Visibility = Visibility.Visible
        lblDownTimeCurVer.Visibility = Visibility.Visible
        txtDownTimeCurVer.Visibility = Visibility.Visible
    End Sub

    Private Sub chkDownTime_Unchecked(sender As Object, e As System.Windows.RoutedEventArgs) Handles chkDownTime.Unchecked
        txtDownTimeFolder.Text = String.Empty
        txtDownTimeFiles.Text = String.Empty
        lblDownTimeFolder.Visibility = Visibility.Hidden
        txtDownTimeFolder.Visibility = Visibility.Hidden
        btnDownTimeFolder.Visibility = Visibility.Hidden
        lblDownTimeFiles.Visibility = Visibility.Hidden
        txtDownTimeFiles.Visibility = Visibility.Hidden
        lblDownTimeCurVer.Visibility = Visibility.Hidden
        txtDownTimeCurVer.Visibility = Visibility.Hidden

    End Sub

    Private Sub chkInitialPgm_Checked(sender As Object, e As System.Windows.RoutedEventArgs) Handles chkInitialPgm.Checked
        txtInitialPgmFolder.Text = My.Settings.strIPCInitialPgmFolder
        txtInitialPgmFiles.Text = My.Settings.strIPCInitialPgmFiles
        lblInitialPgmFolder.Visibility = Visibility.Visible
        txtInitialPgmFolder.Visibility = Visibility.Visible
        btnInitialPgmFolder.Visibility = Visibility.Visible
        lblInitialPgmFiles.Visibility = Visibility.Visible
        txtInitialPgmFiles.Visibility = Visibility.Visible
        lblInitialPgmCurVer.Visibility = Visibility.Visible
        txtInitialPgmCurVer.Visibility = Visibility.Visible
    End Sub

    Private Sub chkInitialPgm_Unchecked(sender As Object, e As System.Windows.RoutedEventArgs) Handles chkInitialPgm.Unchecked
        txtInitialPgmFolder.Text = String.Empty
        txtInitialPgmFiles.Text = String.Empty
        lblInitialPgmFolder.Visibility = Visibility.Hidden
        txtInitialPgmFolder.Visibility = Visibility.Hidden
        btnInitialPgmFolder.Visibility = Visibility.Hidden
        lblInitialPgmFiles.Visibility = Visibility.Hidden
        txtInitialPgmFiles.Visibility = Visibility.Hidden
        lblInitialPgmCurVer.Visibility = Visibility.Hidden
        txtInitialPgmCurVer.Visibility = Visibility.Hidden
    End Sub
    Private Sub chkWCF_Checked(sender As Object, e As System.Windows.RoutedEventArgs) Handles chkWCF.Checked
        txtWCFFolder.Text = My.Settings.strWCFFolder
        txtWCFFiles.Text = My.Settings.strWCFFiles
        lblWCFFolder.Visibility = Visibility.Visible
        txtWCFFolder.Visibility = Visibility.Visible
        btnWCFFolder.Visibility = Visibility.Visible
        lblWCFFiles.Visibility = Visibility.Visible
        txtWCFFiles.Visibility = Visibility.Visible
        lblWCFCurVer.Visibility = Visibility.Visible
        txtWCFCurVer.Visibility = Visibility.Visible
        btnWCFFolder.Visibility = Visibility.Visible
    End Sub

    Private Sub chkWCF_Unchecked(sender As Object, e As System.Windows.RoutedEventArgs) Handles chkWCF.Unchecked
        txtWCFFolder.Text = String.Empty
        txtWCFFiles.Text = String.Empty
        lblWCFFolder.Visibility = Visibility.Hidden
        txtWCFFolder.Visibility = Visibility.Hidden
        btnWCFFolder.Visibility = Visibility.Hidden
        lblWCFFiles.Visibility = Visibility.Hidden
        txtWCFFiles.Visibility = Visibility.Hidden
        lblWCFCurVer.Visibility = Visibility.Hidden
        txtWCFCurVer.Visibility = Visibility.Hidden
    End Sub

    Private Sub chkTCP_Checked(sender As Object, e As System.Windows.RoutedEventArgs) Handles chkTCP.Checked
        txtTCPFolder.Text = My.Settings.strTCPFolder
        txtTCPFiles.Text = My.Settings.strTCPFiles
        lblTCPFolder.Visibility = Visibility.Visible
        txtTCPFolder.Visibility = Visibility.Visible
        btnTCPFolder.Visibility = Visibility.Visible
        lblTCPFiles.Visibility = Visibility.Visible
        txtTCPFiles.Visibility = Visibility.Visible
        lblTCPCurVer.Visibility = Visibility.Visible
        txtTCPCurVer.Visibility = Visibility.Visible
        btnTCPFolder.Visibility = Visibility.Visible
    End Sub

    Private Sub chkTCP_Unchecked(sender As Object, e As System.Windows.RoutedEventArgs) Handles chkTCP.Unchecked
        txtTCPFolder.Text = String.Empty
        txtTCPFiles.Text = String.Empty
        lblTCPFolder.Visibility = Visibility.Hidden
        txtTCPFolder.Visibility = Visibility.Hidden
        btnTCPFolder.Visibility = Visibility.Hidden
        lblTCPFiles.Visibility = Visibility.Hidden
        txtTCPFiles.Visibility = Visibility.Hidden
        lblTCPCurVer.Visibility = Visibility.Hidden
        txtTCPCurVer.Visibility = Visibility.Hidden
    End Sub

    Private Sub btnSourceFolder_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnSourceFolder.Click
        txtSourceFolder.Text = BrowseFolder(txtSourceFolder.Text)
    End Sub

    Private Sub btnWCFTCPSourceFolder_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnWCFTCPSourceFolder.Click
        txtWCFTCPSourceFolder.Text = BrowseFolder(txtWCFTCPSourceFolder.Text)
    End Sub

    Private Sub btnPowerPlantFolder_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnPowerPlantFolder.Click
        txtPowerPlantFolder.Text = BrowseFolder(txtPowerPlantFolder.Text)
    End Sub

    Private Sub btnDownTimeFolder_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnDownTimeFolder.Click
        txtDownTimeFolder.Text = BrowseFolder(txtDownTimeFolder.Text)
    End Sub

    Private Sub btnInitialPgmFolder_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnInitialPgmFolder.Click
        txtInitialPgmFolder.Text = BrowseFolder(txtInitialPgmFolder.Text)
    End Sub

    Private Sub btnWCFFolder_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnWCFFolder.Click
        txtWCFFolder.Text = BrowseFolder(txtWCFFolder.Text)
    End Sub

    Private Sub btnTCPFolder_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnTCPFolder.Click
        txtTCPFolder.Text = BrowseFolder(txtTCPFolder.Text)
    End Sub

    Private Function BrowseFolder(strDftPath As String) As String
        Dim strPath As String = strDftPath
        Try
            ' Configure file folder dialog box
            Dim fldDialog As FolderBrowserDialog = New FolderBrowserDialog
            With fldDialog
                ' Show file folder dialog box
                .SelectedPath = strDftPath
                Dim result As DialogResult = fldDialog.ShowDialog()

                ' Process file folder dialog box results
                If result = Forms.DialogResult.OK Then
                    'Get selected path
                    strPath = .SelectedPath
                End If
            End With
            Return strPath
        Catch ex As Exception
            Throw New Exception(ex.Message & " - error in BrowseFolder", ex)
        End Try
    End Function

    Private Function BrowseFile(strDftFile As String, strFilter As String) As String
        Dim strFile As String = strDftFile
        Try
            Dim fileDialog As FileDialog = New OpenFileDialog
            With fileDialog
                .FileName = strDftFile
                .Filter = strFilter & "All files (*.*)|*.*"
                Dim result As DialogResult = fileDialog.ShowDialog()
                If result = Forms.DialogResult.OK Then
                    'Get selected path
                    strFile = .FileName
                End If
            End With
            Return strFile
        Catch ex As Exception
            Throw New Exception(ex.Message & " - error in BrowseFile", ex)
        End Try
    End Function


    'Private Function ChangeConnectionString(ByVal strOrginCnn As String, ByVal strNewServerName As String) As String
    '    Dim strCnn As String
    '    Try
    '        strCnn = strOrginCnn.Replace("MPHOPP01", strNewServerName.Replace("_", ""))
    '        If rbnProd.IsChecked Then
    '            strCnn = strCnn.Replace("PowerPlant_UA", "PowerPlant_Prd")
    '        Else
    '            strCnn = strCnn.Replace("PowerPlant_Prd", "PowerPlant_UA")
    '        End If
    '        Return strCnn
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message & " - error in ChangeConnectionString", ex)
    '    End Try

    'End Function

    'Private Sub cboIPC_PreviewMouseDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs)

    '    Dim dt As dsIPCNames.tblcomputerConfigDataTable
    '    Try
    '        If Not rbnProd.IsChecked And Not rbnUA.IsChecked Then
    '            MsgBox("Please select one of the environments first.")
    '            rbnUA.Focus()
    '        ElseIf cboPlant.SelectedValue Is Nothing Then
    '            MsgBox("Please select plant first.")
    '            cboPlant.Focus()
    '        Else

    '            dt = SharedFunctions.GetComputerList(cboPlant.SelectedValue.name, rbnProd.IsChecked)
    '            If dt.Rows.Count > 0 Then
    '                With cboIPC
    '                    .DataContext = dt
    '                    .DisplayMemberPath = dt.ComputerNameColumn.ColumnName
    '                    .SelectedValuePath = dt.ComputerNameColumn.ColumnName
    '                End With
    '            End If
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Sub

    Private Sub cboTargetUnit_PreviewMouseDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles cboTargetUnit.PreviewMouseDown

        Dim dt As dsIPCNames.tblcomputerConfigDataTable
        Dim dr As dsIPCNames.tblcomputerConfigRow

        Try

            If cboPlant.SelectionBoxItem Is Nothing Then
                MsgBox("Please select plant first.")
                cboPlant.Focus()
            ElseIf cbxEnv.SelectionBoxItem Is Nothing Then
                MsgBox("Please select one of the environments first.")
                cbxEnv.Focus()
            ElseIf cboServerOrIPC.SelectionBoxItem Is Nothing Then
                MsgBox("Please select Server Or IPC for deployment.")
                cboServerOrIPC.Focus()
            Else

                If cboServerOrIPC.SelectionBoxItem = "IPC" Then
                    'dt = SharedFunctions.GetComputerList(cboPlant.SelectedValue.name, rbnProd.IsChecked, cboDataBase.SelectedValue
                    dt = SharedFunctions.GetComputerList(cboPlant.SelectedValue.tag, cbxEnv.SelectionBoxItem, GetDBName())
                Else
                    ' Populate one row with the server name.
                    dt = New dsIPCNames.tblcomputerConfigDataTable
                    dr = dt.NewRow()
                    dr.ComputerName = cboPlant.SelectedValue.tag
                    dt.Rows.Add(dr)
                End If
                If dt.Rows.Count > 0 Then
                    With cboTargetUnit
                        .DataContext = dt
                        .DisplayMemberPath = dt.ComputerNameColumn.ColumnName
                        .SelectedValuePath = dt.ComputerNameColumn.ColumnName
                    End With
                End If
                End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnDeploy_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnDeploy.Click
        Dim strConn As String = String.Empty
        Dim strServerName As String = String.Empty
        'Dim strDataBase As String = String.Empty
        Dim daCF As New dsComputerConfigTableAdapters.tblcomputerConfigTableAdapter
        Dim dtCF As New dsComputerConfig.tblcomputerConfigDataTable
        Dim dr As dsComputerConfig.tblcomputerConfigRow
        Dim strMsg As String = String.Empty
        Dim stuOTI As SharedFunctions.objectsToInstall
        Dim strComputerName As String = ""

        Dim arlInstallPgms As New ArrayList
        Dim sb As New System.Text.StringBuilder
        Dim blnRestartIPC As Boolean

        Try
            If cboPlant.SelectedValue Is Nothing Then
                strMsg = "Plant is missing"
                cboPlant.Focus()
            ElseIf txtSourceFolder.Text Is String.Empty Then
                strMsg = "Soure Folder is missing"
                txtSourceFolder.Focus()
            ElseIf cbxEnv.SelectionBoxItem Is Nothing Then
                strMsg = "Please select one of the environments"
                cbxEnv.Focus()
            End If

            If Not strMsg Is String.Empty Then
                MsgBox(strMsg)
            Else



                'Select Case cboPlant.SelectedValue.name
                '    Case "FW"
                '        strServerName = "MPFWPP01"
                '    Case "AJ"
                '        strServerName = "MPAJPP01"
                '    Case "HO"
                '        strServerName = "MPHOPP01"
                'End Select

                'If rbnProd.IsChecked Then
                '    strDataBase = "POWERPLANT_PRD"
                'Else
                '    strDataBase = "POWERPLANT_UA"
                'End If

                'strConn = My.Settings.strConn
                'strConn = strConn.Replace("MPHOPP01", strServerName)
                'strConn = strConn.Replace("PowerPlant_UA", strDataBase)


                stuOTI = New SharedFunctions.objectsToInstall
                UpdateDeploymentInfo()

                With stuOTI
                    If chkPowerPlant.IsChecked Then
                        .PgmModule = "PowerPlant"
                        .FileFolder = txtPowerPlantFolder.Text
                        .FileNames = txtPowerPlantFiles.Text.Split(",")
                        .CurVersion = txtPowerPlantCurVer.Text
                        arlInstallPgms.Add(stuOTI)
                    End If

                    If chkDownTime.IsChecked Then
                        .PgmModule = "DownTime"
                        .FileFolder = txtDownTimeFolder.Text
                        .FileNames = txtDownTimeFiles.Text.Split(",")
                        .CurVersion = txtDownTimeCurVer.Text
                        arlInstallPgms.Add(stuOTI)
                    End If

                    If chkInitialPgm.IsChecked Then
                        .PgmModule = "IPCInitialPgm"
                        .FileFolder = txtInitialPgmFolder.Text
                        .FileNames = txtInitialPgmFiles.Text.Split(",")
                        .CurVersion = txtInitialPgmCurVer.Text
                        arlInstallPgms.Add(stuOTI)
                    End If
                End With

                If cboRestartIPC.SelectionBoxItem = "Yes" Then
                    blnRestartIPC = True
                Else
                    blnRestartIPC = False
                End If
                If cboTargetUnit.SelectedValue = "*ALL" Then
                    strConn = SharedFunctions.ChangeConnectionString(My.Settings.strConn, cboPlant.SelectedValue.tag, cbxEnv.SelectionBoxItem, GetDBName())
                    daCF.Connection.ConnectionString = strConn
                    daCF.Fill(dtCF)

                    If dtCF.Rows.Count > 0 Then
                        For Each dr In dtCF.Rows
                            strComputerName = dr.ComputerName
                            sb.AppendFormat("Deploy to - {0} started", dr.ComputerName)
                            sb.AppendLine()
                            SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
                            sb.Length = 0
                            If My.Computer.Network.Ping(dr.ComputerName, 1500) Then
                                'SharedFunctions.UpdateLog(txtLog, "Deploy to - " & dr.ComputerName)
                                strMsg = SharedFunctions.Deploy(dr.ComputerName, txtSourceFolder.Text, arlInstallPgms, blnRestartIPC, txtLog, gstuDplymtInfo)
                                'strMsg = "TEST - " & Now().ToString
                                'SharedFunctions.UpdateLog(txtLog, strMsg)
                                'SharedFunctions.UpdateLog(txtLog, "Deploy to - " & dr.ComputerName & " completed.")
                                sb.AppendLine(strMsg)
                                sb.AppendFormat("Deploy to - {0} completed", dr.ComputerName).AppendLine()
                            Else
                                'strMsg = "Can not connect to " & dr.ComputerName & vbLf & "** deployment Failure."
                                'SharedFunctions.UpdateLog(txtLog, strMsg)
                                sb.AppendFormat("** Can not connect to {0}. Deployment Failure.", dr.ComputerName).AppendLine()
                            End If
                        Next
                        SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
                    End If
                Else
                    sb.AppendFormat("Deploy to - {0} started", cboTargetUnit.SelectedValue)
                    strComputerName = cboTargetUnit.SelectedValue
                    sb.AppendLine()
                    SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
                    sb.Length = 0
                    If My.Computer.Network.Ping(cboTargetUnit.SelectedValue, 1500) Then
                        'SharedFunctions.UpdateLog(txtLog, "Deploy to - " & cboTargetUnit.SelectedValue)
                        'strMsg = "TEST - " & Now().ToString
                        strMsg = SharedFunctions.Deploy(cboTargetUnit.SelectedValue, txtSourceFolder.Text, arlInstallPgms, blnRestartIPC, txtLog, gstuDplymtInfo)
                        'SharedFunctions.UpdateLog(txtLog, strMsg)
                        'SharedFunctions.UpdateLog(txtLog, "Deploy to - " & cboTargetUnit.SelectedValue & " completed.")
                        sb.AppendLine(strMsg)
                        sb.AppendFormat("Deploy to - {0} completed", cboTargetUnit.SelectedValue).AppendLine()
                    Else
                        'strMsg = "Can not connect to " & cboTargetUnit.SelectedValue & vbLf & "** deployment Failure."
                        'SharedFunctions.UpdateLog(txtLog, strMsg)
                        sb.AppendFormat("** Can not connect to {0}. Deployment Failure.", cboTargetUnit.SelectedValue).AppendLine()
                    End If
                    SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
                End If


            End If
        Catch ex As Exception
            If sb.Length > 0 Then
                sb.Append("")
                sb.Append("** Deploy to - {0} Failure.", strComputerName)
                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
            End If
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub btnDeploySQL_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnDeploySQL.Click
        Dim strServerName As String = String.Empty
        Dim strDataBase As String = String.Empty
        Dim daDeployItems As New dsDeployItemsTableAdapters.tblDeploymentListTableAdapter
        Dim dtDeployItems As New dsDeployItems.tblDeploymentListDataTable
        Dim dtCF As dsIPCNames.tblcomputerConfigDataTable
        Dim dr As dsIPCNames.tblcomputerConfigRow
        Dim strMsg As String = String.Empty
        Dim strSQLInstance As String = ""
        Dim sb As New System.Text.StringBuilder

        Try
            If cboPlant.SelectionBoxItem Is Nothing Then
                strMsg = "Plant is missing"
                cboPlant.Focus()
            ElseIf txtSourceFolderSQL.Text Is String.Empty Then
                strMsg = "Soure Folder is missing"
                txtSourceFolder.Focus()
            ElseIf cbxEnv.SelectionBoxItem Is Nothing Then
                strMsg = "Please select one of the environments"
                cbxEnv.Focus()
            ElseIf cboServerOrIPC.SelectionBoxItem Is Nothing Then
                MsgBox("Please select where to deploy to.")
                cboServerOrIPC.Focus()
            End If

            If Not strMsg Is String.Empty Then
                MsgBox(strMsg)
            Else


                Dim strEnvironment As String
                'If rbnProd.IsChecked Then
                '    'strDataBase = "PowerPlant_Prd"
                '    strEnvironment = "Prd"
                'Else
                '    'strDataBase = "PowerPlant_UA"
                '    strEnvironment = "UA"
                'End If
                strEnvironment = cbxEnv.SelectionBoxItem

                strDataBase = cboDataBase.SelectionBoxItem
                'strDataBase = "PowerPlant_" & strEnvironment
                'strDataBase = GetDBName()

                Dim strObjectType As String
                Select Case cboObjectType.SelectionBoxItem
                    Case "*ALL"
                        strObjectType = Nothing
                    Case Else
                        strObjectType = cboObjectType.SelectionBoxItem.ToString.Substring(0, 1)
                End Select

                UpdateDeploymentInfo()

                daDeployItems.Fill(dtDeployItems, cboPlant.SelectionBoxItem, strEnvironment, cboServerOrIPC.SelectionBoxItem, cboDeployProject.SelectedValue, strObjectType)

                If cboTargetUnit.SelectedValue = "*ALL" Then
                    'dtCF = SharedFunctions.GetComputerList(cboPlant.SelectedValue.tag, cbxEnv.SelectionBoxItem, cboDataBase.SelectionBoxItem)
                    ' dtCF = SharedFunctions.GetComputerList(cboPlant.SelectedValue.tag, cbxEnv.SelectionBoxItem, strDataBase)
                    'get computer name list from the server database.
                    dtCF = SharedFunctions.GetComputerList(cboPlant.SelectedValue.tag, cbxEnv.SelectionBoxItem, GetDBName())

                    If dtCF.Rows.Count > 0 Then
                        For Each dr In dtCF.Rows
                            If (dr.ComputerName <> "*ALL") Then
                                If cboServerOrIPC.SelectionBoxItem = "IPC" Then
                                    strSQLInstance = dr.ComputerName & "\sqlexpress"
                                Else
                                    strSQLInstance = dr.ComputerName
                                End If

                                sb.AppendFormat("Deploy to - {0},{1} started at {2}.", strSQLInstance, strDataBase, Now)
                                sb.AppendLine()
                                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
                                sb.Length = 0
                                If My.Computer.Network.Ping(dr.ComputerName, 1500) Then

                                    'strMsg = "TEST - " & Now().ToString
                                    'strMsg = SharedFunctions.DeploySQL(txtLog, strSQLInstance, strDataBase, txtSourceFolderSQL.Text, dtDeployItems, cbxDeployItems.SelectedValue, gstuDplymtInfo)
                                    strMsg = SharedFunctions.DeploySQL(txtLog, strSQLInstance, strDataBase, txtSourceFolderSQL.Text, dtDeployItems, cbxDeployItems.SelectedValue, gstuDplymtInfo)
                                    sb.AppendLine(strMsg)
                                    sb.AppendFormat("Deploy to - {0},{1} completed", strSQLInstance, strDataBase)
                                    sb.AppendLine()
                                Else
                                    'strMsg = "Can not connect to " & dr.ComputerName & vbLf & "** deployment Failure."
                                    'SharedFunctions.UpdateLog(txtLog, strMsg)
                                    sb.AppendFormat("** Can not connect to {0},{1}. Deployment Failure.", strSQLInstance, strDataBase)
                                    sb.AppendLine()
                                End If
                            End If
                        Next
                    End If

                Else
                    If cboServerOrIPC.SelectionBoxItem = "IPC" Then
                        strSQLInstance = cboTargetUnit.SelectedValue & "\sqlexpress"
                    Else
                        strSQLInstance = cboTargetUnit.SelectedValue
                    End If
                    sb.AppendFormat("Deploy to - {0},{1} started at {2}.", strSQLInstance, strDataBase, Now)
                    sb.AppendLine()

                    If My.Computer.Network.Ping(cboTargetUnit.SelectedValue, 1500) Then
                        'strMsg = "TEST - " & Now().ToString
                        strMsg = SharedFunctions.DeploySQL(txtLog, strSQLInstance, strDataBase, txtSourceFolderSQL.Text, dtDeployItems, cbxDeployItems.SelectedValue, gstuDplymtInfo)
                        sb.AppendLine(strMsg)
                        sb.AppendLine()
                    Else
                        'strMsg = "Can not connect to " & cboTargetUnit.SelectedValue & vbLf & "** deployment Failure."
                        'SharedFunctions.UpdateLog(txtLog, strMsg)
                        sb.AppendFormat("** Can not connect to {0}. Deployment Failure.", cboTargetUnit.SelectedValue)
                        sb.AppendLine()
                    End If
                End If
                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
            End If
        Catch ex As Exception
            If sb.Length > 0 Then
                sb.Append("")
                sb.AppendFormat("** Deploy to - {0},{1} Deployment Failure.", strSQLInstance, strDataBase)
                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
            End If
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnDeployWCFTCP_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnDeployWCFTCP.Click
        Dim strConn As String = String.Empty
        Dim strServerName As String = String.Empty
        'Dim strDataBase As String = String.Empty
        Dim daCF As New dsComputerConfigTableAdapters.tblcomputerConfigTableAdapter
        Dim dtCF As New dsComputerConfig.tblcomputerConfigDataTable
        Dim drCF As dsComputerConfig.tblcomputerConfigRow
        Dim strMsg As String = String.Empty
        Dim stuOTI As SharedFunctions.objectsToInstall
        Dim strComputerName As String = ""

        Dim arlInstallPgms As New ArrayList
        Dim sb As New System.Text.StringBuilder
        Dim blnRestartIPC As Boolean

        Try
            If cboPlant.SelectedValue Is Nothing Then
                strMsg = "Plant is missing"
                cboPlant.Focus()
            ElseIf txtWCFTCPSourceFolder.Text Is String.Empty Then
                strMsg = "Soure Folder is missing"
                txtWCFTCPSourceFolder.Focus()
            ElseIf cbxEnv.SelectionBoxItem Is Nothing Then
                strMsg = "Please select one of the environments"
                cbxEnv.Focus()
            End If

            If Not strMsg Is String.Empty Then
                MsgBox(strMsg)
            Else
                stuOTI = New SharedFunctions.objectsToInstall
                UpdateDeploymentInfo()

                With stuOTI
                    If chkWCF.IsChecked Then
                        .PgmModule = "PPWCFService"
                        .FileFolder = txtWCFFolder.Text
                        .FileNames = txtWCFFiles.Text.Split(",")
                        .CurVersion = txtWCFCurVer.Text
                        arlInstallPgms.Add(stuOTI)
                    End If

                    If chkTCP.IsChecked Then
                        .PgmModule = "PPTCPServer"
                        .FileFolder = txtTCPFolder.Text
                        .FileNames = txtTCPFiles.Text.Split(",")
                        .CurVersion = txtTCPCurVer.Text
                        arlInstallPgms.Add(stuOTI)
                    End If

                End With

                If cboWCFTCPRestartIPC.SelectionBoxItem = "Yes" Then
                    blnRestartIPC = True
                Else
                    blnRestartIPC = False
                End If

                If cboTargetUnit.SelectedValue = "*ALL" Then
                    strConn = SharedFunctions.ChangeConnectionString(My.Settings.strConn, cboPlant.SelectedValue.tag, cbxEnv.SelectionBoxItem, GetDBName())
                    daCF.Connection.ConnectionString = strConn
                    daCF.Fill(dtCF)

                    If dtCF.Rows.Count > 0 Then
                        For Each drCF In dtCF.Rows
                            DeployDotNetPrograms(drCF.ComputerName, arlInstallPgms, blnRestartIPC, txtWCFTCPSourceFolder.Text)
                        Next
                    End If
                Else
                    DeployDotNetPrograms(cboTargetUnit.SelectedValue, arlInstallPgms, blnRestartIPC, txtWCFTCPSourceFolder.Text)
                End If

            End If
        Catch ex As Exception
            If sb.Length > 0 Then
                sb.Append("")
                sb.Append("** Deploy to - {0} Failure.", strComputerName)
                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
            End If
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub DeployDotNetPrograms(strComputerName As String, arlInstallPgms As ArrayList, blnRestartIPC As Boolean, strSourceFolder As String)
        Dim sb As New System.Text.StringBuilder
        Dim StrMsg As String = String.Empty
        Try
            sb.AppendFormat("Deploy to - {0} started", strComputerName)
            sb.AppendLine()
            SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
            sb.Length = 0
            If My.Computer.Network.Ping(strComputerName, 1500) Then
                StrMsg = SharedFunctions.Deploy(strComputerName, strSourceFolder, arlInstallPgms, blnRestartIPC, txtLog, gstuDplymtInfo)
                sb.AppendLine(StrMsg)
                sb.AppendFormat("Deploy to - {0} completed", strComputerName).AppendLine()
            Else
                sb.AppendFormat("** Can not connect to {0}. Deployment Failure.", strComputerName).AppendLine()
            End If
            SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
        Catch ex As Exception
            Throw New Exception(ex.Message & " - error in DeployDotNetPrograms", ex)
        End Try
    End Sub

    Private Sub btnBackupIPC_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnBackupIPC.Click
        Dim strServerName As String = String.Empty
        Dim dtCF As dsIPCNames.tblcomputerConfigDataTable
        Dim dr As dsIPCNames.tblcomputerConfigRow
        Dim strMsg As String = String.Empty
        Dim strSQLInstance As String
        Dim sb As New System.Text.StringBuilder

        Try
            If cboPlant.SelectionBoxItem Is Nothing Or cboPlant.SelectionBoxItem = String.Empty Then
                strMsg = "Plant is missing"
                cboPlant.Focus()
            ElseIf txtIPCBackupPgm.Text Is String.Empty Then
                strMsg = "IPC backup program is missing"
                txtSourceFolder.Focus()
            ElseIf cbxEnv.SelectionBoxItem Is Nothing Then
                strMsg = "Please select one of the environments"
                cbxEnv.Focus()
            ElseIf cboTargetUnit.SelectedValue Is Nothing Then
                MsgBox("Please select the target computer for backup.")
                cboServerOrIPC.Focus()
            End If

            If Not strMsg Is String.Empty Then
                MsgBox(strMsg)
            Else
                UpdateDeploymentInfo()
                If cboTargetUnit.SelectedValue = "*ALL" Then
                    dtCF = SharedFunctions.GetComputerList(cboPlant.SelectedValue.tag, cbxEnv.SelectionBoxItem, GetDBName())

                    If dtCF.Rows.Count > 0 Then
                        For Each dr In dtCF.Rows
                            If dr.ComputerName <> "*ALL" Then
                                If My.Computer.Network.Ping(dr.ComputerName, 1500) Then
                                    If cboServerOrIPC.SelectionBoxItem = "IPC" Then
                                        strSQLInstance = dr.ComputerName & "\sqlexpress"
                                    Else
                                        strSQLInstance = dr.ComputerName
                                    End If
                                    strMsg = SharedFunctions.IPC_DBBackup(txtLog, strSQLInstance, txtIPCBackupPgm.Text, gstuDplymtInfo)
                                    'SharedFunctions.UpdateLog(txtLog, strMsg)
                                    sb.AppendLine(strMsg)
                                    sb.AppendFormat("Back up - {0} completed", dr.ComputerName)
                                    sb.AppendLine()
                                Else
                                    'strMsg = "Can not connect to " & dr.ComputerName & vbLf & "** Back up Failure."
                                    'SharedFunctions.UpdateLog(txtLog, strMsg)
                                    sb.AppendFormat("** Can not connect to {0}. Backup Failure.", dr.ComputerName)
                                    sb.AppendLine()
                                End If
                                SharedFunctions.UpdateLog(txtLog, "", gstuDplymtInfo)
                            End If
                        Next
                    End If

                Else
                    If cboServerOrIPC.SelectionBoxItem = "IPC" Then
                        strSQLInstance = cboTargetUnit.SelectedValue & "\sqlexpress"
                    Else
                        strSQLInstance = cboTargetUnit.SelectedValue
                    End If
                    If My.Computer.Network.Ping(cboTargetUnit.SelectedValue, 1500) Then
                        strMsg = SharedFunctions.IPC_DBBackup(txtLog, strSQLInstance, txtIPCBackupPgm.Text, gstuDplymtInfo)
                        'SharedFunctions.UpdateLog(txtLog, strMsg)
                        sb.AppendLine(strMsg)
                        sb.AppendFormat("Back up - {0} completed", cboTargetUnit.SelectedValue)
                        sb.AppendLine()
                    Else
                        'strMsg = "Can not connect to " & cboServerOrIPC.SelectionBoxItem & " " & strSQLInstance & vbLf & "** Back up Failure."
                        'SharedFunctions.UpdateLog(txtLog, strMsg)
                        sb.AppendFormat("** Can not connect to {0}. Back up Failure.", cboTargetUnit.SelectedValue)
                        sb.AppendLine()
                    End If
                End If
                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
            End If
        Catch ex As Exception
            If sb.Length > 0 Then
                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
            End If
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnRestoreIPC_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnRestoreIPC.Click
        Dim strServerName As String = String.Empty
        Dim dtCF As dsIPCNames.tblcomputerConfigDataTable
        Dim dr As dsIPCNames.tblcomputerConfigRow
        Dim strMsg As String = String.Empty
        Dim strSQLInstance As String
        Dim sb As New System.Text.StringBuilder

        Try
            If cboPlant.SelectionBoxItem Is Nothing Or cboPlant.SelectionBoxItem = String.Empty Then
                strMsg = "Plant is missing"
                cboPlant.Focus()
            ElseIf txtIPCBackupPgm.Text Is String.Empty Then
                strMsg = "IPC restore program is missing"
                txtSourceFolder.Focus()
            ElseIf cbxEnv.SelectionBoxItem Is Nothing Then
                strMsg = "Please select one of the environments"
                cbxEnv.Focus()
            ElseIf cboTargetUnit.SelectedValue Is Nothing Then
                MsgBox("Please select the target computer for backup.")
                cboServerOrIPC.Focus()
            End If

            If Not strMsg Is String.Empty Then
                MsgBox(strMsg)
            Else
                UpdateDeploymentInfo()
                If cboTargetUnit.SelectedValue = "*ALL" Then
                    dtCF = SharedFunctions.GetComputerList(cboPlant.SelectedValue.tag, cbxEnv.SelectionBoxItem, GetDBName())

                    If dtCF.Rows.Count > 0 Then
                        For Each dr In dtCF.Rows
                            If dr.ComputerName <> "*ALL" Then
                                If My.Computer.Network.Ping(dr.ComputerName, 1500) Then
                                    If cboServerOrIPC.SelectionBoxItem = "IPC" Then
                                        strSQLInstance = dr.ComputerName & "\sqlexpress"
                                    Else
                                        strSQLInstance = dr.ComputerName
                                    End If
                                    strMsg = SharedFunctions.IPC_DBRestore(txtLog, strSQLInstance, txtIPCBackupPgm.Text, gstuDplymtInfo)

                                    sb.AppendLine(strMsg)
                                    sb.AppendFormat("Restore DB - {0} completed", dr.ComputerName)
                                    sb.AppendLine()
                                Else
                                    sb.AppendFormat("** Can not connect to {0}. Restore DB Failure.", dr.ComputerName)
                                    sb.AppendLine()
                                End If
                                SharedFunctions.UpdateLog(txtLog, "", gstuDplymtInfo)
                            End If
                        Next
                    End If

                Else
                    If cboServerOrIPC.SelectionBoxItem = "IPC" Then
                        strSQLInstance = cboTargetUnit.SelectedValue & "\sqlexpress"
                    Else
                        strSQLInstance = cboTargetUnit.SelectedValue
                    End If
                    If My.Computer.Network.Ping(cboTargetUnit.SelectedValue, 1500) Then
                        strMsg = SharedFunctions.IPC_DBRestore(txtLog, strSQLInstance, txtIPCBackupPgm.Text, gstuDplymtInfo)
                        sb.AppendLine(strMsg)
                        sb.AppendFormat("Restore DB - {0} completed", cboTargetUnit.SelectedValue)
                        sb.AppendLine()
                    Else
                        sb.AppendFormat("** Can not connect to {0}. Restore DB Failure.", cboTargetUnit.SelectedValue)
                        sb.AppendLine()
                    End If
                End If
                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
            End If
        Catch ex As Exception
            If sb.Length > 0 Then
                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
            End If
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub btnSourceFolderSQL_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnSourceFolderSQL.Click
        txtSourceFolderSQL.Text = BrowseFolder(txtSourceFolderSQL.Text)
    End Sub

    Private Sub btnIPCBackupPgm_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnIPCBackupPgm.Click
        txtIPCBackupPgm.Text = BrowseFile(txtIPCBackupPgm.Text, "SQL Source files (*.sql)|*.sql|")
    End Sub

    Private Sub btnIPCRestorePgm_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnIPCBackupPgm.Click
        txtIPCBackupPgm.Text = BrowseFile(txtIPCRestorePgm.Text, "SQL Source files (*.sql)|*.sql|")
    End Sub

    Private Sub cboDeployProject_PreviewMouseDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles cboDeployProject.PreviewMouseDown
        Dim daDeployProjects As New dsDeployProjectsTableAdapters.tblDeploymentsTableAdapter
        Dim dtDeployProjects As New dsDeployProjects.tblDeploymentsDataTable

        daDeployProjects.Fill(dtDeployProjects)
        With cboDeployProject
            .DataContext = dtDeployProjects
            .DisplayMemberPath = dtDeployProjects.DescriptionColumn.ColumnName
            .SelectedValuePath = dtDeployProjects.DeploymentIDColumn.ColumnName
        End With
    End Sub

    Private Sub btnClearLog_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnClearLog.Click
        txtLog.Text = String.Empty
    End Sub

    Private Sub cboDataBase_PreviewMouseDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles cboDataBase.PreviewMouseDown
        Dim cbxItem As New ComboBoxItem
        Dim cbxItem1 As New ComboBoxItem
        Dim cbxItem2 As New ComboBoxItem
        Dim strDBNamePrefix As String
        Dim strSite As String

        Try
            cboDataBase.Items.Clear()

            Select Case cboServerOrIPC.SelectionBoxItem
                Case "Server"
                    strDBNamePrefix = "PowerPlant"
                    If cbxEnv.SelectionBoxItem = "Prd" Then
                        cbxItem.Content = strDBNamePrefix & "_" & cbxEnv.SelectionBoxItem
                    Else
                        If cboPlant.SelectionBoxItem.ToString.Substring(0, 2) = "AX" Or cboPlant.SelectionBoxItem.ToString.Substring(0, 2) = "CU" Then
                            'If cbxEnv.SelectionBoxItem = "DEV" Then
                            '    cbxItem.Content = strDBNamePrefix & "AX_Dev"
                            'Else
                            cbxItem.Content = strDBNamePrefix & cboPlant.SelectionBoxItem.ToString & "_" & cbxEnv.SelectionBoxItem.ToString
                            'End If

                        Else
                            cbxItem.Content = strDBNamePrefix & "_" & cbxEnv.SelectionBoxItem.ToString
                        End If
                    End If
                    strDBNamePrefix = "Probat"

                    Select Case Right(cboPlant.SelectedValue.Content, 2)
                        Case "HO"
                            strSite = "01"
                        Case "AJ"
                            strSite = "01"
                        Case "FW"
                            strSite = "07"
                        Case "SP"
                            strSite = "09"
                        Case Else
                            strSite = ""
                    End Select

                    If cbxEnv.SelectionBoxItem = "Prd" Then
                        cbxItem1.Content = strDBNamePrefix & strSite & "_" & cbxEnv.SelectionBoxItem
                    Else
                        If cboPlant.SelectionBoxItem.ToString.Substring(0, 2) = "AX" Or cboPlant.SelectionBoxItem.ToString.Substring(0, 2) = "CU" Then
                            'If cbxEnv.SelectionBoxItem = "DEV" Then
                            '    cbxItem.Content = strDBNamePrefix & "AX_Dev"
                            'Else
                            cbxItem1.Content = strDBNamePrefix & cboPlant.SelectionBoxItem.ToString.Substring(0, 2) & strSite & "_" & cbxEnv.SelectionBoxItem.ToString
                            'End If

                        Else
                            cbxItem1.Content = strDBNamePrefix & strSite & "_" & cbxEnv.SelectionBoxItem.ToString
                        End If
                    End If
                Case "SvrStaging"
                    strDBNamePrefix = "ImportData"
                    If cbxEnv.SelectionBoxItem = "Prd" Then
                        cbxItem.Content = strDBNamePrefix
                    Else
                        If cboPlant.SelectionBoxItem.ToString.Substring(0, 2) = "AX" Or cboPlant.SelectionBoxItem.ToString.Substring(0, 2) = "CU" Then
                            'If cbxEnv.SelectionBoxItem = "DEV" Then
                            '    cbxItem.Content = strDBNamePrefix & "AX_Dev"
                            'Else
                            '    'cbxItem.Content = "ImportDataAXHO_UA"
                            '    'cbxItem1.Content = "ImportDataAXFW_UA"
                            cbxItem.Content = strDBNamePrefix & cboPlant.SelectionBoxItem.ToString & "_" & cbxEnv.SelectionBoxItem.ToString
                            'End If
                        Else
                            cbxItem.Content = strDBNamePrefix & "_" & cbxEnv.SelectionBoxItem.ToString
                        End If
                    End If

                Case Else   'IPC
                    cbxItem.Content = "TempDB"
                    cbxItem1.Content = "ImportData"
                    cbxItem2.Content = "LocalPowerPlant"
                    cbxItem.IsSelected = True
            End Select

            'If cboServerOrIPC.SelectionBoxItem = "Server" Then
            '    If rbnProd.IsChecked Then
            '        cbxItem.Content = "PowerPlant_Prd"
            '        cbxItem1.Content = "ImportData"
            '    Else
            '        If cboPlant.SelectionBoxItem = "AX" Then
            '            cbxItem.Content = "PowerPlantAXHO_UA"
            '            cbxItem1.Content = "PowerPlantAXFW_UA"
            '            cbxItem2.Content = "ImportDataAXHO_UA"
            '            cbxItem3.Content = "ImportDataAXFW_UA"
            '        Else
            '            cbxItem.Content = "PowerPlant_UA"
            '            cbxItem1.Content = "ImportData_UA"
            '        End If
            '    End If

            'Else
            '    cbxItem.Content = "TempDB"
            '    cbxItem1.Content = "ImportData"
            '    cbxItem2.Content = "LocalPowerPlant"
            '    cbxItem.IsSelected = True
            '    cboDataBase.Items.Add(cbxItem)
            'End If

            cboDataBase.Items.Add(cbxItem)
            cboDataBase.Items.Add(cbxItem1)
            cboDataBase.Items.Add(cbxItem2)

        Catch ex As Exception
            Throw New Exception(ex.Message & " - error in cboDataBase_PreviewMouseDown", ex)
        End Try
    End Sub

    Private Sub cbxDeployItems_PreviewMouseDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles cbxDeployItems.PreviewMouseDown
        Dim daDeployItems As New dsDeployItemsTableAdapters.tblDeploymentListTableAdapter
        Dim dtDeployItems As New dsDeployItems.tblDeploymentListDataTable
        Dim strEnvironment As String
        Try
            'If rbnProd.IsChecked Then
            '    strEnvironment = "Prd"
            'Else
            '    strEnvironment = "UA"
            'End If

            strEnvironment = cbxEnv.SelectionBoxItem

            Dim strObjectType As String
            Select Case cboObjectType.SelectionBoxItem
                Case "*ALL"
                    strObjectType = Nothing
                Case Else
                    strObjectType = cboObjectType.SelectionBoxItem.ToString.Substring(0, 1)
            End Select

            daDeployItems.FillByID(dtDeployItems, cboDeployProject.SelectedValue, cboPlant.SelectionBoxItem, strEnvironment, cboServerOrIPC.SelectionBoxItem, strObjectType)
            With cbxDeployItems
                .DataContext = dtDeployItems
                .DisplayMemberPath = dtDeployItems.FileNameColumn.ColumnName
                .SelectedValuePath = dtDeployItems.FileNameColumn.ColumnName
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message & " - error in cbxDeployItems_PreviewMouseDown", ex)
        End Try
    End Sub

    Public Sub UpdateDeploymentInfo()

        With gstuDplymtInfo
            .strPlant = cboPlant.SelectionBoxItem

            .intDeploymentID = cboDeployProject.SelectedValue
            'If rbnUA.IsChecked Then
            '    .strEnv = "UA"
            'Else
            '    .strEnv = "Prd"
            'End If

            .strEnv = cbxEnv.SelectionBoxItem
            If cboServerOrIPC.SelectionBoxItem = "IPC" Then
                .strServerOrIPC = "I"
            Else
                .strServerOrIPC = "S"
            End If
            If tabSQL.IsSelected Then
                .strDeploymentType = cboObjectType.SelectionBoxItem
            Else
                .strDeploymentType = ""
            End If
        End With
    End Sub

    Private Sub cboRegHive_PreviewMouseDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles cboRegHive.PreviewMouseDown
        Dim rhNames() As String = CType([Enum].GetNames(GetType(Microsoft.Win32.RegistryHive)), String())

        Dim rhName As String
        cboRegHive.Items.Clear()
        For Each rhName In rhNames
            cboRegHive.Items.Add(rhName)
            'Console.WriteLine("Registry Hive:{0}", rhName)
        Next
        txtRegSubkey.Text = My.Settings.strDftRegSubKeyPath
        txtRegKeyName.Text = My.Settings.strDftRegKeyName
        txtNewRegValue.Text = My.Settings.strDftNewRegValue

    End Sub


    Private Sub btnUpdateRegistry_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnUpdateRegistry.Click
        Dim strConn As String = String.Empty

        Dim daCF As New dsComputerConfigTableAdapters.tblcomputerConfigTableAdapter
        Dim dtCF As New dsComputerConfig.tblcomputerConfigDataTable
        Dim dr As dsComputerConfig.tblcomputerConfigRow
        Dim strMsg As String = String.Empty
        Dim sb As New System.Text.StringBuilder
        Try
            If cboRegHive.SelectionBoxItem Is Nothing Or cboRegHive.SelectionBoxItem = String.Empty Then
                strMsg = "Registry Hive is missing."
                cboRegHive.Focus()
            ElseIf txtRegSubkey.Text Is String.Empty Then
                strMsg = "Registry Subkey is missing."
                txtRegSubkey.Focus()
            ElseIf txtRegKeyName.Text Is String.Empty Then
                strMsg = "Registry key name is missing."
                txtRegSubkey.Focus()
            ElseIf txtNewRegValue.Text Is String.Empty Then
                strMsg = "New Registry Value is missing."
                txtNewRegValue.Focus()
            End If

            If Not strMsg Is String.Empty Then
                MsgBox(strMsg)
            Else
                UpdateDeploymentInfo()
                If cboTargetUnit.SelectedValue = "*ALL" Then
                    strConn = SharedFunctions.ChangeConnectionString(My.Settings.strConn, cboPlant.SelectedValue.tag, cbxEnv.SelectionBoxItem, GetDBName())
                    daCF.Connection.ConnectionString = strConn
                    daCF.Fill(dtCF)

                    If dtCF.Rows.Count > 0 Then
                        For Each dr In dtCF.Rows
                            sb = UpdRegistry(dr.ComputerName)
                            'sb.AppendFormat("Deploy to - {0} started", dr.ComputerName)
                            'sb.AppendLine()
                            'If My.Computer.Network.Ping(dr.ComputerName, 1500) Then
                            '    SharedFunctions.chgRegistry(dr.ComputerName, cboRegHive.SelectionBoxItem, txtRegSubkey.Text, txtRegKeyName.Text, txtNewRegValue.Text)
                            '    sb.AppendLine(strMsg)
                            '    sb.AppendFormat("Deploy to - {0} completed", dr.ComputerName)
                            '    sb.AppendLine()
                            'Else
                            '    sb.AppendFormat("** Can not connect to {0}. Deployment Failure.", dr.ComputerName)
                            '    sb.AppendLine()
                            'End If
                        Next
                        SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
                    End If
                Else
                    sb = UpdRegistry(cboTargetUnit.SelectedValue)
                    'sb.AppendFormat("Deploy to - {0} started", cboTargetUnit.SelectedValue)
                    'sb.AppendLine()
                    'If My.Computer.Network.Ping(cboTargetUnit.SelectedValue, 1500) Then
                    '    SharedFunctions.chgRegistry(cboTargetUnit.SelectedValue, cboRegHive.SelectionBoxItem, txtRegSubkey.Text, txtRegKeyName.Text, txtNewRegValue.Text)
                    '    sb.AppendFormat("Deploy to - {0} completed", cboTargetUnit.SelectedValue)
                    '    sb.AppendLine()
                    'Else
                    '    sb.AppendFormat("** Can not connect to {0}. Deployment Failure.", cboTargetUnit.SelectedValue)
                    '    sb.AppendLine()
                    'End If
                    SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
                End If

            End If
        Catch ex As Exception
            If sb.Length > 0 Then
                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
            End If
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Function UpdRegistry(strComputerName) As System.Text.StringBuilder
        Dim sb As New System.Text.StringBuilder
        Try
            sb.AppendFormat("Update Registry in - {0} started", strComputerName)
            sb.AppendLine()
            If My.Computer.Network.Ping(strComputerName, 1500) Then
                SharedFunctions.chgRegistry(strComputerName, cboRegHive.SelectionBoxItem, txtRegSubkey.Text, txtRegKeyName.Text, txtNewRegValue.Text)
                sb.AppendFormat("Update Registry in - {0} completed", strComputerName)
                sb.AppendLine()
            Else
                sb.AppendFormat("** Can not connect to {0}. Update Registry Failure.", strComputerName)
                sb.AppendLine()
            End If
            Return (sb)
        Catch ex As Exception
            Throw (ex)
        End Try
    End Function

    Private Sub btnBrwProbatSourceFolder_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnBrwProbatSourceFolder.Click
        txtProbatSourceFolder.Text = BrowseFolder(txtProbatSourceFolder.Text)
    End Sub

    Private Sub btnBrwProbatTargetFolder_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnBrwProbatTargetFolder.Click
        txtProbatTargetFolder.Text = BrowseFolder(txtProbatTargetFolder.Text)
    End Sub

    Private Sub btnDeployProbatClient_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnDeployProbatClient.Click
        Dim strConn As String = String.Empty
        Dim strServerName As String = String.Empty
        Dim strDataBase As String = String.Empty
        Dim daCF As New dsComputerConfigTableAdapters.tblcomputerConfigTableAdapter
        Dim dtCF As New dsComputerConfig.tblcomputerConfigDataTable
        Dim dr As dsComputerConfig.tblcomputerConfigRow
        Dim strMsg As String = String.Empty
        Dim sb As New System.Text.StringBuilder
        ' Dim stuOTI As SharedFunctions.objectsToInstall
        Dim arlInstallPgms As New ArrayList

        Try
            If cboPlant.SelectedValue Is Nothing Then
                strMsg = "Plant is missing"
                cboPlant.Focus()
            ElseIf txtProbatSourceFolder.Text Is String.Empty Then
                strMsg = "Probat Source Folder is missing"
                txtProbatSourceFolder.Focus()
            ElseIf txtProbatTargetFolder.Text Is String.Empty Then
                strMsg = "Probat Target Folder is missing"
                txtProbatTargetFolder.Focus()
            ElseIf cbxEnv.SelectionBoxItem Is Nothing Then
                strMsg = "Please select one of the environments"
                cbxEnv.Focus()
            End If

            If Not strMsg Is String.Empty Then
                MsgBox(strMsg)
            Else
                UpdateDeploymentInfo()

                If cboTargetUnit.SelectedValue = "*ALL" Then
                    strConn = SharedFunctions.ChangeConnectionString(My.Settings.strConn, cboPlant.SelectedValue.tag, cbxEnv.SelectionBoxItem, GetDBName())
                    daCF.Connection.ConnectionString = strConn
                    daCF.Fill(dtCF)
                    If dtCF.Rows.Count > 0 Then
                        For Each dr In dtCF.Rows
                            DeployProbatClient(dr.ComputerName, txtProbatSourceFolder.Text, arlInstallPgms, 0, txtLog, gstuDplymtInfo)
                        Next
                        'If txtProbatClientFiles.Text = "" Then
                        '    If dtCF.Rows.Count > 0 Then
                        '        For Each dr In dtCF.Rows
                        '            sb.AppendFormat("Copy folders to - {0} started", dr.ComputerName)
                        '            sb.AppendLine("")
                        '            SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
                        '            sb.Length = 0
                        '            sb.AppendLine(SharedFunctions.XCopyFolders(txtProbatSourceFolder.Text, txtProbatTargetFolder.Text, dr.ComputerName, chkDelProbatClient.IsChecked))
                        '            If sb.Length > 0 Then
                        '                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
                        '            End If
                        '        Next
                        '    Else
                        '        sb.AppendFormat("Copy files to - {0} started", cboTargetUnit.Text)
                        '        sb.AppendLine("")
                        '        SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
                        '        sb.Length = 0
                        '        With stuOTI
                        '            .PgmModule = "ProbatClient"
                        '            .FileFolder = txtProbatTargetFolder.Text
                        '            .FileNames = txtProbatClientFiles.Text.Split(",")
                        '            .CurVersion = txtProbatClientVersion.Text
                        '            arlInstallPgms.Add(stuOTI)
                        '        End With
                        '        strMsg = SharedFunctions.Deploy(cboTargetUnit.Text, txtProbatSourceFolder.Text, arlInstallPgms, 0, txtLog, gstuDplymtInfo)
                        '    End If
                        'End If
                    End If
                Else
                    DeployProbatClient(cboTargetUnit.Text, txtProbatSourceFolder.Text, arlInstallPgms, 0, txtLog, gstuDplymtInfo)
                    'If txtProbatClientFiles.Text = "" Then
                    '    sb.AppendFormat("Copy folders to - {0} started", cboTargetUnit.Text)
                    '    sb.AppendLine("")
                    '    SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
                    '    sb.Length = 0
                    '    'Debug.Print(Now())
                    '    sb.AppendLine(SharedFunctions.XCopyFolders(txtProbatSourceFolder.Text, txtProbatTargetFolder.Text, cboTargetUnit.Text, chkDelProbatClient.IsChecked))
                    '    'SharedFunctions.CopyDirectory(txtProbatSourceFolder.Text, txtProbatTargetFolder.Text)
                    '    'Debug.Print(Now())
                    'Else
                    'sb.AppendFormat("Copy files to - {0} started", cboTargetUnit.Text)
                    'sb.AppendLine("")
                    'SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
                    'sb.Length = 0

                    'With stuOTI
                    '    .PgmModule = "ProbatClient"
                    '    .FileFolder = txtProbatTargetFolder.Text
                    '    .FileNames = txtProbatClientFiles.Text.Split(",")
                    '    .CurVersion = txtProbatClientVersion.Text
                    '    arlInstallPgms.Add(stuOTI)
                    'End With
                    'strMsg = SharedFunctions.Deploy(cboTargetUnit.Text, txtProbatSourceFolder.Text, arlInstallPgms, 0, txtLog, gstuDplymtInfo)
                    '    End If
                End If
                End If
        Catch ex As Exception
            sb.AppendLine("Copy folder Failure")
            MessageBox.Show(ex.Message)
        Finally
            If sb.Length > 0 Then
                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
            End If
        End Try
    End Sub

    Private Sub DeployProbatClient(strComputerName As String, ByVal strProbatSourceFolder As String, ByVal arlPgmModels As ArrayList, strCopyType As String, txtLog As Controls.TextBox, stuDplymtInfo As SharedFunctions.DeploymentInfo)

        Dim sb As New System.Text.StringBuilder
        Dim stuOTI As SharedFunctions.objectsToInstall
        Dim arlInstallPgms As New ArrayList

        Try
            If txtProbatClientFiles.Text = "" Then
                sb.AppendFormat("Copy folders to - {0} started", strComputerName)
                sb.AppendLine("")
                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
                sb.Length = 0
                sb.AppendLine(SharedFunctions.XCopyFolders(txtProbatSourceFolder.Text, txtProbatTargetFolder.Text, strComputerName, chkDelProbatClient.IsChecked))
                'SharedFunctions.CopyDirectory(txtProbatSourceFolder.Text, txtProbatTargetFolder.Text)
            Else
                sb.AppendFormat("Copy files to - {0} started", strComputerName)
                sb.AppendLine("")
                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
                sb.Length = 0

                With stuOTI
                    .PgmModule = "ProbatClient"
                    .FileFolder = txtProbatTargetFolder.Text
                    .FileNames = txtProbatClientFiles.Text.Split(",")
                    .CurVersion = txtProbatClientVersion.Text
                    arlInstallPgms.Add(stuOTI)
                End With
                sb.AppendLine(SharedFunctions.Deploy(strComputerName, strProbatSourceFolder, arlInstallPgms, 0, txtLog, gstuDplymtInfo))
            End If
        Catch ex As Exception
            Throw (ex)
        Finally
            If sb.Length > 0 Then
                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
            End If
        End Try
    End Sub

    Private Sub btnRunRegistry_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnRunRegistry.Click
        Dim sb As New System.Text.StringBuilder
        Dim strConn As String = String.Empty
        Dim strCommand As String = String.Empty
        Dim strProbatClientRegistryFile As String = String.Empty
        Dim daCF As New dsComputerConfigTableAdapters.tblcomputerConfigTableAdapter
        Dim dtCF As New dsComputerConfig.tblcomputerConfigDataTable
        Dim dr As dsComputerConfig.tblcomputerConfigRow
        Dim strMsg As String = String.Empty
        Dim strArgument As String = String.Empty

        Try
            If cboPlant.SelectedValue Is Nothing Then
                strMsg = "Plant is missing"
                cboPlant.Focus()
            ElseIf txtCfgProbatClientRegistryBatchFile.Text Is String.Empty Then
                strMsg = "Probat Client Registry file is missing"
                txtCfgProbatClientRegistryBatchFile.Focus()
            ElseIf cbxEnv.SelectionBoxItem Is Nothing Then
                strMsg = "Please select one of the environments"
                cbxEnv.Focus()
            End If

            If Not strMsg Is String.Empty Then
                MessageBox.Show(strMsg)
            Else
                UpdateDeploymentInfo()

                If cboTargetUnit.SelectedValue = "*ALL" Then
                    strConn = SharedFunctions.ChangeConnectionString(My.Settings.strConn, cboPlant.SelectedValue.tag, cbxEnv.SelectionBoxItem, GetDBName())
                    daCF.Connection.ConnectionString = strConn
                    daCF.Fill(dtCF)
                    If dtCF.Rows.Count > 0 Then
                        For Each dr In dtCF.Rows
                            SharedFunctions.ConfigureProbatClientRegistry(dr.ComputerName, txtCfgProbatClientRegistryBatchFile.Text, txtLog, gstuDplymtInfo)
                        Next
                    End If
                Else
                    SharedFunctions.ConfigureProbatClientRegistry(cboTargetUnit.SelectedValue, txtCfgProbatClientRegistryBatchFile.Text, txtLog, gstuDplymtInfo)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            If sb.Length > 0 Then
                SharedFunctions.UpdateLog(txtLog, sb.ToString, gstuDplymtInfo)
            End If
        End Try

    End Sub

    Private Sub tblProbatClient_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles tblProbatClient.Loaded
        txtProbatSourceFolder.Text = My.Settings.strDftProbatClientSourceFolder
        txtProbatTargetFolder.Text = My.Settings.strDftProbatClientTargetFolder
        txtProbatClientFiles.Text = My.Settings.strDftProbatFiles
        txtCfgProbatClientRegistryBatchFile.Text = My.Settings.strDftCfgProbatRegistryBatchFile
    End Sub

    Private Function GetDBName() As String
        Dim strDBName As String = String.Empty
        Dim strDBNamePrefix As String

        strDBName = cboDataBase.SelectionBoxItem
        strDBNamePrefix = "PowerPlant"
        If cbxEnv.SelectionBoxItem = "Prd" Then
            strDBName = strDBNamePrefix & "_" & cbxEnv.SelectionBoxItem
        Else
            If cboPlant.SelectionBoxItem.ToString.Substring(0, 2) = "AX" Or cboPlant.SelectionBoxItem.ToString.Substring(0, 2) = "CU" Then
                'If cbxEnv.SelectionBoxItem = "DEV" Then
                ' strDBName = strDBNamePrefix & "AX_Dev"
                'Else
                strDBName = strDBNamePrefix & cboPlant.SelectionBoxItem.ToString & "_" & cbxEnv.SelectionBoxItem.ToString
                'End If
            Else
                strDBName = strDBNamePrefix & "_" & cbxEnv.SelectionBoxItem.ToString
            End If
        End If

        Return strDBName
    End Function
End Class
