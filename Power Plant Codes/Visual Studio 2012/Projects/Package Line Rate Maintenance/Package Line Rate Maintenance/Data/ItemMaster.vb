Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("tblItemMaster")>
Partial Public Class ItemMaster
    <Required>
    <StringLength(3)>
    Public Property Facility As String

    <Key>
    <StringLength(35)>
    Public Property ItemNumber As String

    Public Property ProductionShelfLifeDays As Integer

    Public Property LabelWeight As Decimal

    <Required>
    <StringLength(2)>
    Public Property LabelWeightUOM As String

    <Required>
    <StringLength(1)>
    Public Property BagLengthRequired As String

    Public Property BagLength As Decimal

    <Required>
    <StringLength(2)>
    Public Property LabelDateFmtCode As String

    Public Property PackagesPerSaleableUnit As Decimal

    Public Property SaleableUnitPerCase As Decimal

    Public Property QtyPerPallet As Integer

    <Required>
    <StringLength(14)>
    Public Property SCCCode As String

    <Required>
    <StringLength(14)>
    Public Property UPCCode As String

    <Required>
    <StringLength(35)>
    Public Property OverrideItem As String

    <Required>
    <StringLength(50)>
    Public Property ItemDesc1 As String

    <Required>
    <StringLength(50)>
    Public Property ItemDesc2 As String

    <StringLength(50)>
    Public Property ItemDesc3 As String

    <Required>
    <StringLength(12)>
    Public Property PackSize As String

    <Required>
    <StringLength(10)>
    Public Property NetWeight As String

    <Required>
    <StringLength(24)>
    Public Property DomicileText1 As String

    <Required>
    <StringLength(24)>
    Public Property DomicileText2 As String

    <Required>
    <StringLength(24)>
    Public Property DomicileText3 As String

    <Required>
    <StringLength(24)>
    Public Property DomicileText4 As String

    <Required>
    <StringLength(24)>
    Public Property DomicileText5 As String

    <Required>
    <StringLength(24)>
    Public Property DomicileText6 As String

    <Required>
    <StringLength(25)>
    Public Property CaseLabelFmt1 As String

    <Required>
    <StringLength(25)>
    Public Property CaseLabelFmt2 As String

    <Required>
    <StringLength(25)>
    Public Property CaseLabelFmt3 As String

    <Required>
    <StringLength(25)>
    Public Property PackageCoderFmt1 As String

    <Required>
    <StringLength(25)>
    Public Property PackageCoderFmt2 As String

    <Required>
    <StringLength(25)>
    Public Property PackageCoderFmt3 As String

    <Required>
    <StringLength(25)>
    Public Property FilterCoderFmt As String

    <StringLength(30)>
    Public Property ProductionDateDesc As String

    <StringLength(30)>
    Public Property ExpiryDateDesc As String

    <Required>
    <StringLength(1)>
    Public Property PrintSOLot As String

    <Required>
    <StringLength(1)>
    Public Property DateToPrintFlag As String

    <Required>
    <StringLength(1)>
    Public Property PrintCaseLabel As String

    Public Property Tie As Integer

    Public Property Tier As Integer

    Public Property ShipShelfLifeDays As Integer?

    <Required>
    <StringLength(10)>
    Public Property ItemType As String

    <Required>
    <StringLength(10)>
    Public Property ItemMajorClass As String

    <StringLength(1)>
    Public Property PalletCode As String

    Public Property SlipSheet As Boolean?

    <StringLength(2)>
    Public Property PkgLabelDateFmtCode As String

    Public Property StdCostPerLB As Decimal

    Public Property GrsDepth As Decimal?

    Public Property GrsHeight As Decimal?

    Public Property GrsWidth As Decimal?

    Public Property CaseLabelApplicator As Byte?

    Public Property InsertBrewerFilter As Boolean?
End Class
