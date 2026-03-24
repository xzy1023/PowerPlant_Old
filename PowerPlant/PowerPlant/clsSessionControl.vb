'Public Class clsSessionControl
'    Private Facility As String
'    Private ComputerName As String
'    Private StartTime As Date
'    Private StopTime As Date
'    Private DefaultPkgLine As String
'    Private OverridePkgLine As String
'    Private ShopOrder As Integer
'    Private ItemNumber As String
'    Private [Operator] As String
'    Private LogOnTime As Date
'    Private DefaultShiftNo As String
'    Private OverrideShiftNo As String
'    Private CasesScheduled As Integer
'    Private CasesProduced As Integer
'    Private PalletsCreated As Integer
'    Private BagLengthUsed As Decimal
'    Private ReworkWgt As Decimal
'    Private LooseCases As Integer
'    Private OperatorName As String
'    Private ProductionDate As Date
'    Private ItemDesc As String
'    Private ServerCnnIsOk As Boolean
'    Private CarriedForwardCases As Integer
'    Private ShiftProductionDate

'    'Constructor
'    Public Sub New()
'        Dim tblSessCtl As New dsSessionControl.CPPsp_SessionControlIODataTable
'        gtaSessCtl.Fill(tblSessCtl, "SelectAllFields")
'        If Not IsNothing(tblSessCtl) And tblSessCtl.Rows.Count > 0 Then
'            With tblSessCtl.Rows(0)
'                For Each col As DataColumn In tblSessCtl.Columns
'                    col.ColumnName = tblSessCtl.Rows(0).Item(col.ColumnName)
'                Next
'            End With
'        End If
'    End Sub
'    Public Sub New(ByVal row As dsSessionControl.CPPsp_SessionControlIORow)
'        With row
'            For Each col As DataColumn In row.Table.Columns
'                col.ColumnName = row.Item(col.ColumnName)
'            Next
'        End With
'    End Sub

'    Public Sub LogOn_update()

'    End Sub
'End Class
