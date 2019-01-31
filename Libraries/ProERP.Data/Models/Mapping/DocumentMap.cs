using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class DocumentMap : EntityTypeConfiguration<Document>
    {
        public DocumentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.OriginalFileName)
                .HasMaxLength(200);

            this.Property(t => t.SysFileName)
                .HasMaxLength(200);

            this.Property(t => t.ZipFileName)
                .HasMaxLength(200);

            this.Property(t => t.RelativePath)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("Documents");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OriginalFileName).HasColumnName("OriginalFileName");
            this.Property(t => t.SysFileName).HasColumnName("SysFileName");
            this.Property(t => t.ZipFileName).HasColumnName("ZipFileName");
            this.Property(t => t.RelativePath).HasColumnName("RelativePath");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Tags).HasColumnName("Tags");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.PlantId).HasColumnName("PlantId");
            this.Property(t => t.LineId).HasColumnName("LineId");
            this.Property(t => t.MachineId).HasColumnName("MachineId");
            this.Property(t => t.DeletedDate).HasColumnName("DeletedDate");
            this.Property(t => t.DeletedBy).HasColumnName("DeletedBy");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");

            // Relationships
            this.HasOptional(t => t.DocumentType)
                .WithMany(t => t.Documents)
                .HasForeignKey(d => d.Type);
            this.HasOptional(t => t.Line)
                .WithMany(t => t.Documents)
                .HasForeignKey(d => d.LineId);
            this.HasOptional(t => t.Machine)
                .WithMany(t => t.Documents)
                .HasForeignKey(d => d.MachineId);
            this.HasRequired(t => t.Plant)
                .WithMany(t => t.Documents)
                .HasForeignKey(d => d.PlantId);
            this.HasOptional(t => t.User)
                .WithMany(t => t.Documents)
                .HasForeignKey(d => d.CreatedBy);

        }
    }
}
