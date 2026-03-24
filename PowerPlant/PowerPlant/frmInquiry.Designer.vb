<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInquiry
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.BtnChkSONotes = New System.Windows.Forms.Button()
        Me.btnChkBOM = New System.Windows.Forms.Button()
        Me.btnPrcMntor = New System.Windows.Forms.Button()
        Me.btnSOSchedule = New System.Windows.Forms.Button()
        Me.btnCheckNetworkConn = New System.Windows.Forms.Button()
        Me.btnPalletsProduced = New System.Windows.Forms.Button()
        Me.btnCalculator = New System.Windows.Forms.Button()
        Me.ChkLastDownLoad = New System.Windows.Forms.Button()
        Me.btnRefreshData = New System.Windows.Forms.Button()
        Me.btnProbat = New System.Windows.Forms.Button()
        Me.btnPrintQueue = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'UcHeading1
        '
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(0, 0)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "-- Heading --"
        Me.UcHeading1.Size = New System.Drawing.Size(800, 58)
        Me.UcHeading1.TabIndex = 0
        '
        'btnPrvScn
        '
        Me.btnPrvScn.BackColor = System.Drawing.Color.Silver
        Me.btnPrvScn.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrvScn.Location = New System.Drawing.Point(27, 490)
        Me.btnPrvScn.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrvScn.Name = "btnPrvScn"
        Me.btnPrvScn.Size = New System.Drawing.Size(150, 65)
        Me.btnPrvScn.TabIndex = 77
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'BtnChkSONotes
        '
        Me.BtnChkSONotes.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.BtnChkSONotes.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.BtnChkSONotes.Location = New System.Drawing.Point(286, 160)
        Me.BtnChkSONotes.Name = "BtnChkSONotes"
        Me.BtnChkSONotes.Size = New System.Drawing.Size(244, 63)
        Me.BtnChkSONotes.TabIndex = 79
        Me.BtnChkSONotes.Text = "Shop Order Notes"
        Me.BtnChkSONotes.UseVisualStyleBackColor = False
        '
        'btnChkBOM
        '
        Me.btnChkBOM.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnChkBOM.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnChkBOM.Location = New System.Drawing.Point(286, 80)
        Me.btnChkBOM.Name = "btnChkBOM"
        Me.btnChkBOM.Size = New System.Drawing.Size(244, 63)
        Me.btnChkBOM.TabIndex = 78
        Me.btnChkBOM.Text = "Bill Of Materials"
        Me.btnChkBOM.UseVisualStyleBackColor = False
        '
        'btnPrcMntor
        '
        Me.btnPrcMntor.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnPrcMntor.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnPrcMntor.Location = New System.Drawing.Point(286, 320)
        Me.btnPrcMntor.Name = "btnPrcMntor"
        Me.btnPrcMntor.Size = New System.Drawing.Size(244, 63)
        Me.btnPrcMntor.TabIndex = 79
        Me.btnPrcMntor.Text = "Process Monitor"
        Me.btnPrcMntor.UseVisualStyleBackColor = False
        '
        'btnSOSchedule
        '
        Me.btnSOSchedule.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnSOSchedule.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnSOSchedule.Location = New System.Drawing.Point(27, 320)
        Me.btnSOSchedule.Name = "btnSOSchedule"
        Me.btnSOSchedule.Size = New System.Drawing.Size(244, 63)
        Me.btnSOSchedule.TabIndex = 79
        Me.btnSOSchedule.Text = "Shop Order Schedule"
        Me.btnSOSchedule.UseVisualStyleBackColor = False
        '
        'btnCheckNetworkConn
        '
        Me.btnCheckNetworkConn.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnCheckNetworkConn.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnCheckNetworkConn.Location = New System.Drawing.Point(27, 80)
        Me.btnCheckNetworkConn.Name = "btnCheckNetworkConn"
        Me.btnCheckNetworkConn.Size = New System.Drawing.Size(244, 63)
        Me.btnCheckNetworkConn.TabIndex = 79
        Me.btnCheckNetworkConn.Text = "Check Network Connection"
        Me.btnCheckNetworkConn.UseVisualStyleBackColor = False
        '
        'btnPalletsProduced
        '
        Me.btnPalletsProduced.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnPalletsProduced.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnPalletsProduced.Location = New System.Drawing.Point(286, 240)
        Me.btnPalletsProduced.Name = "btnPalletsProduced"
        Me.btnPalletsProduced.Size = New System.Drawing.Size(244, 63)
        Me.btnPalletsProduced.TabIndex = 78
        Me.btnPalletsProduced.Text = "Pallets Produced  "
        Me.btnPalletsProduced.UseVisualStyleBackColor = False
        '
        'btnCalculator
        '
        Me.btnCalculator.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnCalculator.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnCalculator.Location = New System.Drawing.Point(27, 240)
        Me.btnCalculator.Name = "btnCalculator"
        Me.btnCalculator.Size = New System.Drawing.Size(244, 63)
        Me.btnCalculator.TabIndex = 79
        Me.btnCalculator.Text = "Calculator"
        Me.btnCalculator.UseVisualStyleBackColor = False
        '
        'ChkLastDownLoad
        '
        Me.ChkLastDownLoad.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.ChkLastDownLoad.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.ChkLastDownLoad.Location = New System.Drawing.Point(27, 160)
        Me.ChkLastDownLoad.Name = "ChkLastDownLoad"
        Me.ChkLastDownLoad.Size = New System.Drawing.Size(244, 63)
        Me.ChkLastDownLoad.TabIndex = 79
        Me.ChkLastDownLoad.Text = "Check Last Down Load Time"
        Me.ChkLastDownLoad.UseVisualStyleBackColor = False
        '
        'btnRefreshData
        '
        Me.btnRefreshData.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnRefreshData.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnRefreshData.Location = New System.Drawing.Point(542, 160)
        Me.btnRefreshData.Name = "btnRefreshData"
        Me.btnRefreshData.Size = New System.Drawing.Size(244, 63)
        Me.btnRefreshData.TabIndex = 78
        Me.btnRefreshData.Text = "Refresh Data"
        Me.btnRefreshData.UseVisualStyleBackColor = False
        Me.btnRefreshData.Visible = False
        '
        'btnProbat
        '
        Me.btnProbat.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnProbat.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnProbat.Location = New System.Drawing.Point(542, 240)
        Me.btnProbat.Name = "btnProbat"
        Me.btnProbat.Size = New System.Drawing.Size(244, 63)
        Me.btnProbat.TabIndex = 78
        Me.btnProbat.Text = "Probat"
        Me.btnProbat.UseVisualStyleBackColor = False
        Me.btnProbat.Visible = False
        '
        'btnPrintQueue
        '
        Me.btnPrintQueue.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnPrintQueue.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnPrintQueue.Location = New System.Drawing.Point(542, 80)
        Me.btnPrintQueue.Name = "btnPrintQueue"
        Me.btnPrintQueue.Size = New System.Drawing.Size(244, 63)
        Me.btnPrintQueue.TabIndex = 80
        Me.btnPrintQueue.Text = "Label Print Queue"
        Me.btnPrintQueue.UseVisualStyleBackColor = False
        '
        'frmInquiry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnPrintQueue)
        Me.Controls.Add(Me.btnCheckNetworkConn)
        Me.Controls.Add(Me.btnSOSchedule)
        Me.Controls.Add(Me.btnCalculator)
        Me.Controls.Add(Me.ChkLastDownLoad)
        Me.Controls.Add(Me.btnPrcMntor)
        Me.Controls.Add(Me.BtnChkSONotes)
        Me.Controls.Add(Me.btnPalletsProduced)
        Me.Controls.Add(Me.btnProbat)
        Me.Controls.Add(Me.btnRefreshData)
        Me.Controls.Add(Me.btnChkBOM)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInquiry"
        Me.Text = "frmInquiry"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents BtnChkSONotes As System.Windows.Forms.Button
    Friend WithEvents btnChkBOM As System.Windows.Forms.Button
    Friend WithEvents btnPrcMntor As System.Windows.Forms.Button
    Friend WithEvents btnSOSchedule As System.Windows.Forms.Button
    Friend WithEvents btnCheckNetworkConn As System.Windows.Forms.Button
    Friend WithEvents btnPalletsProduced As System.Windows.Forms.Button
    Friend WithEvents btnCalculator As System.Windows.Forms.Button
    Friend WithEvents ChkLastDownLoad As System.Windows.Forms.Button
    Friend WithEvents btnRefreshData As System.Windows.Forms.Button
    Friend WithEvents btnProbat As System.Windows.Forms.Button
    Friend WithEvents btnPrintQueue As System.Windows.Forms.Button
End Class
