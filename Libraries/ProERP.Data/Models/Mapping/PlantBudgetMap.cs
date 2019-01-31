using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class PlantBudgetMap : EntityTypeConfiguration<PlantBudget>
    {
        public PlantBudgetMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("PlantBudget");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PlantId).HasColumnName("PlantId");
            this.Property(t => t.MonthlyBudget).HasColumnName("MonthlyBudget");
            this.Property(t => t.EffectiveFrom).HasColumnName("EffectiveFrom");
            this.Property(t => t.EffectiveTo).HasColumnName("EffectiveTo");

            // Relationships
            this.HasOptional(t => t.Plant)
                .WithMany(t => t.PlantBudgets)
                .HasForeignKey(d => d.PlantId);

        }
    }
}
