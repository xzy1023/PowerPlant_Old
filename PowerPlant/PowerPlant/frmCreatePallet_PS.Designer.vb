<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreatePallet_PS
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
        Me.lblItemNo = New System.Windows.Forms.Label()
        Me.lblItemDesc = New System.Windows.Forms.Label()
        Me.lblSKUNo = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnPalletNotFull = New System.Windows.Forms.Button()
        Me.btnPalletFull = New System.Windows.Forms.Button()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtCasesInPallet = New System.Windows.Forms.TextBox()
        Me.lblCasesInPallet = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnLastPallet = New System.Windows.Forms.Button()
        Me.btnNotLastPallet = New System.Windows.Forms.Button()
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.btnCrtPrtPallet = New System.Windows.Forms.Button()
        Me.txtShopOrder = New System.Windows.Forms.TextBox()
        Me.lblQtyPerPallet = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtPkgLine = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtProdDay = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtProdMonth = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtProdYear = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblPalletMsg = New System.Windows.Forms.Label()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.lblPkgLine = New System.Windows.Forms.Label()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblItemNo
        '
        Me.lblItemNo.AutoSize = True
        Me.lblItemNo.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemNo.ForeColor = System.Drawing.Color.White
        Me.lblItemNo.Location = New System.Drawing.Point(496, 71)
        Me.lblItemNo.Name = "lblItemNo"
        Me.lblItemNo.Size = New System.Drawing.Size(81, 27)
        Me.lblItemNo.TabIndex = 59
        Me.lblItemNo.Text = "SKU #"
        '
        'lblItemDesc
        '
        Me.lblItemDesc.AutoSize = True
        Me.lblItemDesc.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemDesc.ForeColor = System.Drawing.Color.White
        Me.lblItemDesc.Location = New System.Drawing.Point(329, 104)
        Me.lblItemDesc.Name = "lblItemDesc"
        Me.lblItemDesc.Size = New System.Drawing.Size(185, 27)
        Me.lblItemDesc.TabIndex = 57
        Me.lblItemDesc.Text = "Item Description"
        '
        'lblSKUNo
        '
        Me.lblSKUNo.AutoSize = True
        Me.lblSKUNo.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSKUNo.ForeColor = System.Drawing.Color.White
        Me.lblSKUNo.Location = New System.Drawing.Point(329, 71)
        Me.lblSKUNo.Name = "lblSKUNo"
        Me.lblSKUNo.Size = New System.Drawing.Size(161, 27)
        Me.lblSKUNo.TabIndex = 58
        Me.lblSKUNo.Text = "SKU Number:"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(31, 71)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(145, 27)
        Me.Label16.TabIndex = 56
        Me.Label16.Text = "Shop Order:"
        '
        'btnPalletNotFull
        '
        Me.btnPalletNotFull.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnPalletNotFull.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold)
        Me.btnPalletNotFull.Location = New System.Drawing.Point(410, 315)
        Me.btnPalletNotFull.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPalletNotFull.Name = "btnPalletNotFull"
        Me.btnPalletNotFull.Size = New System.Drawing.Size(80, 60)
        Me.btnPalletNotFull.TabIndex = 71
        Me.btnPalletNotFull.Text = "No"
        Me.btnPalletNotFull.UseVisualStyleBackColor = False
        '
        'btnPalletFull
        '
        Me.btnPalletFull.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnPalletFull.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold)
        Me.btnPalletFull.Location = New System.Drawing.Point(313, 315)
        Me.btnPalletFull.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPalletFull.Name = "btnPalletFull"
        Me.btnPalletFull.Size = New System.Drawing.Size(80, 60)
        Me.btnPalletFull.TabIndex = 72
        Me.btnPalletFull.Text = "Yes"
        Me.btnPalletFull.UseVisualStyleBackColor = False
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.White
        Me.Label19.Location = New System.Drawing.Point(33, 333)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(214, 27)
        Me.Label19.TabIndex = 70
        Me.Label19.Text = "Is this a full pallet?"
        '
        'txtCasesInPallet
        '
        Me.txtCasesInPallet.BackColor = System.Drawing.Color.Black
        Me.txtCasesInPallet.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCasesInPallet.ForeColor = System.Drawing.Color.White
        Me.txtCasesInPallet.Location = New System.Drawing.Point(343, 391)
        Me.txtCasesInPallet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtCasesInPallet.MaxLength = 6
        Me.txtCasesInPallet.Name = "txtCasesInPallet"
        Me.txtCasesInPallet.Size = New System.Drawing.Size(86, 35)
        Me.txtCasesInPallet.TabIndex = 74
        '
        'lblCasesInPallet
        '
        Me.lblCasesInPallet.AutoSize = True
        Me.lblCasesInPallet.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCasesInPallet.ForeColor = System.Drawing.Color.White
        Me.lblCasesInPallet.Location = New System.Drawing.Point(31, 394)
        Me.lblCasesInPallet.Name = "lblCasesInPallet"
        Me.lblCasesInPallet.Size = New System.Drawing.Size(297, 27)
        Me.lblCasesInPallet.TabIndex = 73
        Me.lblCasesInPallet.Text = "No. of cases on this pallet:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(29, 260)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(279, 27)
        Me.Label1.TabIndex = 70
        Me.Label1.Text = "Last pallet for this order?"
        '
        'btnLastPallet
        '
        Me.btnLastPallet.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnLastPallet.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold)
        Me.btnLastPallet.Location = New System.Drawing.Point(313, 242)
        Me.btnLastPallet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnLastPallet.Name = "btnLastPallet"
        Me.btnLastPallet.Size = New System.Drawing.Size(80, 60)
        Me.btnLastPallet.TabIndex = 72
        Me.btnLastPallet.Text = "Yes"
        Me.btnLastPallet.UseVisualStyleBackColor = False
        '
        'btnNotLastPallet
        '
        Me.btnNotLastPallet.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnNotLastPallet.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold)
        Me.btnNotLastPallet.Location = New System.Drawing.Point(410, 242)
        Me.btnNotLastPallet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNotLastPallet.Name = "btnNotLastPallet"
        Me.btnNotLastPallet.Size = New System.Drawing.Size(80, 60)
        Me.btnNotLastPallet.TabIndex = 71
        Me.btnNotLastPallet.Text = "No"
        Me.btnNotLastPallet.UseVisualStyleBackColor = False
        '
        'btnPrvScn
        '
        Me.btnPrvScn.BackColor = System.Drawing.Color.Silver
        Me.btnPrvScn.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrvScn.Location = New System.Drawing.Point(27, 490)
        Me.btnPrvScn.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrvScn.Name = "btnPrvScn"
        Me.btnPrvScn.Size = New System.Drawing.Size(150, 65)
        Me.btnPrvScn.TabIndex = 76
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'btnCrtPrtPallet
        '
        Me.btnCrtPrtPallet.BackColor = System.Drawing.Color.Silver
        Me.btnCrtPrtPallet.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnCrtPrtPallet.Location = New System.Drawing.Point(207, 490)
        Me.btnCrtPrtPallet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCrtPrtPallet.Name = "btnCrtPrtPallet"
        Me.btnCrtPrtPallet.Size = New System.Drawing.Size(150, 65)
        Me.btnCrtPrtPallet.TabIndex = 75
        Me.btnCrtPrtPallet.Text = "Create Pallet"
        Me.btnCrtPrtPallet.UseVisualStyleBackColor = False
        '
        'txtShopOrder
        '
        Me.txtShopOrder.BackColor = System.Drawing.Color.Black
        Me.txtShopOrder.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShopOrder.ForeColor = System.Drawing.Color.White
        Me.txtShopOrder.Location = New System.Drawing.Point(182, 68)
        Me.txtShopOrder.MaxLength = 10
        Me.txtShopOrder.Name = "txtShopOrder"
        Me.txtShopOrder.Size = New System.Drawing.Size(129, 35)
        Me.txtShopOrder.TabIndex = 77
        '
        'lblQtyPerPallet
        '
        Me.lblQtyPerPallet.AutoSize = True
        Me.lblQtyPerPallet.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblQtyPerPallet.ForeColor = System.Drawing.Color.White
        Me.lblQtyPerPallet.Location = New System.Drawing.Point(699, 333)
        Me.lblQtyPerPallet.Name = "lblQtyPerPallet"
        Me.lblQtyPerPallet.Size = New System.Drawing.Size(25, 27)
        Me.lblQtyPerPallet.TabIndex = 79
        Me.lblQtyPerPallet.Text = "0"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(522, 333)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(171, 27)
        Me.Label2.TabIndex = 78
        Me.Label2.Text = "Full pallet Qty:"
        '
        'txtPkgLine
        '
        Me.txtPkgLine.BackColor = System.Drawing.Color.Black
        Me.txtPkgLine.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPkgLine.ForeColor = System.Drawing.Color.White
        Me.txtPkgLine.Location = New System.Drawing.Point(221, 141)
        Me.txtPkgLine.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.txtPkgLine.MaxLength = 10
        Me.txtPkgLine.Name = "txtPkgLine"
        Me.txtPkgLine.Size = New System.Drawing.Size(147, 35)
        Me.txtPkgLine.TabIndex = 80
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(33, 144)
        Me.Label7.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(184, 27)
        Me.Label7.TabIndex = 81
        Me.Label7.Text = "Packaging Line:"
        '
        'txtProdDay
        '
        Me.txtProdDay.BackColor = System.Drawing.Color.Black
        Me.txtProdDay.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtProdDay.ForeColor = System.Drawing.Color.White
        Me.txtProdDay.Location = New System.Drawing.Point(675, 191)
        Me.txtProdDay.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtProdDay.MaxLength = 2
        Me.txtProdDay.Name = "txtProdDay"
        Me.txtProdDay.Size = New System.Drawing.Size(51, 35)
        Me.txtProdDay.TabIndex = 88
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(599, 194)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(76, 27)
        Me.Label5.TabIndex = 87
        Me.Label5.Text = "Day - "
        '
        'txtProdMonth
        '
        Me.txtProdMonth.BackColor = System.Drawing.Color.Black
        Me.txtProdMonth.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtProdMonth.ForeColor = System.Drawing.Color.White
        Me.txtProdMonth.Location = New System.Drawing.Point(525, 191)
        Me.txtProdMonth.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtProdMonth.MaxLength = 2
        Me.txtProdMonth.Name = "txtProdMonth"
        Me.txtProdMonth.Size = New System.Drawing.Size(51, 35)
        Me.txtProdMonth.TabIndex = 86
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(422, 194)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(101, 27)
        Me.Label4.TabIndex = 85
        Me.Label4.Text = "Month - "
        '
        'txtProdYear
        '
        Me.txtProdYear.BackColor = System.Drawing.Color.Black
        Me.txtProdYear.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtProdYear.ForeColor = System.Drawing.Color.White
        Me.txtProdYear.Location = New System.Drawing.Point(316, 191)
        Me.txtProdYear.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtProdYear.MaxLength = 4
        Me.txtProdYear.Name = "txtProdYear"
        Me.txtProdYear.Size = New System.Drawing.Size(84, 35)
        Me.txtProdYear.TabIndex = 84
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(231, 194)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(82, 27)
        Me.Label3.TabIndex = 82
        Me.Label3.Text = "Year - "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(33, 194)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(192, 27)
        Me.Label6.TabIndex = 83
        Me.Label6.Text = "Production Date:"
        '
        'lblPalletMsg
        '
        Me.lblPalletMsg.AutoSize = True
        Me.lblPalletMsg.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPalletMsg.ForeColor = System.Drawing.Color.DarkGreen
        Me.lblPalletMsg.Location = New System.Drawing.Point(120, 449)
        Me.lblPalletMsg.Name = "lblPalletMsg"
        Me.lblPalletMsg.Size = New System.Drawing.Size(354, 27)
        Me.lblPalletMsg.TabIndex = 73
        Me.lblPalletMsg.Text = "Pallet has been created with ID:"
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
        'lblPkgLine
        '
        Me.lblPkgLine.AutoSize = True
        Me.lblPkgLine.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPkgLine.ForeColor = System.Drawing.Color.White
        Me.lblPkgLine.Location = New System.Drawing.Point(376, 144)
        Me.lblPkgLine.Name = "lblPkgLine"
        Me.lblPkgLine.Size = New System.Drawing.Size(75, 27)
        Me.lblPkgLine.TabIndex = 57
        Me.lblPkgLine.Text = "Name"
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
        'frmCreatePallet_PS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.txtProdDay)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtProdMonth)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtProdYear)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtPkgLine)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.lblQtyPerPallet)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtShopOrder)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.btnCrtPrtPallet)
        Me.Controls.Add(Me.txtCasesInPallet)
        Me.Controls.Add(Me.lblPalletMsg)
        Me.Controls.Add(Me.lblCasesInPallet)
        Me.Controls.Add(Me.btnNotLastPallet)
        Me.Controls.Add(Me.btnPalletNotFull)
        Me.Controls.Add(Me.btnLastPallet)
        Me.Controls.Add(Me.btnPalletFull)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.lblItemNo)
        Me.Controls.Add(Me.lblPkgLine)
        Me.Controls.Add(Me.lblItemDesc)
        Me.Controls.Add(Me.lblSKUNo)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCreatePallet_PS"
        Me.Text = "Create Pallet"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents lblItemNo As System.Windows.Forms.Label
    Friend WithEvents lblItemDesc As System.Windows.Forms.Label
    Friend WithEvents lblSKUNo As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btnPalletNotFull As System.Windows.Forms.Button
    Friend WithEvents btnPalletFull As System.Windows.Forms.Button
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtCasesInPallet As System.Windows.Forms.TextBox
    Friend WithEvents lblCasesInPallet As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnLastPallet As System.Windows.Forms.Button
    Friend WithEvents btnNotLastPallet As System.Windows.Forms.Button
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents btnCrtPrtPallet As System.Windows.Forms.Button
    Friend WithEvents txtShopOrder As System.Windows.Forms.TextBox
    Friend WithEvents lblQtyPerPallet As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtPkgLine As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtProdDay As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtProdMonth As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtProdYear As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblPalletMsg As System.Windows.Forms.Label
    Friend WithEvents lblPkgLine As System.Windows.Forms.Label
    Friend WithEvents lblMessage As System.Windows.Forms.Label
End Class
