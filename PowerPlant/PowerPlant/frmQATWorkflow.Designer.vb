<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmQATWorkflow
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.btnPrevious = New System.Windows.Forms.Button()
        Me.btnMaterialValidation = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.btnVerifyCartonBox = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(0, 12)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "In Process QA Tests"
        Me.UcHeading1.Size = New System.Drawing.Size(800, 55)
        Me.UcHeading1.TabIndex = 15
        '
        'btnPrevious
        '
        Me.btnPrevious.BackColor = System.Drawing.Color.Silver
        Me.btnPrevious.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrevious.Location = New System.Drawing.Point(27, 490)
        Me.btnPrevious.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrevious.Name = "btnPrevious"
        Me.btnPrevious.Size = New System.Drawing.Size(150, 65)
        Me.btnPrevious.TabIndex = 79
        Me.btnPrevious.Text = "Previous"
        Me.btnPrevious.UseVisualStyleBackColor = False
        '
        'btnMaterialValidation
        '
        Me.btnMaterialValidation.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnMaterialValidation.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnMaterialValidation.Location = New System.Drawing.Point(542, 80)
        Me.btnMaterialValidation.Name = "btnMaterialValidation"
        Me.btnMaterialValidation.Size = New System.Drawing.Size(244, 63)
        Me.btnMaterialValidation.TabIndex = 80
        Me.btnMaterialValidation.Text = "Material Validation"
        Me.btnMaterialValidation.UseVisualStyleBackColor = False
        Me.btnMaterialValidation.Visible = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.Button1.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.Button1.Location = New System.Drawing.Point(27, 80)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(244, 63)
        Me.Button1.TabIndex = 81
        Me.Button1.Text = "Weight Test"
        Me.Button1.UseVisualStyleBackColor = False
        Me.Button1.Visible = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.Button2.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.Button2.Location = New System.Drawing.Point(27, 240)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(244, 63)
        Me.Button2.TabIndex = 82
        Me.Button2.Text = "Package Date Coder"
        Me.Button2.UseVisualStyleBackColor = False
        Me.Button2.Visible = False
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.Button5.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.Button5.Location = New System.Drawing.Point(286, 160)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(244, 63)
        Me.Button5.TabIndex = 85
        Me.Button5.Text = "Visual Verification"
        Me.Button5.UseVisualStyleBackColor = False
        Me.Button5.Visible = False
        '
        'Button6
        '
        Me.Button6.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.Button6.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.Button6.Location = New System.Drawing.Point(286, 80)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(244, 63)
        Me.Button6.TabIndex = 86
        Me.Button6.Text = "Checkweigher Verification"
        Me.Button6.UseVisualStyleBackColor = False
        Me.Button6.Visible = False
        '
        'Button9
        '
        Me.Button9.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.Button9.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.Button9.Location = New System.Drawing.Point(27, 320)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(244, 63)
        Me.Button9.TabIndex = 89
        Me.Button9.Text = "Verify Case label"
        Me.Button9.UseVisualStyleBackColor = False
        Me.Button9.Visible = False
        '
        'Button10
        '
        Me.Button10.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.Button10.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.Button10.Location = New System.Drawing.Point(544, 160)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(244, 63)
        Me.Button10.TabIndex = 90
        Me.Button10.Text = "Pressure Test"
        Me.Button10.UseVisualStyleBackColor = False
        Me.Button10.Visible = False
        '
        'Button11
        '
        Me.Button11.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.Button11.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.Button11.Location = New System.Drawing.Point(27, 160)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(244, 63)
        Me.Button11.TabIndex = 91
        Me.Button11.Text = "Oxygen Test"
        Me.Button11.UseVisualStyleBackColor = False
        Me.Button11.Visible = False
        '
        'btnVerifyCartonBox
        '
        Me.btnVerifyCartonBox.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnVerifyCartonBox.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnVerifyCartonBox.Location = New System.Drawing.Point(27, 400)
        Me.btnVerifyCartonBox.Name = "btnVerifyCartonBox"
        Me.btnVerifyCartonBox.Size = New System.Drawing.Size(244, 63)
        Me.btnVerifyCartonBox.TabIndex = 92
        Me.btnVerifyCartonBox.Text = "Verify Carton Box"
        Me.btnVerifyCartonBox.UseVisualStyleBackColor = False
        Me.btnVerifyCartonBox.Visible = False
        '
        'frmQATInProcess
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnVerifyCartonBox)
        Me.Controls.Add(Me.Button11)
        Me.Controls.Add(Me.Button10)
        Me.Controls.Add(Me.Button9)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnMaterialValidation)
        Me.Controls.Add(Me.btnPrevious)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmQATInProcess"
        Me.Text = "Get Data From Serial Port"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents btnPrevious As System.Windows.Forms.Button
    Friend WithEvents btnMaterialValidation As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Button11 As System.Windows.Forms.Button
    Friend WithEvents btnVerifyCartonBox As System.Windows.Forms.Button
End Class
