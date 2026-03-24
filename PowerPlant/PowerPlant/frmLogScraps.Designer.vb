<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLogScraps
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
        Me.lblItemNo = New System.Windows.Forms.Label()
        Me.lblItemDesc = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.lblShopOrder = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.dgvBOM = New System.Windows.Forms.DataGridView()
        Me.dgvBOMtxtComponent = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvBOMtxtDesc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvBOMtxtUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvBOMtxtQty = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CPPspEditComponentScrapBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsComponentScrap = New PowerPlant.dsComponentScrap()
        Me.btnAccept = New System.Windows.Forms.Button()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        CType(Me.dgvBOM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CPPspEditComponentScrapBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsComponentScrap, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.lblItemDesc.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemDesc.ForeColor = System.Drawing.Color.White
        Me.lblItemDesc.Location = New System.Drawing.Point(334, 95)
        Me.lblItemDesc.Name = "lblItemDesc"
        Me.lblItemDesc.Size = New System.Drawing.Size(185, 27)
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
        Me.lblShopOrder.Size = New System.Drawing.Size(103, 27)
        Me.lblShopOrder.TabIndex = 60
        Me.lblShopOrder.Text = "0000000"
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
        Me.btnPrvScn.Location = New System.Drawing.Point(27, 490)
        Me.btnPrvScn.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrvScn.Name = "btnPrvScn"
        Me.btnPrvScn.Size = New System.Drawing.Size(150, 65)
        Me.btnPrvScn.TabIndex = 65
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'dgvBOM
        '
        Me.dgvBOM.AllowUserToAddRows = False
        Me.dgvBOM.AllowUserToDeleteRows = False
        Me.dgvBOM.AutoGenerateColumns = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvBOM.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvBOM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvBOM.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgvBOMtxtComponent, Me.dgvBOMtxtDesc, Me.dgvBOMtxtUM, Me.dgvBOMtxtQty})
        Me.dgvBOM.DataSource = Me.CPPspEditComponentScrapBindingSource
        Me.dgvBOM.Location = New System.Drawing.Point(19, 133)
        Me.dgvBOM.Name = "dgvBOM"
        Me.dgvBOM.RowHeadersVisible = False
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvBOM.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvBOM.RowTemplate.Height = 55
        Me.dgvBOM.Size = New System.Drawing.Size(769, 316)
        Me.dgvBOM.TabIndex = 67
        '
        'dgvBOMtxtComponent
        '
        Me.dgvBOMtxtComponent.DataPropertyName = "ComponentItem"
        Me.dgvBOMtxtComponent.HeaderText = "Component"
        Me.dgvBOMtxtComponent.Name = "dgvBOMtxtComponent"
        Me.dgvBOMtxtComponent.ReadOnly = True
        Me.dgvBOMtxtComponent.Width = 190
        '
        'dgvBOMtxtDesc
        '
        Me.dgvBOMtxtDesc.DataPropertyName = "ItemDesc1"
        Me.dgvBOMtxtDesc.HeaderText = "Description"
        Me.dgvBOMtxtDesc.Name = "dgvBOMtxtDesc"
        Me.dgvBOMtxtDesc.ReadOnly = True
        Me.dgvBOMtxtDesc.Width = 350
        '
        'dgvBOMtxtUM
        '
        Me.dgvBOMtxtUM.DataPropertyName = "UnitOfMeasure"
        Me.dgvBOMtxtUM.HeaderText = "UM"
        Me.dgvBOMtxtUM.Name = "dgvBOMtxtUM"
        Me.dgvBOMtxtUM.ReadOnly = True
        Me.dgvBOMtxtUM.Width = 50
        '
        'dgvBOMtxtQty
        '
        Me.dgvBOMtxtQty.DataPropertyName = "Quantity"
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.dgvBOMtxtQty.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvBOMtxtQty.HeaderText = "Scrap Qty."
        Me.dgvBOMtxtQty.MaxInputLength = 10
        Me.dgvBOMtxtQty.Name = "dgvBOMtxtQty"
        Me.dgvBOMtxtQty.Width = 150
        '
        'CPPspEditComponentScrapBindingSource
        '
        Me.CPPspEditComponentScrapBindingSource.DataMember = "CPPsp_EditComponentScrap"
        Me.CPPspEditComponentScrapBindingSource.DataSource = Me.DsComponentScrap
        '
        'DsComponentScrap
        '
        Me.DsComponentScrap.DataSetName = "dsComponentScrap"
        Me.DsComponentScrap.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'btnAccept
        '
        Me.btnAccept.BackColor = System.Drawing.Color.Silver
        Me.btnAccept.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAccept.Location = New System.Drawing.Point(213, 490)
        Me.btnAccept.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(150, 65)
        Me.btnAccept.TabIndex = 65
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = False
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.Blue
        Me.UcHeading1.Location = New System.Drawing.Point(-3, 0)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "-- Heading --"
        Me.UcHeading1.Size = New System.Drawing.Size(800, 55)
        Me.UcHeading1.TabIndex = 0
        '
        'frmLogScraps
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.dgvBOM)
        Me.Controls.Add(Me.btnAccept)
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
        Me.Name = "frmLogScraps"
        Me.Text = "Log Scraps"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvBOM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CPPspEditComponentScrapBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsComponentScrap, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents dgvBOM As System.Windows.Forms.DataGridView
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents CPPspEditComponentScrapBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DsComponentScrap As PowerPlant.dsComponentScrap
    Friend WithEvents dgvBOMtxtComponent As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvBOMtxtDesc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvBOMtxtUM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvBOMtxtQty As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
