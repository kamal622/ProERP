using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class IndentDetailAttachmentMap : EntityTypeConfiguration<IndentDetailAttachment>
    {
        public IndentDetailAttachmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.OriginalFileName)
                .IsRequired()
                .HasMaxLength(512);

            this.Property(t => t.SysFileName)
                .IsRequired()
                .HasMaxLength(512);

            // Table & Column Mappings
            this.ToTable("IndentDetailAttachments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IndentDetailId).HasColumnName("IndentDetailId");
            this.Property(t => t.OriginalFileName).HasColumnName("OriginalFileName");
            this.Property(t => t.SysFileName).HasColumnName("SysFileName");

            // Relationships
            this.HasRequired(t => t.IndentDetail)
                .WithMany(t => t.IndentDetailAttachments)
                .HasForeignKey(d => d.IndentDetailId);

        }
    }
}
