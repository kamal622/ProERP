using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class RMRequestDetailMap : EntityTypeConfiguration<RMRequestDetail>
    {
        public RMRequestDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("RMRequestDetail");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RMRequestId).HasColumnName("RMRequestId");
            this.Property(t => t.FormulationRequestId).HasColumnName("FormulationRequestId");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.RequestedQty).HasColumnName("RequestedQty");
            this.Property(t => t.IssuedQty).HasColumnName("IssuedQty");
            this.Property(t => t.ReturnQty).HasColumnName("ReturnQty");
            this.Property(t => t.WIPId).HasColumnName("WIPId");
            this.Property(t => t.WIPQty).HasColumnName("WIPQty");

            // Relationships
            this.HasRequired(t => t.FormulationRequest)
                .WithMany(t => t.RMRequestDetails)
                .HasForeignKey(d => d.FormulationRequestId);
            this.HasRequired(t => t.RMItem)
                .WithMany(t => t.RMRequestDetails)
                .HasForeignKey(d => d.ItemId);
            this.HasRequired(t => t.RMRequestMaster)
                .WithMany(t => t.RMRequestDetails)
                .HasForeignKey(d => d.RMRequestId);
            this.HasOptional(t => t.WIPStore)
                .WithMany(t => t.RMRequestDetails)
                .HasForeignKey(d => d.WIPId);

        }
    }
}
