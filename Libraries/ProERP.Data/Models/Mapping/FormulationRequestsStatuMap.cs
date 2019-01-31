using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class FormulationRequestsStatuMap : EntityTypeConfiguration<FormulationRequestsStatu>
    {
        public FormulationRequestsStatuMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.StatusName)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("FormulationRequestsStatus");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.StatusName).HasColumnName("StatusName");
        }
    }
}
