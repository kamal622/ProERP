using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class UserAssignmentMap : EntityTypeConfiguration<UserAssignment>
    {
        public UserAssignmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("UserAssignments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PreventiveMaintenanceId).HasColumnName("PreventiveMaintenanceId");
            this.Property(t => t.UserId).HasColumnName("UserId");

            // Relationships
            this.HasRequired(t => t.PreventiveMaintenance)
                .WithMany(t => t.UserAssignments)
                .HasForeignKey(d => d.PreventiveMaintenanceId);
            this.HasRequired(t => t.User)
                .WithMany(t => t.UserAssignments)
                .HasForeignKey(d => d.UserId);

        }
    }
}
