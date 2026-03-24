Public Class frmQATStartUpChecks
    Dim intReadingCount As Int16
    Dim dteJobStartTime As DateTime = Now
    Dim dteJobEndTime As DateTime
    Dim drWF As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow
    Dim drQATTask As dsQATTask.CPPsp_QATTask_SelRow
    Dim strCurrQATEntryPoint As String
    Dim intCurrentTaskID As Int16
    Dim dteTaskStartTime As DateTime
    Dim btnCurrent As Button
    ' WO#17432 ADD Start – AT 9/26/2018
    Dim intTaskCount As Int16
    Dim dteTaskEndTime As DateTime
    Const cstrDone = "Done"
    Const cstrNotDone = "Not Done"
    Const cstrNA = "N/A"
    Const cstrNext = "Next"
    Const cstrByPassAll = "ByPassAll"
    Const cstrStart = "Start"
    Dim blnDisableByPass As Boolean
    ' WO#17432 ADD Stop – AT 9/26/2018
    Dim intClosedShopOrder As Integer
    Dim colOriginal As System.Drawing.Color = Color.White
    Dim dtTaskStatus As New DataTable           'WO#17432 AT 12/21/2018
    Dim btnTasks As Button()

    Private Sub frmQATStartUpChecks_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            CreateTaskStatusTable()             'WO#17432 AT 12/21/2018

            'Is Start-Up or In-Process or Closing test?
            strCurrQATEntryPoint = SharedFunctions.FindCurrQATEntryPoint(gdrSessCtl.ShopOrder, gdrSessCtl.DefaultPkgLine)
            If strCurrQATEntryPoint = cstrClosing Then
                Dim drQATStatus As dsQATStatus.tblQATStatusRow = SharedFunctions.GetQATStatus()
                intClosedShopOrder = drQATStatus.ShopOrder
            End If

            'Retrieve QAT work flow and test information
            drWF = SharedFunctions.GetQATWorkFlowInfo(gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, strCurrQATEntryPoint, Me.Name)

            If Not IsNothing(drWF) Then             ' WO#17432 – AT 11/29/2018
                'Set form title
                If drWF.TestFormTitle <> "" Then
                    UcHeading1.ScreenTitle = drWF.TestFormTitle
                    Me.Text = drWF.TestFormTitle
                Else
                    If strCurrQATEntryPoint = cstrChangeShift Then
                        UcHeading1.ScreenTitle = "Change-Shift Checks"
                    End If
                End If

                ' WO#17432 ADD Start – AT 9/26/2018
                ' initialize task counter, job and task time  
                intTaskCount = 0
                dteJobStartTime = Now()
                dteJobEndTime = Now()
                dteTaskEndTime = Now()
                blnDisableByPass = False
                ' WO#17432 ADD Stop – AT 9/26/2018

                'Initiaize the form to display different no. of test boxes for the different tests.
                If InitializeScreen(gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, drWF.QATDefnID, strCurrQATEntryPoint) Then

                    'initialize the sampling counter
                    intReadingCount = 1

                    'Get the test Batch ID
                    gdteTestBatchID = SharedFunctions.GetQATBatchID(drWF.TestSeq, strCurrQATEntryPoint)
                    ' WO#17432 ADD Start – AT 12/03/2018
                    If SharedFunctions.QATIsTested(drWF.FormName, gdteTestBatchID) = True Then
                        MsgBox("The test has already done in the same QAT workflow batch.")
                        CloseForm()
                        Exit Sub
                    End If
                    ' WO#17432 ADD Stop – AT 12/03/2018

                    dteTaskStartTime = Now()
                    ' WO#17432 ADD Stop – AT 11/29/2018

                    frmQATTester.ShowDialog()   ' WO#17432 – BL 03/28/2019
                Else
                    CloseForm()
                    Exit Sub
                End If
            Else
                MessageBox.Show("Cannot find the QAT workflow information. Please contact Supervisor.", "Error - Missing workflow information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                CloseForm()
                Exit Sub
                ' WO#17432 ADD Stop – AT 11/29/2018
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CloseForm()
        End Try
    End Sub

    ' WO#17432 ADD Start – AT 12/21/2018
    Private Sub CreateTaskStatusTable()
        Try
            dtTaskStatus = New DataTable("TaskStatus")
            Dim colTaskID As DataColumn = New DataColumn("colTaskID")
            colTaskID.DataType = System.Type.GetType("System.Int32")
            Dim colTaskStatus As DataColumn = New DataColumn("colTaskStatus")
            colTaskStatus.DataType = System.Type.GetType("System.Int32")
            Dim colByPassAllTest As DataColumn = New DataColumn("colByPassAllTest")
            colByPassAllTest.DataType = System.Type.GetType("System.Int32")
            Dim colTaskStartTime As DataColumn = New DataColumn("colTaskStartTime")
            colTaskStartTime.DataType = System.Type.GetType("System.DateTime")
            Dim colTaskEndTime As DataColumn = New DataColumn("colTaskEndTime")
            colTaskEndTime.DataType = System.Type.GetType("System.DateTime")

            dtTaskStatus.Columns.Add(colTaskID)
            dtTaskStatus.Columns.Add(colTaskStatus)
            dtTaskStatus.Columns.Add(colByPassAllTest)
            dtTaskStatus.Columns.Add(colTaskStartTime)
            dtTaskStatus.Columns.Add(colTaskEndTime)

            Dim primaryKey(1) As DataColumn
            primaryKey(0) = dtTaskStatus.Columns("colTaskID")
            dtTaskStatus.PrimaryKey = primaryKey

        Catch ex As Exception
            Throw New Exception("Error in CreateTaskStatusTable" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Function IsCurrentTaskExist(ByVal intTaskID As Integer) As Boolean
        Try
            For Each row As DataRow In dtTaskStatus.Rows
                If intTaskID = row.Field(Of Integer)(0) Then
                    Return True
                End If
            Next
            Return False
        Catch ex As Exception
            Throw New Exception("Error in IsCurrentTaskExist" & vbCrLf & ex.Message)
        End Try
    End Function

    Private Sub UpdateCurrentTaskStatus(ByVal intByPassAllTest As Integer, ByVal intTaskStatus As Integer, ByVal intTaskID As Integer)
        Try
            If IsCurrentTaskExist(intTaskID) Then
                Dim dr1() As Data.DataRow
                dr1 = dtTaskStatus.Select("colTaskID = " & intTaskID)
                dr1(0)("colTaskStatus") = intTaskStatus
                dr1(0)("colByPassAllTest") = intByPassAllTest
            Else
                Dim dr As DataRow
                dr = dtTaskStatus.NewRow()
                dr("colTaskID") = intTaskID
                dr("colTaskStatus") = intTaskStatus
                dr("colByPassAllTest") = intByPassAllTest
                dr("colTaskStartTime") = dteTaskEndTime
                dr("colTaskEndTime") = dteTaskStartTime
                dtTaskStatus.Rows.Add(dr)
            End If
        Catch ex As Exception
            Throw New Exception("Error in UpdateCurrentTaskStatus" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub UpdateStartUpChecks()
        Dim intShopOrder As Integer
        Try
            If strCurrQATEntryPoint = cstrClosing Then
                intShopOrder = intClosedShopOrder
            Else
                intShopOrder = gdrSessCtl.ShopOrder
            End If
            For Each row As DataRow In dtTaskStatus.Rows
                If row.Field(Of Integer)(1) <> 1 Then
                    SharedFunctions.SaveQATStartUpChecks(gdteTestBatchID, row.Field(Of Integer)(2), gdrSessCtl.Facility, gstrInterfaceID, gdrSessCtl.DefaultPkgLine, _
                        gstrQATWorkFlowType, intShopOrder, gdrSessCtl.StartTime, dteJobEndTime, dteJobStartTime, row.Field(Of DateTime)(4), row.Field(Of Integer)(0), _
                        row.Field(Of DateTime)(3), row.Field(Of Integer)(1), gstrQATTesterID)
                End If
            Next

        Catch ex As Exception
            Throw New Exception("Error in UpdateStartUpChecks" & vbCrLf & ex.Message)
        End Try
    End Sub
    ' WO#17432 ADD Stop– AT 12/21/2018

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Dim intShopOrder As Integer
        'loop thought all the task status buttons to update the Not Done to the table.
        Try
            ' WO#17432 ADD Start – AT 9/26/2018
            ' Update header and detail for Not Done task 
            dteJobEndTime = Now
            dteTaskStartTime = dteTaskEndTime
            UpdateStartUpChecks()                       'WO#17432 AT 12/21/2018
            SaveTaskStatus(cstrNext)
            ' WO#17432 ADD Stop – AT 9/26/2018

            'If it is run at closing shop order, use the shop order number from the QAT Status table when update testing results"
            'Up date QAT Status record
            'Update QAT processing status
            If strCurrQATEntryPoint = cstrClosing Then
                intShopOrder = intClosedShopOrder
            Else
                intShopOrder = gdrSessCtl.ShopOrder
            End If

            SharedFunctions.UpdateQATStatus(False, False, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, drWF.QATDefnID, gstrInterfaceID, drWF.TestSeq)
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnTask_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim btnStatus As Button
        Try

            btnCurrent = CType(sender, Button)
            btnStatus = CType(Controls.Item("btnStatus_" & btnCurrent.Tag), Button)
            ' WO#17432 ADD Start – AT 12/21/2018
            'txtTaskDesc.Text = sender.text & ":"
            txtTaskDesc.Text = "Check No.: " & (CInt(btnCurrent.Tag) + 1).ToString
            txtTaskDesc.Tag = btnCurrent.Tag
            'btnTaskDesc.Text = sender.text & ":"
            'btnTaskDesc.Tag = btnCurrent.Tag
            intCurrentTaskID = btnStatus.Tag

            'Reset the backgrond color of the task no. buttons
            For Each btn As Button In btnTasks
                btn.BackColor = colOriginal
            Next
            btnCurrent.BackColor = Color.Yellow

            Select Case btnStatus.Text
                Case cstrNotDone
                    Panel1.Visible = True
                    'txtTaskDesc.Text = sender.text & ":"
                    'txtTaskDesc.Tag = btnCurrent.Tag
                    'intCurrentTaskID = btnStatus.Tag
                    btnOption1.Text = "N/A"
                    btnOption2.Text = "Done"
                    btnOption1.BackColor = Color.LightGoldenrodYellow
                    btnOption2.BackColor = Color.LawnGreen
                Case cstrDone
                    Panel1.Visible = True
                    'txtTaskDesc.Text = sender.text & ":"
                    'txtTaskDesc.Tag = btnCurrent.Tag
                    'intCurrentTaskID = btnStatus.Tag
                    btnOption1.Text = "N/A"
                    btnOption2.Text = "Not Done"
                    btnOption1.BackColor = Color.LightGoldenrodYellow
                    btnOption2.BackColor = Color.Red
                Case cstrNA
                    Panel1.Visible = True
                    'txtTaskDesc.Text = sender.text & ":"
                    'txtTaskDesc.Tag = btnCurrent.Tag
                    'intCurrentTaskID = btnStatus.Tag
                    btnOption1.Text = "Not Done"
                    btnOption2.Text = "Done"
                    btnOption1.BackColor = Color.Red
                    btnOption2.BackColor = Color.LawnGreen
                Case Else
            End Select
            dteTaskStartTime = GetTaskStartEndTime(sender.text, cstrStart)
            ' WO#17432 ADD Stop – AT 12/21/2018
        Catch ex As Exception
            Throw New Exception("Error in btnTask_click" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub btnSelectedStatus_Click(sender As Object, e As EventArgs) Handles btnOption1.Click, btnOption2.Click
        Dim intTaskStatus As Integer = 0    ' WO#17432 – AT 9/26/2018
        Dim btnStatus As Button
        Try
            btnCurrent.BackColor = colOriginal
            btnStatus = CType(Controls.Item("btnStatus_" & txtTaskDesc.Tag), Button)
            With btnStatus
                .Text = sender.Text
                .BackColor = sender.backcolor
            End With

            Panel1.Visible = False

            ' WO#17432 ADD Start – AT 9/26/2018
            ' Get task end time
            ' Update header and detail for Done task
            intTaskStatus = GetCurrentTaskStatus(sender.text)
            dteTaskEndTime = GetTaskStartEndTime(sender.text, cstrDone)
            'WO#17432 AT 12/21/2018 SaveStartUpChecks(0, intTaskStatus, intCurrentTaskID)
            UpdateCurrentTaskStatus(0, intTaskStatus, intCurrentTaskID)             'WO#17432 AT 12/21/2018
            If blnDisableByPass = False Then
                ' Disable ByPass All Test button when any one of task is selected
                DisableByPassAllTest()
                blnDisableByPass = True
            End If
            ' WO#17432 ADD Stop – AT 9/26/2018

        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnByPassAll_Click()
        Dim intShopOrder As Integer
        ' WO#17432 ADD Start – AT 9/26/2018
        ' Update header and detail for all task 
        Try
            dteJobEndTime = Now
            dteTaskStartTime = dteTaskEndTime
            SaveTaskStatus(cstrByPassAll)

            ' WO#17432 ADD Stop – AT 9/26/2018

            'Update QAT processing status
            If strCurrQATEntryPoint = cstrClosing Then
                intShopOrder = intClosedShopOrder
            Else
                intShopOrder = gdrSessCtl.ShopOrder
            End If
            UpdateStartUpChecks()                   'WO#17432 AT 12/21/2018
            SharedFunctions.UpdateQATStatus(True, False, gdrSessCtl.ShopOrder, strCurrQATEntryPoint, drWF.QATDefnID, gstrInterfaceID, drWF.TestSeq)
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        Catch ex As Exception
            Throw New Exception("Error in btnByPassAll" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Function InitializeScreen(strFacility As String, strPackagingLine As String, intQATDefnID As Integer, strQATEntryPoint As String) As Boolean

        Dim btnStatuses As Button()
        Dim btnTask As Button
        Dim btnStatus As Button
        Dim btnByPassAll As Button

        'Task descripton buttons
        Dim intBtnX As Int16, intBtnY As Int16
        Dim intBtnWidth As Int16 = 250, intBtnHeight As Int32 = 63
        Dim intIntialBtnX As Int16 = 27, intIntialBtnY As Int16 = 70
        Dim intBtnXGap As Int16 = 393               'intBtnXGap = x-location of the 3rd button(420) - x-location of the 1st button(27)
        Dim intBtnYGap As Int16 = 69                'intBtnYGap = y-location of the 1st button(139) - y-location of 2nd row button(70)

        'Task status buttons
        Dim intBtnStatusX As Int16, intBtnStatusY As Int16
        Dim intBtnStatusWidth As Int16 = 111, intBtnStatusHeight As Int16 = 63
        Dim intIntialBtnStatusX As Int16 = 283, intIntialBtnStatusY As Int16 = 70
        Dim intBtnStatusXGap As Int16 = 393         'x-location of the 2rd button(676) - x-location of the 4th button(283)
        Dim intBtnStatusYGap As Int16 = 69          'y-location of the 1st button(139) - y-location of 2nd row button(70)

        Dim intNoOfTasks As Int16, intTaskNo As Int16
        Dim intNoOfRows As Int16, intRowNo As Int16, intColNo As Int16

        Const intMaxRows As Int16 = 6

        Dim drQATTask As dsQATTask.CPPsp_QATTask_SelRow

        Try
            InitializeScreen = True
            Using taQATTask As New dsQATTaskTableAdapters.CPPsp_QATTask_SelTableAdapter
                Using dtQATTask As New dsQATTask.CPPsp_QATTask_SelDataTable
                    taQATTask.Fill(dtQATTask, strFacility, strPackagingLine, intQATDefnID, Nothing, True, Nothing)
                    If dtQATTask.Count > 0 Then
                        intNoOfTasks = dtQATTask.Count

                        'Find out no. of rows of tasks will be displayed.
                        If intNoOfTasks <= intMaxRows Then
                            intNoOfRows = intNoOfTasks
                        Else
                            intNoOfRows = intMaxRows
                        End If

                        ReDim btnTasks(intNoOfTasks - 1)
                        ReDim btnStatuses(intNoOfTasks - 1)

                        intRowNo = 0
                        intColNo = 0
                        For intTaskNo = 0 To intNoOfTasks - 1
                            drQATTask = dtQATTask.Rows(intTaskNo)                   'Read next task

                            'Create button control to display the description of a task and allow to click for updating the task status.
                            'Tag  contains just the button creation sequance no.. 
                            'Name contains "btnTask_" and the sequance no of this loop, such as btntask_1.
                            'Text contains the description of the task from table
                            btnTask = New Button
                            With btnTask
                                intBtnX = intIntialBtnX + intColNo * intBtnXGap
                                intBtnY = intIntialBtnY + intRowNo * intBtnYGap
                                .Location = New System.Drawing.Point(intBtnX, intBtnY)
                                .Text = (intTaskNo + 1).ToString & ". " & drQATTask.TaskDescription
                                .Tag = intTaskNo.ToString
                                .Size = New System.Drawing.Size(intBtnWidth, intBtnHeight)
                                .Name = "btnTask_" & intTaskNo.ToString
                                .ForeColor = Color.Black
                                .BackColor = colOriginal
                                '.Font = New Font("Arial", 18, System.Drawing.FontStyle.Bold)
                                .Font = New Font("Arial", 16)
                                .FlatAppearance.BorderSize = 1
                                .FlatAppearance.BorderColor = Color.White
                                .FlatStyle = FlatStyle.Standard
                                .TextAlign = ContentAlignment.MiddleLeft
                            End With

                            'Add the button control to the screen and the click event to each button.
                            btnTasks(intTaskNo) = btnTask
                            AddHandler btnTasks(intTaskNo).Click, AddressOf btnTask_click
                            Me.Controls.Add(btnTask)

                            'Create buttons for showing the status of the tasks. 
                            'Tag contains the actual task id from the table.
                            'Text contains the status of the task
                            'Name contains "btnStatus_" and the sequance no of this loop, such as "btnStatus_1"
                            btnStatus = New Button
                            With btnStatus
                                intBtnStatusX = intIntialBtnStatusX + intColNo * intBtnStatusXGap
                                intBtnStatusY = intIntialBtnStatusY + intRowNo * intBtnStatusYGap
                                .Location = New System.Drawing.Point(intBtnStatusX, intBtnStatusY)
                                ' WO#17432 ADD Start – AT 9/26/2018
                                .Text = cstrNotDone
                                ' WO#17432 ADD Stop – AT 9/26/2018
                                .Tag = drQATTask.TaskID
                                .Size = New System.Drawing.Size(intBtnStatusWidth, intBtnStatusHeight)
                                .Name = "btnStatus_" & intTaskNo.ToString
                                .ForeColor = Color.Black
                                .BackColor = Color.Red
                                .Font = New Font("Arial", 18, System.Drawing.FontStyle.Bold)
                                .FlatAppearance.BorderSize = 1
                                .FlatAppearance.BorderColor = Color.White
                                .FlatStyle = FlatStyle.Standard
                            End With

                            btnStatuses(intTaskNo) = btnStatus
                            Me.Controls.Add(btnStatus)

                            intRowNo = intRowNo + 1
                            If intRowNo = intMaxRows Then
                                intRowNo = 0
                                intColNo = intColNo + 1
                            End If

                        Next

                        ' WO#17432 ADD Start – AT 9/26/2018
                        ' By Pass All Test button is not created
                        blnDisableByPass = True
                        ' WO#17432 ADD Stop – AT 9/26/2018

                        'if the sequence of the test is 1 , then show the ByPass All Tests button 
                        If drWF.TestSeq = 1 And drWF.QATEntryPoint = cstrStartup Then
                            btnByPassAll = New Button
                            With btnByPassAll
                                intBtnX = 185
                                intBtnY = 490
                                .Location = New System.Drawing.Point(intBtnX, intBtnY)
                                .Text = "ByPass All Test"
                                intBtnWidth = 150
                                intBtnHeight = 65
                                .Size = New System.Drawing.Size(intBtnWidth, intBtnHeight)
                                .Name = "btnByPassAll"
                                .ForeColor = Color.Black
                                .BackColor = Color.Silver
                                .Font = New Font("Arial", 18, System.Drawing.FontStyle.Bold)
                            End With

                            AddHandler btnByPassAll.Click, AddressOf btnByPassAll_Click
                            Me.Controls.Add(btnByPassAll)

                            ' WO#17432 ADD Start – AT 9/26/2018
                            ' By Pass All Test button is created, and ready to be disabled
                            blnDisableByPass = False
                            ' WO#17432 ADD Stop – AT 9/26/2018
                        End If
                    Else
                        MessageBox.Show("Cannot find the QAT start-up tasks for this line. Please contact QA.", "Error - Missing QAT information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        InitializeScreen = False
                    End If
                End Using
            End Using

        Catch ex As Exception
            Throw New Exception("Error in InitializeScreen." & vbCrLf & ex.Message)
        End Try
    End Function
    ' WO#17432 ADD Start – AT 9/26/2018
    Private Sub SaveStartUpChecks(ByVal intByPassAllTest As Integer, ByVal intTaskStatus As Integer, ByVal intTaskID As Integer)
        Dim intShopOrder As Integer
        Try
            If strCurrQATEntryPoint = cstrClosing Then
                intShopOrder = intClosedShopOrder
            Else
                intShopOrder = gdrSessCtl.ShopOrder
            End If
            ' Update start up check header and detail
            SharedFunctions.SaveQATStartUpChecks(gdteTestBatchID, intByPassAllTest, gdrSessCtl.Facility, gstrInterfaceID, gdrSessCtl.DefaultPkgLine, gstrQATWorkFlowType, _
                 intShopOrder, gdrSessCtl.StartTime, dteJobEndTime, dteJobStartTime, dteTaskEndTime, intTaskID, dteTaskStartTime, intTaskStatus, gstrQATTesterID)
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DisableByPassAllTest()
        ' Disable By Pass All Test button when user select any task  
        Dim btnByPass As Button
        btnByPass = CType(Controls.Item("btnBypassAllTests_"), Button)
        btnByPass.Enabled = False
        btnByPass.Visible = False

        Dim btnByPass2 As Button
        btnByPass2 = CType(Controls.Item("btnByPassAll"), Button)
        btnByPass2.Enabled = False
        btnByPass2.Visible = False
    End Sub

    Private Function GetCurrentTaskStatus(ByVal strSenderText As String) As Integer
        ' Get task status
        Dim intTaskStatus As Integer = 0
        Select Case strSenderText
            Case cstrNA
                intTaskStatus = 0
            Case cstrNotDone
                intTaskStatus = 1
            Case cstrDone
                intTaskStatus = 2
            Case Else
        End Select
        Return intTaskStatus
    End Function

    Private Function GetTaskStartEndTime(ByVal senderText As String, ByVal status As String)
        ' Get task start and end time
        Dim dt As DateTime
        Select Case status
            Case cstrStart
                If intTaskCount = 0 Then
                    dt = dteJobStartTime
                Else
                    dt = dteTaskEndTime
                End If
                intTaskCount = intTaskCount + 1
            Case cstrDone
                dt = Now
            Case Else
        End Select
        Return dt
    End Function

    Private Sub SaveTaskStatus(ByVal strTaskType As String)
        ' Find Task and Status button
        ' Save task detail when user click on Next and ByPassAllTask button
        Dim strSeq As String = String.Empty
        Dim strTaskName As String = String.Empty
        Dim strTaskStatus As String = String.Empty
        Dim intTaskID As Integer = 0
        Dim intTaskStatus As Integer = 0
        Dim strTaskIDStatus As String = String.Empty

        'Dim ctl As Control
        Dim btn As Button
        Try
            'Read the Task button and find the corresponding Status button by the button creation sequance 
            'to decode the status from Text property of the button to the corresponding integer.
            'For Each ctl In Me.Controls
            For Each btn In btnTasks
                ' If TypeOf ctl Is Button Then
                'If Microsoft.VisualBasic.Left(ctl.Name, 8) = "btnTask_" Then
                'strTaskName = ctl.Name
                strTaskName = btn.Name
                strSeq = Microsoft.VisualBasic.Right(strTaskName, strTaskName.Length - 8)
                strTaskIDStatus = GetTaskStatus(strSeq)

                If strTaskIDStatus.Contains("_") Then
                    strTaskStatus = Microsoft.VisualBasic.Left(strTaskIDStatus, strTaskIDStatus.IndexOf("_"))
                    intTaskStatus = GetCurrentTaskStatus(strTaskStatus)
                    intTaskID = Microsoft.VisualBasic.Right(strTaskIDStatus, strTaskIDStatus.Length - strTaskIDStatus.IndexOf("_") - 1)
                End If
                'If Next button was clicked, save all the Not Done tasks.
                If strTaskType = cstrNext Then
                    If strTaskStatus = cstrNotDone Then
                        SaveStartUpChecks(0, intTaskStatus, intTaskID)
                    End If
                Else
                    If strTaskIDStatus <> "" Then
                        SaveStartUpChecks(1, intTaskStatus, intTaskID)
                    End If
                End If
                'End If
                'End If
            Next
        Catch ex As Exception
            Throw New Exception("Error in SaveTaskStatus" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Function GetTaskStatus(ByVal strSeq As String) As String
        ' Find text and tag of status button
        Dim intSeq As Integer = 0
        Dim ctl As Control
        intSeq = strSeq.Length + 10
        Try
            For Each ctl In Me.Controls
                If TypeOf ctl Is Button Then
                    If Microsoft.VisualBasic.Left(ctl.Name, intSeq) = "btnStatus_" & strSeq Then
                        Return ctl.Text & "_" & ctl.Tag
                        Exit Function
                    End If
                End If
            Next
            Return ""
        Catch ex As Exception
            Throw New Exception("Error in GetTaskStatus" & vbCrLf & ex.Message)
        End Try
    End Function
    ' WO#17432 ADD Stop - AT 9/26/2018

    ' WO#17432 ADD Start – AT 11/29/2018
    Private Sub CloseForm()
        If gdrCmpCfg.QATWorkFlowInitiation = QATWorkFlow.External Then
            Me.Close()
        End If
    End Sub
    ' WO#17432 ADD Stop – AT 11/29/2018
End Class