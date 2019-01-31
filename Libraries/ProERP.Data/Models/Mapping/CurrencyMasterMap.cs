using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class CurrencyMasterMap : EntityTypeConfiguration<CurrencyMaster>
    {
        public CurrencyMasterMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Currency)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("CurrencyMaster");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Currency).HasColumnName("Currency");
        }
    }
}
