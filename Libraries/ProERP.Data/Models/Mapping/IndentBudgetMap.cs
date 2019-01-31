using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class IndentBudgetMap : EntityTypeConfiguration<IndentBudget>
    {
        public IndentBudgetMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.BudgetType)
                .HasMaxLength(50);

            this.Property(t => t.BudgetCode)
                .HasMaxLength(50);

            this.Property(t => t.FinancialYear)
                .IsRequired()
                .HasMaxLength(16);

            // Table & Column Mappings
            this.ToTable("IndentBudget");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BudgetType).HasColumnName("BudgetType");
            this.Property(t => t.BudgetCode).HasColumnName("BudgetCode");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.PlantId).HasColumnName("PlantId");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.FinancialYear).HasColumnName("FinancialYear");

            // Relationships
            this.HasOptional(t => t.Item)
                .WithMany(t => t.IndentBudgets)
                .HasForeignKey(d => d.ItemId);
            this.HasOptional(t => t.Plant)
                .WithMany(t => t.IndentBudgets)
                .HasForeignKey(d => d.PlantId);

        }
    }
}
