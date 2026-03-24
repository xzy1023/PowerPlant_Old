<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmQATCheckWeigherValidation
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
        Me.lblMinWgt = New System.Windows.Forms.Label()
        Me.lblMaxWgt = New System.Windows.Forms.Label()
        Me.lblTargetWgt = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblActualWgt = New System.Windows.Forms.Label()
        Me.btnFail = New System.Windows.Forms.Button()
        Me.btnPass = New System.Windows.Forms.Button()
        Me.lblTareWgt = New System.Windows.Forms.Label()
        Me.lblTareWgtLabel = New System.Windows.Forms.Label()
        Me.lblRecipeName = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.lblShopOrder = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.lblItemNo = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.lblNotes = New System.Windows.Forms.Label()
        Me.btnRetest = New System.Windows.Forms.Button()
        Me.btnOverride = New System.Windows.Forms.Button()
        Me.lblSNNWgt = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnNA = New System.Windows.Forms.Button()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.SuspendLayout()
        '
        'lblMinWgt
        '
        Me.lblMinWgt.AutoSize = True
        Me.lblMinWgt.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMinWgt.ForeColor = System.Drawing.Color.White
        Me.lblMinWgt.Location = New System.Drawing.Point(903, 238)
        Me.lblMinWgt.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMinWgt.Name = "lblMinWgt"
        Me.lblMinWgt.Size = New System.Drawing.Size(91, 35)
        Me.lblMinWgt.TabIndex = 101
        Me.lblMinWgt.Text = "00.00"
        '
        'lblMaxWgt
        '
        Me.lblMaxWgt.AutoSize = True
        Me.lblMaxWgt.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaxWgt.ForeColor = System.Drawing.Color.White
        Me.lblMaxWgt.Location = New System.Drawing.Point(564, 238)
        Me.lblMaxWgt.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMaxWgt.Name = "lblMaxWgt"
        Me.lblMaxWgt.Size = New System.Drawing.Size(91, 35)
        Me.lblMaxWgt.TabIndex = 100
        Me.lblMaxWgt.Text = "00.00"
        '
        'lblTargetWgt
        '
        Me.lblTargetWgt.AutoSize = True
        Me.lblTargetWgt.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTargetWgt.ForeColor = System.Drawing.Color.White
        Me.lblTargetWgt.Location = New System.Drawing.Point(216, 238)
        Me.lblTargetWgt.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTargetWgt.Name = "lblTargetWgt"
        Me.lblTargetWgt.Size = New System.Drawing.Size(91, 35)
        Me.lblTargetWgt.TabIndex = 99
        Me.lblTargetWgt.Text = "00.00"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(27, 187)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(176, 35)
        Me.Label6.TabIndex = 97
        Me.Label6.Text = "Actual Wgt.:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(741, 238)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(151, 35)
        Me.Label3.TabIndex = 96
        Me.Label3.Text = "Min. Wgt.:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(400, 238)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(157, 35)
        Me.Label2.TabIndex = 95
        Me.Label2.Text = "Max. Wgt.:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(27, 238)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(177, 35)
        Me.Label1.TabIndex = 94
        Me.Label1.Text = "Target Wgt.:"
        '
        'lblActualWgt
        '
        Me.lblActualWgt.AutoSize = True
        Me.lblActualWgt.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActualWgt.ForeColor = System.Drawing.Color.White
        Me.lblActualWgt.Location = New System.Drawing.Point(216, 187)
        Me.lblActualWgt.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblActualWgt.Name = "lblActualWgt"
        Me.lblActualWgt.Size = New System.Drawing.Size(91, 35)
        Me.lblActualWgt.TabIndex = 103
        Me.lblActualWgt.Text = "00.00"
        '
        'btnFail
        '
        Me.btnFail.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnFail.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFail.Location = New System.Drawing.Point(939, 603)
        Me.btnFail.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnFail.Name = "btnFail"
        Me.btnFail.Size = New System.Drawing.Size(100, 80)
        Me.btnFail.TabIndex = 105
        Me.btnFail.Text = "Fail"
        Me.btnFail.UseVisualStyleBackColor = False
        '
        'btnPass
        '
        Me.btnPass.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnPass.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPass.Location = New System.Drawing.Point(831, 603)
        Me.btnPass.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnPass.Name = "btnPass"
        Me.btnPass.Size = New System.Drawing.Size(100, 80)
        Me.btnPass.TabIndex = 104
        Me.btnPass.Text = "Pass"
        Me.btnPass.UseVisualStyleBackColor = False
        '
        'lblTareWgt
        '
        Me.lblTareWgt.AutoSize = True
        Me.lblTareWgt.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTareWgt.ForeColor = System.Drawing.Color.White
        Me.lblTareWgt.Location = New System.Drawing.Point(564, 187)
        Me.lblTareWgt.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTareWgt.Name = "lblTareWgt"
        Me.lblTareWgt.Size = New System.Drawing.Size(91, 35)
        Me.lblTareWgt.TabIndex = 111
        Me.lblTareWgt.Text = "00.00"
        '
        'lblTareWgtLabel
        '
        Me.lblTareWgtLabel.AutoSize = True
        Me.lblTareWgtLabel.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTareWgtLabel.ForeColor = System.Drawing.Color.White
        Me.lblTareWgtLabel.Location = New System.Drawing.Point(400, 187)
        Me.lblTareWgtLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTareWgtLabel.Name = "lblTareWgtLabel"
        Me.lblTareWgtLabel.Size = New System.Drawing.Size(152, 35)
        Me.lblTareWgtLabel.TabIndex = 110
        Me.lblTareWgtLabel.Text = "Tare Wgt.:"
        '
        'lblRecipeName
        '
        Me.lblRecipeName.AutoSize = True
        Me.lblRecipeName.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecipeName.ForeColor = System.Drawing.Color.White
        Me.lblRecipeName.Location = New System.Drawing.Point(216, 135)
        Me.lblRecipeName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRecipeName.Name = "lblRecipeName"
        Me.lblRecipeName.Size = New System.Drawing.Size(85, 35)
        Me.lblRecipeName.TabIndex = 113
        Me.lblRecipeName.Text = "-------"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.White
        Me.Label15.Location = New System.Drawing.Point(27, 135)
        Me.Label15.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(116, 35)
        Me.Label15.TabIndex = 112
        Me.Label15.Text = "Recipe:"
        '
        'lblShopOrder
        '
        Me.lblShopOrder.AutoSize = True
        Me.lblShopOrder.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShopOrder.ForeColor = System.Drawing.Color.White
        Me.lblShopOrder.Location = New System.Drawing.Point(216, 85)
        Me.lblShopOrder.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblShopOrder.Name = "lblShopOrder"
        Me.lblShopOrder.Size = New System.Drawing.Size(151, 35)
        Me.lblShopOrder.TabIndex = 114
        Me.lblShopOrder.Text = "00000000"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(27, 85)
        Me.Label16.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(177, 35)
        Me.Label16.TabIndex = 115
        Me.Label16.Text = "Shop Order:"
        '
        'lblItemNo
        '
        Me.lblItemNo.AutoSize = True
        Me.lblItemNo.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemNo.ForeColor = System.Drawing.Color.White
        Me.lblItemNo.Location = New System.Drawing.Point(623, 85)
        Me.lblItemNo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblItemNo.Name = "lblItemNo"
        Me.lblItemNo.Size = New System.Drawing.Size(102, 35)
        Me.lblItemNo.TabIndex = 117
        Me.lblItemNo.Text = "SKU #"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.White
        Me.Label17.Location = New System.Drawing.Point(400, 85)
        Me.Label17.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(200, 35)
        Me.Label17.TabIndex = 116
        Me.Label17.Text = "SKU Number:"
        '
        'txtNotes
        '
        Me.txtNotes.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtNotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNotes.Location = New System.Drawing.Point(27, 331)
        Me.txtNotes.Margin = New System.Windows.Forms.Padding(4)
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.ReadOnly = True
        Me.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtNotes.Size = New System.Drawing.Size(1005, 253)
        Me.txtNotes.TabIndex = 120
        Me.txtNotes.Visible = False
        Me.txtNotes.WordWrap = False
        '
        'lblNotes
        '
        Me.lblNotes.AutoSize = True
        Me.lblNotes.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNotes.ForeColor = System.Drawing.Color.White
        Me.lblNotes.Location = New System.Drawing.Point(27, 294)
        Me.lblNotes.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNotes.Name = "lblNotes"
        Me.lblNotes.Size = New System.Drawing.Size(100, 35)
        Me.lblNotes.TabIndex = 102
        Me.lblNotes.Text = "Notes:"
        Me.lblNotes.Visible = False
        '
        'btnRetest
        '
        Me.btnRetest.BackColor = System.Drawing.Color.Silver
        Me.btnRetest.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRetest.Location = New System.Drawing.Point(27, 603)
        Me.btnRetest.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnRetest.Name = "btnRetest"
        Me.btnRetest.Size = New System.Drawing.Size(200, 80)
        Me.btnRetest.TabIndex = 121
        Me.btnRetest.Text = "Test After RCA"
        Me.btnRetest.UseVisualStyleBackColor = False
        Me.btnRetest.Visible = False
        '
        'btnOverride
        '
        Me.btnOverride.BackColor = System.Drawing.Color.Gold
        Me.btnOverride.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnOverride.ForeColor = System.Drawing.Color.Crimson
        Me.btnOverride.Location = New System.Drawing.Point(512, 603)
        Me.btnOverride.Margin = New System.Windows.Forms.Padding(4)
        Me.btnOverride.Name = "btnOverride"
        Me.btnOverride.Size = New System.Drawing.Size(200, 80)
        Me.btnOverride.TabIndex = 159
        Me.btnOverride.Text = "Override"
        Me.btnOverride.UseVisualStyleBackColor = False
        Me.btnOverride.Visible = False
        '
        'lblSNNWgt
        '
        Me.lblSNNWgt.AutoSize = True
        Me.lblSNNWgt.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSNNWgt.ForeColor = System.Drawing.Color.White
        Me.lblSNNWgt.Location = New System.Drawing.Point(903, 187)
        Me.lblSNNWgt.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSNNWgt.Name = "lblSNNWgt"
        Me.lblSNNWgt.Size = New System.Drawing.Size(91, 35)
        Me.lblSNNWgt.TabIndex = 161
        Me.lblSNNWgt.Text = "00.00"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(741, 187)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(148, 35)
        Me.Label5.TabIndex = 160
        Me.Label5.Text = "Unit Wgt.:"
        '
        'btnNA
        '
        Me.btnNA.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnNA.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNA.Location = New System.Drawing.Point(721, 603)
        Me.btnNA.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnNA.Name = "btnNA"
        Me.btnNA.Size = New System.Drawing.Size(100, 80)
        Me.btnNA.TabIndex = 162
        Me.btnNA.Text = "N/A"
        Me.btnNA.UseVisualStyleBackColor = False
        Me.btnNA.Visible = False
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(0, -1)
        Me.UcHeading1.Margin = New System.Windows.Forms.Padding(5)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "QAT Checkweigher"
        Me.UcHeading1.Size = New System.Drawing.Size(1067, 68)
        Me.UcHeading1.TabIndex = 15
        '
        'frmQATCheckWeigherValidation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(1067, 738)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnNA)
        Me.Controls.Add(Me.lblSNNWgt)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnOverride)
        Me.Controls.Add(Me.btnRetest)
        Me.Controls.Add(Me.txtNotes)
        Me.Controls.Add(Me.lblItemNo)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.lblShopOrder)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.lblRecipeName)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.lblTareWgt)
        Me.Controls.Add(Me.lblTareWgtLabel)
        Me.Controls.Add(Me.btnFail)
        Me.Controls.Add(Me.btnPass)
        Me.Controls.Add(Me.lblActualWgt)
        Me.Controls.Add(Me.lblNotes)
        Me.Controls.Add(Me.lblMinWgt)
        Me.Controls.Add(Me.lblMaxWgt)
        Me.Controls.Add(Me.lblTargetWgt)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmQATCheckWeigherValidation"
        Me.Text = "QAT Checkweigher"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents lblMinWgt As System.Windows.Forms.Label
    Friend WithEvents lblMaxWgt As System.Windows.Forms.Label
    Friend WithEvents lblTargetWgt As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblActualWgt As System.Windows.Forms.Label
    Friend WithEvents btnFail As System.Windows.Forms.Button
    Friend WithEvents btnPass As System.Windows.Forms.Button
    Friend WithEvents lblTareWgt As System.Windows.Forms.Label
    Friend WithEvents lblTareWgtLabel As System.Windows.Forms.Label
    Friend WithEvents lblRecipeName As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents lblShopOrder As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents lblItemNo As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txtNotes As System.Windows.Forms.TextBox
    Friend WithEvents lblNotes As System.Windows.Forms.Label
    Friend WithEvents btnRetest As System.Windows.Forms.Button
    Friend WithEvents btnOverride As System.Windows.Forms.Button
    Friend WithEvents lblSNNWgt As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnNA As System.Windows.Forms.Button
End Class
