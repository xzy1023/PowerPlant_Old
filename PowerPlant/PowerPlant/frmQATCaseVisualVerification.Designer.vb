<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmQATCaseVisualVerification
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
        Me.btnFail = New System.Windows.Forms.Button()
        Me.btnPass = New System.Windows.Forms.Button()
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.lblUnitPerCase = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lblShopOrder = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnNA = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(0, -1)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "QAT Case Visual Verification"
        Me.UcHeading1.Size = New System.Drawing.Size(800, 55)
        Me.UcHeading1.TabIndex = 15
        '
        'btnFail
        '
        Me.btnFail.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnFail.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFail.Location = New System.Drawing.Point(695, 490)
        Me.btnFail.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnFail.Name = "btnFail"
        Me.btnFail.Size = New System.Drawing.Size(75, 65)
        Me.btnFail.TabIndex = 118
        Me.btnFail.Text = "Fail"
        Me.btnFail.UseVisualStyleBackColor = False
        '
        'btnPass
        '
        Me.btnPass.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnPass.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPass.Location = New System.Drawing.Point(608, 490)
        Me.btnPass.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPass.Name = "btnPass"
        Me.btnPass.Size = New System.Drawing.Size(75, 65)
        Me.btnPass.TabIndex = 117
        Me.btnPass.Text = "Pass"
        Me.btnPass.UseVisualStyleBackColor = False
        '
        'txtNotes
        '
        Me.txtNotes.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtNotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNotes.Location = New System.Drawing.Point(20, 109)
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.ReadOnly = True
        Me.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtNotes.Size = New System.Drawing.Size(755, 369)
        Me.txtNotes.TabIndex = 119
        Me.txtNotes.WordWrap = False
        '
        'lblUnitPerCase
        '
        Me.lblUnitPerCase.AutoSize = True
        Me.lblUnitPerCase.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitPerCase.ForeColor = System.Drawing.Color.White
        Me.lblUnitPerCase.Location = New System.Drawing.Point(705, 73)
        Me.lblUnitPerCase.Name = "lblUnitPerCase"
        Me.lblUnitPerCase.Size = New System.Drawing.Size(25, 27)
        Me.lblUnitPerCase.TabIndex = 123
        Me.lblUnitPerCase.Text = "8"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(521, 73)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(187, 27)
        Me.Label11.TabIndex = 122
        Me.Label11.Text = "Units Per Case: "
        '
        'lblShopOrder
        '
        Me.lblShopOrder.AutoSize = True
        Me.lblShopOrder.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShopOrder.ForeColor = System.Drawing.Color.White
        Me.lblShopOrder.Location = New System.Drawing.Point(162, 73)
        Me.lblShopOrder.Name = "lblShopOrder"
        Me.lblShopOrder.Size = New System.Drawing.Size(116, 27)
        Me.lblShopOrder.TabIndex = 124
        Me.lblShopOrder.Text = "00000000"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(20, 73)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(145, 27)
        Me.Label5.TabIndex = 125
        Me.Label5.Text = "Shop Order:"
        '
        'btnNA
        '
        Me.btnNA.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnNA.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNA.Location = New System.Drawing.Point(520, 490)
        Me.btnNA.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNA.Name = "btnNA"
        Me.btnNA.Size = New System.Drawing.Size(75, 65)
        Me.btnNA.TabIndex = 126
        Me.btnNA.Text = "N/A"
        Me.btnNA.UseVisualStyleBackColor = False
        '
        'frmQATCaseVisualVerification
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnNA)
        Me.Controls.Add(Me.lblShopOrder)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblUnitPerCase)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtNotes)
        Me.Controls.Add(Me.btnFail)
        Me.Controls.Add(Me.btnPass)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmQATCaseVisualVerification"
        Me.Text = "QAT Case Visual Verification"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents btnFail As System.Windows.Forms.Button
    Friend WithEvents btnPass As System.Windows.Forms.Button
    Friend WithEvents txtNotes As System.Windows.Forms.TextBox
    Friend WithEvents lblUnitPerCase As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblShopOrder As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnNA As System.Windows.Forms.Button
End Class
