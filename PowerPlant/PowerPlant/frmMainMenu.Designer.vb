<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMainMenu
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
        Me.btnLogOut = New System.Windows.Forms.Button()
        Me.btnCaseLabel = New System.Windows.Forms.Button()
        Me.btnShopOrder = New System.Windows.Forms.Button()
        Me.btnPrinterControl = New System.Windows.Forms.Button()
        Me.btnCreatePallet = New System.Windows.Forms.Button()
        Me.btnInquiry = New System.Windows.Forms.Button()
        Me.lblItemNo = New System.Windows.Forms.Label()
        Me.lblItemDesc = New System.Windows.Forms.Label()
        Me.lblSKU = New System.Windows.Forms.Label()
        Me.lblShopOrder = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnDownTime = New System.Windows.Forms.Button()
        Me.btnLogScrap = New System.Windows.Forms.Button()
        Me.lblTieTier = New System.Windows.Forms.Label()
        Me.lblTieTierValues = New System.Windows.Forms.Label()
        Me.lblLabelWeightValue = New System.Windows.Forms.Label()
        Me.lblLabelWeight = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.lblLineName = New System.Windows.Forms.Label()
        Me.lblQtyPerPalletValue = New System.Windows.Forms.Label()
        Me.lblQtyPerPallet = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SOScheduled = New System.Windows.Forms.Label()
        Me.lblSOScheduled = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtCaseCounterInPallet = New System.Windows.Forms.TextBox()
        Me.txtShiftProduced = New System.Windows.Forms.TextBox()
        Me.txtSOProduced = New System.Windows.Forms.TextBox()
        Me.btnQATInProcess = New System.Windows.Forms.Button()
        Me.btnQATStartUp = New System.Windows.Forms.Button()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.btnLogScrapRejectPoint = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnLogOut
        '
        Me.btnLogOut.BackColor = System.Drawing.Color.Silver
        Me.btnLogOut.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogOut.Location = New System.Drawing.Point(50, 905)
        Me.btnLogOut.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnLogOut.Name = "btnLogOut"
        Me.btnLogOut.Size = New System.Drawing.Size(275, 120)
        Me.btnLogOut.TabIndex = 77
        Me.btnLogOut.Text = "Log Out"
        Me.btnLogOut.UseVisualStyleBackColor = False
        '
        'btnCaseLabel
        '
        Me.btnCaseLabel.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnCaseLabel.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnCaseLabel.Location = New System.Drawing.Point(592, 546)
        Me.btnCaseLabel.Margin = New System.Windows.Forms.Padding(6)
        Me.btnCaseLabel.Name = "btnCaseLabel"
        Me.btnCaseLabel.Size = New System.Drawing.Size(319, 116)
        Me.btnCaseLabel.TabIndex = 79
        Me.btnCaseLabel.Text = "Case Label"
        Me.btnCaseLabel.UseVisualStyleBackColor = False
        '
        'btnShopOrder
        '
        Me.btnShopOrder.BackColor = System.Drawing.Color.Lime
        Me.btnShopOrder.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnShopOrder.Location = New System.Drawing.Point(138, 399)
        Me.btnShopOrder.Margin = New System.Windows.Forms.Padding(6)
        Me.btnShopOrder.Name = "btnShopOrder"
        Me.btnShopOrder.Size = New System.Drawing.Size(319, 116)
        Me.btnShopOrder.TabIndex = 78
        Me.btnShopOrder.Text = "Shop Order"
        Me.btnShopOrder.UseVisualStyleBackColor = False
        '
        'btnPrinterControl
        '
        Me.btnPrinterControl.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnPrinterControl.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnPrinterControl.Location = New System.Drawing.Point(592, 399)
        Me.btnPrinterControl.Margin = New System.Windows.Forms.Padding(6)
        Me.btnPrinterControl.Name = "btnPrinterControl"
        Me.btnPrinterControl.Size = New System.Drawing.Size(319, 116)
        Me.btnPrinterControl.TabIndex = 79
        Me.btnPrinterControl.Text = "Printer Control"
        Me.btnPrinterControl.UseVisualStyleBackColor = False
        '
        'btnCreatePallet
        '
        Me.btnCreatePallet.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnCreatePallet.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnCreatePallet.Location = New System.Drawing.Point(138, 546)
        Me.btnCreatePallet.Margin = New System.Windows.Forms.Padding(6)
        Me.btnCreatePallet.Name = "btnCreatePallet"
        Me.btnCreatePallet.Size = New System.Drawing.Size(319, 116)
        Me.btnCreatePallet.TabIndex = 79
        Me.btnCreatePallet.Text = "Create Pallet"
        Me.btnCreatePallet.UseVisualStyleBackColor = False
        '
        'btnInquiry
        '
        Me.btnInquiry.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnInquiry.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnInquiry.Location = New System.Drawing.Point(592, 698)
        Me.btnInquiry.Margin = New System.Windows.Forms.Padding(6)
        Me.btnInquiry.Name = "btnInquiry"
        Me.btnInquiry.Size = New System.Drawing.Size(319, 116)
        Me.btnInquiry.TabIndex = 79
        Me.btnInquiry.Text = "Inquiry"
        Me.btnInquiry.UseVisualStyleBackColor = False
        '
        'lblItemNo
        '
        Me.lblItemNo.AutoSize = True
        Me.lblItemNo.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemNo.ForeColor = System.Drawing.Color.White
        Me.lblItemNo.Location = New System.Drawing.Point(689, 142)
        Me.lblItemNo.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblItemNo.Name = "lblItemNo"
        Me.lblItemNo.Size = New System.Drawing.Size(89, 47)
        Me.lblItemNo.TabIndex = 84
        Me.lblItemNo.Text = "000"
        '
        'lblItemDesc
        '
        Me.lblItemDesc.AutoSize = True
        Me.lblItemDesc.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemDesc.ForeColor = System.Drawing.Color.White
        Me.lblItemDesc.Location = New System.Drawing.Point(51, 212)
        Me.lblItemDesc.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblItemDesc.Name = "lblItemDesc"
        Me.lblItemDesc.Size = New System.Drawing.Size(281, 42)
        Me.lblItemDesc.TabIndex = 82
        Me.lblItemDesc.Text = "Item Description"
        '
        'lblSKU
        '
        Me.lblSKU.AutoSize = True
        Me.lblSKU.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSKU.ForeColor = System.Drawing.Color.White
        Me.lblSKU.Location = New System.Drawing.Point(550, 138)
        Me.lblSKU.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblSKU.Name = "lblSKU"
        Me.lblSKU.Size = New System.Drawing.Size(124, 49)
        Me.lblSKU.TabIndex = 83
        Me.lblSKU.Text = "SKU:"
        '
        'lblShopOrder
        '
        Me.lblShopOrder.AutoSize = True
        Me.lblShopOrder.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShopOrder.ForeColor = System.Drawing.Color.White
        Me.lblShopOrder.Location = New System.Drawing.Point(341, 142)
        Me.lblShopOrder.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblShopOrder.Name = "lblShopOrder"
        Me.lblShopOrder.Size = New System.Drawing.Size(158, 47)
        Me.lblShopOrder.TabIndex = 80
        Me.lblShopOrder.Text = "000000"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(51, 138)
        Me.Label16.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(268, 49)
        Me.Label16.TabIndex = 81
        Me.Label16.Text = "Shop Order:"
        '
        'btnDownTime
        '
        Me.btnDownTime.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnDownTime.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnDownTime.Location = New System.Drawing.Point(138, 698)
        Me.btnDownTime.Margin = New System.Windows.Forms.Padding(6)
        Me.btnDownTime.Name = "btnDownTime"
        Me.btnDownTime.Size = New System.Drawing.Size(319, 116)
        Me.btnDownTime.TabIndex = 79
        Me.btnDownTime.Text = "Down Time"
        Me.btnDownTime.UseVisualStyleBackColor = False
        '
        'btnLogScrap
        '
        Me.btnLogScrap.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnLogScrap.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnLogScrap.Location = New System.Drawing.Point(1043, 399)
        Me.btnLogScrap.Margin = New System.Windows.Forms.Padding(6)
        Me.btnLogScrap.Name = "btnLogScrap"
        Me.btnLogScrap.Size = New System.Drawing.Size(319, 116)
        Me.btnLogScrap.TabIndex = 79
        Me.btnLogScrap.Text = "Log Scrap"
        Me.btnLogScrap.UseVisualStyleBackColor = False
        '
        'lblTieTier
        '
        Me.lblTieTier.AutoSize = True
        Me.lblTieTier.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTieTier.ForeColor = System.Drawing.Color.White
        Me.lblTieTier.Location = New System.Drawing.Point(964, 138)
        Me.lblTieTier.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblTieTier.Name = "lblTieTier"
        Me.lblTieTier.Size = New System.Drawing.Size(221, 49)
        Me.lblTieTier.TabIndex = 83
        Me.lblTieTier.Text = "Tie x Tier:"
        '
        'lblTieTierValues
        '
        Me.lblTieTierValues.AutoSize = True
        Me.lblTieTierValues.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTieTierValues.ForeColor = System.Drawing.Color.White
        Me.lblTieTierValues.Location = New System.Drawing.Point(1206, 142)
        Me.lblTieTierValues.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblTieTierValues.Name = "lblTieTierValues"
        Me.lblTieTierValues.Size = New System.Drawing.Size(180, 47)
        Me.lblTieTierValues.TabIndex = 84
        Me.lblTieTierValues.Text = "000x000"
        '
        'lblLabelWeightValue
        '
        Me.lblLabelWeightValue.AutoSize = True
        Me.lblLabelWeightValue.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLabelWeightValue.ForeColor = System.Drawing.Color.White
        Me.lblLabelWeightValue.Location = New System.Drawing.Point(324, 279)
        Me.lblLabelWeightValue.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblLabelWeightValue.Name = "lblLabelWeightValue"
        Me.lblLabelWeightValue.Size = New System.Drawing.Size(70, 42)
        Me.lblLabelWeightValue.TabIndex = 86
        Me.lblLabelWeightValue.Text = "0.0"
        '
        'lblLabelWeight
        '
        Me.lblLabelWeight.AutoSize = True
        Me.lblLabelWeight.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLabelWeight.ForeColor = System.Drawing.Color.White
        Me.lblLabelWeight.Location = New System.Drawing.Point(57, 279)
        Me.lblLabelWeight.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblLabelWeight.Name = "lblLabelWeight"
        Me.lblLabelWeight.Size = New System.Drawing.Size(263, 44)
        Me.lblLabelWeight.TabIndex = 82
        Me.lblLabelWeight.Text = "Label Weight:"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.DodgerBlue
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.Location = New System.Drawing.Point(55, 343)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(6)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(1370, 532)
        Me.PictureBox1.TabIndex = 87
        Me.PictureBox1.TabStop = False
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ForeColor = System.Drawing.Color.LightSalmon
        Me.lblMessage.Location = New System.Drawing.Point(40, 1043)
        Me.lblMessage.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(187, 47)
        Me.lblMessage.TabIndex = 88
        Me.lblMessage.Text = "Message"
        '
        'lblLineName
        '
        Me.lblLineName.AutoSize = True
        Me.lblLineName.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLineName.ForeColor = System.Drawing.Color.White
        Me.lblLineName.Location = New System.Drawing.Point(546, 279)
        Me.lblLineName.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblLineName.Name = "lblLineName"
        Me.lblLineName.Size = New System.Drawing.Size(119, 44)
        Me.lblLineName.TabIndex = 82
        Me.lblLineName.Text = "Line: "
        '
        'lblQtyPerPalletValue
        '
        Me.lblQtyPerPalletValue.AutoSize = True
        Me.lblQtyPerPalletValue.Font = New System.Drawing.Font("Arial", 15.75!)
        Me.lblQtyPerPalletValue.ForeColor = System.Drawing.Color.White
        Me.lblQtyPerPalletValue.Location = New System.Drawing.Point(1304, 279)
        Me.lblQtyPerPalletValue.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblQtyPerPalletValue.Name = "lblQtyPerPalletValue"
        Me.lblQtyPerPalletValue.Size = New System.Drawing.Size(39, 42)
        Me.lblQtyPerPalletValue.TabIndex = 90
        Me.lblQtyPerPalletValue.Text = "0"
        '
        'lblQtyPerPallet
        '
        Me.lblQtyPerPallet.AutoSize = True
        Me.lblQtyPerPallet.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblQtyPerPallet.ForeColor = System.Drawing.Color.White
        Me.lblQtyPerPallet.Location = New System.Drawing.Point(1036, 279)
        Me.lblQtyPerPallet.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblQtyPerPallet.Name = "lblQtyPerPallet"
        Me.lblQtyPerPallet.Size = New System.Drawing.Size(250, 44)
        Me.lblQtyPerPallet.TabIndex = 89
        Me.lblQtyPerPallet.Text = "Cases/pallet:"
        '
        'Timer1
        '
        '
        'SOScheduled
        '
        Me.SOScheduled.AutoSize = True
        Me.SOScheduled.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SOScheduled.ForeColor = System.Drawing.Color.White
        Me.SOScheduled.Location = New System.Drawing.Point(4, 26)
        Me.SOScheduled.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.SOScheduled.Name = "SOScheduled"
        Me.SOScheduled.Size = New System.Drawing.Size(273, 44)
        Me.SOScheduled.TabIndex = 92
        Me.SOScheduled.Text = "SO Scheduled"
        '
        'lblSOScheduled
        '
        Me.lblSOScheduled.AutoSize = True
        Me.lblSOScheduled.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSOScheduled.ForeColor = System.Drawing.Color.White
        Me.lblSOScheduled.Location = New System.Drawing.Point(90, 90)
        Me.lblSOScheduled.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblSOScheduled.Name = "lblSOScheduled"
        Me.lblSOScheduled.Size = New System.Drawing.Size(39, 42)
        Me.lblSOScheduled.TabIndex = 93
        Me.lblSOScheduled.Text = "0"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(710, 26)
        Me.Label5.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(295, 44)
        Me.Label5.TabIndex = 94
        Me.Label5.Text = "Cases on Pallet"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(297, 26)
        Me.Label3.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(178, 44)
        Me.Label3.TabIndex = 95
        Me.Label3.Text = "SO Prod."
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(495, 26)
        Me.Label8.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(206, 44)
        Me.Label8.TabIndex = 98
        Me.Label8.Text = "Shift Prod."
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.DodgerBlue
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.txtCaseCounterInPallet)
        Me.Panel1.Controls.Add(Me.txtShiftProduced)
        Me.Panel1.Controls.Add(Me.txtSOProduced)
        Me.Panel1.Controls.Add(Me.SOScheduled)
        Me.Panel1.Controls.Add(Me.lblSOScheduled)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Location = New System.Drawing.Point(407, 879)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(6)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1018, 155)
        Me.Panel1.TabIndex = 100
        '
        'txtCaseCounterInPallet
        '
        Me.txtCaseCounterInPallet.BackColor = System.Drawing.Color.Blue
        Me.txtCaseCounterInPallet.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.txtCaseCounterInPallet.ForeColor = System.Drawing.Color.White
        Me.txtCaseCounterInPallet.Location = New System.Drawing.Point(779, 78)
        Me.txtCaseCounterInPallet.Margin = New System.Windows.Forms.Padding(6)
        Me.txtCaseCounterInPallet.MaxLength = 4
        Me.txtCaseCounterInPallet.Name = "txtCaseCounterInPallet"
        Me.txtCaseCounterInPallet.ReadOnly = True
        Me.txtCaseCounterInPallet.Size = New System.Drawing.Size(173, 50)
        Me.txtCaseCounterInPallet.TabIndex = 101
        Me.txtCaseCounterInPallet.TabStop = False
        Me.txtCaseCounterInPallet.Text = "--"
        Me.txtCaseCounterInPallet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtShiftProduced
        '
        Me.txtShiftProduced.BackColor = System.Drawing.Color.Blue
        Me.txtShiftProduced.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold)
        Me.txtShiftProduced.ForeColor = System.Drawing.Color.White
        Me.txtShiftProduced.Location = New System.Drawing.Point(519, 78)
        Me.txtShiftProduced.Margin = New System.Windows.Forms.Padding(6)
        Me.txtShiftProduced.MaxLength = 4
        Me.txtShiftProduced.Name = "txtShiftProduced"
        Me.txtShiftProduced.ReadOnly = True
        Me.txtShiftProduced.Size = New System.Drawing.Size(173, 50)
        Me.txtShiftProduced.TabIndex = 101
        Me.txtShiftProduced.TabStop = False
        Me.txtShiftProduced.Text = "--"
        Me.txtShiftProduced.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtSOProduced
        '
        Me.txtSOProduced.BackColor = System.Drawing.Color.Blue
        Me.txtSOProduced.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSOProduced.ForeColor = System.Drawing.Color.White
        Me.txtSOProduced.Location = New System.Drawing.Point(295, 78)
        Me.txtSOProduced.Margin = New System.Windows.Forms.Padding(6)
        Me.txtSOProduced.MaxLength = 7
        Me.txtSOProduced.Name = "txtSOProduced"
        Me.txtSOProduced.ReadOnly = True
        Me.txtSOProduced.Size = New System.Drawing.Size(173, 50)
        Me.txtSOProduced.TabIndex = 101
        Me.txtSOProduced.TabStop = False
        Me.txtSOProduced.Text = "--"
        Me.txtSOProduced.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnQATInProcess
        '
        Me.btnQATInProcess.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnQATInProcess.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnQATInProcess.Location = New System.Drawing.Point(1106, 785)
        Me.btnQATInProcess.Margin = New System.Windows.Forms.Padding(6)
        Me.btnQATInProcess.Name = "btnQATInProcess"
        Me.btnQATInProcess.Size = New System.Drawing.Size(319, 116)
        Me.btnQATInProcess.TabIndex = 101
        Me.btnQATInProcess.Text = "QAT In-Process"
        Me.btnQATInProcess.UseVisualStyleBackColor = False
        Me.btnQATInProcess.Visible = False
        '
        'btnQATStartUp
        '
        Me.btnQATStartUp.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnQATStartUp.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnQATStartUp.Location = New System.Drawing.Point(1106, 714)
        Me.btnQATStartUp.Margin = New System.Windows.Forms.Padding(6)
        Me.btnQATStartUp.Name = "btnQATStartUp"
        Me.btnQATStartUp.Size = New System.Drawing.Size(319, 116)
        Me.btnQATStartUp.TabIndex = 102
        Me.btnQATStartUp.Text = "QAT Start-Up"
        Me.btnQATStartUp.UseVisualStyleBackColor = False
        Me.btnQATStartUp.Visible = False
        '
        'UcHeading1
        '
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(0, 0)
        Me.UcHeading1.Margin = New System.Windows.Forms.Padding(11)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "-- Heading --"
        Me.UcHeading1.Size = New System.Drawing.Size(1467, 107)
        Me.UcHeading1.TabIndex = 0
        '
        'btnLogScrapRejectPoint
        '
        Me.btnLogScrapRejectPoint.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnLogScrapRejectPoint.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnLogScrapRejectPoint.Location = New System.Drawing.Point(1044, 546)
        Me.btnLogScrapRejectPoint.Margin = New System.Windows.Forms.Padding(6)
        Me.btnLogScrapRejectPoint.Name = "btnLogScrapRejectPoint"
        Me.btnLogScrapRejectPoint.Size = New System.Drawing.Size(319, 116)
        Me.btnLogScrapRejectPoint.TabIndex = 103
        Me.btnLogScrapRejectPoint.Text = "Log Scrap Reject Point"
        Me.btnLogScrapRejectPoint.UseVisualStyleBackColor = False
        '
        'frmMainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(1467, 1108)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnLogScrapRejectPoint)
        Me.Controls.Add(Me.btnQATStartUp)
        Me.Controls.Add(Me.btnQATInProcess)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblQtyPerPalletValue)
        Me.Controls.Add(Me.lblQtyPerPallet)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.lblLabelWeightValue)
        Me.Controls.Add(Me.lblTieTierValues)
        Me.Controls.Add(Me.lblItemNo)
        Me.Controls.Add(Me.lblLineName)
        Me.Controls.Add(Me.lblLabelWeight)
        Me.Controls.Add(Me.lblItemDesc)
        Me.Controls.Add(Me.lblTieTier)
        Me.Controls.Add(Me.lblSKU)
        Me.Controls.Add(Me.lblShopOrder)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.btnLogScrap)
        Me.Controls.Add(Me.btnInquiry)
        Me.Controls.Add(Me.btnDownTime)
        Me.Controls.Add(Me.btnCreatePallet)
        Me.Controls.Add(Me.btnPrinterControl)
        Me.Controls.Add(Me.btnCaseLabel)
        Me.Controls.Add(Me.btnShopOrder)
        Me.Controls.Add(Me.btnLogOut)
        Me.Controls.Add(Me.UcHeading1)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(6)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMainMenu"
        Me.Text = "frmMain"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents btnLogOut As System.Windows.Forms.Button
    Friend WithEvents btnCaseLabel As System.Windows.Forms.Button
    Friend WithEvents btnShopOrder As System.Windows.Forms.Button
    Friend WithEvents btnPrinterControl As System.Windows.Forms.Button
    Friend WithEvents btnCreatePallet As System.Windows.Forms.Button
    Friend WithEvents btnInquiry As System.Windows.Forms.Button
    Friend WithEvents lblItemNo As System.Windows.Forms.Label
    Friend WithEvents lblItemDesc As System.Windows.Forms.Label
    Friend WithEvents lblSKU As System.Windows.Forms.Label
    Friend WithEvents lblShopOrder As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btnDownTime As System.Windows.Forms.Button
    Friend WithEvents btnLogScrap As System.Windows.Forms.Button
    Friend WithEvents lblTieTier As System.Windows.Forms.Label
    Friend WithEvents lblTieTierValues As System.Windows.Forms.Label
    Friend WithEvents lblLabelWeightValue As System.Windows.Forms.Label
    Friend WithEvents lblLabelWeight As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents lblLineName As System.Windows.Forms.Label
    Friend WithEvents lblQtyPerPalletValue As System.Windows.Forms.Label
    Friend WithEvents lblQtyPerPallet As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents SOScheduled As System.Windows.Forms.Label
    Friend WithEvents lblSOScheduled As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtShiftProduced As System.Windows.Forms.TextBox
    Friend WithEvents txtSOProduced As System.Windows.Forms.TextBox
    Friend WithEvents txtCaseCounterInPallet As System.Windows.Forms.TextBox
    Friend WithEvents btnQATInProcess As System.Windows.Forms.Button
    Friend WithEvents btnQATStartUp As System.Windows.Forms.Button
    Friend WithEvents btnLogScrapRejectPoint As Button
End Class
