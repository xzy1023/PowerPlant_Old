<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInvokePPForm
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnAccept = New System.Windows.Forms.Button()
        Me.txtInterface = New System.Windows.Forms.TextBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cboForm = New System.Windows.Forms.ComboBox()
        Me.TblQATFormBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsQATForm = New InvokePPForm.dsQATForm()
        Me.cboWFType = New System.Windows.Forms.ComboBox()
        Me.TblQATFormTableAdapter = New InvokePPForm.dsQATFormTableAdapters.tblQATFormTableAdapter()
        Me.DsQATDef1 = New InvokePPForm.dsQATDef()
        Me.DsQATDef1BindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DtQATDefTableAdapter = New InvokePPForm.dsQATDefTableAdapters.dtQATDefTableAdapter()
        Me.DtQATDefBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        CType(Me.TblQATFormBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsQATForm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsQATDef1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsQATDef1BindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DtQATDefBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(54, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(153, 25)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Form Request:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(54, 93)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(127, 25)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Interface ID:"
        '
        'btnAccept
        '
        Me.btnAccept.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAccept.Location = New System.Drawing.Point(303, 260)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(89, 39)
        Me.btnAccept.TabIndex = 10
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'txtInterface
        '
        Me.txtInterface.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInterface.Location = New System.Drawing.Point(233, 90)
        Me.txtInterface.Name = "txtInterface"
        Me.txtInterface.Size = New System.Drawing.Size(204, 31)
        Me.txtInterface.TabIndex = 5
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(59, 260)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(100, 39)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(54, 153)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(173, 25)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Work Flow Type:"
        '
        'cboForm
        '
        Me.cboForm.DataSource = Me.TblQATFormBindingSource
        Me.cboForm.DisplayMember = "TestCategory"
        Me.cboForm.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboForm.FormattingEnabled = True
        Me.cboForm.Location = New System.Drawing.Point(233, 25)
        Me.cboForm.Name = "cboForm"
        Me.cboForm.Size = New System.Drawing.Size(389, 33)
        Me.cboForm.TabIndex = 13
        Me.cboForm.ValueMember = "InterfaceFormID"
        '
        'TblQATFormBindingSource
        '
        Me.TblQATFormBindingSource.DataMember = "tblQATForm"
        Me.TblQATFormBindingSource.DataSource = Me.DsQATForm
        '
        'DsQATForm
        '
        Me.DsQATForm.DataSetName = "dsQATForm"
        Me.DsQATForm.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'cboWFType
        '
        Me.cboWFType.DataSource = Me.DtQATDefBindingSource
        Me.cboWFType.DisplayMember = "WorkFlowType"
        Me.cboWFType.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboWFType.FormattingEnabled = True
        Me.cboWFType.Location = New System.Drawing.Point(233, 150)
        Me.cboWFType.Name = "cboWFType"
        Me.cboWFType.Size = New System.Drawing.Size(75, 33)
        Me.cboWFType.TabIndex = 14
        Me.cboWFType.ValueMember = "WorkFlowType"
        '
        'TblQATFormTableAdapter
        '
        Me.TblQATFormTableAdapter.ClearBeforeFill = True
        '
        'DsQATDef1
        '
        Me.DsQATDef1.DataSetName = "dsQATDef"
        Me.DsQATDef1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'DsQATDef1BindingSource
        '
        Me.DsQATDef1BindingSource.DataSource = Me.DsQATDef1
        Me.DsQATDef1BindingSource.Position = 0
        '
        'DtQATDefTableAdapter
        '
        Me.DtQATDefTableAdapter.ClearBeforeFill = True
        '
        'DtQATDefBindingSource
        '
        Me.DtQATDefBindingSource.DataMember = "dtQATDef"
        Me.DtQATDefBindingSource.DataSource = Me.DsQATDef1BindingSource
        '
        'frmInvokePPForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(680, 345)
        Me.Controls.Add(Me.cboWFType)
        Me.Controls.Add(Me.cboForm)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtInterface)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmInvokePPForm"
        Me.Text = "Invoke PP Form"
        CType(Me.TblQATFormBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsQATForm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsQATDef1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsQATDef1BindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DtQATDefBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout

End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents txtInterface As System.Windows.Forms.TextBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cboForm As System.Windows.Forms.ComboBox
    Friend WithEvents cboWFType As System.Windows.Forms.ComboBox
    Friend WithEvents DsQATForm As InvokePPForm.dsQATForm
    Friend WithEvents TblQATFormBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents TblQATFormTableAdapter As InvokePPForm.dsQATFormTableAdapters.tblQATFormTableAdapter
    Friend WithEvents DsQATDef1BindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DsQATDef1 As InvokePPForm.dsQATDef
    Friend WithEvents DtQATDefTableAdapter As InvokePPForm.dsQATDefTableAdapters.dtQATDefTableAdapter
    Friend WithEvents DtQATDefBindingSource As System.Windows.Forms.BindingSource

End Class
