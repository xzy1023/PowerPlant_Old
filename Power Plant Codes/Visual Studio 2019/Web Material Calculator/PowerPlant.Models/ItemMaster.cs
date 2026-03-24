using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PowerPlant.Models
{
    public partial class ItemMaster
    {
        public string Facility { get; set; }
        public string ItemNumber { get; set; }
        public int ProductionShelfLifeDays { get; set; }
        public decimal LabelWeight { get; set; }
        public string LabelWeightUom { get; set; }
        public string BagLengthRequired { get; set; }
        public decimal BagLength { get; set; }
        public string LabelDateFmtCode { get; set; }
        public decimal PackagesPerSaleableUnit { get; set; }
        public decimal SaleableUnitPerCase { get; set; }
        public int QtyPerPallet { get; set; }
        public string Scccode { get; set; }
        public string Upccode { get; set; }
        public string OverrideItem { get; set; }
        public string ItemDesc1 { get; set; }
        public string ItemDesc2 { get; set; }
        public string ItemDesc3 { get; set; }
        public string PackSize { get; set; }
        public string NetWeight { get; set; }
        public string DomicileText1 { get; set; }
        public string DomicileText2 { get; set; }
        public string DomicileText3 { get; set; }
        public string DomicileText4 { get; set; }
        public string DomicileText5 { get; set; }
        public string DomicileText6 { get; set; }
        public string CaseLabelFmt1 { get; set; }
        public string CaseLabelFmt2 { get; set; }
        public string CaseLabelFmt3 { get; set; }
        public string PackageCoderFmt1 { get; set; }
        public string PackageCoderFmt2 { get; set; }
        public string PackageCoderFmt3 { get; set; }
        public string FilterCoderFmt { get; set; }
        public string ProductionDateDesc { get; set; }
        public string ExpiryDateDesc { get; set; }
        public string PrintSolot { get; set; }
        public string DateToPrintFlag { get; set; }
        public string PrintCaseLabel { get; set; }
        public int Tie { get; set; }
        public int Tier { get; set; }
        public int? ShipShelfLifeDays { get; set; }
        public string ItemType { get; set; }
        public string ItemMajorClass { get; set; }
        public string PalletCode { get; set; }
        public bool? SlipSheet { get; set; }
        public string PkgLabelDateFmtCode { get; set; }
        public decimal StdCostPerLb { get; set; }
        public decimal? GrsDepth { get; set; }
        public decimal? GrsHeight { get; set; }
        public decimal? GrsWidth { get; set; }
        public byte? CaseLabelApplicator { get; set; }
        public bool? InsertBrewerFilter { get; set; }


        public WebMaterial WebMaterial { get; set; }
    }
}
