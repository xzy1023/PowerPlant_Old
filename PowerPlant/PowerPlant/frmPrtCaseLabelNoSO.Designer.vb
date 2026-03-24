<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrtCaseLabelNoSO
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
        Me.UcHeading1 = New PowerPlant.ucHeading
        Me.lblItemDesc = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtNoOfLabels = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtItemNo = New System.Windows.Forms.TextBox
        Me.btnPrtCaseLabels = New System.Windows.Forms.Button
        Me.btnPrvScn = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtProdYear = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtProdMonth = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtProdDay = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.lblMessage = New System.Windows.Forms.Label
        Me.SuspendLayout()
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
        'lblItemDesc
        '
        Me.lblItemDesc.AutoSize = True
        Me.lblItemDesc.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemDesc.ForeColor = System.Drawing.Color.White
        Me.lblItemDesc.Location = New System.Drawing.Point(35, 114)
        Me.lblItemDesc.Name = "lblItemDesc"
        Me.lblItemDesc.Size = New System.Drawing.Size(162, 24)
        Me.lblItemDesc.TabIndex = 4
        Me.lblItemDesc.Text = "Item Description"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label14.ForeColor = System.Drawing.Color.White
        Me.Label14.Location = New System.Drawing.Point(35, 81)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(65, 27)
        Me.Label14.TabIndex = 3
        Me.Label14.Text = "Item:"
        '
        'txtNoOfLabels
        '
        Me.txtNoOfLabels.BackColor = System.Drawing.Color.Black
        Me.txtNoOfLabels.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtNoOfLabels.ForeColor = System.Drawing.Color.White
        Me.txtNoOfLabels.Location = New System.Drawing.Point(315, 151)
        Me.txtNoOfLabels.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNoOfLabels.Name = "txtNoOfLabels"
        Me.txtNoOfLabels.Size = New System.Drawing.Size(84, 35)
        Me.txtNoOfLabels.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(33, 155)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(245, 27)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "No. of Labels to Print:"
        '
        'txtItemNo
        '
        Me.txtItemNo.BackColor = System.Drawing.Color.Black
        Me.txtItemNo.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtItemNo.ForeColor = System.Drawing.Color.White
        Me.txtItemNo.Location = New System.Drawing.Point(108, 76)
        Me.txtItemNo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtItemNo.Name = "txtItemNo"
        Me.txtItemNo.Size = New System.Drawing.Size(168, 35)
        Me.txtItemNo.TabIndex = 6
        '
        'btnPrtCaseLabels
        '
        Me.btnPrtCaseLabels.BackColor = System.Drawing.Color.Silver
        Me.btnPrtCaseLabels.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnPrtCaseLabels.Location = New System.Drawing.Point(212, 490)
        Me.btnPrtCaseLabels.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrtCaseLabels.Name = "btnPrtCaseLabels"
        Me.btnPrtCaseLabels.Size = New System.Drawing.Size(150, 65)
        Me.btnPrtCaseLabels.TabIndex = 11
        Me.btnPrtCaseLabels.Text = "Print Case Labels"
        Me.btnPrtCaseLabels.UseVisualStyleBackColor = False
        '
        'btnPrvScn
        '
        Me.btnPrvScn.BackColor = System.Drawing.Color.Silver
        Me.btnPrvScn.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrvScn.Location = New System.Drawing.Point(27, 490)
        Me.btnPrvScn.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPrvScn.Name = "btnPrvScn"
        Me.btnPrvScn.Size = New System.Drawing.Size(150, 65)
        Me.btnPrvScn.TabIndex = 10
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(32, 207)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(192, 27)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Production Date:"
        '
        'txtProdYear
        '
        Me.txtProdYear.BackColor = System.Drawing.Color.Black
        Me.txtProdYear.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtProdYear.ForeColor = System.Drawing.Color.White
        Me.txtProdYear.Location = New System.Drawing.Point(315, 204)
        Me.txtProdYear.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtProdYear.MaxLength = 4
        Me.txtProdYear.Name = "txtProdYear"
        Me.txtProdYear.Size = New System.Drawing.Size(84, 35)
        Me.txtProdYear.TabIndex = 8
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(230, 207)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 27)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Year - "
        '
        'txtProdMonth
        '
        Me.txtProdMonth.BackColor = System.Drawing.Color.Black
        Me.txtProdMonth.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtProdMonth.ForeColor = System.Drawing.Color.White
        Me.txtProdMonth.Location = New System.Drawing.Point(524, 204)
        Me.txtProdMonth.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtProdMonth.MaxLength = 2
        Me.txtProdMonth.Name = "txtProdMonth"
        Me.txtProdMonth.Size = New System.Drawing.Size(51, 35)
        Me.txtProdMonth.TabIndex = 13
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(421, 207)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(101, 27)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Month - "
        '
        'txtProdDay
        '
        Me.txtProdDay.BackColor = System.Drawing.Color.Black
        Me.txtProdDay.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.txtProdDay.ForeColor = System.Drawing.Color.White
        Me.txtProdDay.Location = New System.Drawing.Point(674, 204)
        Me.txtProdDay.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtProdDay.MaxLength = 2
        Me.txtProdDay.Name = "txtProdDay"
        Me.txtProdDay.Size = New System.Drawing.Size(51, 35)
        Me.txtProdDay.TabIndex = 18
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(598, 207)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(76, 27)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Day - "
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ForeColor = System.Drawing.Color.LightSalmon
        Me.lblMessage.Location = New System.Drawing.Point(22, 565)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(108, 27)
        Me.lblMessage.TabIndex = 89
        Me.lblMessage.Text = "Message"
        '
        'frmPrtCaseLabelNoSO
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.txtProdDay)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtProdMonth)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.btnPrtCaseLabels)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.txtItemNo)
        Me.Controls.Add(Me.txtProdYear)
        Me.Controls.Add(Me.txtNoOfLabels)
        Me.Controls.Add(Me.lblItemDesc)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPrtCaseLabelNoSO"
        Me.Text = "Print Case Label By Item"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents lblItemDesc As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtNoOfLabels As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtItemNo As System.Windows.Forms.TextBox
    Friend WithEvents btnPrtCaseLabels As System.Windows.Forms.Button
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtProdYear As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtProdMonth As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtProdDay As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblMessage As System.Windows.Forms.Label
End Class
