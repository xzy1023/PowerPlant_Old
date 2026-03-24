<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrtPalletLabels
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
        Dim DataGridViewCellStyle20 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle17 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle18 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle19 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.txtShopOrder = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtPkgLine = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.dgvPallet = New System.Windows.Forms.DataGridView()
        Me.btnPrint = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.DefaultPkgLine = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ShopOrder = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PalletID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StartTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ProductionDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LotID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ItemNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Quantity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CreationDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OrderComplete = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PrintStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnDelete = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.TblPalletBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsPallet = New PowerPlant.dsPallet()
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.CPPsp_PalletIOTableAdapter = New PowerPlant.dsPalletTableAdapters.CPPsp_PalletIOTableAdapter()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.lblMessage = New System.Windows.Forms.Label()
        CType(Me.dgvPallet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TblPalletBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsPallet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtShopOrder
        '
        Me.txtShopOrder.BackColor = System.Drawing.Color.Black
        Me.txtShopOrder.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtShopOrder.ForeColor = System.Drawing.Color.White
        Me.txtShopOrder.Location = New System.Drawing.Point(222, 112)
        Me.txtShopOrder.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.txtShopOrder.Name = "txtShopOrder"
        Me.txtShopOrder.Size = New System.Drawing.Size(147, 42)
        Me.txtShopOrder.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(28, 115)
        Me.Label1.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(177, 35)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Shop Order:"
        '
        'txtPkgLine
        '
        Me.txtPkgLine.BackColor = System.Drawing.Color.Black
        Me.txtPkgLine.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtPkgLine.ForeColor = System.Drawing.Color.White
        Me.txtPkgLine.Location = New System.Drawing.Point(222, 71)
        Me.txtPkgLine.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.txtPkgLine.MaxLength = 10
        Me.txtPkgLine.Name = "txtPkgLine"
        Me.txtPkgLine.Size = New System.Drawing.Size(147, 42)
        Me.txtPkgLine.TabIndex = 1
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(28, 74)
        Me.Label7.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(228, 35)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Packaging Line:"
        '
        'dgvPallet
        '
        Me.dgvPallet.AllowUserToAddRows = False
        Me.dgvPallet.AllowUserToDeleteRows = False
        Me.dgvPallet.AllowUserToResizeColumns = False
        Me.dgvPallet.AllowUserToResizeRows = False
        Me.dgvPallet.AutoGenerateColumns = False
        Me.dgvPallet.ColumnHeadersHeight = 35
        Me.dgvPallet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvPallet.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.btnPrint, Me.DefaultPkgLine, Me.ShopOrder, Me.PalletID, Me.StartTime, Me.ProductionDate, Me.LotID, Me.ItemNumber, Me.Quantity, Me.CreationDate, Me.OrderComplete, Me.PrintStatus, Me.btnDelete})
        Me.dgvPallet.DataSource = Me.TblPalletBindingSource
        Me.dgvPallet.Location = New System.Drawing.Point(14, 159)
        Me.dgvPallet.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.dgvPallet.Name = "dgvPallet"
        Me.dgvPallet.RowHeadersVisible = False
        Me.dgvPallet.RowHeadersWidth = 10
        DataGridViewCellStyle20.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvPallet.RowsDefaultCellStyle = DataGridViewCellStyle20
        Me.dgvPallet.RowTemplate.Height = 65
        Me.dgvPallet.RowTemplate.ReadOnly = True
        Me.dgvPallet.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPallet.Size = New System.Drawing.Size(772, 293)
        Me.dgvPallet.TabIndex = 4
        '
        'btnPrint
        '
        DataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        DataGridViewCellStyle17.ForeColor = System.Drawing.Color.Black
        Me.btnPrint.DefaultCellStyle = DataGridViewCellStyle17
        Me.btnPrint.HeaderText = "Print"
        Me.btnPrint.MinimumWidth = 6
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.btnPrint.Text = "Print"
        Me.btnPrint.ToolTipText = "Print pallet in this row."
        Me.btnPrint.UseColumnTextForButtonValue = True
        Me.btnPrint.Width = 70
        '
        'DefaultPkgLine
        '
        Me.DefaultPkgLine.DataPropertyName = "DefaultPkgLine"
        Me.DefaultPkgLine.HeaderText = "Pkg Line"
        Me.DefaultPkgLine.MinimumWidth = 6
        Me.DefaultPkgLine.Name = "DefaultPkgLine"
        Me.DefaultPkgLine.ReadOnly = True
        Me.DefaultPkgLine.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DefaultPkgLine.Width = 125
        '
        'ShopOrder
        '
        Me.ShopOrder.DataPropertyName = "ShopOrder"
        Me.ShopOrder.HeaderText = "Shop Order"
        Me.ShopOrder.MinimumWidth = 6
        Me.ShopOrder.Name = "ShopOrder"
        Me.ShopOrder.ReadOnly = True
        Me.ShopOrder.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ShopOrder.Width = 115
        '
        'PalletID
        '
        Me.PalletID.DataPropertyName = "PalletID"
        Me.PalletID.HeaderText = "Pallet ID"
        Me.PalletID.MinimumWidth = 6
        Me.PalletID.Name = "PalletID"
        Me.PalletID.ReadOnly = True
        Me.PalletID.Visible = False
        Me.PalletID.Width = 6
        '
        'StartTime
        '
        Me.StartTime.DataPropertyName = "StartTime"
        Me.StartTime.HeaderText = "StartTime"
        Me.StartTime.MinimumWidth = 6
        Me.StartTime.Name = "StartTime"
        Me.StartTime.ReadOnly = True
        Me.StartTime.Visible = False
        Me.StartTime.Width = 125
        '
        'ProductionDate
        '
        Me.ProductionDate.DataPropertyName = "ProductionDate"
        Me.ProductionDate.HeaderText = "ProductionDate"
        Me.ProductionDate.MinimumWidth = 6
        Me.ProductionDate.Name = "ProductionDate"
        Me.ProductionDate.ReadOnly = True
        Me.ProductionDate.Visible = False
        Me.ProductionDate.Width = 125
        '
        'LotID
        '
        Me.LotID.DataPropertyName = "LotID"
        Me.LotID.HeaderText = "LotID"
        Me.LotID.MinimumWidth = 6
        Me.LotID.Name = "LotID"
        Me.LotID.ReadOnly = True
        Me.LotID.Visible = False
        Me.LotID.Width = 125
        '
        'ItemNumber
        '
        Me.ItemNumber.DataPropertyName = "ItemNumber"
        Me.ItemNumber.HeaderText = "Item"
        Me.ItemNumber.MinimumWidth = 6
        Me.ItemNumber.Name = "ItemNumber"
        Me.ItemNumber.ReadOnly = True
        Me.ItemNumber.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ItemNumber.Width = 130
        '
        'Quantity
        '
        Me.Quantity.DataPropertyName = "Quantity"
        Me.Quantity.HeaderText = "Qty"
        Me.Quantity.MinimumWidth = 6
        Me.Quantity.Name = "Quantity"
        Me.Quantity.ReadOnly = True
        Me.Quantity.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Quantity.Width = 60
        '
        'CreationDate
        '
        Me.CreationDate.DataPropertyName = "CreationDateTime"
        DataGridViewCellStyle18.Format = "MM/dd/yy HH:mm"
        DataGridViewCellStyle18.NullValue = Nothing
        Me.CreationDate.DefaultCellStyle = DataGridViewCellStyle18
        Me.CreationDate.HeaderText = "Create Date"
        Me.CreationDate.MinimumWidth = 6
        Me.CreationDate.Name = "CreationDate"
        Me.CreationDate.ReadOnly = True
        Me.CreationDate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.CreationDate.Width = 160
        '
        'OrderComplete
        '
        Me.OrderComplete.DataPropertyName = "OrderComplete"
        Me.OrderComplete.HeaderText = "Close"
        Me.OrderComplete.MinimumWidth = 6
        Me.OrderComplete.Name = "OrderComplete"
        Me.OrderComplete.ReadOnly = True
        Me.OrderComplete.Width = 55
        '
        'PrintStatus
        '
        Me.PrintStatus.DataPropertyName = "PrintStatus"
        Me.PrintStatus.HeaderText = "PrintStatus"
        Me.PrintStatus.MinimumWidth = 6
        Me.PrintStatus.Name = "PrintStatus"
        Me.PrintStatus.ReadOnly = True
        Me.PrintStatus.Visible = False
        Me.PrintStatus.Width = 125
        '
        'btnDelete
        '
        DataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle19.ForeColor = System.Drawing.Color.Red
        Me.btnDelete.DefaultCellStyle = DataGridViewCellStyle19
        Me.btnDelete.HeaderText = "Delete"
        Me.btnDelete.MinimumWidth = 6
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.ToolTipText = "Delete pallet in this row."
        Me.btnDelete.UseColumnTextForButtonValue = True
        Me.btnDelete.Width = 70
        '
        'TblPalletBindingSource
        '
        Me.TblPalletBindingSource.DataMember = "CPPsp_PalletIO"
        Me.TblPalletBindingSource.DataSource = Me.DsPallet
        '
        'DsPallet
        '
        Me.DsPallet.DataSetName = "dsPallet"
        Me.DsPallet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'btnPrvScn
        '
        Me.btnPrvScn.BackColor = System.Drawing.Color.Silver
        Me.btnPrvScn.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrvScn.Location = New System.Drawing.Point(27, 490)
        Me.btnPrvScn.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrvScn.Name = "btnPrvScn"
        Me.btnPrvScn.Size = New System.Drawing.Size(150, 65)
        Me.btnPrvScn.TabIndex = 13
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'btnSearch
        '
        Me.btnSearch.BackColor = System.Drawing.Color.Silver
        Me.btnSearch.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold)
        Me.btnSearch.Location = New System.Drawing.Point(412, 76)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(150, 65)
        Me.btnSearch.TabIndex = 3
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = False
        '
        'CPPsp_PalletIOTableAdapter
        '
        Me.CPPsp_PalletIOTableAdapter.ClearBeforeFill = True
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UcHeading1.Location = New System.Drawing.Point(0, 0)
        Me.UcHeading1.Margin = New System.Windows.Forms.Padding(5)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "-- Heading --"
        Me.UcHeading1.Size = New System.Drawing.Size(1029, 57)
        Me.UcHeading1.TabIndex = 14
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ForeColor = System.Drawing.Color.LightSalmon
        Me.lblMessage.Location = New System.Drawing.Point(22, 565)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(135, 35)
        Me.lblMessage.TabIndex = 91
        Me.lblMessage.Text = "Message"
        '
        'frmPrtPalletLabels
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(13.0!, 28.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.UcHeading1)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.dgvPallet)
        Me.Controls.Add(Me.txtPkgLine)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtShopOrder)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 14.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPrtPalletLabels"
        Me.Text = "Print Pallet Labels"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvPallet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TblPalletBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsPallet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtShopOrder As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPkgLine As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents DsPallet As PowerPlant.dsPallet
    Friend WithEvents TblPalletBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents CPPsp_PalletIOTableAdapter As PowerPlant.dsPalletTableAdapters.CPPsp_PalletIOTableAdapter
    Friend WithEvents dgvPallet As System.Windows.Forms.DataGridView
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents btnPrint As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents DefaultPkgLine As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ShopOrder As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PalletID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents StartTime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ProductionDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LotID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ItemNumber As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Quantity As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CreationDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OrderComplete As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PrintStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnDelete As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents lblMessage As System.Windows.Forms.Label
End Class
