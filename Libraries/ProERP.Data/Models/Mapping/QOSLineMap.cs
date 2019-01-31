using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class QOSLineMap : EntityTypeConfiguration<QOSLine>
    {
        public QOSLineMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("QOSLines");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LineId).HasColumnName("LineId");
        }
    }
}
