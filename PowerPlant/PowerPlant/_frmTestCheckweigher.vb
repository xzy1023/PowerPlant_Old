Public Class frmTestCheckweigher
    Dim strCurrQATEntryPoint As String
    Dim drwf As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow
    Const strNoData As String = "No Data"

    Private Sub frmTestCheckweigher_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Retrieve QAT work flow and test information
        strCurrQATEntryPoint = "S"
        drwf = SharedFunctions.GetQATWorkFlowInfo(gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, strCurrQATEntryPoint, "frmQATCheckWeigherValidation")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        LoadDataFromCheckWeigher(drwf.TCPConnID)
    End Sub

    Private Sub LoadDataFromCheckWeigher(intTCPConnID As Integer)
        ' WO#17432 ADD Start – AT 10/23/2018
        Dim strIPAddress As String = String.Empty
        ' WO#17432 ADD Stop – AT 10/23/2018
        Dim strResponse As String = Nothing
        Dim drTCPConn As dsQATTCPConn.CPPsp_QATTCPConn_SelRow
        Dim strMessage As String() = {"Please wait ...", "ElapseTime", "Retrieving data from Checkweigher"}
        'Dim strSplashOption As String = "ElapseTime"
        'Dim strSplashTitle As String = "Retrieving data from Checkweigher"
        Dim objCheckWeigher As clsCheckWeigher
        Try

            Using taTCPConn As New dsQATTCPConnTableAdapters.CPPsp_QATTCPConn_SelTableAdapter
                Using dtTCPConn As New dsQATTCPConn.CPPsp_QATTCPConn_SelDataTable
                    taTCPConn.Fill(dtTCPConn, gdrSessCtl.Facility, drwf.TCPConnID, True)
                    If dtTCPConn.Count > 0 Then
                        drTCPConn = dtTCPConn(0)
                        'objCheckWeigher = New clsCheckWeigher(drTCPConn.IPAddress, drTCPConn.Port)
                        ' WO#17432 ADD Start – AT 10/23/2018
                        strIPAddress = drTCPConn.IPAddress
                        objCheckWeigher = New clsCheckWeigher(strIPAddress.Replace(vbCr, "").Replace(vbLf, "").TrimEnd(" ", ""), drTCPConn.Port)
                        ' WO#17432 ADD Stop – AT 10/23/2018
                        With objCheckWeigher
                            strResponse = .TCPConnect()
                            'If strResponse <> "No Connection" Then
                            If IsNothing(strResponse) Then
                                If dtTCPConn(0).Model = "XE" Then
                                    If .IsHandShakeOK() Then
                                        'send command "WD_SET_FORMAT 2100" to change data receiving format 2100 for all the detail data.
                                        .TCPSendCommand(drTCPConn.Command1)
                                        strResponse = objCheckWeigher.TCPReceiveMessage()
                                        If strResponse <> strNoData Then
                                            ReceivedFormat2100Data(strResponse)
                                            If drTCPConn.Command2 <> String.Empty Then
                                                'send command "WD_SET_FORMAT x" to change data receiving format x for recepe name.
                                                .TCPSendCommand(drTCPConn.Command2)
                                                System.Threading.Thread.Sleep(3000)
                                                strResponse = .TCPReceiveMessage()
                                                If strResponse <> strNoData Then
                                                    ReceivedXSFormat3Data(strResponse)
                                                    ' WO#17432 ADD Start – AT 10/23/2018
                                                Else
                                                    ' WO#17432 ADD Stop – AT 10/23/2018

                                                    MessageBox.Show("No data is received " & drTCPConn.Command2 & ". Is checkweigher operating properly? Please click on RETRY button to load data again.", "Checkweigher - " & drTCPConn.IPAddress, MessageBoxButtons.OK)
                                                End If
                                            End If
                                            ' WO#17432 ADD Start – AT 10/23/2018
                                        Else
                                            ' WO#17432 ADD Stop – AT 10/23/2018

                                            MessageBox.Show("No data is received from " & drTCPConn.Command1 & ". Is checkweigher operating properly? Please click on RETRY button to load data again.", "Checkweigher - " & drTCPConn.IPAddress, MessageBoxButtons.OK)
                                        End If
                                    Else

                                        MessageBox.Show("The checkweigher is not responding in handshake or being allocated by another session. If retry few times still having same issue, please contact supervisor.", "Checkweigher - " & drTCPConn.IPAddress, MessageBoxButtons.OK)
                                    End If
                                ElseIf dtTCPConn(0).Model = "XC" Then
                                    If .IsHandShakeOK() Then
                                        'send command "WD_SET_FORMAT 3" to change data receiving format 3 for detail data.
                                        .TCPSendCommand(drTCPConn.Command1)
                                        System.Threading.Thread.Sleep(4000)
                                        'send command "WD_START to start receiving data from checkweigher
                                        strResponse = .Start()
                                        Debug.Print(strResponse)
                                        If strResponse <> strNoData Then
                                            ReceivedXCFormat3Data(strResponse)
                                            For i As Integer = 1 To 2
                                                strResponse = objCheckWeigher.TCPReceiveMessage()
                                                ReceivedXCFormat3Data(strResponse)
                                                Me.ListBox1.Refresh()
                                            Next
                                            ' WO#17432 ADD Start – AT 10/23/2018
                                        Else
                                            ' WO#17432 ADD Stop– AT 10/23/2018

                                            ' Me.Refresh()
                                            MessageBox.Show("No data is received from checkweigher. Is it operating properly? Please click on RETRY button to load data again.", "Checkweigher - " & drTCPConn.IPAddress, MessageBoxButtons.OK)
                                        End If
                                    Else

                                        'Me.Refresh()
                                        MessageBox.Show("The checkweigher is not responding in handshake or being allocated by another session. If retry few times still having same issue, please contact supervisor.", "Checkweigher - " & drTCPConn.IPAddress, MessageBoxButtons.OK)
                                    End If
                                End If
                            Else
                                ' WO#17432 ADD Start – AT 10/23/2018
                                'MessageBox.Show("The checkweigher is not responding.", "Checkweigher - " & drTCPConn.IPAddress, MessageBoxButtons.OK)


                                MessageBox.Show(strResponse, "Checkweigher - " & drTCPConn.IPAddress, MessageBoxButtons.OK)
                                ' WO#17432 ADD Stop – AT 10/23/2018

                            End If
                            .TCPDisconnect()
                        End With
                    End If
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error in LoadDataFromCheckWeigher" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub ReceivedFormat2100Data(strResponse As String)
        'Dim decTargetWgt As Decimal
        'Dim strUnit As String = String.Empty
        'Dim strActualWeight As String
        'Dim decValue As Decimal

        'Decimal.TryParse(strResponse.Substring(0, 6), decTargetWgt)
        ''lblTargetWgt.Text = decTargetWgt.ToString
        ''lblMaxWgt.Text = (decTargetWgt + Convert.ToDecimal(strResponse.Substring(8, 6))).ToString
        ''lblMinWgt.Text = (decTargetWgt + Convert.ToDecimal(strResponse.Substring(15, 6))).ToString
        ''lblTareWgt.Text = Convert.ToDecimal(strResponse.Substring(43, 6)).ToString

        'Decimal.TryParse(strResponse.Substring(0, 6), decTargetWgt)
        'lblTareWgt.Text = Convert.ToDecimal(strResponse.Substring(43, 6)).ToString

        '' WO#17432 ADD Start – AT 10/05/2018
        'strUnit = strResponse.Substring(39, 3).TrimEnd(" ", "")
        'strActualWeight = strResponse.Substring(32, 6)
        'decActualWeight = Convert.ToDecimal(strActualWeight).ToString
        'lblActualWgt.Text = Convert.ToDecimal(strActualWeight).ToString & " " & strUnit.Substring(0, 3).TrimEnd(".", "")
        '' lblActualWgt.Text = Convert.ToDecimal(strResponse.Substring(32, 6)).ToString 
        '' WO#17432 ADD Stop – AT 10/05/2018
        'If Decimal.TryParse(decActualWeight, decValue) Then
        '    If IsWeightPass(decActualWeight) Then
        '        TestFailed()
        '    Else
        '        TestPassed()
        '    End If
        'Else
        '    TestFailed()
        'End If
    End Sub

    Private Sub ReceivedXSFormat3Data(strResponse As String)
        'lblRecipeName.Text = strResponse.Substring(0, 10).Trim
    End Sub

    Private Sub ReceivedXCFormat3Data(strResponse As String)
        'Dim strActualWeight As String
        'Dim decValue As Decimal

        Try
            ListBox1.BeginUpdate()
            ListBox1.Items.Add(strResponse)
            ListBox1.EndUpdate()

            'lblRecipeName.Text = strResponse.Substring(4, 6).Trim
            'strActualWeight = strResponse.Substring(10, 7)
            'decActualWeight = Convert.ToDecimal(strActualWeight).ToString
            'lblActualWgt.Text = Convert.ToDecimal(strActualWeight).ToString & " " & strResponse.Substring(17, 3).TrimEnd(" ", "")
            'If Decimal.TryParse(decActualWeight, decValue) Then
            '    If IsWeightPass(decActualWeight) Then
            '        TestFailed()
            '    Else
            '        TestPassed()
            '    End If
            'Else
            '    TestFailed()
            'End If

        Catch ex As Exception
            Throw New Exception("Error in ReceivedXCFormat3Data" & vbCrLf & ex.Message)
        End Try
    End Sub
End Class