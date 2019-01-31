using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class BreakDownMenPowerMap : EntityTypeConfiguration<BreakDownMenPower>
    {
        public BreakDownMenPowerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("BreakDownMenPower");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BreakDownId).HasColumnName("BreakDownId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.EmployeeTypeId).HasColumnName("EmployeeTypeId");
            this.Property(t => t.HourlyRate).HasColumnName("HourlyRate");
            this.Property(t => t.IsOverTime).HasColumnName("IsOverTime");
            this.Property(t => t.Comments).HasColumnName("Comments");

            // Relationships
            this.HasRequired(t => t.BreakDown)
                .WithMany(t => t.BreakDownMenPowers)
                .HasForeignKey(d => d.BreakDownId);
            this.HasRequired(t => t.EmployeeType)
                .WithMany(t => t.BreakDownMenPowers)
                .HasForeignKey(d => d.EmployeeTypeId);

        }
    }
}
