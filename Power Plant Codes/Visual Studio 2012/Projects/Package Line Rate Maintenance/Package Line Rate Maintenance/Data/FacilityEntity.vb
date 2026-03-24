Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("tblFacility")>
Partial Public Class FacilityEntity
    <Key>
    <StringLength(3)>
    Public Property Facility As String

    <Required>
    <StringLength(3)>
    Public Property Region As String

    <Required>
    <StringLength(30)>
    Public Property Description As String

    <Required>
    <StringLength(10)>
    Public Property ShortDescription As String
End Class
