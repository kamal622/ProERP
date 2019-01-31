using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class FormulationRequestsChangeHistoryMap : EntityTypeConfiguration<FormulationRequestsChangeHistory>
    {
        public FormulationRequestsChangeHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Comment)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("FormulationRequestsChangeHistory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FormulationRequestId).HasColumnName("FormulationRequestId");
            this.Property(t => t.RequestStatus).HasColumnName("RequestStatus");
            this.Property(t => t.Comment).HasColumnName("Comment");
            this.Property(t => t.UpdateBy).HasColumnName("UpdateBy");
            this.Property(t => t.UpdateOn).HasColumnName("UpdateOn");

            // Relationships
            this.HasRequired(t => t.FormulationRequest)
                .WithMany(t => t.FormulationRequestsChangeHistories)
                .HasForeignKey(d => d.FormulationRequestId);
            this.HasRequired(t => t.FormulationRequestsStatu)
                .WithMany(t => t.FormulationRequestsChangeHistories)
                .HasForeignKey(d => d.RequestStatus);

        }
    }
}
