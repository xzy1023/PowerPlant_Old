Imports System.Reflection

Public Class frmQATWorkflow

    Dim drWF As dsQATWorkFlow.CPPsp_QATWorkFlow_SelRow
    Dim dtWf As New dsQATWorkFlow.CPPsp_QATWorkFlow_SelDataTable
    Dim strScreenTitle As String = String.Empty
    Dim strQATEntryPoint As String = String.Empty

    Public Property QATEntryPoint As String
        Get
            Return strQATEntryPoint
        End Get
        Set(value As String)
            strQATEntryPoint = value
        End Set
    End Property

    Public Property ScreenTitle As String
        Get
            Return strScreenTitle
        End Get
        Set(value As String)
            strScreenTitle = value
        End Set
    End Property

    Private Sub frmQATWorkflow_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            UcHeading1.ScreenTitle = strScreenTitle
            Me.Text = strScreenTitle
            dtWf = SharedFunctions.GetQATWorkFlowInfo(gdrSessCtl.Facility, gdrSessCtl.DefaultPkgLine, strQATEntryPoint)
            If Not IsNothing(dtWf) Then
                gdteTestBatchID = Now()
                InitializeScreen()
            Else
                MessageBox.Show("Cannot find the QAT workflow. Please contact Supervisor.", "Error - Missing workflow inoformation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Close()
                Exit Sub
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub InitializeScreen()
        Dim btnQATs As Button()
        Dim btnQAT As Button

        Dim intBtnX As Int16, intBtnY As Int16
        Dim intBtnWidth As Int16 = 244, intBtnHeight As Int32 = 63
        Dim intIntialBtnX As Int16 = 27, intIntialBtnY As Int16 = 80
        Dim intBtnXGap As Int16 = 257, intBtnYGap As Int16 = 80

        Dim intNoOfQATs As Int16, intQATNo As Int16
        Dim intNoOfRows As Int16, intRowNo As Int16, intColNo As Int16
        Const intMaxRows As Int16 = 5
        Const intMaxCols As Int16 = 3
        Const cstrQATButtonPrefix As String = "btnQAT_"

        Try
            intNoOfQATs = dtWf.Count
            If intNoOfQATs = 0 Then
                Exit Sub
            Else
                ReDim btnQATs(intNoOfQATs)
            End If

            'Remove the buttons created before.
            For i As Integer = 1 To 10
                For Each ctrl As Control In Controls
                    If TypeOf ctrl Is Button AndAlso ctrl.Name.Length > cstrQATButtonPrefix.Length AndAlso _
                        ctrl.Name.Substring(0, 7) = cstrQATButtonPrefix Then
                        Me.Controls.Remove(ctrl)
                    End If
                Next
            Next

            For Each drWF In dtWf
                'Create button control to display the description of a QAT and allow to click for openning the QAT form.
                'Tag  contains form name of the QAT to be called when the button is clicked. 
                'Name contains "btnQAT_" and the sequance no of this loop, such as btnQAT_1.
                'Text contains the description of the QAT from table
                btnQAT = New Button
                With btnQAT
                    intBtnX = intIntialBtnX + intColNo * intBtnXGap
                    intBtnY = intIntialBtnY + intRowNo * intBtnYGap
                    .Location = New System.Drawing.Point(intBtnX, intBtnY)
                    .Text = drWF.TestCategory
                    .Tag = drWF.FormName
                    .Size = New System.Drawing.Size(intBtnWidth, intBtnHeight)
                    .Name = cstrQATButtonPrefix & intQATNo.ToString
                    .ForeColor = Color.Black
                    .BackColor = Color.LightGoldenrodYellow
                    .Font = New Font("Arial", 18, System.Drawing.FontStyle.Bold)
                    .FlatAppearance.BorderSize = 1
                    .FlatAppearance.BorderColor = Color.White
                    .FlatStyle = FlatStyle.Standard
                End With

                'Add the button control to the screen and the click event to each button.
                btnQATs(intQATNo) = btnQAT
                AddHandler btnQATs(intQATNo).Click, AddressOf btnQAT_click
                Me.Controls.Add(btnQAT)

                intQATNo = intQATNo + 1

                If intColNo = intMaxCols - 1 Then
                    intColNo = 0
                    intRowNo = intRowNo + 1
                    intNoOfRows = intNoOfRows + 1
                Else
                    intColNo = intColNo + 1
                End If
                If intRowNo > intMaxRows - 1 Then
                    Exit For
                End If
                If intQATNo = intNoOfQATs Then
                    Exit For
                End If

            Next
        Catch ex As Exception
            Throw New Exception("Error in InitializeScreen." & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub btnQAT_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim strFormName As String = String.Empty
        Dim btnCurrent As Button
        Try
            btnCurrent = CType(sender, Button)
            strFormName = btnCurrent.Tag
            ShowFormDynamically(strFormName)
        Catch ex As Exception
            Throw New Exception("Error in btnQAT_click" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Sub ShowFormDynamically(strFormName As String)
        Dim frmNewForm As Form = Nothing
        Dim strAssemblyName As String = String.Empty
        Dim frmNewForm_Type As Type
        Try
            strAssemblyName = [Assembly].GetExecutingAssembly().GetName().Name
            frmNewForm_Type = Type.GetType(strAssemblyName & "." & strFormName)
            frmNewForm = CType(Activator.CreateInstance(frmNewForm_Type), Form)
            frmNewForm.ShowDialog()
        Catch ex As Exception
            Throw New Exception("Error in ShowFormDynamically" & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Close()
    End Sub

End Class