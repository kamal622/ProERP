using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class BreakDownAnalysyMap : EntityTypeConfiguration<BreakDownAnalysy>
    {
        public BreakDownAnalysyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.DocumentLocation)
                .HasMaxLength(500);

            this.Property(t => t.DocumentNo)
                .HasMaxLength(500);

            this.Property(t => t.DoneBy)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("BreakDownAnalysys");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PlantId).HasColumnName("PlantId");
            this.Property(t => t.LineId).HasColumnName("LineId");
            this.Property(t => t.MachineId).HasColumnName("MachineId");
            this.Property(t => t.SubAssemblyId).HasColumnName("SubAssemblyId");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.BreakDownTypeId).HasColumnName("BreakDownTypeId");
            this.Property(t => t.FailureDescription).HasColumnName("FailureDescription");
            this.Property(t => t.DocumentLocation).HasColumnName("DocumentLocation");
            this.Property(t => t.DocumentNo).HasColumnName("DocumentNo");
            this.Property(t => t.SpareTypeId).HasColumnName("SpareTypeId");
            this.Property(t => t.SpareDescription).HasColumnName("SpareDescription");
            this.Property(t => t.DoneBy).HasColumnName("DoneBy");
            this.Property(t => t.RootCause).HasColumnName("RootCause");
            this.Property(t => t.Correction).HasColumnName("Correction");
            this.Property(t => t.CorrectiveAction).HasColumnName("CorrectiveAction");
            this.Property(t => t.PreventingAction).HasColumnName("PreventingAction");

            // Relationships
            this.HasOptional(t => t.BreakDownType)
                .WithMany(t => t.BreakDownAnalysys)
                .HasForeignKey(d => d.BreakDownTypeId);
            this.HasOptional(t => t.Line)
                .WithMany(t => t.BreakDownAnalysys)
                .HasForeignKey(d => d.LineId);
            this.HasOptional(t => t.Machine)
                .WithMany(t => t.BreakDownAnalysys)
                .HasForeignKey(d => d.MachineId);
            this.HasOptional(t => t.Plant)
                .WithMany(t => t.BreakDownAnalysys)
                .HasForeignKey(d => d.PlantId);
            this.HasOptional(t => t.SubAssembly)
                .WithMany(t => t.BreakDownAnalysys)
                .HasForeignKey(d => d.SubAssemblyId);

        }
    }
}
