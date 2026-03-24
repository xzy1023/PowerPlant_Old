<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRePrtPalletLabels
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
        Dim DataGridViewCellStyle22 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle21 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.txtShopOrder = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtPkgLine = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.dgvPalletHst = New System.Windows.Forms.DataGridView()
        Me.dgvPalletHstBtnPrint = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.dgvPalletHstTxtPkgLine = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvPalletHstTxtShopOrder = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvPalletHstTxtItem = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvPalletHstTxtPalletID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvPalletHstTxtQuantity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvPalletHstTxtStartTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvPalletHstTxtFacility = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CPPsp_PalletHstIOBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsPalletHst = New PowerPlant.dsPalletHst()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtPalletID = New System.Windows.Forms.TextBox()
        Me.btnPrintPallet = New System.Windows.Forms.Button()
        Me.CPPsp_PalletHstIOTableAdapter = New PowerPlant.dsPalletHstTableAdapters.CPPsp_PalletHstIOTableAdapter()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.lblMessage = New System.Windows.Forms.Label()
        CType(Me.dgvPalletHst, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CPPsp_PalletHstIOBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsPalletHst, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtShopOrder
        '
        Me.txtShopOrder.BackColor = System.Drawing.Color.Black
        Me.txtShopOrder.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtShopOrder.ForeColor = System.Drawing.Color.White
        Me.txtShopOrder.Location = New System.Drawing.Point(213, 111)
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
        Me.Label1.Location = New System.Drawing.Point(30, 114)
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
        Me.txtPkgLine.Location = New System.Drawing.Point(213, 70)
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
        Me.Label7.Location = New System.Drawing.Point(30, 73)
        Me.Label7.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(228, 35)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Packaging Line:"
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
        Me.btnSearch.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(414, 74)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(150, 65)
        Me.btnSearch.TabIndex = 3
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = False
        '
        'dgvPalletHst
        '
        Me.dgvPalletHst.AllowUserToAddRows = False
        Me.dgvPalletHst.AllowUserToDeleteRows = False
        Me.dgvPalletHst.AutoGenerateColumns = False
        Me.dgvPalletHst.ColumnHeadersHeight = 35
        Me.dgvPalletHst.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgvPalletHstBtnPrint, Me.dgvPalletHstTxtPkgLine, Me.dgvPalletHstTxtShopOrder, Me.dgvPalletHstTxtItem, Me.dgvPalletHstTxtPalletID, Me.dgvPalletHstTxtQuantity, Me.dgvPalletHstTxtStartTime, Me.dgvPalletHstTxtFacility})
        Me.dgvPalletHst.DataSource = Me.CPPsp_PalletHstIOBindingSource
        Me.dgvPalletHst.Location = New System.Drawing.Point(32, 166)
        Me.dgvPalletHst.Name = "dgvPalletHst"
        Me.dgvPalletHst.RowHeadersVisible = False
        Me.dgvPalletHst.RowHeadersWidth = 51
        Me.dgvPalletHst.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        DataGridViewCellStyle22.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvPalletHst.RowsDefaultCellStyle = DataGridViewCellStyle22
        Me.dgvPalletHst.RowTemplate.Height = 65
        Me.dgvPalletHst.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPalletHst.Size = New System.Drawing.Size(749, 235)
        Me.dgvPalletHst.TabIndex = 16
        '
        'dgvPalletHstBtnPrint
        '
        DataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle21.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        DataGridViewCellStyle21.ForeColor = System.Drawing.Color.Black
        Me.dgvPalletHstBtnPrint.DefaultCellStyle = DataGridViewCellStyle21
        Me.dgvPalletHstBtnPrint.HeaderText = "Print"
        Me.dgvPalletHstBtnPrint.MinimumWidth = 6
        Me.dgvPalletHstBtnPrint.Name = "dgvPalletHstBtnPrint"
        Me.dgvPalletHstBtnPrint.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPalletHstBtnPrint.Text = "Print"
        Me.dgvPalletHstBtnPrint.UseColumnTextForButtonValue = True
        Me.dgvPalletHstBtnPrint.Width = 125
        '
        'dgvPalletHstTxtPkgLine
        '
        Me.dgvPalletHstTxtPkgLine.DataPropertyName = "DefaultPkgLine"
        Me.dgvPalletHstTxtPkgLine.HeaderText = "Pkg Line"
        Me.dgvPalletHstTxtPkgLine.MinimumWidth = 6
        Me.dgvPalletHstTxtPkgLine.Name = "dgvPalletHstTxtPkgLine"
        Me.dgvPalletHstTxtPkgLine.ReadOnly = True
        Me.dgvPalletHstTxtPkgLine.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPalletHstTxtPkgLine.Width = 120
        '
        'dgvPalletHstTxtShopOrder
        '
        Me.dgvPalletHstTxtShopOrder.DataPropertyName = "ShopOrder"
        Me.dgvPalletHstTxtShopOrder.HeaderText = "Shop Order"
        Me.dgvPalletHstTxtShopOrder.MinimumWidth = 6
        Me.dgvPalletHstTxtShopOrder.Name = "dgvPalletHstTxtShopOrder"
        Me.dgvPalletHstTxtShopOrder.ReadOnly = True
        Me.dgvPalletHstTxtShopOrder.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPalletHstTxtShopOrder.Width = 120
        '
        'dgvPalletHstTxtItem
        '
        Me.dgvPalletHstTxtItem.DataPropertyName = "ItemNumber"
        Me.dgvPalletHstTxtItem.HeaderText = "Item"
        Me.dgvPalletHstTxtItem.MinimumWidth = 6
        Me.dgvPalletHstTxtItem.Name = "dgvPalletHstTxtItem"
        Me.dgvPalletHstTxtItem.ReadOnly = True
        Me.dgvPalletHstTxtItem.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPalletHstTxtItem.Width = 140
        '
        'dgvPalletHstTxtPalletID
        '
        Me.dgvPalletHstTxtPalletID.DataPropertyName = "PalletID"
        Me.dgvPalletHstTxtPalletID.HeaderText = "Pallet"
        Me.dgvPalletHstTxtPalletID.MinimumWidth = 6
        Me.dgvPalletHstTxtPalletID.Name = "dgvPalletHstTxtPalletID"
        Me.dgvPalletHstTxtPalletID.ReadOnly = True
        Me.dgvPalletHstTxtPalletID.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPalletHstTxtPalletID.Width = 150
        '
        'dgvPalletHstTxtQuantity
        '
        Me.dgvPalletHstTxtQuantity.DataPropertyName = "Quantity"
        Me.dgvPalletHstTxtQuantity.HeaderText = "Quantity"
        Me.dgvPalletHstTxtQuantity.MinimumWidth = 6
        Me.dgvPalletHstTxtQuantity.Name = "dgvPalletHstTxtQuantity"
        Me.dgvPalletHstTxtQuantity.ReadOnly = True
        Me.dgvPalletHstTxtQuantity.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPalletHstTxtQuantity.Width = 125
        '
        'dgvPalletHstTxtStartTime
        '
        Me.dgvPalletHstTxtStartTime.DataPropertyName = "StartTime"
        Me.dgvPalletHstTxtStartTime.HeaderText = "StartTime"
        Me.dgvPalletHstTxtStartTime.MinimumWidth = 6
        Me.dgvPalletHstTxtStartTime.Name = "dgvPalletHstTxtStartTime"
        Me.dgvPalletHstTxtStartTime.ReadOnly = True
        Me.dgvPalletHstTxtStartTime.Visible = False
        Me.dgvPalletHstTxtStartTime.Width = 125
        '
        'dgvPalletHstTxtFacility
        '
        Me.dgvPalletHstTxtFacility.DataPropertyName = "Facility"
        Me.dgvPalletHstTxtFacility.HeaderText = "Facility"
        Me.dgvPalletHstTxtFacility.MinimumWidth = 6
        Me.dgvPalletHstTxtFacility.Name = "dgvPalletHstTxtFacility"
        Me.dgvPalletHstTxtFacility.ReadOnly = True
        Me.dgvPalletHstTxtFacility.Visible = False
        Me.dgvPalletHstTxtFacility.Width = 125
        '
        'CPPsp_PalletHstIOBindingSource
        '
        Me.CPPsp_PalletHstIOBindingSource.DataMember = "CPPsp_PalletHstIO"
        Me.CPPsp_PalletHstIOBindingSource.DataSource = Me.DsPalletHst
        '
        'DsPalletHst
        '
        Me.DsPalletHst.DataSetName = "dsPalletHst"
        Me.DsPalletHst.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(37, 434)
        Me.Label2.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 35)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "- OR -"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(113, 434)
        Me.Label3.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(213, 35)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "Enter Pallet ID:"
        '
        'txtPalletID
        '
        Me.txtPalletID.BackColor = System.Drawing.Color.Black
        Me.txtPalletID.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtPalletID.ForeColor = System.Drawing.Color.White
        Me.txtPalletID.Location = New System.Drawing.Point(295, 431)
        Me.txtPalletID.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.txtPalletID.Name = "txtPalletID"
        Me.txtPalletID.Size = New System.Drawing.Size(156, 42)
        Me.txtPalletID.TabIndex = 18
        '
        'btnPrintPallet
        '
        Me.btnPrintPallet.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnPrintPallet.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold)
        Me.btnPrintPallet.Location = New System.Drawing.Point(473, 418)
        Me.btnPrintPallet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrintPallet.Name = "btnPrintPallet"
        Me.btnPrintPallet.Size = New System.Drawing.Size(116, 57)
        Me.btnPrintPallet.TabIndex = 13
        Me.btnPrintPallet.Text = "Print"
        Me.btnPrintPallet.UseVisualStyleBackColor = False
        '
        'CPPsp_PalletHstIOTableAdapter
        '
        Me.CPPsp_PalletHstIOTableAdapter.ClearBeforeFill = True
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
        Me.lblMessage.TabIndex = 90
        Me.lblMessage.Text = "Message"
        '
        'frmRePrtPalletLabels
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(13.0!, 28.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.txtPalletID)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dgvPalletHst)
        Me.Controls.Add(Me.UcHeading1)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.btnPrintPallet)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.txtPkgLine)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtShopOrder)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 14.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRePrtPalletLabels"
        Me.Text = "Reprint Pallet Labels"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvPalletHst, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CPPsp_PalletHstIOBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsPalletHst, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtShopOrder As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPkgLine As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents DsPalletHst As PowerPlant.dsPalletHst
    Friend WithEvents CPPsp_PalletHstIOBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents CPPsp_PalletHstIOTableAdapter As PowerPlant.dsPalletHstTableAdapters.CPPsp_PalletHstIOTableAdapter
    Friend WithEvents dgvPalletHst As System.Windows.Forms.DataGridView
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtPalletID As System.Windows.Forms.TextBox
    Friend WithEvents btnPrintPallet As System.Windows.Forms.Button
    Friend WithEvents dgvPalletHstBtnPrint As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents dgvPalletHstTxtPkgLine As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvPalletHstTxtShopOrder As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvPalletHstTxtItem As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvPalletHstTxtPalletID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvPalletHstTxtQuantity As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvPalletHstTxtStartTime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvPalletHstTxtFacility As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblMessage As System.Windows.Forms.Label
End Class
