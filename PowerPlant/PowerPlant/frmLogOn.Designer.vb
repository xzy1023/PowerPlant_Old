<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLogOn
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
        Me.txtPkgLine = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtOperator = New System.Windows.Forms.TextBox()
        Me.txtShift = New System.Windows.Forms.TextBox()
        Me.lblOperator = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.btnPrintControl = New System.Windows.Forms.Button()
        Me.lblShift = New System.Windows.Forms.Label()
        Me.lblPkgLine = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnTSCtlPnl = New System.Windows.Forms.Button()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtPkgLine
        '
        Me.txtPkgLine.BackColor = System.Drawing.Color.Black
        Me.txtPkgLine.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPkgLine.ForeColor = System.Drawing.Color.White
        Me.txtPkgLine.Location = New System.Drawing.Point(346, 284)
        Me.txtPkgLine.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.txtPkgLine.MaxLength = 10
        Me.txtPkgLine.Name = "txtPkgLine"
        Me.txtPkgLine.Size = New System.Drawing.Size(129, 35)
        Me.txtPkgLine.TabIndex = 6
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(158, 287)
        Me.Label7.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(184, 27)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Packaging Line:"
        '
        'txtOperator
        '
        Me.txtOperator.BackColor = System.Drawing.Color.Black
        Me.txtOperator.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOperator.ForeColor = System.Drawing.Color.White
        Me.txtOperator.Location = New System.Drawing.Point(346, 341)
        Me.txtOperator.MaxLength = 4
        Me.txtOperator.Name = "txtOperator"
        Me.txtOperator.Size = New System.Drawing.Size(70, 35)
        Me.txtOperator.TabIndex = 12
        Me.txtOperator.UseSystemPasswordChar = True
        '
        'txtShift
        '
        Me.txtShift.BackColor = System.Drawing.Color.Black
        Me.txtShift.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShift.ForeColor = System.Drawing.Color.White
        Me.txtShift.Location = New System.Drawing.Point(346, 393)
        Me.txtShift.MaxLength = 1
        Me.txtShift.Name = "txtShift"
        Me.txtShift.Size = New System.Drawing.Size(31, 35)
        Me.txtShift.TabIndex = 11
        '
        'lblOperator
        '
        Me.lblOperator.AutoSize = True
        Me.lblOperator.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOperator.ForeColor = System.Drawing.Color.White
        Me.lblOperator.Location = New System.Drawing.Point(421, 344)
        Me.lblOperator.Name = "lblOperator"
        Me.lblOperator.Size = New System.Drawing.Size(75, 27)
        Me.lblOperator.TabIndex = 10
        Me.lblOperator.Text = "Name"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(158, 344)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(114, 27)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Operator:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(159, 396)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 27)
        Me.Label8.TabIndex = 9
        Me.Label8.Text = "Shift:"
        '
        'btnPrintControl
        '
        Me.btnPrintControl.BackColor = System.Drawing.Color.Silver
        Me.btnPrintControl.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrintControl.Location = New System.Drawing.Point(616, 490)
        Me.btnPrintControl.Name = "btnPrintControl"
        Me.btnPrintControl.Size = New System.Drawing.Size(150, 65)
        Me.btnPrintControl.TabIndex = 13
        Me.btnPrintControl.Text = "Print Control"
        Me.btnPrintControl.UseVisualStyleBackColor = False
        '
        'lblShift
        '
        Me.lblShift.AutoSize = True
        Me.lblShift.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShift.ForeColor = System.Drawing.Color.White
        Me.lblShift.Location = New System.Drawing.Point(394, 396)
        Me.lblShift.MaximumSize = New System.Drawing.Size(250, 27)
        Me.lblShift.Name = "lblShift"
        Me.lblShift.Size = New System.Drawing.Size(75, 27)
        Me.lblShift.TabIndex = 10
        Me.lblShift.Text = "Name"
        '
        'lblPkgLine
        '
        Me.lblPkgLine.AutoSize = True
        Me.lblPkgLine.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPkgLine.ForeColor = System.Drawing.Color.White
        Me.lblPkgLine.Location = New System.Drawing.Point(481, 287)
        Me.lblPkgLine.Name = "lblPkgLine"
        Me.lblPkgLine.Size = New System.Drawing.Size(75, 27)
        Me.lblPkgLine.TabIndex = 10
        Me.lblPkgLine.Text = "Name"
        '
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.Color.Firebrick
        Me.PictureBox2.Image = Global.PowerPlant.My.Resources.Resources.mp_logo_l
        Me.PictureBox2.Location = New System.Drawing.Point(0, 56)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(257, 174)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 15
        Me.PictureBox2.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.BackColor = System.Drawing.Color.Firebrick
        Me.PictureBox3.Image = Global.PowerPlant.My.Resources.Resources.mp_logo
        Me.PictureBox3.Location = New System.Drawing.Point(257, 56)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(285, 174)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox3.TabIndex = 15
        Me.PictureBox3.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Firebrick
        Me.PictureBox1.Image = Global.PowerPlant.My.Resources.Resources.mp_logo_r
        Me.PictureBox1.Location = New System.Drawing.Point(542, 56)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(257, 174)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 15
        Me.PictureBox1.TabStop = False
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
        'btnTSCtlPnl
        '
        Me.btnTSCtlPnl.BackColor = System.Drawing.Color.Silver
        Me.btnTSCtlPnl.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTSCtlPnl.Location = New System.Drawing.Point(203, 490)
        Me.btnTSCtlPnl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnTSCtlPnl.Name = "btnTSCtlPnl"
        Me.btnTSCtlPnl.Size = New System.Drawing.Size(150, 65)
        Me.btnTSCtlPnl.TabIndex = 80
        Me.btnTSCtlPnl.Text = "Touch Screen Control Panel"
        Me.btnTSCtlPnl.UseVisualStyleBackColor = False
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
        'frmLogOn
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnTSCtlPnl)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.PictureBox3)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.UcHeading1)
        Me.Controls.Add(Me.btnPrintControl)
        Me.Controls.Add(Me.txtOperator)
        Me.Controls.Add(Me.txtShift)
        Me.Controls.Add(Me.lblPkgLine)
        Me.Controls.Add(Me.lblShift)
        Me.Controls.Add(Me.lblOperator)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtPkgLine)
        Me.Controls.Add(Me.Label7)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLogOn"
        Me.Text = "Log On"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtPkgLine As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtOperator As System.Windows.Forms.TextBox
    Friend WithEvents txtShift As System.Windows.Forms.TextBox
    Friend WithEvents lblOperator As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnPrintControl As System.Windows.Forms.Button
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents lblShift As System.Windows.Forms.Label
    Friend WithEvents lblPkgLine As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnTSCtlPnl As System.Windows.Forms.Button
End Class
