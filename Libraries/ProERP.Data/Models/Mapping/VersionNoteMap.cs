using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class VersionNoteMap : EntityTypeConfiguration<VersionNote>
    {
        public VersionNoteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Notes)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("VersionNotes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.VersionId).HasColumnName("VersionId");
            this.Property(t => t.Notes).HasColumnName("Notes");

            // Relationships
            this.HasRequired(t => t.PLMMVersion)
                .WithMany(t => t.VersionNotes)
                .HasForeignKey(d => d.VersionId);

        }
    }
}
