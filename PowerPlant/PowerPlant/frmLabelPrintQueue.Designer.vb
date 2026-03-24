<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLabelPrintQueue
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.dgvPrintQueue = New System.Windows.Forms.DataGridView()
        Me.PPspLabelPrintJobsSelBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsLabelPrintJobs = New PowerPlant.dsLabelPrintJobs()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.PPsp_LabelPrintJobs_SelTableAdapter = New PowerPlant.dsLabelPrintJobsTableAdapters.PPsp_LabelPrintJobs_SelTableAdapter()
        Me.RowNumberDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DeviceNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.JobNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TimeSubmitDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvPrintQueue, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PPspLabelPrintJobsSelBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsLabelPrintJobs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        'btnPrvScn
        '
        Me.btnPrvScn.BackColor = System.Drawing.Color.Silver
        Me.btnPrvScn.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrvScn.Location = New System.Drawing.Point(27, 490)
        Me.btnPrvScn.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrvScn.Name = "btnPrvScn"
        Me.btnPrvScn.Size = New System.Drawing.Size(150, 65)
        Me.btnPrvScn.TabIndex = 66
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'dgvPrintQueue
        '
        Me.dgvPrintQueue.AllowUserToAddRows = False
        Me.dgvPrintQueue.AllowUserToDeleteRows = False
        Me.dgvPrintQueue.AllowUserToResizeColumns = False
        Me.dgvPrintQueue.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue
        Me.dgvPrintQueue.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvPrintQueue.AutoGenerateColumns = False
        Me.dgvPrintQueue.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 8.75!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPrintQueue.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvPrintQueue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPrintQueue.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.RowNumberDataGridViewTextBoxColumn, Me.DeviceNameDataGridViewTextBoxColumn, Me.JobNameDataGridViewTextBoxColumn, Me.TimeSubmitDataGridViewTextBoxColumn})
        Me.dgvPrintQueue.DataSource = Me.PPspLabelPrintJobsSelBindingSource
        Me.dgvPrintQueue.Location = New System.Drawing.Point(0, 55)
        Me.dgvPrintQueue.Name = "dgvPrintQueue"
        Me.dgvPrintQueue.ReadOnly = True
        Me.dgvPrintQueue.RowHeadersVisible = False
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Tahoma", 10.0!)
        Me.dgvPrintQueue.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvPrintQueue.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Tahoma", 10.0!)
        Me.dgvPrintQueue.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvPrintQueue.RowTemplate.Height = 30
        Me.dgvPrintQueue.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvPrintQueue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgvPrintQueue.Size = New System.Drawing.Size(800, 420)
        Me.dgvPrintQueue.TabIndex = 68
        Me.dgvPrintQueue.TabStop = False
        '
        'PPspLabelPrintJobsSelBindingSource
        '
        Me.PPspLabelPrintJobsSelBindingSource.DataMember = "PPsp_LabelPrintJobs_Sel"
        Me.PPspLabelPrintJobsSelBindingSource.DataSource = Me.DsLabelPrintJobs
        '
        'DsLabelPrintJobs
        '
        Me.DsLabelPrintJobs.DataSetName = "dsLabelPrintJobs"
        Me.DsLabelPrintJobs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.Silver
        Me.btnRefresh.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.Location = New System.Drawing.Point(221, 490)
        Me.btnRefresh.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(150, 65)
        Me.btnRefresh.TabIndex = 69
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'PPsp_LabelPrintJobs_SelTableAdapter
        '
        Me.PPsp_LabelPrintJobs_SelTableAdapter.ClearBeforeFill = True
        '
        'RowNumberDataGridViewTextBoxColumn
        '
        Me.RowNumberDataGridViewTextBoxColumn.DataPropertyName = "RowNumber"
        Me.RowNumberDataGridViewTextBoxColumn.HeaderText = "Seq."
        Me.RowNumberDataGridViewTextBoxColumn.Name = "RowNumberDataGridViewTextBoxColumn"
        Me.RowNumberDataGridViewTextBoxColumn.ReadOnly = True
        Me.RowNumberDataGridViewTextBoxColumn.Width = 60
        '
        'DeviceNameDataGridViewTextBoxColumn
        '
        Me.DeviceNameDataGridViewTextBoxColumn.DataPropertyName = "DeviceName"
        Me.DeviceNameDataGridViewTextBoxColumn.HeaderText = "Device Name"
        Me.DeviceNameDataGridViewTextBoxColumn.Name = "DeviceNameDataGridViewTextBoxColumn"
        Me.DeviceNameDataGridViewTextBoxColumn.ReadOnly = True
        Me.DeviceNameDataGridViewTextBoxColumn.Width = 320
        '
        'JobNameDataGridViewTextBoxColumn
        '
        Me.JobNameDataGridViewTextBoxColumn.DataPropertyName = "JobName"
        Me.JobNameDataGridViewTextBoxColumn.HeaderText = "Job Name"
        Me.JobNameDataGridViewTextBoxColumn.Name = "JobNameDataGridViewTextBoxColumn"
        Me.JobNameDataGridViewTextBoxColumn.ReadOnly = True
        Me.JobNameDataGridViewTextBoxColumn.Width = 200
        '
        'TimeSubmitDataGridViewTextBoxColumn
        '
        Me.TimeSubmitDataGridViewTextBoxColumn.DataPropertyName = "TimeSubmit"
        Me.TimeSubmitDataGridViewTextBoxColumn.HeaderText = "Time Submit"
        Me.TimeSubmitDataGridViewTextBoxColumn.Name = "TimeSubmitDataGridViewTextBoxColumn"
        Me.TimeSubmitDataGridViewTextBoxColumn.ReadOnly = True
        Me.TimeSubmitDataGridViewTextBoxColumn.Width = 200
        '
        'frmLabelPrintQueue
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.dgvPrintQueue)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLabelPrintQueue"
        Me.Text = "Label Print Queue"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvPrintQueue, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PPspLabelPrintJobsSelBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsLabelPrintJobs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents dgvPrintQueue As System.Windows.Forms.DataGridView
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents PPspLabelPrintJobsSelBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DsLabelPrintJobs As PowerPlant.dsLabelPrintJobs
    Friend WithEvents PPsp_LabelPrintJobs_SelTableAdapter As PowerPlant.dsLabelPrintJobsTableAdapters.PPsp_LabelPrintJobs_SelTableAdapter
    Friend WithEvents RowNumberDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DeviceNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents JobNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TimeSubmitDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
