<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSplash
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
        Me.components = New System.ComponentModel.Container()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.pbxLoading = New System.Windows.Forms.PictureBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.lblElapseTime = New System.Windows.Forms.Label()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.btnExit = New System.Windows.Forms.Button()
        CType(Me.pbxLoading, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ForeColor = System.Drawing.SystemColors.MenuHighlight
        Me.lblMessage.Location = New System.Drawing.Point(45, 187)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(403, 24)
        Me.lblMessage.TabIndex = 0
        Me.lblMessage.Text = "Please wait . . . . . . . . . . . . . . . . . . . . . . . ."
        '
        'pbxLoading
        '
        Me.pbxLoading.Image = Global.PowerPlant.My.Resources.Resources.loading
        Me.pbxLoading.Location = New System.Drawing.Point(185, 30)
        Me.pbxLoading.Name = "pbxLoading"
        Me.pbxLoading.Size = New System.Drawing.Size(124, 125)
        Me.pbxLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbxLoading.TabIndex = 1
        Me.pbxLoading.TabStop = False
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        '
        'lblElapseTime
        '
        Me.lblElapseTime.AutoSize = True
        Me.lblElapseTime.BackColor = System.Drawing.Color.Black
        Me.lblElapseTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblElapseTime.Font = New System.Drawing.Font("Arial", 23.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblElapseTime.ForeColor = System.Drawing.Color.White
        Me.lblElapseTime.Location = New System.Drawing.Point(182, 79)
        Me.lblElapseTime.Name = "lblElapseTime"
        Me.lblElapseTime.Size = New System.Drawing.Size(98, 38)
        Me.lblElapseTime.TabIndex = 3
        Me.lblElapseTime.Text = " Time"
        '
        'Timer2
        '
        Me.Timer2.Enabled = True
        '
        'btnExit
        '
        Me.btnExit.BackColor = System.Drawing.Color.Silver
        Me.btnExit.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(174, 238)
        Me.btnExit.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(150, 65)
        Me.btnExit.TabIndex = 66
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = False
        '
        'frmSplash
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Ivory
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ClientSize = New System.Drawing.Size(496, 328)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.lblElapseTime)
        Me.Controls.Add(Me.pbxLoading)
        Me.Controls.Add(Me.lblMessage)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSplash"
        Me.Opacity = 0.9R
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "In progress..."
        CType(Me.pbxLoading, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents pbxLoading As System.Windows.Forms.PictureBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents lblElapseTime As System.Windows.Forms.Label
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents btnExit As System.Windows.Forms.Button

End Class
