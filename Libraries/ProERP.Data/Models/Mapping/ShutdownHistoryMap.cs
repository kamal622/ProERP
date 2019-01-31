using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class ShutdownHistoryMap : EntityTypeConfiguration<ShutdownHistory>
    {
        public ShutdownHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("ShutdownHistory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PlantId).HasColumnName("PlantId");
            this.Property(t => t.LineId).HasColumnName("LineId");
            this.Property(t => t.MachineId).HasColumnName("MachineId");
            this.Property(t => t.ShutdownDate).HasColumnName("ShutdownDate");
            this.Property(t => t.ShutdownBy).HasColumnName("ShutdownBy");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.StartBy).HasColumnName("StartBy");

            // Relationships
            this.HasRequired(t => t.Line)
                .WithMany(t => t.ShutdownHistories)
                .HasForeignKey(d => d.LineId);
            this.HasOptional(t => t.Machine)
                .WithMany(t => t.ShutdownHistories)
                .HasForeignKey(d => d.MachineId);
            this.HasRequired(t => t.Plant)
                .WithMany(t => t.ShutdownHistories)
                .HasForeignKey(d => d.PlantId);

        }
    }
}
