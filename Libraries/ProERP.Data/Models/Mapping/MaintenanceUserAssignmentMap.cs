using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class MaintenanceUserAssignmentMap : EntityTypeConfiguration<MaintenanceUserAssignment>
    {
        public MaintenanceUserAssignmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("MaintenanceUserAssignments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MaintenanceRequestId).HasColumnName("MaintenanceRequestId");
            this.Property(t => t.UserId).HasColumnName("UserId");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.MaintenanceUserAssignments)
                .HasForeignKey(d => d.UserId);

        }
    }
}
