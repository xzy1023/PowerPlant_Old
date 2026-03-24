Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("tblOverrideMachineEfficiencyRate")>
Partial Public Class OverrideMachineRate
    <Required>
    <StringLength(3)>
    Public Property Facility As String

    <Required>
    <StringLength(10)>
    Public Property MachineID As String

    Public Property Active As Boolean

    Public Property RateMultiplier As Decimal

    <StringLength(100)>
    Public Property LogicForRateMultiplier As String

    <StringLength(50)>
    Public Property CreatedBy As String

    Public Property CreatedAt As Date

    Public Property LastUpdated As Date

    <Required>
    <StringLength(35)>
    Public Property ItemNumber As String

    Public Property RunOperatorsMultiplier As Short

    <Key>
    Public Property RRN As Integer
End Class
