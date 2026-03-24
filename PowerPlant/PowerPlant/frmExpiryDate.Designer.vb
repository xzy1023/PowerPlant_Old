<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExpiryDate
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
        Me.txtExpiryDay = New System.Windows.Forms.TextBox()
        Me.lblExpiryDay = New System.Windows.Forms.Label()
        Me.txtExpiryMonth = New System.Windows.Forms.TextBox()
        Me.lblExpiryMonth = New System.Windows.Forms.Label()
        Me.txtExpiryYear = New System.Windows.Forms.TextBox()
        Me.lblExpiryYear = New System.Windows.Forms.Label()
        Me.lblExpiryDate = New System.Windows.Forms.Label()
        Me.btnConfirm = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtExpiryDay
        '
        Me.txtExpiryDay.BackColor = System.Drawing.Color.Black
        Me.txtExpiryDay.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtExpiryDay.ForeColor = System.Drawing.Color.White
        Me.txtExpiryDay.Location = New System.Drawing.Point(316, 140)
        Me.txtExpiryDay.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtExpiryDay.MaxLength = 2
        Me.txtExpiryDay.Name = "txtExpiryDay"
        Me.txtExpiryDay.Size = New System.Drawing.Size(51, 35)
        Me.txtExpiryDay.TabIndex = 25
        '
        'lblExpiryDay
        '
        Me.lblExpiryDay.AutoSize = True
        Me.lblExpiryDay.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.lblExpiryDay.ForeColor = System.Drawing.Color.CornflowerBlue
        Me.lblExpiryDay.Location = New System.Drawing.Point(209, 148)
        Me.lblExpiryDay.Name = "lblExpiryDay"
        Me.lblExpiryDay.Size = New System.Drawing.Size(76, 27)
        Me.lblExpiryDay.TabIndex = 24
        Me.lblExpiryDay.Text = "Day - "
        '
        'txtExpiryMonth
        '
        Me.txtExpiryMonth.BackColor = System.Drawing.Color.Black
        Me.txtExpiryMonth.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtExpiryMonth.ForeColor = System.Drawing.Color.White
        Me.txtExpiryMonth.Location = New System.Drawing.Point(316, 86)
        Me.txtExpiryMonth.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtExpiryMonth.MaxLength = 2
        Me.txtExpiryMonth.Name = "txtExpiryMonth"
        Me.txtExpiryMonth.Size = New System.Drawing.Size(51, 35)
        Me.txtExpiryMonth.TabIndex = 23
        '
        'lblExpiryMonth
        '
        Me.lblExpiryMonth.AutoSize = True
        Me.lblExpiryMonth.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.lblExpiryMonth.ForeColor = System.Drawing.Color.CornflowerBlue
        Me.lblExpiryMonth.Location = New System.Drawing.Point(209, 89)
        Me.lblExpiryMonth.Name = "lblExpiryMonth"
        Me.lblExpiryMonth.Size = New System.Drawing.Size(101, 27)
        Me.lblExpiryMonth.TabIndex = 22
        Me.lblExpiryMonth.Text = "Month - "
        '
        'txtExpiryYear
        '
        Me.txtExpiryYear.BackColor = System.Drawing.Color.Black
        Me.txtExpiryYear.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtExpiryYear.ForeColor = System.Drawing.Color.White
        Me.txtExpiryYear.Location = New System.Drawing.Point(313, 30)
        Me.txtExpiryYear.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtExpiryYear.MaxLength = 4
        Me.txtExpiryYear.Name = "txtExpiryYear"
        Me.txtExpiryYear.Size = New System.Drawing.Size(84, 35)
        Me.txtExpiryYear.TabIndex = 21
        '
        'lblExpiryYear
        '
        Me.lblExpiryYear.AutoSize = True
        Me.lblExpiryYear.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.lblExpiryYear.ForeColor = System.Drawing.Color.CornflowerBlue
        Me.lblExpiryYear.Location = New System.Drawing.Point(209, 33)
        Me.lblExpiryYear.Name = "lblExpiryYear"
        Me.lblExpiryYear.Size = New System.Drawing.Size(82, 27)
        Me.lblExpiryYear.TabIndex = 19
        Me.lblExpiryYear.Text = "Year - "
        '
        'lblExpiryDate
        '
        Me.lblExpiryDate.AutoSize = True
        Me.lblExpiryDate.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.lblExpiryDate.ForeColor = System.Drawing.Color.CornflowerBlue
        Me.lblExpiryDate.Location = New System.Drawing.Point(52, 33)
        Me.lblExpiryDate.Name = "lblExpiryDate"
        Me.lblExpiryDate.Size = New System.Drawing.Size(142, 27)
        Me.lblExpiryDate.TabIndex = 20
        Me.lblExpiryDate.Text = "Expiry Date:"
        '
        'btnConfirm
        '
        Me.btnConfirm.BackColor = System.Drawing.Color.Silver
        Me.btnConfirm.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnConfirm.Location = New System.Drawing.Point(160, 221)
        Me.btnConfirm.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnConfirm.Name = "btnConfirm"
        Me.btnConfirm.Size = New System.Drawing.Size(150, 65)
        Me.btnConfirm.TabIndex = 27
        Me.btnConfirm.Text = "Confirm"
        Me.btnConfirm.UseVisualStyleBackColor = False
        '
        'frmExpiryDate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Ivory
        Me.ClientSize = New System.Drawing.Size(509, 328)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnConfirm)
        Me.Controls.Add(Me.txtExpiryDay)
        Me.Controls.Add(Me.lblExpiryDay)
        Me.Controls.Add(Me.txtExpiryMonth)
        Me.Controls.Add(Me.lblExpiryMonth)
        Me.Controls.Add(Me.txtExpiryYear)
        Me.Controls.Add(Me.lblExpiryYear)
        Me.Controls.Add(Me.lblExpiryDate)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmExpiryDate"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Enter Expiry Date"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtExpiryDay As System.Windows.Forms.TextBox
    Friend WithEvents lblExpiryDay As System.Windows.Forms.Label
    Friend WithEvents txtExpiryMonth As System.Windows.Forms.TextBox
    Friend WithEvents lblExpiryMonth As System.Windows.Forms.Label
    Friend WithEvents txtExpiryYear As System.Windows.Forms.TextBox
    Friend WithEvents lblExpiryYear As System.Windows.Forms.Label
    Friend WithEvents lblExpiryDate As System.Windows.Forms.Label
    Friend WithEvents btnConfirm As System.Windows.Forms.Button

End Class
