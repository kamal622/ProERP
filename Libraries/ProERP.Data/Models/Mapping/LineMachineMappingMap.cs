using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class LineMachineMappingMap : EntityTypeConfiguration<LineMachineMapping>
    {
        public LineMachineMappingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("LineMachineMapping");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LineId).HasColumnName("LineId");
            this.Property(t => t.MachineId).HasColumnName("MachineId");

            // Relationships
            this.HasOptional(t => t.Line)
                .WithMany(t => t.LineMachineMappings)
                .HasForeignKey(d => d.LineId);
            this.HasOptional(t => t.Machine)
                .WithMany(t => t.LineMachineMappings)
                .HasForeignKey(d => d.MachineId);

        }
    }
}
