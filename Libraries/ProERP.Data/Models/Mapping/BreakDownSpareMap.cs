using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class BreakDownSpareMap : EntityTypeConfiguration<BreakDownSpare>
    {
        public BreakDownSpareMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("BreakDownSpares");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BreakDownId).HasColumnName("BreakDownId");
            this.Property(t => t.PartId).HasColumnName("PartId");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.Quantity).HasColumnName("Quantity");

            // Relationships
            this.HasRequired(t => t.BreakDown)
                .WithMany(t => t.BreakDownSpares)
                .HasForeignKey(d => d.BreakDownId);
            this.HasRequired(t => t.Part)
                .WithMany(t => t.BreakDownSpares)
                .HasForeignKey(d => d.PartId);

        }
    }
}
