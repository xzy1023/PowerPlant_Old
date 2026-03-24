<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBillOfMaterials
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
        Me.lblItemNo = New System.Windows.Forms.Label()
        Me.lblItemDesc = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.lblShopOrder = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.dgvBillOfMaterials = New System.Windows.Forms.DataGridView()
        Me.ItemDesc1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CPPsp_BillOfMaterialsIOBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsBillOfMaterials = New PowerPlant.dsBillOfMaterials()
        Me.CPPsp_BillOfMaterialsIOTableAdapter = New PowerPlant.dsBillOfMaterialsTableAdapters.CPPsp_BillOfMaterialsIOTableAdapter()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        CType(Me.dgvBillOfMaterials, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CPPsp_BillOfMaterialsIOBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsBillOfMaterials, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblItemNo
        '
        Me.lblItemNo.AutoSize = True
        Me.lblItemNo.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemNo.ForeColor = System.Drawing.Color.White
        Me.lblItemNo.Location = New System.Drawing.Point(501, 68)
        Me.lblItemNo.Name = "lblItemNo"
        Me.lblItemNo.Size = New System.Drawing.Size(81, 27)
        Me.lblItemNo.TabIndex = 64
        Me.lblItemNo.Text = "SKU #"
        '
        'lblItemDesc
        '
        Me.lblItemDesc.AutoSize = True
        Me.lblItemDesc.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemDesc.ForeColor = System.Drawing.Color.White
        Me.lblItemDesc.Location = New System.Drawing.Point(14, 106)
        Me.lblItemDesc.Name = "lblItemDesc"
        Me.lblItemDesc.Size = New System.Drawing.Size(162, 24)
        Me.lblItemDesc.TabIndex = 62
        Me.lblItemDesc.Text = "Item Description"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.White
        Me.Label17.Location = New System.Drawing.Point(334, 68)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(161, 27)
        Me.Label17.TabIndex = 63
        Me.Label17.Text = "SKU Number:"
        '
        'lblShopOrder
        '
        Me.lblShopOrder.AutoSize = True
        Me.lblShopOrder.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShopOrder.ForeColor = System.Drawing.Color.White
        Me.lblShopOrder.Location = New System.Drawing.Point(154, 68)
        Me.lblShopOrder.Name = "lblShopOrder"
        Me.lblShopOrder.Size = New System.Drawing.Size(116, 27)
        Me.lblShopOrder.TabIndex = 60
        Me.lblShopOrder.Text = "00000000"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(14, 68)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(145, 27)
        Me.Label16.TabIndex = 61
        Me.Label16.Text = "Shop Order:"
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
        'dgvBillOfMaterials
        '
        Me.dgvBillOfMaterials.AllowUserToAddRows = False
        Me.dgvBillOfMaterials.AllowUserToDeleteRows = False
        Me.dgvBillOfMaterials.AllowUserToResizeColumns = False
        Me.dgvBillOfMaterials.AllowUserToResizeRows = False
        Me.dgvBillOfMaterials.AutoGenerateColumns = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvBillOfMaterials.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvBillOfMaterials.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvBillOfMaterials.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ItemDesc1, Me.DataGridViewTextBoxColumn4, Me.DataGridViewTextBoxColumn6, Me.DataGridViewTextBoxColumn5})
        Me.dgvBillOfMaterials.DataSource = Me.CPPsp_BillOfMaterialsIOBindingSource
        Me.dgvBillOfMaterials.Location = New System.Drawing.Point(19, 147)
        Me.dgvBillOfMaterials.Name = "dgvBillOfMaterials"
        Me.dgvBillOfMaterials.RowHeadersVisible = False
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvBillOfMaterials.RowsDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvBillOfMaterials.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvBillOfMaterials.RowTemplate.Height = 30
        Me.dgvBillOfMaterials.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvBillOfMaterials.Size = New System.Drawing.Size(769, 441)
        Me.dgvBillOfMaterials.TabIndex = 67
        Me.dgvBillOfMaterials.TabStop = False
        '
        'ItemDesc1
        '
        Me.ItemDesc1.DataPropertyName = "ItemDesc1"
        Me.ItemDesc1.HeaderText = "Description"
        Me.ItemDesc1.Name = "ItemDesc1"
        Me.ItemDesc1.ReadOnly = True
        Me.ItemDesc1.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ItemDesc1.Width = 400
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "ComponentItem"
        Me.DataGridViewTextBoxColumn4.HeaderText = "Component"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 140
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "UnitOfMeasure"
        Me.DataGridViewTextBoxColumn6.HeaderText = "UM"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewTextBoxColumn6.Width = 50
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "Quantity"
        Me.DataGridViewTextBoxColumn5.HeaderText = "Quantity"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewTextBoxColumn5.Width = 150
        '
        'CPPsp_BillOfMaterialsIOBindingSource
        '
        Me.CPPsp_BillOfMaterialsIOBindingSource.DataMember = "CPPsp_BillOfMaterialsIO"
        Me.CPPsp_BillOfMaterialsIOBindingSource.DataSource = Me.DsBillOfMaterials
        '
        'DsBillOfMaterials
        '
        Me.DsBillOfMaterials.DataSetName = "dsBillOfMaterials"
        Me.DsBillOfMaterials.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'CPPsp_BillOfMaterialsIOTableAdapter
        '
        Me.CPPsp_BillOfMaterialsIOTableAdapter.ClearBeforeFill = True
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
        'frmBillOfMaterials
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.dgvBillOfMaterials)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.lblItemNo)
        Me.Controls.Add(Me.lblItemDesc)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.lblShopOrder)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmBillOfMaterials"
        Me.Text = "Bill Of Materials"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvBillOfMaterials, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CPPsp_BillOfMaterialsIOBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsBillOfMaterials, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents lblItemNo As System.Windows.Forms.Label
    Friend WithEvents lblItemDesc As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents lblShopOrder As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents DsBillOfMaterials As PowerPlant.dsBillOfMaterials
    Friend WithEvents CPPsp_BillOfMaterialsIOBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents CPPsp_BillOfMaterialsIOTableAdapter As PowerPlant.dsBillOfMaterialsTableAdapters.CPPsp_BillOfMaterialsIOTableAdapter
    Friend WithEvents dgvBillOfMaterials As System.Windows.Forms.DataGridView
    Friend WithEvents ItemDesc1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
