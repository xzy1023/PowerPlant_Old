<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmViewDTLog
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.dgvDownTimeLog = New System.Windows.Forms.DataGridView()
        Me.dvgTxtDTBegin = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MachineID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ReasonCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvTxtDuration = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Description = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Comment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TblDownTimeLogBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsDownTimeLog = New DownTime.dsDownTimeLog()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.TblDownTimeLogTableAdapter = New DownTime.dsDownTimeLogTableAdapters.CPPsp_DownTimeLog_SelTableAdapter()
        Me.btnOption = New System.Windows.Forms.Button()
        Me.lblShiftDuration = New System.Windows.Forms.Label()
        Me.txtShiftDuration = New System.Windows.Forms.TextBox()
        Me.UcHeading1 = New DownTime.ucHeading()
        CType(Me.dgvDownTimeLog, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TblDownTimeLogBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsDownTimeLog, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnPrvScn
        '
        Me.btnPrvScn.BackColor = System.Drawing.Color.Silver
        Me.btnPrvScn.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrvScn.Location = New System.Drawing.Point(27, 490)
        Me.btnPrvScn.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrvScn.Name = "btnPrvScn"
        Me.btnPrvScn.Size = New System.Drawing.Size(150, 65)
        Me.btnPrvScn.TabIndex = 12
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'dgvDownTimeLog
        '
        Me.dgvDownTimeLog.AllowUserToAddRows = False
        Me.dgvDownTimeLog.AllowUserToDeleteRows = False
        Me.dgvDownTimeLog.AllowUserToResizeColumns = False
        Me.dgvDownTimeLog.AllowUserToResizeRows = False
        Me.dgvDownTimeLog.AutoGenerateColumns = False
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvDownTimeLog.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvDownTimeLog.ColumnHeadersHeight = 30
        Me.dgvDownTimeLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvDownTimeLog.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dvgTxtDTBegin, Me.MachineID, Me.ReasonCode, Me.dgvTxtDuration, Me.Description, Me.Comment})
        Me.dgvDownTimeLog.DataSource = Me.TblDownTimeLogBindingSource
        Me.dgvDownTimeLog.Location = New System.Drawing.Point(10, 64)
        Me.dgvDownTimeLog.Name = "dgvDownTimeLog"
        Me.dgvDownTimeLog.ReadOnly = True
        Me.dgvDownTimeLog.RowHeadersVisible = False
        Me.dgvDownTimeLog.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvDownTimeLog.RowTemplate.Height = 30
        Me.dgvDownTimeLog.RowTemplate.ReadOnly = True
        Me.dgvDownTimeLog.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvDownTimeLog.Size = New System.Drawing.Size(770, 419)
        Me.dgvDownTimeLog.TabIndex = 13
        '
        'dvgTxtDTBegin
        '
        Me.dvgTxtDTBegin.DataPropertyName = "DownTimeBegin"
        DataGridViewCellStyle4.Format = "MM/dd/yy HH:mm"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.dvgTxtDTBegin.DefaultCellStyle = DataGridViewCellStyle4
        Me.dvgTxtDTBegin.HeaderText = "Begin at"
        Me.dvgTxtDTBegin.Name = "dvgTxtDTBegin"
        Me.dvgTxtDTBegin.ReadOnly = True
        Me.dvgTxtDTBegin.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dvgTxtDTBegin.Width = 130
        '
        'MachineID
        '
        Me.MachineID.DataPropertyName = "MachineID"
        Me.MachineID.HeaderText = "Line"
        Me.MachineID.Name = "MachineID"
        Me.MachineID.ReadOnly = True
        Me.MachineID.Width = 90
        '
        'ReasonCode
        '
        Me.ReasonCode.DataPropertyName = "ReasonCode"
        Me.ReasonCode.HeaderText = "Code"
        Me.ReasonCode.Name = "ReasonCode"
        Me.ReasonCode.ReadOnly = True
        Me.ReasonCode.Width = 60
        '
        'dgvTxtDuration
        '
        Me.dgvTxtDuration.DataPropertyName = "Duration"
        Me.dgvTxtDuration.HeaderText = "Mins."
        Me.dgvTxtDuration.Name = "dgvTxtDuration"
        Me.dgvTxtDuration.ReadOnly = True
        Me.dgvTxtDuration.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvTxtDuration.Width = 60
        '
        'Description
        '
        Me.Description.DataPropertyName = "Description"
        Me.Description.HeaderText = "Reason"
        Me.Description.Name = "Description"
        Me.Description.ReadOnly = True
        Me.Description.Width = 425
        '
        'Comment
        '
        Me.Comment.DataPropertyName = "Comment"
        Me.Comment.HeaderText = "Comment"
        Me.Comment.Name = "Comment"
        Me.Comment.ReadOnly = True
        Me.Comment.Width = 500
        '
        'TblDownTimeLogBindingSource
        '
        Me.TblDownTimeLogBindingSource.DataMember = "CPPsp_DownTimeLog_Sel"
        Me.TblDownTimeLogBindingSource.DataSource = Me.DsDownTimeLog
        '
        'DsDownTimeLog
        '
        Me.DsDownTimeLog.DataSetName = "dsDownTimeLog"
        Me.DsDownTimeLog.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ForeColor = System.Drawing.Color.LightSalmon
        Me.lblMessage.Location = New System.Drawing.Point(22, 565)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(108, 27)
        Me.lblMessage.TabIndex = 91
        Me.lblMessage.Text = "Message"
        '
        'TblDownTimeLogTableAdapter
        '
        Me.TblDownTimeLogTableAdapter.ClearBeforeFill = True
        '
        'btnOption
        '
        Me.btnOption.BackColor = System.Drawing.Color.Silver
        Me.btnOption.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOption.Location = New System.Drawing.Point(195, 490)
        Me.btnOption.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnOption.Name = "btnOption"
        Me.btnOption.Size = New System.Drawing.Size(150, 65)
        Me.btnOption.TabIndex = 97
        Me.btnOption.Text = "By Shift"
        Me.btnOption.UseVisualStyleBackColor = False
        '
        'lblShiftDuration
        '
        Me.lblShiftDuration.AutoSize = True
        Me.lblShiftDuration.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShiftDuration.ForeColor = System.Drawing.Color.White
        Me.lblShiftDuration.Location = New System.Drawing.Point(527, 511)
        Me.lblShiftDuration.Name = "lblShiftDuration"
        Me.lblShiftDuration.Size = New System.Drawing.Size(166, 27)
        Me.lblShiftDuration.TabIndex = 96
        Me.lblShiftDuration.Text = "Shift Duration:"
        Me.lblShiftDuration.Visible = False
        '
        'txtShiftDuration
        '
        Me.txtShiftDuration.BackColor = System.Drawing.Color.Black
        Me.txtShiftDuration.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShiftDuration.ForeColor = System.Drawing.Color.White
        Me.txtShiftDuration.Location = New System.Drawing.Point(698, 508)
        Me.txtShiftDuration.MaxLength = 4
        Me.txtShiftDuration.Name = "txtShiftDuration"
        Me.txtShiftDuration.ReadOnly = True
        Me.txtShiftDuration.Size = New System.Drawing.Size(80, 35)
        Me.txtShiftDuration.TabIndex = 95
        Me.txtShiftDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtShiftDuration.Visible = False
        '
        'UcHeading1
        '
        Me.UcHeading1.BackColor = System.Drawing.Color.Blue
        Me.UcHeading1.Location = New System.Drawing.Point(0, 0)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "-- Heading --"
        Me.UcHeading1.Size = New System.Drawing.Size(800, 58)
        Me.UcHeading1.TabIndex = 0
        '
        'frmViewDTLog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnOption)
        Me.Controls.Add(Me.lblShiftDuration)
        Me.Controls.Add(Me.txtShiftDuration)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.dgvDownTimeLog)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmViewDTLog"
        Me.Text = "frmViewDTLog"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvDownTimeLog, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TblDownTimeLogBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsDownTimeLog, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As DownTime.ucHeading
    Friend WithEvents TblDownTimeLogBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DsDownTimeLog As DownTime.dsDownTimeLog
    Friend WithEvents TblDownTimeLogTableAdapter As DownTime.dsDownTimeLogTableAdapters.CPPsp_DownTimeLog_SelTableAdapter
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents dgvDownTimeLog As System.Windows.Forms.DataGridView
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents dvgTxtDTBegin As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MachineID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ReasonCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvTxtDuration As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Description As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Comment As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnOption As System.Windows.Forms.Button
    Friend WithEvents lblShiftDuration As System.Windows.Forms.Label
    Friend WithEvents txtShiftDuration As System.Windows.Forms.TextBox
End Class
