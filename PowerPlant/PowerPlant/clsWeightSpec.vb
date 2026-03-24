'Public Class clsWeightSpec
'    Private sngTargetWeight As Single
'    Private sngMaxWeight As Single
'    Private sngMinWeight As Single
'    Private sngLegalMinWgt As Single
'    Private drSO As DataRow
'    Private sngLabelWeight As Single
'    Private blnOverfillProduct As Boolean = False
'    Private blnWholeBeanProduct As Boolean = False
'    Private taOverFill As New dsOverFillTableAdapters.tblOverFillTableAdapter
'    Private tblOverFill As New dsOverFill.tblOverFillDataTable
'    'Constructor
'    Public Sub New(ByVal drShopOrder As DataRow)
'        drSO = drShopOrder
'        sngLabelWeight = drSO("LabelWeight")
'        taOverFill.Fill(tblOverFill, drSO("ItemNumber"))
'        If tblOverFill.Rows.Count > 0 Then
'            blnOverfillProduct = True

'        End If
'        If drSO("Blend") <> "" And drSO("Grid") = "" Then
'            blnWholeBeanProduct = True
'        End If
'    End Sub
'    Public ReadOnly Property TargetWeight() As Single
'        Get
'            Return sngTargetWeight
'        End Get
'    End Property
'    Public ReadOnly Property MaxWeight() As Single
'        Get
'            Return sngMaxWeight
'        End Get
'    End Property
'    Public ReadOnly Property MinWeight() As Single
'        Get
'            Return sngMinWeight
'        End Get
'    End Property
'    Public ReadOnly Property LegalMinWgt() As Single
'        Get
'            Return sngLegalMinWgt
'        End Get
'    End Property
'End Class
