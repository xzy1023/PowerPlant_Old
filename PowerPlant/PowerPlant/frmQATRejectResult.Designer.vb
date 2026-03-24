<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmQATRejectResult
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnPass = New System.Windows.Forms.Button()
        Me.btnFail = New System.Windows.Forms.Button()
        Me.lblItemNo = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.lblShopOrder = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.btnRetest = New System.Windows.Forms.Button()
        Me.btnNA = New System.Windows.Forms.Button()
        Me.lblRetestNo = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(40, 310)
        Me.Label1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(1036, 141)
        Me.Label1.TabIndex = 80
        Me.Label1.Text = "Remove all capsules for one full layer from the bank " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "and verify that all ventur" &
    "i valves show red on the HMI. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Verify the case rejects. "
        '
        'btnPass
        '
        Me.btnPass.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnPass.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPass.Location = New System.Drawing.Point(1037, 910)
        Me.btnPass.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnPass.Name = "btnPass"
        Me.btnPass.Size = New System.Drawing.Size(138, 111)
        Me.btnPass.TabIndex = 81
        Me.btnPass.Text = "Pass"
        Me.btnPass.UseVisualStyleBackColor = False
        '
        'btnFail
        '
        Me.btnFail.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnFail.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFail.Location = New System.Drawing.Point(1226, 910)
        Me.btnFail.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnFail.Name = "btnFail"
        Me.btnFail.Size = New System.Drawing.Size(138, 111)
        Me.btnFail.TabIndex = 86
        Me.btnFail.Text = "Fail"
        Me.btnFail.UseVisualStyleBackColor = False
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
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(0, -2)
        Me.UcHeading1.Margin = New System.Windows.Forms.Padding(11)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "QAT Reject Result"
        Me.UcHeading1.Size = New System.Drawing.Size(1467, 102)
        Me.UcHeading1.TabIndex = 15
        '
        'btnRetest
        '
        Me.btnRetest.BackColor = System.Drawing.Color.Silver
        Me.btnRetest.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRetest.Location = New System.Drawing.Point(63, 905)
        Me.btnRetest.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnRetest.Name = "btnRetest"
        Me.btnRetest.Size = New System.Drawing.Size(275, 120)
        Me.btnRetest.TabIndex = 122
        Me.btnRetest.Text = "Test After RCA"
        Me.btnRetest.UseVisualStyleBackColor = False
        Me.btnRetest.Visible = False
        '
        'btnNA
        '
        Me.btnNA.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnNA.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNA.Location = New System.Drawing.Point(842, 910)
        Me.btnNA.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnNA.Name = "btnNA"
        Me.btnNA.Size = New System.Drawing.Size(138, 111)
        Me.btnNA.TabIndex = 125
        Me.btnNA.Text = "N/A"
        Me.btnNA.UseVisualStyleBackColor = False
        '
        'lblRetestNo
        '
        Me.lblRetestNo.AutoSize = True
        Me.lblRetestNo.Font = New System.Drawing.Font("Arial", 14.14286!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRetestNo.ForeColor = System.Drawing.Color.White
        Me.lblRetestNo.Location = New System.Drawing.Point(55, 815)
        Me.lblRetestNo.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblRetestNo.Name = "lblRetestNo"
        Me.lblRetestNo.Size = New System.Drawing.Size(178, 38)
        Me.lblRetestNo.TabIndex = 127
        Me.lblRetestNo.Text = "Retest No: "
        '
        'frmQATRejectResult
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(1467, 1108)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblRetestNo)
        Me.Controls.Add(Me.btnNA)
        Me.Controls.Add(Me.btnRetest)
        Me.Controls.Add(Me.lblItemNo)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.lblShopOrder)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.btnFail)
        Me.Controls.Add(Me.btnPass)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(6)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmQATRejectResult"
        Me.Text = "QAT Cameras Verification"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnPass As System.Windows.Forms.Button
    Friend WithEvents btnFail As System.Windows.Forms.Button
    Friend WithEvents lblItemNo As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents lblShopOrder As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btnRetest As Button
    Friend WithEvents btnNA As Button
    Friend WithEvents lblRetestNo As Label
End Class
