Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("tblControl")>
Partial Public Class ControlEntity
    <Required>
    <StringLength(3)>
    Public Property Facility As String

    <Key>
    <StringLength(50)>
    Public Property Key As String

    <Required>
    <StringLength(50)>
    Public Property SubKey As String

    <StringLength(255)>
    Public Property Description As String

    <StringLength(255)>
    Public Property Value1 As String

    <StringLength(255)>
    Public Property Value2 As String
End Class
