<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmQATCaseDateCoder
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvPrtDev = New System.Windows.Forms.DataGridView()
        Me.dgvPrtDev_btnPrtName = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.TblPkgLinePrinterDeviceBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsPrinterDevice = New PowerPlant.dsPrinterDevice()
        Me.TaPrinterDevice = New PowerPlant.dsPrinterDeviceTableAdapters.taPrinterDevice()
        Me.pbxLabelImage = New System.Windows.Forms.PictureBox()
        Me.btnFail = New System.Windows.Forms.Button()
        Me.btnPass = New System.Windows.Forms.Button()
        Me.btnRetest = New System.Windows.Forms.Button()
        Me.lblPreFmtExpiryDate = New System.Windows.Forms.Label()
        Me.lblPreFmtProductionDate = New System.Windows.Forms.Label()
        Me.btnNA = New System.Windows.Forms.Button()
        Me.lblPreFmtExpiryDateDesc = New System.Windows.Forms.Label()
        Me.lblPreFmtProductionDateDesc = New System.Windows.Forms.Label()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        CType(Me.dgvPrtDev, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TblPkgLinePrinterDeviceBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsPrinterDevice, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbxLabelImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvPrtDev
        '
        Me.dgvPrtDev.AllowUserToAddRows = False
        Me.dgvPrtDev.AllowUserToDeleteRows = False
        Me.dgvPrtDev.AllowUserToResizeColumns = False
        Me.dgvPrtDev.AllowUserToResizeRows = False
        Me.dgvPrtDev.AutoGenerateColumns = False
        Me.dgvPrtDev.BackgroundColor = System.Drawing.Color.CornflowerBlue
        Me.dgvPrtDev.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvPrtDev.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvPrtDev.ColumnHeadersHeight = 35
        Me.dgvPrtDev.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvPrtDev.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgvPrtDev_btnPrtName})
        Me.dgvPrtDev.DataSource = Me.TblPkgLinePrinterDeviceBindingSource
        Me.dgvPrtDev.Location = New System.Drawing.Point(3, 56)
        Me.dgvPrtDev.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.dgvPrtDev.Name = "dgvPrtDev"
        Me.dgvPrtDev.ReadOnly = True
        Me.dgvPrtDev.RowHeadersVisible = False
        Me.dgvPrtDev.RowHeadersWidth = 10
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Tahoma", 15.75!)
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Green
        Me.dgvPrtDev.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvPrtDev.RowTemplate.Height = 65
        Me.dgvPrtDev.RowTemplate.ReadOnly = True
        Me.dgvPrtDev.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPrtDev.Size = New System.Drawing.Size(375, 415)
        Me.dgvPrtDev.TabIndex = 79
        '
        'dgvPrtDev_btnPrtName
        '
        Me.dgvPrtDev_btnPrtName.DataPropertyName = "DeviceName"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.dgvPrtDev_btnPrtName.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvPrtDev_btnPrtName.HeaderText = "Printer Name"
        Me.dgvPrtDev_btnPrtName.Name = "dgvPrtDev_btnPrtName"
        Me.dgvPrtDev_btnPrtName.ReadOnly = True
        Me.dgvPrtDev_btnPrtName.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPrtDev_btnPrtName.Width = 350
        '
        'TblPkgLinePrinterDeviceBindingSource
        '
        Me.TblPkgLinePrinterDeviceBindingSource.DataMember = "tblPkgLinePrinterDevice"
        Me.TblPkgLinePrinterDeviceBindingSource.DataSource = Me.DsPrinterDevice
        '
        'DsPrinterDevice
        '
        Me.DsPrinterDevice.DataSetName = "dsPrinterDevice"
        Me.DsPrinterDevice.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'TaPrinterDevice
        '
        Me.TaPrinterDevice.ClearBeforeFill = True
        '
        'pbxLabelImage
        '
        Me.pbxLabelImage.BackColor = System.Drawing.Color.CornflowerBlue
        Me.pbxLabelImage.Location = New System.Drawing.Point(380, 56)
        Me.pbxLabelImage.Name = "pbxLabelImage"
        Me.pbxLabelImage.Size = New System.Drawing.Size(415, 415)
        Me.pbxLabelImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbxLabelImage.TabIndex = 80
        Me.pbxLabelImage.TabStop = False
        '
        'btnFail
        '
        Me.btnFail.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnFail.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFail.Location = New System.Drawing.Point(707, 492)
        Me.btnFail.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnFail.Name = "btnFail"
        Me.btnFail.Size = New System.Drawing.Size(75, 60)
        Me.btnFail.TabIndex = 120
        Me.btnFail.Text = "Fail"
        Me.btnFail.UseVisualStyleBackColor = False
        '
        'btnPass
        '
        Me.btnPass.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnPass.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPass.Location = New System.Drawing.Point(620, 492)
        Me.btnPass.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPass.Name = "btnPass"
        Me.btnPass.Size = New System.Drawing.Size(75, 60)
        Me.btnPass.TabIndex = 119
        Me.btnPass.Text = "Pass"
        Me.btnPass.UseVisualStyleBackColor = False
        '
        'btnRetest
        '
        Me.btnRetest.BackColor = System.Drawing.Color.Silver
        Me.btnRetest.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRetest.Location = New System.Drawing.Point(12, 492)
        Me.btnRetest.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnRetest.Name = "btnRetest"
        Me.btnRetest.Size = New System.Drawing.Size(150, 65)
        Me.btnRetest.TabIndex = 121
        Me.btnRetest.Text = "Test After RCA"
        Me.btnRetest.UseVisualStyleBackColor = False
        Me.btnRetest.Visible = False
        '
        'lblPreFmtExpiryDate
        '
        Me.lblPreFmtExpiryDate.AutoSize = True
        Me.lblPreFmtExpiryDate.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreFmtExpiryDate.ForeColor = System.Drawing.Color.White
        Me.lblPreFmtExpiryDate.Location = New System.Drawing.Point(241, 495)
        Me.lblPreFmtExpiryDate.Name = "lblPreFmtExpiryDate"
        Me.lblPreFmtExpiryDate.Size = New System.Drawing.Size(217, 27)
        Me.lblPreFmtExpiryDate.TabIndex = 122
        Me.lblPreFmtExpiryDate.Text = "BB/MA: 2019 JA 23"
        Me.lblPreFmtExpiryDate.Visible = False
        '
        'lblPreFmtProductionDate
        '
        Me.lblPreFmtProductionDate.AutoSize = True
        Me.lblPreFmtProductionDate.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreFmtProductionDate.ForeColor = System.Drawing.Color.White
        Me.lblPreFmtProductionDate.Location = New System.Drawing.Point(241, 530)
        Me.lblPreFmtProductionDate.Name = "lblPreFmtProductionDate"
        Me.lblPreFmtProductionDate.Size = New System.Drawing.Size(217, 27)
        Me.lblPreFmtProductionDate.TabIndex = 123
        Me.lblPreFmtProductionDate.Text = "BB/MA: 2019 JA 23"
        Me.lblPreFmtProductionDate.Visible = False
        '
        'btnNA
        '
        Me.btnNA.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnNA.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNA.Location = New System.Drawing.Point(531, 492)
        Me.btnNA.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNA.Name = "btnNA"
        Me.btnNA.Size = New System.Drawing.Size(75, 60)
        Me.btnNA.TabIndex = 124
        Me.btnNA.Text = "N/A"
        Me.btnNA.UseVisualStyleBackColor = False
        '
        'lblPreFmtExpiryDateDesc
        '
        Me.lblPreFmtExpiryDateDesc.AutoSize = True
        Me.lblPreFmtExpiryDateDesc.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreFmtExpiryDateDesc.ForeColor = System.Drawing.Color.White
        Me.lblPreFmtExpiryDateDesc.Location = New System.Drawing.Point(186, 495)
        Me.lblPreFmtExpiryDateDesc.Name = "lblPreFmtExpiryDateDesc"
        Me.lblPreFmtExpiryDateDesc.Size = New System.Drawing.Size(60, 27)
        Me.lblPreFmtExpiryDateDesc.TabIndex = 125
        Me.lblPreFmtExpiryDateDesc.Text = "Exp:"
        Me.lblPreFmtExpiryDateDesc.Visible = False
        '
        'lblPreFmtProductionDateDesc
        '
        Me.lblPreFmtProductionDateDesc.AutoSize = True
        Me.lblPreFmtProductionDateDesc.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreFmtProductionDateDesc.ForeColor = System.Drawing.Color.White
        Me.lblPreFmtProductionDateDesc.Location = New System.Drawing.Point(186, 530)
        Me.lblPreFmtProductionDateDesc.Name = "lblPreFmtProductionDateDesc"
        Me.lblPreFmtProductionDateDesc.Size = New System.Drawing.Size(57, 27)
        Me.lblPreFmtProductionDateDesc.TabIndex = 126
        Me.lblPreFmtProductionDateDesc.Text = "Prd:"
        Me.lblPreFmtProductionDateDesc.Visible = False
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(0, 0)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "QAT Case Date Code"
        Me.UcHeading1.Size = New System.Drawing.Size(800, 55)
        Me.UcHeading1.TabIndex = 14
        '
        'frmQATCaseDateCoder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblPreFmtProductionDateDesc)
        Me.Controls.Add(Me.lblPreFmtExpiryDateDesc)
        Me.Controls.Add(Me.btnNA)
        Me.Controls.Add(Me.lblPreFmtProductionDate)
        Me.Controls.Add(Me.lblPreFmtExpiryDate)
        Me.Controls.Add(Me.btnRetest)
        Me.Controls.Add(Me.btnFail)
        Me.Controls.Add(Me.btnPass)
        Me.Controls.Add(Me.pbxLabelImage)
        Me.Controls.Add(Me.dgvPrtDev)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmQATCaseDateCoder"
        Me.Text = "QAT Case Date Code"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvPrtDev, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TblPkgLinePrinterDeviceBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsPrinterDevice, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbxLabelImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents dgvPrtDev As System.Windows.Forms.DataGridView
    Friend WithEvents TblPkgLinePrinterDeviceBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DsPrinterDevice As PowerPlant.dsPrinterDevice
    Friend WithEvents TaPrinterDevice As PowerPlant.dsPrinterDeviceTableAdapters.taPrinterDevice
    Friend WithEvents pbxLabelImage As System.Windows.Forms.PictureBox
    Friend WithEvents dgvPrtDev_btnPrtName As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents btnFail As System.Windows.Forms.Button
    Friend WithEvents btnPass As System.Windows.Forms.Button
    Friend WithEvents btnRetest As System.Windows.Forms.Button
    Friend WithEvents lblPreFmtExpiryDate As System.Windows.Forms.Label
    Friend WithEvents lblPreFmtProductionDate As System.Windows.Forms.Label
    Friend WithEvents btnNA As System.Windows.Forms.Button
    Friend WithEvents lblPreFmtExpiryDateDesc As System.Windows.Forms.Label
    Friend WithEvents lblPreFmtProductionDateDesc As System.Windows.Forms.Label
End Class
