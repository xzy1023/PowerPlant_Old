'WO#755 ADD Start
Imports System.Xml
Imports System.IO


Public Class XMLInterface

    Private xDoc As XmlDocument
    Private xnl As XmlNodeList
    Private xnd As XmlNode
    Private strdocName As String

    Public Sub New(ByVal strCaseLabelFileName As String)
        xDoc = New XmlDocument()
        strdocName = strCaseLabelFileName
        xDoc.Load(strdocName)
    End Sub

    Public Sub StartShopOrder(ByVal strPkgLine As String, ByVal strSCC As String, ByVal strUPC As String, ByVal intShopOrder As Integer, ByVal intPackagesPerSaleableUnit As Integer, ByVal intUnitsPerCase As Integer,
                                       ByVal intCasesPerPallet As Integer, ByVal intScheduledCases As Integer, ByVal intCaseCount As Integer, ByVal strItemNumber As String,
                                       ByVal strItemDescription As String, ByVal intTie As Integer, ByVal intTier As Integer, ByVal strComputerName As String,
                                       ByVal strSOStartTime As String, ByVal strPalletCode As String, ByVal blnSlipSheet As Boolean)

        xnl = xDoc.SelectNodes("/CaseLabels/CaseLabel")
        For Each xnd In xnl
            For i As Integer = 0 To xnd.ChildNodes.Count - 1

                With xnd.ChildNodes.Item(i)
                    Select Case xnd.ChildNodes.Item(i).Name
                        Case ("SCC")
                            .InnerText = strSCC
                        Case ("UPC")
                            .InnerText = strUPC
                        Case ("ShopOrder")
                            .InnerText = intShopOrder.ToString
                        Case ("LabelKey")
                            .InnerText = strPkgLine & intShopOrder.ToString
                        Case ("PackagesPerSaleableUnit")
                            .InnerText = intPackagesPerSaleableUnit.ToString
                        Case ("UnitsPerCase")
                            .InnerText = intUnitsPerCase.ToString
                        Case ("CasesPerPallet")
                            .InnerText = intCasesPerPallet.ToString
                        Case ("ScheduledCases")
                            .InnerText = intScheduledCases.ToString
                        Case ("CaseCount")
                            .InnerText = intCaseCount.ToString
                        Case ("ItemNumber")
                            .InnerText = strItemNumber
                        Case ("ItemDescription")
                            .InnerText = strItemDescription
                        Case ("Tie")
                            .InnerText = intTie.ToString
                        Case ("Tier")
                            .InnerText = intTier.ToString
                        Case ("ComputerName")
                            .InnerText = strComputerName
                        Case ("SOStartTime")
                            .InnerText = strSOStartTime
                        Case ("ServerCnnIsOK")
                            .InnerText = "0"
                        Case ("LastUpdateTime")
                            .InnerText = Now().ToString("yyyy/MM/dd HH:mm:ss")
                        Case ("PalletType")
                            .InnerText = strPalletCode
                        Case ("SlipSheet")
                            .InnerText = IIf(blnSlipSheet, "1", "0")
                    End Select
                End With

            Next
        Next
        xDoc.Save(strdocName)

    End Sub

    'Public Sub UpdateSOInitialCaseCount(ByRef intSOInitialCaseCount As Integer)
    '    Try
    '        xnl = xDoc.SelectNodes("/CaseLabels/CaseLabel")
    '        For Each xnd In xnl
    '            For i As Integer = 0 To xnd.ChildNodes.Count - 1

    '                With xnd.ChildNodes.Item(i)
    '                    Select Case xnd.ChildNodes.Item(i).Name
    '                        Case ("CaseCount")
    '                            .InnerText = intSOInitialCaseCount
    '                        Case ("LastUpdateTime")
    '                            .InnerText = Now().ToString("yyyy/MM/dd HH:mm:ss")
    '                    End Select
    '                End With

    '            Next
    '        Next
    '        xDoc.Save(strdocName)

    '        Dim daSessCtl As New dsSessionControlTableAdapters.CPPsp_SessionControlIOTableAdapter
    '        daSessCtl.UpdateCasesProduced(intSOInitialCaseCount)

    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub


    'Public Sub StopShopOrder()
    '    xnl = xDoc.SelectNodes("/CaseLabels/CaseLabel")
    '    For Each xnd In xnl
    '        For i As Integer = 0 To xnd.ChildNodes.Count - 1

    '            With xnd.ChildNodes.Item(i)
    '                Select Case xnd.ChildNodes.Item(i).Name
    '                    Case ("ShopOrder")
    '                        .InnerText = "0"
    '                    Case ("LastUpdateTime")
    '                        .InnerText = Now().ToString("yyyy/MM/dd HH:mm:ss")
    '                End Select
    '            End With

    '        Next
    '    Next
    '    xDoc.Save(strdocName)

    'End Sub

End Class

'WO#755 ADD Stop
