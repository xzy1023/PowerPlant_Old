using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlant.Models.Context
{
    public partial class PPDbContext : DbContext
    {
        // Define virtual relationships, this ForeignKey does not exist in the real database
        public void DefineVirtualRelationship(ModelBuilder modelBuilder)
        {
            // <ItemMaster> one to one <WebMaterial>
            modelBuilder.Entity<ItemMaster>()
                .HasOne(a => a.WebMaterial)
                .WithOne(b => b.ItemMaster)
                .HasForeignKey<WebMaterial>(b => b.ItemNumber)
                .HasPrincipalKey<ItemMaster>(e => e.ItemNumber);
        }
    }
}
