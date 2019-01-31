using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class FormulationRequestsDetailMap : EntityTypeConfiguration<FormulationRequestsDetail>
    {
        public FormulationRequestsDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("FormulationRequestsDetails");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FormulationRequestId).HasColumnName("FormulationRequestId");
            this.Property(t => t.MachineId).HasColumnName("MachineId");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.ItemQtyPercentage).HasColumnName("ItemQtyPercentage");
            this.Property(t => t.ItemQtyGram).HasColumnName("ItemQtyGram");
            this.Property(t => t.UOM).HasColumnName("UOM");
            this.Property(t => t.VerNo).HasColumnName("VerNo");

            // Relationships
            this.HasRequired(t => t.FormulationRequest)
                .WithMany(t => t.FormulationRequestsDetails)
                .HasForeignKey(d => d.FormulationRequestId);
            this.HasRequired(t => t.RMItem)
                .WithMany(t => t.FormulationRequestsDetails)
                .HasForeignKey(d => d.ItemId);
            this.HasOptional(t => t.Machine)
                .WithMany(t => t.FormulationRequestsDetails)
                .HasForeignKey(d => d.MachineId);

        }
    }
}
