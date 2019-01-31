using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class PlantMap : EntityTypeConfiguration<Plant>
    {
        public PlantMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(500);

            this.Property(t => t.PlantInCharge)
                .HasMaxLength(500);

            this.Property(t => t.Location)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Plant");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.PlantInCharge).HasColumnName("PlantInCharge");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.SiteId).HasColumnName("SiteId");

            // Relationships
            this.HasOptional(t => t.Site)
                .WithMany(t => t.Plants)
                .HasForeignKey(d => d.SiteId);

        }
    }
}
