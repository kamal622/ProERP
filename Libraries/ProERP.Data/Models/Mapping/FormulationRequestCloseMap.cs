using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class FormulationRequestCloseMap : EntityTypeConfiguration<FormulationRequestClose>
    {
        public FormulationRequestCloseMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.GradeName)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.LotNo)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("FormulationRequestClose");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.GradeName).HasColumnName("GradeName");
            this.Property(t => t.LotNo).HasColumnName("LotNo");
            this.Property(t => t.FGPackedQty).HasColumnName("FGPackedQty");
            this.Property(t => t.NSP).HasColumnName("NSP");
            this.Property(t => t.StartUpTrials).HasColumnName("StartUpTrials");
            this.Property(t => t.QCRejected).HasColumnName("QCRejected");
            this.Property(t => t.MixMaterial).HasColumnName("MixMaterial");
            this.Property(t => t.Lumps).HasColumnName("Lumps");
            this.Property(t => t.LongsandFines).HasColumnName("LongsandFines");
            this.Property(t => t.LabSample).HasColumnName("LabSample");
            this.Property(t => t.Sweepaged).HasColumnName("Sweepaged");
            this.Property(t => t.Additives).HasColumnName("Additives");
            this.Property(t => t.PackingBags).HasColumnName("PackingBags");
            this.Property(t => t.VerifiedDate).HasColumnName("VerifiedDate");
            this.Property(t => t.VerifiedBy).HasColumnName("VerifiedBy");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateBy).HasColumnName("CreateBy");
        }
    }
}
