<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class _frmQATSwitchComponent
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
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.btnAccept = New System.Windows.Forms.Button()
        Me.lblFromComponent = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.lblToComponent = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnStartWithNoLabel = New System.Windows.Forms.Button()
        Me.txtPkgLine = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(0, -1)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "QAT Switching Component"
        Me.UcHeading1.Size = New System.Drawing.Size(800, 55)
        Me.UcHeading1.TabIndex = 15
        '
        'btnAccept
        '
        Me.btnAccept.BackColor = System.Drawing.Color.Silver
        Me.btnAccept.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAccept.Location = New System.Drawing.Point(27, 490)
        Me.btnAccept.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(150, 65)
        Me.btnAccept.TabIndex = 79
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = False
        '
        'lblFromComponent
        '
        Me.lblFromComponent.AutoSize = True
        Me.lblFromComponent.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFromComponent.ForeColor = System.Drawing.Color.White
        Me.lblFromComponent.Location = New System.Drawing.Point(608, 71)
        Me.lblFromComponent.Name = "lblFromComponent"
        Me.lblFromComponent.Size = New System.Drawing.Size(129, 27)
        Me.lblFromComponent.TabIndex = 83
        Me.lblFromComponent.Text = "000000000"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.White
        Me.Label17.Location = New System.Drawing.Point(502, 71)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(75, 27)
        Me.Label17.TabIndex = 82
        Me.Label17.Text = "From:"
        '
        'lblToComponent
        '
        Me.lblToComponent.AutoSize = True
        Me.lblToComponent.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblToComponent.ForeColor = System.Drawing.Color.White
        Me.lblToComponent.Location = New System.Drawing.Point(231, 71)
        Me.lblToComponent.Name = "lblToComponent"
        Me.lblToComponent.Size = New System.Drawing.Size(129, 27)
        Me.lblToComponent.TabIndex = 80
        Me.lblToComponent.Text = "000000000"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(32, 71)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(175, 27)
        Me.Label16.TabIndex = 81
        Me.Label16.Text = "Component To:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(32, 109)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(176, 27)
        Me.Label1.TabIndex = 84
        Me.Label1.Text = "Shop Order To:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(231, 109)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(129, 27)
        Me.Label2.TabIndex = 85
        Me.Label2.Text = "000000000"
        '
        'txtNotes
        '
        Me.txtNotes.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtNotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNotes.Location = New System.Drawing.Point(37, 151)
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.ReadOnly = True
        Me.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtNotes.Size = New System.Drawing.Size(726, 332)
        Me.txtNotes.TabIndex = 86
        Me.txtNotes.WordWrap = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(502, 109)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(75, 27)
        Me.Label4.TabIndex = 88
        Me.Label4.Text = "From:"
        '
        'btnStartWithNoLabel
        '
        Me.btnStartWithNoLabel.BackColor = System.Drawing.Color.Gold
        Me.btnStartWithNoLabel.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnStartWithNoLabel.ForeColor = System.Drawing.Color.Crimson
        Me.btnStartWithNoLabel.Location = New System.Drawing.Point(574, 490)
        Me.btnStartWithNoLabel.Name = "btnStartWithNoLabel"
        Me.btnStartWithNoLabel.Size = New System.Drawing.Size(150, 65)
        Me.btnStartWithNoLabel.TabIndex = 90
        Me.btnStartWithNoLabel.Text = "Override"
        Me.btnStartWithNoLabel.UseVisualStyleBackColor = False
        '
        'txtPkgLine
        '
        Me.txtPkgLine.BackColor = System.Drawing.Color.Black
        Me.txtPkgLine.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPkgLine.ForeColor = System.Drawing.Color.White
        Me.txtPkgLine.Location = New System.Drawing.Point(613, 106)
        Me.txtPkgLine.MaxLength = 10
        Me.txtPkgLine.Name = "txtPkgLine"
        Me.txtPkgLine.Size = New System.Drawing.Size(129, 35)
        Me.txtPkgLine.TabIndex = 91
        Me.txtPkgLine.Text = "000000000"
        '
        'frmQATSwitchComponent
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtPkgLine)
        Me.Controls.Add(Me.btnStartWithNoLabel)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtNotes)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblFromComponent)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.lblToComponent)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.UcHeading1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmQATSwitchComponent"
        Me.Text = "Get Data From Serial Port"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents lblFromComponent As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents lblToComponent As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtNotes As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnStartWithNoLabel As System.Windows.Forms.Button
    Friend WithEvents txtPkgLine As System.Windows.Forms.TextBox
End Class
