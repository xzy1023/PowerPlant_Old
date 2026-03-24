<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStartShopOrder
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
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblBagLength = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.btnStartShopOrder = New System.Windows.Forms.Button()
        Me.btnChkBOM = New System.Windows.Forms.Button()
        Me.BtnChkSONotes = New System.Windows.Forms.Button()
        Me.txtPkgLine = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblItemDesc = New System.Windows.Forms.Label()
        Me.lblUnitPerCase = New System.Windows.Forms.Label()
        Me.lblItemNo = New System.Windows.Forms.Label()
        Me.lblLabelWeight = New System.Windows.Forms.Label()
        Me.lblCasesRemainInShift = New System.Windows.Forms.Label()
        Me.txtShift = New System.Windows.Forms.TextBox()
        Me.txtBagLengthUsed = New System.Windows.Forms.TextBox()
        Me.lblOperator = New System.Windows.Forms.Label()
        Me.lblUtilityTech1 = New System.Windows.Forms.Label()
        Me.txtUtilityTech1 = New System.Windows.Forms.TextBox()
        Me.txtUtilityTech2 = New System.Windows.Forms.TextBox()
        Me.txtUtilityTech3 = New System.Windows.Forms.TextBox()
        Me.txtUtilityTech4 = New System.Windows.Forms.TextBox()
        Me.lblUtilityTech2 = New System.Windows.Forms.Label()
        Me.lblUtilityTech3 = New System.Windows.Forms.Label()
        Me.lblUtilityTech4 = New System.Windows.Forms.Label()
        Me.lblStdBagLength = New System.Windows.Forms.Label()
        Me.lblCasesProducedInShift = New System.Windows.Forms.Label()
        Me.lblActBagLength = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtOperator = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.gbxUtilityTech = New System.Windows.Forms.GroupBox()
        Me.txtCasesScheduledInShift = New System.Windows.Forms.TextBox()
        Me.cboShopOrder = New System.Windows.Forms.ComboBox()
        Me.lblTotalProduced = New System.Windows.Forms.Label()
        Me.lblTotalRemain = New System.Windows.Forms.Label()
        Me.lblTotalScheduled = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.lblCFCases = New System.Windows.Forms.Label()
        Me.txtCFCases = New System.Windows.Forms.TextBox()
        Me.btnStartWithNoLabel = New System.Windows.Forms.Button()
        Me.lblOutputLocationLabel = New System.Windows.Forms.Label()
        Me.lblDestSOLabel = New System.Windows.Forms.Label()
        Me.lblDestShopOrder = New System.Windows.Forms.Label()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.lblOutputLocation = New System.Windows.Forms.Label()
        Me.gbxUtilityTech.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnPrvScn
        '
        Me.btnPrvScn.BackColor = System.Drawing.Color.Silver
        Me.btnPrvScn.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrvScn.Location = New System.Drawing.Point(18, 490)
        Me.btnPrvScn.Name = "btnPrvScn"
        Me.btnPrvScn.Size = New System.Drawing.Size(150, 65)
        Me.btnPrvScn.TabIndex = 10
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(26, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(177, 35)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Shop Order:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(26, 139)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(169, 35)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Units/Case:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(26, 181)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(345, 35)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Unit Label Weight (gms):"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(23, 452)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(126, 35)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Remain:"
        '
        'lblBagLength
        '
        Me.lblBagLength.AutoSize = True
        Me.lblBagLength.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBagLength.ForeColor = System.Drawing.Color.White
        Me.lblBagLength.Location = New System.Drawing.Point(25, 222)
        Me.lblBagLength.Name = "lblBagLength"
        Me.lblBagLength.Size = New System.Drawing.Size(352, 35)
        Me.lblBagLength.TabIndex = 2
        Me.lblBagLength.Text = "Std. Bag Length (inches):"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(23, 412)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(151, 35)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Produced:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(428, 139)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(228, 35)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "Packaging Line:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(428, 184)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(83, 35)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Shift:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(424, 225)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(139, 35)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Operator:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(17, 41)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(40, 35)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "1."
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(17, 85)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(40, 35)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "2."
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.White
        Me.Label12.Location = New System.Drawing.Point(17, 173)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(40, 35)
        Me.Label12.TabIndex = 2
        Me.Label12.Text = "4."
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.White
        Me.Label13.Location = New System.Drawing.Point(17, 129)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(40, 35)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "3."
        '
        'btnStartShopOrder
        '
        Me.btnStartShopOrder.BackColor = System.Drawing.Color.Silver
        Me.btnStartShopOrder.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnStartShopOrder.Location = New System.Drawing.Point(174, 490)
        Me.btnStartShopOrder.Name = "btnStartShopOrder"
        Me.btnStartShopOrder.Size = New System.Drawing.Size(150, 65)
        Me.btnStartShopOrder.TabIndex = 11
        Me.btnStartShopOrder.Text = "Start Shop Order"
        Me.btnStartShopOrder.UseVisualStyleBackColor = False
        '
        'btnChkBOM
        '
        Me.btnChkBOM.BackColor = System.Drawing.Color.Silver
        Me.btnChkBOM.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnChkBOM.Location = New System.Drawing.Point(330, 490)
        Me.btnChkBOM.Name = "btnChkBOM"
        Me.btnChkBOM.Size = New System.Drawing.Size(150, 65)
        Me.btnChkBOM.TabIndex = 12
        Me.btnChkBOM.Text = "Check BOM"
        Me.btnChkBOM.UseVisualStyleBackColor = False
        '
        'BtnChkSONotes
        '
        Me.BtnChkSONotes.BackColor = System.Drawing.Color.Silver
        Me.BtnChkSONotes.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.BtnChkSONotes.Location = New System.Drawing.Point(486, 490)
        Me.BtnChkSONotes.Name = "BtnChkSONotes"
        Me.BtnChkSONotes.Size = New System.Drawing.Size(150, 65)
        Me.BtnChkSONotes.TabIndex = 13
        Me.BtnChkSONotes.Text = "Shop Order Notes"
        Me.BtnChkSONotes.UseVisualStyleBackColor = False
        '
        'txtPkgLine
        '
        Me.txtPkgLine.BackColor = System.Drawing.Color.Black
        Me.txtPkgLine.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPkgLine.ForeColor = System.Drawing.Color.White
        Me.txtPkgLine.Location = New System.Drawing.Point(624, 136)
        Me.txtPkgLine.MaxLength = 10
        Me.txtPkgLine.Name = "txtPkgLine"
        Me.txtPkgLine.Size = New System.Drawing.Size(129, 42)
        Me.txtPkgLine.TabIndex = 2
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.White
        Me.Label14.Location = New System.Drawing.Point(26, 103)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(200, 35)
        Me.Label14.TabIndex = 2
        Me.Label14.Text = "SKU Number:"
        '
        'lblItemDesc
        '
        Me.lblItemDesc.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemDesc.ForeColor = System.Drawing.Color.White
        Me.lblItemDesc.Location = New System.Drawing.Point(428, 103)
        Me.lblItemDesc.Name = "lblItemDesc"
        Me.lblItemDesc.Size = New System.Drawing.Size(360, 27)
        Me.lblItemDesc.TabIndex = 2
        Me.lblItemDesc.Text = "Desc"
        '
        'lblUnitPerCase
        '
        Me.lblUnitPerCase.AutoSize = True
        Me.lblUnitPerCase.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitPerCase.ForeColor = System.Drawing.Color.White
        Me.lblUnitPerCase.Location = New System.Drawing.Point(311, 139)
        Me.lblUnitPerCase.Name = "lblUnitPerCase"
        Me.lblUnitPerCase.Size = New System.Drawing.Size(32, 35)
        Me.lblUnitPerCase.TabIndex = 2
        Me.lblUnitPerCase.Text = "0"
        '
        'lblItemNo
        '
        Me.lblItemNo.AutoSize = True
        Me.lblItemNo.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemNo.ForeColor = System.Drawing.Color.White
        Me.lblItemNo.Location = New System.Drawing.Point(201, 101)
        Me.lblItemNo.Name = "lblItemNo"
        Me.lblItemNo.Size = New System.Drawing.Size(0, 35)
        Me.lblItemNo.TabIndex = 2
        '
        'lblLabelWeight
        '
        Me.lblLabelWeight.AutoSize = True
        Me.lblLabelWeight.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLabelWeight.ForeColor = System.Drawing.Color.White
        Me.lblLabelWeight.Location = New System.Drawing.Point(309, 184)
        Me.lblLabelWeight.Name = "lblLabelWeight"
        Me.lblLabelWeight.Size = New System.Drawing.Size(74, 35)
        Me.lblLabelWeight.TabIndex = 2
        Me.lblLabelWeight.Text = "0.00"
        '
        'lblCasesRemainInShift
        '
        Me.lblCasesRemainInShift.AutoSize = True
        Me.lblCasesRemainInShift.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCasesRemainInShift.ForeColor = System.Drawing.Color.White
        Me.lblCasesRemainInShift.Location = New System.Drawing.Point(275, 452)
        Me.lblCasesRemainInShift.Name = "lblCasesRemainInShift"
        Me.lblCasesRemainInShift.Size = New System.Drawing.Size(32, 35)
        Me.lblCasesRemainInShift.TabIndex = 2
        Me.lblCasesRemainInShift.Text = "0"
        '
        'txtShift
        '
        Me.txtShift.BackColor = System.Drawing.Color.Black
        Me.txtShift.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShift.ForeColor = System.Drawing.Color.White
        Me.txtShift.Location = New System.Drawing.Point(550, 176)
        Me.txtShift.MaxLength = 1
        Me.txtShift.Name = "txtShift"
        Me.txtShift.Size = New System.Drawing.Size(31, 42)
        Me.txtShift.TabIndex = 3
        '
        'txtBagLengthUsed
        '
        Me.txtBagLengthUsed.BackColor = System.Drawing.Color.Black
        Me.txtBagLengthUsed.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBagLengthUsed.ForeColor = System.Drawing.Color.White
        Me.txtBagLengthUsed.Location = New System.Drawing.Point(310, 260)
        Me.txtBagLengthUsed.MaxLength = 5
        Me.txtBagLengthUsed.Name = "txtBagLengthUsed"
        Me.txtBagLengthUsed.Size = New System.Drawing.Size(83, 42)
        Me.txtBagLengthUsed.TabIndex = 9
        '
        'lblOperator
        '
        Me.lblOperator.AutoSize = True
        Me.lblOperator.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOperator.ForeColor = System.Drawing.Color.White
        Me.lblOperator.Location = New System.Drawing.Point(619, 225)
        Me.lblOperator.MaximumSize = New System.Drawing.Size(250, 27)
        Me.lblOperator.Name = "lblOperator"
        Me.lblOperator.Size = New System.Drawing.Size(94, 27)
        Me.lblOperator.TabIndex = 2
        Me.lblOperator.Text = "Name"
        '
        'lblUtilityTech1
        '
        Me.lblUtilityTech1.AutoSize = True
        Me.lblUtilityTech1.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUtilityTech1.ForeColor = System.Drawing.Color.White
        Me.lblUtilityTech1.Location = New System.Drawing.Point(140, 41)
        Me.lblUtilityTech1.Name = "lblUtilityTech1"
        Me.lblUtilityTech1.Size = New System.Drawing.Size(94, 35)
        Me.lblUtilityTech1.TabIndex = 2
        Me.lblUtilityTech1.Text = "Name"
        '
        'txtUtilityTech1
        '
        Me.txtUtilityTech1.BackColor = System.Drawing.Color.Black
        Me.txtUtilityTech1.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUtilityTech1.ForeColor = System.Drawing.Color.White
        Me.txtUtilityTech1.Location = New System.Drawing.Point(58, 38)
        Me.txtUtilityTech1.MaxLength = 4
        Me.txtUtilityTech1.Name = "txtUtilityTech1"
        Me.txtUtilityTech1.Size = New System.Drawing.Size(75, 42)
        Me.txtUtilityTech1.TabIndex = 5
        '
        'txtUtilityTech2
        '
        Me.txtUtilityTech2.BackColor = System.Drawing.Color.Black
        Me.txtUtilityTech2.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUtilityTech2.ForeColor = System.Drawing.Color.White
        Me.txtUtilityTech2.Location = New System.Drawing.Point(58, 82)
        Me.txtUtilityTech2.MaxLength = 4
        Me.txtUtilityTech2.Name = "txtUtilityTech2"
        Me.txtUtilityTech2.Size = New System.Drawing.Size(75, 42)
        Me.txtUtilityTech2.TabIndex = 6
        '
        'txtUtilityTech3
        '
        Me.txtUtilityTech3.BackColor = System.Drawing.Color.Black
        Me.txtUtilityTech3.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUtilityTech3.ForeColor = System.Drawing.Color.White
        Me.txtUtilityTech3.Location = New System.Drawing.Point(59, 126)
        Me.txtUtilityTech3.MaxLength = 4
        Me.txtUtilityTech3.Name = "txtUtilityTech3"
        Me.txtUtilityTech3.Size = New System.Drawing.Size(75, 42)
        Me.txtUtilityTech3.TabIndex = 7
        '
        'txtUtilityTech4
        '
        Me.txtUtilityTech4.BackColor = System.Drawing.Color.Black
        Me.txtUtilityTech4.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUtilityTech4.ForeColor = System.Drawing.Color.White
        Me.txtUtilityTech4.Location = New System.Drawing.Point(59, 170)
        Me.txtUtilityTech4.MaxLength = 4
        Me.txtUtilityTech4.Name = "txtUtilityTech4"
        Me.txtUtilityTech4.Size = New System.Drawing.Size(75, 42)
        Me.txtUtilityTech4.TabIndex = 8
        '
        'lblUtilityTech2
        '
        Me.lblUtilityTech2.AutoSize = True
        Me.lblUtilityTech2.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUtilityTech2.ForeColor = System.Drawing.Color.White
        Me.lblUtilityTech2.Location = New System.Drawing.Point(140, 85)
        Me.lblUtilityTech2.Name = "lblUtilityTech2"
        Me.lblUtilityTech2.Size = New System.Drawing.Size(94, 35)
        Me.lblUtilityTech2.TabIndex = 2
        Me.lblUtilityTech2.Text = "Name"
        '
        'lblUtilityTech3
        '
        Me.lblUtilityTech3.AutoSize = True
        Me.lblUtilityTech3.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUtilityTech3.ForeColor = System.Drawing.Color.White
        Me.lblUtilityTech3.Location = New System.Drawing.Point(140, 129)
        Me.lblUtilityTech3.Name = "lblUtilityTech3"
        Me.lblUtilityTech3.Size = New System.Drawing.Size(94, 35)
        Me.lblUtilityTech3.TabIndex = 2
        Me.lblUtilityTech3.Text = "Name"
        '
        'lblUtilityTech4
        '
        Me.lblUtilityTech4.AutoSize = True
        Me.lblUtilityTech4.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUtilityTech4.ForeColor = System.Drawing.Color.White
        Me.lblUtilityTech4.Location = New System.Drawing.Point(140, 173)
        Me.lblUtilityTech4.Name = "lblUtilityTech4"
        Me.lblUtilityTech4.Size = New System.Drawing.Size(94, 35)
        Me.lblUtilityTech4.TabIndex = 2
        Me.lblUtilityTech4.Text = "Name"
        '
        'lblStdBagLength
        '
        Me.lblStdBagLength.AutoSize = True
        Me.lblStdBagLength.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStdBagLength.ForeColor = System.Drawing.Color.White
        Me.lblStdBagLength.Location = New System.Drawing.Point(311, 225)
        Me.lblStdBagLength.Name = "lblStdBagLength"
        Me.lblStdBagLength.Size = New System.Drawing.Size(57, 35)
        Me.lblStdBagLength.TabIndex = 2
        Me.lblStdBagLength.Text = "0.0"
        '
        'lblCasesProducedInShift
        '
        Me.lblCasesProducedInShift.AutoSize = True
        Me.lblCasesProducedInShift.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCasesProducedInShift.ForeColor = System.Drawing.Color.White
        Me.lblCasesProducedInShift.Location = New System.Drawing.Point(275, 412)
        Me.lblCasesProducedInShift.Name = "lblCasesProducedInShift"
        Me.lblCasesProducedInShift.Size = New System.Drawing.Size(32, 35)
        Me.lblCasesProducedInShift.TabIndex = 2
        Me.lblCasesProducedInShift.Text = "0"
        '
        'lblActBagLength
        '
        Me.lblActBagLength.AutoSize = True
        Me.lblActBagLength.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActBagLength.ForeColor = System.Drawing.Color.White
        Me.lblActBagLength.Location = New System.Drawing.Point(25, 263)
        Me.lblActBagLength.Name = "lblActBagLength"
        Me.lblActBagLength.Size = New System.Drawing.Size(349, 35)
        Me.lblActBagLength.TabIndex = 14
        Me.lblActBagLength.Text = "Act. Bag Length (inches):"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.Color.White
        Me.Label18.Location = New System.Drawing.Point(22, 371)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(165, 35)
        Me.Label18.TabIndex = 2
        Me.Label18.Text = "Scheduled:"
        '
        'txtOperator
        '
        Me.txtOperator.BackColor = System.Drawing.Color.Black
        Me.txtOperator.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOperator.ForeColor = System.Drawing.Color.White
        Me.txtOperator.Location = New System.Drawing.Point(551, 219)
        Me.txtOperator.MaxLength = 4
        Me.txtOperator.Name = "txtOperator"
        Me.txtOperator.Size = New System.Drawing.Size(61, 42)
        Me.txtOperator.TabIndex = 4
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.White
        Me.Label15.Location = New System.Drawing.Point(26, 103)
        Me.Label15.Margin = New System.Windows.Forms.Padding(3, 0, 1, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(200, 35)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = "SKU Number:"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(201, 101)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(0, 35)
        Me.Label16.TabIndex = 2
        '
        'gbxUtilityTech
        '
        Me.gbxUtilityTech.Controls.Add(Me.txtUtilityTech1)
        Me.gbxUtilityTech.Controls.Add(Me.lblUtilityTech1)
        Me.gbxUtilityTech.Controls.Add(Me.lblUtilityTech2)
        Me.gbxUtilityTech.Controls.Add(Me.txtUtilityTech4)
        Me.gbxUtilityTech.Controls.Add(Me.lblUtilityTech3)
        Me.gbxUtilityTech.Controls.Add(Me.txtUtilityTech3)
        Me.gbxUtilityTech.Controls.Add(Me.lblUtilityTech4)
        Me.gbxUtilityTech.Controls.Add(Me.txtUtilityTech2)
        Me.gbxUtilityTech.Controls.Add(Me.Label10)
        Me.gbxUtilityTech.Controls.Add(Me.Label11)
        Me.gbxUtilityTech.Controls.Add(Me.Label12)
        Me.gbxUtilityTech.Controls.Add(Me.Label13)
        Me.gbxUtilityTech.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbxUtilityTech.ForeColor = System.Drawing.Color.White
        Me.gbxUtilityTech.Location = New System.Drawing.Point(415, 265)
        Me.gbxUtilityTech.Name = "gbxUtilityTech"
        Me.gbxUtilityTech.Size = New System.Drawing.Size(365, 214)
        Me.gbxUtilityTech.TabIndex = 16
        Me.gbxUtilityTech.TabStop = False
        Me.gbxUtilityTech.Text = "Utility Tech."
        '
        'txtCasesScheduledInShift
        '
        Me.txtCasesScheduledInShift.BackColor = System.Drawing.Color.Black
        Me.txtCasesScheduledInShift.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCasesScheduledInShift.ForeColor = System.Drawing.Color.White
        Me.txtCasesScheduledInShift.Location = New System.Drawing.Point(280, 368)
        Me.txtCasesScheduledInShift.MaxLength = 7
        Me.txtCasesScheduledInShift.Name = "txtCasesScheduledInShift"
        Me.txtCasesScheduledInShift.Size = New System.Drawing.Size(86, 42)
        Me.txtCasesScheduledInShift.TabIndex = 10
        '
        'cboShopOrder
        '
        Me.cboShopOrder.BackColor = System.Drawing.Color.Black
        Me.cboShopOrder.DropDownHeight = 320
        Me.cboShopOrder.DropDownWidth = 425
        Me.cboShopOrder.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboShopOrder.ForeColor = System.Drawing.Color.White
        Me.cboShopOrder.FormattingEnabled = True
        Me.cboShopOrder.IntegralHeight = False
        Me.cboShopOrder.ItemHeight = 35
        Me.cboShopOrder.Location = New System.Drawing.Point(175, 62)
        Me.cboShopOrder.MaxDropDownItems = 7
        Me.cboShopOrder.MaxLength = 8
        Me.cboShopOrder.Name = "cboShopOrder"
        Me.cboShopOrder.Size = New System.Drawing.Size(255, 43)
        Me.cboShopOrder.TabIndex = 1
        '
        'lblTotalProduced
        '
        Me.lblTotalProduced.AutoSize = True
        Me.lblTotalProduced.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalProduced.ForeColor = System.Drawing.Color.White
        Me.lblTotalProduced.Location = New System.Drawing.Point(159, 412)
        Me.lblTotalProduced.Name = "lblTotalProduced"
        Me.lblTotalProduced.Size = New System.Drawing.Size(32, 35)
        Me.lblTotalProduced.TabIndex = 2
        Me.lblTotalProduced.Text = "0"
        '
        'lblTotalRemain
        '
        Me.lblTotalRemain.AutoSize = True
        Me.lblTotalRemain.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalRemain.ForeColor = System.Drawing.Color.White
        Me.lblTotalRemain.Location = New System.Drawing.Point(159, 452)
        Me.lblTotalRemain.Name = "lblTotalRemain"
        Me.lblTotalRemain.Size = New System.Drawing.Size(32, 35)
        Me.lblTotalRemain.TabIndex = 2
        Me.lblTotalRemain.Text = "0"
        '
        'lblTotalScheduled
        '
        Me.lblTotalScheduled.AutoSize = True
        Me.lblTotalScheduled.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalScheduled.ForeColor = System.Drawing.Color.White
        Me.lblTotalScheduled.Location = New System.Drawing.Point(160, 371)
        Me.lblTotalScheduled.Name = "lblTotalScheduled"
        Me.lblTotalScheduled.Size = New System.Drawing.Size(32, 35)
        Me.lblTotalScheduled.TabIndex = 2
        Me.lblTotalScheduled.Text = "0"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.RoyalBlue
        Me.Label5.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Italic Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(25, 339)
        Me.Label5.Margin = New System.Windows.Forms.Padding(0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(179, 28)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "(Cases)            "
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.BackColor = System.Drawing.Color.RoyalBlue
        Me.Label17.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Italic Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.White
        Me.Label17.Location = New System.Drawing.Point(162, 339)
        Me.Label17.Margin = New System.Windows.Forms.Padding(0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(153, 28)
        Me.Label17.TabIndex = 2
        Me.Label17.Text = "Order Total   "
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.BackColor = System.Drawing.Color.RoyalBlue
        Me.Label19.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Italic Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.White
        Me.Label19.Location = New System.Drawing.Point(281, 339)
        Me.Label19.Margin = New System.Windows.Forms.Padding(0)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(134, 28)
        Me.Label19.TabIndex = 2
        Me.Label19.Text = "Shift Total  "
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ForeColor = System.Drawing.Color.LightSalmon
        Me.lblMessage.Location = New System.Drawing.Point(22, 565)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(135, 35)
        Me.lblMessage.TabIndex = 89
        Me.lblMessage.Text = "Message"
        '
        'lblCFCases
        '
        Me.lblCFCases.AutoSize = True
        Me.lblCFCases.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCFCases.ForeColor = System.Drawing.Color.White
        Me.lblCFCases.Location = New System.Drawing.Point(26, 302)
        Me.lblCFCases.Name = "lblCFCases"
        Me.lblCFCases.Size = New System.Drawing.Size(329, 35)
        Me.lblCFCases.TabIndex = 14
        Me.lblCFCases.Text = "Carried Forward Cases:"
        '
        'txtCFCases
        '
        Me.txtCFCases.BackColor = System.Drawing.Color.Black
        Me.txtCFCases.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCFCases.ForeColor = System.Drawing.Color.White
        Me.txtCFCases.Location = New System.Drawing.Point(310, 299)
        Me.txtCFCases.MaxLength = 6
        Me.txtCFCases.Name = "txtCFCases"
        Me.txtCFCases.Size = New System.Drawing.Size(83, 42)
        Me.txtCFCases.TabIndex = 9
        '
        'btnStartWithNoLabel
        '
        Me.btnStartWithNoLabel.BackColor = System.Drawing.Color.Gold
        Me.btnStartWithNoLabel.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnStartWithNoLabel.ForeColor = System.Drawing.Color.Crimson
        Me.btnStartWithNoLabel.Location = New System.Drawing.Point(641, 490)
        Me.btnStartWithNoLabel.Name = "btnStartWithNoLabel"
        Me.btnStartWithNoLabel.Size = New System.Drawing.Size(150, 65)
        Me.btnStartWithNoLabel.TabIndex = 11
        Me.btnStartWithNoLabel.Text = "Start With No Label"
        Me.btnStartWithNoLabel.UseVisualStyleBackColor = False
        Me.btnStartWithNoLabel.Visible = False
        '
        'lblOutputLocationLabel
        '
        Me.lblOutputLocationLabel.AutoSize = True
        Me.lblOutputLocationLabel.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutputLocationLabel.ForeColor = System.Drawing.Color.White
        Me.lblOutputLocationLabel.Location = New System.Drawing.Point(26, 222)
        Me.lblOutputLocationLabel.Name = "lblOutputLocationLabel"
        Me.lblOutputLocationLabel.Size = New System.Drawing.Size(233, 35)
        Me.lblOutputLocationLabel.TabIndex = 101
        Me.lblOutputLocationLabel.Text = "Output Location:"
        Me.lblOutputLocationLabel.Visible = False
        '
        'lblDestSOLabel
        '
        Me.lblDestSOLabel.AutoSize = True
        Me.lblDestSOLabel.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDestSOLabel.ForeColor = System.Drawing.Color.White
        Me.lblDestSOLabel.Location = New System.Drawing.Point(26, 264)
        Me.lblDestSOLabel.Name = "lblDestSOLabel"
        Me.lblDestSOLabel.Size = New System.Drawing.Size(254, 35)
        Me.lblDestSOLabel.TabIndex = 105
        Me.lblDestSOLabel.Text = "Dest. Shop Order:"
        Me.lblDestSOLabel.Visible = False
        '
        'lblDestShopOrder
        '
        Me.lblDestShopOrder.BackColor = System.Drawing.Color.OliveDrab
        Me.lblDestShopOrder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblDestShopOrder.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDestShopOrder.ForeColor = System.Drawing.Color.White
        Me.lblDestShopOrder.Location = New System.Drawing.Point(234, 259)
        Me.lblDestShopOrder.Name = "lblDestShopOrder"
        Me.lblDestShopOrder.Size = New System.Drawing.Size(159, 35)
        Me.lblDestShopOrder.TabIndex = 106
        Me.lblDestShopOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblDestShopOrder.Visible = False
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(0, 0)
        Me.UcHeading1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "-- Heading --"
        Me.UcHeading1.Size = New System.Drawing.Size(925, 55)
        Me.UcHeading1.TabIndex = 15
        '
        'lblOutputLocation
        '
        Me.lblOutputLocation.BackColor = System.Drawing.Color.OliveDrab
        Me.lblOutputLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblOutputLocation.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutputLocation.ForeColor = System.Drawing.Color.White
        Me.lblOutputLocation.Location = New System.Drawing.Point(234, 216)
        Me.lblOutputLocation.Name = "lblOutputLocation"
        Me.lblOutputLocation.Size = New System.Drawing.Size(159, 35)
        Me.lblOutputLocation.TabIndex = 107
        Me.lblOutputLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblOutputLocation.Visible = False
        '
        'frmStartShopOrder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblOutputLocation)
        Me.Controls.Add(Me.lblDestShopOrder)
        Me.Controls.Add(Me.lblDestSOLabel)
        Me.Controls.Add(Me.lblOutputLocationLabel)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.cboShopOrder)
        Me.Controls.Add(Me.gbxUtilityTech)
        Me.Controls.Add(Me.UcHeading1)
        Me.Controls.Add(Me.lblCFCases)
        Me.Controls.Add(Me.lblActBagLength)
        Me.Controls.Add(Me.txtOperator)
        Me.Controls.Add(Me.txtCasesScheduledInShift)
        Me.Controls.Add(Me.txtCFCases)
        Me.Controls.Add(Me.txtBagLengthUsed)
        Me.Controls.Add(Me.txtShift)
        Me.Controls.Add(Me.txtPkgLine)
        Me.Controls.Add(Me.lblBagLength)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblOperator)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblTotalProduced)
        Me.Controls.Add(Me.lblTotalScheduled)
        Me.Controls.Add(Me.lblCasesProducedInShift)
        Me.Controls.Add(Me.lblTotalRemain)
        Me.Controls.Add(Me.lblCasesRemainInShift)
        Me.Controls.Add(Me.lblStdBagLength)
        Me.Controls.Add(Me.lblLabelWeight)
        Me.Controls.Add(Me.lblUnitPerCase)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.lblItemNo)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.lblItemDesc)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BtnChkSONotes)
        Me.Controls.Add(Me.btnChkBOM)
        Me.Controls.Add(Me.btnStartWithNoLabel)
        Me.Controls.Add(Me.btnStartShopOrder)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmStartShopOrder"
        Me.Text = "Start Shop Order"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.gbxUtilityTech.ResumeLayout(False)
        Me.gbxUtilityTech.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblBagLength As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents btnStartShopOrder As System.Windows.Forms.Button
    Friend WithEvents btnChkBOM As System.Windows.Forms.Button
    Friend WithEvents BtnChkSONotes As System.Windows.Forms.Button
    Friend WithEvents txtPkgLine As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lblItemDesc As System.Windows.Forms.Label
    Friend WithEvents lblUnitPerCase As System.Windows.Forms.Label
    Friend WithEvents lblItemNo As System.Windows.Forms.Label
    Friend WithEvents lblLabelWeight As System.Windows.Forms.Label
    Friend WithEvents lblCasesRemainInShift As System.Windows.Forms.Label
    Friend WithEvents txtShift As System.Windows.Forms.TextBox
    Friend WithEvents txtBagLengthUsed As System.Windows.Forms.TextBox
    Friend WithEvents lblOperator As System.Windows.Forms.Label
    Friend WithEvents lblUtilityTech1 As System.Windows.Forms.Label
    Friend WithEvents txtUtilityTech1 As System.Windows.Forms.TextBox
    Friend WithEvents txtUtilityTech2 As System.Windows.Forms.TextBox
    Friend WithEvents txtUtilityTech3 As System.Windows.Forms.TextBox
    Friend WithEvents txtUtilityTech4 As System.Windows.Forms.TextBox
    Friend WithEvents lblUtilityTech2 As System.Windows.Forms.Label
    Friend WithEvents lblUtilityTech3 As System.Windows.Forms.Label
    Friend WithEvents lblUtilityTech4 As System.Windows.Forms.Label
    Friend WithEvents lblStdBagLength As System.Windows.Forms.Label
    Friend WithEvents lblCasesProducedInShift As System.Windows.Forms.Label
    Friend WithEvents lblActBagLength As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtOperator As System.Windows.Forms.TextBox
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents gbxUtilityTech As System.Windows.Forms.GroupBox
    Friend WithEvents txtCasesScheduledInShift As System.Windows.Forms.TextBox
    Friend WithEvents cboShopOrder As System.Windows.Forms.ComboBox
    Friend WithEvents lblTotalProduced As System.Windows.Forms.Label
    Friend WithEvents lblTotalRemain As System.Windows.Forms.Label
    Friend WithEvents lblTotalScheduled As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents lblCFCases As System.Windows.Forms.Label
    Friend WithEvents txtCFCases As System.Windows.Forms.TextBox
    Friend WithEvents btnStartWithNoLabel As System.Windows.Forms.Button
    Friend WithEvents lblOutputLocationLabel As System.Windows.Forms.Label
    Friend WithEvents lblDestSOLabel As System.Windows.Forms.Label
    Friend WithEvents lblDestShopOrder As System.Windows.Forms.Label
    Friend WithEvents lblOutputLocation As System.Windows.Forms.Label

End Class
