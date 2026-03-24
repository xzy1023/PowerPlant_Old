<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCfgIPCViewer
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
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboComputerName = New System.Windows.Forms.ComboBox()
        Me.tblComputerConfigBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsComputerCfg = New IPCInitialPgm.dsComputerCfg()
        Me.btnAccept = New System.Windows.Forms.Button()
        Me.tblComputerConfigTableAdapter = New IPCInitialPgm.dsComputerCfgTableAdapters.tblComputerConfigTableAdapter()
        Me.UcHeading1 = New IPCInitialPgm.ucHeading()
        Me.lblMsg = New System.Windows.Forms.Label()
        Me.cboFacility = New System.Windows.Forms.ComboBox()
        Me.PPspFacilitySelBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsFacilityBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsFacility = New IPCInitialPgm.dsFacility()
        Me.PPsp_Facility_SelTableAdapter = New IPCInitialPgm.dsFacilityTableAdapters.PPsp_Facility_SelTableAdapter()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.tblComputerConfigBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsComputerCfg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PPspFacilitySelBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsFacilityBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsFacility, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnPrvScn
        '
        Me.btnPrvScn.BackColor = System.Drawing.Color.Silver
        Me.btnPrvScn.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrvScn.Location = New System.Drawing.Point(27, 490)
        Me.btnPrvScn.Name = "btnPrvScn"
        Me.btnPrvScn.Size = New System.Drawing.Size(150, 65)
        Me.btnPrvScn.TabIndex = 11
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(37, 93)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(442, 29)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Facility of IPC Viewer To Be Replaced"
        '
        'cboComputerName
        '
        Me.cboComputerName.BackColor = System.Drawing.Color.Black
        Me.cboComputerName.DataSource = Me.tblComputerConfigBindingSource
        Me.cboComputerName.DisplayMember = "PkgLineDescription"
        Me.cboComputerName.DropDownHeight = 320
        Me.cboComputerName.DropDownWidth = 400
        Me.cboComputerName.Font = New System.Drawing.Font("Arial", 26.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboComputerName.ForeColor = System.Drawing.Color.White
        Me.cboComputerName.FormattingEnabled = True
        Me.cboComputerName.IntegralHeight = False
        Me.cboComputerName.ItemHeight = 40
        Me.cboComputerName.Location = New System.Drawing.Point(42, 278)
        Me.cboComputerName.MaxDropDownItems = 7
        Me.cboComputerName.MaxLength = 10
        Me.cboComputerName.Name = "cboComputerName"
        Me.cboComputerName.Size = New System.Drawing.Size(734, 48)
        Me.cboComputerName.TabIndex = 49
        Me.cboComputerName.ValueMember = "ComputerName"
        '
        'tblComputerConfigBindingSource
        '
        Me.tblComputerConfigBindingSource.DataMember = "tblComputerConfig"
        Me.tblComputerConfigBindingSource.DataSource = Me.DsComputerCfg
        '
        'DsComputerCfg
        '
        Me.DsComputerCfg.DataSetName = "dsComputerCfg"
        Me.DsComputerCfg.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'btnAccept
        '
        Me.btnAccept.BackColor = System.Drawing.Color.Silver
        Me.btnAccept.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnAccept.Location = New System.Drawing.Point(201, 490)
        Me.btnAccept.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(150, 65)
        Me.btnAccept.TabIndex = 50
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = False
        '
        'tblComputerConfigTableAdapter
        '
        Me.tblComputerConfigTableAdapter.ClearBeforeFill = True
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(-3, -1)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "-- Heading --"
        Me.UcHeading1.Size = New System.Drawing.Size(800, 55)
        Me.UcHeading1.TabIndex = 0
        '
        'lblMsg
        '
        Me.lblMsg.AutoSize = True
        Me.lblMsg.BackColor = System.Drawing.Color.CornflowerBlue
        Me.lblMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMsg.ForeColor = System.Drawing.Color.LightSalmon
        Me.lblMsg.Location = New System.Drawing.Point(22, 565)
        Me.lblMsg.Name = "lblMsg"
        Me.lblMsg.Size = New System.Drawing.Size(92, 29)
        Me.lblMsg.TabIndex = 51
        Me.lblMsg.Text = "Label2"
        '
        'cboFacility
        '
        Me.cboFacility.BackColor = System.Drawing.Color.Black
        Me.cboFacility.DataSource = Me.PPspFacilitySelBindingSource
        Me.cboFacility.DisplayMember = "Description"
        Me.cboFacility.Font = New System.Drawing.Font("Arial", 26.25!)
        Me.cboFacility.ForeColor = System.Drawing.Color.White
        Me.cboFacility.FormattingEnabled = True
        Me.cboFacility.Location = New System.Drawing.Point(42, 139)
        Me.cboFacility.Name = "cboFacility"
        Me.cboFacility.Size = New System.Drawing.Size(429, 48)
        Me.cboFacility.TabIndex = 1
        Me.cboFacility.ValueMember = "Facility"
        '
        'PPspFacilitySelBindingSource
        '
        Me.PPspFacilitySelBindingSource.DataMember = "PPsp_Facility_Sel"
        Me.PPspFacilitySelBindingSource.DataSource = Me.DsFacilityBindingSource
        '
        'DsFacilityBindingSource
        '
        Me.DsFacilityBindingSource.DataSource = Me.DsFacility
        Me.DsFacilityBindingSource.Position = 0
        '
        'DsFacility
        '
        Me.DsFacility.DataSetName = "dsFacility"
        Me.DsFacility.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'PPsp_Facility_SelTableAdapter
        '
        Me.PPsp_Facility_SelTableAdapter.ClearBeforeFill = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(37, 227)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(242, 29)
        Me.Label2.TabIndex = 52
        Me.Label2.Text = "Replace IPC Viewer:"
        '
        'frmCfgIPCViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cboFacility)
        Me.Controls.Add(Me.lblMsg)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.cboComputerName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCfgIPCViewer"
        Me.Text = "Configure IPC"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.tblComputerConfigBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsComputerCfg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PPspFacilitySelBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsFacilityBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsFacility, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As IPCInitialPgm.ucHeading
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboComputerName As System.Windows.Forms.ComboBox
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents DsComputerCfg As IPCInitialPgm.dsComputerCfg
    Friend WithEvents tblComputerConfigTableAdapter As IPCInitialPgm.dsComputerCfgTableAdapters.tblComputerConfigTableAdapter
    Friend WithEvents tblComputerConfigBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents lblMsg As System.Windows.Forms.Label
    Friend WithEvents cboFacility As System.Windows.Forms.ComboBox
    Friend WithEvents PPspFacilitySelBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DsFacilityBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DsFacility As IPCInitialPgm.dsFacility
    Friend WithEvents PPsp_Facility_SelTableAdapter As IPCInitialPgm.dsFacilityTableAdapters.PPsp_Facility_SelTableAdapter
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
