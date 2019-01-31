using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class VendorMap : EntityTypeConfiguration<Vendor>
    {
        public VendorMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.PhoneNo)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("Vendors");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.PhoneNo).HasColumnName("PhoneNo");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.StarRating).HasColumnName("StarRating");
            this.Property(t => t.Email).HasColumnName("Email");

            // Relationships
            this.HasOptional(t => t.VendorCategory)
                .WithMany(t => t.Vendors)
                .HasForeignKey(d => d.CategoryId);

        }
    }
}
