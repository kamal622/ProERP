using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class MachineReadingMap : EntityTypeConfiguration<MachineReading>
    {
        public MachineReadingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("MachineReading");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FormulationRequestId).HasColumnName("FormulationRequestId");
            this.Property(t => t.MachineId).HasColumnName("MachineId");
            this.Property(t => t.Reading).HasColumnName("Reading");

            // Relationships
            this.HasRequired(t => t.FormulationRequest)
                .WithMany(t => t.MachineReadings)
                .HasForeignKey(d => d.FormulationRequestId);
            this.HasRequired(t => t.Machine)
                .WithMany(t => t.MachineReadings)
                .HasForeignKey(d => d.MachineId);

        }
    }
}
