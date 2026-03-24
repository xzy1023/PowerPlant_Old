Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("tblEquipmentGroup")>
Partial Public Class EquipmentGroup
    <Key>
    <Column(Order:=0)>
    <StringLength(3)>
    Public Property Facility As String

    <Key>
    <Column(Order:=1)>
    <StringLength(10)>
    Public Property EquipmentID As String

    <Required>
    <StringLength(10)>
    Public Property GroupID As String
End Class
