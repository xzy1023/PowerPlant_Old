'WO#6059 Add Start
Public Class frmLabelPrintQueue
    Private Sub btnPrvScn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrvScn.Click
        Me.Close()
    End Sub

    Private Sub frmLabelPrintQueue_Load(sender As Object, e As EventArgs) Handles Me.Load
        UcHeading1.ScreenTitle = "Label Print Queue"
        RefreshData()
    End Sub

    Public Sub RefreshData()
        Try
            If gblnSvrConnIsUp = True Then
                Dim dtLPQ As dsLabelPrintJobs.PPsp_LabelPrintJobs_SelDataTable
                dtLPQ = SharedFunctions.JobsInPrintQueue(gdrSessCtl.Facility, Nothing, Nothing)
                dgvPrintQueue.DataSource = dtLPQ
                dgvPrintQueue.Refresh()
            Else
                MessageBox.Show("No connection to the server. Data is not available.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            If ex.Message.IndexOf("transport-level error") Then
                MessageBox.Show("No connection to the server. Data is not available or not accruate.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace, "** Unexpected Application Error - Contact Supervisor", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        RefreshData()
    End Sub
End Class
'WO#6059 ADD Stop