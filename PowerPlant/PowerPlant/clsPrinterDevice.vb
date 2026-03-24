Public Class PrinterDevice
    Private strFacility As String, strPackagingLine As String
    'WO#359 Private taPrintDevice As New dsPrintDeviceTableAdapters.tblPkgLinePrinterDeviceTableAdapter
    'WO#359 Private tblPrintDevice As New dsPrintDevice.tblPkgLinePrinterDeviceDataTable
    Private taPrinter As New dsPrinterDeviceTableAdapters.taPrinterDevice   'WO#359
    Private dtPrinter As New dsPrinterDevice.tblPkgLinePrinterDeviceDataTable   'WO#359

    Private blnHasCasePrinter As Boolean = False
    Private blnHasPalletPrinter As Boolean = False
    Private blnHasFilterCoderPrinter As Boolean = False
    Private blnHasPackageCoderPrinter As Boolean = False
    Private blnHasPrinter As Boolean = False

    'Constructor
    Public Sub New()
    End Sub
    'Constructor
    Public Sub New(ByVal Facility As String, ByVal PackagingLine As String)
        Dim drDeviceType As DataRow
        'WO#359 taPrintDevice.Connection.ConnectionString = gstrServerConnectionString
        'WO#359 taPrintDevice.Fill(tblPrintDevice, Facility, PackagingLine)
        'WO#359 If tblPrintDevice.Rows.Count > 0 Then
        'WO#359    For Each drDeviceType In tblPrintDevice
        taPrinter.Fill(dtPrinter, "ListAll", Facility, PackagingLine) 'WO#359
        If dtPrinter.Rows.Count > 0 Then    'WO#359
            For Each drDeviceType In dtPrinter  'WO#359
                Select Case drDeviceType("DeviceType")
                    Case "C"
                        blnHasCasePrinter = True
                    Case "P"
                        blnHasPalletPrinter = True
                    Case "F"
                        blnHasFilterCoderPrinter = True
                    Case "X"
                        blnHasPackageCoderPrinter = True
                End Select
            Next
            If blnHasCasePrinter Or blnHasPalletPrinter Or blnHasFilterCoderPrinter Or blnHasPackageCoderPrinter Then
                blnHasPrinter = True
            End If
        End If
    End Sub
    Public ReadOnly Property HasCasePrinter() As Boolean
        Get
            Return blnHasCasePrinter
        End Get
    End Property
    Public ReadOnly Property HasPalletPrinter() As Boolean
        Get
            Return blnHasPalletPrinter
        End Get
    End Property
    Public ReadOnly Property HasFilterCoderPrinter() As Boolean
        Get
            Return blnHasFilterCoderPrinter
        End Get
    End Property
    Public ReadOnly Property HasPackageCoderPrinter() As Boolean
        Get
            Return blnHasPackageCoderPrinter
        End Get
    End Property
    Public ReadOnly Property HasPrinter() As Boolean
        Get
            Return blnHasPrinter
        End Get
    End Property
End Class
