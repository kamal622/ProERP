using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class FormulationMasterMap : EntityTypeConfiguration<FormulationMaster>
    {
        public FormulationMasterMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("FormulationMaster");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.MachineId).HasColumnName("MachineId");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.BaseValue).HasColumnName("BaseValue");
            this.Property(t => t.PreviousBaseValue).HasColumnName("PreviousBaseValue");

            // Relationships
            this.HasRequired(t => t.RMItem)
                .WithMany(t => t.FormulationMasters)
                .HasForeignKey(d => d.ItemId);
            this.HasRequired(t => t.ProductMaster)
                .WithMany(t => t.FormulationMasters)
                .HasForeignKey(d => d.ProductId);

        }
    }
}
