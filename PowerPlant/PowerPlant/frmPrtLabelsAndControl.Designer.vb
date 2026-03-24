<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrtLabelsAndControl
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
        Me.btnFilterCoder = New System.Windows.Forms.Button
        Me.btnPackageCoder = New System.Windows.Forms.Button
        Me.btnCaseLabeler = New System.Windows.Forms.Button
        Me.btnPrvScn = New System.Windows.Forms.Button
        Me.btnPrintPalletLabels = New System.Windows.Forms.Button
        Me.btnReprintPalletLabels = New System.Windows.Forms.Button
        Me.btnPrtCaseLabelsNoSO = New System.Windows.Forms.Button
        Me.btnCreatePallet = New System.Windows.Forms.Button
        Me.UcHeading1 = New PowerPlant.ucHeading
        Me.lblMessage = New System.Windows.Forms.Label
        Me.btnInquiry = New System.Windows.Forms.Button
        Me.btnPrtCaseLabelsWithSO = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnFilterCoder
        '
        Me.btnFilterCoder.BackColor = System.Drawing.Color.Silver
        Me.btnFilterCoder.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnFilterCoder.Location = New System.Drawing.Point(29, 265)
        Me.btnFilterCoder.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnFilterCoder.Name = "btnFilterCoder"
        Me.btnFilterCoder.Size = New System.Drawing.Size(230, 78)
        Me.btnFilterCoder.TabIndex = 15
        Me.btnFilterCoder.Text = "Filter Coder"
        Me.btnFilterCoder.UseVisualStyleBackColor = False
        '
        'btnPackageCoder
        '
        Me.btnPackageCoder.BackColor = System.Drawing.Color.Silver
        Me.btnPackageCoder.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnPackageCoder.Location = New System.Drawing.Point(29, 169)
        Me.btnPackageCoder.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPackageCoder.Name = "btnPackageCoder"
        Me.btnPackageCoder.Size = New System.Drawing.Size(230, 78)
        Me.btnPackageCoder.TabIndex = 14
        Me.btnPackageCoder.Text = "Package Coder"
        Me.btnPackageCoder.UseVisualStyleBackColor = False
        '
        'btnCaseLabeler
        '
        Me.btnCaseLabeler.BackColor = System.Drawing.Color.Silver
        Me.btnCaseLabeler.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnCaseLabeler.Location = New System.Drawing.Point(29, 73)
        Me.btnCaseLabeler.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCaseLabeler.Name = "btnCaseLabeler"
        Me.btnCaseLabeler.Size = New System.Drawing.Size(230, 78)
        Me.btnCaseLabeler.TabIndex = 13
        Me.btnCaseLabeler.Text = "Case Labeler"
        Me.btnCaseLabeler.UseVisualStyleBackColor = False
        '
        'btnPrvScn
        '
        Me.btnPrvScn.BackColor = System.Drawing.Color.Silver
        Me.btnPrvScn.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrvScn.Location = New System.Drawing.Point(27, 490)
        Me.btnPrvScn.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrvScn.Name = "btnPrvScn"
        Me.btnPrvScn.Size = New System.Drawing.Size(150, 65)
        Me.btnPrvScn.TabIndex = 12
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'btnPrintPalletLabels
        '
        Me.btnPrintPalletLabels.BackColor = System.Drawing.Color.Silver
        Me.btnPrintPalletLabels.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnPrintPalletLabels.Location = New System.Drawing.Point(546, 169)
        Me.btnPrintPalletLabels.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrintPalletLabels.Name = "btnPrintPalletLabels"
        Me.btnPrintPalletLabels.Size = New System.Drawing.Size(230, 78)
        Me.btnPrintPalletLabels.TabIndex = 13
        Me.btnPrintPalletLabels.Text = "Print Pallet Labels"
        Me.btnPrintPalletLabels.UseVisualStyleBackColor = False
        '
        'btnReprintPalletLabels
        '
        Me.btnReprintPalletLabels.BackColor = System.Drawing.Color.Silver
        Me.btnReprintPalletLabels.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnReprintPalletLabels.Location = New System.Drawing.Point(546, 265)
        Me.btnReprintPalletLabels.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnReprintPalletLabels.Name = "btnReprintPalletLabels"
        Me.btnReprintPalletLabels.Size = New System.Drawing.Size(230, 78)
        Me.btnReprintPalletLabels.TabIndex = 14
        Me.btnReprintPalletLabels.Text = "Reprint Pallet Labels"
        Me.btnReprintPalletLabels.UseVisualStyleBackColor = False
        '
        'btnPrtCaseLabelsNoSO
        '
        Me.btnPrtCaseLabelsNoSO.BackColor = System.Drawing.Color.Silver
        Me.btnPrtCaseLabelsNoSO.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnPrtCaseLabelsNoSO.Location = New System.Drawing.Point(289, 73)
        Me.btnPrtCaseLabelsNoSO.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrtCaseLabelsNoSO.Name = "btnPrtCaseLabelsNoSO"
        Me.btnPrtCaseLabelsNoSO.Size = New System.Drawing.Size(230, 78)
        Me.btnPrtCaseLabelsNoSO.TabIndex = 15
        Me.btnPrtCaseLabelsNoSO.Text = "Print Case Labels (W/O Shop Order)"
        Me.btnPrtCaseLabelsNoSO.UseVisualStyleBackColor = False
        '
        'btnCreatePallet
        '
        Me.btnCreatePallet.BackColor = System.Drawing.Color.Silver
        Me.btnCreatePallet.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnCreatePallet.Location = New System.Drawing.Point(546, 73)
        Me.btnCreatePallet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCreatePallet.Name = "btnCreatePallet"
        Me.btnCreatePallet.Size = New System.Drawing.Size(230, 78)
        Me.btnCreatePallet.TabIndex = 15
        Me.btnCreatePallet.Text = "Create Pallet"
        Me.btnCreatePallet.UseVisualStyleBackColor = False
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.Blue
        Me.UcHeading1.Location = New System.Drawing.Point(0, 0)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "-- Heading --"
        Me.UcHeading1.Size = New System.Drawing.Size(800, 55)
        Me.UcHeading1.TabIndex = 0
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ForeColor = System.Drawing.Color.LightSalmon
        Me.lblMessage.Location = New System.Drawing.Point(22, 565)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(108, 27)
        Me.lblMessage.TabIndex = 89
        Me.lblMessage.Text = "Message"
        '
        'btnInquiry
        '
        Me.btnInquiry.BackColor = System.Drawing.Color.Silver
        Me.btnInquiry.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnInquiry.Location = New System.Drawing.Point(29, 367)
        Me.btnInquiry.Name = "btnInquiry"
        Me.btnInquiry.Size = New System.Drawing.Size(230, 78)
        Me.btnInquiry.TabIndex = 90
        Me.btnInquiry.Text = "Inquiry"
        Me.btnInquiry.UseVisualStyleBackColor = False
        '
        'btnPrtCaseLabelsWithSO
        '
        Me.btnPrtCaseLabelsWithSO.BackColor = System.Drawing.Color.Silver
        Me.btnPrtCaseLabelsWithSO.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnPrtCaseLabelsWithSO.Location = New System.Drawing.Point(290, 169)
        Me.btnPrtCaseLabelsWithSO.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrtCaseLabelsWithSO.Name = "btnPrtCaseLabelsWithSO"
        Me.btnPrtCaseLabelsWithSO.Size = New System.Drawing.Size(230, 78)
        Me.btnPrtCaseLabelsWithSO.TabIndex = 15
        Me.btnPrtCaseLabelsWithSO.Text = "Print Case Labels With Shop Order"
        Me.btnPrtCaseLabelsWithSO.UseVisualStyleBackColor = False
        '
        'frmPrtLabelsAndControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnInquiry)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.btnCreatePallet)
        Me.Controls.Add(Me.btnPrtCaseLabelsWithSO)
        Me.Controls.Add(Me.btnPrtCaseLabelsNoSO)
        Me.Controls.Add(Me.btnFilterCoder)
        Me.Controls.Add(Me.btnReprintPalletLabels)
        Me.Controls.Add(Me.btnPackageCoder)
        Me.Controls.Add(Me.btnPrintPalletLabels)
        Me.Controls.Add(Me.btnCaseLabeler)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPrtLabelsAndControl"
        Me.Text = "Print Labels/Printer Control"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents btnFilterCoder As System.Windows.Forms.Button
    Friend WithEvents btnPackageCoder As System.Windows.Forms.Button
    Friend WithEvents btnCaseLabeler As System.Windows.Forms.Button
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents btnPrintPalletLabels As System.Windows.Forms.Button
    Friend WithEvents btnReprintPalletLabels As System.Windows.Forms.Button
    Friend WithEvents btnPrtCaseLabelsNoSO As System.Windows.Forms.Button
    Friend WithEvents btnCreatePallet As System.Windows.Forms.Button
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents btnInquiry As System.Windows.Forms.Button
    Friend WithEvents btnPrtCaseLabelsWithSO As System.Windows.Forms.Button
End Class
