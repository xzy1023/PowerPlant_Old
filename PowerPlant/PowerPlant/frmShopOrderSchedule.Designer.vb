<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmShopOrderSchedule
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.dgvSOSchedule = New System.Windows.Forms.DataGridView()
        Me.strBlend = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.strItemDesc1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.strItemDesc2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ShopOrder = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ItemNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SODescription = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OrderQty = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StartDateTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CPPspShopOrderIOBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsShopOrder = New PowerPlant.dsShopOrder()
        Me.CPPsp_ShopOrderIOTableAdapter = New PowerPlant.dsShopOrderTableAdapters.CPPsp_ShopOrderIOTableAdapter()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.lblPkgLine = New System.Windows.Forms.Label()
        Me.txtPkgLine = New System.Windows.Forms.TextBox()
        CType(Me.dgvSOSchedule, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CPPspShopOrderIOBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsShopOrder, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'dgvSOSchedule
        '
        Me.dgvSOSchedule.AllowUserToAddRows = False
        Me.dgvSOSchedule.AllowUserToDeleteRows = False
        Me.dgvSOSchedule.AllowUserToResizeColumns = False
        Me.dgvSOSchedule.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue
        Me.dgvSOSchedule.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvSOSchedule.AutoGenerateColumns = False
        Me.dgvSOSchedule.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 8.75!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvSOSchedule.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvSOSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSOSchedule.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.strBlend, Me.strItemDesc1, Me.strItemDesc2, Me.ShopOrder, Me.ItemNumber, Me.SODescription, Me.OrderQty, Me.StartDateTime})
        Me.dgvSOSchedule.DataSource = Me.CPPspShopOrderIOBindingSource
        Me.dgvSOSchedule.Location = New System.Drawing.Point(19, 147)
        Me.dgvSOSchedule.Name = "dgvSOSchedule"
        Me.dgvSOSchedule.RowHeadersVisible = False
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Tahoma", 10.0!)
        Me.dgvSOSchedule.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvSOSchedule.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Tahoma", 10.0!)
        Me.dgvSOSchedule.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvSOSchedule.RowTemplate.Height = 30
        Me.dgvSOSchedule.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvSOSchedule.Size = New System.Drawing.Size(769, 437)
        Me.dgvSOSchedule.TabIndex = 67
        Me.dgvSOSchedule.TabStop = False
        '
        'strBlend
        '
        Me.strBlend.DataPropertyName = "Blend"
        Me.strBlend.HeaderText = "Blend"
        Me.strBlend.Name = "strBlend"
        Me.strBlend.ReadOnly = True
        Me.strBlend.Visible = False
        '
        'strItemDesc1
        '
        Me.strItemDesc1.DataPropertyName = "ItemDesc1"
        Me.strItemDesc1.HeaderText = "ItemDesc1"
        Me.strItemDesc1.Name = "strItemDesc1"
        Me.strItemDesc1.Visible = False
        '
        'strItemDesc2
        '
        Me.strItemDesc2.DataPropertyName = "ItemDesc2"
        Me.strItemDesc2.HeaderText = "ItemDesc2"
        Me.strItemDesc2.Name = "strItemDesc2"
        Me.strItemDesc2.Visible = False
        '
        'ShopOrder
        '
        Me.ShopOrder.DataPropertyName = "ShopOrder"
        Me.ShopOrder.HeaderText = "Shop Order"
        Me.ShopOrder.Name = "ShopOrder"
        Me.ShopOrder.ReadOnly = True
        Me.ShopOrder.Width = 90
        '
        'ItemNumber
        '
        Me.ItemNumber.DataPropertyName = "ItemNumber"
        Me.ItemNumber.HeaderText = "Item"
        Me.ItemNumber.Name = "ItemNumber"
        Me.ItemNumber.ReadOnly = True
        Me.ItemNumber.Width = 90
        '
        'SODescription
        '
        Me.SODescription.DataPropertyName = "SODescription"
        Me.SODescription.HeaderText = "Description"
        Me.SODescription.Name = "SODescription"
        Me.SODescription.ReadOnly = True
        Me.SODescription.Width = 400
        '
        'OrderQty
        '
        Me.OrderQty.DataPropertyName = "OrderQty"
        Me.OrderQty.HeaderText = "Order Qty"
        Me.OrderQty.Name = "OrderQty"
        Me.OrderQty.ReadOnly = True
        Me.OrderQty.Width = 85
        '
        'StartDateTime
        '
        Me.StartDateTime.DataPropertyName = "StartDateTime"
        Me.StartDateTime.HeaderText = "Start Time"
        Me.StartDateTime.Name = "StartDateTime"
        Me.StartDateTime.ReadOnly = True
        '
        'CPPspShopOrderIOBindingSource
        '
        Me.CPPspShopOrderIOBindingSource.DataMember = "CPPsp_ShopOrderIO"
        Me.CPPspShopOrderIOBindingSource.DataSource = Me.DsShopOrder
        '
        'DsShopOrder
        '
        Me.DsShopOrder.DataSetName = "dsShopOrder"
        Me.DsShopOrder.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'CPPsp_ShopOrderIOTableAdapter
        '
        Me.CPPsp_ShopOrderIOTableAdapter.ClearBeforeFill = True
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
        'txtPkgLine
        '
        Me.txtPkgLine.BackColor = System.Drawing.Color.Black
        Me.txtPkgLine.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPkgLine.ForeColor = System.Drawing.Color.White
        Me.txtPkgLine.Location = New System.Drawing.Point(198, 62)
        Me.txtPkgLine.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.txtPkgLine.MaxLength = 10
        Me.txtPkgLine.Name = "txtPkgLine"
        Me.txtPkgLine.Size = New System.Drawing.Size(129, 35)
        Me.txtPkgLine.TabIndex = 95
        '
        'frmShopOrderSchedule
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblPkgLine)
        Me.Controls.Add(Me.txtPkgLine)
        Me.Controls.Add(Me.dgvSOSchedule)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmShopOrderSchedule"
        Me.Text = "Shop Order Schedule"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvSOSchedule, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CPPspShopOrderIOBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsShopOrder, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents dgvSOSchedule As System.Windows.Forms.DataGridView
    Friend WithEvents CPPspShopOrderIOBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DsShopOrder As PowerPlant.dsShopOrder
    Friend WithEvents CPPsp_ShopOrderIOTableAdapter As PowerPlant.dsShopOrderTableAdapters.CPPsp_ShopOrderIOTableAdapter
    Friend WithEvents strBlend As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents strItemDesc1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents strItemDesc2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ShopOrder As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ItemNumber As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SODescription As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OrderQty As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents StartDateTime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblPkgLine As System.Windows.Forms.Label
    Friend WithEvents txtPkgLine As System.Windows.Forms.TextBox
End Class
