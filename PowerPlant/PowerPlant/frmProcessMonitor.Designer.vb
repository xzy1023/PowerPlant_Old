<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProcessMonitor
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
        Me.components = New System.ComponentModel.Container
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.UcHeading1 = New PowerPlant.ucHeading
        Me.btnPrvScn = New System.Windows.Forms.Button
        Me.lblShift = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.lblSOSch_CaseProd = New System.Windows.Forms.Label
        Me.lblSOSch_CsPerHr = New System.Windows.Forms.Label
        Me.lblSO_Efficiency = New System.Windows.Forms.Label
        Me.lblSOAct_CaseProd = New System.Windows.Forms.Label
        Me.lblSOAct_CsPerHr = New System.Windows.Forms.Label
        Me.lblShiftAct_CsPerHr = New System.Windows.Forms.Label
        Me.lblShiftAct_CaseProd = New System.Windows.Forms.Label
        Me.lblShift_Efficiency = New System.Windows.Forms.Label
        Me.lblShiftSch_CsPerHr = New System.Windows.Forms.Label
        Me.lblShiftSch_CaseProd = New System.Windows.Forms.Label
        Me.lblOperator = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.lblPkgLine = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.lblSO_Progress = New System.Windows.Forms.Label
        Me.lblShift_Progress = New System.Windows.Forms.Label
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(265, 159)
        Me.Label7.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(209, 27)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "----Shop Order ----"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(32, 261)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(197, 27)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Cases Produced:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(32, 305)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(145, 27)
        Me.Label8.TabIndex = 9
        Me.Label8.Text = "Cases/Hour:"
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.Blue
        Me.UcHeading1.Location = New System.Drawing.Point(0, 0)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "-- Heading --"
        Me.UcHeading1.Size = New System.Drawing.Size(800, 55)
        Me.UcHeading1.TabIndex = 14
        '
        'btnPrvScn
        '
        Me.btnPrvScn.BackColor = System.Drawing.Color.Silver
        Me.btnPrvScn.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrvScn.Location = New System.Drawing.Point(27, 490)
        Me.btnPrvScn.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrvScn.Name = "btnPrvScn"
        Me.btnPrvScn.Size = New System.Drawing.Size(150, 65)
        Me.btnPrvScn.TabIndex = 79
        Me.btnPrvScn.Text = "Close Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'lblShift
        '
        Me.lblShift.AutoSize = True
        Me.lblShift.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShift.ForeColor = System.Drawing.Color.White
        Me.lblShift.Location = New System.Drawing.Point(537, 159)
        Me.lblShift.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblShift.Name = "lblShift"
        Me.lblShift.Size = New System.Drawing.Size(212, 27)
        Me.lblShift.TabIndex = 80
        Me.lblShift.Text = "---------Shift ---------"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(241, 205)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(128, 27)
        Me.Label2.TabIndex = 81
        Me.Label2.Text = "Scheduled"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(398, 205)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 27)
        Me.Label3.TabIndex = 82
        Me.Label3.Text = "Actual"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(685, 205)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 27)
        Me.Label4.TabIndex = 84
        Me.Label4.Text = "Actual"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(528, 205)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(119, 27)
        Me.Label5.TabIndex = 83
        Me.Label5.Text = "Estimated"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(32, 356)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(143, 27)
        Me.Label6.TabIndex = 85
        Me.Label6.Text = "%Efficiency:"
        '
        'lblSOSch_CaseProd
        '
        Me.lblSOSch_CaseProd.AutoSize = True
        Me.lblSOSch_CaseProd.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSOSch_CaseProd.ForeColor = System.Drawing.Color.White
        Me.lblSOSch_CaseProd.Location = New System.Drawing.Point(284, 261)
        Me.lblSOSch_CaseProd.Name = "lblSOSch_CaseProd"
        Me.lblSOSch_CaseProd.Size = New System.Drawing.Size(51, 27)
        Me.lblSOSch_CaseProd.TabIndex = 86
        Me.lblSOSch_CaseProd.Text = "423"
        '
        'lblSOSch_CsPerHr
        '
        Me.lblSOSch_CsPerHr.AutoSize = True
        Me.lblSOSch_CsPerHr.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSOSch_CsPerHr.ForeColor = System.Drawing.Color.White
        Me.lblSOSch_CsPerHr.Location = New System.Drawing.Point(287, 305)
        Me.lblSOSch_CsPerHr.Name = "lblSOSch_CsPerHr"
        Me.lblSOSch_CsPerHr.Size = New System.Drawing.Size(38, 27)
        Me.lblSOSch_CsPerHr.TabIndex = 87
        Me.lblSOSch_CsPerHr.Text = "60"
        '
        'lblSO_Efficiency
        '
        Me.lblSO_Efficiency.AutoSize = True
        Me.lblSO_Efficiency.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSO_Efficiency.ForeColor = System.Drawing.Color.GreenYellow
        Me.lblSO_Efficiency.Location = New System.Drawing.Point(352, 356)
        Me.lblSO_Efficiency.Name = "lblSO_Efficiency"
        Me.lblSO_Efficiency.Size = New System.Drawing.Size(45, 27)
        Me.lblSO_Efficiency.TabIndex = 88
        Me.lblSO_Efficiency.Text = "0.0"
        '
        'lblSOAct_CaseProd
        '
        Me.lblSOAct_CaseProd.AutoSize = True
        Me.lblSOAct_CaseProd.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSOAct_CaseProd.ForeColor = System.Drawing.Color.White
        Me.lblSOAct_CaseProd.Location = New System.Drawing.Point(399, 261)
        Me.lblSOAct_CaseProd.Name = "lblSOAct_CaseProd"
        Me.lblSOAct_CaseProd.Size = New System.Drawing.Size(38, 27)
        Me.lblSOAct_CaseProd.TabIndex = 89
        Me.lblSOAct_CaseProd.Text = "40"
        '
        'lblSOAct_CsPerHr
        '
        Me.lblSOAct_CsPerHr.AutoSize = True
        Me.lblSOAct_CsPerHr.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSOAct_CsPerHr.ForeColor = System.Drawing.Color.White
        Me.lblSOAct_CsPerHr.Location = New System.Drawing.Point(398, 305)
        Me.lblSOAct_CsPerHr.Name = "lblSOAct_CsPerHr"
        Me.lblSOAct_CsPerHr.Size = New System.Drawing.Size(38, 27)
        Me.lblSOAct_CsPerHr.TabIndex = 90
        Me.lblSOAct_CsPerHr.Text = "50"
        '
        'lblShiftAct_CsPerHr
        '
        Me.lblShiftAct_CsPerHr.AutoSize = True
        Me.lblShiftAct_CsPerHr.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShiftAct_CsPerHr.ForeColor = System.Drawing.Color.White
        Me.lblShiftAct_CsPerHr.Location = New System.Drawing.Point(685, 305)
        Me.lblShiftAct_CsPerHr.Name = "lblShiftAct_CsPerHr"
        Me.lblShiftAct_CsPerHr.Size = New System.Drawing.Size(38, 27)
        Me.lblShiftAct_CsPerHr.TabIndex = 95
        Me.lblShiftAct_CsPerHr.Text = "56"
        '
        'lblShiftAct_CaseProd
        '
        Me.lblShiftAct_CaseProd.AutoSize = True
        Me.lblShiftAct_CaseProd.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShiftAct_CaseProd.ForeColor = System.Drawing.Color.White
        Me.lblShiftAct_CaseProd.Location = New System.Drawing.Point(686, 261)
        Me.lblShiftAct_CaseProd.Name = "lblShiftAct_CaseProd"
        Me.lblShiftAct_CaseProd.Size = New System.Drawing.Size(51, 27)
        Me.lblShiftAct_CaseProd.TabIndex = 94
        Me.lblShiftAct_CaseProd.Text = "213"
        '
        'lblShift_Efficiency
        '
        Me.lblShift_Efficiency.AutoSize = True
        Me.lblShift_Efficiency.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShift_Efficiency.ForeColor = System.Drawing.Color.GreenYellow
        Me.lblShift_Efficiency.Location = New System.Drawing.Point(639, 356)
        Me.lblShift_Efficiency.Name = "lblShift_Efficiency"
        Me.lblShift_Efficiency.Size = New System.Drawing.Size(45, 27)
        Me.lblShift_Efficiency.TabIndex = 93
        Me.lblShift_Efficiency.Text = "0.0"
        '
        'lblShiftSch_CsPerHr
        '
        Me.lblShiftSch_CsPerHr.AutoSize = True
        Me.lblShiftSch_CsPerHr.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShiftSch_CsPerHr.ForeColor = System.Drawing.Color.White
        Me.lblShiftSch_CsPerHr.Location = New System.Drawing.Point(574, 305)
        Me.lblShiftSch_CsPerHr.Name = "lblShiftSch_CsPerHr"
        Me.lblShiftSch_CsPerHr.Size = New System.Drawing.Size(38, 27)
        Me.lblShiftSch_CsPerHr.TabIndex = 92
        Me.lblShiftSch_CsPerHr.Text = "58"
        '
        'lblShiftSch_CaseProd
        '
        Me.lblShiftSch_CaseProd.AutoSize = True
        Me.lblShiftSch_CaseProd.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShiftSch_CaseProd.ForeColor = System.Drawing.Color.White
        Me.lblShiftSch_CaseProd.Location = New System.Drawing.Point(571, 261)
        Me.lblShiftSch_CaseProd.Name = "lblShiftSch_CaseProd"
        Me.lblShiftSch_CaseProd.Size = New System.Drawing.Size(51, 27)
        Me.lblShiftSch_CaseProd.TabIndex = 91
        Me.lblShiftSch_CaseProd.Text = "650"
        '
        'lblOperator
        '
        Me.lblOperator.AutoSize = True
        Me.lblOperator.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOperator.ForeColor = System.Drawing.Color.White
        Me.lblOperator.Location = New System.Drawing.Point(147, 117)
        Me.lblOperator.MaximumSize = New System.Drawing.Size(250, 27)
        Me.lblOperator.Name = "lblOperator"
        Me.lblOperator.Size = New System.Drawing.Size(139, 27)
        Me.lblOperator.TabIndex = 98
        Me.lblOperator.Text = "Tina Becker"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(32, 115)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(120, 29)
        Me.Label10.TabIndex = 99
        Me.Label10.Text = "Operator:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.White
        Me.Label12.Location = New System.Drawing.Point(32, 72)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(197, 29)
        Me.Label12.TabIndex = 97
        Me.Label12.Text = "Packaging Line:"
        '
        'lblPkgLine
        '
        Me.lblPkgLine.AutoSize = True
        Me.lblPkgLine.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPkgLine.ForeColor = System.Drawing.Color.White
        Me.lblPkgLine.Location = New System.Drawing.Point(230, 74)
        Me.lblPkgLine.MaximumSize = New System.Drawing.Size(250, 27)
        Me.lblPkgLine.Name = "lblPkgLine"
        Me.lblPkgLine.Size = New System.Drawing.Size(119, 27)
        Me.lblPkgLine.TabIndex = 100
        Me.lblPkgLine.Text = "Triangle 3"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(32, 406)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(192, 27)
        Me.Label11.TabIndex = 101
        Me.Label11.Text = "Progress(Cs/Hr):"
        '
        'lblSO_Progress
        '
        Me.lblSO_Progress.AutoSize = True
        Me.lblSO_Progress.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSO_Progress.ForeColor = System.Drawing.Color.GreenYellow
        Me.lblSO_Progress.Location = New System.Drawing.Point(359, 406)
        Me.lblSO_Progress.Name = "lblSO_Progress"
        Me.lblSO_Progress.Size = New System.Drawing.Size(25, 27)
        Me.lblSO_Progress.TabIndex = 102
        Me.lblSO_Progress.Text = "0"
        '
        'lblShift_Progress
        '
        Me.lblShift_Progress.AutoSize = True
        Me.lblShift_Progress.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShift_Progress.ForeColor = System.Drawing.Color.GreenYellow
        Me.lblShift_Progress.Location = New System.Drawing.Point(646, 406)
        Me.lblShift_Progress.Name = "lblShift_Progress"
        Me.lblShift_Progress.Size = New System.Drawing.Size(25, 27)
        Me.lblShift_Progress.TabIndex = 103
        Me.lblShift_Progress.Text = "0"
        '
        'Timer1
        '
        '
        'frmProcessMonitor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblShift_Progress)
        Me.Controls.Add(Me.lblSO_Progress)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.lblPkgLine)
        Me.Controls.Add(Me.lblOperator)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.lblShiftAct_CsPerHr)
        Me.Controls.Add(Me.lblShiftAct_CaseProd)
        Me.Controls.Add(Me.lblShift_Efficiency)
        Me.Controls.Add(Me.lblShiftSch_CsPerHr)
        Me.Controls.Add(Me.lblShiftSch_CaseProd)
        Me.Controls.Add(Me.lblSOAct_CsPerHr)
        Me.Controls.Add(Me.lblSOAct_CaseProd)
        Me.Controls.Add(Me.lblSO_Efficiency)
        Me.Controls.Add(Me.lblSOSch_CsPerHr)
        Me.Controls.Add(Me.lblSOSch_CaseProd)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblShift)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.UcHeading1)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmProcessMonitor"
        Me.Text = "Process Monitor"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents lblShift As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblSOSch_CaseProd As System.Windows.Forms.Label
    Friend WithEvents lblSOSch_CsPerHr As System.Windows.Forms.Label
    Friend WithEvents lblSO_Efficiency As System.Windows.Forms.Label
    Friend WithEvents lblSOAct_CaseProd As System.Windows.Forms.Label
    Friend WithEvents lblSOAct_CsPerHr As System.Windows.Forms.Label
    Friend WithEvents lblShiftAct_CsPerHr As System.Windows.Forms.Label
    Friend WithEvents lblShiftAct_CaseProd As System.Windows.Forms.Label
    Friend WithEvents lblShift_Efficiency As System.Windows.Forms.Label
    Friend WithEvents lblShiftSch_CsPerHr As System.Windows.Forms.Label
    Friend WithEvents lblShiftSch_CaseProd As System.Windows.Forms.Label
    Friend WithEvents lblOperator As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lblPkgLine As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblSO_Progress As System.Windows.Forms.Label
    Friend WithEvents lblShift_Progress As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
End Class
