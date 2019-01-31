using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class MachineMap : EntityTypeConfiguration<Machine>
    {
        public MachineMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.Make)
                .HasMaxLength(500);

            this.Property(t => t.Model)
                .HasMaxLength(500);

            this.Property(t => t.MachineInCharge)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Machine");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Make).HasColumnName("Make");
            this.Property(t => t.Model).HasColumnName("Model");
            this.Property(t => t.InstallationDate).HasColumnName("InstallationDate");
            this.Property(t => t.MachineInCharge).HasColumnName("MachineInCharge");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.MachineTypeId).HasColumnName("MachineTypeId");
            this.Property(t => t.PlantId).HasColumnName("PlantId");
            this.Property(t => t.LineId).HasColumnName("LineId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.IsShutdown).HasColumnName("IsShutdown");

            // Relationships
            this.HasRequired(t => t.Line)
                .WithMany(t => t.Machines)
                .HasForeignKey(d => d.LineId);
            this.HasOptional(t => t.Machine2)
                .WithMany(t => t.Machine1)
                .HasForeignKey(d => d.ParentId);
            this.HasRequired(t => t.MachineType)
                .WithMany(t => t.Machines)
                .HasForeignKey(d => d.MachineTypeId);
            this.HasRequired(t => t.Plant)
                .WithMany(t => t.Machines)
                .HasForeignKey(d => d.PlantId);

        }
    }
}
