using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class PreventiveMaintenanceMap : EntityTypeConfiguration<PreventiveMaintenance>
    {
        public PreventiveMaintenanceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Checkpoints)
                .HasMaxLength(500);

            this.Property(t => t.WorkDescription)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("PreventiveMaintenance");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PlantId).HasColumnName("PlantId");
            this.Property(t => t.LineId).HasColumnName("LineId");
            this.Property(t => t.MachineId).HasColumnName("MachineId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Checkpoints).HasColumnName("Checkpoints");
            this.Property(t => t.ScheduleType).HasColumnName("ScheduleType");
            this.Property(t => t.Interval).HasColumnName("Interval");
            this.Property(t => t.ShutdownRequired).HasColumnName("ShutdownRequired");
            this.Property(t => t.ScheduleStartDate).HasColumnName("ScheduleStartDate");
            this.Property(t => t.ScheduleEndDate).HasColumnName("ScheduleEndDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedOn).HasColumnName("UpdatedOn");
            this.Property(t => t.LastReviewDate).HasColumnName("LastReviewDate");
            this.Property(t => t.NextReviewDate).HasColumnName("NextReviewDate");
            this.Property(t => t.PreferredVendorId).HasColumnName("PreferredVendorId");
            this.Property(t => t.WorkDescription).HasColumnName("WorkDescription");
            this.Property(t => t.IsDeletedOn).HasColumnName("IsDeletedOn");
            this.Property(t => t.IsDeletedBy).HasColumnName("IsDeletedBy");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.Severity).HasColumnName("Severity");
            this.Property(t => t.IsObservation).HasColumnName("IsObservation");

            // Relationships
            this.HasRequired(t => t.Line)
                .WithMany(t => t.PreventiveMaintenances)
                .HasForeignKey(d => d.LineId);
            this.HasRequired(t => t.Machine)
                .WithMany(t => t.PreventiveMaintenances)
                .HasForeignKey(d => d.MachineId);
            this.HasRequired(t => t.Plant)
                .WithMany(t => t.PreventiveMaintenances)
                .HasForeignKey(d => d.PlantId);
            this.HasRequired(t => t.PreventiveScheduleType)
                .WithMany(t => t.PreventiveMaintenances)
                .HasForeignKey(d => d.ScheduleType);

        }
    }
}
