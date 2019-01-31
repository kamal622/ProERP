using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class ItemMap : EntityTypeConfiguration<Item>
    {
        public ItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ItemCode)
                .HasMaxLength(100);

            this.Property(t => t.Name)
                .IsRequired();

            this.Property(t => t.Make)
                .HasMaxLength(500);

            this.Property(t => t.Model)
                .HasMaxLength(500);

            this.Property(t => t.UnitOfMeasure)
                .HasMaxLength(100);

            this.Property(t => t.MOC)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("Items");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ItemCode).HasColumnName("ItemCode");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.SpecificationFile).HasColumnName("SpecificationFile");
            this.Property(t => t.IsImported).HasColumnName("IsImported");
            this.Property(t => t.Make).HasColumnName("Make");
            this.Property(t => t.Model).HasColumnName("Model");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.TotalQty).HasColumnName("TotalQty");
            this.Property(t => t.UnitOfMeasure).HasColumnName("UnitOfMeasure");
            this.Property(t => t.AvailableQty).HasColumnName("AvailableQty");
            this.Property(t => t.MOC).HasColumnName("MOC");
            this.Property(t => t.VendorId).HasColumnName("VendorId");

            // Relationships
            this.HasOptional(t => t.Vendor)
                .WithMany(t => t.Items)
                .HasForeignKey(d => d.VendorId);

        }
    }
}
