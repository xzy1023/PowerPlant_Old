<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCheckConnection
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
        Me.btnCheckNetworkConn = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.dgvPrtDev = New System.Windows.Forms.DataGridView()
        Me.TblPkgLinePrinterDeviceBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsPrinterDevice = New PowerPlant.dsPrinterDevice()
        Me.TaPrinterDevice = New PowerPlant.dsPrinterDeviceTableAdapters.taPrinterDevice()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.dgvPrtDev_txtPrtName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvPrtDev_txtIPAddress = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvPrtDev_btnTest = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.dgvPrtDev_txtResult = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvPrtDev, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TblPkgLinePrinterDeviceBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsPrinterDevice, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCheckNetworkConn
        '
        Me.btnCheckNetworkConn.BackColor = System.Drawing.Color.Silver
        Me.btnCheckNetworkConn.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCheckNetworkConn.Location = New System.Drawing.Point(201, 490)
        Me.btnCheckNetworkConn.Name = "btnCheckNetworkConn"
        Me.btnCheckNetworkConn.Size = New System.Drawing.Size(152, 65)
        Me.btnCheckNetworkConn.TabIndex = 13
        Me.btnCheckNetworkConn.Text = "Test IPC Connection"
        Me.btnCheckNetworkConn.UseVisualStyleBackColor = False
        '
        'btnExit
        '
        Me.btnExit.BackColor = System.Drawing.Color.Silver
        Me.btnExit.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(27, 490)
        Me.btnExit.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(150, 65)
        Me.btnExit.TabIndex = 78
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = False
        '
        'dgvPrtDev
        '
        Me.dgvPrtDev.AllowUserToAddRows = False
        Me.dgvPrtDev.AllowUserToDeleteRows = False
        Me.dgvPrtDev.AllowUserToResizeColumns = False
        Me.dgvPrtDev.AllowUserToResizeRows = False
        Me.dgvPrtDev.AutoGenerateColumns = False
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
        Me.dgvPrtDev.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgvPrtDev_txtPrtName, Me.dgvPrtDev_txtIPAddress, Me.dgvPrtDev_btnTest, Me.dgvPrtDev_txtResult})
        Me.dgvPrtDev.DataSource = Me.TblPkgLinePrinterDeviceBindingSource
        Me.dgvPrtDev.Location = New System.Drawing.Point(3, 56)
        Me.dgvPrtDev.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.dgvPrtDev.Name = "dgvPrtDev"
        Me.dgvPrtDev.ReadOnly = True
        Me.dgvPrtDev.RowHeadersVisible = False
        Me.dgvPrtDev.RowHeadersWidth = 10
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Tahoma", 15.75!)
        Me.dgvPrtDev.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvPrtDev.RowTemplate.Height = 65
        Me.dgvPrtDev.RowTemplate.ReadOnly = True
        Me.dgvPrtDev.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPrtDev.Size = New System.Drawing.Size(790, 423)
        Me.dgvPrtDev.TabIndex = 79
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
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(0, 0)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "-- Heading --"
        Me.UcHeading1.Size = New System.Drawing.Size(800, 55)
        Me.UcHeading1.TabIndex = 14
        '
        'dgvPrtDev_txtPrtName
        '
        Me.dgvPrtDev_txtPrtName.DataPropertyName = "DeviceName"
        Me.dgvPrtDev_txtPrtName.HeaderText = "Printer Name"
        Me.dgvPrtDev_txtPrtName.Name = "dgvPrtDev_txtPrtName"
        Me.dgvPrtDev_txtPrtName.ReadOnly = True
        Me.dgvPrtDev_txtPrtName.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPrtDev_txtPrtName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.dgvPrtDev_txtPrtName.Width = 430
        '
        'dgvPrtDev_txtIPAddress
        '
        Me.dgvPrtDev_txtIPAddress.DataPropertyName = "IPAddress"
        Me.dgvPrtDev_txtIPAddress.HeaderText = "IP Address"
        Me.dgvPrtDev_txtIPAddress.Name = "dgvPrtDev_txtIPAddress"
        Me.dgvPrtDev_txtIPAddress.ReadOnly = True
        Me.dgvPrtDev_txtIPAddress.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPrtDev_txtIPAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.dgvPrtDev_txtIPAddress.Width = 160
        '
        'dgvPrtDev_btnTest
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.dgvPrtDev_btnTest.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvPrtDev_btnTest.HeaderText = "Action"
        Me.dgvPrtDev_btnTest.Name = "dgvPrtDev_btnTest"
        Me.dgvPrtDev_btnTest.ReadOnly = True
        Me.dgvPrtDev_btnTest.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPrtDev_btnTest.Text = "Test"
        Me.dgvPrtDev_btnTest.ToolTipText = "Click to check connection"
        Me.dgvPrtDev_btnTest.UseColumnTextForButtonValue = True
        Me.dgvPrtDev_btnTest.Width = 85
        '
        'dgvPrtDev_txtResult
        '
        Me.dgvPrtDev_txtResult.HeaderText = "Result"
        Me.dgvPrtDev_txtResult.Name = "dgvPrtDev_txtResult"
        Me.dgvPrtDev_txtResult.ReadOnly = True
        Me.dgvPrtDev_txtResult.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPrtDev_txtResult.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.dgvPrtDev_txtResult.Width = 90
        '
        'frmCheckConnection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.dgvPrtDev)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.UcHeading1)
        Me.Controls.Add(Me.btnCheckNetworkConn)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCheckConnection"
        Me.Text = "Log On"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvPrtDev, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TblPkgLinePrinterDeviceBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsPrinterDevice, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCheckNetworkConn As System.Windows.Forms.Button
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents dgvPrtDev As System.Windows.Forms.DataGridView
    Friend WithEvents TblPkgLinePrinterDeviceBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DsPrinterDevice As PowerPlant.dsPrinterDevice
    Friend WithEvents TaPrinterDevice As PowerPlant.dsPrinterDeviceTableAdapters.taPrinterDevice
    Friend WithEvents dgvPrtDev_txtPrtName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvPrtDev_txtIPAddress As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvPrtDev_btnTest As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents dgvPrtDev_txtResult As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
