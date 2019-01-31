using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class RMRequestMasterMap : EntityTypeConfiguration<RMRequestMaster>
    {
        public RMRequestMasterMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.LotNo)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.RequestRemarks)
                .HasMaxLength(500);

            this.Property(t => t.DispatchRemarks)
                .HasMaxLength(500);

            this.Property(t => t.ReceviedRemarks)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("RMRequestMaster");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LotNo).HasColumnName("LotNo");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.FormulationRequestId).HasColumnName("FormulationRequestId");
            this.Property(t => t.RequestStatus).HasColumnName("RequestStatus");
            this.Property(t => t.RequestDate).HasColumnName("RequestDate");
            this.Property(t => t.RequestBy).HasColumnName("RequestBy");
            this.Property(t => t.RequestRemarks).HasColumnName("RequestRemarks");
            this.Property(t => t.DispatchDate).HasColumnName("DispatchDate");
            this.Property(t => t.DispatchBy).HasColumnName("DispatchBy");
            this.Property(t => t.DispatchRemarks).HasColumnName("DispatchRemarks");
            this.Property(t => t.ReceviedDate).HasColumnName("ReceviedDate");
            this.Property(t => t.ReceviedBy).HasColumnName("ReceviedBy");
            this.Property(t => t.ReceviedRemarks).HasColumnName("ReceviedRemarks");

            // Relationships
            this.HasRequired(t => t.FormulationRequest)
                .WithMany(t => t.RMRequestMasters)
                .HasForeignKey(d => d.FormulationRequestId);
            this.HasRequired(t => t.ProductMaster)
                .WithMany(t => t.RMRequestMasters)
                .HasForeignKey(d => d.ProductId);
            this.HasRequired(t => t.RMRequestStatu)
                .WithMany(t => t.RMRequestMasters)
                .HasForeignKey(d => d.RequestStatus);

        }
    }
}
