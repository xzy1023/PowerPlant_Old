<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPalletInquiry
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.dgvPalletProduced = New System.Windows.Forms.DataGridView()
        Me.PPspPalletSelBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsPalletsOnServer = New PowerPlant.dsPalletsOnServer()
        Me.txtPkgLine = New System.Windows.Forms.TextBox()
        Me.lblPkgLine = New System.Windows.Forms.Label()
        Me.PalletIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ShopOrderDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ItemNumberDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.QuantityDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OrderCompleteDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CreationDateTimeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PPsp_Pallet_SelTableAdapter = New PowerPlant.dsPalletsOnServerTableAdapters.PPsp_Pallet_SelTableAdapter()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LblNoOfPallets = New System.Windows.Forms.Label()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.PalletIDDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ShopOrderDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ItemNumberDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.QuantityDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OutputLocation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OrderCompleteDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CreationDateTimeDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvPalletProduced, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PPspPalletSelBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsPalletsOnServer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(14, 68)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(184, 27)
        Me.Label16.TabIndex = 61
        Me.Label16.Text = "Packaging Line:"
        '
        'btnPrvScn
        '
        Me.btnPrvScn.BackColor = System.Drawing.Color.Silver
        Me.btnPrvScn.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrvScn.Location = New System.Drawing.Point(640, 68)
        Me.btnPrvScn.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrvScn.Name = "btnPrvScn"
        Me.btnPrvScn.Size = New System.Drawing.Size(150, 65)
        Me.btnPrvScn.TabIndex = 65
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'dgvPalletProduced
        '
        Me.dgvPalletProduced.AllowUserToAddRows = False
        Me.dgvPalletProduced.AllowUserToDeleteRows = False
        Me.dgvPalletProduced.AllowUserToResizeColumns = False
        Me.dgvPalletProduced.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvPalletProduced.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvPalletProduced.AutoGenerateColumns = False
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPalletProduced.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvPalletProduced.ColumnHeadersHeight = 30
        Me.dgvPalletProduced.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvPalletProduced.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.PalletIDDataGridViewTextBoxColumn1, Me.ShopOrderDataGridViewTextBoxColumn1, Me.ItemNumberDataGridViewTextBoxColumn1, Me.QuantityDataGridViewTextBoxColumn1, Me.OutputLocation, Me.OrderCompleteDataGridViewTextBoxColumn1, Me.CreationDateTimeDataGridViewTextBoxColumn1})
        Me.dgvPalletProduced.DataSource = Me.PPspPalletSelBindingSource
        Me.dgvPalletProduced.Location = New System.Drawing.Point(19, 147)
        Me.dgvPalletProduced.Name = "dgvPalletProduced"
        Me.dgvPalletProduced.ReadOnly = True
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvPalletProduced.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvPalletProduced.RowHeadersVisible = False
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvPalletProduced.RowsDefaultCellStyle = DataGridViewCellStyle5
        Me.dgvPalletProduced.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.dgvPalletProduced.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvPalletProduced.RowTemplate.Height = 30
        Me.dgvPalletProduced.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPalletProduced.Size = New System.Drawing.Size(769, 437)
        Me.dgvPalletProduced.TabIndex = 67
        Me.dgvPalletProduced.TabStop = False
        '
        'PPspPalletSelBindingSource
        '
        Me.PPspPalletSelBindingSource.DataMember = "PPsp_Pallet_Sel"
        Me.PPspPalletSelBindingSource.DataSource = Me.DsPalletsOnServer
        '
        'DsPalletsOnServer
        '
        Me.DsPalletsOnServer.DataSetName = "dsPalletsOnServer"
        Me.DsPalletsOnServer.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'txtPkgLine
        '
        Me.txtPkgLine.BackColor = System.Drawing.Color.Black
        Me.txtPkgLine.Enabled = False
        Me.txtPkgLine.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPkgLine.ForeColor = System.Drawing.Color.White
        Me.txtPkgLine.Location = New System.Drawing.Point(198, 62)
        Me.txtPkgLine.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.txtPkgLine.MaxLength = 10
        Me.txtPkgLine.Name = "txtPkgLine"
        Me.txtPkgLine.Size = New System.Drawing.Size(114, 35)
        Me.txtPkgLine.TabIndex = 95
        '
        'lblPkgLine
        '
        Me.lblPkgLine.AutoSize = True
        Me.lblPkgLine.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.lblPkgLine.ForeColor = System.Drawing.Color.White
        Me.lblPkgLine.Location = New System.Drawing.Point(16, 110)
        Me.lblPkgLine.Name = "lblPkgLine"
        Me.lblPkgLine.Size = New System.Drawing.Size(75, 27)
        Me.lblPkgLine.TabIndex = 96
        Me.lblPkgLine.Text = "Name"
        '
        'PalletIDDataGridViewTextBoxColumn
        '
        Me.PalletIDDataGridViewTextBoxColumn.DataPropertyName = "PalletID"
        Me.PalletIDDataGridViewTextBoxColumn.HeaderText = "Pallet ID"
        Me.PalletIDDataGridViewTextBoxColumn.Name = "PalletIDDataGridViewTextBoxColumn"
        Me.PalletIDDataGridViewTextBoxColumn.ReadOnly = True
        '
        'ShopOrderDataGridViewTextBoxColumn
        '
        Me.ShopOrderDataGridViewTextBoxColumn.DataPropertyName = "ShopOrder"
        Me.ShopOrderDataGridViewTextBoxColumn.HeaderText = "Shop Order"
        Me.ShopOrderDataGridViewTextBoxColumn.Name = "ShopOrderDataGridViewTextBoxColumn"
        Me.ShopOrderDataGridViewTextBoxColumn.ReadOnly = True
        '
        'ItemNumberDataGridViewTextBoxColumn
        '
        Me.ItemNumberDataGridViewTextBoxColumn.DataPropertyName = "ItemNumber"
        Me.ItemNumberDataGridViewTextBoxColumn.HeaderText = "Item"
        Me.ItemNumberDataGridViewTextBoxColumn.Name = "ItemNumberDataGridViewTextBoxColumn"
        Me.ItemNumberDataGridViewTextBoxColumn.ReadOnly = True
        '
        'QuantityDataGridViewTextBoxColumn
        '
        Me.QuantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity"
        Me.QuantityDataGridViewTextBoxColumn.HeaderText = "Qty"
        Me.QuantityDataGridViewTextBoxColumn.Name = "QuantityDataGridViewTextBoxColumn"
        Me.QuantityDataGridViewTextBoxColumn.ReadOnly = True
        '
        'OrderCompleteDataGridViewTextBoxColumn
        '
        Me.OrderCompleteDataGridViewTextBoxColumn.DataPropertyName = "OrderComplete"
        Me.OrderCompleteDataGridViewTextBoxColumn.HeaderText = "Complete"
        Me.OrderCompleteDataGridViewTextBoxColumn.Name = "OrderCompleteDataGridViewTextBoxColumn"
        Me.OrderCompleteDataGridViewTextBoxColumn.ReadOnly = True
        '
        'CreationDateTimeDataGridViewTextBoxColumn
        '
        Me.CreationDateTimeDataGridViewTextBoxColumn.DataPropertyName = "CreationDateTime"
        Me.CreationDateTimeDataGridViewTextBoxColumn.HeaderText = "Created"
        Me.CreationDateTimeDataGridViewTextBoxColumn.Name = "CreationDateTimeDataGridViewTextBoxColumn"
        Me.CreationDateTimeDataGridViewTextBoxColumn.ReadOnly = True
        '
        'PPsp_Pallet_SelTableAdapter
        '
        Me.PPsp_Pallet_SelTableAdapter.ClearBeforeFill = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(438, 69)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(92, 27)
        Me.Label1.TabIndex = 96
        Me.Label1.Text = "Pallets:"
        '
        'LblNoOfPallets
        '
        Me.LblNoOfPallets.AutoSize = True
        Me.LblNoOfPallets.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.LblNoOfPallets.ForeColor = System.Drawing.Color.White
        Me.LblNoOfPallets.Location = New System.Drawing.Point(539, 69)
        Me.LblNoOfPallets.Name = "LblNoOfPallets"
        Me.LblNoOfPallets.Size = New System.Drawing.Size(19, 27)
        Me.LblNoOfPallets.TabIndex = 96
        Me.LblNoOfPallets.Text = " "
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(0, 0)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "-- Heading --"
        Me.UcHeading1.Size = New System.Drawing.Size(800, 55)
        Me.UcHeading1.TabIndex = 0
        '
        'PalletIDDataGridViewTextBoxColumn1
        '
        Me.PalletIDDataGridViewTextBoxColumn1.DataPropertyName = "PalletID"
        Me.PalletIDDataGridViewTextBoxColumn1.HeaderText = "Pallet ID"
        Me.PalletIDDataGridViewTextBoxColumn1.Name = "PalletIDDataGridViewTextBoxColumn1"
        Me.PalletIDDataGridViewTextBoxColumn1.ReadOnly = True
        Me.PalletIDDataGridViewTextBoxColumn1.Width = 110
        '
        'ShopOrderDataGridViewTextBoxColumn1
        '
        Me.ShopOrderDataGridViewTextBoxColumn1.DataPropertyName = "ShopOrder"
        Me.ShopOrderDataGridViewTextBoxColumn1.HeaderText = "Shop Order"
        Me.ShopOrderDataGridViewTextBoxColumn1.Name = "ShopOrderDataGridViewTextBoxColumn1"
        Me.ShopOrderDataGridViewTextBoxColumn1.ReadOnly = True
        Me.ShopOrderDataGridViewTextBoxColumn1.Width = 120
        '
        'ItemNumberDataGridViewTextBoxColumn1
        '
        Me.ItemNumberDataGridViewTextBoxColumn1.DataPropertyName = "ItemNumber"
        Me.ItemNumberDataGridViewTextBoxColumn1.HeaderText = "Item"
        Me.ItemNumberDataGridViewTextBoxColumn1.Name = "ItemNumberDataGridViewTextBoxColumn1"
        Me.ItemNumberDataGridViewTextBoxColumn1.ReadOnly = True
        '
        'QuantityDataGridViewTextBoxColumn1
        '
        Me.QuantityDataGridViewTextBoxColumn1.DataPropertyName = "Quantity"
        Me.QuantityDataGridViewTextBoxColumn1.HeaderText = "Quantity"
        Me.QuantityDataGridViewTextBoxColumn1.Name = "QuantityDataGridViewTextBoxColumn1"
        Me.QuantityDataGridViewTextBoxColumn1.ReadOnly = True
        Me.QuantityDataGridViewTextBoxColumn1.Width = 90
        '
        'OutputLocation
        '
        Me.OutputLocation.DataPropertyName = "OutputLocation"
        Me.OutputLocation.HeaderText = "Loc."
        Me.OutputLocation.Name = "OutputLocation"
        Me.OutputLocation.ReadOnly = True
        Me.OutputLocation.Width = 80
        '
        'OrderCompleteDataGridViewTextBoxColumn1
        '
        Me.OrderCompleteDataGridViewTextBoxColumn1.DataPropertyName = "OrderComplete"
        Me.OrderCompleteDataGridViewTextBoxColumn1.HeaderText = "Closed"
        Me.OrderCompleteDataGridViewTextBoxColumn1.Name = "OrderCompleteDataGridViewTextBoxColumn1"
        Me.OrderCompleteDataGridViewTextBoxColumn1.ReadOnly = True
        Me.OrderCompleteDataGridViewTextBoxColumn1.Width = 80
        '
        'CreationDateTimeDataGridViewTextBoxColumn1
        '
        Me.CreationDateTimeDataGridViewTextBoxColumn1.DataPropertyName = "CreationDateTime"
        DataGridViewCellStyle3.Format = "MM/dd/yyyy HH:mm:ss"
        DataGridViewCellStyle3.NullValue = Nothing
        Me.CreationDateTimeDataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle3
        Me.CreationDateTimeDataGridViewTextBoxColumn1.HeaderText = "Creation Time"
        Me.CreationDateTimeDataGridViewTextBoxColumn1.Name = "CreationDateTimeDataGridViewTextBoxColumn1"
        Me.CreationDateTimeDataGridViewTextBoxColumn1.ReadOnly = True
        Me.CreationDateTimeDataGridViewTextBoxColumn1.Width = 190
        '
        'frmPalletInquiry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.LblNoOfPallets)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblPkgLine)
        Me.Controls.Add(Me.txtPkgLine)
        Me.Controls.Add(Me.dgvPalletProduced)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPalletInquiry"
        Me.Text = "Pallets Produced"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvPalletProduced, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PPspPalletSelBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsPalletsOnServer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents dgvPalletProduced As System.Windows.Forms.DataGridView
    Friend WithEvents txtPkgLine As System.Windows.Forms.TextBox
    Friend WithEvents lblPkgLine As System.Windows.Forms.Label
    Friend WithEvents PPspPalletSelBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DsPalletsOnServer As PowerPlant.dsPalletsOnServer
    Friend WithEvents PalletIDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ShopOrderDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ItemNumberDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents QuantityDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OrderCompleteDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CreationDateTimeDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PPsp_Pallet_SelTableAdapter As PowerPlant.dsPalletsOnServerTableAdapters.PPsp_Pallet_SelTableAdapter
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LblNoOfPallets As System.Windows.Forms.Label
    Friend WithEvents PalletIDDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ShopOrderDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ItemNumberDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents QuantityDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OutputLocation As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OrderCompleteDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CreationDateTimeDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
