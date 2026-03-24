<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmStopShopOrder
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblScreenTitle = New System.Windows.Forms.Label()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.lblSO = New System.Windows.Forms.Label()
        Me.lblItemNo = New System.Windows.Forms.Label()
        Me.lblSKUNumber = New System.Windows.Forms.Label()
        Me.lblShopOrder = New System.Windows.Forms.Label()
        Me.lblProduced = New System.Windows.Forms.Label()
        Me.lblFCarriedFwd = New System.Windows.Forms.Label()
        Me.lblScheduled = New System.Windows.Forms.Label()
        Me.lblCaseScheduledInShift = New System.Windows.Forms.Label()
        Me.lblBagLengthUsed = New System.Windows.Forms.Label()
        Me.lblRework = New System.Windows.Forms.Label()
        Me.lblLooseCases = New System.Windows.Forms.Label()
        Me.lblIsLastPallet = New System.Windows.Forms.Label()
        Me.lblLogScraps = New System.Windows.Forms.Label()
        Me.txtBagLengthUsed = New System.Windows.Forms.TextBox()
        Me.txtRework = New System.Windows.Forms.TextBox()
        Me.txtLooseCases = New System.Windows.Forms.TextBox()
        Me.lblItemDesc = New System.Windows.Forms.Label()
        Me.btnStopShopOrder = New System.Windows.Forms.Button()
        Me.btnPrvScn = New System.Windows.Forms.Button()
        Me.btnPalletFull = New System.Windows.Forms.Button()
        Me.btnPalletNotFull = New System.Windows.Forms.Button()
        Me.btnCloseSO = New System.Windows.Forms.Button()
        Me.btnNoLogScraps = New System.Windows.Forms.Button()
        Me.btnNoCloseSO = New System.Windows.Forms.Button()
        Me.btnLogScraps = New System.Windows.Forms.Button()
        Me.lblCasesRemainInShift = New System.Windows.Forms.Label()
        Me.lblRemain = New System.Windows.Forms.Label()
        Me.lblFullPalletQty = New System.Windows.Forms.Label()
        Me.lblCloseSO = New System.Windows.Forms.Label()
        Me.lblQtyPerPallet = New System.Windows.Forms.Label()
        Me.lblTotalRemain = New System.Windows.Forms.Label()
        Me.lblTotalScheduled = New System.Windows.Forms.Label()
        Me.lblTotalProduced = New System.Windows.Forms.Label()
        Me.lblCases = New System.Windows.Forms.Label()
        Me.lblOrderTotal = New System.Windows.Forms.Label()
        Me.lblShiftTotal = New System.Windows.Forms.Label()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.lblCasesProducedInShift = New System.Windows.Forms.Label()
        Me.lblFromCarriedFwd = New System.Windows.Forms.Label()
        Me.UcHeading1 = New PowerPlant.ucHeading()
        Me.btnLogRejectPoints = New System.Windows.Forms.Button()
        Me.btnNoLogRejectPoints = New System.Windows.Forms.Button()
        Me.lblLogRejectPoints = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblScreenTitle
        '
        Me.lblScreenTitle.AutoSize = True
        Me.lblScreenTitle.Font = New System.Drawing.Font("Tahoma", 25.0!, System.Drawing.FontStyle.Bold)
        Me.lblScreenTitle.Location = New System.Drawing.Point(-29, -234)
        Me.lblScreenTitle.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblScreenTitle.Name = "lblScreenTitle"
        Me.lblScreenTitle.Size = New System.Drawing.Size(634, 71)
        Me.lblScreenTitle.TabIndex = 3
        Me.lblScreenTitle.Text = "START SHOP ORDER"
        Me.lblScreenTitle.UseWaitCursor = True
        '
        'lblTime
        '
        Me.lblTime.AutoSize = True
        Me.lblTime.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTime.Location = New System.Drawing.Point(796, -198)
        Me.lblTime.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(94, 27)
        Me.lblTime.TabIndex = 4
        Me.lblTime.Text = "lblTime"
        Me.lblTime.UseWaitCursor = True
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDate.Location = New System.Drawing.Point(-356, -198)
        Me.lblDate.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(91, 27)
        Me.lblDate.TabIndex = 5
        Me.lblDate.Text = "lblDate"
        Me.lblDate.UseWaitCursor = True
        '
        'lblSO
        '
        Me.lblSO.AutoSize = True
        Me.lblSO.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSO.ForeColor = System.Drawing.Color.White
        Me.lblSO.Location = New System.Drawing.Point(22, 116)
        Me.lblSO.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblSO.Name = "lblSO"
        Me.lblSO.Size = New System.Drawing.Size(248, 47)
        Me.lblSO.TabIndex = 1
        Me.lblSO.Text = "Shop Order:"
        Me.lblSO.UseWaitCursor = True
        '
        'lblItemNo
        '
        Me.lblItemNo.AutoSize = True
        Me.lblItemNo.Font = New System.Drawing.Font("Arial", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemNo.ForeColor = System.Drawing.Color.White
        Me.lblItemNo.Location = New System.Drawing.Point(1027, 111)
        Me.lblItemNo.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblItemNo.Name = "lblItemNo"
        Me.lblItemNo.Size = New System.Drawing.Size(26, 40)
        Me.lblItemNo.TabIndex = 4
        Me.lblItemNo.Text = "I"
        Me.lblItemNo.UseWaitCursor = True
        '
        'lblSKUNumber
        '
        Me.lblSKUNumber.AutoSize = True
        Me.lblSKUNumber.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSKUNumber.ForeColor = System.Drawing.Color.White
        Me.lblSKUNumber.Location = New System.Drawing.Point(720, 111)
        Me.lblSKUNumber.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblSKUNumber.Name = "lblSKUNumber"
        Me.lblSKUNumber.Size = New System.Drawing.Size(279, 47)
        Me.lblSKUNumber.TabIndex = 3
        Me.lblSKUNumber.Text = "SKU Number:"
        Me.lblSKUNumber.UseWaitCursor = True
        '
        'lblShopOrder
        '
        Me.lblShopOrder.AutoSize = True
        Me.lblShopOrder.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShopOrder.ForeColor = System.Drawing.Color.White
        Me.lblShopOrder.Location = New System.Drawing.Point(299, 116)
        Me.lblShopOrder.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblShopOrder.Name = "lblShopOrder"
        Me.lblShopOrder.Size = New System.Drawing.Size(181, 47)
        Me.lblShopOrder.TabIndex = 2
        Me.lblShopOrder.Text = "0000000"
        Me.lblShopOrder.UseWaitCursor = True
        '
        'lblProduced
        '
        Me.lblProduced.AutoSize = True
        Me.lblProduced.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProduced.ForeColor = System.Drawing.Color.White
        Me.lblProduced.Location = New System.Drawing.Point(24, 310)
        Me.lblProduced.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblProduced.Name = "lblProduced"
        Me.lblProduced.Size = New System.Drawing.Size(210, 47)
        Me.lblProduced.TabIndex = 14
        Me.lblProduced.Text = "Produced:"
        Me.lblProduced.UseWaitCursor = True
        '
        'lblFCarriedFwd
        '
        Me.lblFCarriedFwd.AutoSize = True
        Me.lblFCarriedFwd.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFCarriedFwd.ForeColor = System.Drawing.Color.White
        Me.lblFCarriedFwd.Location = New System.Drawing.Point(720, 310)
        Me.lblFCarriedFwd.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblFCarriedFwd.Name = "lblFCarriedFwd"
        Me.lblFCarriedFwd.Size = New System.Drawing.Size(370, 47)
        Me.lblFCarriedFwd.TabIndex = 17
        Me.lblFCarriedFwd.Text = "From Carried Fwd:"
        Me.lblFCarriedFwd.UseWaitCursor = True
        '
        'lblScheduled
        '
        Me.lblScheduled.AutoSize = True
        Me.lblScheduled.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScheduled.ForeColor = System.Drawing.Color.White
        Me.lblScheduled.Location = New System.Drawing.Point(18, 241)
        Me.lblScheduled.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblScheduled.Name = "lblScheduled"
        Me.lblScheduled.Size = New System.Drawing.Size(229, 47)
        Me.lblScheduled.TabIndex = 9
        Me.lblScheduled.Text = "Scheduled:"
        Me.lblScheduled.UseWaitCursor = True
        '
        'lblCaseScheduledInShift
        '
        Me.lblCaseScheduledInShift.AutoSize = True
        Me.lblCaseScheduledInShift.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaseScheduledInShift.ForeColor = System.Drawing.Color.White
        Me.lblCaseScheduledInShift.Location = New System.Drawing.Point(488, 241)
        Me.lblCaseScheduledInShift.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblCaseScheduledInShift.Name = "lblCaseScheduledInShift"
        Me.lblCaseScheduledInShift.Size = New System.Drawing.Size(43, 47)
        Me.lblCaseScheduledInShift.TabIndex = 11
        Me.lblCaseScheduledInShift.Text = "0"
        Me.lblCaseScheduledInShift.UseWaitCursor = True
        '
        'lblBagLengthUsed
        '
        Me.lblBagLengthUsed.AutoSize = True
        Me.lblBagLengthUsed.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBagLengthUsed.ForeColor = System.Drawing.Color.White
        Me.lblBagLengthUsed.Location = New System.Drawing.Point(720, 240)
        Me.lblBagLengthUsed.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblBagLengthUsed.Name = "lblBagLengthUsed"
        Me.lblBagLengthUsed.Size = New System.Drawing.Size(490, 47)
        Me.lblBagLengthUsed.TabIndex = 12
        Me.lblBagLengthUsed.Text = "Act. Bag Length (inches):"
        Me.lblBagLengthUsed.UseWaitCursor = True
        '
        'lblRework
        '
        Me.lblRework.AutoSize = True
        Me.lblRework.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRework.ForeColor = System.Drawing.Color.White
        Me.lblRework.Location = New System.Drawing.Point(720, 384)
        Me.lblRework.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblRework.Name = "lblRework"
        Me.lblRework.Size = New System.Drawing.Size(265, 47)
        Me.lblRework.TabIndex = 22
        Me.lblRework.Text = "Rework (lbs):"
        Me.lblRework.UseWaitCursor = True
        '
        'lblLooseCases
        '
        Me.lblLooseCases.AutoSize = True
        Me.lblLooseCases.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLooseCases.ForeColor = System.Drawing.Color.White
        Me.lblLooseCases.Location = New System.Drawing.Point(22, 864)
        Me.lblLooseCases.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblLooseCases.Name = "lblLooseCases"
        Me.lblLooseCases.Size = New System.Drawing.Size(651, 47)
        Me.lblLooseCases.TabIndex = 35
        Me.lblLooseCases.Text = "No. of cases on incomplete pallet:"
        Me.lblLooseCases.UseWaitCursor = True
        Me.lblLooseCases.Visible = False
        '
        'lblIsLastPallet
        '
        Me.lblIsLastPallet.AutoSize = True
        Me.lblIsLastPallet.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsLastPallet.ForeColor = System.Drawing.Color.White
        Me.lblIsLastPallet.Location = New System.Drawing.Point(24, 767)
        Me.lblIsLastPallet.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblIsLastPallet.Name = "lblIsLastPallet"
        Me.lblIsLastPallet.Size = New System.Drawing.Size(381, 47)
        Me.lblIsLastPallet.TabIndex = 30
        Me.lblIsLastPallet.Text = "Zero Loose Cases?"
        Me.lblIsLastPallet.UseWaitCursor = True
        '
        'lblLogScraps
        '
        Me.lblLogScraps.AutoSize = True
        Me.lblLogScraps.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLogScraps.ForeColor = System.Drawing.Color.White
        Me.lblLogScraps.Location = New System.Drawing.Point(22, 487)
        Me.lblLogScraps.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblLogScraps.Name = "lblLogScraps"
        Me.lblLogScraps.Size = New System.Drawing.Size(253, 47)
        Me.lblLogScraps.TabIndex = 24
        Me.lblLogScraps.Text = "Log Scraps?"
        Me.lblLogScraps.UseWaitCursor = True
        '
        'txtBagLengthUsed
        '
        Me.txtBagLengthUsed.BackColor = System.Drawing.Color.Black
        Me.txtBagLengthUsed.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBagLengthUsed.ForeColor = System.Drawing.Color.White
        Me.txtBagLengthUsed.Location = New System.Drawing.Point(1254, 236)
        Me.txtBagLengthUsed.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.txtBagLengthUsed.MaxLength = 6
        Me.txtBagLengthUsed.Name = "txtBagLengthUsed"
        Me.txtBagLengthUsed.Size = New System.Drawing.Size(154, 56)
        Me.txtBagLengthUsed.TabIndex = 13
        Me.txtBagLengthUsed.UseWaitCursor = True
        '
        'txtRework
        '
        Me.txtRework.BackColor = System.Drawing.Color.Black
        Me.txtRework.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRework.ForeColor = System.Drawing.Color.White
        Me.txtRework.Location = New System.Drawing.Point(1014, 378)
        Me.txtRework.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.txtRework.MaxLength = 8
        Me.txtRework.Name = "txtRework"
        Me.txtRework.Size = New System.Drawing.Size(228, 56)
        Me.txtRework.TabIndex = 23
        Me.txtRework.UseWaitCursor = True
        '
        'txtLooseCases
        '
        Me.txtLooseCases.BackColor = System.Drawing.Color.Black
        Me.txtLooseCases.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLooseCases.ForeColor = System.Drawing.Color.White
        Me.txtLooseCases.Location = New System.Drawing.Point(730, 859)
        Me.txtLooseCases.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.txtLooseCases.MaxLength = 6
        Me.txtLooseCases.Name = "txtLooseCases"
        Me.txtLooseCases.Size = New System.Drawing.Size(154, 56)
        Me.txtLooseCases.TabIndex = 36
        Me.txtLooseCases.UseWaitCursor = True
        Me.txtLooseCases.Visible = False
        '
        'lblItemDesc
        '
        Me.lblItemDesc.AutoSize = True
        Me.lblItemDesc.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemDesc.ForeColor = System.Drawing.Color.White
        Me.lblItemDesc.Location = New System.Drawing.Point(720, 153)
        Me.lblItemDesc.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblItemDesc.Name = "lblItemDesc"
        Me.lblItemDesc.Size = New System.Drawing.Size(126, 47)
        Me.lblItemDesc.TabIndex = 5
        Me.lblItemDesc.Text = "Desc."
        Me.lblItemDesc.UseWaitCursor = True
        '
        'btnStopShopOrder
        '
        Me.btnStopShopOrder.BackColor = System.Drawing.Color.Silver
        Me.btnStopShopOrder.Font = New System.Drawing.Font("Arial", 17.0!, System.Drawing.FontStyle.Bold)
        Me.btnStopShopOrder.Location = New System.Drawing.Point(385, 932)
        Me.btnStopShopOrder.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnStopShopOrder.Name = "btnStopShopOrder"
        Me.btnStopShopOrder.Size = New System.Drawing.Size(275, 120)
        Me.btnStopShopOrder.TabIndex = 38
        Me.btnStopShopOrder.Text = "Stop Shop Order"
        Me.btnStopShopOrder.UseVisualStyleBackColor = False
        Me.btnStopShopOrder.UseWaitCursor = True
        '
        'btnPrvScn
        '
        Me.btnPrvScn.BackColor = System.Drawing.Color.Silver
        Me.btnPrvScn.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrvScn.Location = New System.Drawing.Point(50, 932)
        Me.btnPrvScn.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnPrvScn.Name = "btnPrvScn"
        Me.btnPrvScn.Size = New System.Drawing.Size(275, 120)
        Me.btnPrvScn.TabIndex = 37
        Me.btnPrvScn.Text = "Previous Screen"
        Me.btnPrvScn.UseVisualStyleBackColor = False
        Me.btnPrvScn.UseWaitCursor = True
        '
        'btnPalletFull
        '
        Me.btnPalletFull.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnPalletFull.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPalletFull.Location = New System.Drawing.Point(438, 735)
        Me.btnPalletFull.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnPalletFull.Name = "btnPalletFull"
        Me.btnPalletFull.Size = New System.Drawing.Size(110, 110)
        Me.btnPalletFull.TabIndex = 31
        Me.btnPalletFull.Text = "Yes"
        Me.btnPalletFull.UseVisualStyleBackColor = False
        Me.btnPalletFull.UseWaitCursor = True
        '
        'btnPalletNotFull
        '
        Me.btnPalletNotFull.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnPalletNotFull.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPalletNotFull.Location = New System.Drawing.Point(590, 735)
        Me.btnPalletNotFull.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnPalletNotFull.Name = "btnPalletNotFull"
        Me.btnPalletNotFull.Size = New System.Drawing.Size(110, 110)
        Me.btnPalletNotFull.TabIndex = 32
        Me.btnPalletNotFull.Text = "No"
        Me.btnPalletNotFull.UseVisualStyleBackColor = False
        Me.btnPalletNotFull.UseWaitCursor = True
        '
        'btnCloseSO
        '
        Me.btnCloseSO.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnCloseSO.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCloseSO.Location = New System.Drawing.Point(1144, 593)
        Me.btnCloseSO.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnCloseSO.Name = "btnCloseSO"
        Me.btnCloseSO.Size = New System.Drawing.Size(110, 110)
        Me.btnCloseSO.TabIndex = 28
        Me.btnCloseSO.Text = "Yes"
        Me.btnCloseSO.UseVisualStyleBackColor = False
        Me.btnCloseSO.UseWaitCursor = True
        '
        'btnNoLogScraps
        '
        Me.btnNoLogScraps.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnNoLogScraps.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNoLogScraps.Location = New System.Drawing.Point(588, 455)
        Me.btnNoLogScraps.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnNoLogScraps.Name = "btnNoLogScraps"
        Me.btnNoLogScraps.Size = New System.Drawing.Size(110, 110)
        Me.btnNoLogScraps.TabIndex = 26
        Me.btnNoLogScraps.Text = "No"
        Me.btnNoLogScraps.UseVisualStyleBackColor = False
        Me.btnNoLogScraps.UseWaitCursor = True
        '
        'btnNoCloseSO
        '
        Me.btnNoCloseSO.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnNoCloseSO.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNoCloseSO.Location = New System.Drawing.Point(1296, 593)
        Me.btnNoCloseSO.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnNoCloseSO.Name = "btnNoCloseSO"
        Me.btnNoCloseSO.Size = New System.Drawing.Size(110, 110)
        Me.btnNoCloseSO.TabIndex = 29
        Me.btnNoCloseSO.Text = "No"
        Me.btnNoCloseSO.UseVisualStyleBackColor = False
        Me.btnNoCloseSO.UseWaitCursor = True
        '
        'btnLogScraps
        '
        Me.btnLogScraps.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnLogScraps.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogScraps.Location = New System.Drawing.Point(438, 455)
        Me.btnLogScraps.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnLogScraps.Name = "btnLogScraps"
        Me.btnLogScraps.Size = New System.Drawing.Size(110, 110)
        Me.btnLogScraps.TabIndex = 25
        Me.btnLogScraps.Text = "Yes"
        Me.btnLogScraps.UseVisualStyleBackColor = False
        Me.btnLogScraps.UseWaitCursor = True
        '
        'lblCasesRemainInShift
        '
        Me.lblCasesRemainInShift.AutoSize = True
        Me.lblCasesRemainInShift.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCasesRemainInShift.ForeColor = System.Drawing.Color.White
        Me.lblCasesRemainInShift.Location = New System.Drawing.Point(490, 384)
        Me.lblCasesRemainInShift.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblCasesRemainInShift.Name = "lblCasesRemainInShift"
        Me.lblCasesRemainInShift.Size = New System.Drawing.Size(43, 47)
        Me.lblCasesRemainInShift.TabIndex = 21
        Me.lblCasesRemainInShift.Text = "0"
        Me.lblCasesRemainInShift.UseWaitCursor = True
        '
        'lblRemain
        '
        Me.lblRemain.AutoSize = True
        Me.lblRemain.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRemain.ForeColor = System.Drawing.Color.White
        Me.lblRemain.Location = New System.Drawing.Point(24, 384)
        Me.lblRemain.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblRemain.Name = "lblRemain"
        Me.lblRemain.Size = New System.Drawing.Size(177, 47)
        Me.lblRemain.TabIndex = 19
        Me.lblRemain.Text = "Remain:"
        Me.lblRemain.UseWaitCursor = True
        '
        'lblFullPalletQty
        '
        Me.lblFullPalletQty.AutoSize = True
        Me.lblFullPalletQty.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFullPalletQty.ForeColor = System.Drawing.Color.White
        Me.lblFullPalletQty.Location = New System.Drawing.Point(739, 767)
        Me.lblFullPalletQty.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblFullPalletQty.Name = "lblFullPalletQty"
        Me.lblFullPalletQty.Size = New System.Drawing.Size(297, 47)
        Me.lblFullPalletQty.TabIndex = 33
        Me.lblFullPalletQty.Text = "Full Pallet Qty:"
        Me.lblFullPalletQty.UseWaitCursor = True
        '
        'lblCloseSO
        '
        Me.lblCloseSO.AutoSize = True
        Me.lblCloseSO.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCloseSO.ForeColor = System.Drawing.Color.White
        Me.lblCloseSO.Location = New System.Drawing.Point(739, 625)
        Me.lblCloseSO.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblCloseSO.Name = "lblCloseSO"
        Me.lblCloseSO.Size = New System.Drawing.Size(377, 47)
        Me.lblCloseSO.TabIndex = 27
        Me.lblCloseSO.Text = "Close Shop Order?"
        Me.lblCloseSO.UseWaitCursor = True
        '
        'lblQtyPerPallet
        '
        Me.lblQtyPerPallet.AutoSize = True
        Me.lblQtyPerPallet.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblQtyPerPallet.ForeColor = System.Drawing.Color.White
        Me.lblQtyPerPallet.Location = New System.Drawing.Point(1067, 767)
        Me.lblQtyPerPallet.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblQtyPerPallet.Name = "lblQtyPerPallet"
        Me.lblQtyPerPallet.Size = New System.Drawing.Size(43, 47)
        Me.lblQtyPerPallet.TabIndex = 34
        Me.lblQtyPerPallet.Text = "0"
        Me.lblQtyPerPallet.UseWaitCursor = True
        '
        'lblTotalRemain
        '
        Me.lblTotalRemain.AutoSize = True
        Me.lblTotalRemain.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalRemain.ForeColor = System.Drawing.Color.White
        Me.lblTotalRemain.Location = New System.Drawing.Point(271, 384)
        Me.lblTotalRemain.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblTotalRemain.Name = "lblTotalRemain"
        Me.lblTotalRemain.Size = New System.Drawing.Size(43, 47)
        Me.lblTotalRemain.TabIndex = 20
        Me.lblTotalRemain.Text = "0"
        Me.lblTotalRemain.UseWaitCursor = True
        '
        'lblTotalScheduled
        '
        Me.lblTotalScheduled.AutoSize = True
        Me.lblTotalScheduled.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalScheduled.ForeColor = System.Drawing.Color.White
        Me.lblTotalScheduled.Location = New System.Drawing.Point(271, 241)
        Me.lblTotalScheduled.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblTotalScheduled.Name = "lblTotalScheduled"
        Me.lblTotalScheduled.Size = New System.Drawing.Size(43, 47)
        Me.lblTotalScheduled.TabIndex = 10
        Me.lblTotalScheduled.Text = "0"
        Me.lblTotalScheduled.UseWaitCursor = True
        '
        'lblTotalProduced
        '
        Me.lblTotalProduced.AutoSize = True
        Me.lblTotalProduced.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalProduced.ForeColor = System.Drawing.Color.White
        Me.lblTotalProduced.Location = New System.Drawing.Point(271, 310)
        Me.lblTotalProduced.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblTotalProduced.Name = "lblTotalProduced"
        Me.lblTotalProduced.Size = New System.Drawing.Size(43, 47)
        Me.lblTotalProduced.TabIndex = 15
        Me.lblTotalProduced.Text = "0"
        Me.lblTotalProduced.UseWaitCursor = True
        '
        'lblCases
        '
        Me.lblCases.AutoSize = True
        Me.lblCases.BackColor = System.Drawing.Color.RoyalBlue
        Me.lblCases.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Italic Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCases.ForeColor = System.Drawing.Color.White
        Me.lblCases.Location = New System.Drawing.Point(20, 193)
        Me.lblCases.Margin = New System.Windows.Forms.Padding(0)
        Me.lblCases.Name = "lblCases"
        Me.lblCases.Size = New System.Drawing.Size(262, 39)
        Me.lblCases.TabIndex = 6
        Me.lblCases.Text = "(Cases)              "
        Me.lblCases.UseWaitCursor = True
        '
        'lblOrderTotal
        '
        Me.lblOrderTotal.AutoSize = True
        Me.lblOrderTotal.BackColor = System.Drawing.Color.RoyalBlue
        Me.lblOrderTotal.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Italic Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrderTotal.ForeColor = System.Drawing.Color.White
        Me.lblOrderTotal.Location = New System.Drawing.Point(273, 193)
        Me.lblOrderTotal.Margin = New System.Windows.Forms.Padding(0)
        Me.lblOrderTotal.Name = "lblOrderTotal"
        Me.lblOrderTotal.Size = New System.Drawing.Size(203, 39)
        Me.lblOrderTotal.TabIndex = 7
        Me.lblOrderTotal.Text = "Order Total  "
        Me.lblOrderTotal.UseWaitCursor = True
        '
        'lblShiftTotal
        '
        Me.lblShiftTotal.AutoSize = True
        Me.lblShiftTotal.BackColor = System.Drawing.Color.RoyalBlue
        Me.lblShiftTotal.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Italic Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShiftTotal.ForeColor = System.Drawing.Color.White
        Me.lblShiftTotal.Location = New System.Drawing.Point(490, 193)
        Me.lblShiftTotal.Margin = New System.Windows.Forms.Padding(0)
        Me.lblShiftTotal.Name = "lblShiftTotal"
        Me.lblShiftTotal.Size = New System.Drawing.Size(185, 39)
        Me.lblShiftTotal.TabIndex = 8
        Me.lblShiftTotal.Text = "Shift Total  "
        Me.lblShiftTotal.UseWaitCursor = True
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ForeColor = System.Drawing.Color.LightSalmon
        Me.lblMessage.Location = New System.Drawing.Point(40, 1056)
        Me.lblMessage.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(187, 47)
        Me.lblMessage.TabIndex = 39
        Me.lblMessage.Text = "Message"
        Me.lblMessage.UseWaitCursor = True
        '
        'lblCasesProducedInShift
        '
        Me.lblCasesProducedInShift.AutoSize = True
        Me.lblCasesProducedInShift.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCasesProducedInShift.ForeColor = System.Drawing.Color.White
        Me.lblCasesProducedInShift.Location = New System.Drawing.Point(488, 310)
        Me.lblCasesProducedInShift.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblCasesProducedInShift.Name = "lblCasesProducedInShift"
        Me.lblCasesProducedInShift.Size = New System.Drawing.Size(43, 47)
        Me.lblCasesProducedInShift.TabIndex = 16
        Me.lblCasesProducedInShift.Text = "0"
        Me.lblCasesProducedInShift.UseWaitCursor = True
        '
        'lblFromCarriedFwd
        '
        Me.lblFromCarriedFwd.AutoSize = True
        Me.lblFromCarriedFwd.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFromCarriedFwd.ForeColor = System.Drawing.Color.White
        Me.lblFromCarriedFwd.Location = New System.Drawing.Point(1116, 310)
        Me.lblFromCarriedFwd.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblFromCarriedFwd.Name = "lblFromCarriedFwd"
        Me.lblFromCarriedFwd.Size = New System.Drawing.Size(43, 47)
        Me.lblFromCarriedFwd.TabIndex = 18
        Me.lblFromCarriedFwd.Text = "0"
        Me.lblFromCarriedFwd.UseWaitCursor = True
        '
        'UcHeading1
        '
        Me.UcHeading1.AutoSize = True
        Me.UcHeading1.BackColor = System.Drawing.Color.YellowGreen
        Me.UcHeading1.Location = New System.Drawing.Point(0, 0)
        Me.UcHeading1.Margin = New System.Windows.Forms.Padding(11)
        Me.UcHeading1.Name = "UcHeading1"
        Me.UcHeading1.ScreenTitle = "-- Heading --"
        Me.UcHeading1.Size = New System.Drawing.Size(1467, 102)
        Me.UcHeading1.TabIndex = 40
        Me.UcHeading1.UseWaitCursor = True
        '
        'btnLogRejectPoints
        '
        Me.btnLogRejectPoints.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnLogRejectPoints.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogRejectPoints.Location = New System.Drawing.Point(438, 593)
        Me.btnLogRejectPoints.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnLogRejectPoints.Name = "btnLogRejectPoints"
        Me.btnLogRejectPoints.Size = New System.Drawing.Size(110, 110)
        Me.btnLogRejectPoints.TabIndex = 42
        Me.btnLogRejectPoints.Text = "Yes"
        Me.btnLogRejectPoints.UseVisualStyleBackColor = False
        Me.btnLogRejectPoints.UseWaitCursor = True
        '
        'btnNoLogRejectPoints
        '
        Me.btnNoLogRejectPoints.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnNoLogRejectPoints.Font = New System.Drawing.Font("Arial", 17.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNoLogRejectPoints.Location = New System.Drawing.Point(588, 593)
        Me.btnNoLogRejectPoints.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.btnNoLogRejectPoints.Name = "btnNoLogRejectPoints"
        Me.btnNoLogRejectPoints.Size = New System.Drawing.Size(110, 110)
        Me.btnNoLogRejectPoints.TabIndex = 43
        Me.btnNoLogRejectPoints.Text = "No"
        Me.btnNoLogRejectPoints.UseVisualStyleBackColor = False
        Me.btnNoLogRejectPoints.UseWaitCursor = True
        '
        'lblLogRejectPoints
        '
        Me.lblLogRejectPoints.AutoSize = True
        Me.lblLogRejectPoints.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLogRejectPoints.ForeColor = System.Drawing.Color.White
        Me.lblLogRejectPoints.Location = New System.Drawing.Point(22, 625)
        Me.lblLogRejectPoints.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblLogRejectPoints.Name = "lblLogRejectPoints"
        Me.lblLogRejectPoints.Size = New System.Drawing.Size(371, 47)
        Me.lblLogRejectPoints.TabIndex = 41
        Me.lblLogRejectPoints.Text = "Log Reject Points?"
        Me.lblLogRejectPoints.UseWaitCursor = True
        '
        'frmStopShopOrder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.CornflowerBlue
        Me.ClientSize = New System.Drawing.Size(1467, 1108)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnLogRejectPoints)
        Me.Controls.Add(Me.btnNoLogRejectPoints)
        Me.Controls.Add(Me.lblLogRejectPoints)
        Me.Controls.Add(Me.lblFromCarriedFwd)
        Me.Controls.Add(Me.lblCasesProducedInShift)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.lblShiftTotal)
        Me.Controls.Add(Me.lblOrderTotal)
        Me.Controls.Add(Me.lblCases)
        Me.Controls.Add(Me.lblTotalProduced)
        Me.Controls.Add(Me.lblTotalScheduled)
        Me.Controls.Add(Me.lblTotalRemain)
        Me.Controls.Add(Me.lblQtyPerPallet)
        Me.Controls.Add(Me.lblCloseSO)
        Me.Controls.Add(Me.lblFullPalletQty)
        Me.Controls.Add(Me.lblRemain)
        Me.Controls.Add(Me.lblCasesRemainInShift)
        Me.Controls.Add(Me.btnLogScraps)
        Me.Controls.Add(Me.btnNoCloseSO)
        Me.Controls.Add(Me.btnNoLogScraps)
        Me.Controls.Add(Me.btnCloseSO)
        Me.Controls.Add(Me.btnPalletNotFull)
        Me.Controls.Add(Me.btnPalletFull)
        Me.Controls.Add(Me.btnPrvScn)
        Me.Controls.Add(Me.btnStopShopOrder)
        Me.Controls.Add(Me.lblItemDesc)
        Me.Controls.Add(Me.lblItemNo)
        Me.Controls.Add(Me.lblSKUNumber)
        Me.Controls.Add(Me.txtLooseCases)
        Me.Controls.Add(Me.txtRework)
        Me.Controls.Add(Me.txtBagLengthUsed)
        Me.Controls.Add(Me.lblLogScraps)
        Me.Controls.Add(Me.lblIsLastPallet)
        Me.Controls.Add(Me.lblLooseCases)
        Me.Controls.Add(Me.lblRework)
        Me.Controls.Add(Me.lblBagLengthUsed)
        Me.Controls.Add(Me.lblCaseScheduledInShift)
        Me.Controls.Add(Me.lblScheduled)
        Me.Controls.Add(Me.lblFCarriedFwd)
        Me.Controls.Add(Me.lblProduced)
        Me.Controls.Add(Me.lblShopOrder)
        Me.Controls.Add(Me.lblSO)
        Me.Controls.Add(Me.UcHeading1)
        Me.Controls.Add(Me.lblScreenTitle)
        Me.Controls.Add(Me.lblTime)
        Me.Controls.Add(Me.lblDate)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(6)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmStopShopOrder"
        Me.Text = "Stop Shop Order"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblScreenTitle As System.Windows.Forms.Label
    Friend WithEvents lblTime As System.Windows.Forms.Label
    Friend WithEvents lblDate As System.Windows.Forms.Label
    Friend WithEvents UcHeading1 As PowerPlant.ucHeading
    Friend WithEvents lblSO As System.Windows.Forms.Label
    Friend WithEvents lblItemNo As System.Windows.Forms.Label
    Friend WithEvents lblSKUNumber As System.Windows.Forms.Label
    Friend WithEvents lblShopOrder As System.Windows.Forms.Label
    Friend WithEvents lblProduced As System.Windows.Forms.Label
    Friend WithEvents lblFCarriedFwd As System.Windows.Forms.Label
    Friend WithEvents lblScheduled As System.Windows.Forms.Label
    Friend WithEvents lblCaseScheduledInShift As System.Windows.Forms.Label
    Friend WithEvents lblBagLengthUsed As System.Windows.Forms.Label
    Friend WithEvents lblRework As System.Windows.Forms.Label
    Friend WithEvents lblLooseCases As System.Windows.Forms.Label
    Friend WithEvents lblIsLastPallet As System.Windows.Forms.Label
    Friend WithEvents lblLogScraps As System.Windows.Forms.Label
    Friend WithEvents txtBagLengthUsed As System.Windows.Forms.TextBox
    Friend WithEvents txtRework As System.Windows.Forms.TextBox
    Friend WithEvents txtLooseCases As System.Windows.Forms.TextBox
    Friend WithEvents lblItemDesc As System.Windows.Forms.Label
    Friend WithEvents btnStopShopOrder As System.Windows.Forms.Button
    Friend WithEvents btnPrvScn As System.Windows.Forms.Button
    Friend WithEvents btnPalletFull As System.Windows.Forms.Button
    Friend WithEvents btnPalletNotFull As System.Windows.Forms.Button
    Friend WithEvents btnCloseSO As System.Windows.Forms.Button
    Friend WithEvents btnNoLogScraps As System.Windows.Forms.Button
    Friend WithEvents btnNoCloseSO As System.Windows.Forms.Button
    Friend WithEvents btnLogScraps As System.Windows.Forms.Button
    Friend WithEvents lblCasesRemainInShift As System.Windows.Forms.Label
    Friend WithEvents lblRemain As System.Windows.Forms.Label
    Friend WithEvents lblFullPalletQty As System.Windows.Forms.Label
    Friend WithEvents lblCloseSO As System.Windows.Forms.Label
    Friend WithEvents lblQtyPerPallet As System.Windows.Forms.Label
    Friend WithEvents lblTotalRemain As System.Windows.Forms.Label
    Friend WithEvents lblTotalScheduled As System.Windows.Forms.Label
    Friend WithEvents lblTotalProduced As System.Windows.Forms.Label
    Friend WithEvents lblCases As System.Windows.Forms.Label
    Friend WithEvents lblOrderTotal As System.Windows.Forms.Label
    Friend WithEvents lblShiftTotal As System.Windows.Forms.Label
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents lblCasesProducedInShift As System.Windows.Forms.Label
    Friend WithEvents lblFromCarriedFwd As System.Windows.Forms.Label
    Friend WithEvents btnLogRejectPoints As Button
    Friend WithEvents btnNoLogRejectPoints As Button
    Friend WithEvents lblLogRejectPoints As Label
End Class
