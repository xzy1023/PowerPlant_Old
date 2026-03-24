Public Class frmCheckConnection
    '---------------------------------------------------------------------------------
    'Author:        Bong Lee
    'Creation date: Mar. 8,2012
    'Description:   WO#359 - allow user to verify network connection for IPC 
    '               and IPC's associated printers
    '---------------------------------------------------------------------------------
    Dim strMsgBoxTitle As String = "Check Network Connection"

    Private Sub frmCheckConnection_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            UcHeading1.ScreenTitle = "Check Connections"
            LoadPrinterList("me.Load")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub dgvPrtDev_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvPrtDev.CellMouseDown
        Try
            If My.Computer.Network.IsAvailable = True Then 'WO#359
                'Clear the result in each row
                Dim row As DataGridViewRow
                For Each row In dgvPrtDev.Rows
                    row.Cells("dgvPrtDev_txtResult").Value = String.Empty
                Next
                If e.RowIndex >= 0 AndAlso dgvPrtDev.Columns(e.ColumnIndex).Name = "dgvPrtDev_btnTest" Then
                    With dgvPrtDev.Rows(e.RowIndex)
                        Try
                            If My.Computer.Network.Ping(.Cells("dgvPrtDev_txtIPAddress").Value) Then
                                If SharedFunctions.IsDeviceConnected(.Cells("dgvPrtDev_txtIPAddress").Value) = True Then
                                    .Cells("dgvPrtDev_txtResult").Value = "Pass"
                                    .Cells("dgvPrtDev_txtResult").Style.ForeColor = Color.LimeGreen
                                Else
                                    .Cells("dgvPrtDev_txtResult").Value = "Failure"
                                    .Cells("dgvPrtDev_txtResult").Style.ForeColor = Color.Red
                                End If
                            Else
                                .Cells("dgvPrtDev_txtResult").Value = "Failure"
                                .Cells("dgvPrtDev_txtResult").Style.ForeColor = Color.Red
                            End If
                        Catch sqlEX As SqlClient.SqlException
                            Throw sqlEX
                        Catch ex As Exception
                            .Cells("dgvPrtDev_txtResult").Value = "Wrg. IP"
                            .Cells("dgvPrtDev_txtResult").Style.ForeColor = Color.Red
                        End Try
                    End With
                End If
            Else
                Dim strMsg As String
                strMsg = "Network connection is not ready, Please check the IPC connection before any printer connections."
                SharedFunctions.PoPUpMSG(strMsg, strMsgBoxTitle, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCheckNetworkConn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheckNetworkConn.Click
        Dim cnnServer As New System.Data.SqlClient.SqlConnection(gstrServerConnectionString)
        Dim strMsg As String

        Try
            If My.Computer.Network.IsAvailable = True Then
                Try
                    cnnServer.Open()
                Catch ex As System.Data.SqlClient.SqlException
                    strMsg = "Network connection is ready but data server connection is failure. Please try again Later."
                    SharedFunctions.PoPUpMSG(strMsg, "Check Network Connection", MessageBoxButtons.OK)
                    Exit Sub
                End Try
                If gdrCmpCfg.PalletStation = True Then
                    strMsg = "Network Connection for this IPC is ready. Please return to Log On Menu to re-logon to reclaim the connection."
                Else
                    strMsg = "Network Connection for this IPC is ready. If the connection was failure when shop order was running, Please restart the shop order."
                End If
                LoadPrinterList()
            Else
                strMsg = "Network connection is not ready, Please try again Later."
            End If
            SharedFunctions.PoPUpMSG(strMsg, strMsgBoxTitle, MessageBoxButtons.OK)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub LoadPrinterList(Optional ByVal strWhen As String = "")
        Dim da As New dsPrinterDeviceTableAdapters.taPrinterDevice
        Dim dt As New dsPrinterDevice.tblPkgLinePrinterDeviceDataTable
        Try
            Dim strMsg As String   'FX150604
            If My.Computer.Network.IsAvailable = True Then
                'I found the following codes do not work, I have to do it from the dsPrintDevice.xsd
                'With DsPrinterDevice.tblPkgLinePrinterDevice
                '    .PackagingLineColumn.AllowDBNull = True
                '    .DeviceTypeColumn.AllowDBNull = True
                '    .facilityColumn.AllowDBNull = True
                '    .UseNativeDriverColumn.AllowDBNull = True
                'End With

                Try 'FX150604
                    da.Fill(dt, "VerifyConnection", gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine)
                    Me.dgvPrtDev.DataSource = dt
                    dgvPrtDev.Refresh()
                    'FX150604 ADD Start
                Catch sqEx As SqlClient.SqlException
                    strMsg = "Can not retrieve the printer list for the line. Please check the IPC or data server connection."
                    SharedFunctions.PoPUpMSG(strMsg, strMsgBoxTitle, MessageBoxButtons.OK)
                End Try
                'FX150604 ADD Stop
            Else
                'FX150604   Dim strMsg As String
                If strWhen = "me.Load" Then
                    'FX150604 strMsg = "Network connection is not ready, Please check the IPC connection."
                    strMsg = "Please check the IPC network connection." 'FX150604
                Else
                    'FX150604 strMsg = "Network connection is not ready, Please check the IPC connection before any printer connections."
                    strMsg = "Please check the IPC network connection before any printer network connections." 'FX150604
                End If
                SharedFunctions.PoPUpMSG(strMsg, strMsgBoxTitle, MessageBoxButtons.OK)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class