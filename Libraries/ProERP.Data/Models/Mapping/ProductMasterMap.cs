using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class ProductMasterMap : EntityTypeConfiguration<ProductMaster>
    {
        public ProductMasterMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Code)
                .HasMaxLength(100);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.ShortName)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("ProductMaster");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ShortName).HasColumnName("ShortName");
            this.Property(t => t.Description).HasColumnName("Description");

            // Relationships
            this.HasRequired(t => t.ProductCategory)
                .WithMany(t => t.ProductMasters)
                .HasForeignKey(d => d.CategoryId);

        }
    }
}
