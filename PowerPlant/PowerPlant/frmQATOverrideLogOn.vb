Public Class frmQATOverrideLogOn

    Private dteOverrideID As DateTime
    Private intQATDefnID As Integer
    Private strQATEntryPoint As String
    Private dteBatchID As DateTime
    Private blnAlert As Boolean
    Private strTestTitle As String

    Dim strOriginalByPassLanes As String
    Dim gstrErrMsg As String
    Dim drWF As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow
    Dim drPlantStaff As dsPlantStaffing.CPPsp_PlantStaffingIORow

    Const strBtnPrefix As String = "btn_"
    ' WO#17432 ADD Stop – AT 12/05/2018
    Dim strNewPass As String
    Dim blnGimaNonStartup12Samples As Boolean

    Public Property NewPass As String
        Get
            Return strNewPass
        End Get
        Set(value As String)
            strNewPass = value
        End Set
    End Property
    ' WO#17432 ADD Stop – AT 12/05/2018
    Public Property Alert As Boolean
        Get
            Return blnAlert
        End Get
        Set(value As Boolean)
            blnAlert = value
        End Set
    End Property

    Public Property OverrideID As DateTime
        Get
            Return dteOverrideID
        End Get
        Set(value As DateTime)
            dteOverrideID = value
        End Set
    End Property

    Public Property QATEntryPoint As String
        Get
            Return strQATEntryPoint
        End Get
        Set(value As String)
            strQATEntryPoint = value
        End Set
    End Property

    Public Property QATDefnID As Integer
        Get
            Return intQATDefnID
        End Get
        Set(value As Integer)
            intQATDefnID = value
        End Set
    End Property

    Public Property BatchID As DateTime
        Get
            Return dteBatchID
        End Get
        Set(value As DateTime)
            dteBatchID = value
        End Set
    End Property

    Public Property TestTitle As String
        Get
            Return strTestTitle
        End Get
        Set(value As String)
            strTestTitle = value
        End Set
    End Property

    Private Sub frmQATOverrrideLogOn_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strCallerFormName As String = Nothing
        Try
            'Set default return value of the form.
            Me.DialogResult = Windows.Forms.DialogResult.None
            txtUserID.Text = String.Empty
            txtPassword.Text = String.Empty
            lblUserName.Text = String.Empty

            strCallerFormName = Me.Owner.Name
            Using daWorkFlow As New dsQATWorkFlowTableAdapters.CPPsp_QATWorkFlow_SelTableAdapter
                Using dtWorkFlow As New dsQATWorkFlow.CPPsp_QATWorkFlow_SelDataTable
                    daWorkFlow.Fill(dtWorkFlow, gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, strQATEntryPoint, True, strCallerFormName)
                    If dtWorkFlow.Count = 1 Then
                        drWF = dtWorkFlow(0)
                    Else
                        MessageBox.Show("Cannot find the QAT definition for this line. Please contact QA.", "Error - Missing test specification", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Close()
                        Exit Sub
                    End If
                End Using
            End Using

            If drWF.NoOfLanes > 0 Then
                lblOvrLanes.Visible = True
                lblTestType.Text = strTestTitle
                lblTestType.Visible = True
                lblItemNo.Text = "Item: " & gdrSessCtl.ItemNumber
                lblItemNo.Visible = True
                ' WO#17432 ADD Start – AT 11/19/2018
                If drWF.FormName = "frmQATOxygen" AndAlso drWF.NoOfSamples = 12 AndAlso gdrCmpCfg.PkgLineType = "Gima" AndAlso QATEntryPoint <> "S" Then
                    blnGimaNonStartup12Samples = True
                    InitializeLaneButtonsGima(6)        'Maximuim no. of lanes for Gima lines are 6
                Else
                    blnGimaNonStartup12Samples = False
                    InitializeLaneButtons(drWF.NoOfLanes)
                End If
                ' WO#17432 ADD Stop – AT 11/19/2018
            Else
                lblOvrLanes.Visible = False
                lblTestType.Visible = False
                lblItemNo.Visible = False
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
        End Try
    End Sub

    Private Sub popupNumKeyPad(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtUserID.MouseDown, txtPassword.MouseDown
        Dim dgrKeyPad As DialogResult
        Try
            'WO#17432 ADD Start
            If sender.name = "txtPassword" Then
                dgrKeyPad = SharedFunctions.PopNumKeyPad(Me, sender, , , "*")
            Else
                dgrKeyPad = SharedFunctions.PopNumKeyPad(Me, sender)

            End If
            'WO#17432 ADD Stop

            If dgrKeyPad = Windows.Forms.DialogResult.OK Then
                If Not IsNothing(gstrNumPadValue) AndAlso gstrNumPadValue <> "" Then
                    sender.text = Microsoft.VisualBasic.Left(gstrNumPadValue, sender.maxLength)
                Else
                    sender.text = ""
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtUserID_TextChanged(sender As Object, e As EventArgs) Handles txtUserID.TextChanged
        Dim strUserName As String = String.Empty
        Try
            gstrErrMsg = Nothing

            If txtUserID.Text <> "" Then
                lblUserName.Visible = False
                'strUserName = SharedFunctions.GetStaffName(txtUserID.Text, Nothing, "Supervisor")
                drPlantStaff = SharedFunctions.GetPlantStaff(txtUserID.Text, Nothing, True, "Supervisor")
                If Not IsNothing(drPlantStaff) Then
                    strUserName = drPlantStaff.FullName
                End If

                If strUserName <> "" Then
                    lblUserName.Visible = True
                    lblUserName.Text = strUserName

                    If drPlantStaff.ActiveRecord = False Then
                        gstrErrMsg = "Please enter an active user ID."
                        MessageBox.Show(gstrErrMsg, "Invalid information")
                        txtUserID.Focus()
                    End If
                Else
                    gstrErrMsg = "Please enter a valid User ID."
                    MessageBox.Show(gstrErrMsg, "Invalid information")
                    txtUserID.Focus()
                End If

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtUserID_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtUserID.Validating
        If Not IsNothing(gstrErrMsg) Then
            MessageBox.Show(gstrErrMsg, "Invalid information")
            e.Cancel = True
        End If
    End Sub

    Private Sub txtPassword_TextChanged(sender As Object, e As EventArgs) Handles txtPassword.TextChanged
        Dim strCurrPass As String = String.Empty
        Dim strInputPass As String = String.Empty
        Try
            gstrErrMsg = Nothing
            If txtPassword.Text <> "" Then
                If txtUserID.Text = "" Then
                    gstrErrMsg = "Please enter the user ID first."
                    MessageBox.Show(gstrErrMsg, "Missing information")
                    txtUserID.Focus()
                    Exit Sub                'WO#17432 – AT 12/05/2018
                Else
                    If txtPassword.Text <> Cryptography.Decrypt(drPlantStaff.Password) Then
                        gstrErrMsg = "Please enter valid password."
                        MessageBox.Show(gstrErrMsg, "Invalid information")
                        txtPassword.Focus()
                        Exit Sub            'WO#17432 – AT 12/05/2018
                    End If
                End If
                If drPlantStaff.ResetPassword = True Then
                    DisableAcceptButtons()
                    EnableChangePassword()
                Else
                    EnableAcceptButtons()
                End If
            End If
        Catch ex As Exception
            Throw New Exception("Error in txtPassword_TextChanged" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub txtPassword_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtPassword.Validating
        If Not IsNothing(gstrErrMsg) Then
            MessageBox.Show(gstrErrMsg, "Invalid information")
            e.Cancel = True
        End If
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Me.DialogResult = Windows.Forms.DialogResult.Ignore
        Me.Close()
    End Sub

    Private Sub btnBypassCurrentTest_Click(sender As Object, e As EventArgs) Handles btnBypassCurrentTest.Click
        Dim drOvr As dsQATOverride.CPPsp_QATOverride_SelRow
        Try
            If txtUserID.Text = String.Empty Then
                MessageBox.Show("PLease enter User ID.", "Missing input data", MessageBoxButtons.OK)
            ElseIf txtPassword.Text = String.Empty Then
                MessageBox.Show("PLease enter Password.", "Missing input data", MessageBoxButtons.OK)
            Else

                Using daOvr As New dsQATOverrideTableAdapters.CPPsp_QATOverride_SelTableAdapter
                    Using dt As New dsQATOverride.CPPsp_QATOverride_SelDataTable
                        drOvr = dt.NewRow()
                        With drOvr
                            .BatchID = dteBatchID
                            .ByPassLanes = String.Empty
                            .ByPassTest = True
                            .Facility = gdrSessCtl.Facility
                            .OverridedBy = txtUserID.Text
                            .OverrideID = dteOverrideID
                            .PackagingLine = gdrSessCtl.DefaultPkgLine
                            .QATDefnID = intQATDefnID
                            .ShopOrder = gdrSessCtl.ShopOrder
                            .SOStartTime = gdrSessCtl.StartTime
                            .Alert = blnAlert
                        End With
                        dt.Rows.Add(drOvr)
                        If gblnSvrConnIsUp = True Then
                            Try
                                daOvr.Connection.ConnectionString = gstrServerConnectionString
                                daOvr.Update(dt)
                            Catch ex As SqlClient.SqlException When (ex.Number = 64 Or ex.Number = 1231) And gblnSvrConnIsUp = True
                                SharedFunctions.SetServerCnnStatusInSessCtl(False)
                                daOvr.Connection.ConnectionString = gstrLocalDBConnectionString
                                daOvr.Update(dt)
                            End Try
                        Else
                            daOvr.Connection.ConnectionString = gstrLocalDBConnectionString
                            daOvr.Update(dt)
                        End If
                    End Using
                End Using
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnAccept_Click(sender As Object, e As EventArgs) Handles btnAccept.Click
        Dim drOvr As dsQATOverride.CPPsp_QATOverride_SelRow
        Dim strByPassLanes As String = String.Empty
        Dim intSkipCount As Int16
        Dim intNoOfLanesToTest As Int16
        Try
            If txtUserID.Text = String.Empty Then
                MessageBox.Show("PLease enter User ID.", "Missing input data", MessageBoxButtons.OK)
            ElseIf txtPassword.Text = String.Empty Then
                MessageBox.Show("PLease enter Password.", "Missing input data", MessageBoxButtons.OK)
            Else
                If drWF.NoOfLanes <> 0 Then
                    For Each ctrl As Control In Controls
                        If TypeOf ctrl Is Button AndAlso ctrl.Name.Substring(0, 4) = strBtnPrefix Then
                            If ctrl.Tag = "1" Then
                                strByPassLanes = strByPassLanes & CInt(ctrl.Text).ToString & ","
                                intSkipCount = intSkipCount + 1
                            End If
                        End If
                    Next
                End If

                If blnGimaNonStartup12Samples Then
                    intNoOfLanesToTest = 2
                Else
                    intNoOfLanesToTest = drWF.NoOfLanes
                End If
                If drWF.NoOfLanes <> 0 AndAlso intSkipCount = intNoOfLanesToTest Then
                    MessageBox.Show("Cannot skip all lanes for testing.", "Invalid Selection", MessageBoxButtons.OK)
                Else

                    Using daOvr As New dsQATOverrideTableAdapters.CPPsp_QATOverride_SelTableAdapter
                        Using dtOvr As New dsQATOverride.CPPsp_QATOverride_SelDataTable
                            drOvr = dtOvr.NewRow()
                            With drOvr
                                .BatchID = dteBatchID
                                .ByPassLanes = strByPassLanes
                                .ByPassTest = False
                                .Facility = gdrSessCtl.Facility
                                .OverridedBy = txtUserID.Text
                                .OverrideID = dteOverrideID
                                .PackagingLine = gdrSessCtl.DefaultPkgLine
                                .QATDefnID = intQATDefnID
                                .ShopOrder = gdrSessCtl.ShopOrder
                                .SOStartTime = gdrSessCtl.StartTime
                                .Alert = blnAlert
                            End With
                            dtOvr.Rows.Add(drOvr)
                            If gblnSvrConnIsUp = True Then
                                Try
                                    daOvr.Update(drOvr)
                                Catch ex As SqlClient.SqlException When (ex.Number = 53 Or ex.Number = 64 Or ex.Number = 1231)
                                    SharedFunctions.SetServerCnnStatusInSessCtl(False)
                                    daOvr.Connection.ConnectionString = gstrLocalDBConnectionString
                                    daOvr.Update(drOvr)
                                End Try
                            Else
                                daOvr.Connection.ConnectionString = gstrLocalDBConnectionString
                                daOvr.Update(drOvr)
                            End If

                        End Using
                    End Using

                    If strByPassLanes = strOriginalByPassLanes Then
                        'Bypass lanes are not changed
                        Me.DialogResult = Windows.Forms.DialogResult.OK
                    Else
                        'Bypass lanes have been changed
                        Me.DialogResult = Windows.Forms.DialogResult.Yes
                    End If
                    Me.Close()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnLane_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim btn As Button
        btn = CType(sender, Button)

        If btn.BackColor = Color.LemonChiffon Then
            btn.BackColor = Color.Red
            btn.Tag = 1
        ElseIf btn.BackColor = Color.Red Then
            btn.BackColor = Color.LemonChiffon
            btn.Tag = 0
        End If
    End Sub

    ' WO#17432 ADD Start – AT 11/19/2018
    Private Sub InitializeLaneButtonsGima(intNoOfLanes As Int16)
        '
        ' Assumption: Maximum no. of samples is 36,
        '             Maximum no. of lanes is 6,
        '             no. of lanes if specified can not be less than no. of samples.
        '

        Dim btns As Button()
        Dim btn As Button

        Dim intBtnX As Int16, intBtnY As Int16
        Dim intLaneNo As Int16, intRowNo As Int16, intColNo As Int16

        Const intBtnWidth As Int16 = 107, intBtnHeight As Int32 = 60
        Const intIntialBtnX As Int16 = 41, intIntialBtnY As Int16 = 302
        Const intBtnXGap As Int16 = 122, intBtnYGap As Int16 = 0
        Const intMaxRows As Int16 = 2, intMaxCols As Int16 = 6

        Dim intNoOfRows As Int16

        Try
            'Remove all the button controls for Overridding the lanes.
            For cnt As Integer = 1 To intNoOfLanes
                For Each ctrl As Control In Controls
                    If TypeOf ctrl Is Button Then
                        If ctrl.Name.Substring(0, 4) = strBtnPrefix Then
                            Me.Controls.Remove(ctrl)
                        End If
                    End If
                Next
            Next

            strOriginalByPassLanes = SharedFunctions.GetLastQATOverrideByPassLanes(gdrSessCtl.Facility, gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)

            intNoOfRows = Math.Floor(intNoOfLanes / intMaxCols)
            If intNoOfRows = 0 Then
                intNoOfRows = 1
            End If

            If intNoOfRows > intMaxRows Then
                intNoOfRows = intMaxRows
            End If

            ReDim btns(intNoOfLanes)    'no. of elements in the array is intNoOfLanes + 1
            Dim intFirstLaneNo As Integer = 0
            Dim intLastLaneNo As Integer = 0
            Dim intNewColNo As Integer = 0
            intFirstLaneNo = 2
            intLastLaneNo = intNoOfLanes - 1
            intNewColNo = 2
            intLaneNo = 1
            For intColNo = 0 To intMaxCols - 1
                For intRowNo = 0 To intNoOfRows - 1 Step 1
                    If intLaneNo < intFirstLaneNo Or intLaneNo > intLastLaneNo Then
                        If intLaneNo > intLastLaneNo Then
                            intNewColNo = 1
                        Else
                            intNewColNo = intColNo
                        End If
                        btn = New Button
                        With btn
                            intBtnX = intIntialBtnX + intNewColNo * intBtnXGap
                            intBtnY = intIntialBtnY + intRowNo * intBtnYGap
                            .Location = New System.Drawing.Point(intBtnX, intBtnY)
                            .Text = Format(intLaneNo, "00")
                            .Size = New System.Drawing.Size(intBtnWidth, intBtnHeight)
                            .Name = strBtnPrefix & (intLaneNo).ToString
                            .ForeColor = Color.Black
                            If strOriginalByPassLanes <> String.Empty AndAlso strOriginalByPassLanes.IndexOf(intLaneNo.ToString & ",") >= 0 Then
                                .BackColor = Color.Red
                                .Tag = 1
                            Else
                                .BackColor = Color.LemonChiffon
                                .Tag = 0
                            End If
                            .Font = New Font("Arial", 18, System.Drawing.FontStyle.Regular)

                            .BringToFront()
                        End With
                        btns(intLaneNo) = btn
                        AddHandler btns(intLaneNo).Click, AddressOf btnLane_click
                        Me.Controls.Add(btn)
                    End If

                    intLaneNo = intLaneNo + 1
                Next
            Next

        Catch ex As Exception
            Throw New Exception("Error in InitializeLaneButtonsGima" & vbCrLf & ex.Message)
        End Try
    End Sub
    ' WO#17432 ADD Stop – AT 11/19/2018

    Private Sub InitializeLaneButtons(intNoOfLanes As Int16)
        '
        ' Assumption: Maximum no. of samples is 36,
        '             Maximum no. of lanes is 6,
        '             no. of lanes if specified can not be less than no. of samples.
        '

        Dim btns As Button()
        Dim btn As Button

        Dim intBtnX As Int16, intBtnY As Int16
        Dim intLaneNo As Int16, intRowNo As Int16, intColNo As Int16

        Const intBtnWidth As Int16 = 107, intBtnHeight As Int32 = 60
        Const intIntialBtnX As Int16 = 41, intIntialBtnY As Int16 = 302
        Const intBtnXGap As Int16 = 122, intBtnYGap As Int16 = 0
        Const intMaxRows As Int16 = 2, intMaxCols As Int16 = 6

        Dim intNoOfRows As Int16

        Try
            'Remove all the button controls for Overridding the lanes.
            For cnt As Integer = 1 To intNoOfLanes
                For Each ctrl As Control In Controls
                    If TypeOf ctrl Is Button Then
                        If ctrl.Name.Substring(0, 4) = strBtnPrefix Then
                            Me.Controls.Remove(ctrl)
                        End If
                    End If
                Next
            Next

            strOriginalByPassLanes = SharedFunctions.GetLastQATOverrideByPassLanes(gdrSessCtl.Facility, gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)

            intNoOfRows = Math.Floor(intNoOfLanes / intMaxCols)
            If intNoOfRows = 0 Then
                intNoOfRows = 1
            End If

            If intNoOfRows > intMaxRows Then
                intNoOfRows = intMaxRows
            End If

            ReDim btns(intNoOfLanes)    'no. of elements in the array is intNoOfLanes + 1

            intLaneNo = 1
            For intColNo = 0 To intMaxCols - 1
                For intRowNo = 0 To intNoOfRows - 1 Step 1

                    btn = New Button
                    With btn
                        intBtnX = intIntialBtnX + intColNo * intBtnXGap
                        intBtnY = intIntialBtnY + intRowNo * intBtnYGap
                        .Location = New System.Drawing.Point(intBtnX, intBtnY)
                        .Text = Format(intLaneNo, "00")
                        .Size = New System.Drawing.Size(intBtnWidth, intBtnHeight)
                        .Name = strBtnPrefix & (intLaneNo).ToString
                        .ForeColor = Color.Black
                        If strOriginalByPassLanes <> String.Empty AndAlso strOriginalByPassLanes.IndexOf(intLaneNo.ToString & ",") >= 0 Then
                            .BackColor = Color.Red
                            .Tag = 1
                        Else
                            .BackColor = Color.LemonChiffon
                            .Tag = 0
                        End If
                        .Font = New Font("Arial", 18, System.Drawing.FontStyle.Regular)

                        .BringToFront()
                    End With
                    btns(intLaneNo) = btn
                    AddHandler btns(intLaneNo).Click, AddressOf btnLane_click
                    Me.Controls.Add(btn)
                    intLaneNo = intLaneNo + 1
                Next
            Next

        Catch ex As Exception
            Throw New Exception("Error in InitializeLaneButtons" & vbCrLf & ex.Message)
        End Try

    End Sub

    Private Sub btnChangePassword_Click(sender As Object, e As EventArgs) Handles btnChangePassword.Click
        Try
            EnableChangePassword()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub EnableChangePassword()
        Dim drScreen As DialogResult
        Try
            If txtUserID.Text = String.Empty Then
                MessageBox.Show("PLease enter User ID.", "Missing input data", MessageBoxButtons.OK)
            ElseIf txtPassword.Text = String.Empty Then
                MessageBox.Show("Please enter Password.", "Missing input data", MessageBoxButtons.OK)
            Else
                With frmChangePassword
                    .UserID = txtUserID.Text
                    .UserName = lblUserName.Text
                    .OldPass = txtPassword.Text
                    drScreen = .ShowDialog(Me)
                End With

                'If password was changed, fill in the new password to the password text box
                If drScreen = Windows.Forms.DialogResult.Yes Then
                    drPlantStaff = SharedFunctions.GetPlantStaff(txtUserID.Text, Nothing, True, "Supervisor")
                    txtPassword.Text = NewPass
                    EnableAcceptButtons()
                End If
                If drScreen = Windows.Forms.DialogResult.Ignore Then
                    txtPassword.Text = ""
                End If
            End If
        Catch ex As Exception
            Throw New Exception("Error in EnableChangePassword" & vbCrLf & ex.Message)
        End Try

    End Sub
    Private Sub EnableAcceptButtons()
        btnAccept.Enabled = True
        btnAccept.Visible = True
        btnBypassCurrentTest.Enabled = True
        btnBypassCurrentTest.Visible = True
    End Sub
    Private Sub DisableAcceptButtons()
        btnAccept.Enabled = False
        btnAccept.Visible = False
        btnBypassCurrentTest.Enabled = False
        btnBypassCurrentTest.Visible = False
    End Sub
    ' WO#17432 ADD Stop – AT 12/05/2018
End Class

