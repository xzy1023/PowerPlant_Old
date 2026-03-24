Imports System.IO
Imports Microsoft.Win32
Imports System.Data.SqlClient

Public Enum RunAs
    PalletStation
    HasIndusoft
    HasNoIndusoft
    IPCViewer
End Enum

Public Class myUtilities

    Public Shared Sub SetDefaultProfile(ByVal strDefaultUserName As String, ByVal strDefaultPassword As String)
        Dim regKey As RegistryKey = Nothing
        Dim strKeyPath As String = String.Empty
        Try
            If My.Settings.gblnChgDftProfile = True Then
                strKeyPath = "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"
                regKey = Registry.LocalMachine.OpenSubKey(strKeyPath, False)
                regKey.SetValue("DefaultUserName", strDefaultUserName)
                regKey.SetValue("DefaultPassword", strDefaultPassword)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(regKey) Then
                regKey.Close()
            End If
        End Try

    End Sub

    Public Shared Function IsConfigured(ByVal intSelectedOption As RunAs) As Boolean
        Dim taSessCtl As New dsSessCtlTableAdapters.tblSessionControlTableAdapter
        Dim dtSessCtl As New dsSessCtl.tblSessionControlDataTable
        Dim drSessCtl As dsSessCtl.tblSessionControlRow
        Try
            If My.Settings.gblnCheckConfiguration Then
                taSessCtl.Fill(dtSessCtl)
                If dtSessCtl.Rows.Count > 0 Then
                    drSessCtl = dtSessCtl.Rows(0)

                    If drSessCtl.DefaultPkgLine.TrimEnd = "Spare" And intSelectedOption <> RunAs.IPCViewer Then
                        Return False
                    ElseIf drSessCtl.DefaultPkgLine.TrimEnd <> "Spare" And intSelectedOption = RunAs.IPCViewer Then
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return False
                End If
            Else
                Return True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function IsSelectedRight(ByVal strComputerName As String, ByVal intSelectedOption As Int16) As String
        Try
            Dim taCfg As New dsComputerCfgTableAdapters.tblComputerConfigTableAdapter
            Dim dtCfg As New dsComputerCfg.tblComputerConfigDataTable
            Dim drCfg As dsComputerCfg.tblComputerConfigRow

            taCfg.FillByComputer(dtCfg, "SelectAllFields", strComputerName, Nothing)
            If dtCfg.Rows.Count > 0 Then
                drCfg = dtCfg.Rows(0)
                With dtCfg.Rows(0)
                    'Note: In dsComputerCfg.xsd, the DBnull of drCfg.IndusoftPgmName is converted to Nothing
                    Select Case intSelectedOption
                        Case RunAs.PalletStation  ' Pallet Station
                            If drCfg.PalletStation <> True Then
                                Return "IPC is NOT configured for Pallet Staion, please select the correct option."
                            End If
                        Case RunAs.HasIndusoft  ' Has Indusoft
                            If IsNothing(drCfg.IndusoftPgmName) Then
                                Return "IPC is NOT configured to run on Indusoft HMI, please select the correct option."
                            End If
                        Case RunAs.HasNoIndusoft  ' Has no Indusoft
                            If Not IsNothing(drCfg.IndusoftPgmName) Then
                                Return "IPC is configured to run on Indusoft HMI, please select the correct option."
                            End If
                        Case RunAs.IPCViewer  ' IPC Viewer
                            If IsNothing(drCfg.IndusoftPgmName) Or drCfg.PkgLineType <> "IPCViewer" Then
                                Return "IPC is NOT configured for IPC Viewer, please select the correct option."
                            End If
                    End Select
                    Return (String.Empty)
                End With
            Else
                Return "IPC is not configured properly, please contact IT."
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''WO#16894 ADD Start
    'Public Shared Sub SyncIPCCfgData(ByVal strComputerName As String)
    '    Dim taSessCtl As New dsSessCtlTableAdapters.tblSessionControlTableAdapter
    '    Dim dtSessCtl As New dsSessCtl.tblSessionControlDataTable
    '    Dim ta As New dsComputerCfgTableAdapters.tblComputerConfigTableAdapter
    '    Try
    '        taSessCtl.Fill(dtSessCtl)
    '        If dtSessCtl.Rows.Count > 0 Then
    '            ta.PPsp_SyncLineCfgDataFromServer(dtSessCtl.Rows(0).Item("Facility"), strComputerName)
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub

    Public Shared Sub ExtractZip(ByVal ZipFile As String, ByVal Output As String)

        Dim ShellAppType As Type = Type.GetTypeFromProgID("Shell.Application")
        Dim objShell As Object = Activator.CreateInstance(ShellAppType)
        Dim objOutput = objShell.NameSpace(Output.ToString)
        Dim objZipFile = objShell.NameSpace(ZipFile.ToString)

        Try
            For Each files As Object In objZipFile.Items
                objOutput.MoveHere(files)
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'WO#16894 ADD Stop
End Class
