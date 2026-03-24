<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreatePallet
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
        Me.Label17 = New System.Windows.Forms.Label()
        Me.lblShopOrder = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.lblCaseScheduled = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblCasesRemain = New System.Windows.Forms.Label()
        Me.btnPalletNotFull = New System.Windows.Forms.Button()
        Me.btnPalletFull = New System.Windows.Forms.Button()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtCasesInPallet = New System.Windows.Forms.TextBox()
        Me.lblCasesInPallet = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnLastPallet = New System.Windows.Forms.Button()
        Me.btnNotLastPallet = New System.Windows.Forms.Button()
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.btnCreatePallet = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblQtyPerPallet = New System.Windows.Forms.Label()
        Me.lblTotalScheduled = New System.Windows.Forms.Label()
        Me.lblTotalRemain = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.txtExpiryDay = New System.Windows.Forms.TextBox()
        Me.lblExpiryDay = New System.Windows.Forms.Label()
        Me.txtExpiryMonth = New System.Windows.Forms.TextBox()
        Me.lblExpiryMonth = New System.Windows.Forms.Label()
        Me.txtExpiryYear = New System.Windows.Forms.TextBox()
        Me.lblExpiryYear = New System.Windows.Forms.Label()
        Me.lblExpiryDate = New System.Windows.Forms.Label()
        Me.lblOutputLocation = New System.Windows.Forms.Label()
        Me.cboOutputLocation = New System.Windows.Forms.ComboBox()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.SuspendLayout()
        '
        'lblItemNo
        '
        Me.lblItemNo.AutoSize = True
        Me.lblItemNo.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.lblItemNo.ForeColor = System.Drawing.Color.White
        Me.lblItemNo.Location = New System.Drawing.Point(477, 68)
        Me.lblItemNo.Name = "lblItemNo"
        Me.lblItemNo.Size = New System.Drawing.Size(81, 27)
        Me.lblItemNo.TabIndex = 59
        Me.lblItemNo.Text = "SKU #"
        '
        'lblItemDesc
        '
        Me.lblItemDesc.AutoSize = True
        Me.lblItemDesc.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.lblItemDesc.ForeColor = System.Drawing.Color.White
        Me.lblItemDesc.Location = New System.Drawing.Point(310, 99)
        Me.lblItemDesc.Name = "lblItemDesc"
        Me.lblItemDesc.Size = New System.Drawing.Size(185, 27)
        Me.lblItemDesc.TabIndex = 57
        Me.lblItemDesc.Text = "Item Description"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.White
        Me.Label17.Location = New System.Drawing.Point(310, 68)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(161, 27)
        Me.Label17.TabIndex = 58
        Me.Label17.Text = "SKU Number:"
        '
        'lblShopOrder
        '
        Me.lblShopOrder.AutoSize = True
        Me.lblShopOrder.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.lblShopOrder.ForeColor = System.Drawing.Color.White
        Me.lblShopOrder.Location = New System.Drawing.Point(182, 68)
        Me.lblShopOrder.Name = "lblShopOrder"
        Me.lblShopOrder.Size = New System.Drawing.Size(103, 27)
        Me.lblShopOrder.TabIndex = 55
        Me.lblShopOrder.Text = "0000000"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(31, 68)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(145, 27)
        Me.Label16.TabIndex = 56
        Me.Label16.Text = "Shop Order:"
        '
        'lblCaseScheduled
        '
        Me.lblCaseScheduled.AutoSize = True
        Me.lblCaseScheduled.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.lblCaseScheduled.ForeColor = System.Drawing.Color.White
        Me.lblCaseScheduled.Location = New System.Drawing.Point(286, 163)
        Me.lblCaseScheduled.Name = "lblCaseScheduled"
        Me.lblCaseScheduled.Size = New System.Drawing.Size(84, 27)
        Me.lblCaseScheduled.TabIndex = 61
        Me.lblCaseScheduled.Text = "000.00"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(31, 163)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(135, 27)
        Me.Label6.TabIndex = 60
        Me.Label6.Text = "Scheduled:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(31, 196)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(101, 27)
        Me.Label4.TabIndex = 62
        Me.Label4.Text = "Remain:"
        '
        'lblCasesRemain
        '
        Me.lblCasesRemain.AutoSize = True
        Me.lblCasesRemain.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.lblCasesRemain.ForeColor = System.Drawing.Color.White
        Me.lblCasesRemain.Location = New System.Drawing.Point(286, 196)
        Me.lblCasesRemain.Name = "lblCasesRemain"
        Me.lblCasesRemain.Size = New System.Drawing.Size(84, 27)
        Me.lblCasesRemain.TabIndex = 61
        Me.lblCasesRemain.Text = "000.00"
        '
        'btnPalletNotFull
        '
        Me.btnPalletNotFull.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnPalletNotFull.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPalletNotFull.Location = New System.Drawing.Point(408, 312)
        Me.btnPalletNotFull.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPalletNotFull.Name = "btnPalletNotFull"
        Me.btnPalletNotFull.Size = New System.Drawing.Size(75, 60)
        Me.btnPalletNotFull.TabIndex = 4
        Me.btnPalletNotFull.Text = "No"
        Me.btnPalletNotFull.UseVisualStyleBackColor = False
        '
        'btnPalletFull
        '
        Me.btnPalletFull.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnPalletFull.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPalletFull.Location = New System.Drawing.Point(315, 312)
        Me.btnPalletFull.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPalletFull.Name = "btnPalletFull"
        Me.btnPalletFull.Size = New System.Drawing.Size(75, 60)
        Me.btnPalletFull.TabIndex = 3
        Me.btnPalletFull.Text = "Yes"
        Me.btnPalletFull.UseVisualStyleBackColor = False
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.White
        Me.Label19.Location = New System.Drawing.Point(34, 330)
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
        Me.txtCasesInPallet.Location = New System.Drawing.Point(334, 389)
        Me.txtCasesInPallet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtCasesInPallet.MaxLength = 6
        Me.txtCasesInPallet.Name = "txtCasesInPallet"
        Me.txtCasesInPallet.Size = New System.Drawing.Size(86, 35)
        Me.txtCasesInPallet.TabIndex = 5
        '
        'lblCasesInPallet
        '
        Me.lblCasesInPallet.AutoSize = True
        Me.lblCasesInPallet.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCasesInPallet.ForeColor = System.Drawing.Color.White
        Me.lblCasesInPallet.Location = New System.Drawing.Point(31, 397)
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
        Me.Label1.Location = New System.Drawing.Point(30, 254)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(279, 27)
        Me.Label1.TabIndex = 70
        Me.Label1.Text = "Last pallet for this order?"
        '
        'btnLastPallet
        '
        Me.btnLastPallet.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnLastPallet.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLastPallet.Location = New System.Drawing.Point(315, 236)
        Me.btnLastPallet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnLastPallet.Name = "btnLastPallet"
        Me.btnLastPallet.Size = New System.Drawing.Size(75, 60)
        Me.btnLastPallet.TabIndex = 1
        Me.btnLastPallet.Text = "Yes"
        Me.btnLastPallet.UseVisualStyleBackColor = False
        '
        'btnNotLastPallet
        '
        Me.btnNotLastPallet.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnNotLastPallet.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNotLastPallet.Location = New System.Drawing.Point(411, 236)
        Me.btnNotLastPallet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNotLastPallet.Name = "btnNotLastPallet"
        Me.btnNotLastPallet.Size = New System.Drawing.Size(75, 60)
        Me.btnNotLastPallet.TabIndex = 2
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
        Me.btnPrvScn.TabIndex = 6
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'btnCreatePallet
        '
        Me.btnCreatePallet.BackColor = System.Drawing.Color.Silver
        Me.btnCreatePallet.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnCreatePallet.Location = New System.Drawing.Point(207, 490)
        Me.btnCreatePallet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCreatePallet.Name = "btnCreatePallet"
        Me.btnCreatePallet.Size = New System.Drawing.Size(150, 65)
        Me.btnCreatePallet.TabIndex = 7
        Me.btnCreatePallet.Text = "Create Pallet"
        Me.btnCreatePallet.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(515, 330)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(171, 27)
        Me.Label2.TabIndex = 70
        Me.Label2.Text = "Full pallet Qty:"
        '
        'lblQtyPerPallet
        '
        Me.lblQtyPerPallet.AutoSize = True
        Me.lblQtyPerPallet.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblQtyPerPallet.ForeColor = System.Drawing.Color.White
        Me.lblQtyPerPallet.Location = New System.Drawing.Point(692, 330)
        Me.lblQtyPerPallet.Name = "lblQtyPerPallet"
        Me.lblQtyPerPallet.Size = New System.Drawing.Size(25, 27)
        Me.lblQtyPerPallet.TabIndex = 70
        Me.lblQtyPerPallet.Text = "0"
        '
        'lblTotalScheduled
        '
        Me.lblTotalScheduled.AutoSize = True
        Me.lblTotalScheduled.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalScheduled.ForeColor = System.Drawing.Color.White
        Me.lblTotalScheduled.Location = New System.Drawing.Point(169, 163)
        Me.lblTotalScheduled.Name = "lblTotalScheduled"
        Me.lblTotalScheduled.Size = New System.Drawing.Size(25, 27)
        Me.lblTotalScheduled.TabIndex = 79
        Me.lblTotalScheduled.Text = "0"
        '
        'lblTotalRemain
        '
        Me.lblTotalRemain.AutoSize = True
        Me.lblTotalRemain.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalRemain.ForeColor = System.Drawing.Color.White
        Me.lblTotalRemain.Location = New System.Drawing.Point(169, 196)
        Me.lblTotalRemain.Name = "lblTotalRemain"
        Me.lblTotalRemain.Size = New System.Drawing.Size(25, 27)
        Me.lblTotalRemain.TabIndex = 78
        Me.lblTotalRemain.Text = "0"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.RoyalBlue
        Me.Label3.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Italic Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(287, 138)
        Me.Label3.Margin = New System.Windows.Forms.Padding(0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(106, 23)
        Me.Label3.TabIndex = 82
        Me.Label3.Text = "Shift Total  "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.RoyalBlue
        Me.Label5.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Italic Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(168, 138)
        Me.Label5.Margin = New System.Windows.Forms.Padding(0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(121, 23)
        Me.Label5.TabIndex = 81
        Me.Label5.Text = "Order Total   "
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.RoyalBlue
        Me.Label7.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Italic Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(31, 138)
        Me.Label7.Margin = New System.Windows.Forms.Padding(0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(138, 23)
        Me.Label7.TabIndex = 80
        Me.Label7.Text = "(Cases)            "
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
        'txtExpiryDay
        '
        Me.txtExpiryDay.BackColor = System.Drawing.Color.Black
        Me.txtExpiryDay.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtExpiryDay.ForeColor = System.Drawing.Color.White
        Me.txtExpiryDay.Location = New System.Drawing.Point(692, 439)
        Me.txtExpiryDay.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtExpiryDay.MaxLength = 2
        Me.txtExpiryDay.Name = "txtExpiryDay"
        Me.txtExpiryDay.Size = New System.Drawing.Size(51, 35)
        Me.txtExpiryDay.TabIndex = 96
        Me.txtExpiryDay.Visible = False
        '
        'lblExpiryDay
        '
        Me.lblExpiryDay.AutoSize = True
        Me.lblExpiryDay.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.lblExpiryDay.ForeColor = System.Drawing.Color.White
        Me.lblExpiryDay.Location = New System.Drawing.Point(616, 442)
        Me.lblExpiryDay.Name = "lblExpiryDay"
        Me.lblExpiryDay.Size = New System.Drawing.Size(76, 27)
        Me.lblExpiryDay.TabIndex = 95
        Me.lblExpiryDay.Text = "Day - "
        Me.lblExpiryDay.Visible = False
        '
        'txtExpiryMonth
        '
        Me.txtExpiryMonth.BackColor = System.Drawing.Color.Black
        Me.txtExpiryMonth.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtExpiryMonth.ForeColor = System.Drawing.Color.White
        Me.txtExpiryMonth.Location = New System.Drawing.Point(542, 439)
        Me.txtExpiryMonth.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtExpiryMonth.MaxLength = 2
        Me.txtExpiryMonth.Name = "txtExpiryMonth"
        Me.txtExpiryMonth.Size = New System.Drawing.Size(51, 35)
        Me.txtExpiryMonth.TabIndex = 94
        Me.txtExpiryMonth.Visible = False
        '
        'lblExpiryMonth
        '
        Me.lblExpiryMonth.AutoSize = True
        Me.lblExpiryMonth.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.lblExpiryMonth.ForeColor = System.Drawing.Color.White
        Me.lblExpiryMonth.Location = New System.Drawing.Point(439, 442)
        Me.lblExpiryMonth.Name = "lblExpiryMonth"
        Me.lblExpiryMonth.Size = New System.Drawing.Size(101, 27)
        Me.lblExpiryMonth.TabIndex = 93
        Me.lblExpiryMonth.Text = "Month - "
        Me.lblExpiryMonth.Visible = False
        '
        'txtExpiryYear
        '
        Me.txtExpiryYear.BackColor = System.Drawing.Color.Black
        Me.txtExpiryYear.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtExpiryYear.ForeColor = System.Drawing.Color.White
        Me.txtExpiryYear.Location = New System.Drawing.Point(333, 439)
        Me.txtExpiryYear.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtExpiryYear.MaxLength = 4
        Me.txtExpiryYear.Name = "txtExpiryYear"
        Me.txtExpiryYear.Size = New System.Drawing.Size(84, 35)
        Me.txtExpiryYear.TabIndex = 92
        Me.txtExpiryYear.Visible = False
        '
        'lblExpiryYear
        '
        Me.lblExpiryYear.AutoSize = True
        Me.lblExpiryYear.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.lblExpiryYear.ForeColor = System.Drawing.Color.White
        Me.lblExpiryYear.Location = New System.Drawing.Point(248, 442)
        Me.lblExpiryYear.Name = "lblExpiryYear"
        Me.lblExpiryYear.Size = New System.Drawing.Size(82, 27)
        Me.lblExpiryYear.TabIndex = 90
        Me.lblExpiryYear.Text = "Year - "
        Me.lblExpiryYear.Visible = False
        '
        'lblExpiryDate
        '
        Me.lblExpiryDate.AutoSize = True
        Me.lblExpiryDate.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.lblExpiryDate.ForeColor = System.Drawing.Color.White
        Me.lblExpiryDate.Location = New System.Drawing.Point(29, 442)
        Me.lblExpiryDate.Name = "lblExpiryDate"
        Me.lblExpiryDate.Size = New System.Drawing.Size(204, 27)
        Me.lblExpiryDate.TabIndex = 91
        Me.lblExpiryDate.Text = "Prod. Expiry date:"
        Me.lblExpiryDate.Visible = False
        '
        'lblOutputLocation
        '
        Me.lblOutputLocation.AutoSize = True
        Me.lblOutputLocation.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutputLocation.ForeColor = System.Drawing.Color.White
        Me.lblOutputLocation.Location = New System.Drawing.Point(515, 254)
        Me.lblOutputLocation.Name = "lblOutputLocation"
        Me.lblOutputLocation.Size = New System.Drawing.Size(109, 27)
        Me.lblOutputLocation.TabIndex = 97
        Me.lblOutputLocation.Text = "Location:"
        '
        'cboOutputLocation
        '
        Me.cboOutputLocation.BackColor = System.Drawing.Color.Black
        Me.cboOutputLocation.DropDownHeight = 320
        Me.cboOutputLocation.DropDownWidth = 113
        Me.cboOutputLocation.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOutputLocation.ForeColor = System.Drawing.Color.White
        Me.cboOutputLocation.FormattingEnabled = True
        Me.cboOutputLocation.IntegralHeight = False
        Me.cboOutputLocation.ItemHeight = 27
        Me.cboOutputLocation.Location = New System.Drawing.Point(630, 249)
        Me.cboOutputLocation.MaxDropDownItems = 7
        Me.cboOutputLocation.MaxLength = 8
        Me.cboOutputLocation.Name = "cboOutputLocation"
        Me.cboOutputLocation.Size = New System.Drawing.Size(113, 35)
        Me.cboOutputLocation.TabIndex = 98
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
        'frmCreatePallet
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.cboOutputLocation)
        Me.Controls.Add(Me.lblOutputLocation)
        Me.Controls.Add(Me.txtExpiryDay)
        Me.Controls.Add(Me.lblExpiryDay)
        Me.Controls.Add(Me.txtExpiryMonth)
        Me.Controls.Add(Me.lblExpiryMonth)
        Me.Controls.Add(Me.txtExpiryYear)
        Me.Controls.Add(Me.lblExpiryYear)
        Me.Controls.Add(Me.lblExpiryDate)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.lblTotalScheduled)
        Me.Controls.Add(Me.lblTotalRemain)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.btnCreatePallet)
        Me.Controls.Add(Me.txtCasesInPallet)
        Me.Controls.Add(Me.lblCasesInPallet)
        Me.Controls.Add(Me.btnNotLastPallet)
        Me.Controls.Add(Me.btnPalletNotFull)
        Me.Controls.Add(Me.btnLastPallet)
        Me.Controls.Add(Me.btnPalletFull)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblQtyPerPallet)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblCasesRemain)
        Me.Controls.Add(Me.lblCaseScheduled)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lblItemNo)
        Me.Controls.Add(Me.lblItemDesc)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.lblShopOrder)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCreatePallet"
        Me.Text = "Create Pallet"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents lblItemNo As System.Windows.Forms.Label
    Friend WithEvents lblItemDesc As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents lblShopOrder As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents lblCaseScheduled As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblCasesRemain As System.Windows.Forms.Label
    Friend WithEvents btnPalletNotFull As System.Windows.Forms.Button
    Friend WithEvents btnPalletFull As System.Windows.Forms.Button
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtCasesInPallet As System.Windows.Forms.TextBox
    Friend WithEvents lblCasesInPallet As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnLastPallet As System.Windows.Forms.Button
    Friend WithEvents btnNotLastPallet As System.Windows.Forms.Button
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents btnCreatePallet As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblQtyPerPallet As System.Windows.Forms.Label
    Friend WithEvents lblTotalScheduled As System.Windows.Forms.Label
    Friend WithEvents lblTotalRemain As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents txtExpiryDay As System.Windows.Forms.TextBox
    Friend WithEvents lblExpiryDay As System.Windows.Forms.Label
    Friend WithEvents txtExpiryMonth As System.Windows.Forms.TextBox
    Friend WithEvents lblExpiryMonth As System.Windows.Forms.Label
    Friend WithEvents txtExpiryYear As System.Windows.Forms.TextBox
    Friend WithEvents lblExpiryYear As System.Windows.Forms.Label
    Friend WithEvents lblExpiryDate As System.Windows.Forms.Label
    Friend WithEvents lblOutputLocation As System.Windows.Forms.Label
    Friend WithEvents cboOutputLocation As System.Windows.Forms.ComboBox
End Class
