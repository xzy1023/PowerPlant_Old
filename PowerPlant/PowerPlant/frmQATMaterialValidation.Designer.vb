<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmQATMaterialValidation
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvMaterialsValidation = New System.Windows.Forms.DataGridView()
        Me.ItemDesc1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ComponentItem = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Lot = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Verified = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OverrideID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TestTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CPPspBillOfMaterialsIOBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsBillOfMaterials = New PowerPlant.dsBillOfMaterials()
        Me.btnDone = New System.Windows.Forms.Button()
        Me.lblItemNo = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnOverride = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtMaterialID = New System.Windows.Forms.TextBox()
        Me.CPPsp_BillOfMaterialsIOTableAdapter = New PowerPlant.dsBillOfMaterialsTableAdapters.CPPsp_BillOfMaterialsIOTableAdapter()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.txtLotID = New System.Windows.Forms.TextBox()
        CType(Me.dgvMaterialsValidation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CPPspBillOfMaterialsIOBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsBillOfMaterials, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvMaterialsValidation
        '
        Me.dgvMaterialsValidation.AllowUserToAddRows = False
        Me.dgvMaterialsValidation.AllowUserToDeleteRows = False
        Me.dgvMaterialsValidation.AllowUserToResizeColumns = False
        Me.dgvMaterialsValidation.AllowUserToResizeRows = False
        Me.dgvMaterialsValidation.AutoGenerateColumns = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGoldenrodYellow
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvMaterialsValidation.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvMaterialsValidation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMaterialsValidation.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ItemDesc1, Me.ComponentItem, Me.Lot, Me.Verified, Me.OverrideID, Me.TestTime})
        Me.dgvMaterialsValidation.DataSource = Me.CPPspBillOfMaterialsIOBindingSource
        Me.dgvMaterialsValidation.Location = New System.Drawing.Point(17, 169)
        Me.dgvMaterialsValidation.Name = "dgvMaterialsValidation"
        Me.dgvMaterialsValidation.ReadOnly = True
        Me.dgvMaterialsValidation.RowHeadersVisible = False
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvMaterialsValidation.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvMaterialsValidation.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvMaterialsValidation.RowTemplate.Height = 30
        Me.dgvMaterialsValidation.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvMaterialsValidation.Size = New System.Drawing.Size(769, 415)
        Me.dgvMaterialsValidation.TabIndex = 74
        Me.dgvMaterialsValidation.TabStop = False
        '
        'ItemDesc1
        '
        Me.ItemDesc1.DataPropertyName = "ItemDesc1"
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ItemDesc1.DefaultCellStyle = DataGridViewCellStyle2
        Me.ItemDesc1.HeaderText = "Description"
        Me.ItemDesc1.Name = "ItemDesc1"
        Me.ItemDesc1.ReadOnly = True
        Me.ItemDesc1.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ItemDesc1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.ItemDesc1.Width = 320
        '
        'ComponentItem
        '
        Me.ComponentItem.DataPropertyName = "ComponentItem"
        Me.ComponentItem.HeaderText = "Component"
        Me.ComponentItem.Name = "ComponentItem"
        Me.ComponentItem.ReadOnly = True
        Me.ComponentItem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.ComponentItem.Width = 150
        '
        'Lot
        '
        Me.Lot.HeaderText = "Lot"
        Me.Lot.Name = "Lot"
        Me.Lot.ReadOnly = True
        Me.Lot.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Lot.Width = 130
        '
        'Verified
        '
        Me.Verified.HeaderText = "Verified"
        Me.Verified.Name = "Verified"
        Me.Verified.ReadOnly = True
        Me.Verified.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Verified.Width = 150
        '
        'OverrideID
        '
        Me.OverrideID.HeaderText = "OverrideID"
        Me.OverrideID.Name = "OverrideID"
        Me.OverrideID.ReadOnly = True
        Me.OverrideID.Visible = False
        '
        'TestTime
        '
        Me.TestTime.HeaderText = "TestTime"
        Me.TestTime.Name = "TestTime"
        Me.TestTime.ReadOnly = True
        Me.TestTime.Visible = False
        Me.TestTime.Width = 10
        '
        'CPPspBillOfMaterialsIOBindingSource
        '
        Me.CPPspBillOfMaterialsIOBindingSource.DataMember = "CPPsp_BillOfMaterialsIO"
        Me.CPPspBillOfMaterialsIOBindingSource.DataSource = Me.DsBillOfMaterials
        '
        'DsBillOfMaterials
        '
        Me.DsBillOfMaterials.DataSetName = "dsBillOfMaterials"
        Me.DsBillOfMaterials.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'btnDone
        '
        Me.btnDone.BackColor = System.Drawing.Color.Silver
        Me.btnDone.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDone.Location = New System.Drawing.Point(462, 98)
        Me.btnDone.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnDone.Name = "btnDone"
        Me.btnDone.Size = New System.Drawing.Size(150, 65)
        Me.btnDone.TabIndex = 73
        Me.btnDone.Text = "Done"
        Me.btnDone.UseVisualStyleBackColor = False
        '
        'lblItemNo
        '
        Me.lblItemNo.AutoSize = True
        Me.lblItemNo.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemNo.ForeColor = System.Drawing.Color.White
        Me.lblItemNo.Location = New System.Drawing.Point(622, 64)
        Me.lblItemNo.Name = "lblItemNo"
        Me.lblItemNo.Size = New System.Drawing.Size(81, 27)
        Me.lblItemNo.TabIndex = 72
        Me.lblItemNo.Text = "SKU #"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.White
        Me.Label17.Location = New System.Drawing.Point(455, 64)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(161, 27)
        Me.Label17.TabIndex = 71
        Me.Label17.Text = "SKU Number:"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(12, 118)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(82, 27)
        Me.Label16.TabIndex = 69
        Me.Label16.Text = "Lot ID:"
        '
        'btnOverride
        '
        Me.btnOverride.BackColor = System.Drawing.Color.Gold
        Me.btnOverride.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnOverride.ForeColor = System.Drawing.Color.Crimson
        Me.btnOverride.Location = New System.Drawing.Point(627, 98)
        Me.btnOverride.Name = "btnOverride"
        Me.btnOverride.Size = New System.Drawing.Size(150, 65)
        Me.btnOverride.TabIndex = 76
        Me.btnOverride.Text = "Override"
        Me.btnOverride.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(12, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(133, 27)
        Me.Label2.TabIndex = 77
        Me.Label2.Text = "Material ID:"
        '
        'txtMaterialID
        '
        Me.txtMaterialID.AcceptsReturn = True
        Me.txtMaterialID.BackColor = System.Drawing.Color.Black
        Me.txtMaterialID.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaterialID.ForeColor = System.Drawing.Color.White
        Me.txtMaterialID.Location = New System.Drawing.Point(146, 58)
        Me.txtMaterialID.MaxLength = 35
        Me.txtMaterialID.Multiline = True
        Me.txtMaterialID.Name = "txtMaterialID"
        Me.txtMaterialID.Size = New System.Drawing.Size(263, 38)
        Me.txtMaterialID.TabIndex = 78
        '
        'CPPsp_BillOfMaterialsIOTableAdapter
        '
        Me.CPPsp_BillOfMaterialsIOTableAdapter.ClearBeforeFill = True
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(0, -1)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "QAT Materials Validation"
        Me.UcHeading1.Size = New System.Drawing.Size(800, 55)
        Me.UcHeading1.TabIndex = 15
        '
        'txtLotID
        '
        Me.txtLotID.AcceptsReturn = True
        Me.txtLotID.BackColor = System.Drawing.Color.Black
        Me.txtLotID.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLotID.ForeColor = System.Drawing.Color.White
        Me.txtLotID.Location = New System.Drawing.Point(146, 115)
        Me.txtLotID.MaxLength = 35
        Me.txtLotID.Multiline = True
        Me.txtLotID.Name = "txtLotID"
        Me.txtLotID.Size = New System.Drawing.Size(263, 38)
        Me.txtLotID.TabIndex = 79
        '
        'frmQATMaterialValidation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtLotID)
        Me.Controls.Add(Me.txtMaterialID)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnOverride)
        Me.Controls.Add(Me.dgvMaterialsValidation)
        Me.Controls.Add(Me.btnDone)
        Me.Controls.Add(Me.lblItemNo)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmQATMaterialValidation"
        Me.Text = "QAT Materials Validation"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvMaterialsValidation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CPPspBillOfMaterialsIOBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsBillOfMaterials, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents dgvMaterialsValidation As System.Windows.Forms.DataGridView
    Friend WithEvents btnDone As System.Windows.Forms.Button
    Friend WithEvents lblItemNo As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btnOverride As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtMaterialID As System.Windows.Forms.TextBox
    Friend WithEvents CPPspBillOfMaterialsIOBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DsBillOfMaterials As PowerPlant.dsBillOfMaterials
    Friend WithEvents CPPsp_BillOfMaterialsIOTableAdapter As PowerPlant.dsBillOfMaterialsTableAdapters.CPPsp_BillOfMaterialsIOTableAdapter
    Friend WithEvents txtLotID As System.Windows.Forms.TextBox
    Friend WithEvents ItemDesc1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ComponentItem As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Lot As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Verified As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OverrideID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TestTime As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
