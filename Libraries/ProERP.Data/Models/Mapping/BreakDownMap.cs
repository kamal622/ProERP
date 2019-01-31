using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class BreakDownMap : EntityTypeConfiguration<BreakDown>
    {
        public BreakDownMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DoneBy)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("BreakDown");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PlantId).HasColumnName("PlantId");
            this.Property(t => t.LineId).HasColumnName("LineId");
            this.Property(t => t.MachineId).HasColumnName("MachineId");
            this.Property(t => t.SubAssemblyId).HasColumnName("SubAssemblyId");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.TotalTime).HasColumnName("TotalTime");
            this.Property(t => t.FailureDescription).HasColumnName("FailureDescription");
            this.Property(t => t.ElectricalTime).HasColumnName("ElectricalTime");
            this.Property(t => t.MechTime).HasColumnName("MechTime");
            this.Property(t => t.InstrTime).HasColumnName("InstrTime");
            this.Property(t => t.UtilityTime).HasColumnName("UtilityTime");
            this.Property(t => t.PowerTime).HasColumnName("PowerTime");
            this.Property(t => t.ProcessTime).HasColumnName("ProcessTime");
            this.Property(t => t.PrvTime).HasColumnName("PrvTime");
            this.Property(t => t.IdleTime).HasColumnName("IdleTime");
            this.Property(t => t.ResolveTimeTaken).HasColumnName("ResolveTimeTaken");
            this.Property(t => t.SpareTypeId).HasColumnName("SpareTypeId");
            this.Property(t => t.SpareDescription).HasColumnName("SpareDescription");
            this.Property(t => t.DoneBy).HasColumnName("DoneBy");
            this.Property(t => t.RootCause).HasColumnName("RootCause");
            this.Property(t => t.Correction).HasColumnName("Correction");
            this.Property(t => t.CorrectiveAction).HasColumnName("CorrectiveAction");
            this.Property(t => t.PreventingAction).HasColumnName("PreventingAction");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t => t.IsHistory).HasColumnName("IsHistory");
            this.Property(t => t.DeletedDate).HasColumnName("DeletedDate");
            this.Property(t => t.DeletedBy).HasColumnName("DeletedBy");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.UploadId).HasColumnName("UploadId");
            this.Property(t => t.IsRepeated).HasColumnName("IsRepeated");
            this.Property(t => t.IsMajor).HasColumnName("IsMajor");

            // Relationships
            this.HasOptional(t => t.BreakDownUploadHistory)
                .WithMany(t => t.BreakDowns)
                .HasForeignKey(d => d.UploadId);
            this.HasRequired(t => t.Line)
                .WithMany(t => t.BreakDowns)
                .HasForeignKey(d => d.LineId);
            this.HasOptional(t => t.Machine)
                .WithMany(t => t.BreakDowns)
                .HasForeignKey(d => d.SubAssemblyId);
            this.HasRequired(t => t.User)
                .WithMany(t => t.BreakDowns)
                .HasForeignKey(d => d.CreatedBy);
            this.HasOptional(t => t.User1)
                .WithMany(t => t.BreakDowns1)
                .HasForeignKey(d => d.UpdatedBy);
            this.HasRequired(t => t.Machine1)
                .WithMany(t => t.BreakDowns1)
                .HasForeignKey(d => d.MachineId);
            this.HasRequired(t => t.Plant)
                .WithMany(t => t.BreakDowns)
                .HasForeignKey(d => d.PlantId);

        }
    }
}
