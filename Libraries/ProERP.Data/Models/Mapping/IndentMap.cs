using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class IndentMap : EntityTypeConfiguration<Indent>
    {
        public IndentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.IndentNo)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.RequisitionType)
                .HasMaxLength(50);

            this.Property(t => t.BudgetHead)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.PoNo)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Indents");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.Priority).HasColumnName("Priority");
            this.Property(t => t.IndentNo).HasColumnName("IndentNo");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.BudgetId).HasColumnName("BudgetId");
            this.Property(t => t.RequisitionType).HasColumnName("RequisitionType");
            this.Property(t => t.BudgetHead).HasColumnName("BudgetHead");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.StatusId).HasColumnName("StatusId");
            this.Property(t => t.ApprovedBy).HasColumnName("ApprovedBy");
            this.Property(t => t.ApprovedOn).HasColumnName("ApprovedOn");
            this.Property(t => t.RejectedBy).HasColumnName("RejectedBy");
            this.Property(t => t.RejectedOn).HasColumnName("RejectedOn");
            this.Property(t => t.PoDate).HasColumnName("PoDate");
            this.Property(t => t.DeliveryDate).HasColumnName("DeliveryDate");
            this.Property(t => t.PoNo).HasColumnName("PoNo");
            this.Property(t => t.PoAmount).HasColumnName("PoAmount");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedOn).HasColumnName("UpdatedOn");

            // Relationships
            this.HasRequired(t => t.IndentBudget)
                .WithMany(t => t.Indents)
                .HasForeignKey(d => d.BudgetId);
            this.HasRequired(t => t.IndentStatu)
                .WithMany(t => t.Indents)
                .HasForeignKey(d => d.StatusId);
            this.HasRequired(t => t.MaintenancePriorityType)
                .WithMany(t => t.Indents)
                .HasForeignKey(d => d.Priority);
            this.HasOptional(t => t.User)
                .WithMany(t => t.Indents)
                .HasForeignKey(d => d.CreatedBy);

        }
    }
}
