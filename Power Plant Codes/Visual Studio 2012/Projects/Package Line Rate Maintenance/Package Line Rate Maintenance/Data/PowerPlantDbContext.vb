Imports System
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity
Imports System.Linq

Partial Public Class PowerPlantDbContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=PowerPlantCnnStr")
    End Sub

    Public Overridable Property Equipments As DbSet(Of Equipment)
    Public Overridable Property EquipmentGroups As DbSet(Of EquipmentGroup)
    Public Overridable Property Facilities As DbSet(Of FacilityEntity)
    Public Overridable Property OverrideMachineRates As DbSet(Of OverrideMachineRate)
    Public Overridable Property StdMachineRates As DbSet(Of StdMachineRate)
    Public Overridable Property ControlEntity As DbSet(Of ControlEntity)
    Public Overridable Property ItemMasters As DbSet(Of ItemMaster)

    Protected Overrides Sub OnModelCreating(ByVal modelBuilder As DbModelBuilder)
        modelBuilder.Entity(Of Equipment)() _
            .Property(Function(e) e.facility) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of Equipment)() _
            .Property(Function(e) e.EquipmentID) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of Equipment)() _
            .Property(Function(e) e.Type) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of Equipment)() _
            .Property(Function(e) e.SubType) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of Equipment)() _
            .Property(Function(e) e.Description) _
            .IsUnicode(False)

        modelBuilder.Entity(Of Equipment)() _
            .Property(Function(e) e.GroupID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of Equipment)() _
            .Property(Function(e) e.ProbatID) _
            .IsUnicode(False)

        modelBuilder.Entity(Of Equipment)() _
            .Property(Function(e) e.WorkCenter) _
            .IsUnicode(False)

        modelBuilder.Entity(Of EquipmentGroup)() _
            .Property(Function(e) e.Facility) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of EquipmentGroup)() _
            .Property(Function(e) e.EquipmentID) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of EquipmentGroup)() _
            .Property(Function(e) e.GroupID) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of FacilityEntity)() _
            .Property(Function(e) e.Facility) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of FacilityEntity)() _
            .Property(Function(e) e.Region) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of FacilityEntity)() _
            .Property(Function(e) e.Description) _
            .IsUnicode(False)

        modelBuilder.Entity(Of FacilityEntity)() _
            .Property(Function(e) e.ShortDescription) _
            .IsUnicode(False)

        modelBuilder.Entity(Of OverrideMachineRate)() _
            .Property(Function(e) e.Facility) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of OverrideMachineRate)() _
            .Property(Function(e) e.MachineID) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of OverrideMachineRate)() _
            .Property(Function(e) e.RateMultiplier) _
            .HasPrecision(7, 4)

        modelBuilder.Entity(Of OverrideMachineRate)() _
            .Property(Function(e) e.LogicForRateMultiplier) _
            .IsUnicode(False)

        modelBuilder.Entity(Of OverrideMachineRate)() _
            .Property(Function(e) e.CreatedBy) _
            .IsUnicode(False)

        modelBuilder.Entity(Of OverrideMachineRate)() _
            .Property(Function(e) e.ItemNumber) _
            .IsUnicode(False)

        modelBuilder.Entity(Of StdMachineRate)() _
            .Property(Function(e) e.Facility) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of StdMachineRate)() _
            .Property(Function(e) e.ItemNumber) _
            .IsUnicode(False)

        modelBuilder.Entity(Of StdMachineRate)() _
            .Property(Function(e) e.MachineID) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of StdMachineRate)() _
            .Property(Function(e) e.MachineHours) _
            .HasPrecision(8, 3)

        modelBuilder.Entity(Of StdMachineRate)() _
            .Property(Function(e) e.BasisCode) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of StdMachineRate)() _
            .Property(Function(e) e.StdWorkCenterEfficiency) _
            .HasPrecision(5, 4)

        modelBuilder.Entity(Of StdMachineRate)() _
            .Property(Function(e) e.MachineHoursOriginal) _
            .HasPrecision(8, 3)

        modelBuilder.Entity(Of ControlEntity)() _
            .Property(Function(e) e.Facility) _
            .IsFixedLength()

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.Facility) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.ItemNumber) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.LabelWeight) _
            .HasPrecision(10, 3)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.LabelWeightUOM) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.BagLengthRequired) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.BagLength) _
            .HasPrecision(4, 2)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.LabelDateFmtCode) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.PackagesPerSaleableUnit) _
            .HasPrecision(5, 0)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.SaleableUnitPerCase) _
            .HasPrecision(5, 0)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.SCCCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.UPCCode) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.OverrideItem) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.ItemDesc1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.ItemDesc2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.ItemDesc3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.PackSize) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.NetWeight) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.DomicileText1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.DomicileText2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.DomicileText3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.DomicileText4) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.DomicileText5) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.DomicileText6) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.CaseLabelFmt1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.CaseLabelFmt2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.CaseLabelFmt3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.PackageCoderFmt1) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.PackageCoderFmt2) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.PackageCoderFmt3) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.FilterCoderFmt) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.ProductionDateDesc) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.ExpiryDateDesc) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.PrintSOLot) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.DateToPrintFlag) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.PrintCaseLabel) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.ItemType) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.ItemMajorClass) _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.PalletCode) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.PkgLabelDateFmtCode) _
            .IsFixedLength() _
            .IsUnicode(False)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.StdCostPerLB) _
            .HasPrecision(15, 5)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.GrsDepth) _
            .HasPrecision(16, 8)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.GrsHeight) _
            .HasPrecision(16, 8)

        modelBuilder.Entity(Of ItemMaster)() _
            .Property(Function(e) e.GrsWidth) _
            .HasPrecision(16, 8)

    End Sub
End Class
