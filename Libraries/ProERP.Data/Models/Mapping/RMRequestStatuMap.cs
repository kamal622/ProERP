using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class RMRequestStatuMap : EntityTypeConfiguration<RMRequestStatu>
    {
        public RMRequestStatuMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.StatusName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("RMRequestStatus");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.StatusName).HasColumnName("StatusName");
        }
    }
}
