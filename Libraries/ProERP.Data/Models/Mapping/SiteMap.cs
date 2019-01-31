using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class SiteMap : EntityTypeConfiguration<Site>
    {
        public SiteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(1000);

            this.Property(t => t.Address)
                .HasMaxLength(1000);

            this.Property(t => t.InCharge)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Site");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.InCharge).HasColumnName("InCharge");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
