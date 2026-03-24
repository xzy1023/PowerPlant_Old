Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("tblEquipment")>
Partial Public Class Equipment

    Public Property Active As Boolean


    <Key>
    <Column(Order:=0)>
    <StringLength(3)>
    Public Property facility As String

    <Key>
    <Column(Order:=1)>
    <StringLength(10)>
    Public Property EquipmentID As String

    <Key>
    <Column(Order:=2)>
    <StringLength(1)>
    Public Property Type As String

    <StringLength(1)>
    Public Property SubType As String

    <Required>
    <StringLength(30)>
    Public Property Description As String

    <StringLength(10)>
    Public Property GroupID As String

    <StringLength(10)>
    Public Property ProbatID As String

    Public Property IPCSharedGroup As Byte?

    <StringLength(10)>
    Public Property WorkCenter As String

    Public Property EnableDownTimeDuration As Boolean
End Class
