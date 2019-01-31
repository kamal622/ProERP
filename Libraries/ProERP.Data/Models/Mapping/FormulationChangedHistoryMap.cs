using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class FormulationChangedHistoryMap : EntityTypeConfiguration<FormulationChangedHistory>
    {
        public FormulationChangedHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("FormulationChangedHistory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FormulationRequestId).HasColumnName("FormulationRequestId");
            this.Property(t => t.VerNo).HasColumnName("VerNo");
            this.Property(t => t.CreateBy).HasColumnName("CreateBy");
            this.Property(t => t.CreateOn).HasColumnName("CreateOn");

            // Relationships
            this.HasRequired(t => t.FormulationRequest)
                .WithMany(t => t.FormulationChangedHistories)
                .HasForeignKey(d => d.FormulationRequestId);

        }
    }
}
