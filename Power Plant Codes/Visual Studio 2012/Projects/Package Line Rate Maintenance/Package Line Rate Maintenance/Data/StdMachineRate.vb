Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("tblStdMachineEfficiencyRateHst")>
Partial Public Class StdMachineRate
    <Key>
    <Column(Order:=0)>
    <StringLength(3)>
    Public Property Facility As String

    <Key>
    <Column(Order:=1)>
    <StringLength(50)>
    Public Property ItemNumber As String

    <Key>
    <Column(Order:=2)>
    <DatabaseGenerated(DatabaseGeneratedOption.None)>
    Public Property WorkCenter As Integer

    <Key>
    <Column(Order:=3)>
    <StringLength(10)>
    Public Property MachineID As String

    Public Property MachineHours As Decimal

    <Required>
    <StringLength(1)>
    Public Property BasisCode As String

    Public Property StdWorkCenterEfficiency As Decimal

    Public Property RunOperators As Short?

    Public Property MachineHoursOriginal As Decimal

    Public Property RunOperatorsOriginal As Short
End Class
