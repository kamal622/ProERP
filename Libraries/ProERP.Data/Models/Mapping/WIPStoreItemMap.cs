using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class WIPStoreItemMap : EntityTypeConfiguration<WIPStoreItem>
    {
        public WIPStoreItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("WIPStoreItems");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.StoreId).HasColumnName("StoreId");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.ItemQty).HasColumnName("ItemQty");

            // Relationships
            this.HasRequired(t => t.RMItem)
                .WithMany(t => t.WIPStoreItems)
                .HasForeignKey(d => d.ItemId);
            this.HasRequired(t => t.WIPStore)
                .WithMany(t => t.WIPStoreItems)
                .HasForeignKey(d => d.StoreId);

        }
    }
}
