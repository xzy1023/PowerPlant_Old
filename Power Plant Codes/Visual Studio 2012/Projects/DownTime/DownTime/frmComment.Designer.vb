<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmComment
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
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.lblComment = New System.Windows.Forms.Label()
        Me.RTBComment = New System.Windows.Forms.RichTextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblReasonCodeDesc = New System.Windows.Forms.Label()
        Me.lblShopOrderNo = New System.Windows.Forms.Label()
        Me.lblShopOrder = New System.Windows.Forms.Label()
        Me.UcHeading1 = New DownTime.ucHeading()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lblOperator = New System.Windows.Forms.Label()
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
        Me.btnPrvScn.TabIndex = 13
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'btnApply
        '
        Me.btnApply.BackColor = System.Drawing.Color.Silver
        Me.btnApply.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnApply.Location = New System.Drawing.Point(200, 490)
        Me.btnApply.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(150, 65)
        Me.btnApply.TabIndex = 14
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = False
        '
        'lblComment
        '
        Me.lblComment.AutoSize = True
        Me.lblComment.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComment.ForeColor = System.Drawing.Color.White
        Me.lblComment.Location = New System.Drawing.Point(22, 170)
        Me.lblComment.Name = "lblComment"
        Me.lblComment.Size = New System.Drawing.Size(123, 27)
        Me.lblComment.TabIndex = 39
        Me.lblComment.Text = "Comment:"
        '
        'RTBComment
        '
        Me.RTBComment.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.RTBComment.Location = New System.Drawing.Point(151, 170)
        Me.RTBComment.Name = "RTBComment"
        Me.RTBComment.Size = New System.Drawing.Size(605, 300)
        Me.RTBComment.TabIndex = 46
        Me.RTBComment.Text = ""
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(22, 112)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(101, 27)
        Me.Label8.TabIndex = 47
        Me.Label8.Text = "Reason:"
        '
        'lblReasonCodeDesc
        '
        Me.lblReasonCodeDesc.AutoSize = True
        Me.lblReasonCodeDesc.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReasonCodeDesc.ForeColor = System.Drawing.Color.White
        Me.lblReasonCodeDesc.Location = New System.Drawing.Point(119, 112)
        Me.lblReasonCodeDesc.Name = "lblReasonCodeDesc"
        Me.lblReasonCodeDesc.Size = New System.Drawing.Size(66, 27)
        Me.lblReasonCodeDesc.TabIndex = 48
        Me.lblReasonCodeDesc.Text = "Desc"
        '
        'lblShopOrderNo
        '
        Me.lblShopOrderNo.AutoSize = True
        Me.lblShopOrderNo.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShopOrderNo.ForeColor = System.Drawing.Color.White
        Me.lblShopOrderNo.Location = New System.Drawing.Point(167, 70)
        Me.lblShopOrderNo.Name = "lblShopOrderNo"
        Me.lblShopOrderNo.Size = New System.Drawing.Size(129, 27)
        Me.lblShopOrderNo.TabIndex = 50
        Me.lblShopOrderNo.Text = "000000000"
        '
        'lblShopOrder
        '
        Me.lblShopOrder.AutoSize = True
        Me.lblShopOrder.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShopOrder.ForeColor = System.Drawing.Color.White
        Me.lblShopOrder.Location = New System.Drawing.Point(22, 70)
        Me.lblShopOrder.Name = "lblShopOrder"
        Me.lblShopOrder.Size = New System.Drawing.Size(145, 27)
        Me.lblShopOrder.TabIndex = 49
        Me.lblShopOrder.Text = "Shop Order:"
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.Blue
        Me.UcHeading1.Location = New System.Drawing.Point(0, 0)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "-- Heading --"
        Me.UcHeading1.Size = New System.Drawing.Size(800, 55)
        Me.UcHeading1.TabIndex = 1
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(306, 70)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(114, 27)
        Me.Label9.TabIndex = 51
        Me.Label9.Text = "Operator:"
        '
        'lblOperator
        '
        Me.lblOperator.AutoSize = True
        Me.lblOperator.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOperator.ForeColor = System.Drawing.Color.White
        Me.lblOperator.Location = New System.Drawing.Point(413, 70)
        Me.lblOperator.MaximumSize = New System.Drawing.Size(300, 27)
        Me.lblOperator.Name = "lblOperator"
        Me.lblOperator.Size = New System.Drawing.Size(75, 27)
        Me.lblOperator.TabIndex = 52
        Me.lblOperator.Text = "Name"
        '
        'frmComment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.Controls.Add(Me.lblOperator)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.lblShopOrderNo)
        Me.Controls.Add(Me.lblShopOrder)
        Me.Controls.Add(Me.lblReasonCodeDesc)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.RTBComment)
        Me.Controls.Add(Me.lblComment)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmComment"
        Me.Text = "frmComment"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As DownTime.ucHeading
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents lblComment As System.Windows.Forms.Label
    Friend WithEvents RTBComment As System.Windows.Forms.RichTextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblReasonCodeDesc As System.Windows.Forms.Label
    Friend WithEvents lblShopOrderNo As System.Windows.Forms.Label
    Friend WithEvents lblShopOrder As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lblOperator As System.Windows.Forms.Label
End Class
