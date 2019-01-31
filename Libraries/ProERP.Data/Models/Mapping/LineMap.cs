using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class LineMap : EntityTypeConfiguration<Line>
    {
        public LineMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(500);

            this.Property(t => t.Location)
                .HasMaxLength(500);

            this.Property(t => t.InCharge)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Line");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PlantId).HasColumnName("PlantId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.InCharge).HasColumnName("InCharge");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.IsShutdown).HasColumnName("IsShutdown");

            // Relationships
            this.HasRequired(t => t.Plant)
                .WithMany(t => t.Lines)
                .HasForeignKey(d => d.PlantId);

        }
    }
}
