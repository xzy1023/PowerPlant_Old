Public Class frmLogOn_NoPLC
    Dim gshtCurrentShift As Short
    Dim gstrErrMsg As String
    Dim ldaShift As New dsShiftTableAdapters.CPPsp_ShiftIOTableAdapter          'WO#2645
    Dim ldtShift As New dsShift.CPPsp_ShiftIODataTable                          'WO#2645
    Dim ldrshift As dsShift.CPPsp_ShiftIORow                                    'WO#2645

    'Private Sub frmLogOn_NoPLC_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated 'WO#755
    '    txtPkgLine.Text = gdrSessCtl.DefaultPkgLine                                                                'WO#755
    'End Sub                                                                                                        'WO#755

    Private Sub frmSignOn_NoPLC_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            SharedFunctions.clearInputFields(Me)
            Me.UcHeading1.lblScreenTitle.Text = "Log On"

            'Me.txtPkgLine.Text = gdrCmpCfg("PackagingLine")
            txtPkgLine.Text = gdrSessCtl.DefaultPkgLine
            If txtPkgLine.Text = "" Then
                lblPkgLine.Visible = False
            End If

            If Not IsDBNull(gdrSessCtl.Operator) Then
                Me.lblOperator.Text = ""
                txtOperator.Text = gdrSessCtl.Operator

                If lblOperator.Text = "" Then
                    Me.lblOperator.Visible = False
                Else
                    Me.lblOperator.Visible = True
                End If
                'Me.txtOperator.Focus()
            End If

            If gdrSessCtl.ShopOrder > 0 Then
                'WO#359 ADD Start
                'get the current shift no before compare with the one from shop order
                'WO#2645 Add Start
                ldaShift.Fill(ldtShift, "ExpectedShift", gdrSessCtl.Facility, gdrCmpCfg.WorkShiftType, Now(), Nothing, 0)
                If ldtShift.Rows.Count > 0 Then
                    gshtCurrentShift = ldtShift.Rows(0).Item("Shift")
                End If
                'WO#2645 Add Stop
                'WO#2645 DEL Start
                'Dim myShift As New WorkShift(Now, gdrCmpCfg("WorkShiftTYpe"))
                'If Not IsNothing(myShift) Then
                '    gshtCurrentShift = myShift.Shift
                'End If
                'WO#2645 DEL Stop
                'WO#359 ADD Stop
                txtShift.Text = gdrSessCtl.OverrideShiftNo
            Else
                refreshShiftInfo()
            End If

            'txtOperator.Focus()

            If gdrSessCtl.ShopOrder > 0 Then
                SharedFunctions.PoPUpMSG(String.Format("Shop order {0} started by {1} is still open.", gdrSessCtl.ShopOrder, gdrSessCtl.OperatorName), _
                    "Warning - Last Shop Order is still left open.", MessageBoxButtons.OK)
                'WO#5370 ADD Start
                'If gblnSarongAutoCountLine = True Then
                '    'Retreive the last output location and destination shop order from tblUnitCountOutBound
                '    Try
                '        Dim daOutBound As New dsUnitCountOutBoundTableAdapters.tblUnitCountOutboundTableAdapter
                '        Dim dtOutBound As New dsUnitCountOutBound.tblUnitCountOutboundDataTable
                '        daOutBound.Fill(dtOutBound, gdrSessCtl.DefaultPkgLine)
                '        If dtOutBound.Rows.Count > 0 Then
                '            gstrLastOutputLocation = dtOutBound.Rows(0).Item("OutputLocation")
                '            gintLastDestinationShopOrder = dtOutBound.Rows(0).Item("DestinationShopOrder")
                '        End If
                '    Catch ex As SqlClient.SqlException When gblnSvrConnIsUp = True
                '        SharedFunctions.SetServerCnnStatusInSessCtl(False)
                '        SharedFunctions.AddMessageLineToForm(Form.ActiveForm, gcstSvrCnnFailure)
                '    End Try
                'End If
                'WO#5370 ADD Stop
                Exit Sub
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub popupNumKeyPad(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtShift.MouseDown, txtOperator.MouseDown
        Dim dgrKeyPad As DialogResult
        Try

            dgrKeyPad = SharedFunctions.PopNumKeyPad(Me, sender)
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
    Private Sub popupAlphaNumKB(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtPkgLine.MouseDown
        Dim dgrKeyPad As DialogResult
        Try
            dgrKeyPad = SharedFunctions.PopAlphaNumKB(Me, sender)
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

    Private Sub btnMainMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMainMenu.Click
        'WO#871 Dim strMyComputerName As String = My.Computer.Name
        If InputDataOK() Then
            'SharedFunctions.ImportMasterTables(strMyComputerName)
            SharedFunctions.ProcessLogOn(txtPkgLine.Text, txtOperator.Text, txtShift.Text, gshtCurrentShift)
            'WO#650 SharedFunctions.RefreshComputerConfig()
            'WO#871 SharedFunctions.RefreshComputerConfig(strMyComputerName)    'WO#650
            'WO#5370 SharedFunctions.RefreshComputerConfig(gstrMyComputerName)           'WO#871
            SharedFunctions.RefreshComputerConfig(gstrMyComputerName, txtPkgLine.Text)   'WO#5370 
            frmMainMenu.ShowDialog()
            refreshShiftInfo()
        End If
    End Sub
    Private Function InputDataOK() As Boolean
        InputDataOK = True
        If Me.txtPkgLine.Text = "" Then
            MessageBox.Show("Please enter a Packaging Line.", "Missing information")
            Me.txtPkgLine.Focus()
            InputDataOK = False
        ElseIf IsValidPkgLine() = False Then
            InputDataOK = False
        ElseIf Me.txtShift.Text = "" Then
            MessageBox.Show("Please enter a Shift No.", "Missing information")
            Me.txtShift.Focus()
            InputDataOK = False
        ElseIf IsValidOperator() = False Then
            InputDataOK = False
        ElseIf Me.txtOperator.Text = "" Then
            MessageBox.Show("Please enter a Operator ID.", "Missing information")
            Me.txtOperator.Focus()
            InputDataOK = False
        End If
    End Function

    Private Sub txtOperator_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOperator.TextChanged
        'Dim strOperatorName As String      'WO#755
        Try
            IsValidOperator()               'WO#755
            'WO#755 DEL Start
            'gstrErrMsg = Nothing
            'If txtOperator.Text <> "" Then
            '    lblOperator.Visible = False
            '    strOperatorName = SharedFunctions.GetStaffName(sender.text, "P")
            '    If strOperatorName <> "" Then
            '        lblOperator.Visible = True
            '        lblOperator.Text = strOperatorName
            '        If txtOperator.Text <> gdrSessCtl.Operator And gdrSessCtl.ShopOrder <> 0 Then
            '            gstrErrMsg = String.Format("{0} started the shop order {1} has not been stopped. Please stop it with his ID {2} before start yours.", _
            '                    gdrSessCtl.OperatorName, gdrSessCtl.ShopOrder, gdrSessCtl.Operator)
            '            MessageBox.Show(gstrErrMsg, "Warning - Shop Order is still left open.", MessageBoxButtons.OK)
            '            sender.focus()
            '        End If
            '    Else
            '        gstrErrMsg = "Please enter a valid Opeartor ID."
            '        MessageBox.Show(gstrErrMsg, "Invalid information")
            '        sender.focus()
            '    End If
            'End If
            'WO#755 DEL Stop

        Catch ex As Exception
            Throw New Exception("Error in txtOperator_TextChanged" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub txtOperator_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtOperator.Validating
        Dim drResponse As DialogResult
        If Me.ActiveControl.Name = "btnExit" Then
            drResponse = MessageBox.Show("Are you sure to exit the application to restart the computer?", "Exit application", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If drResponse = Windows.Forms.DialogResult.Yes Then
                'FX150505   Me.Close()
                frmShutDown.ShowDialog()    'FX150505
            Else                            'WO#2645
                txtOperator.Focus()         'WO#2645
            End If
        Else
            If Not IsNothing(gstrErrMsg) Then
                MessageBox.Show(gstrErrMsg, "Invalid information")
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub txtShift_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtShift.TextChanged
        Dim intEnteredShift As Integer                                              'WO#3695
        Dim objShift As New WorkShift                                                'WO#3695
        Try
            gstrErrMsg = Nothing

            If txtShift.Text <> "" Then
                lblShift.Text = ""                                                      'WO#3695
                intEnteredShift = CType(txtShift.Text, Integer)                         'WO#3695
                'WO#2645 If CType(txtShift.Text, Integer) < 1 Or CType(txtShift.Text, Integer) > 4 Then
                'WO#2645 Add Start
                ldaShift.Fill(ldtShift, "AllShifts", gdrSessCtl.Facility, gdrCmpCfg.WorkShiftType, Now(), Nothing, 1)
                If ldtShift.Rows.Count = 0 Then
                    gstrErrMsg = "The pre-defined shift information is missing, please contact supervisor."
                    MessageBox.Show(gstrErrMsg, "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtShift.Focus()
                ElseIf CType(txtShift.Text, Integer) < 1 Or CType(txtShift.Text, Integer) > ldtShift.Rows.Count Then
                    gstrErrMsg = "Please entered a valid shift no. from 1 to " & CType(ldtShift.Rows.Count, String) & "."
                    'WO#2645 Add Stop
                    'WO#2645    gstrErrMsg = "Please entered a valid shift no. 1 to 4."
                    MessageBox.Show(gstrErrMsg, "Invalid Information")
                    txtShift.Focus()
                End If          'WO#3695
                'WO#3695 ElseIf txtShift.Text <> gshtCurrentShift Then
                'WO#3695 ADD Start
                If gstrErrMsg Is Nothing Then
                    objShift.GetShiftInfoByShiftNo(CType(txtShift.Text, Integer), Now(), gdrCmpCfg("WorkShiftType"), True)
                    If IsNothing(objShift.Description) = False Then
                        lblShift.Text = objShift.Description
                    End If

                    If ldtShift.Rows(0).Item("Method") = ShiftMethod.sequential Then
                        If txtShift.Text <> gshtCurrentShift Then
                            'WO#3695 ADD Stop
                            'Do not assign any values to gstrErrMsg, so that the message works as a warning.
                            MessageBox.Show("Entered shift no. is not the current shift.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Question)
                            txtShift.Focus()
                            'WO#3695 ADD Start
                        End If
                    Else
                        If ldtShift.Rows(0).Item("Method") = ShiftMethod.PatternCode Then
                            gstrErrMsg = objShift.IsEnteredShiftValid(intEnteredShift, Now(), gdrCmpCfg.WorkShiftType, gshtCurrentShift)
                            If Not IsNothing(gstrErrMsg) Then
                                MessageBox.Show(gstrErrMsg, "Invalid Information")
                                txtShift.Focus()
                            ElseIf txtShift.Text <> gshtCurrentShift Then
                                MessageBox.Show("Entered shift no. is not the current shift.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Question)
                                txtShift.Focus()
                            End If
                        End If
                        'WO#3695 ADD Stop
                    End If
                    'WO#2645    Dim myshift As New WorkShift(CType(txtShift.Text, Integer), Now(), gdrCmpCfg("WorkShiftType"))
                    'WO#3695 DEL Start
                    'Dim myShift As New WorkShift                                                                            'WO#2645
                    'myShift.GetShiftInfoByShiftNo(CType(txtShift.Text, Integer), Now(), gdrCmpCfg("WorkShiftType"), True)   'WO#2645
                    'If IsNothing(myShift.Description) = False Then                                                          'WO#2645
                    '    'WO#2645    If IsNothing(myShift) = False Then
                    '    lblShift.Text = myShift.Description
                    'Else                                                                            'WO#2645
                    '    lblShift.Text = ""                                                          'WO#2645
                    'End If
                    'WO#3695 DEL Stop

                End If 'WO#3695
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtShift_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtShift.Validating
        'WO#2645 Add Start
        Dim drResponse As DialogResult
        If Me.ActiveControl.Name = "btnExit" Then
            drResponse = MessageBox.Show("Are you sure to exit the application to restart the computer?", "Exit application", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If drResponse = Windows.Forms.DialogResult.Yes Then
                frmShutDown.ShowDialog()
            Else
                txtShift.Focus()
            End If
        Else
            'WO#2645 Add Stop
            If Not IsNothing(gstrErrMsg) Then
                MessageBox.Show(gstrErrMsg, "Invalid information")
                e.Cancel = True
            End If
        End If      'WO#2645
    End Sub

    Private Sub txtPkgLine_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPkgLine.TextChanged
        Try

            IsValidPkgLine()            'WO#755
            'WO#755 DEL Stop
            '        Dim tblEQ As New dsEquipment.CPPsp_EquipmentIODataTable

            '        'WO#359 Dim intRtnCde As Integer
            '        gstrErrMsg = Nothing
            '        'Retrieve equipment record using the packaging line 
            '        lblPkgLine.Visible = False
            '        If txtPkgLine.Text <> "" Then
            '            lblPkgLine.Text = String.Empty
            '            lblPkgLine.Text = SharedFunctions.GetEquipmentDescription(gdrCmpCfg("Facility"), sender.text)
            '            gtaEQ.Fill(tblEQ, gdrCmpCfg("Facility"), sender.text, "P", "")
            '            If lblPkgLine.Text = String.Empty Then
            '                gstrErrMsg = "Please enter a valid Packaging Line."
            '                MessageBox.Show(gstrErrMsg, "Invalid information")
            '                txtPkgLine.Focus()
            '            Else
            '                lblPkgLine.Visible = True
            '                'lblPkgLine.Text = tblEQ.Rows(0)("Description")
            '            End If
            '        End If
            'WO#755 DEL Stop
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtPkgLine_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtPkgLine.Validating
        Dim drResponse As DialogResult

        If Me.ActiveControl.Name = "btnExit" Then
            drResponse = MessageBox.Show("Are you sure to exit the application to restart the computer?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
            If drResponse = Windows.Forms.DialogResult.Yes Then
                'FX150505   Me.Close()
                frmShutDown.ShowDialog()    'FX150505
            Else                            'WO#2645
                txtPkgLine.Focus()          'WO#2645
            End If
        Else
            If Not IsNothing(gstrErrMsg) Then
                MessageBox.Show(gstrErrMsg, "Invalid information")
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub WindowsShutDown()
        Dim ProcID As Integer
        ' Start the Windows ShutDwon, and store the process id.
        ProcID = Shell("Shutdown", AppWinStyle.NormalFocus)
        ' Activate the shutdown.
        AppActivate(ProcID)
    End Sub
    Private Sub refreshShiftInfo()
        'Dim drShift As DataRow
        'drShift = SharedFunctions.getShiftDetail(Now, gdrCmpCfg("WorkShiftTYpe"))
        'If Not IsNothing(drShift) Then
        Try
            'WO#2645 Dim myShift As New WorkShift(Now, gdrCmpCfg("WorkShiftTYpe"))
            'WO#2645    If IsNothing(myShift) = False Then
            Dim myShift As New WorkShift                                                    'WO#2645
            myShift.GetExpectedShiftInfoByTime(Now(), gdrCmpCfg("WorkShiftType"), True)     'WO#2645
            If Not IsNothing(myShift.Shift) Then                                            'WO#2645
                'gshtCurrentShift = drShift("Shift")
                gshtCurrentShift = myShift.Shift
                Me.txtShift.Text = gshtCurrentShift
                'lblShift.Text = drShift("Description")
                lblShift.Text = myShift.Description
                lblShift.Visible = True
            Else
                lblShift.Visible = False
            End If
        Catch ex As Exception
            Throw New Exception("Error in refreshShiftInfo" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Dim drResponse As DialogResult
        drResponse = MessageBox.Show("Are you sure to exit the application to restart the computer?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If drResponse = Windows.Forms.DialogResult.Yes Then
            'FX150505   Me.Close()
            frmShutDown.ShowDialog()    'FX150505
        End If
    End Sub

    Private Sub btnTSCtlPnl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTSCtlPnl.Click
        Try
            SharedFunctions.ShowTSCtlPnl()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'WO#755 ADD Start
    Private Function IsValidPkgLine() As Boolean
        Try
            Dim tblEQ As New dsEquipment.CPPsp_EquipmentIODataTable

            gstrErrMsg = Nothing
            'Retrieve equipment record using the packaging line 
            lblPkgLine.Visible = False
            If txtPkgLine.Text <> "" Then
                lblPkgLine.Text = String.Empty
                lblPkgLine.Text = SharedFunctions.GetEquipmentDescription(gdrCmpCfg("Facility"), txtPkgLine.Text)
                gtaEQ.Fill(tblEQ, gdrCmpCfg("Facility"), txtPkgLine.Text, "P", "", Nothing)
                If lblPkgLine.Text = String.Empty Then
                    gstrErrMsg = "Please enter a valid Packaging Line."
                    MessageBox.Show(gstrErrMsg, "Invalid information")
                    txtPkgLine.Focus()
                    Return False
                Else
                    lblPkgLine.Visible = True
                    Return True
                End If
            End If

        Catch ex As Exception
            Throw New Exception("Error in IsValidPkgLine" & vbCrLf & ex.Message)
        End Try
    End Function

    Private Function IsValidOperator() As Boolean
        Dim strOperatorName As String
        Try
            gstrErrMsg = Nothing
            If txtOperator.Text <> "" Then
                lblOperator.Visible = False
                strOperatorName = SharedFunctions.GetStaffName(txtOperator.Text, "P")
                If strOperatorName <> "" Then
                    lblOperator.Visible = True
                    lblOperator.Text = strOperatorName

                    If txtOperator.Text <> gdrSessCtl.Operator And gdrSessCtl.ShopOrder <> 0 Then
                        gstrErrMsg = String.Format("{0} started the shop order {1} has not been stopped. Please stop it with his ID {2} before start yours.", _
                                gdrSessCtl.OperatorName, gdrSessCtl.ShopOrder, gdrSessCtl.Operator)
                        MessageBox.Show(gstrErrMsg, "Warning - Shop Order is still left open.", MessageBoxButtons.OK)
                        txtOperator.Focus()
                        'WO#755 ADD Start
                    ElseIf strOperatorName = "Deactivated" Then
                        gstrErrMsg = "Please enter an active Opeartor ID."
                        MessageBox.Show(gstrErrMsg, "Invalid information")
                        txtOperator.Focus()
                        Return False
                        'WO#755 ADD End
                    End If

                    Return True
                Else
                        gstrErrMsg = "Please enter a valid Opeartor ID."
                        MessageBox.Show(gstrErrMsg, "Invalid information")
                        txtOperator.Focus()
                        Return False
                End If
            End If
        Catch ex As Exception
            Throw New Exception("Error in IsValidOperator" & vbCrLf & ex.Message)
        End Try
    End Function
    'WO#755 ADD Stop
End Class