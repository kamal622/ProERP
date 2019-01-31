using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class ColourSpecificationMap : EntityTypeConfiguration<ColourSpecification>
    {
        public ColourSpecificationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DeltaE)
                .HasMaxLength(100);

            this.Property(t => t.DeltaL)
                .HasMaxLength(100);

            this.Property(t => t.Deltaa)
                .HasMaxLength(100);

            this.Property(t => t.Deltab)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ColourSpecification");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FormulationRequestId).HasColumnName("FormulationRequestId");
            this.Property(t => t.DeltaE).HasColumnName("DeltaE");
            this.Property(t => t.DeltaL).HasColumnName("DeltaL");
            this.Property(t => t.Deltaa).HasColumnName("Deltaa");
            this.Property(t => t.Deltab).HasColumnName("Deltab");
            this.Property(t => t.VerNo).HasColumnName("VerNo");

            // Relationships
            this.HasRequired(t => t.FormulationRequest)
                .WithMany(t => t.ColourSpecifications)
                .HasForeignKey(d => d.FormulationRequestId);

        }
    }
}
