Imports System.Windows.Forms

Public Class frmQATTester

    Dim drQATTester As dsQATTesters.tblQATTesterRow
    Dim gBtnFont As New Font("Arial", 17.25, FontStyle.Bold, GraphicsUnit.Point)
    Private Sub frmQATTester_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Using taQATTester As New dsQATTestersTableAdapters.tblQATTesterTableAdapter
                Using dtQATTester As New dsQATTesters.tblQATTesterDataTable
                    taQATTester.Fill(dtQATTester)
                    If dtQATTester.Count = 1 Then
                        gstrQATTesterID = dtQATTester(0).TesterID
                        Me.Close()
                        Exit Sub
                    Else
                        BuildQATTesterList(dtQATTester)
                    End If
                End Using
            End Using

        Catch ex As Exception
            Throw New Exception("Error in frmMsgBox_Load" & vbCrLf & ex.Message)      'WO#17432
            Close()
        End Try
    End Sub

    Private Sub BuildQATTesterList(dtQATTester As dsQATTesters.tblQATTesterDataTable)
        Dim intCount As Integer = 1
        Dim strButtonName As String
        Try
            For Each drQATTester In dtQATTester.Rows
                strButtonName = "btnQATTester" & CStr(intCount)
                With Me.Controls.Item(strButtonName)
                    .Text = drQATTester.TesterName
                    .Tag = drQATTester.TesterID
                    .Visible = True
                End With
                intCount = intCount + 1
            Next
        Catch ex As Exception
            Throw New Exception("Error in BuildTesterList" & vbCrLf & ex.Message)      'WO#17432
        End Try
    End Sub

    Private Sub btnQATTester_Click(sender As Object, e As EventArgs) Handles btnQATTester1.Click, btnQATTester2.Click, btnQATTester3.Click, btnQATTester4.Click, btnQATTester5.Click
        gstrQATTesterID = sender.Tag
        Me.Close()
    End Sub
End Class
