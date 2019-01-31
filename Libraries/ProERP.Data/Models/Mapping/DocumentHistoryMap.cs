using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class DocumentHistoryMap : EntityTypeConfiguration<DocumentHistory>
    {
        public DocumentHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("DocumentHistory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.ActionId).HasColumnName("ActionId");
            this.Property(t => t.ActionDate).HasColumnName("ActionDate");
            this.Property(t => t.ActionBy).HasColumnName("ActionBy");

            // Relationships
            this.HasOptional(t => t.DocumentAction)
                .WithMany(t => t.DocumentHistories)
                .HasForeignKey(d => d.ActionId);
            this.HasOptional(t => t.Document)
                .WithMany(t => t.DocumentHistories)
                .HasForeignKey(d => d.DocumentId);
            this.HasOptional(t => t.User)
                .WithMany(t => t.DocumentHistories)
                .HasForeignKey(d => d.ActionBy);

        }
    }
}
