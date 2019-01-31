using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class MaintanceRequestStatuMap : EntityTypeConfiguration<MaintanceRequestStatu>
    {
        public MaintanceRequestStatuMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.StatusName)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("MaintanceRequestStatus");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.StatusName).HasColumnName("StatusName");
        }
    }
}
