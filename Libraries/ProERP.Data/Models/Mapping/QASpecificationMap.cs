using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class QASpecificationMap : EntityTypeConfiguration<QASpecification>
    {
        public QASpecificationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.MFI220c10kg)
                .HasMaxLength(100);

            this.Property(t => t.SPGravity)
                .HasMaxLength(100);

            this.Property(t => t.AshContent)
                .HasMaxLength(100);

            this.Property(t => t.NotchImpact)
                .HasMaxLength(100);

            this.Property(t => t.Tensile)
                .HasMaxLength(100);

            this.Property(t => t.FlexuralModule)
                .HasMaxLength(100);

            this.Property(t => t.FlexuralStrength)
                .HasMaxLength(100);

            this.Property(t => t.Elongation)
                .HasMaxLength(100);

            this.Property(t => t.Flammability)
                .HasMaxLength(100);

            this.Property(t => t.GWTAt)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("QASpecification");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FormulationRequestId).HasColumnName("FormulationRequestId");
            this.Property(t => t.MFI220c10kg).HasColumnName("MFI220c10kg");
            this.Property(t => t.SPGravity).HasColumnName("SPGravity");
            this.Property(t => t.AshContent).HasColumnName("AshContent");
            this.Property(t => t.NotchImpact).HasColumnName("NotchImpact");
            this.Property(t => t.Tensile).HasColumnName("Tensile");
            this.Property(t => t.FlexuralModule).HasColumnName("FlexuralModule");
            this.Property(t => t.FlexuralStrength).HasColumnName("FlexuralStrength");
            this.Property(t => t.Elongation).HasColumnName("Elongation");
            this.Property(t => t.Flammability).HasColumnName("Flammability");
            this.Property(t => t.GWTAt).HasColumnName("GWTAt");
            this.Property(t => t.VerNo).HasColumnName("VerNo");

            // Relationships
            this.HasRequired(t => t.FormulationRequest)
                .WithMany(t => t.QASpecifications)
                .HasForeignKey(d => d.FormulationRequestId);

        }
    }
}
