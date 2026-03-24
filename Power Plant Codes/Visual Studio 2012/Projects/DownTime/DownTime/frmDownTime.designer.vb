<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDownTime
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
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.btnAccept = New System.Windows.Forms.Button()
        Me.lblShopOrderNo = New System.Windows.Forms.Label()
        Me.lblShopOrder = New System.Windows.Forms.Label()
        Me.txtOperator = New System.Windows.Forms.TextBox()
        Me.lblOperator = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtTechnician = New System.Windows.Forms.TextBox()
        Me.lblTechnician = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblDuration = New System.Windows.Forms.Label()
        Me.lblOR = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtBeginDate = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtBeginTime = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblComment = New System.Windows.Forms.Label()
        Me.txtDuration = New System.Windows.Forms.TextBox()
        Me.btnEndNow = New System.Windows.Forms.Button()
        Me.RTBComment = New System.Windows.Forms.RichTextBox()
        Me.btnDTInquiry = New System.Windows.Forms.Button()
        Me.cboReasonType = New System.Windows.Forms.ComboBox()
        Me.cboReasonCode = New System.Windows.Forms.ComboBox()
        Me.lblReasonCodeDesc = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtShift = New System.Windows.Forms.TextBox()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.dgvDTLog = New System.Windows.Forms.DataGridView()
        Me.DurationDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MachineID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ReasonCodeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Description = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Comment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TblDownTimeLogBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsDownTimeLog = New DownTime.dsDownTimeLog()
        Me.TblDownTimeLogTableAdapter = New DownTime.dsDownTimeLogTableAdapters.CPPsp_DownTimeLog_SelTableAdapter()
        Me.cboMachine = New System.Windows.Forms.ComboBox()
        Me.btnHybernate = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblBeginTime = New System.Windows.Forms.Label()
        Me.lblBeginDate = New System.Windows.Forms.Label()
        Me.lblDurationInMin = New System.Windows.Forms.Label()
        Me.UcHeading1 = New DownTime.ucHeading()
        CType(Me.dgvDTLog, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.btnPrvScn.TabIndex = 11
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'btnAccept
        '
        Me.btnAccept.BackColor = System.Drawing.Color.Silver
        Me.btnAccept.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnAccept.Location = New System.Drawing.Point(200, 490)
        Me.btnAccept.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(150, 65)
        Me.btnAccept.TabIndex = 12
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = False
        '
        'lblShopOrderNo
        '
        Me.lblShopOrderNo.AutoSize = True
        Me.lblShopOrderNo.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShopOrderNo.ForeColor = System.Drawing.Color.White
        Me.lblShopOrderNo.Location = New System.Drawing.Point(673, 67)
        Me.lblShopOrderNo.Name = "lblShopOrderNo"
        Me.lblShopOrderNo.Size = New System.Drawing.Size(129, 27)
        Me.lblShopOrderNo.TabIndex = 37
        Me.lblShopOrderNo.Text = "000000000"
        '
        'lblShopOrder
        '
        Me.lblShopOrder.AutoSize = True
        Me.lblShopOrder.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShopOrder.ForeColor = System.Drawing.Color.White
        Me.lblShopOrder.Location = New System.Drawing.Point(528, 67)
        Me.lblShopOrder.Name = "lblShopOrder"
        Me.lblShopOrder.Size = New System.Drawing.Size(145, 27)
        Me.lblShopOrder.TabIndex = 36
        Me.lblShopOrder.Text = "Shop Order:"
        '
        'txtOperator
        '
        Me.txtOperator.BackColor = System.Drawing.Color.Black
        Me.txtOperator.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOperator.ForeColor = System.Drawing.Color.White
        Me.txtOperator.Location = New System.Drawing.Point(135, 115)
        Me.txtOperator.Margin = New System.Windows.Forms.Padding(2)
        Me.txtOperator.MaxLength = 4
        Me.txtOperator.Name = "txtOperator"
        Me.txtOperator.Size = New System.Drawing.Size(61, 35)
        Me.txtOperator.TabIndex = 1
        '
        'lblOperator
        '
        Me.lblOperator.AutoSize = True
        Me.lblOperator.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOperator.ForeColor = System.Drawing.Color.White
        Me.lblOperator.Location = New System.Drawing.Point(199, 118)
        Me.lblOperator.MaximumSize = New System.Drawing.Size(250, 27)
        Me.lblOperator.Name = "lblOperator"
        Me.lblOperator.Size = New System.Drawing.Size(75, 27)
        Me.lblOperator.TabIndex = 39
        Me.lblOperator.Text = "Name"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(22, 118)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(114, 27)
        Me.Label9.TabIndex = 38
        Me.Label9.Text = "Operator:"
        '
        'txtTechnician
        '
        Me.txtTechnician.BackColor = System.Drawing.Color.Black
        Me.txtTechnician.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTechnician.ForeColor = System.Drawing.Color.White
        Me.txtTechnician.Location = New System.Drawing.Point(542, 110)
        Me.txtTechnician.MaxLength = 4
        Me.txtTechnician.Name = "txtTechnician"
        Me.txtTechnician.Size = New System.Drawing.Size(61, 35)
        Me.txtTechnician.TabIndex = 2
        '
        'lblTechnician
        '
        Me.lblTechnician.AutoSize = True
        Me.lblTechnician.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTechnician.ForeColor = System.Drawing.Color.White
        Me.lblTechnician.Location = New System.Drawing.Point(609, 116)
        Me.lblTechnician.MaximumSize = New System.Drawing.Size(250, 27)
        Me.lblTechnician.Name = "lblTechnician"
        Me.lblTechnician.Size = New System.Drawing.Size(75, 27)
        Me.lblTechnician.TabIndex = 42
        Me.lblTechnician.Text = "Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(205, 67)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(109, 27)
        Me.Label2.TabIndex = 41
        Me.Label2.Text = "Machine:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(403, 164)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(161, 27)
        Me.Label3.TabIndex = 38
        Me.Label3.Text = "Time (hhmm):"
        '
        'lblDuration
        '
        Me.lblDuration.AutoSize = True
        Me.lblDuration.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDuration.ForeColor = System.Drawing.Color.White
        Me.lblDuration.Location = New System.Drawing.Point(22, 215)
        Me.lblDuration.Name = "lblDuration"
        Me.lblDuration.Size = New System.Drawing.Size(171, 27)
        Me.lblDuration.TabIndex = 38
        Me.lblDuration.Text = "Duration (Min):"
        '
        'lblOR
        '
        Me.lblOR.AutoSize = True
        Me.lblOR.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOR.ForeColor = System.Drawing.Color.White
        Me.lblOR.Location = New System.Drawing.Point(370, 220)
        Me.lblOR.Name = "lblOR"
        Me.lblOR.Size = New System.Drawing.Size(78, 27)
        Me.lblOR.TabIndex = 38
        Me.lblOR.Text = "- OR -"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(22, 267)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(160, 27)
        Me.Label6.TabIndex = 38
        Me.Label6.Text = "Reason Type:"
        '
        'txtBeginDate
        '
        Me.txtBeginDate.BackColor = System.Drawing.Color.Black
        Me.txtBeginDate.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBeginDate.ForeColor = System.Drawing.Color.White
        Me.txtBeginDate.Location = New System.Drawing.Point(241, 164)
        Me.txtBeginDate.MaxLength = 8
        Me.txtBeginDate.Name = "txtBeginDate"
        Me.txtBeginDate.Size = New System.Drawing.Size(117, 35)
        Me.txtBeginDate.TabIndex = 3
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(22, 167)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(213, 27)
        Me.Label7.TabIndex = 38
        Me.Label7.Text = "Begin Date (YMD):"
        '
        'txtBeginTime
        '
        Me.txtBeginTime.BackColor = System.Drawing.Color.Black
        Me.txtBeginTime.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBeginTime.ForeColor = System.Drawing.Color.White
        Me.txtBeginTime.Location = New System.Drawing.Point(570, 159)
        Me.txtBeginTime.MaxLength = 4
        Me.txtBeginTime.Name = "txtBeginTime"
        Me.txtBeginTime.Size = New System.Drawing.Size(79, 35)
        Me.txtBeginTime.TabIndex = 40
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(22, 313)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(101, 27)
        Me.Label8.TabIndex = 38
        Me.Label8.Text = "Reason:"
        '
        'lblComment
        '
        Me.lblComment.AutoSize = True
        Me.lblComment.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComment.ForeColor = System.Drawing.Color.White
        Me.lblComment.Location = New System.Drawing.Point(16, 498)
        Me.lblComment.Name = "lblComment"
        Me.lblComment.Size = New System.Drawing.Size(123, 27)
        Me.lblComment.TabIndex = 38
        Me.lblComment.Text = "Comment:"
        '
        'txtDuration
        '
        Me.txtDuration.BackColor = System.Drawing.Color.Black
        Me.txtDuration.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDuration.ForeColor = System.Drawing.Color.White
        Me.txtDuration.Location = New System.Drawing.Point(241, 212)
        Me.txtDuration.MaxLength = 4
        Me.txtDuration.Name = "txtDuration"
        Me.txtDuration.Size = New System.Drawing.Size(80, 35)
        Me.txtDuration.TabIndex = 40
        '
        'btnEndNow
        '
        Me.btnEndNow.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnEndNow.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEndNow.Location = New System.Drawing.Point(490, 201)
        Me.btnEndNow.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEndNow.Name = "btnEndNow"
        Me.btnEndNow.Size = New System.Drawing.Size(159, 49)
        Me.btnEndNow.TabIndex = 44
        Me.btnEndNow.Text = "End Now"
        Me.btnEndNow.UseVisualStyleBackColor = False
        '
        'RTBComment
        '
        Me.RTBComment.Location = New System.Drawing.Point(235, 498)
        Me.RTBComment.Name = "RTBComment"
        Me.RTBComment.Size = New System.Drawing.Size(539, 21)
        Me.RTBComment.TabIndex = 45
        Me.RTBComment.Text = ""
        '
        'btnDTInquiry
        '
        Me.btnDTInquiry.BackColor = System.Drawing.Color.Silver
        Me.btnDTInquiry.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnDTInquiry.Location = New System.Drawing.Point(372, 490)
        Me.btnDTInquiry.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnDTInquiry.Name = "btnDTInquiry"
        Me.btnDTInquiry.Size = New System.Drawing.Size(150, 65)
        Me.btnDTInquiry.TabIndex = 12
        Me.btnDTInquiry.Text = "Down Time Inquiry"
        Me.btnDTInquiry.UseVisualStyleBackColor = False
        '
        'cboReasonType
        '
        Me.cboReasonType.BackColor = System.Drawing.Color.Black
        Me.cboReasonType.DropDownHeight = 300
        Me.cboReasonType.DropDownWidth = 400
        Me.cboReasonType.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReasonType.ForeColor = System.Drawing.Color.White
        Me.cboReasonType.FormattingEnabled = True
        Me.cboReasonType.IntegralHeight = False
        Me.cboReasonType.ItemHeight = 27
        Me.cboReasonType.Location = New System.Drawing.Point(241, 263)
        Me.cboReasonType.MaxDropDownItems = 4
        Me.cboReasonType.MaxLength = 4
        Me.cboReasonType.Name = "cboReasonType"
        Me.cboReasonType.Size = New System.Drawing.Size(418, 35)
        Me.cboReasonType.TabIndex = 47
        '
        'cboReasonCode
        '
        Me.cboReasonCode.BackColor = System.Drawing.Color.Black
        Me.cboReasonCode.DropDownHeight = 300
        Me.cboReasonCode.DropDownWidth = 400
        Me.cboReasonCode.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReasonCode.ForeColor = System.Drawing.Color.White
        Me.cboReasonCode.FormattingEnabled = True
        Me.cboReasonCode.IntegralHeight = False
        Me.cboReasonCode.ItemHeight = 27
        Me.cboReasonCode.Location = New System.Drawing.Point(241, 310)
        Me.cboReasonCode.MaxDropDownItems = 4
        Me.cboReasonCode.MaxLength = 4
        Me.cboReasonCode.Name = "cboReasonCode"
        Me.cboReasonCode.Size = New System.Drawing.Size(105, 35)
        Me.cboReasonCode.TabIndex = 47
        '
        'lblReasonCodeDesc
        '
        Me.lblReasonCodeDesc.AutoSize = True
        Me.lblReasonCodeDesc.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReasonCodeDesc.ForeColor = System.Drawing.Color.White
        Me.lblReasonCodeDesc.Location = New System.Drawing.Point(354, 313)
        Me.lblReasonCodeDesc.Name = "lblReasonCodeDesc"
        Me.lblReasonCodeDesc.Size = New System.Drawing.Size(66, 27)
        Me.lblReasonCodeDesc.TabIndex = 38
        Me.lblReasonCodeDesc.Text = "Desc"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(25, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 27)
        Me.Label1.TabIndex = 41
        Me.Label1.Text = "Shift:"
        '
        'txtShift
        '
        Me.txtShift.BackColor = System.Drawing.Color.Black
        Me.txtShift.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtShift.ForeColor = System.Drawing.Color.White
        Me.txtShift.Location = New System.Drawing.Point(99, 64)
        Me.txtShift.MaxLength = 1
        Me.txtShift.Name = "txtShift"
        Me.txtShift.Size = New System.Drawing.Size(40, 35)
        Me.txtShift.TabIndex = 48
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ForeColor = System.Drawing.Color.LightSalmon
        Me.lblMessage.Location = New System.Drawing.Point(22, 565)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(108, 27)
        Me.lblMessage.TabIndex = 90
        Me.lblMessage.Text = "Message"
        '
        'dgvDTLog
        '
        Me.dgvDTLog.AllowUserToAddRows = False
        Me.dgvDTLog.AllowUserToDeleteRows = False
        Me.dgvDTLog.AutoGenerateColumns = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvDTLog.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvDTLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDTLog.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DurationDataGridViewTextBoxColumn, Me.MachineID, Me.ReasonCodeDataGridViewTextBoxColumn, Me.Description, Me.Comment})
        Me.dgvDTLog.DataSource = Me.TblDownTimeLogBindingSource
        Me.dgvDTLog.Location = New System.Drawing.Point(27, 353)
        Me.dgvDTLog.Name = "dgvDTLog"
        Me.dgvDTLog.ReadOnly = True
        Me.dgvDTLog.RowHeadersVisible = False
        Me.dgvDTLog.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.dgvDTLog.Size = New System.Drawing.Size(745, 130)
        Me.dgvDTLog.TabIndex = 91
        '
        'DurationDataGridViewTextBoxColumn
        '
        Me.DurationDataGridViewTextBoxColumn.DataPropertyName = "Duration"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DurationDataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle2
        Me.DurationDataGridViewTextBoxColumn.HeaderText = "Duration"
        Me.DurationDataGridViewTextBoxColumn.MaxInputLength = 6
        Me.DurationDataGridViewTextBoxColumn.Name = "DurationDataGridViewTextBoxColumn"
        Me.DurationDataGridViewTextBoxColumn.ReadOnly = True
        Me.DurationDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DurationDataGridViewTextBoxColumn.Width = 80
        '
        'MachineID
        '
        Me.MachineID.DataPropertyName = "MachineID"
        Me.MachineID.HeaderText = "Line"
        Me.MachineID.Name = "MachineID"
        Me.MachineID.ReadOnly = True
        '
        'ReasonCodeDataGridViewTextBoxColumn
        '
        Me.ReasonCodeDataGridViewTextBoxColumn.DataPropertyName = "ReasonCode"
        Me.ReasonCodeDataGridViewTextBoxColumn.HeaderText = "Code"
        Me.ReasonCodeDataGridViewTextBoxColumn.MaxInputLength = 4
        Me.ReasonCodeDataGridViewTextBoxColumn.Name = "ReasonCodeDataGridViewTextBoxColumn"
        Me.ReasonCodeDataGridViewTextBoxColumn.ReadOnly = True
        Me.ReasonCodeDataGridViewTextBoxColumn.Width = 70
        '
        'Description
        '
        Me.Description.DataPropertyName = "Description"
        Me.Description.HeaderText = "Reason"
        Me.Description.Name = "Description"
        Me.Description.ReadOnly = True
        Me.Description.Width = 480
        '
        'Comment
        '
        Me.Comment.DataPropertyName = "Comment"
        Me.Comment.HeaderText = "Comment"
        Me.Comment.Name = "Comment"
        Me.Comment.ReadOnly = True
        Me.Comment.Visible = False
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
        'TblDownTimeLogTableAdapter
        '
        Me.TblDownTimeLogTableAdapter.ClearBeforeFill = True
        '
        'cboMachine
        '
        Me.cboMachine.BackColor = System.Drawing.Color.Black
        Me.cboMachine.DropDownHeight = 300
        Me.cboMachine.DropDownWidth = 170
        Me.cboMachine.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMachine.ForeColor = System.Drawing.Color.White
        Me.cboMachine.FormattingEnabled = True
        Me.cboMachine.IntegralHeight = False
        Me.cboMachine.ItemHeight = 27
        Me.cboMachine.Location = New System.Drawing.Point(320, 64)
        Me.cboMachine.MaxDropDownItems = 4
        Me.cboMachine.MaxLength = 4
        Me.cboMachine.Name = "cboMachine"
        Me.cboMachine.Size = New System.Drawing.Size(166, 35)
        Me.cboMachine.TabIndex = 47
        '
        'btnHybernate
        '
        Me.btnHybernate.BackColor = System.Drawing.Color.Silver
        Me.btnHybernate.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnHybernate.Location = New System.Drawing.Point(543, 490)
        Me.btnHybernate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnHybernate.Name = "btnHybernate"
        Me.btnHybernate.Size = New System.Drawing.Size(150, 65)
        Me.btnHybernate.TabIndex = 12
        Me.btnHybernate.Text = "Hibernate"
        Me.btnHybernate.UseVisualStyleBackColor = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(403, 118)
        Me.Label10.MaximumSize = New System.Drawing.Size(250, 27)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(132, 27)
        Me.Label10.TabIndex = 42
        Me.Label10.Text = "Technician:"
        '
        'lblBeginTime
        '
        Me.lblBeginTime.AutoSize = True
        Me.lblBeginTime.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBeginTime.ForeColor = System.Drawing.Color.White
        Me.lblBeginTime.Location = New System.Drawing.Point(570, 164)
        Me.lblBeginTime.MaximumSize = New System.Drawing.Size(250, 27)
        Me.lblBeginTime.Name = "lblBeginTime"
        Me.lblBeginTime.Size = New System.Drawing.Size(71, 27)
        Me.lblBeginTime.TabIndex = 92
        Me.lblBeginTime.Text = "00:00"
        '
        'lblBeginDate
        '
        Me.lblBeginDate.AutoSize = True
        Me.lblBeginDate.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBeginDate.ForeColor = System.Drawing.Color.White
        Me.lblBeginDate.Location = New System.Drawing.Point(242, 167)
        Me.lblBeginDate.MaximumSize = New System.Drawing.Size(250, 27)
        Me.lblBeginDate.Name = "lblBeginDate"
        Me.lblBeginDate.Size = New System.Drawing.Size(116, 27)
        Me.lblBeginDate.TabIndex = 93
        Me.lblBeginDate.Text = "00000000"
        '
        'lblDurationInMin
        '
        Me.lblDurationInMin.AutoSize = True
        Me.lblDurationInMin.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDurationInMin.ForeColor = System.Drawing.Color.White
        Me.lblDurationInMin.Location = New System.Drawing.Point(242, 215)
        Me.lblDurationInMin.Name = "lblDurationInMin"
        Me.lblDurationInMin.Size = New System.Drawing.Size(25, 27)
        Me.lblDurationInMin.TabIndex = 94
        Me.lblDurationInMin.Text = "0"
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
        'frmDownTime
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblDurationInMin)
        Me.Controls.Add(Me.lblBeginDate)
        Me.Controls.Add(Me.lblBeginTime)
        Me.Controls.Add(Me.dgvDTLog)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.txtShift)
        Me.Controls.Add(Me.cboReasonCode)
        Me.Controls.Add(Me.cboMachine)
        Me.Controls.Add(Me.cboReasonType)
        Me.Controls.Add(Me.btnEndNow)
        Me.Controls.Add(Me.txtTechnician)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.lblTechnician)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtBeginTime)
        Me.Controls.Add(Me.txtDuration)
        Me.Controls.Add(Me.txtBeginDate)
        Me.Controls.Add(Me.txtOperator)
        Me.Controls.Add(Me.lblOperator)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lblOR)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.lblDuration)
        Me.Controls.Add(Me.lblReasonCodeDesc)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.lblShopOrderNo)
        Me.Controls.Add(Me.lblShopOrder)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.btnHybernate)
        Me.Controls.Add(Me.btnDTInquiry)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.UcHeading1)
        Me.Controls.Add(Me.RTBComment)
        Me.Controls.Add(Me.lblComment)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDownTime"
        Me.Text = "frmDownTime"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvDTLog, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TblDownTimeLogBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsDownTimeLog, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As ucHeading
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents lblShopOrderNo As System.Windows.Forms.Label
    Friend WithEvents lblShopOrder As System.Windows.Forms.Label
    Friend WithEvents txtOperator As System.Windows.Forms.TextBox
    Friend WithEvents lblOperator As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtTechnician As System.Windows.Forms.TextBox
    Friend WithEvents lblTechnician As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblDuration As System.Windows.Forms.Label
    Friend WithEvents lblOR As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtBeginDate As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtBeginTime As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblComment As System.Windows.Forms.Label
    Friend WithEvents txtDuration As System.Windows.Forms.TextBox
    Friend WithEvents btnEndNow As System.Windows.Forms.Button
    Friend WithEvents RTBComment As System.Windows.Forms.RichTextBox
    Friend WithEvents btnDTInquiry As System.Windows.Forms.Button
    Friend WithEvents cboReasonType As System.Windows.Forms.ComboBox
    Friend WithEvents cboReasonCode As System.Windows.Forms.ComboBox
    Friend WithEvents lblReasonCodeDesc As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtShift As System.Windows.Forms.TextBox
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents dgvDTLog As System.Windows.Forms.DataGridView
    Friend WithEvents TblDownTimeLogBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DsDownTimeLog As DownTime.dsDownTimeLog
    Friend WithEvents TblDownTimeLogTableAdapter As DownTime.dsDownTimeLogTableAdapters.CPPsp_DownTimeLog_SelTableAdapter
    Friend WithEvents cboMachine As System.Windows.Forms.ComboBox
    Friend WithEvents btnHybernate As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents DurationDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MachineID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ReasonCodeDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Description As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Comment As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblBeginTime As System.Windows.Forms.Label
    Friend WithEvents lblBeginDate As System.Windows.Forms.Label
    Friend WithEvents lblDurationInMin As System.Windows.Forms.Label
End Class
