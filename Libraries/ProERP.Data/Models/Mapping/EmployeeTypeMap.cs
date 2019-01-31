using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class EmployeeTypeMap : EntityTypeConfiguration<EmployeeType>
    {
        public EmployeeTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Type)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("EmployeeType");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.NormalCharges).HasColumnName("NormalCharges");
            this.Property(t => t.OverTimeCharges).HasColumnName("OverTimeCharges");
            this.Property(t => t.PlantId).HasColumnName("PlantId");

            // Relationships
            this.HasRequired(t => t.Plant)
                .WithMany(t => t.EmployeeTypes)
                .HasForeignKey(d => d.PlantId);

        }
    }
}
