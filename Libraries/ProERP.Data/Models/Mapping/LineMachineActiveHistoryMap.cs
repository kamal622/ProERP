using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class LineMachineActiveHistoryMap : EntityTypeConfiguration<LineMachineActiveHistory>
    {
        public LineMachineActiveHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("LineMachineActiveHistory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LineId).HasColumnName("LineId");
            this.Property(t => t.MachineId).HasColumnName("MachineId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.UpdateBy).HasColumnName("UpdateBy");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            // Relationships
            this.HasRequired(t => t.Line)
                .WithMany(t => t.LineMachineActiveHistories)
                .HasForeignKey(d => d.LineId);
            this.HasOptional(t => t.Machine)
                .WithMany(t => t.LineMachineActiveHistories)
                .HasForeignKey(d => d.MachineId);
            this.HasRequired(t => t.User)
                .WithMany(t => t.LineMachineActiveHistories)
                .HasForeignKey(d => d.UpdateBy);

        }
    }
}
