using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class BreakDownUploadHistoryMap : EntityTypeConfiguration<BreakDownUploadHistory>
    {
        public BreakDownUploadHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("BreakDownUploadHistory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UploadDate).HasColumnName("UploadDate");
            this.Property(t => t.UploadBy).HasColumnName("UploadBy");
            this.Property(t => t.OriginalFileName).HasColumnName("OriginalFileName");
            this.Property(t => t.SystemFileName).HasColumnName("SystemFileName");
        }
    }
}
