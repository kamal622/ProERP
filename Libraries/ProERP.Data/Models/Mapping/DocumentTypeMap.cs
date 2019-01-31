using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProERP.Data.Models.Mapping
{
    public class DocumentTypeMap : EntityTypeConfiguration<DocumentType>
    {
        public DocumentTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Desription)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("DocumentType");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ParentCategoryId).HasColumnName("ParentCategoryId");
            this.Property(t => t.Desription).HasColumnName("Desription");
        }
    }
}
