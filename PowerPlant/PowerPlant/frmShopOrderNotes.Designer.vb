<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmShopOrderNotes
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
        Me.lblItemNo = New System.Windows.Forms.Label()
        Me.lblItemDesc = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.lblShopOrder = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.DsItemNotes = New PowerPlant.dsItemNotes()
        Me.CPPsp_ItemNotesIOBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.CPPsp_ItemNotesIOTableAdapter = New PowerPlant.dsItemNotesTableAdapters.CPPsp_ItemNotesIOTableAdapter()
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        CType(Me.DsItemNotes, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CPPsp_ItemNotesIOBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.lblItemDesc.Location = New System.Drawing.Point(14, 107)
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
        Me.btnPrvScn.Location = New System.Drawing.Point(638, 68)
        Me.btnPrvScn.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrvScn.Name = "btnPrvScn"
        Me.btnPrvScn.Size = New System.Drawing.Size(150, 65)
        Me.btnPrvScn.TabIndex = 65
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'DsItemNotes
        '
        Me.DsItemNotes.DataSetName = "dsItemNotes"
        Me.DsItemNotes.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'CPPsp_ItemNotesIOBindingSource
        '
        Me.CPPsp_ItemNotesIOBindingSource.DataMember = "CPPsp_ItemNotesIO"
        Me.CPPsp_ItemNotesIOBindingSource.DataSource = Me.DsItemNotes
        '
        'CPPsp_ItemNotesIOTableAdapter
        '
        Me.CPPsp_ItemNotesIOTableAdapter.ClearBeforeFill = True
        '
        'txtNotes
        '
        Me.txtNotes.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtNotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNotes.Location = New System.Drawing.Point(18, 146)
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.ReadOnly = True
        Me.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtNotes.Size = New System.Drawing.Size(770, 438)
        Me.txtNotes.TabIndex = 68
        Me.txtNotes.WordWrap = False
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
        'frmShopOrderNotes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtNotes)
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
        Me.Name = "frmShopOrderNotes"
        Me.Text = "Shop Order Notes"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DsItemNotes, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CPPsp_ItemNotesIOBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents DsItemNotes As PowerPlant.dsItemNotes
    Friend WithEvents CPPsp_ItemNotesIOBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents CPPsp_ItemNotesIOTableAdapter As PowerPlant.dsItemNotesTableAdapters.CPPsp_ItemNotesIOTableAdapter
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txtNotes As System.Windows.Forms.TextBox
End Class
