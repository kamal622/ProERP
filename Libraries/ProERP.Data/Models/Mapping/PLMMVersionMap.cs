using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class PLMMVersionMap : EntityTypeConfiguration<PLMMVersion>
    {
        public PLMMVersionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ReleaseVersion)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PLMMVersion");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ReleaseDate).HasColumnName("ReleaseDate");
            this.Property(t => t.ReleaseVersion).HasColumnName("ReleaseVersion");
        }
    }
}
