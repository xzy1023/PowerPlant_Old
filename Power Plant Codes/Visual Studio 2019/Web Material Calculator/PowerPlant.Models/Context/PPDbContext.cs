using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using PowerPlant.Models;

#nullable disable

namespace PowerPlant.Models.Context
{
    public partial class PPDbContext : DbContext
    {
        public PPDbContext()
        {
        }

        public PPDbContext(DbContextOptions<PPDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ItemMaster> ItemMasters { get; set; }
        public virtual DbSet<WebMaterial> WebMaterials { get; set; }
        public virtual DbSet<Control> Controls { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // To setup default point if sql not configured
                // optionsBuilder.UseSqlServer("Server=serverName_xxx;Database=databaseName_xxx;Trusted_Connection=True;");
            }

            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            optionsBuilder.EnableSensitiveDataLogging(true);     // to enable the sensitive data logging
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Control>(entity =>
            {
                entity.HasKey(e => new { e.Facility, e.Key, e.SubKey })
                    .HasName("PK_tblControl_1");

                entity.ToTable("tblControl");

                entity.Property(e => e.Facility)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Key)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubKey)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Value1)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Value2)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ItemMaster>(entity =>
            {
                entity.HasKey(e => new { e.Facility, e.ItemNumber });

                entity.ToTable("tblItemMaster");

                entity.Property(e => e.Facility)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ItemNumber)
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.BagLength).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.BagLengthRequired)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("\"Y\" - Required; \"N\"- Not required so the Bag Length can be zero");

                entity.Property(e => e.CaseLabelFmt1)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CaseLabelFmt2)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CaseLabelFmt3)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.DateToPrintFlag)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("'0' = None; '1'= Print Expiry Date; '2'= Print Production Date; '3'= Print Expiry & Prodcution Date");

                entity.Property(e => e.DomicileText1)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.DomicileText2)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.DomicileText3)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.DomicileText4)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.DomicileText5)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.DomicileText6)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.ExpiryDateDesc)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.FilterCoderFmt)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.GrsDepth).HasColumnType("decimal(16, 8)");

                entity.Property(e => e.GrsHeight).HasColumnType("decimal(16, 8)");

                entity.Property(e => e.GrsWidth).HasColumnType("decimal(16, 8)");

                entity.Property(e => e.ItemDesc1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ItemDesc2)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ItemDesc3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ItemMajorClass)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ItemType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LabelDateFmtCode)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("Code Dating - I$CDC in IIME$ (Date format code for Labels)");

                entity.Property(e => e.LabelWeight).HasColumnType("decimal(10, 3)");

                entity.Property(e => e.LabelWeightUom)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("LabelWeightUOM")
                    .IsFixedLength(true);

                entity.Property(e => e.NetWeight)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.OverrideItem)
                    .IsRequired()
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.PackSize)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.PackageCoderFmt1)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.PackageCoderFmt2)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.PackageCoderFmt3)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.PackagesPerSaleableUnit).HasColumnType("decimal(5, 0)");

                entity.Property(e => e.PalletCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PkgLabelDateFmtCode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PrintCaseLabel)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("'Y' - Print Case Label; 'N' - Do Not Print Case Label");

                entity.Property(e => e.PrintSolot)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PrintSOLot")
                    .IsFixedLength(true)
                    .HasComment("'Y' - Print Shop Order Lot; 'N' - Do Not Print");

                entity.Property(e => e.ProductionDateDesc)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.SaleableUnitPerCase).HasColumnType("decimal(5, 0)");

                entity.Property(e => e.Scccode)
                    .IsRequired()
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("SCCCode");

                entity.Property(e => e.StdCostPerLb)
                    .HasColumnType("decimal(15, 5)")
                    .HasColumnName("StdCostPerLB");

                entity.Property(e => e.Upccode)
                    .IsRequired()
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("UPCCode");
            });

            modelBuilder.Entity<WebMaterial>(entity =>
            {
                entity.HasKey(e => e.Rrn);

                entity.HasIndex(e => e.ItemNumber).IsUnique();

                entity.ToTable("tblWebMaterial");

                entity.Property(e => e.Rrn).HasColumnName("RRN");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Implength).HasColumnName("IMPLength");

                entity.Property(e => e.Imps).HasColumnName("IMPs");

                entity.Property(e => e.ItemNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            DefineVirtualRelationship(modelBuilder);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
