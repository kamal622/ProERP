using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class DailyPackingDetailMap : EntityTypeConfiguration<DailyPackingDetail>
    {
        public DailyPackingDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ProductionRemarks)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("DailyPackingDetails");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BatchId).HasColumnName("BatchId");
            this.Property(t => t.GradeId).HasColumnName("GradeId");
            this.Property(t => t.PackingDate).HasColumnName("PackingDate");
            this.Property(t => t.BagFrom).HasColumnName("BagFrom");
            this.Property(t => t.BagTo).HasColumnName("BagTo");
            this.Property(t => t.TotalBags).HasColumnName("TotalBags");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.ProductionRemarks).HasColumnName("ProductionRemarks");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateBy).HasColumnName("CreateBy");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdateBy).HasColumnName("UpdateBy");

            // Relationships
            this.HasRequired(t => t.FormulationRequest)
                .WithMany(t => t.DailyPackingDetails)
                .HasForeignKey(d => d.BatchId);
            this.HasRequired(t => t.ProductMaster)
                .WithMany(t => t.DailyPackingDetails)
                .HasForeignKey(d => d.GradeId);

        }
    }
}
