<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmQATCamerasVerification
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
        Me.lblItemNo = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.lblShopOrder = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnAccept = New System.Windows.Forms.Button()
        Me.btnMinWidthFail = New System.Windows.Forms.Button()
        Me.btnMinWidthPass = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.SuspendLayout()
        '
        'lblItemNo
        '
        Me.lblItemNo.AutoSize = True
        Me.lblItemNo.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemNo.ForeColor = System.Drawing.Color.White
        Me.lblItemNo.Location = New System.Drawing.Point(858, 162)
        Me.lblItemNo.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblItemNo.Name = "lblItemNo"
        Me.lblItemNo.Size = New System.Drawing.Size(141, 47)
        Me.lblItemNo.TabIndex = 121
        Me.lblItemNo.Text = "SKU #"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.White
        Me.Label17.Location = New System.Drawing.Point(552, 162)
        Me.Label17.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(279, 47)
        Me.Label17.TabIndex = 120
        Me.Label17.Text = "SKU Number:"
        '
        'lblShopOrder
        '
        Me.lblShopOrder.AutoSize = True
        Me.lblShopOrder.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShopOrder.ForeColor = System.Drawing.Color.White
        Me.lblShopOrder.Location = New System.Drawing.Point(297, 162)
        Me.lblShopOrder.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblShopOrder.Name = "lblShopOrder"
        Me.lblShopOrder.Size = New System.Drawing.Size(204, 47)
        Me.lblShopOrder.TabIndex = 118
        Me.lblShopOrder.Text = "00000000"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(40, 162)
        Me.Label16.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(248, 47)
        Me.Label16.TabIndex = 119
        Me.Label16.Text = "Shop Order:"
        '
        'btnAccept
        '
        Me.btnAccept.BackColor = System.Drawing.Color.Silver
        Me.btnAccept.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAccept.Location = New System.Drawing.Point(50, 905)
        Me.btnAccept.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(275, 120)
        Me.btnAccept.TabIndex = 122
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = False
        '
        'btnMinWidthFail
        '
        Me.btnMinWidthFail.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnMinWidthFail.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMinWidthFail.Location = New System.Drawing.Point(1181, 445)
        Me.btnMinWidthFail.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnMinWidthFail.Name = "btnMinWidthFail"
        Me.btnMinWidthFail.Size = New System.Drawing.Size(138, 111)
        Me.btnMinWidthFail.TabIndex = 125
        Me.btnMinWidthFail.Text = "Fail"
        Me.btnMinWidthFail.UseVisualStyleBackColor = False
        '
        'btnMinWidthPass
        '
        Me.btnMinWidthPass.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnMinWidthPass.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMinWidthPass.Location = New System.Drawing.Point(992, 445)
        Me.btnMinWidthPass.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnMinWidthPass.Name = "btnMinWidthPass"
        Me.btnMinWidthPass.Size = New System.Drawing.Size(138, 111)
        Me.btnMinWidthPass.TabIndex = 124
        Me.btnMinWidthPass.Text = "Pass"
        Me.btnMinWidthPass.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(40, 478)
        Me.Label2.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(805, 47)
        Me.Label2.TabIndex = 123
        Me.Label2.Text = "Min width check on left and right cameras:"
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(0, -2)
        Me.UcHeading1.Margin = New System.Windows.Forms.Padding(11, 11, 11, 11)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "QAT  Cameras Verification"
        Me.UcHeading1.Size = New System.Drawing.Size(1467, 102)
        Me.UcHeading1.TabIndex = 15
        '
        'frmQATCamerasVerification
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(1467, 1108)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnMinWidthFail)
        Me.Controls.Add(Me.btnMinWidthPass)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.lblItemNo)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.lblShopOrder)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmQATCamerasVerification"
        Me.Text = "QAT Cameras Verification"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents lblItemNo As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents lblShopOrder As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents btnMinWidthFail As Button
    Friend WithEvents btnMinWidthPass As Button
    Friend WithEvents Label2 As Label
End Class
