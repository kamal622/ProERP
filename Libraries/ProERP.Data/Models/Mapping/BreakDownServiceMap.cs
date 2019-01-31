using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class BreakDownServiceMap : EntityTypeConfiguration<BreakDownService>
    {
        public BreakDownServiceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.VendorName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BreakDownServices");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BreakDownId).HasColumnName("BreakDownId");
            this.Property(t => t.VendorName).HasColumnName("VendorName");
            this.Property(t => t.Cost).HasColumnName("Cost");
            this.Property(t => t.Comments).HasColumnName("Comments");

            // Relationships
            this.HasRequired(t => t.BreakDown)
                .WithMany(t => t.BreakDownServices)
                .HasForeignKey(d => d.BreakDownId);

        }
    }
}
