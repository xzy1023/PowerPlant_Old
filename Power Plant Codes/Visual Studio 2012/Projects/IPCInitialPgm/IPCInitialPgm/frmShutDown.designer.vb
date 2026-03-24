<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmShutDown
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
        Me.UcHeading1 = New IPCInitialPgm.ucHeading()
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.BtnRestart = New System.Windows.Forms.Button()
        Me.btnShutDown = New System.Windows.Forms.Button()
        Me.btnLogOff = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.Blue
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
        'BtnRestart
        '
        Me.BtnRestart.BackColor = System.Drawing.Color.Silver
        Me.BtnRestart.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.BtnRestart.Location = New System.Drawing.Point(286, 160)
        Me.BtnRestart.Name = "BtnRestart"
        Me.BtnRestart.Size = New System.Drawing.Size(244, 63)
        Me.BtnRestart.TabIndex = 79
        Me.BtnRestart.Text = "Restart"
        Me.BtnRestart.UseVisualStyleBackColor = False
        '
        'btnShutDown
        '
        Me.btnShutDown.BackColor = System.Drawing.Color.Silver
        Me.btnShutDown.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnShutDown.Location = New System.Drawing.Point(286, 80)
        Me.btnShutDown.Name = "btnShutDown"
        Me.btnShutDown.Size = New System.Drawing.Size(244, 63)
        Me.btnShutDown.TabIndex = 78
        Me.btnShutDown.Text = "Shut Down"
        Me.btnShutDown.UseVisualStyleBackColor = False
        '
        'btnLogOff
        '
        Me.btnLogOff.BackColor = System.Drawing.Color.Silver
        Me.btnLogOff.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnLogOff.Location = New System.Drawing.Point(286, 240)
        Me.btnLogOff.Name = "btnLogOff"
        Me.btnLogOff.Size = New System.Drawing.Size(244, 63)
        Me.btnLogOff.TabIndex = 78
        Me.btnLogOff.Text = "Log Off"
        Me.btnLogOff.UseVisualStyleBackColor = False
        '
        'frmShutDown
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.BtnRestart)
        Me.Controls.Add(Me.btnLogOff)
        Me.Controls.Add(Me.btnShutDown)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmShutDown"
        Me.Text = "frmShutDown"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As IPCInitialPgm.ucHeading
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents BtnRestart As System.Windows.Forms.Button
    Friend WithEvents btnShutDown As System.Windows.Forms.Button
    Friend WithEvents btnLogOff As System.Windows.Forms.Button
End Class
