<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLogScrapsRejectPoint
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
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
        Me.dgvRejectPoints = New System.Windows.Forms.DataGridView()
        Me.CPPspEditComponentScrapBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsComponentScrap = New PowerPlant.dsComponentScrap()
        Me.btnAccept = New System.Windows.Forms.Button()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        CType(Me.dgvRejectPoints, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CPPspEditComponentScrapBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsComponentScrap, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblItemNo
        '
        Me.lblItemNo.AutoSize = True
        Me.lblItemNo.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemNo.ForeColor = System.Drawing.Color.White
        Me.lblItemNo.Location = New System.Drawing.Point(918, 126)
        Me.lblItemNo.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblItemNo.Name = "lblItemNo"
        Me.lblItemNo.Size = New System.Drawing.Size(141, 47)
        Me.lblItemNo.TabIndex = 64
        Me.lblItemNo.Text = "SKU #"
        '
        'lblItemDesc
        '
        Me.lblItemDesc.AutoSize = True
        Me.lblItemDesc.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemDesc.ForeColor = System.Drawing.Color.White
        Me.lblItemDesc.Location = New System.Drawing.Point(612, 175)
        Me.lblItemDesc.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblItemDesc.Name = "lblItemDesc"
        Me.lblItemDesc.Size = New System.Drawing.Size(324, 47)
        Me.lblItemDesc.TabIndex = 62
        Me.lblItemDesc.Text = "Item Description"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.White
        Me.Label17.Location = New System.Drawing.Point(612, 126)
        Me.Label17.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(279, 47)
        Me.Label17.TabIndex = 63
        Me.Label17.Text = "SKU Number:"
        '
        'lblShopOrder
        '
        Me.lblShopOrder.AutoSize = True
        Me.lblShopOrder.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShopOrder.ForeColor = System.Drawing.Color.White
        Me.lblShopOrder.Location = New System.Drawing.Point(282, 126)
        Me.lblShopOrder.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblShopOrder.Name = "lblShopOrder"
        Me.lblShopOrder.Size = New System.Drawing.Size(181, 47)
        Me.lblShopOrder.TabIndex = 60
        Me.lblShopOrder.Text = "0000000"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(26, 126)
        Me.Label16.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(248, 47)
        Me.Label16.TabIndex = 61
        Me.Label16.Text = "Shop Order:"
        '
        'btnPrvScn
        '
        Me.btnPrvScn.BackColor = System.Drawing.Color.Silver
        Me.btnPrvScn.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrvScn.Location = New System.Drawing.Point(50, 905)
        Me.btnPrvScn.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnPrvScn.Name = "btnPrvScn"
        Me.btnPrvScn.Size = New System.Drawing.Size(275, 120)
        Me.btnPrvScn.TabIndex = 65
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'dgvRejectPoints
        '
        Me.dgvRejectPoints.AllowUserToAddRows = False
        Me.dgvRejectPoints.AllowUserToDeleteRows = False
        Me.dgvRejectPoints.AllowUserToResizeColumns = False
        Me.dgvRejectPoints.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvRejectPoints.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvRejectPoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvRejectPoints.Location = New System.Drawing.Point(35, 246)
        Me.dgvRejectPoints.Margin = New System.Windows.Forms.Padding(6)
        Me.dgvRejectPoints.MultiSelect = False
        Me.dgvRejectPoints.Name = "dgvRejectPoints"
        Me.dgvRejectPoints.ReadOnly = True
        Me.dgvRejectPoints.RowHeadersVisible = False
        Me.dgvRejectPoints.RowHeadersWidth = 72
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvRejectPoints.RowsDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvRejectPoints.RowTemplate.Height = 55
        Me.dgvRejectPoints.Size = New System.Drawing.Size(1410, 583)
        Me.dgvRejectPoints.TabIndex = 67
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
        Me.btnAccept.Location = New System.Drawing.Point(390, 905)
        Me.btnAccept.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(275, 120)
        Me.btnAccept.TabIndex = 65
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = False
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(-6, 0)
        Me.UcHeading1.Margin = New System.Windows.Forms.Padding(11)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "-- Heading --"
        Me.UcHeading1.Size = New System.Drawing.Size(1467, 102)
        Me.UcHeading1.TabIndex = 0
        '
        'frmLogScrapsRejectPoint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(1467, 1108)
        Me.ControlBox = False
        Me.Controls.Add(Me.dgvRejectPoints)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.lblItemNo)
        Me.Controls.Add(Me.lblItemDesc)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.lblShopOrder)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(6)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLogScrapsRejectPoint"
        Me.Text = "Log Scraps"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvRejectPoints, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents dgvRejectPoints As System.Windows.Forms.DataGridView
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents CPPspEditComponentScrapBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DsComponentScrap As PowerPlant.dsComponentScrap
End Class
