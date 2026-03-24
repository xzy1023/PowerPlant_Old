<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInitialPgm
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
        Me.btnPalletStation = New System.Windows.Forms.Button()
        Me.btnRunIndusoft = New System.Windows.Forms.Button()
        Me.btnNoIndusoft = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnShupDown = New System.Windows.Forms.Button()
        Me.btnCfgIPC = New System.Windows.Forms.Button()
        Me.btnCfgIPCViewer = New System.Windows.Forms.Button()
        Me.btnIPCViewer = New System.Windows.Forms.Button()
        Me.btnTSCtlPnl = New System.Windows.Forms.Button()
        Me.btnInquiry = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.UcHeading1 = New IPCInitialPgm.ucHeading()
        Me.SuspendLayout()
        '
        'btnPalletStation
        '
        Me.btnPalletStation.BackColor = System.Drawing.Color.OldLace
        Me.btnPalletStation.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPalletStation.Location = New System.Drawing.Point(228, 146)
        Me.btnPalletStation.Name = "btnPalletStation"
        Me.btnPalletStation.Size = New System.Drawing.Size(352, 65)
        Me.btnPalletStation.TabIndex = 0
        Me.btnPalletStation.Text = "Pallet Station"
        Me.btnPalletStation.UseVisualStyleBackColor = False
        '
        'btnRunIndusoft
        '
        Me.btnRunIndusoft.BackColor = System.Drawing.Color.OldLace
        Me.btnRunIndusoft.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRunIndusoft.Location = New System.Drawing.Point(228, 232)
        Me.btnRunIndusoft.Name = "btnRunIndusoft"
        Me.btnRunIndusoft.Size = New System.Drawing.Size(352, 65)
        Me.btnRunIndusoft.TabIndex = 0
        Me.btnRunIndusoft.Text = "Line has HMI"
        Me.btnRunIndusoft.UseVisualStyleBackColor = False
        '
        'btnNoIndusoft
        '
        Me.btnNoIndusoft.BackColor = System.Drawing.Color.OldLace
        Me.btnNoIndusoft.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNoIndusoft.Location = New System.Drawing.Point(228, 320)
        Me.btnNoIndusoft.Name = "btnNoIndusoft"
        Me.btnNoIndusoft.Size = New System.Drawing.Size(352, 65)
        Me.btnNoIndusoft.TabIndex = 0
        Me.btnNoIndusoft.Text = "Line has no HMI"
        Me.btnNoIndusoft.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 18.0!)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(30, 95)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(676, 27)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "2. Select one of the following buttons to let the IPC to Run as:"
        '
        'btnShupDown
        '
        Me.btnShupDown.BackColor = System.Drawing.Color.Silver
        Me.btnShupDown.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnShupDown.Location = New System.Drawing.Point(27, 490)
        Me.btnShupDown.Name = "btnShupDown"
        Me.btnShupDown.Size = New System.Drawing.Size(150, 65)
        Me.btnShupDown.TabIndex = 0
        Me.btnShupDown.Text = "Exit"
        Me.btnShupDown.UseVisualStyleBackColor = False
        '
        'btnCfgIPC
        '
        Me.btnCfgIPC.BackColor = System.Drawing.Color.Silver
        Me.btnCfgIPC.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnCfgIPC.Location = New System.Drawing.Point(200, 490)
        Me.btnCfgIPC.Name = "btnCfgIPC"
        Me.btnCfgIPC.Size = New System.Drawing.Size(150, 65)
        Me.btnCfgIPC.TabIndex = 0
        Me.btnCfgIPC.Text = "Configure IPC"
        Me.btnCfgIPC.UseVisualStyleBackColor = False
        '
        'btnCfgIPCViewer
        '
        Me.btnCfgIPCViewer.BackColor = System.Drawing.Color.Silver
        Me.btnCfgIPCViewer.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnCfgIPCViewer.Location = New System.Drawing.Point(376, 490)
        Me.btnCfgIPCViewer.Name = "btnCfgIPCViewer"
        Me.btnCfgIPCViewer.Size = New System.Drawing.Size(150, 65)
        Me.btnCfgIPCViewer.TabIndex = 4
        Me.btnCfgIPCViewer.Text = "Configure IPC Viewer"
        Me.btnCfgIPCViewer.UseVisualStyleBackColor = False
        '
        'btnIPCViewer
        '
        Me.btnIPCViewer.BackColor = System.Drawing.Color.OldLace
        Me.btnIPCViewer.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnIPCViewer.Location = New System.Drawing.Point(228, 408)
        Me.btnIPCViewer.Name = "btnIPCViewer"
        Me.btnIPCViewer.Size = New System.Drawing.Size(352, 65)
        Me.btnIPCViewer.TabIndex = 5
        Me.btnIPCViewer.Text = "IPC Viewer"
        Me.btnIPCViewer.UseVisualStyleBackColor = False
        '
        'btnTSCtlPnl
        '
        Me.btnTSCtlPnl.BackColor = System.Drawing.Color.Silver
        Me.btnTSCtlPnl.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTSCtlPnl.Location = New System.Drawing.Point(27, 146)
        Me.btnTSCtlPnl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnTSCtlPnl.Name = "btnTSCtlPnl"
        Me.btnTSCtlPnl.Size = New System.Drawing.Size(150, 65)
        Me.btnTSCtlPnl.TabIndex = 80
        Me.btnTSCtlPnl.Text = "Touch Screen Control Panel"
        Me.btnTSCtlPnl.UseVisualStyleBackColor = False
        '
        'btnInquiry
        '
        Me.btnInquiry.BackColor = System.Drawing.Color.Silver
        Me.btnInquiry.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnInquiry.Location = New System.Drawing.Point(552, 490)
        Me.btnInquiry.Name = "btnInquiry"
        Me.btnInquiry.Size = New System.Drawing.Size(150, 65)
        Me.btnInquiry.TabIndex = 81
        Me.btnInquiry.Text = "Inquiry"
        Me.btnInquiry.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(30, 66)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(697, 27)
        Me.Label2.TabIndex = 82
        Me.Label2.Text = "1. Inquiry-->Refresh Data. Configure the IPC for a line or viewer."
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(0, 0)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "-- Heading --"
        Me.UcHeading1.Size = New System.Drawing.Size(800, 55)
        Me.UcHeading1.TabIndex = 2
        '
        'frmInitialPgm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnInquiry)
        Me.Controls.Add(Me.btnTSCtlPnl)
        Me.Controls.Add(Me.btnIPCViewer)
        Me.Controls.Add(Me.btnCfgIPCViewer)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.UcHeading1)
        Me.Controls.Add(Me.btnCfgIPC)
        Me.Controls.Add(Me.btnShupDown)
        Me.Controls.Add(Me.btnNoIndusoft)
        Me.Controls.Add(Me.btnRunIndusoft)
        Me.Controls.Add(Me.btnPalletStation)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInitialPgm"
        Me.Text = "Select IPC Function"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnPalletStation As System.Windows.Forms.Button
    Friend WithEvents UcHeading1 As IPCInitialPgm.ucHeading
    Friend WithEvents btnRunIndusoft As System.Windows.Forms.Button
    Friend WithEvents btnNoIndusoft As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnShupDown As System.Windows.Forms.Button
    Friend WithEvents btnCfgIPC As System.Windows.Forms.Button
    Friend WithEvents btnCfgIPCViewer As System.Windows.Forms.Button
    Friend WithEvents btnIPCViewer As System.Windows.Forms.Button
    Friend WithEvents btnTSCtlPnl As System.Windows.Forms.Button
    Friend WithEvents btnInquiry As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label

End Class
