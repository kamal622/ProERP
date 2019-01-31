using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class BreakDownAttachmentMap : EntityTypeConfiguration<BreakDownAttachment>
    {
        public BreakDownAttachmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.OriginalFileName)
                .IsRequired();

            this.Property(t => t.SysFileName)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("BreakDownAttachments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BreakDownId).HasColumnName("BreakDownId");
            this.Property(t => t.OriginalFileName).HasColumnName("OriginalFileName");
            this.Property(t => t.SysFileName).HasColumnName("SysFileName");

            // Relationships
            this.HasRequired(t => t.BreakDown)
                .WithMany(t => t.BreakDownAttachments)
                .HasForeignKey(d => d.BreakDownId);

        }
    }
}
