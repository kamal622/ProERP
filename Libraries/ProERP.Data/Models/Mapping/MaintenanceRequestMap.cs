using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class MaintenanceRequestMap : EntityTypeConfiguration<MaintenanceRequest>
    {
        public MaintenanceRequestMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.SerialNo)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Problem)
                .IsRequired()
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("MaintenanceRequest");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SerialNo).HasColumnName("SerialNo");
            this.Property(t => t.RequestBy).HasColumnName("RequestBy");
            this.Property(t => t.RequestDate).HasColumnName("RequestDate");
            this.Property(t => t.RequestTime).HasColumnName("RequestTime");
            this.Property(t => t.IsBreakdown).HasColumnName("IsBreakdown");
            this.Property(t => t.PlantId).HasColumnName("PlantId");
            this.Property(t => t.LineId).HasColumnName("LineId");
            this.Property(t => t.MachineId).HasColumnName("MachineId");
            this.Property(t => t.Problem).HasColumnName("Problem");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.PriorityId).HasColumnName("PriorityId");
            this.Property(t => t.StatusId).HasColumnName("StatusId");
            this.Property(t => t.IsCritical).HasColumnName("IsCritical");
            this.Property(t => t.AssignTo).HasColumnName("AssignTo");
            this.Property(t => t.AssignBy).HasColumnName("AssignBy");
            this.Property(t => t.AssignDate).HasColumnName("AssignDate");
            this.Property(t => t.ProgressBy).HasColumnName("ProgressBy");
            this.Property(t => t.ProgressDate).HasColumnName("ProgressDate");
            this.Property(t => t.CompleteBy).HasColumnName("CompleteBy");
            this.Property(t => t.CompleteDate).HasColumnName("CompleteDate");
            this.Property(t => t.CloseBy).HasColumnName("CloseBy");
            this.Property(t => t.CloseDate).HasColumnName("CloseDate");
            this.Property(t => t.HoldBy).HasColumnName("HoldBy");
            this.Property(t => t.HoldDate).HasColumnName("HoldDate");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.RemarksBy).HasColumnName("RemarksBy");
            this.Property(t => t.RemarksDate).HasColumnName("RemarksDate");
            this.Property(t => t.BreakdownType).HasColumnName("BreakdownType");
            this.Property(t => t.WorkStartDate).HasColumnName("WorkStartDate");
            this.Property(t => t.WorkStartTime).HasColumnName("WorkStartTime");
            this.Property(t => t.WorkEndDate).HasColumnName("WorkEndDate");
            this.Property(t => t.WorkEndTime).HasColumnName("WorkEndTime");
            this.Property(t => t.ProgressRemarks).HasColumnName("ProgressRemarks");
            this.Property(t => t.CompleteRemarks).HasColumnName("CompleteRemarks");
            this.Property(t => t.CloseRemarks).HasColumnName("CloseRemarks");
            this.Property(t => t.HoldRemarks).HasColumnName("HoldRemarks");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");

            // Relationships
            this.HasOptional(t => t.Line)
                .WithMany(t => t.MaintenanceRequests)
                .HasForeignKey(d => d.LineId);
            this.HasOptional(t => t.Machine)
                .WithMany(t => t.MaintenanceRequests)
                .HasForeignKey(d => d.MachineId);
            this.HasRequired(t => t.MaintanceRequestStatu)
                .WithMany(t => t.MaintenanceRequests)
                .HasForeignKey(d => d.StatusId);
            this.HasRequired(t => t.MaintenancePriorityType)
                .WithMany(t => t.MaintenanceRequests)
                .HasForeignKey(d => d.PriorityId);
            this.HasOptional(t => t.Plant)
                .WithMany(t => t.MaintenanceRequests)
                .HasForeignKey(d => d.PlantId);

        }
    }
}
