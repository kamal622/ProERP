using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class PurchaseMap : EntityTypeConfiguration<Purchase>
    {
        public PurchaseMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.VendorId });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.VendorId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("Purchase");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.OrderQty).HasColumnName("OrderQty");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.TotalPrice).HasColumnName("TotalPrice");
            this.Property(t => t.OrderDate).HasColumnName("OrderDate");
            this.Property(t => t.OrderBy).HasColumnName("OrderBy");
            this.Property(t => t.VendorId).HasColumnName("VendorId");
        }
    }
}
